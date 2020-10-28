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
using System.Collections.Generic;
using PCAxis.Web.Controls.CommandBar.Plugin;
using PCAxis.Web.Core.Management;

namespace PXWeb.Admin
{
    public partial class Settings_Selection : System.Web.UI.Page
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
        /// Read and display Selection settings  
        /// </summary>
        private void ReadSettings()
        {
            cboShowMandatoryMark.SelectedValue = PXWeb.Settings.Current.Selection.ShowMandatoryMark.ToString();
            cboAllowAggregations.SelectedValue = PXWeb.Settings.Current.Selection.AllowAggregations.ToString();
            cboShowHierarchies.SelectedValue = PXWeb.Settings.Current.Selection.Hierarchies.ShowHierarchies.ToString();
            txtHierarchicalLevelsOpen.Text = PXWeb.Settings.Current.Selection.Hierarchies.HierarchicalLevelsOpen.ToString();
            cboShowMarkingTips.SelectedValue = PXWeb.Settings.Current.Selection.MarkingTips.ShowMarkingTips.ToString();
            cboSearchButtonMode.SelectedValue = PXWeb.Settings.Current.Selection.SearchButtonMode.ToString();
            txtMaxRowsWithoutSearch.Text = PXWeb.Settings.Current.Selection.MaxRowsWithoutSearch.ToString();
            cboAlwaysShowTimeVariableWithoutSearch.SelectedValue = PXWeb.Settings.Current.Selection.AlwaysShowTimeVariableWithoutSearch.ToString();
            txtListSize.Text = PXWeb.Settings.Current.Selection.ListSize.ToString();
            cboShowSelectionLimits.SelectedValue = PXWeb.Settings.Current.Selection.ShowSelectionLimits.ToString();
            cboValuesetMustBeSelectedFirst.SelectedValue = PXWeb.Settings.Current.Selection.ValuesetMustBeSelectedFirst.ToString();
            cboShowAllAvailableValuesSearchButton.SelectedValue = PXWeb.Settings.Current.Selection.ShowAllAvailableValuesSearchButton.ToString();
            cboTitleFromMenu.SelectedValue = PXWeb.Settings.Current.Selection.TitleFromMenu.ToString();
            cboStandardApplicationHeadTitle.SelectedValue = PXWeb.Settings.Current.Selection.StandardApplicationHeadTitle.ToString();
            cboMetadataAsLinks.SelectedValue = PXWeb.Settings.Current.Selection.MetadataAsLinks.ToString();

            cboSelectValuesFromGroup.SelectedValue = PXWeb.Settings.Current.Selection.SelectValuesFromGroup.ToString();
            cboButtonsForContentVariable.SelectedValue = PXWeb.Settings.Current.Selection.ButtonsForContentVariable.ToString();
            cboDefaultSearch.SelectedValue = PXWeb.Settings.Current.Selection.SearchValuesBeginningOfWordCheckBoxDefaultChecked.ToString();
            cboPreSelectFirstContentAndTime.SelectedValue = PXWeb.Settings.Current.Selection.PreSelectFirstContentAndTime.ToString();

            if (PXWeb.Settings.Current.Selection.SearchButtonMode != PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode.ManyValues)
            {
                ShowMaxRowsWithoutSearch(false);
            }

            if (PXWeb.Settings.Current.Selection.Hierarchies.ShowHierarchies == false)
            {
                divHierarchicalLevelsOpen.Visible = false;
            }

            rptPresentationViews.DataSource = GetPresentationViewTable();
            rptPresentationViews.DataBind();

            rptOutputFormats.DataSource = GetFileFormatsTable();
            rptOutputFormats.DataBind();
        }


        /// <summary>
        /// Generate table with saved settings for presentation views
        /// </summary>
        private DataTable GetPresentationViewTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("PresentationView", typeof(System.String));
            tbl.Columns.Add("PresentationViewText", typeof(System.String));
            tbl.Columns.Add("PresentationViewSelect", typeof(System.Boolean));

            ////Plugins
            foreach (KeyValuePair<string, CommandBarPluginInfo> plugin in CommandBarPluginManager.Views.OrderBy(x => x.Value.SortOrder))
            {
                // Check if plugin should be displayed or not
                if (SettingsHelper.RemovePlugin(plugin.Value))
                {
                    continue; // Do not display this plugin
                }

                DataRow rw = tbl.NewRow();
                rw["PresentationView"] = plugin.Key;
                rw["PresentationViewText"] = Master.GetLocalizedString(plugin.Value.NameCode);
                rw["PresentationViewSelect"] = false;

                    /*foreach (string presentationView in PXWeb.Settings.Current.Selection.PresentationViews)
                    {
                        if (CommandBarPluginManager.Views.ContainsKey(presentationView))
                        {
                            if (CommandBarPluginManager.Views[presentationView].Name == plugin.Key)
                            {
                                rw["PresentationViewSelect"] = true;
                            }
                        }
                    }*/
                tbl.Rows.Add(rw);
            }

            return tbl;
        }

        /// <summary>
        /// Get table with available file formats
        /// </summary>
        /// <returns></returns>
        private DataTable GetFileFormatsTable()
        {
            DataTable tbl = new DataTable();

            tbl.Columns.Add("FileFormat", typeof(System.String));
            tbl.Columns.Add("FileFormatText", typeof(System.String));
            tbl.Columns.Add("FileFormatSelect", typeof(System.Boolean));

            //foreach (KeyValuePair<string, string> format in CommandBarPluginManager.FileFormats)
            //{
            //    // Check if file format should be displayed or not
            //    if (SettingsHelper.RemoveFileFormat(format.Key))
            //    {
            //        continue; // Do not display this plugin
            //    }

            //    DataRow rw = tbl.NewRow();
            //    rw["FileFormat"] = format.Key;
            //    rw["FileFormatText"] = this.Master.GetLocalizedString( format.Value);
            //    rw["FileFormatSelect"] = false;

            //    foreach (string fileFormat in PXWeb.Settings.Current.Selection.OutputFormats)
            //    {
            //        if (fileFormat == format.Key)
            //        {
            //            rw["FileFormatSelect"] = true;
            //        }
            //    }
            //    tbl.Rows.Add(rw);
            //}
            foreach (string format in CommandBarPluginManager.FileFormatsSorted)
            {
                // Check if file format should be displayed or not
                if (SettingsHelper.RemoveFileFormat(format))
                {
                    continue; // Do not display this plugin
                }

                DataRow rw = tbl.NewRow();
                rw["FileFormat"] = format;
                rw["FileFormatText"] = this.Master.GetLocalizedString(format);
                rw["FileFormatSelect"] = false;

               /* foreach (string fileFormat in PXWeb.Settings.Current.Selection.OutputFormats)
                {
                    if (fileFormat == format)
                    {
                        rw["FileFormatSelect"] = true;
                    }
                }*/
                tbl.Rows.Add(rw);
            }

            return tbl;
        }

        /// <summary>
        /// Save Selection settings
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
                        PXWeb.SelectionSettings sel = (PXWeb.SelectionSettings)PXWeb.Settings.NewSettings.Selection;
                        PXWeb.HierarchiesSettings hier = (PXWeb.HierarchiesSettings)PXWeb.Settings.NewSettings.Selection.Hierarchies;
                        PXWeb.MarkingTipsSettings mark = (PXWeb.MarkingTipsSettings)PXWeb.Settings.NewSettings.Selection.MarkingTips;

                        sel.ShowMandatoryMark = bool.Parse(cboShowMandatoryMark.SelectedValue);
                        sel.AllowAggregations = bool.Parse(cboAllowAggregations.SelectedValue);
                        hier.ShowHierarchies = bool.Parse(cboShowHierarchies.SelectedValue);
                        if (hier.ShowHierarchies == true)
                        {
                            hier.HierarchicalLevelsOpen = int.Parse(txtHierarchicalLevelsOpen.Text);
                        }
                        mark.ShowMarkingTips = bool.Parse(cboShowMarkingTips.SelectedValue);
                        sel.SearchButtonMode = (PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode)Enum.Parse(typeof(PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode), cboSearchButtonMode.SelectedValue.ToString());
                        sel.MaxRowsWithoutSearch = int.Parse(txtMaxRowsWithoutSearch.Text);
                        sel.AlwaysShowTimeVariableWithoutSearch = bool.Parse(cboAlwaysShowTimeVariableWithoutSearch.SelectedValue);
                        sel.ListSize = int.Parse(txtListSize.Text);
                        sel.ShowSelectionLimits = bool.Parse(cboShowSelectionLimits.SelectedValue);
                        sel.ValuesetMustBeSelectedFirst = bool.Parse(cboValuesetMustBeSelectedFirst.SelectedValue);
                        sel.ShowAllAvailableValuesSearchButton = bool.Parse(cboShowAllAvailableValuesSearchButton.SelectedValue);
                        sel.TitleFromMenu = bool.Parse(cboTitleFromMenu.SelectedValue);
                        sel.StandardApplicationHeadTitle = bool.Parse(cboStandardApplicationHeadTitle.SelectedValue);
                        sel.MetadataAsLinks = bool.Parse(cboMetadataAsLinks.SelectedValue);
                        sel.SelectValuesFromGroup = bool.Parse(cboSelectValuesFromGroup.SelectedValue);
                        sel.ButtonsForContentVariable = bool.Parse(cboButtonsForContentVariable.SelectedValue);
                        sel.SearchValuesBeginningOfWordCheckBoxDefaultChecked = bool.Parse(cboDefaultSearch.SelectedValue);
                        sel.PreSelectFirstContentAndTime = bool.Parse(cboPreSelectFirstContentAndTime.SelectedValue);
   //                     SetSelectedPresentationViews((List<String>)sel.PresentationViews);
   //                     SetSelectedOutputFormats((List<String>)sel.OutputFormats);

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
        /// Set selected presentation views
        /// </summary>
        /// <param name="lst">List containing selected presentation views</param>
        private void SetSelectedPresentationViews(List<string> lst)
        {
            lst.Clear();

            foreach (RepeaterItem itm in rptPresentationViews.Items)
            {
                CheckBox chk = (CheckBox)itm.FindControl("cbxSelect");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        HiddenField hid = (HiddenField)itm.FindControl("hidPresentationView");
                        if (hid != null)
                        {
                            lst.Add(hid.Value);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Set selected output formats
        /// </summary>
        /// <param name="lst">List containing selected output formats</param>
        private void SetSelectedOutputFormats(List<string> lst)
        {
            lst.Clear();

            // File formats
            foreach (RepeaterItem itm in rptOutputFormats.Items)
            {
                CheckBox chk = (CheckBox)itm.FindControl("chkOutputFormat");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        HiddenField hid = (HiddenField)itm.FindControl("hidOutputFormat");
                        if (hid != null)
                        {
                            lst.Add(hid.Value);
                        }
                    }
                }
            }
        }

        protected void cboShowHierarchies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bool.Parse(cboShowHierarchies.SelectedValue) == true)
            {
                divHierarchicalLevelsOpen.Visible = true;
            }
            else
            {
                divHierarchicalLevelsOpen.Visible = false;
            }
        }


        /// <summary>
        /// Validates that a entered value is a integer
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateMandatoryInteger(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            int value;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }

            if (!int.TryParse(args.Value, out value))
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationIntegerValue");
                return;
            }

            if (value < 1)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationGreaterThanZero");
                return;
            }

            args.IsValid = true;
            return;
        }

        public void ValidateMarkingTipsURL(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }

            args.IsValid = true;
            return;
        }

        /// <summary>
        /// SearchButtonMode has been changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButtonMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSearchButtonMode.SelectedValue != "ManyValues")
            {
                ShowMaxRowsWithoutSearch(false);
            }
            else
            {
                ShowMaxRowsWithoutSearch(true);
            }
        }

        /// <summary>
        /// Show/hide input field for "Max rows without search"
        /// </summary>
        /// <param name="show"></param>
        private void ShowMaxRowsWithoutSearch(bool show)
        {
            txtMaxRowsWithoutSearch.Visible = show;
            lblMaxRowsWithoutSearch.Visible = show;
            imgMaxRowsWithoutSearch.Visible = show;
            validatorMaxRowsWithoutSearch.Visible = show;
        }

        protected void ShowMandatoryMarkInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionShowMandatoryMark", "PxWebAdminSettingsSelectionShowMandatoryMarkInfo");
        }
        protected void AllowAggregationsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionAllowAggregations", "PxWebAdminSettingsSelectionAllowAggregationsInfo");
        }
        protected void ShowHierarchiesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionShowHierarchies", "PxWebAdminSettingsSelectionShowHierarchiesInfo");
        }
        protected void HierarchicalLevelsOpenInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionHierarchicalLevelsOpen", "PxWebAdminSettingsSelectionHierarchicalLevelsOpenInfo");
        }
        protected void ShowMarkingTipsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionShowMarkingTips", "PxWebAdminSettingsSelectionShowMarkingTipsInfo");
        }
        protected void MarkingTipsURLInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionMarkingTipsURL", "PxWebAdminSettingsSelectionMarkingTipsURLInfo");
        }
        protected void MaxRowsWithoutSearchInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionMaxRowsWithoutSearch", "PxWebAdminSettingsSelectionMaxRowsWithoutSearchInfo");
        }
        protected void AlwaysShowTimeVariableWithoutSearchInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionAlwaysShowTimeVariableWithoutSearch", "PxWebAdminSettingsSelectionAlwaysShowTimeVariableWithoutSearchInfo");
        }
        protected void ListSizeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionListSize", "PxWebAdminSettingsSelectionListSizeInfo");
        }
        protected void ShowSelectionLimitsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionShowSelectionLimits", "PxWebAdminSettingsSelectionShowSelectionLimitsInfo");
        }
        protected void OutputFormatsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionOutputFormats", "PxWebAdminSettingsSelectionOutputFormatsInfo");
        }
        protected void ValuesetMustBeSelectedFirstInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionValuesetMustBeSelectedFirst", "PxWebAdminSettingsSelectionValuesetMustBeSelectedFirstInfo");
        }
        protected void ShowAllAvailableValuesSearchButtonInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionShowAllAvailableValuesSearchButton", "PxWebAdminSettingsSelectionShowAllAvailableValuesSearchButtonInfo");
        }
        protected void SearchButtonModeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionSearchButtonMode", "PxWebAdminSettingsSelectionSearchButtonModeInfo");
        }
        protected void TitleFromMenuInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionTitleFromMenu", "PxWebAdminSettingsSelectionTitleFromMenuInfo");
        }

        protected void StandardApplicationHeadTitleInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionStandardApplicationHeadTitle", "PxWebAdminSettingsSelectionStandardApplicationHeadTitleInfo");
        }
        protected void MetadataAsLinksInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionMetadataAsLinks", "PxWebAdminSettingsSelectionMetadataAsLinksInfo");
        }
        protected void SelectValuesFromGroupInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionShowSelectValuesFromGroup", "PxWebAdminSettingsSelectionSelectValuesFromGroupInfo");
        }
        protected void ButtonsForContentVariableInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionButtonsForContentVariable", "PxWebAdminSettingsSelectionButtonsForContentVariableInfo");
        }
        protected void DefaultSearchInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionDefaultSearch", "PxWebAdminSettingsSelectionDefaultSearchInfo");
        }
        protected void PreSelectFirstContentAndTimeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSelectionPreSelectFirstContentAndTime", "PxWebAdminSettingsSelectionPreSelectFirstContentAndTimeInfo");
        }
    }
}
