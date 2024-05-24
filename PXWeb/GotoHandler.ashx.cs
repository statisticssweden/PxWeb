using PCAxis.Search;
using PCAxis.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

namespace PXWeb
{
    /// <summary>
    /// Summary description for Goto
    /// </summary>
    public class GotoHandler : IHttpHandler
    {

        public GotoHandler()
        {

        }

        protected void AddToQueries(Dictionary<string, string> queries, string key)
        {

            if (HttpContext.Current.Items["RouteData"] != null)
            {
                var routeData = HttpContext.Current.Items["RouteData"] as RouteData;

                if (routeData.Values[key] != null)
                {
                    queries[key] = ValidationManager.GetValue(routeData.Values[key].ToString());
                }
                else
                {
                    queries.Add(key, ValidationManager.GetValue(routeData.Values[key].ToString()));
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string language = null;
            string database = null;
            string table = null;
            List<LinkManager.LinkItem> queryParams = new List<LinkManager.LinkItem>();

            // Fetch the route data
            var routeData = context.Items["RouteData"] as RouteData;
            if (routeData.Values[PxUrl.LANGUAGE_KEY] != null)
                language = ValidationManager.GetValue(routeData.Values[PxUrl.LANGUAGE_KEY].ToString());
            if (routeData.Values[PxUrl.DB_KEY] != null)
                database = ValidationManager.GetValue(routeData.Values[PxUrl.DB_KEY].ToString());
            if (routeData.Values[PxUrl.TABLE_KEY] != null)
                table = ValidationManager.GetValue(routeData.Values[PxUrl.TABLE_KEY].ToString());

            if ((!string.IsNullOrEmpty(language)) && (!string.IsNullOrEmpty(database)) && (!string.IsNullOrEmpty(table)))
            {
                SearchStatusType status;
                List<SearchResultItem> result = SearchManager.Current.Search(database, language, table, out status, SearchConstants.SEARCH_FIELD_SEARCHID);

                if (result.Count == 1)
                {
                    DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(database);

                    foreach (String key in HttpContext.Current.Request.QueryString.AllKeys)
                    {
                        // 
                        queryParams.Add(new LinkManager.LinkItem(key, QuerystringManager.GetQuerystringParameter(key)));
                    }

                    string url = SearchHelper.GetSearchResultUrl(result[0], dbi, language, queryParams);
                    context.Response.Redirect(url);
                }
                else
                {
                    // If table is not found redirect to menu page
                    context.Response.Redirect(LinkManager.CreateLink("Menu.aspx"));
                }
                context.Response.Redirect(LinkManager.CreateLink("Default.aspx"));
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}