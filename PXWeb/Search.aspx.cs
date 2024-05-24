using PCAxis.Search;
using PCAxis.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace PXWeb
{
    public partial class Search : System.Web.UI.Page
    {
        private string _searchQuery;
        private string _database;
        private string _language;
        private DatabaseInfo _dbi;

        #region "Localized strings"
        private const string SEARCH_RESULTTEXT = "PxWebSearchResultText";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            _database = PxUrl.Database;
            _language = PxUrl.Language;
            _dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(_database);

            if (!Page.IsPostBack)
            {
                _searchQuery = QuerystringManager.GetQuerystringParameter("searchquery");
                Master.SetBreadcrumb(PCAxis.Web.Controls.Breadcrumb.BreadcrumbMode.MenuSubPage, "Search");
                Master.SetH1TextDatabase();
                Master.SetNavigationFlowMode(PCAxis.Web.Controls.NavigationFlow.NavigationFlowMode.First);
                Master.SetNavigationFlowVisibility(PXWeb.Settings.Current.Navigation.ShowNavigationFlow);
                PCAxis.Web.Core.Management.PaxiomManager.Clear();

                if (!string.IsNullOrEmpty(_searchQuery))
                {
                    pxSearch.SearchText = _searchQuery;
                }

                pxSearch.RedirectOnSearch = false;
            }
            else
            {
                _searchQuery = pxSearch.SearchText;
            }

            chkCodes.Enabled = !rbAll.Checked;
            chkTitle.Enabled = !rbAll.Checked;
            chkValues.Enabled = !rbAll.Checked;

            if (PXWeb.Settings.Current.Features.General.SearchEnabled == false)
            {
                pxSearch.Visible = false;
                lblSearchResult.Visible = false;
                repSearchResult.Visible = false;
                return;
            }

            string error;
            if (!PXWeb.Misc.InputValidation.ValidateNoIllegalCharcters(_searchQuery, out error))
            {
                //Return and let SearchControl.ascx print the error message
                return;
            }


            if (!string.IsNullOrEmpty(_searchQuery))
            {
                DoSearch();
            }

        }

        private IPxUrl _pxUrl = null;
        private IPxUrl PxUrl
        {
            get
            {
                if (_pxUrl == null)
                {
                    _pxUrl = RouteInstance.PxUrlProvider.Create(null);
                }

                return _pxUrl;
            }
        }

        private void DoSearch()
        {
            string db = PxUrl.Database;
            string lang = PxUrl.Language;
            int hits = 0;

            if (!string.IsNullOrEmpty(db) && !string.IsNullOrEmpty(lang) && !string.IsNullOrEmpty(_searchQuery))
            {
                string filter = GetFilter();
                PCAxis.Search.SearchStatusType status;
                List<SearchResultItem> result = new List<SearchResultItem>();
                List<SearchResultItem> tmpResult = SearchManager.Current.Search(db, lang, _searchQuery, out status, filter, PXWeb.Settings.Current.Features.Search.ResultListLength);

                foreach (var searchResultItem in tmpResult)
                {
                    if (!result.Any(x => x.Table == searchResultItem.Table))
                    {
                        result.Add(searchResultItem);
                    }
                }

                repSearchResult.DataSource = result;
                repSearchResult.DataBind();
                hits = result.Count;
            }

            lblSearchResult.Text = string.Format(Master.GetLocalizedString(SEARCH_RESULTTEXT), hits);
        }

        /// <summary>
        /// Get filter string
        /// </summary>
        /// <returns>String with the fields to search in. Empty string means all fields shall be searched</returns>
        private string GetFilter()
        {
            StringBuilder filter = new StringBuilder();
            bool first = true;

            if (rbSelect.Checked)
            {
                if (chkTitle.Checked)
                {
                    filter.Append(PCAxis.Search.SearchConstants.SEARCH_FIELD_TITLE);
                    first = false;
                }

                if (chkValues.Checked)
                {
                    if (!first)
                    {
                        filter.Append(",");
                    }
                    filter.Append(PCAxis.Search.SearchConstants.SEARCH_FIELD_VALUES);
                    first = false;
                }

                if (chkCodes.Checked)
                {
                    if (!first)
                    {
                        filter.Append(",");
                    }
                    filter.Append(PCAxis.Search.SearchConstants.SEARCH_FIELD_CODES);
                    first = false;
                }
            }

            return filter.ToString();
        }

        protected string CreateUrl(object o)
        {
            SearchResultItem itm = (SearchResultItem)o;
            return SearchHelper.GetSearchResultUrl(itm, _dbi, _language);
        }

        protected string GetPublished(object o)
        {
            SearchResultItem itm = (SearchResultItem)o;
            return SearchHelper.GetSearchResultPublished(itm);
        }
    }
}
