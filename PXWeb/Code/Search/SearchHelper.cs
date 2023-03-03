using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCAxis.Search;
using System.Text;
using PCAxis.Web.Core.Management;
using PCAxis.Web.Controls;
using PCAxis.Paxiom.Extensions;
using PXWeb.Code.Management;

namespace PXWeb
{
    public class SearchHelper
    {
        /// <summary>
        /// Get URL for a search result item
        /// </summary>
        /// <param name="itm">SearchResultItem object</param>
        /// <param name="dbi">DatabaseInfo object</param>
        /// <param name="queryParams">Optional list with querystring parameters that shall be included in the url</param>
        /// <returns>URL to the selection page for the found table</returns>
        public static string GetSearchResultUrl(SearchResultItem itm, DatabaseInfo dbi, string language, List<LinkManager.LinkItem> queryParams = null)
        {
            StringBuilder path = new StringBuilder();
            char[] sep = { '/' };
            string url = "";
            List<LinkManager.LinkItem> linkItems = new List<LinkManager.LinkItem>();

            if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
            {
                if (CnmmDatabaseRootHelper.IsRooted)
                {
                    itm.Path = CnmmDatabaseRootHelper.DatabaseRoot + itm.Path;
                }

                if (!itm.Path.StartsWith("START"))
                {
                    path.Append("START__");
                }
            }

            path.Append(itm.Path.TrimStart(sep).Replace(@"/", PxPathHandler.NODE_DIVIDER));
                       
            linkItems.Add(new LinkManager.LinkItem(PxUrl.TABLE_KEY, itm.Table));
            linkItems.Add(new LinkManager.LinkItem(PxUrl.PATH_KEY, path.ToString()));
            linkItems.Add(new LinkManager.LinkItem(PxUrl.DB_KEY, dbi.Id));
            linkItems.Add(new LinkManager.LinkItem(PxUrl.LANGUAGE_KEY, language));

            // Add extra querystring parameters
            if (queryParams != null)
            {
                foreach (LinkManager.LinkItem item in queryParams)
                {
                    linkItems.Add(item);
                }
            }

            url = LinkManager.CreateLink("Selection.aspx", false, linkItems.ToArray());

            return url;
        }

        /// <summary>
        /// Get the published date string for the found table
        /// </summary>
        /// <param name="itm">SearchResultItem object</param>
        /// <returns>Localized date string for when the table was published</returns>
        public static string GetSearchResultPublished(SearchResultItem itm)
        {
            string published = itm.Published.DateTimeToPxDateString();
            return published.PxDate();
        }
    }
}