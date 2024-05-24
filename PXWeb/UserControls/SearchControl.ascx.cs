using PCAxis.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace PXWeb.UserControls
{
    public partial class SearchControl : System.Web.UI.UserControl
    {

        #region "Localized strings"
        private const string SEARCH_LABEL = "PxWebSearchControlLabel";
        private const string SEARCH_BUTTONTEXT = "PxWebSearchControlButtonText";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetLocalizedTexts();
            }
            lblError.Text = "";
        }

        protected void GoToSearch(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim().Length > 0)
            {
                string error;
                if (!PXWeb.Misc.InputValidation.ValidateNoIllegalCharcters(txtSearch.Text, out error))
                {
                    lblError.Text = LocalizationManager.GetLocalizedString(error);
                    return;
                }

                if (RedirectOnSearch)
                {
                    List<LinkManager.LinkItem> linkItems = new List<LinkManager.LinkItem>();
                    linkItems.Add(new LinkManager.LinkItem("searchquery", txtSearch.Text));
                    string url = PCAxis.Web.Core.Management.LinkManager.CreateLink("Search.aspx", linkItems.ToArray());

                    Response.Redirect(url);
                }
            }
        }

        private void SetLocalizedTexts()
        {
            var pxUrl = RouteInstance.PxUrlProvider.Create(null);

            string db = pxUrl.Database;
            string lang = pxUrl.Language;
            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(db);

            lblSearch.Text = string.Format(LocalizationManager.GetLocalizedString(SEARCH_LABEL), dbi.GetDatabaseName(lang));
            cmdSearch.Text = LocalizationManager.GetLocalizedString(SEARCH_BUTTONTEXT);
        }

        public string SearchText
        {
            get
            {
                return txtSearch.Text.Trim();
            }
            set
            {
                txtSearch.Text = value;
            }
        }

        public bool RedirectOnSearch
        {
            get
            {
                return bool.Parse(hidRedirect.Value);
            }
            set
            {
                hidRedirect.Value = value.ToString();
            }
        }

        //private string GetAppPath()
        //{
        //    string appPath = string.Empty;
        //    HttpContext context = HttpContext.Current;

        //    appPath = string.Format("{0}://{1}{2}{3}", context.Request.Url.Scheme,
        //                                               context.Request.Url.Host,
        //                                               context.Request.Url.Port.Equals(80) ? String.Empty : ":" + context.Request.Url.Port,
        //                                               context.Request.ApplicationPath);
        //    return appPath;
        //}

    }
}
