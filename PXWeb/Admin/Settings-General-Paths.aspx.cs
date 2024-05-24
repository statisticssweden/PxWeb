using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PXWeb.Admin
{
    public partial class Settings_General_Paths : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                ReadSettings();
            }
        }

        /// <summary>
        /// Read and display Databases settings  
        /// </summary>
        private void ReadSettings()
        {
            txtLanguagePath.Text = PXWeb.Settings.Current.General.Paths.LanguagesPath;
            txtImagePath.Text = PXWeb.Settings.Current.General.Paths.ImagesPath;
            txtDatabasePath.Text = PXWeb.Settings.Current.General.Paths.PxDatabasesPath;
            txtAggregationPath.Text = PXWeb.Settings.Current.General.Paths.PxAggregationsPath;
        }

        /// <summary>
        /// Save Paths settings
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
                        PXWeb.PathsSettings paths = (PXWeb.PathsSettings)PXWeb.Settings.NewSettings.General.Paths;

                        paths.LanguagesPath = txtLanguagePath.Text;
                        paths.ImagesPath = txtImagePath.Text;
                        paths.PxDatabasesPath = txtDatabasePath.Text;
                        paths.PxAggregationsPath = txtAggregationPath.Text;

                        PXWeb.Settings.Save();

                        //Paths have been changed - Reset registry objects
                        PCAxis.Paxiom.GroupRegistry.GetRegistry().ReloadGroupings();
                        PCAxis.Web.Core.Management.LocalizationManager.ResetResourceManager();
                        PXWeb.DatabasesSettings databases = (PXWeb.DatabasesSettings)PXWeb.Settings.Current.General.Databases;
                        databases.ResetDatabases();
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// Validate the path settings
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidatePath(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }
            else
            {
                if (!System.IO.Directory.Exists(Server.MapPath(args.Value)))
                {
                    args.IsValid = false;
                    val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationNonexistentPath");
                    return;
                }
            }

            args.IsValid = true;
        }

        protected void imgLanguagePath_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPathsLanguagePath", "PxWebAdminSettingsPathsLanguagePathInfo");
        }

        protected void imgImagePath_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPathsImagePath", "PxWebAdminSettingsPathsImagePathInfo");
        }

        protected void imgDatabasePath_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPathsPxDatabasePath", "PxWebAdminSettingsPathsPxDatabasePathInfo");
        }

        protected void imgAggregationPath_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPathsAggregationPath", "PxWebAdminSettingsPathsAggregationPathInfo");
        }


    }
}
