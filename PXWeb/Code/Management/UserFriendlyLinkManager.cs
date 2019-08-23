using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using PCAxis.Web.Controls;

namespace PXWeb
{
    /// <summary>
    /// Class for generating user friendly links (URLs) in PX-Web
    /// </summary>
    public class UserFriendlyLinkManager
    {

        public static string GetVirtualPath()
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;

            if (path.Length == 1 && path.StartsWith("/"))
            {
                path = path.TrimStart('/');
            }
            return path;
        }

        public static string CreateLink(string page, bool formatHtmlEntities, params PCAxis.Web.Core.Management.LinkManager.LinkItem[] links)
        {
            PxUrl url = new PxUrl(links);

            if (page != null)
            {
                if (page.Contains("Default.aspx"))
                {
                    url.Database = null;
                    url.Path = null;
                    url.Table = null;
                    url.View = null;
                    url.Layout = null;
                }
                else if (page.Contains("Menu.aspx"))
                {
                    url.Table = null;
                    url.View = null;
                    url.Layout = null;
                }
                else if (page.Contains("Search.aspx"))
                {
                    url.Table = null;
                    url.View = null;
                    url.Layout = null;
                }
                else if (page.Contains("Selection.aspx"))
                {
                    url.View = null;
                    url.Layout = null;
                }
            }

            // Analyse and call the right link method
            if (url.Database == null && url.Table == null && url.Layout == null)
            {
                return CreateDefaultLink(url);
            }
            else if (url.Language != null && url.Database != null && url.Table == null && url.Layout == null)
            {
                return CreateMenuLink(page, url);
            }
            else if (url.Language != null && url.Database != null && url.Path != null && url.Table != null && url.Layout == null)
            {
                return CreateSelectionLink(page, url);
            }
            else if (url.Language != null && url.Database != null && url.Path != null && url.Table != null && url.Layout != null)
            {
                return CreatePresentationLink(page, url);
            }

            return "Default.aspx";
        }


        /// <summary>
        /// Create link to the start page
        /// </summary>
        /// <returns></returns>
        private static string CreateDefaultLink(PxUrl pxUrl)
        {
            StringBuilder url = new StringBuilder();
            url.Append(GetVirtualPath());
            url.Append("/");
            url.Append(PxUrl.PX_START + "/");

            if (pxUrl.Language != null)
            {
                url.Append(pxUrl.Language + "/");
            }
            else
            {
                url.Append(PXWeb.Settings.Current.General.Language.DefaultLanguage + "/");
            }

            AddQuerystringParameters(pxUrl, url);

            return url.ToString();
        }
        /// <summary>
        /// Create link to the menu page
        /// </summary>
        /// <returns></returns>

        private static string CreateMenuLink(string page, PxUrl pxUrl)
        {
            StringBuilder url = new StringBuilder();

            url.Append(GetVirtualPath());
            url.Append("/");
            url.Append(PxUrl.PX_START + "/");
            url.Append(pxUrl.Language + "/");
            url.Append(pxUrl.Database + "/");

            // Check if it is a menu sub page
            if ((page != null) && (page.Contains("Search.aspx")))
            {
                url.Append(PxUrl.VIEW_SEARCH + "/");
            }
            else
            {
                if (pxUrl.Path != null)
                {
                    if (!string.IsNullOrEmpty(pxUrl.Path))
                    {
                        url.Append(pxUrl.Path + "/");
                    }
                }
            }

            AddQuerystringParameters(pxUrl, url);

            return url.ToString();
        }

        /// <summary>
        /// Create link to the selection page
        /// </summary>
        /// <returns></returns>
        private static string CreateSelectionLink(string page, PxUrl pxUrl)
        {
            StringBuilder url = new StringBuilder();

            url.Append(GetVirtualPath());
            url.Append("/");
            url.Append(PxUrl.PX_START + "/");
            url.Append(pxUrl.Language + "/");
            url.Append(pxUrl.Database + "/");
            url.Append(pxUrl.Path + "/");
            url.Append(GetTableName(pxUrl.Table) + "/");

            // Check if it is a selection sub page
            if (page != null)
            {
                if (page.Contains(".aspx"))
                {
                    if (page.Contains("InformationSelection.aspx") || page.Contains("FootnotesSelection.aspx") || page.Contains("MarkingTips.aspx"))
                    {
                        url.Append(GetSelectionView(page) + "/");
                    }
                }
                else
                {
                    //If the last part of the friendly URL is not the table it is the view of the selection sub page
                    char[] separator = { '/' };
                    string[] parts = page.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    if (parts[parts.Length - 1] != pxUrl.Table)
                    {
                        url.Append(parts[parts.Length -1] + "/");
                    }
                }
            }

            AddQuerystringParameters(pxUrl, url);

            return url.ToString();
        }

        /// <summary>
        /// Create link to the presentation page
        /// </summary>
        /// <returns></returns>
        private static string CreatePresentationLink(string page, PxUrl pxUrl)
        {
            StringBuilder url = new StringBuilder();

            url.Append(GetVirtualPath());
            url.Append("/");
            url.Append(PxUrl.PX_START + "/");
            url.Append(pxUrl.Language + "/");
            url.Append(pxUrl.Database + "/");
            url.Append(pxUrl.Path + "/");
            url.Append(GetTableName(pxUrl.Table) + "/");
            url.Append(GetView(page, pxUrl.Layout) + "/");
            url.Append(pxUrl.Layout + "/");

            AddQuerystringParameters(pxUrl, url);

            return url.ToString();
        }

        /// <summary>
        /// Get table name
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string GetTableName(string table)
        {
            if (table.IndexOf(":") > 0)
            {
                // Remove database from CNMM table
                table = table.Substring(table.IndexOf(":") + 1);
            }
            return table;
        }


        /// <summary>
        /// Get view
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private static string GetSelectionView(string page)
        {
            if (page.Contains("MarkingTips.aspx"))
            {
                return PxUrl.VIEW_TIPS_IDENTIFIER;
            }
            else if (page.Contains("FootnotesSelection.aspx"))
            {
                return PxUrl.VIEW_FOOTNOTES_IDENTIFIER;
            }
            else if (page.Contains("InformationSelection.aspx"))
            {
                return PxUrl.VIEW_INFORMATION_IDENTIFIER;
            }

            return PxUrl.VIEW_FOOTNOTES_IDENTIFIER;
        }

        /// <summary>
        /// Get view
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private static string GetView(string page, string layout)
        {
            if (page.Contains("Table.aspx"))
            {
                return PxUrl.VIEW_TABLE_IDENTIFIER;
            }
            else if (page.Contains("Chart.aspx"))
            {
                return PxUrl.VIEW_CHART_IDENTIFIER;
            }
            else if (page.Contains("FootnotesPresentation.aspx"))
            {
                return PxUrl.VIEW_FOOTNOTES_IDENTIFIER;
            }
            else if (page.Contains("InformationPresentation.aspx"))
            {
                return PxUrl.VIEW_INFORMATION_IDENTIFIER;
            }
            else if (page.Contains("DataSort.aspx"))
            {
                return PxUrl.VIEW_SORTEDTABLE_IDENTIFIER;
            }
            else if (page.Contains("/" + layout + "/"))
            {
                char[] separator = { '/' };
                string[] parts = page.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                for (int i = parts.Length - 1; i >= 0; i--)
                {
                    if ((parts[i] == layout) && (i > 0))
                    {
                        return parts[i - 1];
                    }
                }
            }

            return PxUrl.VIEW_TABLE_IDENTIFIER;

        }

        /// <summary>
        /// Add querysring parameters to URL
        /// </summary>
        /// <param name="pxUrl">PXUrl object</param>
        /// <param name="url">Stringbuilder object</param>
        private static void AddQuerystringParameters(PxUrl pxUrl, StringBuilder url)
        {
            bool first = true;

            if (pxUrl.QuerystringParameters.Count > 0)
            {
                url.Append("?");

                foreach (KeyValuePair<string, string> param in pxUrl.QuerystringParameters)
                {
                    if (!first)
                    {
                        url.Append("&");
                    }

                    url.Append(param.Key + "=" + param.Value);
                    first = false;
                }
            }

        }
    }
}