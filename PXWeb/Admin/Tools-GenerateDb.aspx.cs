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
using PXWeb.Database;

namespace PXWeb.Admin
{
    public partial class Tools_GenerateDb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnGenerate.Attributes.Add("onclick", "return confirm('" + Master.GetLocalizedString("PxWebAdminConfirmGenerateDb") + "');");

            if (!IsPostBack)
            {
                foreach (var db in PXWeb.Settings.Current.General.Databases.AllPxDatabases)
                {
                    cboSelectDb.Items.Add(new ListItem(db.Id, db.Id));
                }

                if (!PXWeb.Settings.Current.Features.General.SearchEnabled)
                {
                    divCreateIndex.Visible = false;
                }
            }
        }

        protected void imgSelectDb_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsGenerateDbSelectDb", "PxWebAdminToolsGenerateDbSelectDbInfo");
        }

        protected void imgSelectCnmmDb_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsGenerateCnmmDbSelectDb", "PxWebAdminToolsGenerateCnmmDbSelectDbInfo");
        }

        protected void imgLanguageDependent_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsGenerateDbLanguageDependent", "PxWebAdminToolsGenerateDbLanguageDependentInfo");
        }

        protected void imgSortOrder_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsGenerateDbSortOrder", "PxWebAdminToolsGenerateDbSortOrderInfo");
        }

        protected void CreateIndexInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsGenerateDbCreateIndex", "PxWebAdminToolsGenerateDbCreateIndexInfo");
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string path;

            path = Server.MapPath(Settings.Current.General.Paths.PxDatabasesPath);
            path = System.IO.Path.Combine(path, cboSelectDb.SelectedValue);

            bool langDependent = false;
            if (string.Compare(cboLanguageDependent.SelectedValue, "true", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                langDependent = true;
            }

            grdResult.DataSource = AdminTool.GenerateDatabase(path, langDependent, cboSortOrder.SelectedValue);
            grdResult.DataBind();

            //Force that databases are read again
            PXWeb.DatabasesSettings databases = (PXWeb.DatabasesSettings)PXWeb.Settings.Current.General.Databases;
            databases.ResetDatabases();

            if (PXWeb.Settings.Current.Features.General.SearchEnabled)
            {
                if (chkCreateIndex.Checked)
                {
                    PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(cboSelectDb.SelectedValue);
                    PXWeb.SearchIndexSettings searchIndex = (PXWeb.SearchIndexSettings)db.SearchIndex;

                    // Check that the status has not been changed by another system before updating it
                    if (searchIndex.Status != SearchIndexStatusType.Indexing)
                    {
                        searchIndex.Status = SearchIndexStatusType.WaitingCreate;
                        db.Save();
                    }
                    InfoBox.Visible = true;
                }
            }

        }

    }
}
