using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;
using PCAxis.Query;
using PCAxis.Web.Controls;
using PCAxis.Web.Core.Management;
using PXWeb.Code.Management;
using PXWeb.Management;
using PXWeb.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;

namespace PXWeb
{
    /// <summary>
    /// Summary description for SavedQueryHandler
    /// </summary>
    public class SavedQueryHandler : IHttpHandler, IRequiresSessionState
    {

        private static List<string> _blackList;
        private static PCAxis.Api.RequestLimiter _requestLimiter;

        static SavedQueryHandler()
        {
            _blackList = new List<string>();
            _blackList.Add(OperationConstants.SUM);
            _blackList.Add(OperationConstants.PER_PART);
            _blackList.Add(OperationConstants.CHANGE_TEXT);
            if (Settings.Current.Features.SavedQuery.EnableLimitRequest)
                _requestLimiter = new PCAxis.Api.RequestLimiter("RQLIMIT_SQ:", Settings.Current.Features.SavedQuery.LimiterTimespan, Settings.Current.Features.SavedQuery.LimiterRequests, PCAxis.Api.Settings.Current.LimiterHttpHeaderName);
        }

        private string _format;
        private string _language;
        private string _originaleSavedQuerylanguage;

        public SavedQueryHandler() : this(null)
        {

        }

        public SavedQueryHandler(string language)
        {
            this._language = language;
        }

        public void ProcessRequest(HttpContext context)
        {

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

            // Enable CORS and preflight requests for saved queries
            // Preflight requests should also be allowed in the API (SSDHandler). 

            if (Settings.Current.Features.SavedQuery.EnableCORS)
            {
                // Enable CORS (Cross-Origin-Resource-Sharing)
                context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

                // Handle Preflight requests
                if (context.Request.HttpMethod == "OPTIONS")
                {
                    context.Response.AppendHeader("Access-Control-Allow-Methods", "GET");
                    return;
                }
            }

            string queryName;
            var routeData = context.Items["RouteData"] as RouteData;
            if (routeData.Values["QueryName"] != null)
            {
                queryName = ValidationManager.GetValue(routeData.Values["QueryName"].ToString());
            }
            else
            { 
                //No query supplied goto error page.
                //TODO just to shut the compiler up
                queryName = "";
                //TODO redirect
                throw new Exception("No query supplied");
            }

            // ----- Handle changed output format -----
            _format = GetChangedOutputFormat(routeData);

            // ----- Handle changed language -----
            HandleChangedLanguage();

            //Load saved query
            PCAxis.Query.SavedQuery sq = null;
            PXModel model = null;
            bool safe = true;

            try
            {
                if (PCAxis.Query.SavedQueryManager.StorageType == PCAxis.Query.SavedQueryStorageType.File)
                {
                    string path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/queries/");

                    if (!queryName.ToLower().EndsWith(".pxsq"))
                    {
                        queryName = queryName + ".pxsq";
                    }

                    string[] allfiles = Directory.GetFiles(path, queryName, SearchOption.AllDirectories);

                    if (allfiles.Length == 0)
                    {
                        throw new HttpException(404, "HTTP/1.1 404 Not Found ");
                    }

                    queryName = allfiles[0];
                }

                //Check if the database is active. 
                //It should not be possible to run a saved query if the database is not active
                sq = PCAxis.Query.SavedQueryManager.Current.Load(queryName);
				IEnumerable<string> db;
				TableSource src = sq.Sources[0];

				if (src.Type.ToLower() == "cnmm")
				{
					if (!CnmmDatabaseRootHelper.Check(src.Source))
                    {
                        throw new SystemException("Saved query: not authorized to run rooted saved query");
                    }

                    db = PXWeb.Settings.Current.General.Databases.CnmmDatabases;
				}
				else
				{
					db = PXWeb.Settings.Current.General.Databases.PxDatabases;
				}
				bool activeDatabase = false;
				foreach (var item in db)
				{
					if (item.ToLower() == src.DatabaseId.ToLower())
					{
						activeDatabase = true;
						break;
					}					
				}
				if (!activeDatabase)
				{
					throw new SystemException();
				}


				//Validate that the user has the rights to access the table
				string tableName = QueryHelper.GetTableName(src);
                //if (!AuthorizationUtil.IsAuthorized(src.DatabaseId, null, src.Source))
                if (!AuthorizationUtil.IsAuthorized(src.DatabaseId, null, tableName)) //TODO: Should be dbid, menu and selection. Only works for SCB right now... (2018-11-14)
                {
                    List<LinkManager.LinkItem> linkItems = new List<LinkManager.LinkItem>();
                    linkItems.Add(new LinkManager.LinkItem() { Key = PxUrl.LANGUAGE_KEY, Value = src.Language });
                    linkItems.Add(new LinkManager.LinkItem() { Key = PxUrl.DB_KEY, Value = src.DatabaseId });
                    linkItems.Add(new LinkManager.LinkItem() { Key = "msg", Value = "UnauthorizedTable" });

                    string url = LinkManager.CreateLink("~/Menu.aspx", linkItems.ToArray());
                    HttpContext.Current.Response.Redirect(url, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }

                if (string.IsNullOrWhiteSpace(_format))
                {
                    //Output format is not changed - use output format in the saved query
                    _format = sq.Output.Type;
                }

                // "Pre-flight" request from MS Office application
                var userAgent = context.Request.Headers["User-Agent"];
                //if (userAgent.ToLower().Contains("ms-office") && sq.Output.Type == PxUrl.VIEW_TABLE_IDENTIFIER)
                if (userAgent != null && userAgent.ToLower().Contains("ms-office"))
                {
                    context.Response.Write("<html><body>ms office return</body></html>");
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //context.Response.End();
                    return;
                }

                //We need to store to be able to run workflow due to variables are referenced with name and not ids
                _originaleSavedQuerylanguage = sq.Sources[0].Language;

                // Check from saved query output type is on screen. If so createCopy shall be true, else false
                bool createCopy = CreateCopyOfCachedPaxiom(_format);

                // Create cache key
                string cacheKey = "";
                if (_language != null)
                {
                    cacheKey = string.Format("{0}_{1}", queryName, _language);
                }
                else
                {
                    cacheKey = string.Format("{0}_{1}", queryName, _originaleSavedQuerylanguage);
                }

                // Handle redirects to the selection page in a special way. The model object will only contain metadata and no data
                if (_format.Equals(PxUrl.PAGE_SELECT))
                {
                    cacheKey = string.Format("{0}_{1}", cacheKey, PxUrl.PAGE_SELECT);
                }

                // Try to get model from cache
                model = PXWeb.Management.SavedQueryPaxiomCache.Current.Fetch(cacheKey, createCopy);
                PaxiomManager.QueryModel = PXWeb.Management.SavedQueryPaxiomCache.Current.FetchQueryModel(cacheKey, createCopy);

                if (model == null || PaxiomManager.QueryModel == null)
                {
                    DateTime timeStamp = DateTime.Now;
                    // Model not found in cache - load it manually

                    model = LoadData(sq);

                    //Check if we need to change langauge to be able to run workflow due to variables are referenced with name and not ids
                    if (!string.IsNullOrEmpty(_language) && _language != _originaleSavedQuerylanguage)
                    {
                        model.Meta.SetLanguage(_originaleSavedQuerylanguage);
                    }

                    // No need to run workflow if we are redirecting to the selection page
                    if (!_format.Equals(PxUrl.PAGE_SELECT))
                    {
                        model = QueryHelper.RunWorkflow(sq, model);
                    }

                    //Set back to requested langauge after workflow operations
                    if (!string.IsNullOrEmpty(_language) && _language != _originaleSavedQuerylanguage)
                    {
                        if (model.Meta.HasLanguage(_language))
                        {
                            model.Meta.SetLanguage(_language);
                        }
                    }

                    // Store model in cache
                    PXWeb.Management.SavedQueryPaxiomCache.Current.Store(cacheKey, model, timeStamp);
                    PXWeb.Management.SavedQueryPaxiomCache.Current.StoreQueryModel(cacheKey, PaxiomManager.QueryModel, timeStamp);
                }

                if (!sq.Safe)
                {
                    safe = !CheckForUnsafeOperations(sq.Workflow);
                }
            }
            catch (Exception ex)
                {

                if ((PCAxis.Query.SavedQueryManager.StorageType == PCAxis.Query.SavedQueryStorageType.File && System.IO.File.Exists(queryName)) || 
                    (PCAxis.Query.SavedQueryManager.StorageType == PCAxis.Query.SavedQueryStorageType.Database))
                {
                    PCAxis.Query.SavedQueryManager.Current.MarkAsFailed(queryName);
                }

                throw new HttpException(404, "HTTP/1.1 404 Not Found");
                //throw ex;
            }

            sq.LoadedQueryName = queryName;
            PCAxis.Query.SavedQueryManager.Current.MarkAsRunned(queryName);

            // Tell the selection page that it sholud clear the PxModel
            if (_format.Equals(PxUrl.PAGE_SELECT))
            {
                HttpContext.Current.Session.Add("SelectionClearPxModel", true);
            }

            ViewSerializerCreator.GetSerializer(_format).Render(_format, sq, model, safe);
        }

        /// <summary>
        /// This method checks if the output format in the saved query is overridden in the URL.
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns>
        /// The new format if the default format is overridden.
        /// "" is returned if the default output format shall be used
        /// </returns>
        private string GetChangedOutputFormat(RouteData routeData)
        {
            string format = "";

            //Check if the output format is changed in the URL (the saved query URL ends with . followed by the file format, for example .px)
            if (routeData.Values["Format"] != null)
            {
                string desiredformat = ValidationManager.GetValue(routeData.Values["Format"].ToString());
                string tryformat;

                if (QueryHelper.TryParseSavedQueryOutputFormat(desiredformat, out tryformat))
                {
                    format = tryformat;
                }
            }

            //Check if output format should be the selection page in PX-Web = Querystring contains the 'select' value
            //'select' is a keyless value in the querystring
            if (HttpContext.Current.Request.QueryString.GetValues(null) != null) //Keyless values exists in the querystring
            {
                if (HttpContext.Current.Request.QueryString.GetValues(null).Contains(PxUrl.PAGE_SELECT)) //The keyless value 'select' exists in the querystring
                {
                    format = PxUrl.PAGE_SELECT;
                }

            }

            return format;
        }

        /// <summary>
        /// This method handles if the language for the saved query is overridden in the URL
        /// </summary>
        private void HandleChangedLanguage()
        {
            string lang = QuerystringManager.GetQuerystringParameter("lang");

            if (lang != null)
            {
                _language = lang;
            }
        }

        /// <summary>
        /// If the saved query shall result in display on screen, then a copy of the paxiom object shall be returned 
        /// from the Paxiom cache. This method determines if the requested output format is display on screen or not.
        /// </summary>
        /// <param name="output">The requested output format</param>
        /// <returns>True if a copy of the Paxiom object shall be returned from cache (that is display on screen), else false</returns>
        private bool CreateCopyOfCachedPaxiom(string output)
        {
            switch (output)
            {
                case PxUrl.PAGE_SELECT:
                    return true;
                case PxUrl.VIEW_TABLE_IDENTIFIER:
                    return true;
                case PxUrl.VIEW_CHART_IDENTIFIER:
                    return true;
                case PxUrl.VIEW_FOOTNOTES_IDENTIFIER:
                    return true;
                case PxUrl.VIEW_INFORMATION_IDENTIFIER:
                    return true;
                case PxUrl.VIEW_SORTEDTABLE_IDENTIFIER:
                    return true;
                default:
                    return false;
            }

        }

        /// <summary>
        /// Checks if the workflow contains any black listed operations
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool CheckForUnsafeOperations(List<WorkStep> list)
        {
            foreach (var step in list)
            {
                if (_blackList.Contains(step.Type)) return true;
            }
            return false;
        }

        private string CreateUrlForScreenRendering(PCAxis.Query.SavedQuery sq, PXModel model)
        {
            if (sq.Sources.Count < 1) throw new Exception("No source specified"); //TODO fix message
            
            var src = sq.Sources[0];

            if (string.Compare(src.SourceIdType, "path") != 0) throw new Exception("Incompatible source type"); //TODO fix

            string path = src.Source;
            var tableName = QueryHelper.GetTableName(src);

            path = path.Substring(0, path.Length - (tableName.Length + 1));

            if (string.Compare(sq.Sources[0].Type, "CNMM") == 0 )
            {
                if (!path.StartsWith("START"))
                {
                    path = "START__" + path;
                }
            }

            path = path.Replace(@"/", PxPathHandler.NODE_DIVIDER);

            List<LinkManager.LinkItem> linkItems = new List<LinkManager.LinkItem>();

            linkItems.Add(new LinkManager.LinkItem(PxUrl.TABLE_KEY, tableName));
            linkItems.Add(new LinkManager.LinkItem(PxUrl.PATH_KEY, path.ToString()));
            linkItems.Add(new LinkManager.LinkItem(PxUrl.DB_KEY, src.DatabaseId));
            linkItems.Add(new LinkManager.LinkItem(PxUrl.LANGUAGE_KEY, src.Language));
            linkItems.Add(new LinkManager.LinkItem(PxUrl.LAYOUT_KEY, "tableViewLayout1"));
            
            var url = LinkManager.CreateLink("Table.aspx", false, linkItems.ToArray());
            PCAxis.Web.Core.Management.LocalizationManager.ChangeLanguage(src.Language);
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel = model;

            return url;
        }

    

        private void SerializeResult(PCAxis.Query.SavedQuery sq, PXModel model, HttpContext context)
        {
            var info = PCAxis.Web.Controls.CommandBar.Plugin.CommandBarPluginManager.GetFileType(sq.Output.Type);

            PCAxis.Web.Core.ISerializerCreator creator = Activator.CreateInstance(Type.GetType(info.Creator)) as PCAxis.Web.Core.ISerializerCreator;

            context.Response.ContentType = info.MimeType;
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + model.Meta.Matrix + "." + info.FileExtension);

            var serializer = creator.Create(sq.Output.Type);

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(model, ms);
                ms.Position = 0;
                ms.WriteTo(context.Response.OutputStream);
            }
        }

        private string GetContentType(string fileFormat)
        {
            var info = PCAxis.Web.Controls.CommandBar.Plugin.CommandBarPluginManager.GetFileType(fileFormat);

            if (info != null)
            {
                return info.MimeType;
            } 

            return "text/plain";
        }
  

        private PXModel LoadData(PCAxis.Query.SavedQuery sq)
        {
            //Only loads the first table source otherwise redirects to a page
            if (sq.Sources.Count != 1)
            { 
                //TODO redirect to error page incopatable query for PX-Web
            }

            TableSource src = sq.Sources[0];

            IPXModelBuilder builder = null;

            if (src.Type == "CNMM")
            {
                if (RouteInstance.RouteExtender == null)
                {
                    DatabaseInfo db = PXWeb.Settings.Current.General.Databases.GetCnmmDatabase(src.DatabaseId);

                    if (db == null)
                    {
                        //TODO Redirect database does not exist
                        return null;
                    }

                    var tableName = QueryHelper.GetTableName(src);
                    builder = PxContext.CreatePaxiomBuilder(src.DatabaseId, tableName);
                }
                else
                {
                    DatabaseInfo db = PXWeb.Settings.Current.General.Databases.GetCnmmDatabase(RouteInstance.RouteExtender.GetDatabase());

                    var tableName = QueryHelper.GetTableName(src);
                    builder = PxContext.CreatePaxiomBuilder(RouteInstance.RouteExtender.Db.Database.id, tableName);
                }    
            }
            else if (src.Type == "PX")
            {
                DatabaseInfo db = PXWeb.Settings.Current.General.Databases.GetPxDatabase(src.DatabaseId);
                if (db == null)
                {

                }
                else
                {
                    if (!db.HasLanguage(src.Language))
                    {
                        //TODO Redirect that the database is missing
                        return null;
                    }

                    if (src.Source.StartsWith("-/"))
                    {
                        src.Source = src.DatabaseId + src.Source.Substring(1);
                    }

                    builder = PxContext.CreatePaxiomBuilder(src.DatabaseId, src.Source);
                }
            }
            else
            {
                //TODO redirect to error page incompatible datasource type
                return null;
            }

            builder.SetPreferredLanguage(src.Language);

            //If languefe set in reques we must read all langauges to be able to run workflow operations
            if (_language != null)
            {
                builder.ReadAllLanguages = true;
            }

            builder.BuildForSelection();
            var model = builder.Model;

            List<PCAxis.Paxiom.Selection> sel = new List<PCAxis.Paxiom.Selection>();

            foreach (var variable in model.Meta.Variables)
            {
                var query = src.Quieries.FirstOrDefault(q => q.Code == variable.Code);
                PCAxis.Paxiom.Selection s = null;

                if (query == null)
                {
                    //Selects all values for the variable if it can't be eliminated
                    s = new PCAxis.Paxiom.Selection(variable.Code);
                    if (variable.IsContentVariable || !variable.Elimination)
                    {
                        s.ValueCodes.AddRange(variable.Values.Select(v => v.Code).ToArray());
                    }
                }
                else
                {
                    if (PCAxis.Query.QueryHelper.IsAggregation(query.Selection.Filter))
                    {
                        s = QueryHelper.SelectAggregation(variable, query, builder);
                    }
                    else if (query.Selection.Filter.StartsWith("vs:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        s = QueryHelper.SelectValueSet(variable, query, builder);
                    }
                    else
                    {
                        switch (query.Selection.Filter)
                        {

                            case "item":
                                s = QueryHelper.SelectItem(variable, query);
                                break;
                            case "top":
                                s = QueryHelper.SelectTop(variable, query);
                                break;
                            case "from":
                                s = QueryHelper.SelectFrom(variable, query);
                                break;
                            case "all":
                                s = QueryHelper.SelectAll(variable, query);
                                break;
                            default:
                                //TODO unsupported filter 
                                break;
                        }
                    }
                }

                if (s != null)
                {
                    sel.Add(s);
                }
            }

            var selection = sel.ToArray();

            //TODO fixa till
            //if (sq.Output.Type == "SCREEN")
            PCAxis.Query.TableQuery tbl = new PCAxis.Query.TableQuery(builder.Model, selection);
            PaxiomManager.QueryModel = tbl;

            BuildModelForPresentation(builder, selection);

            return builder.Model;
        }

        /// <summary>
        /// Calls BuildForPresentation and handles special case when we have the following condition:
        /// 1. Redirect to the selection page is demanded for the saved query using the ?select switch
        /// 2. The setting "Remove single content" is set to True
        /// 3. There is only one value selected for the content variable
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="selection"></param>
        private void BuildModelForPresentation(IPXModelBuilder builder, PCAxis.Paxiom.Selection[] selection)
        {
            if (_format.Equals(PxUrl.PAGE_SELECT) && PCAxis.Paxiom.Settings.Metadata.RemoveSingleContent && (builder.Model.Meta.ContentVariable != null))
            {
                // Handle the special case...
                Variable contentVar = builder.Model.Meta.ContentVariable.CreateCopyWithValues();
                bool inHeading = (builder.Model.Meta.Heading.GetByCode(contentVar.Code) != null);
                bool addContent = false;

                PCAxis.Paxiom.Selection selCont = selection.First(s => s.VariableCode.Equals(contentVar.Code));
                if (selCont.ValueCodes.Count == 1)
                {
                    // Remove all values except the one selected
                    contentVar.Values.RemoveAll(x => x.Code != selCont.ValueCodes[0]);
                    addContent = true;
                }

                // The content variable will be eliminated in BuildForPresentation ...
                builder.BuildForPresentation(selection);

                // ... but we need it on the selection page so we add it manually to the model after the call to BuildForPresentation
                if (addContent)
                {
                    builder.Model.Meta.Variables.Add(contentVar);
                    if (inHeading)
                    {
                        builder.Model.Meta.Heading.Add(contentVar);
                    }
                    else
                    {
                        builder.Model.Meta.Stub.Add(contentVar);
                    }
                }
            }
            else
            {
                builder.BuildForPresentation(selection);
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static string OperationsConstants { get; set; }
    }
}
