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
using System.Xml;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using Px.Rdf;

namespace PXWeb.Admin
{
    public partial class Tools_XMLGenerator : System.Web.UI.Page
    {

        protected void clearDropDown(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.DataBind();
            ddl.ClearSelection();
        }
        protected void fillPxDatabases(DropDownList ddl)
        {
            clearDropDown(ddl);
            foreach (var db in PXWeb.Settings.Current.General.Databases.AllPxDatabases)
            {
                ddl.Items.Add(new ListItem(db.Id, db.Id));
            }
        }

        protected void fillCNMMDatabases(DropDownList ddl)
        {
            clearDropDown(ddl);
            foreach (var db in PXWeb.Settings.Current.General.Databases.AllCnmmDatabases)
            {
                ddl.Items.Add(new ListItem(db.Id, db.Id));
            }
        }
        protected void cboSelectDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSelectDbType.SelectedItem.Value == "PX") fillPxDatabases(cboSelectDb);
            else fillCNMMDatabases(cboSelectDb);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));
            if (!IsPostBack)
            {
                fillPxDatabases(cboSelectDb);
                ReadSettings();
            }

        }
        protected void imgSelectDb_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectDb", "PxWebAdminToolsXMLGeneratorSelectDbInfo");
        }

        protected void imgSelectDbType_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectDbType", "PxWebAdminToolsXMLGeneratorSelectDbTypeInfo");
        }

        protected void imgSelectPreferredLanguage_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectPreferredLanguage", "PxWebAdminToolsXMLGeneratorSelectPreferredLanguageInfo");
        }

        protected void imgSelectBaseURI_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectBaseURI", "PxWebAdminToolsXMLGeneratorSelectBaseURIInfo");
        }
        protected void imgSelectCatalogTitle_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectCatalogTitle", "PxWebAdminToolsXMLGeneratorSelectCatalogTitleInfo");
        }

        protected void imgSelectCatalogDesc_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectCatalogDesc", "PxWebAdminToolsXMLGeneratorSelectCatalogDescInfo");
        }
        protected void imgSelectLicense_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectLicense", "PxWebAdminToolsXMLGeneratorSelectLicenseInfo");
        }

        protected void imgSelectLandingPageURL_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectLandingPageURL", "PxWebAdminToolsXMLGeneratorSelectLandingPageURLInfo");
        }

        protected void imgSelectApiURL_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectApiURL", "PxWebAdminToolsXMLGeneratorSelectApiURLInfo");
        }
        
        protected void imgSelectPublisher_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectPublisher", "PxWebAdminToolsXMLGeneratorSelectPublisherInfo");
        }

        private string firstTwo(string s) {
            return s.Substring(0,2);
        }

        /// <summary>
        /// Read and display Charts settings  
        /// </summary>
        private void ReadSettings()
        {
            textBoxSelectBaseURI.Text = Settings.Current.Dcat.BaseURI;
            textBoxSelectApiURL.Text = Settings.Current.Dcat.BaseApiUrl;
            textBoxSelectLandingPageURL.Text = Settings.Current.Dcat.LandingPageUrl;
            textBoxSelectPublisher.Text = Settings.Current.Dcat.Publisher;
            textBoxSelectCatalogTitle.Text = Settings.Current.Dcat.CatalogTitle;
            textBoxSelectCatalogDesc.Text = Settings.Current.Dcat.CatalogDescription;
            cboSelectDb.Text = Settings.Current.Dcat.Database;
            cboSelectDbType.Text = Settings.Current.Dcat.DatabaseType;
            textBoxSelectLicense.Text = Settings.Current.Dcat.License;
        }

        protected void MasterSave_Click(object sender, EventArgs e) {
            if (PXWeb.Settings.BeginUpdate())
            {
                try
                {
                    PXWeb.DcatSettings dcats  = (PXWeb.DcatSettings)PXWeb.Settings.NewSettings.Dcat;
                    dcats.BaseURI = textBoxSelectBaseURI.Text;
                    dcats.BaseApiUrl = textBoxSelectApiURL.Text;
                    dcats.LandingPageUrl = textBoxSelectLandingPageURL.Text;
                    dcats.Publisher = textBoxSelectPublisher.Text;
                    dcats.CatalogTitle = textBoxSelectCatalogTitle.Text;
                    dcats.CatalogDescription = textBoxSelectCatalogDesc.Text;
                    dcats.Database = cboSelectDb.Text;
                    dcats.DatabaseType = cboSelectDbType.Text;
                    dcats.License = textBoxSelectLicense.Text;

                    PXWeb.Settings.Save();
                }
                finally
                {
                    PXWeb.Settings.EndUpdate();
                }
            }
        }
        protected void btnGenerateXML_Click(object sender, EventArgs e)
        {
            string baseURI = textBoxSelectBaseURI.Text;
            string catalogTitle = textBoxSelectCatalogTitle.Text;
            string catalogDescription = textBoxSelectCatalogDesc.Text;
            string license = textBoxSelectLicense.Text;
            string baseApiUrl = textBoxSelectApiURL.Text;
            string landingPageUrl = textBoxSelectLandingPageURL.Text;
            string publisher = textBoxSelectPublisher.Text;

            List<string> languages = new List<string>();
            string preferredLanguage = firstTwo(Settings.Current.General.Language.DefaultLanguage);
            foreach (LanguageSettings ls in Settings.Current.General.Language.SiteLanguages) {
                languages.Add(firstTwo(ls.Name));
            }
            string themeMapping = HttpContext.Current.Server.MapPath("~/TMapping.json");
            string dbType = cboSelectDbType.SelectedItem.Value;
            string dbid = "";
            IFetcher fetcher;

            string currPath = HttpContext.Current.Server.MapPath("~");

            switch (dbType) {
                case "PX":
                    dbid = HttpContext.Current.Server.MapPath("~/Resources/PX/Databases/") + cboSelectDb.SelectedItem.Value + "/Menu.xml";
                    string localThemeMapping = HttpContext.Current.Server.MapPath("~/Resources/PX/Databases/") + cboSelectDb.SelectedItem.Value + "/TMapping.json";
                    if (File.Exists(localThemeMapping)) themeMapping = localThemeMapping;
                    fetcher = new PXFetcher(HttpContext.Current.Server.MapPath("~/Resources/PX/Databases/"));
                    break;
                case "CNMM":
                    dbid = cboSelectDb.SelectedItem.Value;
                    fetcher = new CNMMFetcher();
                    break;
                default:
                    Master.ShowInfoDialog("Error", "Please select a database");
                    return;
            }

            RdfSettings settings = new RdfSettings {
                        BaseUri = baseURI, // Base uri, can be anything
        
                        BaseApiUrl = baseApiUrl,// Base url for api request

                        PreferredLanguage = preferredLanguage, // language code 2 letters

                        Languages =  languages,// Read from settings

                        CatalogTitle = catalogTitle,
                        CatalogDescription = catalogDescription,

                        PublisherName = publisher,
                        DBid = dbid,
                        Fetcher =  fetcher,// Create depending on chosen database
                        LandingPageUrl = landingPageUrl,
                        License = license,
                        ThemeMapping = themeMapping
            };
            try
            {
                XML.writeToFile(HttpContext.Current.Server.MapPath("~/dcat-ap.xml"), settings);
            }
            catch(PCAxis.Menu.Exceptions.InvalidMenuFromXMLException)
            {
                Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorLanguageError", "PxWebAdminToolsXMLGeneratorLanguageErrorInfo");
            }
            //Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectLicense", preferredLanguage);
        }
    }
}
