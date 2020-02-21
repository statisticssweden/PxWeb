using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace PXWeb.Admin
{
    public partial class Tools_Reset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnResetAll_Click(object sender, EventArgs e)
        {
            AdminTool.ResetAll();
        }

        protected void btnResetLanguage_Click(object sender, EventArgs e)
        {
            PCAxis.Paxiom.Localization.PxResourceManager.ResetResourceManager();
            PXWeb.LanguagesSettings langs = (PXWeb.LanguagesSettings)PXWeb.Settings.Current.General.Language;
            langs.ResetLanguages();
        }

        protected void btnResetAggregation_Click(object sender, EventArgs e)
        {
            PCAxis.Paxiom.GroupRegistry.GetRegistry().ReloadGroupingsAsync();
        }
        protected void btnClearCaches_Click(object sender, EventArgs e)
        {
            // Clear all caches
            PXWeb.Management.PxContext.CacheController.Clear();
        }

        protected void btnResetDatabases_Click(object sender, EventArgs e)
        {
            PXWeb.DatabasesSettings databases = (PXWeb.DatabasesSettings)PXWeb.Settings.Current.General.Databases;
            databases.ResetDatabases();
            //Reload settings per database
            PXWeb.Settings.Current.LoadDatabaseSettings();
        }

        protected void imgResetAll_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsResetAll", "PxWebAdminToolsResetAllInfo");
        }

        protected void imgResetLanguage_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsResetLanguage", "PxWebAdminToolsResetLanguageInfo");
        }

        protected void imgResetAggregation_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsResetAggregation", "PxWebAdminToolsResetAggregationInfo");
        }

        protected void imgResetDatabases_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsResetDatabases", "PxWebAdminToolsResetDatabasesInfo");
        }

        protected void imgClearCaches_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsResetClearCaches", "PxWebAdminToolsResetClearCachesInfo");
        }
    }
}
