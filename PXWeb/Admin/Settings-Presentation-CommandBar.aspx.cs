using PCAxis.Web.Controls.CommandBar.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PXWeb.Admin
{
    public partial class Settings_Presentation_CommandBar : System.Web.UI.Page
    {
        private DataTable _settingsTable;
        private DataTable _fileFormatsTable;
        private DataTable _presentationViewsTable;
        private PCAxis.Web.Controls.CommandBar.CommandBarViewMode _viewMode;
        private Boolean _showDropDownModeControl;

        protected Boolean ShowDropDownModeControl
        {
            get
            {
                return _showDropDownModeControl;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));
            if (IsPostBack)
            {
                _viewMode = (PCAxis.Web.Controls.CommandBar.CommandBarViewMode)Enum.Parse(typeof(PCAxis.Web.Controls.CommandBar.CommandBarViewMode), lstViewMode.SelectedValue.ToString());
            }
            else
            {
                lstViewMode.Items.Add(new ListItem(Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarViewModeDropDown"), PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown.ToString()));
                lstViewMode.Items.Add(new ListItem(Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarViewModeImageButtons"), PCAxis.Web.Controls.CommandBar.CommandBarViewMode.ImageButtons.ToString()));
                lstViewMode.Items.Add(new ListItem(Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarViewModeHidden"), PCAxis.Web.Controls.CommandBar.CommandBarViewMode.Hidden.ToString()));
                _viewMode = PXWeb.Settings.Current.Presentation.CommandBar.ViewMode;
                DisplaySavedSettings();
            }

        }

        /// <summary>
        /// Set headers for repeaters and other languagedepending texts
        /// Databind repeater presenting the saved settings for operations
        /// Databind repeater presenting the saved settings for output formats
        /// </summary>
        private void DisplaySavedSettings()
        {
            pnlContent.Visible = true;
            lstViewMode.SelectedValue = _viewMode.ToString();

            InitializeSettingsTable();
            InitializePresentationViewTable();

            switch (_viewMode)
            {
                case PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown:
                    _showDropDownModeControl = true;
                    InitializeFileFormatsTableDropDown();
                    break;
                case PCAxis.Web.Controls.CommandBar.CommandBarViewMode.ImageButtons:
                    _showDropDownModeControl = false;
                    InitializeFileFormatsTableImageButtons();
                    break;
                case PCAxis.Web.Controls.CommandBar.CommandBarViewMode.Hidden:
                    pnlContent.Visible = false;
                    break;
                default:
                    break;
            }

            rptSettings.DataSource = _settingsTable;
            rptSettings.DataBind();

            rptFileFormats.DataSource = _fileFormatsTable;
            rptFileFormats.DataBind();

            rptPresentationViews.DataSource = _presentationViewsTable;
            rptPresentationViews.DataBind();

            ShowHideDropDownModeControls(_showDropDownModeControl);
            SetLocalizedTexts();
        }

        /// <summary>
        /// Set localized texts
        /// </summary>
        private void SetLocalizedTexts()
        {
            lbViewMode.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarViewModeText");
            lbHeaderSettingText.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderSetting");
            lbHeaderOperationShortcut.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderOperationShortcut");
            lbHeaderFileFormatText.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderFileFormat");
            lbFileFormatShortcut.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderFileFormatShortcut");
            lbHeaderPresentationViewText.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderPresentationView");

            if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
            {
                lbHeaderOperationSelect.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderDropDown");
                lbFileFormatSelect.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderFileFormatDropDown");
                lbHeaderPresentationViewSelect.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderDropDown");
                lbHeaderPresentationViewShortcut.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderOperationShortcut");
                lbHeaderCommandbarShortcut.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderCommandbarShortcut");
            }
            else
            {
                lbHeaderOperationSelect.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderButton");
                lbFileFormatSelect.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderButton");
                lbHeaderPresentationViewSelect.Text = Master.GetLocalizedString("PxWebAdminSettingsPresentationCommandBarHeaderButton");
            }
        }

        /// <summary>
        /// Set visibility of controls exclusive for the dropdown mode
        /// </summary>
        /// <param name="visible">true if the controls exclusive for the dropdown mode shall be shown</param>
        private void ShowHideDropDownModeControls(Boolean visible)
        {
            lbHeaderOperationShortcut.Visible = visible;
            lbHeaderCommandbarShortcut.Visible = visible;
            lbFileFormatShortcut.Visible = visible;
            lbHeaderPresentationViewShortcut.Visible = visible;
        }

        /// <summary>
        /// Generate table with saved settings for operations
        /// </summary>
        private void InitializeSettingsTable()
        {
            _settingsTable = new DataTable();
            _settingsTable.Columns.Add("Setting", typeof(System.String));
            _settingsTable.Columns.Add("SettingText", typeof(System.String));
            _settingsTable.Columns.Add("OperationSelect", typeof(System.Boolean));
            _settingsTable.Columns.Add("OperationShortcut", typeof(System.Boolean));

            ////Plugins
            foreach (KeyValuePair<string, CommandBarPluginInfo> plugin in CommandBarPluginManager.Operations.OrderBy(x => x.Value.SortOrder))
            {
                // Check if plugin should be displayed or not
                if (SettingsHelper.RemovePlugin(plugin.Value))
                {
                    continue; // Do not display this plugin
                }

                DataRow rw = _settingsTable.NewRow();
                rw["Setting"] = plugin.Key;
                rw["SettingText"] = Master.GetLocalizedString(plugin.Value.NameCode);
                rw["OperationSelect"] = false;
                rw["OperationShortcut"] = false;

                //CommandBar.Operations used for dropdown and the dropdown mode specifics
                if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
                {
                    foreach (string operation in PXWeb.Settings.Current.Presentation.CommandBar.Operations)
                    {
                        if (CommandBarPluginManager.Operations.ContainsKey(operation))
                        {
                            if (CommandBarPluginManager.Operations[operation].Name == plugin.Key)
                            {
                                rw["OperationSelect"] = true;
                            }
                        }
                    }
                    foreach (string operationShortcut in PXWeb.Settings.Current.Presentation.CommandBar.OperationShortcuts)
                    {
                        if (CommandBarPluginManager.Operations.ContainsKey(operationShortcut))
                        {
                            if (CommandBarPluginManager.Operations[operationShortcut].Name == plugin.Key)
                            {
                                rw["OperationShortcut"] = true;
                            }
                        }
                    }
                }
                else //CommandBar.OperationButtons used for buttons
                {
                    foreach (string operation in PXWeb.Settings.Current.Presentation.CommandBar.OperationButtons)
                    {
                        if (CommandBarPluginManager.Operations.ContainsKey(operation))
                        {
                            if (CommandBarPluginManager.Operations[operation].Name == plugin.Key)
                            {
                                rw["OperationSelect"] = true;
                            }
                        }
                    }
                }
                _settingsTable.Rows.Add(rw);
            }
        }


        /// <summary>
        /// Generate table with saved settings for outputformats when ViewMode = DropDown
        /// </summary>
        private void InitializeFileFormatsTableDropDown()
        {
            _fileFormatsTable = new DataTable();
            _fileFormatsTable.Columns.Add("OutputFormat", typeof(System.String));
            _fileFormatsTable.Columns.Add("OutputFormatText", typeof(System.String));
            _fileFormatsTable.Columns.Add("OutputFormatSelect", typeof(System.Boolean));
            _fileFormatsTable.Columns.Add("OutputFormatShortcut", typeof(System.Boolean));

            //PCAxis.Web.Controls.FileGenerator fileGenerator = new PCAxis.Web.Controls.FileGenerator(Master.SelectedCulture());
            //foreach (KeyValuePair<string, string> fileFormat in CommandBarPluginManager.FileFormats)
            //{
            //    // Check if file format should be displayed or not
            //    if (SettingsHelper.RemoveFileFormat(fileFormat.Key))
            //    {
            //        continue; // Do not display this plugin
            //    }

            //    DataRow rw = _fileFormatsTable.NewRow();
            //    rw["OutputFormat"] = fileFormat.Key;// outputFormat;
            //    rw["OutputFormatText"] = this.Master.GetLocalizedString(fileFormat.Value); //outputFormat;
            //    rw["OutputFormatSelect"] = false;
            //    rw["OutputFormatShortcut"] = false;

            //    if (((List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OutputFormats).Contains(fileFormat.Key))
            //    {
            //        rw["OutputFormatSelect"] = true;
            //    }
            //    if (((List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OutputFormatShortcuts).Contains(fileFormat.Key))
            //    {
            //        rw["OutputFormatShortcut"] = true;
            //    }

            //    _fileFormatsTable.Rows.Add(rw);
            //}
            foreach (string fileFormat in CommandBarPluginManager.FileFormatsSorted)
            {
                // Check if file format should be displayed or not
                if (SettingsHelper.RemoveFileFormat(fileFormat))
                {
                    continue; // Do not display this plugin
                }

                DataRow rw = _fileFormatsTable.NewRow();
                rw["OutputFormat"] = fileFormat;// outputFormat;
                rw["OutputFormatText"] = this.Master.GetLocalizedString(fileFormat); //outputFormat;
                rw["OutputFormatSelect"] = false;
                rw["OutputFormatShortcut"] = false;

                if (((List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OutputFormats).Contains(fileFormat))
                {
                    rw["OutputFormatSelect"] = true;
                }
                if (((List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OutputFormatShortcuts).Contains(fileFormat))
                {
                    rw["OutputFormatShortcut"] = true;
                }

                _fileFormatsTable.Rows.Add(rw);
            }
        }

        /// <summary>
        /// Generate table with saved settings for outputformats when ViewMode = ImageButtons
        /// </summary>
        private void InitializeFileFormatsTableImageButtons()
        {
            _fileFormatsTable = new DataTable();
            _fileFormatsTable.Columns.Add("OutputFormat", typeof(System.String));
            _fileFormatsTable.Columns.Add("OutputFormatText", typeof(System.String));
            _fileFormatsTable.Columns.Add("OutputFormatSelect", typeof(System.Boolean));
            _fileFormatsTable.Columns.Add("OutputFormatShortcut", typeof(System.Boolean));

            PCAxis.Web.Core.FileType fileType;

            //PCAxis.Web.Controls.FileGenerator fileGenerator = new PCAxis.Web.Controls.FileGenerator(Master.SelectedCulture());
            foreach (KeyValuePair<string, PCAxis.Web.Core.FileType> kvp in CommandBarPluginManager.FileTypes)
            {
                // Check if file type should be displayed or not
                if (SettingsHelper.RemoveFileType(kvp.Value))
                {
                    continue; // Do not display this plugin
                }

                DataRow rw = _fileFormatsTable.NewRow();
                rw["OutputFormat"] = kvp.Key;// outputFormat;

                fileType = kvp.Value;

                rw["OutputFormatText"] = this.Master.GetLocalizedString(fileType.TranslatedText); //outputFormat;
                rw["OutputFormatSelect"] = false;
                rw["OutputFormatShortcut"] = false;

                if (((List<string>)PXWeb.Settings.Current.Presentation.CommandBar.FileTypeButtons).Contains(kvp.Key))
                {
                    rw["OutputFormatSelect"] = true;
                }

                _fileFormatsTable.Rows.Add(rw);
            }
        }

        /// <summary>
        /// Generate table with saved settings for presentation views
        /// </summary>
        private void InitializePresentationViewTable()
        {
            _presentationViewsTable = new DataTable();
            _presentationViewsTable.Columns.Add("PresentationView", typeof(System.String));
            _presentationViewsTable.Columns.Add("PresentationViewText", typeof(System.String));
            _presentationViewsTable.Columns.Add("PresentationViewSelect", typeof(System.Boolean));
            _presentationViewsTable.Columns.Add("PresentationViewShortcut", typeof(System.Boolean));
            _presentationViewsTable.Columns.Add("CommandbarShortcut", typeof(System.Boolean));

            ////Plugins
            foreach (KeyValuePair<string, CommandBarPluginInfo> plugin in CommandBarPluginManager.Views.OrderBy(x => x.Value.SortOrder))
            {
                // Check if plugin should be displayed or not
                if (SettingsHelper.RemovePlugin(plugin.Value))
                {
                    continue; // Do not display this plugin
                }

                DataRow rw = _presentationViewsTable.NewRow();
                rw["PresentationView"] = plugin.Key;
                rw["PresentationViewText"] = Master.GetLocalizedString(plugin.Value.NameCode);
                rw["PresentationViewSelect"] = false;
                rw["PresentationViewShortcut"] = false;
                rw["CommandbarShortcut"] = false;

                //CommandBar.Operations used for dropdown and the dropdown mode specifics
                if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
                {
                    foreach (string presentationView in PXWeb.Settings.Current.Presentation.CommandBar.PresentationViews)
                    {
                        if (CommandBarPluginManager.Views.ContainsKey(presentationView))
                        {
                            if (CommandBarPluginManager.Views[presentationView].Name == plugin.Key)
                            {
                                rw["PresentationViewSelect"] = true;
                            }
                        }
                    }
                    foreach (string presentationViewShortcut in PXWeb.Settings.Current.Presentation.CommandBar.PresentationViewShortcuts)
                    {
                        if (CommandBarPluginManager.Views.ContainsKey(presentationViewShortcut))
                        {
                            if (CommandBarPluginManager.Views[presentationViewShortcut].Name == plugin.Key)
                            {
                                rw["PresentationViewShortcut"] = true;
                            }
                        }
                    }
                    foreach (string commandbarShortcut in PXWeb.Settings.Current.Presentation.CommandBar.CommandBarShortcuts)
                    {
                        if (CommandBarPluginManager.Views.ContainsKey(commandbarShortcut))
                        {
                            if (CommandBarPluginManager.Views[commandbarShortcut].Name == plugin.Key)
                            {
                                rw["CommandbarShortcut"] = true;
                            }
                        }
                    }
                }
                else //CommandBar.PresentationViewButtons used for buttons
                {
                    foreach (string presentationView in PXWeb.Settings.Current.Presentation.CommandBar.PresentationViewButtons)
                    {
                        if (CommandBarPluginManager.Views.ContainsKey(presentationView))
                        {
                            if (CommandBarPluginManager.Views[presentationView].Name == plugin.Key)
                            {
                                rw["PresentationViewSelect"] = true;
                            }
                        }
                    }
                }
                _presentationViewsTable.Rows.Add(rw);
            }
        }


        /// <summary>
        /// Save ViewMode setting
        /// Read checkboxes and save the settings
        /// </summary>
        private void SaveSettings()
        {
            if (PXWeb.Settings.BeginUpdate())
            {
                try
                {
                    CommandBarSettings cmd = (CommandBarSettings)PXWeb.Settings.NewSettings.Presentation.CommandBar;
                    _viewMode = (PCAxis.Web.Controls.CommandBar.CommandBarViewMode)Enum.Parse(typeof(PCAxis.Web.Controls.CommandBar.CommandBarViewMode), lstViewMode.SelectedValue.ToString());
                    cmd.ViewMode = _viewMode;

                    //Dropdown mode
                    if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
                    {
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.Operations).Clear();
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.OperationShortcuts).Clear();
                    }
                    else //Button mode
                    {
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.OperationButtons).Clear();
                    }

                    CheckBox cbx;
                    foreach (RepeaterItem itm in rptSettings.Items)
                    {
                        if ((itm.ItemType == ListItemType.Item) || (itm.ItemType == ListItemType.AlternatingItem))
                        {
                            HiddenField hidSetting = (HiddenField)itm.FindControl("hidSetting");

                            //Saved if viewmode is DropDown
                            if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
                            {
                                //Operations
                                cbx = (CheckBox)itm.FindControl("cbxOperationSelect");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.Operations).Add(hidSetting.Value);
                                }
                                //OperationShortcuts
                                cbx = (CheckBox)itm.FindControl("cbxOperationShortcut");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.OperationShortcuts).Add(hidSetting.Value);
                                }
                            }
                            else //Saved if viewmode is Buttons
                            {
                                //CommandBar.OperationButtons
                                cbx = (CheckBox)itm.FindControl("cbxOperationSelect");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.OperationButtons).Add(hidSetting.Value);
                                }

                            }
                        }
                    }
                    PXWeb.Settings.Save();
                }
                finally
                {
                    PXWeb.Settings.EndUpdate();
                }
            }
        }


        /// <summary>
        /// Read checkboxes and save the fileformats
        /// </summary>
        private void SaveFileFormats()
        {
            if (PXWeb.Settings.BeginUpdate())
            {
                try
                {
                    //Dropdown mode
                    if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
                    {
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.OutputFormats).Clear();
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.OutputFormatShortcuts).Clear();
                    }
                    else //Button mode
                    {
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.FileTypeButtons).Clear();
                    }

                    foreach (RepeaterItem itm in rptFileFormats.Items)
                    {
                        if ((itm.ItemType == ListItemType.Item) || (itm.ItemType == ListItemType.AlternatingItem))
                        {
                            HiddenField hidSetting = (HiddenField)itm.FindControl("hidFileFormat");

                            CheckBox cbx;
                            //CommandBar.OutputFormats used for dropdown
                            if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
                            {
                                cbx = (CheckBox)itm.FindControl("cbxFileFormatSelect");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.OutputFormats).Add(hidSetting.Value);
                                }
                                cbx = (CheckBox)itm.FindControl("cbxFileFormatShortcut");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.OutputFormatShortcuts).Add(hidSetting.Value);
                                }
                            }
                            else //CommandBar.FileTypeButtons used for buttons
                            {
                                cbx = (CheckBox)itm.FindControl("cbxFileFormatSelect");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.FileTypeButtons).Add(hidSetting.Value);
                                }
                            }
                        }
                    }
                    PXWeb.Settings.Save();
                }
                finally
                {
                    PXWeb.Settings.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Read checkboxes and save the presentation views
        /// </summary>
        private void SavePresentationViews()
        {
            if (PXWeb.Settings.BeginUpdate())
            {
                try
                {
                    //Dropdown mode
                    if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
                    {
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.PresentationViews).Clear();
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.PresentationViewShortcuts).Clear();
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.CommandBarShortcuts).Clear();
                    }
                    else //Button mode
                    {
                        ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.PresentationViewButtons).Clear();
                    }

                    CheckBox cbx;
                    foreach (RepeaterItem itm in rptPresentationViews.Items)
                    {
                        if ((itm.ItemType == ListItemType.Item) || (itm.ItemType == ListItemType.AlternatingItem))
                        {
                            HiddenField hidPresentationView = (HiddenField)itm.FindControl("hidPresentationView");

                            //Saved if viewmode is DropDown
                            if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
                            {
                                //Presentation views
                                cbx = (CheckBox)itm.FindControl("cbxSelect");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.PresentationViews).Add(hidPresentationView.Value);
                                }
                                //Presentation view shortcuts
                                cbx = (CheckBox)itm.FindControl("cbxShortcut");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.PresentationViewShortcuts).Add(hidPresentationView.Value);
                                }

                                //Commandbar shortcuts
                                cbx = (CheckBox)itm.FindControl("cbxCommandbarShortcut");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.CommandBarShortcuts).Add(hidPresentationView.Value);
                                }
                            }
                            else //Saved if viewmode is Buttons
                            {
                                //CommandBar.PresentationViewButtons
                                cbx = (CheckBox)itm.FindControl("cbxSelect");
                                if (cbx.Checked)
                                {
                                    ((List<string>)PXWeb.Settings.NewSettings.Presentation.CommandBar.PresentationViewButtons).Add(hidPresentationView.Value);
                                }

                            }
                        }
                    }
                    PXWeb.Settings.Save();
                }
                finally
                {
                    PXWeb.Settings.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Save ViewMode setting
        /// Save operation and outputformat settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {
            SaveSettings();
            SaveFileFormats();
            SavePresentationViews();
        }

        /// <summary>
        /// Change viewmode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewMode = (PCAxis.Web.Controls.CommandBar.CommandBarViewMode)Enum.Parse(typeof(PCAxis.Web.Controls.CommandBar.CommandBarViewMode), lstViewMode.SelectedValue.ToString());
            DisplaySavedSettings();
        }

        /// <summary>
        /// Display information about setting category
        /// Different depending on ViewMode because controls are reused
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgOperations_Click(object sender, ImageClickEventArgs e)
        {
            if (_viewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown)
            {
                Master.ShowInfoDialog("PxWebAdminSettingsPresentationCommandBarHeaderOperations", "PxWebAdminSettingsPresentationCommandBarHeaderOperationsInfo");
            }
            else
            {
                Master.ShowInfoDialog("PxWebAdminSettingsPresentationCommandBarHeaderButton", "PxWebAdminSettingsPresentationCommandBarHeaderButtonInfo");
            }
        }


        /// <summary>
        /// Display information about setting category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgHeaderFileFormatText_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationCommandBarHeaderFileformats", "PxWebAdminSettingsPresentationCommandBarHeaderFileformatsInfo");
        }

        /// <summary>
        /// Display information about presentation views
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgPresentationViews_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationCommandBarHeaderPresentationViews", "PxWebAdminSettingsPresentationCommandBarHeaderPresentationViewsInfo");
        }

    }
}
