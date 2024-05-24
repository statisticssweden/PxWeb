using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace PXWeb.Admin
{
    public partial class AdminDefault : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["adminlang"] == null)
            {
                Session["adminlang"] = PXWeb.Settings.Current.General.Language.DefaultLanguage;
            }

            BuildMenu();

            //Languages
            CultureInfo culture;
            foreach (LanguageSettings lang in PXWeb.Settings.Current.General.Language.SiteLanguages)
            {
                culture = new CultureInfo(lang.Name);
                cboLanguage.Items.Add(new ListItem(culture.NativeName, lang.Name));
            }

            if (!IsPostBack)
            {
                cboLanguage.SelectedValue = Session["adminlang"].ToString();
            }
        }

        private void BuildMenu()
        {
            LinkButton lnk;
            string url;

            foreach (SiteMapNode node in AdminSiteMap.RootNode.ChildNodes)
            {
                url = "";
                lnk = new LinkButton();
                lnk.Text = GetLocalizedString(node.Title);

                if (string.IsNullOrEmpty(node.Url))
                {
                    //Node in sitemap without a URL - get the URL from it´s first child...
                    url = GetUrlFromChild(node);
                }
                else
                {
                    url = node.Url;
                }

                if (!string.IsNullOrEmpty(url))
                {
                    lnk.PostBackUrl = url;
                    if ((AdminSiteMap.CurrentNode.IsDescendantOf(node)) || (AdminSiteMap.CurrentNode == node))
                    {
                        lnk.CssClass = "navigation_selected";
                    }
                    else
                    {
                        lnk.CssClass = "navigation_unselected";
                    }

                    pnlMenuLevel1.Controls.Add(lnk);

                }
            }
        }

        /// <summary>
        /// Get Menu node URL from the nodes first child. Called recursivly until a URL is found
        /// </summary>
        /// <param name="parent">The parent node</param>
        /// <returns>The URL. Is empty if no URL is found in children.</returns>
        private string GetUrlFromChild(SiteMapNode parent)
        {
            string url = "";

            if (parent.ChildNodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(parent.ChildNodes[0].Url))
                {
                    url = parent.ChildNodes[0].Url;
                }
                else
                {
                    url = GetUrlFromChild(parent.ChildNodes[0]);
                }
            }

            return url;
        }

        /// <summary>
        /// Get text in the currently selected language
        /// </summary>
        /// <param name="key">Key identifying the string in the language file</param>
        /// <returns>Localized string</returns>
        public string GetLocalizedString(string key)
        {
            return PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(key, new CultureInfo(GetSelectedLanguage()));
        }

        /// <summary>
        /// Get selected language
        /// </summary>
        /// <returns>If language is not selected english is set as selected language</returns>
        private string GetSelectedLanguage()
        {
            if (Session["adminlang"] == null)
            {
                Session["adminlang"] = "en";
            }

            return Session["adminlang"].ToString();
        }

        protected void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["adminlang"] = cboLanguage.SelectedValue;
            Response.Redirect(AdminSiteMap.CurrentNode.Url);
        }

        /// <summary>
        /// The Administration sitemap
        /// </summary>
        public SiteMapProvider AdminSiteMap
        {
            get
            {
                return SiteMap.Providers["AdminSiteMap"];
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }

    }
}
