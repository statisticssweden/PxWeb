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
using PCAxis.Web.Controls.CommandBar.Plugin;
using System.Collections.Generic;
using PXWeb.Misc;

namespace PXWeb.Admin
{
    public partial class Features_General : System.Web.UI.Page
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
        /// Read and display Features settings  
        /// </summary>
        private void ReadSettings()
        {
            chkCharts.Checked = PXWeb.Settings.Current.Features.General.ChartsEnabled;
            chkApi.Checked = PXWeb.Settings.Current.Features.General.ApiEnabled;
            chkSavedQuery.Checked = PXWeb.Settings.Current.Features.General.SavedQueryEnabled;
            chkUserFriendlyUrls.Checked = PXWeb.Settings.Current.Features.General.UserFriendlyUrlsEnabled;
            chkUserStatistics.Checked = PXWeb.Settings.Current.Features.General.UserStatisticsEnabled;
            chkSearch.Checked = PXWeb.Settings.Current.Features.General.SearchEnabled;
            chkBackgroundWorker.Checked = PXWeb.Settings.Current.Features.General.BackgroundWorkerEnabled;
            chkBulkLink.Checked = PXWeb.Settings.Current.Features.General.BulkLinkEnabled;
            txtClearCache.Text = PXWeb.Settings.Current.Features.General.ClearCache;
            
        }

        /// <summary>
        /// Save Features settings
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
                        PXWeb.FeaturesGeneralSettings general = (PXWeb.FeaturesGeneralSettings)PXWeb.Settings.NewSettings.Features.General;

                        general.ChartsEnabled = chkCharts.Checked;

                        if (!general.ChartsEnabled)
                        {
                            RemoveChartSettings();
                        }

                        general.ApiEnabled = chkApi.Checked;
                        UpdateRouteTable();

                        general.UserFriendlyUrlsEnabled = chkUserFriendlyUrls.Checked;
                        if (general.UserFriendlyUrlsEnabled)
                        {
                            PCAxis.Web.Core.Management.LinkManager.CreateLinkMethod = new PCAxis.Web.Core.Management.LinkManager.LinkMethod(PXWeb.UserFriendlyLinkManager.CreateLink);
                        }
                        else
                        {
                            PCAxis.Web.Core.Management.LinkManager.CreateLinkMethod = null;
                        }

                        general.UserStatisticsEnabled = chkUserStatistics.Checked;
                        general.SearchEnabled = chkSearch.Checked;
                        general.BackgroundWorkerEnabled = chkBackgroundWorker.Checked;
                        general.BulkLinkEnabled = chkBulkLink.Checked;
                        general.SavedQueryEnabled = chkSavedQuery.Checked;
                        general.ClearCache = txtClearCache.Text.Replace(" ","");
                        

                        PXWeb.Settings.Save();

                        if (chkBackgroundWorker.Checked)
                        {
                            BackgroundWorker.PxWebBackgroundWorker.Work(PXWeb.Settings.Current.Features.BackgroundWorker.SleepTime);
                        }
                        else
                        {
                            BackgroundWorker.PxWebBackgroundWorker.Stop();
                        }
                        //
                        PCAxis.Api.Settings.Current.ClearCache = general.ClearCache;
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// Update RouteTable depending on if API is enabled or not
        /// </summary>
        private void UpdateRouteTable()
        {
            if (chkApi.Checked)
            {
                RouteManager.AddApiRoute();
            }
            else
            {
                RouteManager.RemoveApiRoute();
            }
        }

        /// <summary>
        /// Removes all chart related settings
        /// </summary>
        private void RemoveChartSettings()
        {
            // Remove chart from Commandbar settings
            CommandBarSettings cmd = (CommandBarSettings)PXWeb.Settings.NewSettings.Presentation.CommandBar;
            RemovePresentationViewsByCategory((List<string>)cmd.PresentationViews, PCAxis.Web.Controls.Plugins.Categories.CHART);
            RemovePresentationViewsByCategory((List<string>)cmd.PresentationViewShortcuts, PCAxis.Web.Controls.Plugins.Categories.CHART);
            RemovePresentationViewsByCategory((List<string>)cmd.PresentationViewButtons, PCAxis.Web.Controls.Plugins.Categories.CHART);
            RemovePresentationViewsByCategory((List<string>)cmd.CommandBarShortcuts, PCAxis.Web.Controls.Plugins.Categories.CHART);
            RemoveFileFormatsByCategory((List<string>)cmd.OutputFormats, PCAxis.Web.Controls.Plugins.Categories.CHART);
            RemoveFileFormatsByCategory((List<string>)cmd.OutputFormatShortcuts, PCAxis.Web.Controls.Plugins.Categories.CHART);
            RemoveFileTypesByCategory((List<string>)cmd.FileTypeButtons, PCAxis.Web.Controls.Plugins.Categories.CHART);
        }

        /// <summary>
        /// Removes all presentation views from the specified list that have the given category
        /// </summary>
        /// <param name="lst">List with presentation views</param>
        /// <param name="category">Category to remove</param>
        private void RemovePresentationViewsByCategory(List<string> lst, string category)
        {
            CommandBarPluginInfo plugin;

            for (int i = lst.Count - 1; i >= 0; i--)
            {
                plugin = CommandBarPluginManager.Views[lst[i]];
                if (plugin != null)
                {
                    if (plugin.Category != null)
                    {
                        if (plugin.Category.Equals(category))
                        {
                            lst.RemoveAt(i);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes all file formats from the specified list that have the given category
        /// </summary>
        /// <param name="lst">List with file formats</param>
        /// <param name="category">Category to remove</param>
        private void RemoveFileFormatsByCategory(List<string> lst, string category)
        {
            //PCAxis.Web.Controls.FileGenerator fileGenerator = new PCAxis.Web.Controls.FileGenerator(Master.SelectedCulture());
            
            for (int i = lst.Count - 1; i >= 0; i--)
            {
                if (CommandBarPluginManager.GetFileType(lst[i]).Category.Equals(category))
                {
                    lst.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Removes all file types from the specified list that have the given category
        /// </summary>
        /// <param name="lst">List with file types</param>
        /// <param name="category">Category to remove</param>
        private void RemoveFileTypesByCategory(List<string> lst, string category)
        {
            //PCAxis.Web.Controls.FileGenerator fileGenerator = new PCAxis.Web.Controls.FileGenerator(Master.SelectedCulture());
            PCAxis.Web.Core.FileType fileType;

            for (int i = lst.Count - 1; i >= 0; i--)
            {
                fileType = CommandBarPluginManager.FileTypes[lst[i]];

                if (fileType != null)
                {
                    if (fileType.Category != null)
                    {
                        if (fileType.Category.Equals(category))
                        {
                            lst.RemoveAt(i);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validates that value is an string is format correct
        /// and check for illegal characters
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateClearCacheTimes(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateNoIllegalCharcters(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }
            if (InputValidation.ValidateStringOfTimes(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }
        }
        /// <summary>
        /// Set error message for validator with invalid value
        /// </summary>
        /// <param name="val">Validator object</param>
        /// <param name="args">Validator arguments</param>
        /// <param name="errorKey">Key for error message</param>
        /// <param name="parameters">Eventual parameters to the localized string</param>
        private void SetValidationError(CustomValidator val, System.Web.UI.WebControls.ServerValidateEventArgs args, string errorKey, params string[] parameters)
        {
            args.IsValid = false;
            if (parameters.Length > 0)
            {
                val.ErrorMessage = string.Format(Master.GetLocalizedString(errorKey), parameters);
            }
            else
            {
                val.ErrorMessage = Master.GetLocalizedString(errorKey);
            }
        }
        // Help about enabling/disabling features
        protected void imgFeature_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesGeneralFeature", "PxWebAdminFeaturesGeneralFeatureInfo");
        }
        protected void ClearCacheInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesGeneralClearCache", "PxWebAdminFeaturesGeneralClearCacheInfo");
        }

    }
}
