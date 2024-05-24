using log4net;
using PCAxis.Menu;
using PCAxis.Web.Core;
using PCAxis.Web.Core.Enums;
using PCAxis.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;

namespace PXWeb
{
    public partial class PxWeb : System.Web.UI.MasterPage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PxWeb));

        public string HtmlLang
        {
            get
            {
                return LocalizationManager.GetTwoLetterLanguageCode();
            }
        }

        public string HeadTitle
        {
            get
            {
                return LiteralTitle.Text;
            }
            set
            {
                LiteralTitle.Text = value;
            }
        }

        public enum ModalDialogType
        {
            Information,
            Footnotes
        }

        private string _footertext = "";
        private string _imagesPath = "";
        private string _logoPath = "";

        public string FooterText
        {
            get
            {
                return _footertext;
            }
            set
            {
                _footertext = value;
            }
        }

        public string ImagesPath
        {
            get
            {
                return _imagesPath;
            }
        }
        public string OfficialStatisticsImage
        {
            get;
            set;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!PXWeb.Settings.Current.Selection.StandardApplicationHeadTitle)
            {
                HeadTitle = Server.HtmlEncode(GetLocalizedString("PxWebApplicationTitle"));
            }

            if (DoNotUseBreadCrumb())
            {
                Page.Controls.Remove(this.breadcrumb1);
            }

            //Add eventhandlers
            LinkManager.RegisterEnsureQueries(new EnsureQueriesEventHandler(LinkManager_EnsureQueries));

            if (PxUrlObj.Language != null)
            {
                if (!(LocalizationManager.CurrentCulture.Name == PxUrlObj.Language))
                {
                    if (PxUrlObj.Database != null)
                    {
                        DatabaseInfo dbi = null;
                        dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(PxUrlObj.Database);

                        if (dbi.Type == DatabaseType.CNMM)
                        {
                            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;
                            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel = null;
                        }
                    }
                    LocalizationManager.ChangeLanguage(PxUrlObj.Language);
                }
            }
            else
            {
                string lang = PXWeb.Settings.Current.General.Language.DefaultLanguage;
                LocalizationManager.ChangeLanguage(lang);

                List<LinkManager.LinkItem> linkItems = new List<LinkManager.LinkItem>();
                linkItems.Add(new LinkManager.LinkItem(PxUrl.LANGUAGE_KEY, lang));

                //Replaced Request.Url.AbsolutePath with Request.AppRelativeCurrentExecutionFilePath
                //so that the links will be right even if the site is running without UserFriendlyURL
                string url = PCAxis.Web.Core.Management.LinkManager.CreateLink(Request.AppRelativeCurrentExecutionFilePath, linkItems.ToArray());
                Response.Redirect(url);
            }

            ////Add eventhandlers
            //LinkManager.EnsureQueries += new LinkManager.EnsureQueriesEventHandler(LinkManager_EnsureQueries);
            _imagesPath = PXWeb.Settings.Current.General.Paths.ImagesPath;
            _logoPath = PXWeb.Settings.Current.General.Site.LogoPath;
            LoadPageContent();

            if (!IsPostBack)
            {
                InitializeLogoLink();

                if (!DoNotUseBreadCrumb())
                {
                    InitializeBreadcrumb();
                }


                if (PXWeb.Settings.Current.Navigation.ShowNavigationFlow)
                    InitializeNavigationFlow();
            }

            if (!DoNotUseBreadCrumb())
            {
                breadcrumb1.GetMenu = GetMenu;
            }

            //navigationFlowControl.GetMenu = GetMenu;
        }

        private bool DoNotUseBreadCrumb()
        {
            if (RouteInstance.RouteExtender == null) return false;
            return !RouteInstance.RouteExtender.ShowBreadcrumb();
        }

        /// <summary>
        /// Page load - set private properties and page content
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            ToTheTopButtonLiteralText.Text = GetLocalizedString("PxWebToTheTopButtonLiteralText");
            SkipToMain.Text = GetLocalizedString("PxWebSkipToMainContentLinkText");
            SkipToMain.Attributes.Add("aria-label", GetLocalizedString("PxWebSkipToMainContentLinkScreenReader"));
            SkipToMain.Attributes.Add("href", GetLocalizedString("#pxcontent"));

            if (PXWeb.Settings.Current.General.Global.ShowInformationTypes.OfficialStatistics)
            {
                if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null && PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.OfficialStatistics)
                {
                    OfficialStatisticsImage = "";
                    if (!string.IsNullOrWhiteSpace(PxUrlObj.Language))
                    {
                        string img = $"official_statistics_{PxUrlObj.Language}.svg";
                        OfficialStatisticsImage = ResolveUrl(System.IO.Path.Combine(PXWeb.Settings.Current.General.Paths.ImagesPath, img));
                    }
                }
            }

        }

        /// <summary>
        /// Set database name as H1 text
        /// </summary>
        public void SetH1TextDatabase()
        {
            if (!string.IsNullOrEmpty(PxUrlObj.Database) && !string.IsNullOrEmpty(PxUrlObj.Language))
            {
                DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(PxUrlObj.Database);

                if (dbi != null)
                {
                    lblH1Title.Text = dbi.GetDatabaseName(PxUrlObj.Language);
                }
            }

        }

        /// <summary>
        /// Set menu level as H1 text
        /// </summary>
        public void SetH1TextMenuLevel()
        {
            if (PXWeb.Settings.Current.General.Site.MainHeaderForTables == MainHeaderForTablesType.StatisticArea)
            {
                if (!string.IsNullOrEmpty(PxUrlObj.Path))
                {
                    PCAxis.Menu.Item itm = GetMenu(PxUrlObj.Path);
                    if (itm != null && !string.IsNullOrEmpty(itm.Text))
                    {
                        lblH1Title.Text = itm.Text;
                    }
                }
            }
            else
            {
                lblH1Title.Visible = false;
            }
        }

        /// <summary>
        /// Set custom H1 text 
        /// </summary>
        /// <param name="text"></param>
        public void SetH1TextCustom(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                lblH1Title.Text = text;
            }
        }

        /// <summary>
        /// Page unload - remove eventhandler for LinkManager.EnsureQueries
        /// </summary>
        protected void Page_Unload(object sender, EventArgs e)
        {

            LinkManager.UnregisterEnsureQueries(LinkManager_EnsureQueries);
        }


        /// <summary>
        /// Set page content
        /// </summary>
        private void LoadPageContent()
        {

            //Logo
            if (!PXWeb.Settings.Current.Selection.StandardApplicationHeadTitle)
            {
                HeadTitle = Server.HtmlEncode(GetLocalizedString("PxWebApplicationTitle"));
            }

            imgSiteLogo.Src = Path.Combine(_imagesPath, _logoPath);
            imgSiteLogo.Alt = GetLocalizedString("PxWebLogoAlt");
            if (_logoPath.Length < 5)
            {
                imgSiteLogo.Visible = false;
            }

            //Application name
            litAppName.Text = Server.HtmlEncode(GetLocalizedString("PxWebApplicationName"));

            //Languages

            string html = "";
            foreach (LanguageSettings lang in PXWeb.Settings.Current.General.Language.SiteLanguages)
            {
                if (!(lang.Name == PxUrlObj.Language))
                {
                    html += GetChangeToLanguageLink(lang.Name);
                }

            }
            //For testing CSS when more than one language: html += GetChangeToLanguageLink("sv");
            ChangeLanguageLiteral.Text = html;

            //Footer
            lblFooterText.Text = _footertext;
        }

        private string GetChangeToLanguageLink(string langName)
        {

            List<LinkManager.LinkItem> linkItems = new List<LinkManager.LinkItem>();
            linkItems.Add(new LinkManager.LinkItem("px_language", langName));
            if (!string.IsNullOrEmpty(PxUrlObj.Layout))
            {
                linkItems.Add(new LinkManager.LinkItem(PxUrl.LAYOUT_KEY, PxUrlObj.Layout));
            }
            //Replaced Request.Url.AbsolutePath with Request.AppRelativeCurrentExecutionFilePath
            //so that the links will be right even if the site is running without UserFriendlyURL
            string langUrl = PCAxis.Web.Core.Management.LinkManager.CreateLink(Request.AppRelativeCurrentExecutionFilePath, false, linkItems.ToArray());

            string langText = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebChangeToThisLanguage", new CultureInfo(langName));
            return String.Format("<div class=\"pxweb-link\"> <a class=\"px-change-lang\" href=\"{0}\"> <span lang=\"{2}\" class=\"link-text px-change-lang\">{1}</span></a> </div> ", langUrl, Server.HtmlEncode(langText), langName);
        }

        /// <summary>
        /// Eventhandler for LinkManager.EnsureQueries (calles from LinkManager.CreateLink) 
        /// that adds dictionaryitems to add to the created link.
        /// </summary>
        /// <param name="queries"></param>
        protected void LinkManager_EnsureQueries(object sender, EnsureQueriesEventArgs e)
        {
            Dictionary<string, string> queries = e.Queries;
            AddToQueries(queries, "px_db"); // Identifies selected PX- or SQL-database
            AddToQueries(queries, "px_language");
            AddToQueries(queries, "px_path"); // path within database 
            AddToQueries(queries, "px_tableid"); // Identifies selected PX-file or SQL-table
            AddToQueries(queries, "rxid");
        }


        protected void AddToQueries(Dictionary<string, string> queries, string key)
        {
            if (Page.RouteData.Values[key] != null)
            {
                if (queries.ContainsKey(key))
                {
                    queries[key] = ValidationManager.GetValue(Page.RouteData.Values[key].ToString());
                }
                else
                {
                    queries.Add(key, ValidationManager.GetValue(Page.RouteData.Values[key].ToString()));
                }
            }
            else if (QuerystringManager.GetQuerystringParameter(key) != null)
            {
                if (queries.ContainsKey(key))
                {
                    queries[key] = QuerystringManager.GetQuerystringParameter(key);
                }
                else
                {
                    queries.Add(key, QuerystringManager.GetQuerystringParameter(key));
                }
            }
        }

        /// <summary>
        /// Get text in the currently selected language
        /// </summary>
        /// <param name="key">Key identifying the string in the language file</param>
        /// <returns>Localized string</returns>
        public string GetLocalizedString(string key)
        {
            string lang = LocalizationManager.CurrentCulture.Name;
            return PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(key, new CultureInfo(lang));
        }

        /// <summary>
        /// Set breadcrumb
        /// </summary>
        /// <param name="mode">Breadcrumb mode</param>
        /// <param name="subpage">Optional parameter breadcrumb name</param>
        public void SetBreadcrumb(PCAxis.Web.Controls.Breadcrumb.BreadcrumbMode mode, string subpage = "")
        {
            if (!DoNotUseBreadCrumb())
            {
                breadcrumb1.Update(mode, subpage);
            }
        }

        public void SetNavigationFlowMode(PCAxis.Web.Controls.NavigationFlow.NavigationFlowMode mode)
        {
            if (PXWeb.Settings.Current.Navigation.ShowNavigationFlow)
            {
                navigationFlowControl.UpdateNavigationFlowMode(mode);
            }
        }

        /// <summary>
        /// If the Navigationflow 
        /// </summary>
        /// <param name="show"></param>
        public void SetNavigationFlowVisibility(Boolean show)
        {
            navigationFlowControl.Visible = show;
        }


        private void InitializeNavigationFlow()
        {
            IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
            DatabaseInfo dbi = null;

            navigationFlowControl.MenuPage = "Menu.aspx";
            navigationFlowControl.SelectionPage = "Selection.aspx";
            navigationFlowControl.TablePathParam = "px_path";
            navigationFlowControl.LayoutParam = "layout";

            if (string.IsNullOrEmpty(url.Database))
            {
                return;
            }

            dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(url.Database);

            navigationFlowControl.DatabaseId = dbi.Id;
            navigationFlowControl.DatabaseName = dbi.GetDatabaseName(LocalizationManager.CurrentCulture.TwoLetterISOLanguageName);

            if (string.IsNullOrEmpty(url.Path))
            {
                return;
            }


            if (dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM)
            {
                navigationFlowControl.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.CNMM;
            }
            else
            {
                navigationFlowControl.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX;
            }

            navigationFlowControl.TablePath = System.Web.HttpUtility.UrlDecode(url.Path);

            if (string.IsNullOrEmpty(url.Table))
            {
                return;
            }

            if ((dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM) && (!url.Table.Contains(":")))
            {
                navigationFlowControl.Table = url.Database + ":" + url.Table;
            }
            else
            {
                navigationFlowControl.Table = url.Table;
            }
        }

        private void InitializeLogoLink()
        {
            IPxUrl url = RouteInstance.PxUrlProvider.Create(null);

            if (!string.IsNullOrEmpty(url.Database))
            {
                try
                {
                    IHomepageSettings homepage = PXWeb.Settings.Current.Database[url.Database].Homepages.GetHomepage(url.Language);

                    if (string.IsNullOrWhiteSpace(homepage.Url))
                    {
                        return;
                    }

                    if (homepage.IsExternal)
                    {
                        LogoLink.NavigateUrl = homepage.Url;
                    }
                    else
                    {
                        LogoLink.NavigateUrl = LinkManager.CreateLink(homepage.Url);
                    }
                }
                catch (Exception e)
                {
                    log.Debug("url.Database = " + url.Database + ", url.Language = " + url.Language);
                    log.Error("The error.", e);
                    throw e;
                }
            }
            else
            {
                LogoLink.NavigateUrl = LinkManager.CreateLink("Default.aspx");
            }
        }

        private void InitializeBreadcrumb()
        {
            IPxUrl url = RouteInstance.PxUrlProvider.Create(null);
            DatabaseInfo dbi = null;

            if (!string.IsNullOrEmpty(url.Database))
            {

                try
                {
                    IHomepageSettings homepage = PXWeb.Settings.Current.Database[url.Database].Homepages.GetHomepage(url.Language);
                    breadcrumb1.HomePageIsExternal = homepage.IsExternal;
                    breadcrumb1.HomePage = homepage.Url;
                    if (PXWeb.Settings.Current.Menu.MenuMode == MenuModeType.TreeViewWithoutFiles)
                    {
                        breadcrumb1.UseTableList = true;
                    }
                    else
                    {
                        breadcrumb1.UseTableList = false;
                    }
                }
                catch (KeyNotFoundException e)
                {
                    log.Debug("url.Database = " + url.Database + ", url.Language = " + url.Language);
                    log.Debug("Getting KeyNotFoundException for url.Database. Possible keys are:");
                    foreach (string dbid in PXWeb.Settings.Current.Database.Keys)
                    {
                        log.Debug("dbid = " + dbid);
                    }
                    log.Debug("That all, folks!");
                    log.Error("The error.", e);
                    throw e;
                }

            }
            else
            {
                breadcrumb1.HomePageIsExternal = false;
                breadcrumb1.HomePage = "Default.aspx";
            }

            breadcrumb1.HomePageName = GetLocalizedString("PxWebHome");
            breadcrumb1.HomePageImage = true;
            breadcrumb1.MenuPage = "Menu.aspx";
            breadcrumb1.SelectionPage = "Selection.aspx";
            breadcrumb1.TablePathParam = "px_path";
            breadcrumb1.LayoutParam = "layout";

            if (string.IsNullOrEmpty(url.Database))
            {
                return;
            }

            dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(url.Database);

            breadcrumb1.DatabaseId = dbi.Id;
            breadcrumb1.DatabaseName = dbi.GetDatabaseName(LocalizationManager.CurrentCulture.TwoLetterISOLanguageName);

            if (string.IsNullOrEmpty(url.Path))
            {
                return;
            }

            //MenuPath path;
            //if (dbi.Type == DatabaseType.CNMM)
            //{
            //    path = MenuPathFactory.Create(LinkType.Table);
            //}
            //else
            //{
            //    path = MenuPathFactory.Create(LinkType.PX);
            //}

            ////string tablePath = url.Path.Replace("___", "/");
            //string tablePath = path.Decompress(url.Path);
            //breadcrumb1.TablePath = tablePath;

            if (dbi.Type == DatabaseType.CNMM)
            {
                breadcrumb1.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.CNMM;
            }
            else
            {
                breadcrumb1.DatabaseType = PCAxis.Web.Core.Enums.DatabaseType.PX;
            }

            breadcrumb1.TablePath = System.Web.HttpUtility.UrlDecode(url.Path);

            if (string.IsNullOrEmpty(url.Table))
            {
                return;
            }

            if ((dbi.Type == PCAxis.Web.Core.Enums.DatabaseType.CNMM) && (!url.Table.Contains(":")))
            {
                breadcrumb1.Table = url.Database + ":" + url.Table;
            }
            else
            {
                breadcrumb1.Table = url.Table;
            }
        }


        private IPxUrl _pxUrl = null;

        private IPxUrl PxUrlObj
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

        /// <summary>
        /// Gets the menu object
        /// </summary>
        /// <returns>returns the menu object</returns>
        private Item GetMenu(string nodeId)
        {
            //Checks that the necessary parameters are present
            if (String.IsNullOrEmpty(PxUrlObj.Database))
            {
                //if parameters is missing redirect to the start page
                Response.Redirect("Default.aspx", false);
            }

            try
            {
                string db = PxUrlObj.Database;
                return PXWeb.Management.PxContext.GetMenuItem(db, nodeId);
            }
            catch (Exception e)
            {
                log.Error("An error occured in GetMenu(string nodeId). So it returns null after logging this message.", e);
                return null;
            }
        }

        private string _displayVersion = null;

        public string DisplayVersion
        {
            get
            {
                if (_displayVersion == null)
                {
                    var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    var displayVersion = version.Major + "." + version.Minor + "." + version.Build;

                    if (version.Revision > 0)
                    {
                        displayVersion += "." + version.Revision;
                    }

                    _displayVersion = displayVersion;
                }

                return _displayVersion;
            }
        }


    }
}
