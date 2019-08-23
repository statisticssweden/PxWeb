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
    public partial class Settings_Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                ReadSettings();
                cboMenuMode.SelectedValue = PXWeb.Settings.Current.Menu.MenuMode.ToString();
                SetSelectedPanel();
            }
        }

        /// <summary>
        /// Read and display Menu settings  
        /// </summary>
        private void ReadSettings()
        {
            cboTree1ShowRoot.SelectedValue = PXWeb.Settings.Current.Menu.ShowRoot.ToString();
            cboTree1ExpandAll.SelectedValue = PXWeb.Settings.Current.Menu.ExpandAll.ToString();
            //cboTree1SortByAlias.SelectedValue = PXWeb.Settings.Current.Menu.SortByAlias.ToString();
            cboTree1ShowSelectionLink.SelectedValue = PXWeb.Settings.Current.Menu.ShowSelectLink.ToString();
            cboTree1ShowDownloadLink.SelectedValue = PXWeb.Settings.Current.Menu.ShowDownloadLink.ToString();
            cboTree1ViewLinkMode.SelectedValue = PXWeb.Settings.Current.Menu.ViewLinkMode.ToString();
            txtTree1NumberOfValuesInDefaultView.Text = PXWeb.Settings.Current.Menu.NumberOfValuesInDefaultView.ToString();
            cboTree1ShowModifiedDate.SelectedValue = PXWeb.Settings.Current.Menu.ShowModifiedDate.ToString();
            cboTree1ShowLastUpdated.SelectedValue = PXWeb.Settings.Current.Menu.ShowLastUpdated.ToString();
            cboTree1ShowFileSize.SelectedValue = PXWeb.Settings.Current.Menu.ShowFileSize.ToString();
            cboTree1ShowVariablesAndValues.SelectedValue = PXWeb.Settings.Current.Menu.ShowVariablesAndValues.ToString();

            //cboListSortByAlias.SelectedValue = PXWeb.Settings.Current.Menu.SortByAlias.ToString();
            //cboListShowSelectionLink.SelectedValue = PXWeb.Settings.Current.Menu.ShowSelectLink.ToString();
            //cboListShowDownloadLink.SelectedValue = PXWeb.Settings.Current.Menu.ShowDownloadLink.ToString();
            //cboListViewLinkMode.SelectedValue = PXWeb.Settings.Current.Menu.ViewLinkMode.ToString();
            //txtListNumberOfValuesInDefaultView.Text = PXWeb.Settings.Current.Menu.NumberOfValuesInDefaultView.ToString();
            //cboListShowModifiedDate.SelectedValue = PXWeb.Settings.Current.Menu.ShowModifiedDate.ToString();
            //cboListShowLastUpdated.SelectedValue = PXWeb.Settings.Current.Menu.ShowLastUpdated.ToString();
            //cboListShowFileSize.SelectedValue = PXWeb.Settings.Current.Menu.ShowFileSize.ToString();
            //cboListShowVariablesAndValues.SelectedValue = PXWeb.Settings.Current.Menu.ShowVariablesAndValues.ToString();

            cboTree2ShowRoot.SelectedValue = PXWeb.Settings.Current.Menu.ShowRoot.ToString();
            cboTree2ExpandAll.SelectedValue = PXWeb.Settings.Current.Menu.ExpandAll.ToString();
            //cboTree2SortByAlias.SelectedValue = PXWeb.Settings.Current.Menu.SortByAlias.ToString();
            cboTree2ShowModifiedDate.SelectedValue = PXWeb.Settings.Current.Menu.ShowModifiedDate.ToString();
            cboTree2ShowLastUpdated.SelectedValue = PXWeb.Settings.Current.Menu.ShowLastUpdated.ToString();
            cboTree2ShowFileSize.SelectedValue = PXWeb.Settings.Current.Menu.ShowFileSize.ToString();
            cboTree2ShowTableCategory.SelectedValue = PXWeb.Settings.Current.Menu.ShowTableCategory.ToString();
            cboTree2ShowTableUpdatedAfterPublish.SelectedValue = PXWeb.Settings.Current.Menu.ShowTableUpdatedAfterPublish.ToString();
            cboTree2MetadataAsIcons.SelectedValue = PXWeb.Settings.Current.Menu.MetadataAsIcons.ToString();
            cboShowTextToMetadata.SelectedValue = PXWeb.Settings.Current.Menu.ShowTextToMetadata.ToString();
            cboShowMenuExplanation.SelectedValue = PXWeb.Settings.Current.Menu.ShowMenuExplanation.ToString();
            
            if (PXWeb.Settings.Current.Menu.ViewLinkMode != MenuViewLinkModeType.DefaultValues)
            {
                //lblListNumberOfValuesInDefaultView.Visible = false;
                //txtListNumberOfValuesInDefaultView.Visible = false;
                //imgListNumberOfValuesInDefaultView.Visible = false;
                //validatorListNumberOfValuesInDefaultView.Visible = false;

                lblTree1NumberOfValuesInDefaultView.Visible = false;
                txtTree1NumberOfValuesInDefaultView.Visible = false;
                imgTree1NumberOfValuesInDefaultView.Visible = false;
                validatorTree1NumberOfValuesInDefaultView.Visible = false;
            }
        }

        /// <summary>
        /// Depending on selected Menu mode - display the right panel
        /// </summary>
        private void SetSelectedPanel()
        {

            //pnlList.Visible = false;
            pnlTreeViewWithFiles.Visible = false;
            pnlWithoutAndFiles.Visible = false;

            switch (cboMenuMode.SelectedValue)
            {
                case "List":
                    //pnlList.Visible = true;
                    break;
                case "TreeViewWithoutFiles":
                case "TreeViewAndFiles":
                    pnlWithoutAndFiles.Visible = true;
                    break;
                case "TreeViewWithFiles":
                    pnlTreeViewWithFiles.Visible = true;
                    break;
                default:
                    break;
            }

            pnlShowTextForMetadata.Visible = false;

            if (cboTree2MetadataAsIcons.SelectedValue == "False")
            {
                pnlShowTextForMetadata.Visible = true;   
            }
        }
        

        /// <summary>
        /// Save Menu settings
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
                        PXWeb.MenuSettings menu = (PXWeb.MenuSettings)PXWeb.Settings.NewSettings.Menu;
                        switch (cboMenuMode.SelectedValue)
                        {
                            case "List":
                                menu.MenuMode = MenuModeType.List;
                                break;
                            case "TreeViewWithoutFiles":
                                menu.MenuMode = MenuModeType.TreeViewWithoutFiles;
                                break;
                            case "TreeViewWithFiles":
                                menu.MenuMode = MenuModeType.TreeViewWithFiles;
                                break;
                            case "TreeViewAndFiles":
                                // TreeViewAndFiles is not implemented in first version
                                //menu.MenuMode = MenuModeType.TreeViewAndFiles; 
                                menu.MenuMode = MenuModeType.TreeViewWithoutFiles;
                                break;
                            default:
                                menu.MenuMode = MenuModeType.TreeViewWithoutFiles;
                                break;
                        }

                        menu.SortByAlias = true; // Functionality to turn on/off SortByAlias is not implemented in first version

                        switch (menu.MenuMode)
                        {
                            case MenuModeType.List:
                                //menu.ShowSelectLink = bool.Parse(cboListShowSelectionLink.SelectedValue);
                                //switch (cboListShowDownloadLink.SelectedValue)
                                //{
                                //    case "AlwaysHide":
                                //        menu.ShowDownloadLink = PCAxis.Web.Controls.DownloadLinkVisibilityType.AlwaysHide;
                                //        break;
                                //    case "AlwaysShow":
                                //        menu.ShowDownloadLink = PCAxis.Web.Controls.DownloadLinkVisibilityType.AlwaysShow;
                                //        break;
                                //    default:
                                //        menu.ShowDownloadLink = PCAxis.Web.Controls.DownloadLinkVisibilityType.ShowIfSmallFile;
                                //        break;
                                //}
                                //menu.ViewLinkMode = GetViewLinkMode(cboListViewLinkMode.SelectedValue);
                                //if (menu.ViewLinkMode == MenuViewLinkModeType.DefaultValues)
                                //{
                                //    menu.NumberOfValuesInDefaultView = int.Parse(txtListNumberOfValuesInDefaultView.Text);
                                //}
                                //menu.ShowModifiedDate = bool.Parse(cboListShowModifiedDate.SelectedValue);
                                //menu.ShowLastUpdated = bool.Parse(cboListShowLastUpdated.SelectedValue);
                                //menu.ShowFileSize = bool.Parse(cboListShowFileSize.SelectedValue);
                                //menu.ShowVariablesAndValues = bool.Parse(cboListShowVariablesAndValues.SelectedValue);
                                break;
                            case MenuModeType.TreeViewWithoutFiles:
                            case MenuModeType.TreeViewAndFiles:
                                menu.ShowRoot = bool.Parse(cboTree1ShowRoot.SelectedValue);
                                menu.ExpandAll = bool.Parse(cboTree1ExpandAll.SelectedValue);
                                menu.ShowSelectLink = bool.Parse(cboTree1ShowSelectionLink.SelectedValue);
                                switch (cboTree1ShowDownloadLink.SelectedValue)
                                {
                                    case "AlwaysHide":
                                        menu.ShowDownloadLink = PCAxis.Web.Controls.DownloadLinkVisibilityType.AlwaysHide;
                                        break;
                                    case "AlwaysShow":
                                        menu.ShowDownloadLink = PCAxis.Web.Controls.DownloadLinkVisibilityType.AlwaysShow;
                                        break;
                                    default:
                                        menu.ShowDownloadLink = PCAxis.Web.Controls.DownloadLinkVisibilityType.ShowIfSmallFile;
                                        break;
                                }
                                menu.ViewLinkMode = GetViewLinkMode(cboTree1ViewLinkMode.SelectedValue);
                                if (menu.ViewLinkMode == MenuViewLinkModeType.DefaultValues)
                                {
                                    menu.NumberOfValuesInDefaultView = int.Parse(txtTree1NumberOfValuesInDefaultView.Text);
                                }
                                menu.ShowModifiedDate = bool.Parse(cboTree1ShowModifiedDate.SelectedValue);
                                menu.ShowLastUpdated = bool.Parse(cboTree1ShowLastUpdated.SelectedValue);
                                menu.ShowFileSize = bool.Parse(cboTree1ShowFileSize.SelectedValue);
                                menu.ShowVariablesAndValues = bool.Parse(cboTree1ShowVariablesAndValues.SelectedValue);
                                break;
                            case MenuModeType.TreeViewWithFiles:
                                menu.ShowRoot = bool.Parse(cboTree2ShowRoot.SelectedValue);
                                menu.ExpandAll = bool.Parse(cboTree2ExpandAll.SelectedValue);
                                menu.ShowModifiedDate = bool.Parse(cboTree2ShowModifiedDate.SelectedValue);
                                menu.ShowLastUpdated = bool.Parse(cboTree2ShowLastUpdated.SelectedValue);
                                menu.ShowFileSize = bool.Parse(cboTree2ShowFileSize.SelectedValue);
                                menu.ShowTableCategory = bool.Parse(cboTree2ShowTableCategory.SelectedValue);
                                menu.ShowTableUpdatedAfterPublish = bool.Parse(cboTree2ShowTableUpdatedAfterPublish.SelectedValue);
                                menu.MetadataAsIcons = bool.Parse(cboTree2MetadataAsIcons.SelectedValue);
                                menu.ShowTextToMetadata = bool.Parse(cboShowTextToMetadata.SelectedValue);
                                menu.ShowMenuExplanation = bool.Parse(cboShowMenuExplanation.SelectedValue);
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
        /// Validate number of values in default view
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateNumberOfValuesInDefaultView(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
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

        ///// <summary>
        ///// Verifies that the entered settings have valid values
        ///// </summary>
        ///// <returns>True if all setiings are ok, else false</returns>
        //private bool VerifySettings()
        //{
        //    int i;

        //    switch (cboMenuMode.SelectedValue)
        //    {
        //        case "TreeViewWithFiles":
        //            // All settings are set by dropdownboxes
        //            return true;
        //        case "List":
        //            if (!int.TryParse(txtListNumberOfValuesInDefaultView.Text, out i))
        //            {
        //                return false;
        //            }
        //            break;
        //        case "TreeViewWithoutFiles":
        //        case "TreeViewAndFiles":
        //            if (!int.TryParse(txtTree1NumberOfValuesInDefaultView.Text, out i))
        //            {
        //                return false;
        //            }
        //            break;
        //    }

        //    return true;
        //}

        /// <summary>
        /// Get selected View link mode
        /// </summary>
        /// <param name="strViewLinkMode">string to get value from</param>
        /// <returns>Selected view link mode</returns>
        private MenuViewLinkModeType GetViewLinkMode(string strViewLinkMode)
        {
            switch (strViewLinkMode)
            {
                case "Hidden":
                    return MenuViewLinkModeType.Hidden;
                case "DefaultValues":
                    return MenuViewLinkModeType.DefaultValues;
                case "AllValues":
                    return MenuViewLinkModeType.AllValues;
                default:
                    return MenuViewLinkModeType.DefaultValues;
            }
        }

        protected void cboMenuMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSelectedPanel();
        }
        protected void cboTree2MetadataAsIcons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTree2MetadataAsIcons.SelectedValue == "False")
            {
                pnlShowTextForMetadata.Visible = true;

            }
            else 
            {
                pnlShowTextForMetadata.Visible = false;                
            }
        }

        
        protected void MenuModeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuMenuMode", "PxWebAdminSettingsMenuMenuModeInfo");
        }

        protected void ShowRootInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowRoot", "PxWebAdminSettingsMenuShowRootInfo");
        }

        protected void ExpandAllInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuExpandAll", "PxWebAdminSettingsMenuExpandAllInfo");
        }
        
        protected void SortByAliasInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuSortByAlias", "PxWebAdminSettingsMenuSortByAliasInfo");
        }

        protected void ShowSelectionLinkInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowSelectionLink", "PxWebAdminSettingsMenuShowSelectionLinkInfo");
        }

        protected void ShowDownloadLinkInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowDownloadLink", "PxWebAdminSettingsMenuShowDownloadLinkInfo");
        }

        protected void ViewLinkModeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuViewLinkMode", "PxWebAdminSettingsMenuViewLinkModeInfo");
        }

        protected void NumberOfValuesInDefaultViewInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuNumberOfValuesInDefaultView", "PxWebAdminSettingsMenuNumberOfValuesInDefaultViewInfo");
        }

        protected void ShowModifiedDateInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowModifiedDate", "PxWebAdminSettingsMenuShowModifiedDateInfo");
        }

        protected void ShowLastUpdatedInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowLastUpdated", "PxWebAdminSettingsMenuShowLastUpdatedInfo");
        }

        protected void ShowFileSizeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowFileSize", "PxWebAdminSettingsMenuShowFileSizeInfo");
        }

        protected void ShowTableCategoryInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowTableCategory", "PxWebAdminSettingsMenuShowTableCategoryInfo");
        }

        protected void ShowTableUpdatedAfterPublishInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowTableUpdatedAfterPublish", "PxWebAdminSettingsMenuShowTableUpdatedAfterPublishInfo");
        }

        protected void ShowVariablesAndValuesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowVariablesAndValues", "PxWebAdminSettingsMenuShowVariablesAndValuesInfo");
        }

        protected void MetadataAsIconsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuMetadataAsIcons", "PxWebAdminSettingsMenuMetadataAsIconsInfo");
        }

        protected void ShowMenuExplanationInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowMenuExplanation", "PxWebAdminSettingsMenuShowMenuExplanationInfo");
        }

        protected void ShowTextToMetadataInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsMenuShowTextToMetadata", "PxWebAdminSettingsMenuShowTextToMetadataInfo");
        }

        //protected void cboListViewLinkMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cboListViewLinkMode.SelectedValue != "DefaultValues")
        //    {
        //        txtListNumberOfValuesInDefaultView.Visible = false;
        //        lblListNumberOfValuesInDefaultView.Visible = false;
        //        imgListNumberOfValuesInDefaultView.Visible = false;
        //        validatorListNumberOfValuesInDefaultView.Visible = false;
        //    }
        //    else
        //    {
        //        txtListNumberOfValuesInDefaultView.Visible = true;
        //        lblListNumberOfValuesInDefaultView.Visible = true;
        //        imgListNumberOfValuesInDefaultView.Visible = true;
        //        validatorListNumberOfValuesInDefaultView.Visible = true;
        //    }
        //}

        protected void cboTree1ViewLinkMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTree1ViewLinkMode.SelectedValue != "DefaultValues")
            {
                txtTree1NumberOfValuesInDefaultView.Visible = false;
                lblTree1NumberOfValuesInDefaultView.Visible = false;
                imgTree1NumberOfValuesInDefaultView.Visible = false;
                validatorTree1NumberOfValuesInDefaultView.Visible = false;
            }
            else
            {
                txtTree1NumberOfValuesInDefaultView.Visible = true;
                lblTree1NumberOfValuesInDefaultView.Visible = true;
                imgTree1NumberOfValuesInDefaultView.Visible = true;
                validatorTree1NumberOfValuesInDefaultView.Visible = true;
            }
        }



    }
}
