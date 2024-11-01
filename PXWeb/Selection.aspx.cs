using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCAxis.Paxiom;
using PCAxis.Web.Controls;
using PCAxis.Web.Core.Management;

namespace PXWeb
{
    public enum LayoutFormat
    {
        simple = 0,
        compact = 1,
    }
    public partial class Selection : System.Web.UI.Page
    {
        /// Used to initialize selected values from previous selection
        private PXModel _previousModel = null;
        private String _pageUrl = String.Empty;
        private String _tableTitle = String.Empty;
        private String _lastModified = String.Empty;
        private String _switchToCompactTxt;
        private String _switchToListTxt;
        private String _switchToCompactTxtScreenReader;
        private String _switchToListTxtScreenReader;

        private PCAxis.Metadata.IMetaIdProvider _linkManager;

        //Class Properties used for metatags
        public string TableTitle
        {
            get { return _tableTitle; }
            set { _tableTitle = value ; }
        }
        public string PageUrl
        {
            get { return _pageUrl; }
            set { _pageUrl = value; }
        }
        public string LastModified
        {
            get { return _lastModified; }
            set { _lastModified = value; }
        }

        private IPxUrl _pxUrl = null;

        private IPxUrl PxUrl
        {
            get
            {
                if (_pxUrl == null)
                {
                    _pxUrl = RouteInstance.PxUrlProvider.Create(null);
                }

                return _pxUrl;
            }
        }

        private LayoutFormat _selectionLayout;
        public LayoutFormat SelectionLayout
        {
            get { return _selectionLayout; }
            set { _selectionLayout = value; }
        }
        const string layoutCookie="layoutCookie";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (PaxiomManager.PaxiomModel != null && !PaxiomManager.PaxiomModel.Meta.MainTable.Contains(PxUrl.Table))
            {
                PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;
                PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel = null;
                PCAxis.Web.Core.Management.PaxiomManager.QueryModel = null;
                PCAxis.Web.Controls.VariableSelector.SelectedVariableValues.Clear();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Path for selection should always contain tableid and not table name
            if (RouteInstance.RouteExtender != null && RouteInstance.RouteExtender.DoesSelectionPathContainTableName(this))
            {
                var linkItems = new List<LinkManager.LinkItem>();
                linkItems.Add(new LinkManager.LinkItem(PXWeb.PxUrl.TABLE_KEY, PxUrl.Table));
                string redirectLink = LinkManager.CreateLinkMethod("Selection.aspx", false, linkItems.ToArray());
                Response.Redirect(redirectLink, true);
            }

            if (RouteInstance.RouteExtender != null)
            {
                var tableId = RouteInstance.RouteExtender.GetTableIdByName(PxUrl.Table);
                bool hasTableData = RouteInstance.RouteExtender.HasTableData(tableId);

                if (!hasTableData)
                {
                    Response.Redirect(RouteInstance.RouteExtender.GetRedirectNoDataPath(tableId), true);
                }
            }

            ((PxWeb)this.Master).FooterText = "Selection";
            
            string lang = PxUrl.Language;
            string db = PxUrl.Database;
            string path = PxUrl.Path;
            string table = PxUrl.Table;

            string partTable = "";

            _switchToCompactTxt = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebSwitchToCompactView");
            _switchToListTxt = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebSwitchToListView");
            _switchToCompactTxtScreenReader = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebSwitchToCompactViewScreenReader");
            _switchToListTxtScreenReader = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebSwitchToListViewScreenReader");

            //Check if the queryStrings contains partTable 
            if (QuerystringManager.GetQuerystringParameter("partTable") != null)
            {
                partTable = QuerystringManager.GetQuerystringParameter("partTable");

            }
            // Bug 273
            // If we have no builder the groupings should be reloaded
            if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder == null)
            {
                VariableSelector1.ReloadGroupings = true;
            }

            if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null && PaxiomManager.QueryModel != null)
            {
                _previousModel = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel;
                VariableSelector1.PreSelectFirstContentAndTime = false;
            }
            else
            {
                VariableSelector1.PreSelectFirstContentAndTime = PXWeb.Settings.Current.Selection.PreSelectFirstContentAndTime;
            }

            // Check if we are being called from a saved query with the ?select switch. If that is the case we should always clear the PxModel.
            bool clearModel = false;
            if (HttpContext.Current.Session["SelectionClearPxModel"] != null)
            {
                clearModel = (bool)HttpContext.Current.Session["SelectionClearPxModel"];
                HttpContext.Current.Session.Remove("SelectionClearPxModel");
            }

            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel = PXWeb.Management.PxContext.GetPaxiomForSelection(db, path, table, lang, clearModel);
           _linkManager = PXWeb.Settings.Current.Database[PxUrl.Database].Metadata.MetaLinkMethod;
            InitializeLayoutFormat();
            InitializeTableHeadings();

            if (!IsPostBack)
            {
                Master.HeadTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebTitleSelection");
                //imgShowInformationExpander.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(BreadcrumbCodebehind), "PCAxis.Web.Controls.spacer.gif");
                //imgShowFootnotesExpander.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(BreadcrumbCodebehind), "PCAxis.Web.Controls.spacer.gif");
                //imgShowMetadataExpander.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(BreadcrumbCodebehind), "PCAxis.Web.Controls.spacer.gif");
                Master.SetBreadcrumb(PCAxis.Web.Controls.Breadcrumb.BreadcrumbMode.Selection);
                Master.SetH1TextMenuLevel();
                Master.SetNavigationFlowMode(PCAxis.Web.Controls.NavigationFlow.NavigationFlowMode.Second);
                Master.SetNavigationFlowVisibility(PXWeb.Settings.Current.Navigation.ShowNavigationFlow);
                InitializeVariableSelector();
                InitializeTableInformation();
                SelectionFootnotes.ShowNoFootnotes = PXWeb.Settings.Current.Selection.ShowNoFootnoteForSelection;
                InitializeMetatags();
                InitializeMetadata(path);
                //Check if the queryStrings contains partTable and the database type is CNMM
                //if so download subtable variables
                if (!string.IsNullOrEmpty(partTable))
                {                  
                    DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(db);
                    if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
                    {
                        SetValuesFromPartTable(partTable);
                    }                                        
                }
            }

            VariableSelector1.MetaLinkProvider = _linkManager;
            VariableSelector1.PxActionEvent += new PCAxis.Web.Controls.PxActionEventHandler(HandlePxAction);
            VariableSelector1.MetadataInformationSelected += new VariableSelector.MetadataInformationSelectedEventHandler(HandleMetaDataInformationAction);
            VariableSelector1.LeaveVariableSelectorMain += new VariableSelector.LeaveVariableSelectorMainEventHandler(HandleLeaveVariableSelectorMain);
            VariableSelector1.ReenterVariableSelectorMain += new VariableSelector.ReenterVariableSelectorMainEventHandler(HandleReenterVariableSelectorMain);

            if (_previousModel != null && _previousModel.IsComplete && !IsPostBack)
            {
                VariableSelector1.InitializeSelectedValuesetsAndGroupings(_previousModel);
            }

        }


        protected void SetValuesFromPartTable(string partTable){
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder.ApplyValueSet(partTable);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (_previousModel != null && _previousModel.IsComplete)
            {
                if (!IsPostBack)
                {
                    VariableSelector1.InitializeSelection(_previousModel);
                    _previousModel = null;
                }
            }
        }

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            VariableSelector1.PxActionEvent -= new PCAxis.Web.Controls.PxActionEventHandler(HandlePxAction);
        }

        

        private void InitializeTableHeadings()
        {
            if (PXWeb.Settings.Current.General.Site.MainHeaderForTables == MainHeaderForTablesType.TableName)
            {
                TableInformationSelect.TitleTag = TableInformationCodebehind.TitleTags.H1;
                MenuTitle.Level = CustomControls.HeadingLabel.HeadingLevel.H1;
                lblSubHeader.Level = CustomControls.HeadingLabel.HeadingLevel.H2;
                lblSubHeader.Visible = true;
            }
            else
            {
                TableInformationSelect.TitleTag = TableInformationCodebehind.TitleTags.H2;
                MenuTitle.Level = CustomControls.HeadingLabel.HeadingLevel.H2;
                lblSubHeader.Level = CustomControls.HeadingLabel.HeadingLevel.H3;
                lblSubHeader.Visible = false;
            }
        }

        /// <summary>
        /// Initialize the variable selector web control
        /// </summary>
        private void InitializeVariableSelector()
        {
            VariableSelector1.SortVariableOrder = PXWeb.Settings.Current.Selection.SortVariableOrder;
            VariableSelector1.SelectedTotalCellsLimit = PXWeb.Settings.Current.Selection.CellLimitScreen;
            VariableSelector1.SelectedTotalCellsDownloadLimit = PXWeb.Settings.Current.General.FileFormats.CellLimitDownloads;
            VariableSelector1.ShowElimMark = PXWeb.Settings.Current.Selection.ShowMandatoryMark;
            VariableSelector1.AllowAggreg = PXWeb.Settings.Current.Selection.AllowAggregations;
            VariableSelector1.ShowHierarchies = PXWeb.Settings.Current.Selection.Hierarchies.ShowHierarchies;
            VariableSelector1.HierarchicalSelectionLevelsOpen = PXWeb.Settings.Current.Selection.Hierarchies.HierarchicalLevelsOpen;
            VariableSelector1.ShowMarkingTips = PXWeb.Settings.Current.Selection.MarkingTips.ShowMarkingTips;
            VariableSelector1.ClientSideValidation  = PXWeb.Settings.Current.Selection.ClientSideValidation;

            string markingTipsPage;
            markingTipsPage = PCAxis.Web.Controls.Configuration.ConfigurationHelper.GetPxPage("markingtips");
            if (string.IsNullOrEmpty(markingTipsPage))
            {
                markingTipsPage = "MarkingTips.aspx";
            }
            VariableSelector1.MarkingTipsLinkNavigateUrl = markingTipsPage;
            
            VariableSelector1.SearchButtonMode = PXWeb.Settings.Current.Selection.SearchButtonMode;
            VariableSelector1.MaxRowsWithoutSearch = PXWeb.Settings.Current.Selection.MaxRowsWithoutSearch;
            VariableSelector1.AlwaysShowTimeVariableWithoutSearch = PXWeb.Settings.Current.Selection.AlwaysShowTimeVariableWithoutSearch;
            VariableSelector1.ListSize = PXWeb.Settings.Current.Selection.ListSize;
            VariableSelector1.ShowSelectionLimits = PXWeb.Settings.Current.Selection.ShowSelectionLimits;

            IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
            VariableSelector1.MetadataInformationMode = PXWeb.Settings.Current.Database[url.Database].Metadata.UseMetadata;
            VariableSelector1.SelectionFromGroupButtonMode = PXWeb.Settings.Current.Selection.SelectValuesFromGroup;
            VariableSelector1.ButtonsForContentVariable = PXWeb.Settings.Current.Selection.ButtonsForContentVariable;
            VariableSelector1.SearchValuesBeginningOfWordCheckBoxDefaultChecked = PXWeb.Settings.Current.Selection.SearchValuesBeginningOfWordCheckBoxDefaultChecked;

            SetPresentationView();

            VariableSelector1.NumberOfValuesInDefaultView = PXWeb.Settings.Current.Menu.NumberOfValuesInDefaultView;

            VariableSelector1.LimitSelectionsBy = "Cells";
            //VariableSelector1.LimitSelectionsBy = "RowsColumns";
            VariableSelector1.SelectedColumnsLimit = PXWeb.Settings.Current.Presentation.Table.MaxColumns;
            VariableSelector1.SelectedRowsLimit = PXWeb.Settings.Current.Presentation.Table.MaxRows;
            VariableSelector1.ShowSearchInformationLink = false;
            VariableSelector1.ShowTableNameInSearch = false;
            VariableSelector1.ValuesetMustBeSelectedFirst = PXWeb.Settings.Current.Selection.ValuesetMustBeSelectedFirst;
            VariableSelector1.ShowAllAvailableValuesSearchButton = PXWeb.Settings.Current.Selection.ShowAllAvailableValuesSearchButton;
            VariableSelector1.AlwaysShowCodeAndTextInAdvancedSearchResult = PXWeb.Settings.Current.Selection.AlwaysShowCodeAndTextInAdvancedSearchResult;
        }

        /// <summary>
        /// Initializes the TableInformation web control
        /// </summary>
        private void InitializeTableInformation()
        {
            var siteTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("SiteTitle");

            if (PXWeb.Settings.Current.Selection.TitleFromMenu)
            {
                //Show table title as it was displayed in the menu
                IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
                PCAxis.Menu.Item currentItem = PXWeb.Management.PxContext.GetMenuItem(url.Database, url.TablePath);

                if (!String.IsNullOrEmpty(currentItem.Text))
                {
                    MenuTitle.Text = currentItem.Text;
                }
                else
                {
                    //ssb:Jira:UUP-267  For cases where the table has been repositioned in the menu-tree, and the user uses an old url:
                    // https..../START__old_pos/MyTableStillExistsElsewhere
                   MenuTitle.Text = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Title;
                }

                MenuTitle.Visible = true;
                if (!PXWeb.Settings.Current.Selection.StandardApplicationHeadTitle)
                {
                    Master.HeadTitle = MenuTitle.Text + ". " + siteTitle;
                }
                TableInformationSelect.Visible = false;               
            }
            else
            {
                //Show table title in TableInformation web control
                TableInformationSelect.ShowSourceDescription = PXWeb.Settings.Current.General.Global.ShowSourceDescription;
                MenuTitle.Visible = false;
                TableInformationSelect.Visible = true;

                if (!PXWeb.Settings.Current.Selection.StandardApplicationHeadTitle)
                {
                    if (PaxiomManager.PaxiomModel.Meta.DescriptionDefault && !string.IsNullOrEmpty(PaxiomManager.PaxiomModel.Meta.Description))
                    {
                        Master.HeadTitle = PaxiomManager.PaxiomModel.Meta.Description;
                    }
                    else
                    {
                        Master.HeadTitle = PaxiomManager.PaxiomModel.Meta.Title;
                    }

                    Master.HeadTitle += ". " + siteTitle;
                }
            }

        }
        /// <summary>
        /// Initializes the Meta tags on the Selectionpage
        /// Tags for search engine 
        /// </summary>
        private void InitializeMetatags()
        {
            //Set value on metatags

            //Meta name/property Title
            if (PXWeb.Settings.Current.Selection.TitleFromMenu)
            {
                //Retrieve text for the meta tag title from the menu or table heading
                IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
                PCAxis.Menu.Item currentItem = PXWeb.Management.PxContext.GetMenuItem(url.Database, url.TablePath);

                TableTitle = currentItem.Text;
            }
            else 
            {
                TableTitle = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Data.Model.Meta.Title;
            }
            //meta property URL            
            System.Text.StringBuilder sbPageUrl = new System.Text.StringBuilder();       
            sbPageUrl.Append(GetAppPath());
            //If the method returns a link that started with / so it must the / be removed otherwise it will be // in the path 
            if (PCAxis.Web.Core.Management.LinkManager.CreateLink("Selection.aspx").ToString().StartsWith("/"))
            {
                sbPageUrl.Append(PCAxis.Web.Core.Management.LinkManager.CreateLink("Selection.aspx",null).ToString().Remove(0,1));                            
            }
            else
            {
                sbPageUrl.Append(PCAxis.Web.Core.Management.LinkManager.CreateLink("Selection.aspx").ToString());
            }
            //If the pageurl contains rxid, remove this key because it´s not useful in the metatagg
            if (sbPageUrl.ToString().Contains("rxid")) {
                PageUrl =RemoveQueryStringByKey(sbPageUrl.ToString(), "rxid"); 
            }
            else
            {
                PageUrl = sbPageUrl.ToString();
                 
            }
        }
        private String GetAppPath() {
            string appPath = String.Empty;            
            System.Web.HttpContext context   = System.Web.HttpContext.Current;

            appPath = String.Format("{0}://{1}{2}{3}",
                                        context.Request.Url.Scheme,
                                        context.Request.Url.Host,
                                        context.Request.Url.Port.Equals(80) ? string.Empty : ":" + context.Request.Url.Port,
                                        context.Request.ApplicationPath);

            return appPath;
        }
        
        /// <summary>
        /// Initializes the metadata part of the selection page (footnotes and information)
        /// </summary>
        private void InitializeMetadata(string path)
        {
            if (PXWeb.Settings.Current.Selection.MetadataAsLinks)
            {
                lnkInformation.NavigateUrl = PCAxis.Web.Core.Management.LinkManager.CreateLink("~/InformationSelection.aspx");
                lnkInformation.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebInformation");
                lnkFootnotes.NavigateUrl = PCAxis.Web.Core.Management.LinkManager.CreateLink("~/FootnotesSelection.aspx");
                lnkFootnotes.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebFootnotes");              
                InitializeDetailedInformation(path);
                InformationLinks.Visible = true;
                SelectionFootnotes.Visible = false;
                divFootnotes.Visible = false;
            }
            else
            {
                InitializeDetailedInformation(path);
                InformationLinks.Visible = false;
                SelectionFootnotes.Visible = true;
                IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
                bool bMeta = PXWeb.Settings.Current.Database[url.Database].Metadata.UseMetadata;
            }


           SetBulkLink();

        }

        private void SetBulkLink()
        {
            var bBulkLink = PXWeb.Settings.Current.Features.General.BulkLinkEnabled;

            if (!bBulkLink)
            {
                linkBulkLink.Visible = false;
                linkBulkDiv.Visible = false;
                return;
            }
            else
            {  
                var language = PaxiomManager.PaxiomModel.Meta.Language;
                var tableid = PaxiomManager.PaxiomModel.Meta.TableID;
                if (tableid != null)
                {
                    IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
                    var path = "/Resources/PX/bulk/" + url.Database + "/" + language + "/" + tableid + "_" +language + ".zip";
                    var linkText = LocalizationManager.GetLocalizedString("PxWebBulkLink") + " (" + tableid + "_" + language + ".zip)";
                    var realPath = Server.MapPath(path);

                    if (File.Exists(realPath))
                    {
                        linkBulkLink.Visible = true;
                        linkBulkDiv.Visible = true;
                        linkBulkLink.Text = linkText;
                        linkBulkLink.NavigateUrl = path;
                        return;
                    }
                }
                else
                {
                    linkBulkLink.Visible = false;
                    linkBulkDiv.Visible = false;
                    return;
                }                
                
            }
        }

        /// <summary>
        /// Initializes the link with detailed information about the table
        /// </summary>
        /// <param name="path">Path to the table</param>
        private void InitializeDetailedInformation(string path)
        {
            string strPathTmp = "";
            string strInfoFile = "";
            int iStart;
            char[] tilde = {'~'};

            lnkDetailedInformation.Visible = false;
            
            if (!string.IsNullOrEmpty(PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile) && PXWeb.Settings.Current.General.Global.ShowInfoFile)
            {
                try
                {

                    //Check if infofile is an URL
                    Uri uriResult;
                    if (Uri.TryCreate(PaxiomManager.PaxiomModel.Meta.InfoFile, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp)
                    {
                        SetDetailedInformationLink(uriResult.ToString());
                    }
                    else
                    {
                        path = path.Replace("__", @"\");

                        strPathTmp = Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath + path);

                        if (Directory.Exists(strPathTmp))
                        {
                            foreach (var file in Directory.GetFiles(strPathTmp))
                            {
                                if (file != null && Path.GetFileNameWithoutExtension(file).Equals(PaxiomManager.PaxiomModel.Meta.InfoFile, StringComparison.OrdinalIgnoreCase))
                                {
                                    strInfoFile = file;
                                }
                            }
                        }
                    }


                    if (strInfoFile.Length > 0)
                    {
                        strInfoFile = strInfoFile.Replace(@"\", @"/");
                        iStart = strInfoFile.LastIndexOf(PXWeb.Settings.Current.General.Paths.PxDatabasesPath.TrimStart(tilde));

                        if (iStart > -1)
                        {
                            strInfoFile = strInfoFile.Substring(iStart);
                            if (!strInfoFile.StartsWith("/"))
                            {
                                strInfoFile = "~/" + strInfoFile;
                            }
                            else
                            {
                                strInfoFile = "~" + strInfoFile;
                            }

                            SetDetailedInformationLink(HttpUtility.UrlPathEncode(strInfoFile));

                        }
                    }
                }
                catch (SystemException)
                {
                    // Not a realtive path

                    // Is it a HTML-link?
                    if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile.Contains("<") &&
                        PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile.Contains(">")) 
                    {
                        //check if the infofile shall appears as links next to the footnote link or as a link in 
                        //the information section on the tab About table
                        if (PXWeb.Settings.Current.Selection.MetadataAsLinks)
                        {
                            litDetailedInformation.Visible = true;
                            litDetailedInformation.Text = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile.ToLower();
                            lnkDetailedInformation.Visible = false;
                        }
                    }
                    else
                    {
                         //check if the infofile shall appears as links next to the footnote link or as a link in 
                         //the information section on the tab About table
                        if (PXWeb.Settings.Current.Selection.MetadataAsLinks)
                        {
                            // Show it as it is...
                            lnkDetailedInformation.Visible = true;
                            lnkDetailedInformation.NavigateUrl = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.InfoFile;
                            lnkDetailedInformation.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebDetailedInformation");
                        }
                    }
                }
            }
        }

        private void SetDetailedInformationLink(string link)
        {
            //check if the infofile shall appears as links next to the footnote link or as a link in 
            //the information section on the tab About table
            if (PXWeb.Settings.Current.Selection.MetadataAsLinks)
            {
                lnkDetailedInformation.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebDetailedInformation");
                lnkDetailedInformation.Visible = true;
                lnkDetailedInformation.NavigateUrl = link;
            }
        }

        public static string RemoveQueryStringByKey(string url, string key)
        {
            var indexOfQuestionMark = url.IndexOf("?");
            if (indexOfQuestionMark == -1)
            {
                return url;
            }

            var result = url.Substring(0, indexOfQuestionMark);
            var queryStrings = url.Substring(indexOfQuestionMark + 1);
            var queryStringParts = queryStrings.Split(new[] { '&' });
            var isFirstAdded = false;

            for (int index = 0; index < queryStringParts.Length; index++)
            {
                var keyValue = queryStringParts[index].Split(new char[] { '=' });
                if (keyValue[0] == key)
                {
                    continue;
                }

                if (!isFirstAdded)
                {
                    result += "?";
                    isFirstAdded = true;
                }
                else
                {
                    result += "&";
                }

                result += queryStringParts[index];
            }

            return result;
        }

        /// <summary>
        /// Handle PX-actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandlePxAction(object sender, PCAxis.Web.Controls.PxActionEventArgs e)
        {
            //Log visitor statistics
            if (PXWeb.Settings.Current.Features.General.UserStatisticsEnabled)
            {
                PCAxis.Web.Controls.VisitorStatisticsHelper.LogEvent(PCAxis.Web.Controls.ActionContext.Selection, "userid", PxUrl.Language, PxUrl.Database, e);
            }
            if (e.ActionName.Equals("metadata"))
            {
                
            }
        }

        public void HandleMetaDataInformationAction(object sender, PCAxis.Web.Controls.VariableSelector.MatadataInformationEventArgs e)
        {
            //MetadataTab.Attributes["aria-selected"] = "true";
            AboutTableSelectedAccordion.Value = "metadata";
            //ucMetadataSystem.SelectVariable(e.Variable);
        }

        private void HandleLeaveVariableSelectorMain(object sender, EventArgs e)
        {
            //ucVariableOverview.Visible = false;
            Master.SetNavigationFlowVisibility(false);
            SwitchLayout.Visible = false;
            SelectionFootnotes.Visible = false;
            MaintainScrollPositionOnPostBack = false;
        }

        private void HandleReenterVariableSelectorMain(object sender, EventArgs e)
        {
            //ucVariableOverview.Visible = SelectionLayout == LayoutFormat.simple ? true : false;
            Master.SetNavigationFlowVisibility(PXWeb.Settings.Current.Navigation.ShowNavigationFlow);
            SwitchLayout.Visible = true;
            SelectionFootnotes.Visible = PXWeb.Settings.Current.Selection.MetadataAsLinks != true;
            MaintainScrollPositionOnPostBack = false;
        }

        private void SetPresentationView()
        {
            string defaultLayout = PXWeb.Settings.Current.Presentation.Table.DefaultLayout.ToString();

            switch (defaultLayout)
            {
                case "Layout1":
                {
                    VariableSelector1.PresentationView = Plugins.Views.TABLE_LAYOUT1;
                    break;
                }
                case "Layout2":
                {
                    VariableSelector1.PresentationView = Plugins.Views.TABLE_LAYOUT2;
                    break;
                }
                default:
                {
                    VariableSelector1.PresentationView = Plugins.Views.TABLE_LAYOUT1;
                    break;
                }
            }
        }
        protected void ShowHideAboutTablePanel(object sender, EventArgs e)
        {
            LinkButton clickedButton = (LinkButton)sender;
            if (clickedButton.ID== "aboutTablePanelButton")
            {
                ShowAboutTablePanel();
            }
            else
            {
                HideAboutTablePane();
            }
            //if (AboutTablePanel.Visible)
            //{

            //    HideAboutTablePane();

            //}
            //else
            //{
            //    ShowAboutTablePanel();
            //    // ViewState["ShowPresentationViewPanel"] = "true";
            //}
        }

            public void ShowAboutTablePanel()
        {
            //AboutTablePanelExpanded.Visible = true;
            //AboutTablePanelCollapsed.Visible = false;
        }

        public void HideAboutTablePane()
        {
            //AboutTablePanelExpanded.Visible = false;
            //AboutTablePanelCollapsed.Visible = true;
        }

        public void InitializeLayoutFormat()
        {
            HttpCookie myLayoutCookie = Request.Cookies[layoutCookie];

            if (myLayoutCookie == null)
            {
                _selectionLayout = LayoutFormat.compact;

                myLayoutCookie = new HttpCookie(layoutCookie);
                myLayoutCookie.Value = LayoutFormat.compact.ToString();
                myLayoutCookie.Expires = DateTime.Now.AddDays(370);
                Response.Cookies.Add(myLayoutCookie);
            }
            else
            {
                _selectionLayout = Request.Cookies[layoutCookie].Value.ToString() != "compact" ? LayoutFormat.simple : LayoutFormat.compact;
            }
            if (_selectionLayout== LayoutFormat.compact)
            {
                SwitchLayout.Text = _switchToListTxt;
                SwitchLayout.CssClass = "variableselector-list-view  pxweb-btn icon-placement variableselector-buttons";
                SwitchLayout.Attributes.Add("aria-label", _switchToListTxtScreenReader);
                //ucVariableOverview.Visible = false;
            }
            else
            {
                SwitchLayout.Text = _switchToCompactTxt; ;
                SwitchLayout.CssClass = "variableselector-compact-view  pxweb-btn icon-placement variableselector-buttons";
                SwitchLayout.Attributes.Add("aria-label", _switchToCompactTxtScreenReader);
                //ucVariableOverview.Visible = true;
            }

            
        }
         protected void SwitchLayout_Click(object sender, EventArgs e)
        {
            HttpCookie myLayoutCookie = new HttpCookie(layoutCookie);
            if (SelectionLayout == LayoutFormat.simple)
            {
                myLayoutCookie.Value = LayoutFormat.compact.ToString();
                SwitchLayout.Text = _switchToListTxt;
                SwitchLayout.CssClass = "variableselector-list-view  pxweb-btn icon-placement variableselector-buttons";
                SwitchLayout.Attributes.Add("aria-label", _switchToListTxtScreenReader);
                //ucVariableOverview.Visible = false;
            }
            else
            {
                myLayoutCookie.Value = LayoutFormat.simple.ToString();             
                SwitchLayout.Text = _switchToCompactTxt;
                SwitchLayout.CssClass = "variableselector-compact-view  pxweb-btn icon-placement variableselector-buttons";
                SwitchLayout.Attributes.Add("aria-label", _switchToCompactTxtScreenReader);
                //ucVariableOverview.Visible = true;
            }
            myLayoutCookie.Expires = DateTime.Now.AddDays(370);
            Response.Cookies.Add(myLayoutCookie);
        }



    }
}
