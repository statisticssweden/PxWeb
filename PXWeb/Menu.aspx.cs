using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Text;
using PCAxis.Web;
using PCAxis.Web.Core.Management;
using PCAxis.Menu;
using PCAxis.Menu.Implementations;


namespace PXWeb
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //Set method to retrieve menu nodes dynamically
            TableOfContent1.GetMenu = GetMenu;
            TableList1.GetMenu = GetMenu;
            MenuExplanation.Visible = PXWeb.Settings.Current.Menu.ShowMenuExplanation;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel = null;
            PCAxis.Web.Controls.VariableSelector.SelectedVariableValues.Clear();

            ((PxWeb)this.Master).FooterText = "Menu";

            Master.HeadTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebTitleMenu");
            if (!Page.IsPostBack )
            {
                Master.SetBreadcrumb(PCAxis.Web.Controls.Breadcrumb.BreadcrumbMode.Menu);
                Master.SetNavigationFlowMode(PCAxis.Web.Controls.NavigationFlow.NavigationFlowMode.First); 
                Master.SetNavigationFlowVisibility(PXWeb.Settings.Current.Navigation.ShowNavigationFlow);

                string msg = QuerystringManager.GetQuerystringParameter("msg");
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    dialogModal.Visible = true;

                    dialogModal.Attributes["title"] = Master.GetLocalizedString(msg + "Title");
                    lblMsg.Text = Master.GetLocalizedString(msg + "Message");
                    if (PXWeb.Settings.Current.Menu.MenuMode == MenuModeType.TreeViewWithFiles)
                    {
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowModalDialog", "<script type='text/javascript'> jQuery(function() {var offset = jQuery('.AspNet-TreeView-Collapse:first').offset().top; jQuery('#" + dialogModal.ClientID + "').dialog({ width: 480, height: 200,  modal: true, position: ['top', offset], buttons: {" + Master.GetLocalizedString("PxWebPopupDialogClose") + ": function () {jQuery(this).dialog('close');} } }); });</script>  ");
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowModalDialog", "<script type='text/javascript'> jQuery(function() {var offset = jQuery('.AspNet-TreeView-Collapse:first').offset(); jQuery('#" + dialogModal.ClientID + "').dialog({ width: 480, height: 200,  modal: true, position: ['top', offset], buttons: {" + Master.GetLocalizedString("PxWebPopupDialogClose") + ": function () {jQuery(this).dialog('close');} } }); });</script>  ");
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowModalDialog", "<script type='text/javascript'> jQuery(function() { jQuery('#" + dialogModal.ClientID + "').dialog({ width: 480, height: 200,  modal: true, buttons: {" + Master.GetLocalizedString("PxWebPopupDialogClose") + ": function () {jQuery(this).dialog('close');} } }); });</script>  ");
                    }
                }
               
                PCAxis.Web.Core.Management.PaxiomManager.Clear();
                InitializeTableOfContent();
                InitializeTableList();
                InitializeSearch();
                SetLocalizedTexts();
            }
        }

        /// <summary>
        /// Gets the menu object
        /// </summary>
        /// <returns>returns the menu object</returns>
        private Item GetMenu(string nodeId)
        {
            var pxUrl = RouteInstance.PxUrlProvider.Create(null);

            //Checks that the necessary parameters are present
            if (String.IsNullOrEmpty(pxUrl.Database))
            {
                //if parameters is missing redirect to the start page
                Response.Redirect("Default.aspx", false);
            }

            try
            {
                return PXWeb.Management.PxContext.GetMenuItem(pxUrl.Database, nodeId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void MyLoadAll(PCAxis.Menu.Item item, PxMenuBase menu)
        {
            try
            {
                menu.SetCurrentItemBySelection(item.ID.Menu, item.ID.Selection);
            }
            catch (Exception ex)
            {
                Trace.Write(ex.ToString());
            }
            
            if (menu.CurrentItem is PxMenuItem)
            {
                foreach (var subItem in  ((PxMenuItem)menu.CurrentItem).SubItems)
                {
                    MyLoadAll(subItem, menu);
                }
            }
        }

        /// <summary>
        /// Initializes Table of content
        /// </summary>
        private void InitializeTableOfContent()
        {
            TableOfContent1.MetaIconSizeImagePath = "fileSize.gif";
            TableOfContent1.MetaIconModifiedImagePath = "fileModified.gif";
            TableOfContent1.MetaIconUpdatedImagePath = "fileUpdated.gif";
            TableOfContent1.MetaIconUpdatedAfterPublishImagePath = "refresh.png";
            TableOfContent1.ShowRootName = PXWeb.Settings.Current.Menu.ShowRoot;
            TableOfContent1.SortByAlias = true; // Functionality to switch on/off SortByAlias is not implemented in first version
            TableOfContent1.ExpandAllNodes = PXWeb.Settings.Current.Menu.ExpandAll;
            TableOfContent1.DefaultPageURL = "Default.aspx";
            TableOfContent1.MenuPageURL = "Menu.aspx";
            TableOfContent1.SelectPageURL = "Selection.aspx";
            TableOfContent1.UrlLinkMode = PCAxis.Web.Controls.UrlLinkModeType.TreeNode;
            TableOfContent1.ShowTableCategory = PXWeb.Settings.Current.Menu.ShowTableCategory;
            TableOfContent1.ShowTableUpdatedAfterPublish = PXWeb.Settings.Current.Menu.ShowTableUpdatedAfterPublish;

            switch (PXWeb.Settings.Current.Menu.MenuMode)
            {
                case MenuModeType.List:
                    TableOfContent1.ShowTreeViewMenu = false;
                    break;
                case MenuModeType.TreeViewWithFiles:
                    TableOfContent1.ShowTreeViewMenu = true;
                    TableOfContent1.IncludePXFilesInTreeView = true;
                    TableOfContent1.ShowFileSize = PXWeb.Settings.Current.Menu.ShowFileSize;
                    TableOfContent1.ShowLastUpdated = PXWeb.Settings.Current.Menu.ShowLastUpdated;
                    TableOfContent1.ShowModified = PXWeb.Settings.Current.Menu.ShowModifiedDate;
                    TableOfContent1.MetadataAsIcons = PXWeb.Settings.Current.Menu.MetadataAsIcons;
                    TableOfContent1.ShowTextForMetadata = PXWeb.Settings.Current.Menu.ShowTextToMetadata;
                    break;
                case MenuModeType.TreeViewAndFiles:
                //TreeViewAndFiles is not implemented in first version
                case MenuModeType.TreeViewWithoutFiles:
                    TableOfContent1.ShowTreeViewMenu = true;
                    TableOfContent1.IncludePXFilesInTreeView = false;
                    break;
            }

            IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
            DatabaseInfo dbi = null;
            dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(url.Database);

            if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
            {
                TableOfContent1.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.CNMM;
            }

            if (!string.IsNullOrEmpty(url.Path))
            {
                TableOfContent1.ExpandNode = url.Path;
            }
        }

        /// <summary>
        /// Initializes Table list
        /// </summary>
        private void InitializeTableList()
        {
            TableList1.ShowFileSize = PXWeb.Settings.Current.Menu.ShowFileSize;
            TableList1.ShowLastUpdated = PXWeb.Settings.Current.Menu.ShowLastUpdated;
            TableList1.ShowModifiedDate = PXWeb.Settings.Current.Menu.ShowModifiedDate;
            TableList1.ShowVariablesAndValues = PXWeb.Settings.Current.Menu.ShowVariablesAndValues;
            TableList1.SelectOption_Select = PXWeb.Settings.Current.Menu.ShowSelectLink;
            TableList1.ShowAsListOfContent = false;
            TableList1.DefaultPageURL = "Default.aspx";
            TableList1.MenuPageURL = "Menu.aspx";
            TableList1.SelectPageURL = "Selection.aspx";
            TableList1.ViewPageURL = "Presentation.aspx";
            TableList1.SmallFileSizeMode = PCAxis.Web.Controls.SmallFileSizeModeType.NumberOfCells;
            TableList1.MaxFileziseForSmallFile = PXWeb.Settings.Current.General.FileFormats.CellLimitDownloads;
            TableList1.DatabasePath = PXWeb.Settings.Current.General.Paths.PxDatabasesPath;

            TableList1.SelectOption_Download = PXWeb.Settings.Current.Menu.ShowDownloadLink;
            TableList1.SelectOption_View = false;
            TableList1.SelectOption_ViewDefaultValues = false;
            TableList1.SelectOption_ViewDefaultValuesWithCommandbar = false;
            TableList1.SelectOption_ViewWithCommandbar = false;
            switch (PXWeb.Settings.Current.Menu.ViewLinkMode)
            {
                case MenuViewLinkModeType.Hidden:
                    break;
                case MenuViewLinkModeType.DefaultValues:
                    if (PXWeb.Settings.Current.Presentation.CommandBar.ViewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.Hidden)
                    {
                        TableList1.SelectOption_ViewDefaultValues = true;
                    }
                    else
                    {
                        TableList1.SelectOption_ViewDefaultValuesWithCommandbar = true;
                    }
                    break;
                case MenuViewLinkModeType.AllValues:
                    if (PXWeb.Settings.Current.Presentation.CommandBar.ViewMode == PCAxis.Web.Controls.CommandBar.CommandBarViewMode.Hidden)
                    {
                        TableList1.SelectOption_View = true;
                    }
                    else
                    {
                        TableList1.SelectOption_ViewWithCommandbar = true;
                    }
                    break;
                default:
                    break;
            }

            IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
            DatabaseInfo dbi = null;
            dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(url.Database);

            if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
            {
                TableList1.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.CNMM;
            }
        }

        private void InitializeSearch()
        {
            pxSearch.RedirectOnSearch = true;
            if (PXWeb.Settings.Current.Features.General.SearchEnabled == false)
            {
                pxSearch.Visible = false;
                return;
            }

            IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
            //PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(url.Database);
            PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabaseSettings(url.Database);
            PXWeb.SearchIndexSettings searchIndex = (PXWeb.SearchIndexSettings)db.SearchIndex;

            if (searchIndex.Status == SearchIndexStatusType.NotIndexed)
            {
                pxSearch.Visible = false;
            }
        }

        private void SetLocalizedTexts()
        {
            var pxUrl = RouteInstance.PxUrlProvider.Create(null);

            string db = pxUrl.Database;
            string lang = pxUrl.Language;
            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(db);

            lblDatabase.Text = dbi.GetDatabaseName(lang);

 
        }

    }
}
