using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PCAxis.Web.Core.Management;

namespace PXWeb
{
    /// <summary>
    /// Represents a URL in PX-Web
    /// </summary>
    public class PxUrl : IPxUrl
    {
        #region "Constants"
        //Parts of the user friendly URL
        //------------------------------

        /// <summary>
        /// Start for all PX-Web URL:s
        /// </summary>
        public const string PX_START = "pxweb";

        /// <summary>
        /// Start for short-links
        /// </summary>
        public const string PX_GOTO = "goto"; 

        /// <summary>
        /// Defines language
        /// </summary>
        public const string LANGUAGE_IDENTIFIER = "lang";
        public const string LANGUAGE_KEY = "px_language";

        /// <summary>
        /// Defines database
        /// </summary>
        public const string DB_IDENTIFIER = "db";
        public const string DB_KEY = "px_db";

        /// <summary>
        /// Defines path within database
        /// </summary>
        public const string PATH_IDENTIFIER = "path";
        public const string PATH_KEY = "px_path";

        /// <summary>
        /// Defines table
        /// </summary>
        public const string TABLE_IDENTIFIER = "table";
        public const string TABLE_KEY = "px_tableid";

        /// <summary>
        /// Defines pages
        /// </summary>
        public const string PAGE_SELECT = "select";

        /// <summary>
        /// Defines presentation view
        /// </summary>
        public const string VIEW_IDENTIFIER = "view";
        public const string VIEW_TABLE_IDENTIFIER = "table";
        public const string VIEW_SORTEDTABLE_IDENTIFIER = "sortedtable";
        public const string VIEW_CHART_IDENTIFIER = "chart";
        public const string VIEW_FOOTNOTES_IDENTIFIER = "footnotes";
        public const string VIEW_INFORMATION_IDENTIFIER = "information";
        public const string VIEW_TIPS_IDENTIFIER = "tips";
        public const string VIEW_SEARCH = "search";

        /// <summary>
        /// Defines presentation view layout
        /// </summary>
        public const string LAYOUT_IDENTIFIER = "layout";
        public const string LAYOUT_KEY = "layout";

        //Querystring parameters
        //----------------------

        #endregion

        #region "Private fields"

        /// <summary>
        /// Querystring parameters
        /// </summary>
        private List<KeyValuePair<string, string>> _params;
        
        #endregion

        #region "Public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        public PxUrl()
        {
            _params = new List<KeyValuePair<string, string>>();

        }

        /// <summary>
        /// Constructor. Initializes the PxUrl with the querystring parameters
        /// </summary>
        /// <param name="links">Array of querystring parameters</param>
        public PxUrl(params PCAxis.Web.Core.Management.LinkManager.LinkItem[] links)
        {
            Language = GetParameter(PxUrl.LANGUAGE_KEY);
            Layout = GetParameter(PxUrl.LAYOUT_KEY);
            View = GetParameter(PxUrl.VIEW_IDENTIFIER);
            _params = new List<KeyValuePair<string, string>>();
            Dictionary<string, string> queries;

            // Get querystring items from subscribers
            queries = PCAxis.Web.Core.Management.LinkManager.GetQueries();
            foreach (KeyValuePair<string, string> querie in queries)
            {
                this.AddParameter(querie.Key, HttpUtility.UrlPathEncode(querie.Value));
            }

            if (links != null)
            {
                foreach (PCAxis.Web.Core.Management.LinkManager.LinkItem itm in links)
                {
                    this.AddParameter(itm.Key, itm.Value);
                }
            }
        }

        /// <summary>
        /// Add parameter to URL
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddParameter(string key, string value)
        {
            // Check that the value do not contain illegal characters
            if (!QuerystringManager.CheckValue(value))
            {
                return;
            }

            switch (key)
            {
                case LANGUAGE_KEY:
                    Language = System.Web.HttpUtility.UrlDecode(value);
                    break;
                case DB_KEY:
                    Database = System.Web.HttpUtility.UrlDecode(value);
                    break;
                case PATH_KEY:
                    Path = System.Web.HttpUtility.UrlDecode(value);
                    break;
                case TABLE_KEY:
                    Table = System.Web.HttpUtility.UrlDecode(value);
                    break;
                case LAYOUT_KEY:
                    Layout = System.Web.HttpUtility.UrlDecode(value);
                    break;
                default:
                    _params.Add(new KeyValuePair<string, string>(key, value));
                    break;
            }
        }

        #endregion

        #region "Public properties"

        public string Language { get; set; }
        public string Database { get; set; }
        public string Path { get; set; }
        public string Table { get; set; }
        public string View { get; set; }
        public string Layout { get; set; }
        public List<KeyValuePair<string, string>> QuerystringParameters { get { return _params; } }
        public string TablePath { get { return Path + PCAxis.Web.Controls.PathHandler.NODE_DIVIDER + Table; } }

        #endregion

        #region "Public static methods"

        /// <summary>
        /// Get querystring or routedata value depending on if user friendly URL:s are used or not
        /// </summary>
        /// <param name="key">parameter key</param>
        /// <returns>Parameter value</returns>
        public static string GetParameter(string key)
        {
            if (PXWeb.Settings.Current.Features.General.UserFriendlyUrlsEnabled)
            {
                System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;

                if (page != null)
                {
                    if (page.RouteData.Values[key] != null)
                    {
                        return ValidationManager.GetValue(page.RouteData.Values[key].ToString());
                    }
                }
            }
            else
            {
                if (QuerystringManager.GetQuerystringParameter(key) != null)
                {
                    return QuerystringManager.GetQuerystringParameter(key);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the view name by Page class
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetView(Page page)
        {
            if (page is Table)
            {
                return PxUrl.VIEW_TABLE_IDENTIFIER;
            }
            else if (page is Chart)
            {
                return PxUrl.VIEW_CHART_IDENTIFIER;
            }
            else if (page is FootnotesPresentation)
            {
                return PxUrl.VIEW_FOOTNOTES_IDENTIFIER;
            }
            else if (page is InformationPresentation)
            {
                return PxUrl.VIEW_INFORMATION_IDENTIFIER;
            }
            else if (page is DataSort)
            {
                return PxUrl.VIEW_SORTEDTABLE_IDENTIFIER;
            }

            return PxUrl.VIEW_TABLE_IDENTIFIER;

        }

        #endregion
    }
}
