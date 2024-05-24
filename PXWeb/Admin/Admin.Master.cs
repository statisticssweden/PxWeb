using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PXWeb.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave1.Attributes.Add("onclick", "return confirm('" + GetLocalizedString("PxWebAdminConfirmSaveSettings") + "');");
            this.btnSave2.Attributes.Add("onclick", "return confirm('" + GetLocalizedString("PxWebAdminConfirmSaveSettings") + "');");
        }


        #region Build menu methods

        /// <summary>
        /// Builds the administration menu
        /// </summary>
        private void BuildMenu()
        {
            btnSave1.Visible = false;
            btnSave2.Visible = false;

            int level = FindMenuLevels();
            int i = 1;
            SiteMapNode parent = AdminSiteMap.RootNode;

            while ((i <= level) && (parent != null))
            {
                parent = BuildMenuLevel(i, parent);
                i++;
            }
        }

        /// <summary>
        /// Builds one level of the administration menu
        /// </summary>
        /// <param name="level">The menu level to build</param>
        /// <param name="parent">Parent node for the current page at the specific level</param>
        /// <returns></returns>
        private SiteMapNode BuildMenuLevel(int level, SiteMapNode parent)
        {
            LinkButton lnk;
            SiteMapNode nextParent = null;
            string url;

            //Can only have level 1-3
            if ((level == 0) || (level > 3))
            {
                return null;
            }

            foreach (SiteMapNode node in parent.ChildNodes)
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
                        nextParent = node;
                        lnk.CssClass = "navigation_selected";
                    }
                    else
                    {
                        lnk.CssClass = "navigation_unselected";
                    }

                    switch (level)
                    {
                        case 1:
                            pnlMenuLevel1.Controls.Add(lnk);
                            break;
                        case 2:
                            pnlMenuLevel2.Controls.Add(lnk);
                            break;
                        case 3:
                            pnlMenuLevel3.Controls.Add(lnk);
                            pnlMenuLevel3.Controls.Add(new LiteralControl("<br />"));
                            break;
                        default:
                            break;
                    }
                }
            }

            return nextParent;
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
        /// Find out how many levels down in the administration sitemap the currently selected page is located.
        /// </summary>
        /// <returns>Level (in the sitemap) of the currently selected page</returns>
        private int FindMenuLevels()
        {
            int level = 0;
            SiteMapNode node = AdminSiteMap.CurrentNode;

            while ((node != AdminSiteMap.RootNode) && (node != null))
            {
                level++;
                node = node.ParentNode;
            }
            return level;
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


        #endregion

        #region Methods used by content pages
        /// <summary>
        /// Enables save-functionality in content page
        /// </summary>
        /// <param name="saveHandler">Handler of the save event in the content page</param>
        public void EnableSave(EventHandler saveHandler)
        {
            btnSave1.Visible = true;
            btnSave1.Click += saveHandler;
            btnSave2.Visible = true;
            btnSave2.Click += saveHandler;
        }

        /// <summary>
        /// Get text in the currently selected language
        /// </summary>
        /// <param name="key">Key identifying the string in the language file</param>
        /// <returns>Localized string</returns>
        public string GetLocalizedString(string key)
        {
            return PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(key, new CultureInfo(Session["adminlang"].ToString()));
        }

        /// <summary>
        /// Show modal dialog with information
        /// </summary>
        /// <param name="keyTitle">key (in language file) for the title</param>
        /// <param name="keyText">key (in language file) for the text</param>
        public void ShowInfoDialog(string keyTitle, string keyText)
        {
            dialogmodal.Visible = true;
            dialogmodal.Attributes["title"] = GetLocalizedString(keyTitle);
            litInfoDialog.Text = GetLocalizedString(keyText);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowModalDialog", "<script type='text/javascript'> jQuery(function() { jQuery('#" + dialogmodal.ClientID + "').dialog({ width: 480, height: 400,  modal: true }); });</script>  ");
        }

        /// <summary>
        /// Returns the selected culture 
        /// </summary>
        /// <returns>Current culture</returns>
        public System.Globalization.CultureInfo SelectedCulture()
        {
            return new CultureInfo(Session["adminlang"].ToString());
        }

        #endregion


        protected void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["adminlang"] = cboLanguage.SelectedValue;
            Response.Redirect(AdminSiteMap.CurrentNode.Url);
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }

    }
}
