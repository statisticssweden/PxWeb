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
    public partial class Settings_General_Site : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                // Read settings
                txtApplicationName.Text = PXWeb.Settings.Current.General.Site.ApplicationName;
                cboMainHeaderForTables.SelectedValue = PXWeb.Settings.Current.General.Site.MainHeaderForTables.ToString();
            }
        }

        /// <summary>
        /// Save Site settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                if (PXWeb.Settings.BeginUpdate())
                {
                    try
                    {
                        PXWeb.SiteSettings site = (PXWeb.SiteSettings)PXWeb.Settings.NewSettings.General.Site;
                        site.ApplicationName = txtApplicationName.Text;
                        site.LogoPath = txtLogoPath.Text;
                        switch (cboMainHeaderForTables.SelectedValue)
                        {
                            case "TableName":
                                site.MainHeaderForTables = MainHeaderForTablesType.TableName;
                                break;
                            case "StatisticArea":
                                site.MainHeaderForTables = MainHeaderForTablesType.StatisticArea;
                                break;
                            default:
                                site.MainHeaderForTables = MainHeaderForTablesType.TableName;
                                break;
                        }

                        PXWeb.Settings.Save();
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                    }

                }
            }
        }

        /// <summary>
        /// Validate the Logo path settings
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateLogoPath(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string path;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }
            else
            {
                path = System.IO.Path.Combine(PXWeb.Settings.Current.General.Paths.ImagesPath, args.Value);
                if (!System.IO.File.Exists(Server.MapPath(path)))
                {
                    args.IsValid = false;
                    val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationNonexistentImage");
                    return;
                }
            }

            args.IsValid = true;
        }


        protected void imgApplicationName_Click(object sender, ImageClickEventArgs e)
        { 
            Master.ShowInfoDialog("PxWebAdminSettingsGeneralSiteApplicationName", "PxWebAdminSettingsGeneralSiteApplicationNameInfo");
        }

        protected void imgLogoPath_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsGeneralSiteLogoPath", "PxWebAdminSettingsGeneralSiteLogoPathInfo");
        }

        protected void imgMainHeaderForTables_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMainHeaderForTables", "PxWebAdminSettingsMainHeaderForTablesInfo");
        }
    }
}
