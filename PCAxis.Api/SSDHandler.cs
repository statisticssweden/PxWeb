using System;
using System.Web;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;
using PCAxis.Query;
using PCAxis.Menu;
using System.IO;
using PCAxis.Api.Serializers;
using System.Configuration;
using System.Net;


namespace PCAxis.Api
{
    public class SSDHandler : IHttpHandler
    {
        /// <summary>
        /// Maximum number of variable values returned in a meta query
        /// </summary>
        //private readonly int MAX_VALUES;
        //private readonly bool CORS_ENABLED = false;
        private RequestLimiter _requestLimiter;
        private string _defaultResponseFormat = "px";

        private static log4net.ILog _usageLogger = log4net.LogManager.GetLogger("api-usage");
        private static log4net.ILog _apiCacheLogger = log4net.LogManager.GetLogger("api-cache-logger");
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(SSDHandler));
#if DEBUG
        private static readonly log4net.ILog logTime = log4net.LogManager.GetLogger("LogTime");
#endif
        /// <summary>
        /// Constructor which sets ut the 
        /// </summary>
        public SSDHandler() : this(Settings.Current.DefaultResponseFormat)
        {
        }

        /// <summary>
        /// Constructor which sets ut the 
        /// </summary>
        public SSDHandler(string defaultResponseFormat)
        {
            if (!string.IsNullOrEmpty(defaultResponseFormat))
            {
                _defaultResponseFormat = defaultResponseFormat;
            }

            if (Settings.Current.EnableLimiter)
                _requestLimiter = new RequestLimiter("RQLIMIT_API:", Settings.Current.LimiterTimeSpan, Settings.Current.LimiterRequests, Settings.Current.LimiterHttpHeaderName);
        }

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        /// <summary>
        /// Returns a list of metadata for the specified item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private IEnumerable<MetaList> GetMetaList(HttpContext context, PxMenuItem item)
        {
            // Logs usage
            _usageLogger.Info(String.Format("url={0}, type=metadata, caller={1}, cached=false", context.Request.RawUrl, context.Request.UserHostAddress));

            return item.SubItems.Select(i => new MetaList
            {
                Id = i.ID.Selection.Replace('\\', '/'),
                Text = i.Text,
                Type = GetMetaListType(i),
                Updated =  i is TableLink ? (((TableLink)i).Published) : null
            });
        }



        /// <summary>
        /// Returns the coded type indicating that the item  t(able), h(eadline) or l(ink)/folder 
        /// </summary>
        /// <param name="menuItem">the item </param>
        /// <returns>the coded type</returns>
        private static string GetMetaListType(Item menuItem)
        {
            if (menuItem is TableLink)
            {
                return "t";
            }
            else if (menuItem is Headline)
            {
                return "h";
            }
            else
            {
                return "l";
            }
        }




        /// <summary>
        /// Find tables using the search function
        /// </summary>
        /// <param name="database"></param>
        /// <param name="language"></param>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        private IEnumerable<PCAxis.Search.SearchResultItem> GetSearchResult(string database, string language, string query, string filter, List<string> routeParts, out PCAxis.Search.SearchStatusType status)
        {
            System.Text.StringBuilder nodeFilter = new System.Text.StringBuilder(); // Only include tables under this node
            var db = ExposedDatabases.DatabaseConfigurations[language][database];

            if (db.Type == "PX")
            {
                nodeFilter.Append(database);
            }

            foreach (string node in routeParts)
            {
                if (node != "START")
                {
                    nodeFilter.Append("/");
                    nodeFilter.Append(node);
                }
            }

            // jfi: 1000000  means "all" ;-) Could not find a magic number ( looked at Lucene 3.0.3 ) 
            // There is an MatchAllDocsQuery class, but this means a bigger change
            // edit: never mind MatchAllDocsQuery, is the java versjon. Make sure to add .net when you google. Still 
            //could not find a magic int ...
            
            // Get all tables that match the query under the search node
            var result = PCAxis.Search.SearchManager.Current.Search(database, language, query, out status, filter,1000000).Where(r => r.Path.StartsWith(nodeFilter.ToString())).ToList();

            // Make the path in the result items relative the search node
            int length = nodeFilter.ToString().Length;
            foreach (PCAxis.Search.SearchResultItem item in result)
            {
                item.Path = item.Path.Substring(length);
            }

            return result;
        }

        /// <summary>
        /// Creates and returns an IPXModelBuilder depending on the type of database provided
        /// </summary>
        /// <param name="language">database language</param>
        /// <param name="db">id for the database</param>
        /// <param name="tablePath">path to the table</param>
        /// <returns>The builder for the table</returns>
        private static PCAxis.Paxiom.IPXModelBuilder GetPXBuilder(string language, string db, string[] tablePath)
        {
            PCAxis.Paxiom.IPXModelBuilder builder;
            switch (ExposedDatabases.DatabaseConfigurations[language][db].Type)
            {
                case "PX":
                    builder = new PCAxis.Paxiom.PXFileBuilder();
                    builder.SetPath(ExposedDatabases.DatabaseConfigurations[language][db].RootPath + "\\" + string.Join("\\", tablePath.ToArray())); 
                    break;
                case "CNMM":
                    builder = new PCAxis.PlugIn.Sql.PXSQLBuilder();
                    builder.SetPath( string.Format("{0}:{1}",db,tablePath.Last()));
                    break;
                default:
                    throw new NotImplementedException("Database type not implemented");
            }
            return builder;
        }

        /// <summary>
        /// Returns metadata for the specified table
        /// </summary>
        /// <param name="tableName">The table id</param>
        /// <returns>Metadata for the table</returns>
        private MetaTable GetTableMeta(HttpContext context, string language, string db, string[] tablePath)
        {
            PCAxis.Paxiom.IPXModelBuilder builder = GetPXBuilder(language, db, tablePath);
            builder.DoNotApplyCurrentValueSet = true;  // DoNotApplyCurrentValueSet means the "client that made the request" is an api(, not a GUI) so that
                                                       // CNMM2.4 property DefaultInGUI (for Valueset/grouping) should not be used  
            builder.SetPreferredLanguage(language);
            builder.BuildForSelection();


            return new MetaTable
            {
                Title = builder.Model.Meta.Title,
                Variables = builder.Model.Meta.Variables.Select(variable => new MetaVariable
                {
                    Code = variable.Code,
                    Text = variable.Name,
                    Elimination = variable.Elimination,
                    Time = variable.IsTime,
                    Values = variable.Values.Count > Settings.Current.MaxValues ? null : variable.Values.Select(value => value.Code).ToArray(),
                    ValueTexts = variable.Values.Count > Settings.Current.MaxValues ? null : variable.Values.Select(value => value.Value).ToArray()
                }).ToArray()
            };
        }

        /// <summary>
        /// Returns a JSON-formatted error message
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        private string Error(string message, bool prettyPrint)
        {
            return new
            {
                error = message
            }.ToJSON(prettyPrint);
        }

        /// <summary>
        /// Serializes and sends table data back to the client
        /// </summary>
        /// <param name="context"></param>
        private void SendTableData(HttpContext context, string language, string db, string[] tablePath, ResponseBucket cacheResponse)
        {
            //// TODO: Limit data size?
            //string data = "";
            //using (var stream = new StreamReader(context.Request.InputStream))
            //{
            //    data = stream.ReadToEnd();
            //}

            var tableQuery = JsonHelper.Deserialize<TableQuery>(cacheResponse.PostData) as TableQuery;
            if (tableQuery.Response == null || tableQuery.Response.Format == null)
            {
                tableQuery.Response = new QueryResponse() { Format = _defaultResponseFormat };
            }

            // Initialize the builder
            PCAxis.Paxiom.IPXModelBuilder builder = GetPXBuilder(language, db, tablePath);
            builder.DoNotApplyCurrentValueSet = true;  // DoNotApplyCurrentValueSet means the "client that made the request" is an api(, not a GUI) so that
                                                       // CNMM2.4 property DefaultInGUI (for Valueset/grouping) should not be used  
            builder.SetPreferredLanguage(language);
            builder.BuildForSelection();

            // Process selections
            var selections = PCAxisRepository.BuildSelections(builder, tableQuery);

            // Check that the number of selected cells do not exceed the limit
            long cellCount = 1;
            foreach (var sel in selections)
            {
                if (sel.ValueCodes.Count > 0)
                {
                    cellCount *= sel.ValueCodes.Count;
                }
            }
            if (cellCount > Settings.Current.FetchCellLimit)
            {
                Write403Response(context);
                return;
            }

            builder.BuildForPresentation(selections.ToArray());

            // Output handling starts here
            context.Response.Clear();

            IWebSerializer serializer;
            switch (tableQuery.Response.Format != null ? tableQuery.Response.Format.ToLower() : null)
            {
                case null:
                case "px":
                    serializer = new PxSerializer();
                    break;
                case "csv":
                    serializer = new CsvSerializer();
                    break;
                case "json":
                    serializer = new JsonSerializer();
                    break;
                case "json-stat":
                    serializer = new JsonStatSeriaizer();
                    break;
                case "json-stat2":
                    serializer = new JsonStat2Seriaizer();
                    break;
                case "xlsx":
                    serializer = new XlsxSerializer();
                    break;
                //case "png":
                //    int? width = tableQuery.Response.GetParamInt("width");
                //    int? height = tableQuery.Response.GetParamInt("height");
                //    string encoding = tableQuery.Response.GetParamString("encoding");
                //    serializer = new ChartSerializer(width, height, encoding);
                //    break;
                case "sdmx":
                    serializer = new SdmxDataSerializer();
                    break;
                default:
                    throw new NotImplementedException("Serialization for " + tableQuery.Response.Format + " is not implemented");
            }
            //serializer.Serialize(builder.Model, context.Response);
            serializer.Serialize(builder.Model, cacheResponse);
            //context.Response.AddHeader("Content-Type", cacheResponse.ContentType);
            //context.Response.OutputStream.Write(cacheResponse.ResponseData, 0, cacheResponse.ResponseData.Length);
            context.Send(cacheResponse, true);
            //Logs usage
            _usageLogger.Info(String.Format("url={0}, type=data, caller={3}, cached=false, format={1}, matrix-size={2}", context.Request.RawUrl, tableQuery.Response.Format, builder.Model.Data.MatrixSize, context.Request.UserHostAddress));
        }

        private void Write403Response(HttpContext context)
        {
            context.SendJSONError(Error("Too many values selected", false), 403);
            _usageLogger.Info(String.Format("url={0}, type=error, caller={1}, cached=false, message=Too many values selected", context.Request.RawUrl, context.Request.UserHostAddress));
        }


        /// <summary>
        /// Handles client requests and routes to GET or POST methods accordingly
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            logTime.Info("Start " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif
            try
            {
                string cacheKey = ""; // Key to request stored in cache
                ResponseBucket cacheResponse; // Request stored in cache
                string postData = ""; // Data part of POST request (contains JSON query)
                
                // Negotiate with the request limiter (if enabled)
                if (_requestLimiter != null) 
                {
                   
                    if (!_requestLimiter.ClientLimitOK(context.Request))
                    {

                        // Deny request
                        // see 409 - http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
                        context.Response.AppendHeader("Retry-After", _requestLimiter.LimiterTimeSpan.ToString());
                        throw new HttpException(429, "429 - Too many requests in too short timeframe. Please try again later.");
                    }
                }
              

                if (Settings.Current.EnableCORS)
                {
                    // Enable CORS (Cross-Origin-Resource-Sharing)
                    context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

                    // Enable Preflight requests
                    if (context.Request.HttpMethod == "OPTIONS")
                    {
                        context.Response.AppendHeader("Access-Control-Allow-Methods", "GET, POST");
                        context.Response.AppendHeader("Access-Control-Allow-Headers", "Content-Type");
                        //context.Response.End();
                        return;
                    }
                }


                // Create cache key by combining URL and query strings
                if (context.Request.HttpMethod == "POST")
                {
                    using (var stream = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        postData = stream.ReadToEnd();
                    }
                }


                cacheKey = ApiCache.CreateKey(context.Request.Url.AbsoluteUri + postData);
                // Try to get request from cache
                cacheResponse = ApiCache.Current.Fetch(cacheKey);
                if (cacheResponse != null)
                {
                    if (cacheResponse.Url.Equals(context.Request.Url.AbsoluteUri) && cacheResponse.PostData.Equals(postData))
                    {
                        // Use cached object
                        context.Send(cacheResponse, false);
                        if (context.Request.HttpMethod == "POST")
                        {
                            _usageLogger.Info(String.Format("url={0}, type=data, caller={3}, cached=true, format={1}, matrix-size={2}", context.Request.RawUrl, cacheResponse.ContentType, "?", context.Request.UserHostAddress));
                        }
                        else
                        {
                            _usageLogger.Info(String.Format("url={0}, type=metadata, caller={1}, cached=true, format=?, matrix-size=?", context.Request.RawUrl, context.Request.UserHostAddress));
                        }
#if DEBUG
                        stopWatch.Stop();
                        logTime.InfoFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " Done from cahce in ms = {0}", stopWatch.ElapsedMilliseconds);
#endif
                        return;
                    }
                }
                if (_apiCacheLogger.IsDebugEnabled)
                {
                    _apiCacheLogger.DebugFormat("Key {0} not found in cache, creating response from backend.", cacheKey);
                }
                // Request object to be stored in cache
                cacheResponse = new ResponseBucket();
                cacheResponse.Key = cacheKey;
                cacheResponse.Url = context.Request.Url.AbsoluteUri;
                cacheResponse.PostData = postData;
                
                List<string> routeParts = null;
                string language = null;
                string db = null;

                // Fetch the route data
                var routeData = context.Items["RouteData"] as RouteData;
                if (routeData.Values["language"] != null)
                    language = routeData.Values["language"].ToString();

                if (routeData.Values["path"] != null)
                {
                    routeParts = routeData.Values["path"].ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    db = routeParts.First();
                    routeParts.RemoveAt(0);
                }
                // Parse query string
                var options = new Options(context.Request.QueryString);

                var queryKeys = context.Request.QueryString.ToString().Split('&');

                if (queryKeys.Contains("config"))
                {
                    var obj = new {
                        maxValues = Settings.Current.MaxValues,
                        maxCells = Settings.Current.FetchCellLimit,
                        maxCalls = Settings.Current.LimiterRequests,
                        timeWindow = Settings.Current.LimiterTimeSpan,
                        CORS = Settings.Current.EnableCORS};
                    
                    cacheResponse.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
                    cacheResponse.ResponseData = context.Response.ContentEncoding.GetBytes(obj.ToJSON(options.PrettyPrint));
                    context.Send(cacheResponse, true);
                    _usageLogger.Info(String.Format("url={0}, type=config, caller={1}, cached=false", context.Request.RawUrl, context.Request.UserHostAddress));

#if DEBUG
                    stopWatch.Stop();
                    logTime.InfoFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " Done in ms = {0}", stopWatch.ElapsedMilliseconds);
#endif
                    return;
                }

                if (ExposedDatabases.DatabaseConfigurations.ContainsKey(language) == false)
                {
                    throw new ArgumentException("The specified language '" + language + "' does not exist");
                }
                else if (db == null)
                {
                    var databases = new List<MetaDb>();
                    foreach (var database in ExposedDatabases.DatabaseConfigurations[language].Keys)
                    {
                        databases.Add(new MetaDb
                        {
                            Id = database,
                            Text = ExposedDatabases.DatabaseConfigurations[language][database].Name
                        });
                    }
                    if (databases.Count > 0)
                    {
                        cacheResponse.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
                        cacheResponse.ResponseData = context.Response.ContentEncoding.GetBytes(databases.ToJSON(options.PrettyPrint));
                        context.Send(cacheResponse, true);
                        //context.SendJSON(databases.ToJSON(options.PrettyPrint));
                    }
                    else
                        throw new ArgumentException("No databases found in the specified language");
                }
                else // We have a database specified
                {
                    if (ExposedDatabases.DatabaseConfigurations[language].ContainsKey(db) == false)
                    {
                        // Database does not exist
                        throw new ArgumentException("The specified database '" + db + "' does not exist");
                    }
                    else
                    {
                        // Try to get a menu item first
                        string[] strNodePath = (routeParts == null || routeParts.Count == 0) ? new string[] { "" } : routeParts.ToArray();


                        var menu = PCAxisRepository.GetMenu(db, language, strNodePath);
                        if (menu == null)
                        {
                            //TODO Check exception type
                            throw new Exception("Error while connecting to data source");
                        }
                        else if ((menu.ID.Menu == "") && (menu.ID.Selection == ""))
                        {
                            // Could not find level in tree - Check if it is a short URL with table
                            if (strNodePath.Length == 1 && strNodePath[0] != "")
                            {
                                var possibleNameOrId = strNodePath[0];
                                PCAxis.Search.SearchStatusType status;
                                var text = string.Format("{0}:{2} OR {1}:{2}", PCAxis.Search.SearchConstants.SEARCH_FIELD_SEARCHID, PCAxis.Search.SearchConstants.SEARCH_FIELD_TABLEID, possibleNameOrId);
                                var results = PCAxis.Search.SearchManager.Current.Search(db, language, text, out status);

                                if (status == Search.SearchStatusType.Successful && results.Count > 0)
                                {
                                    if (_logger.IsDebugEnabled)
                                    {
                                        if (results.Count > 1)
                                        {
                                            for (int cnter = 1; cnter < results.Count; cnter++)
                                            {
                                                if (!String.Equals(results[cnter - 1].Table, results[cnter].Table))
                                                {
                                                    _logger.DebugFormat("Hmmm, not some result have different table names: {0}  differs from  {1}", results[cnter - 1].Table, results[cnter].Table);
                                                }
                                            }
                                        }
                                    }

                                    strNodePath = (results[0].Path + "/" + results[0].Table).Split('/');
                                    //Remove first element in array
                                    strNodePath = strNodePath.Where((source, index) => index != 0).ToArray();
                                    routeParts = strNodePath.ToList();

                                    // Get the menu again...
                                    menu = PCAxisRepository.GetMenu(db, language, strNodePath);
                                }
                                
                            }
                        }
                        //else
                        //{
                            var currentItem = menu;//.CurrentItem as Item;

                            if (currentItem is PxMenuItem && ((PxMenuItem)currentItem).HasSubItems)
                            {
                                if (!string.IsNullOrEmpty(options.SearchQuery))
                                {
                                    // Find tables using the search function
                                    PCAxis.Search.SearchStatusType status;

                                    cacheResponse.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
                                    cacheResponse.ResponseData = context.Response.ContentEncoding.GetBytes(GetSearchResult(db, language, options.SearchQuery, options.SearchFilter, routeParts, out status).ToJSON(options.PrettyPrint));
                                    
                                    if (status == Search.SearchStatusType.Successful)
                                    {
                                        context.Send(cacheResponse, false); // Send without caching
                                    }
                                    else
                                    {
                                        // Trying to search a non-indexed database...
                                        context.SendJSONError(Error("Search not activated", false), 400);
                                        _usageLogger.Info(String.Format("url={0}, type=error, caller={1}, cached=false, message=Search not activated", context.Request.RawUrl, context.Request.UserHostAddress));
                                        _logger.Warn(String.Format("Search not activated for {0} - {1}", db, language));
#if DEBUG
                                        stopWatch.Stop();
                                        logTime.InfoFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " Done in ms = {0}", stopWatch.ElapsedMilliseconds);
#endif
                                        return;
                                    }
                                }
                                else
                                {
                                    // Current item is a list item - return meta data for this list
                                    cacheResponse.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
                                    cacheResponse.ResponseData = context.Response.ContentEncoding.GetBytes(GetMetaList(context, (PxMenuItem)currentItem).ToJSON(options.PrettyPrint));
                                    context.Send(cacheResponse, true);
                                }

                                //context.SendJSON(GetMetaList(context, (PxMenuItem)currentItem).ToJSON(options.PrettyPrint));
                            }
                            else if (context.Request.HttpMethod == "GET" && currentItem is TableLink)
                            {
                                // Current item is not a list. Try to return table meta data
                                cacheResponse.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
                                cacheResponse.ResponseData = context.Response.ContentEncoding.GetBytes(GetTableMeta(context, language, db, strNodePath).ToJSON(options.PrettyPrint));

                                context.Send(cacheResponse, true);

                                // Logs usage
                                _usageLogger.Info(String.Format("url={0}, type=metadata, caller={1}, cached=false, format=json", context.Request.RawUrl, context.Request.UserHostAddress));
                            }
                            else if (context.Request.HttpMethod == "POST" && currentItem is TableLink)
                            {
                                // Current item is not a list. Try to return table data.
                                SendTableData(context, language, db, routeParts.ToArray(), cacheResponse);
                            }
                            else
                            {
                                context.SendJSONError(Error("Parameter error", false), 404);
                                // Logs usage
                                _usageLogger.Info(String.Format("url={0}, type=error, caller={1}, cached=false, message=Parameter error", context.Request.RawUrl, context.Request.UserHostAddress));
                            }
                        //}
                    }

                }
#if DEBUG
                stopWatch.Stop();
                logTime.InfoFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " Done in ms = {0}", stopWatch.ElapsedMilliseconds);
#endif
            }
            catch (HttpException ex)
            {
#if DEBUG
                stopWatch.Stop();
                logTime.InfoFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " Error Done in ms = {0}", stopWatch.ElapsedMilliseconds);
#endif
                if (ex.GetHttpCode() == 429)
                {
                    context.Response.TrySkipIisCustomErrors = true;
                    context.SendJSONError(Error(ex.Message, false), 429);
                    
                }
                else
                {
                    context.SendJSONError(Error("Parameter error", false), ex.GetHttpCode());
                }
                // Logs usage
                _usageLogger.Info(String.Format("url={0}, type=error, caller={1}, cached=false", context.Request.RawUrl, context.Request.UserHostAddress), ex);
                _logger.Warn(ex);
            }
            catch (Exception ex)
            {
                //if (context.Request.HttpMethod != "OPTIONS")
                //{

#if DEBUG
                    stopWatch.Stop();
                    logTime.InfoFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " Error Done in ms = {0}", stopWatch.ElapsedMilliseconds);
#endif
                    context.SendJSONError(Error("Parameter error", false), 404);
                    _usageLogger.Info(String.Format("url={0}, type=error, caller={1}, cached=false ", context.Request.RawUrl, context.Request.UserHostAddress), ex);
                    _logger.Warn(ex);
                //}
            }
        }
    }
}
