using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using PCAxis.Chart;
using PCAxis.Web.Controls;
using System.Web.Routing;
using PCAxis.Api;
using PCAxis.Search;
using PXWeb.BackgroundWorker;
using System.Collections.Generic;
using log4net;
using PX.Web.Interfaces.Cache;
using System.Runtime.Caching;
using System.Web.Http;
using PXWeb.API;
using Ninject;
using Ninject.Web.Common;
using PXWeb.Code.Management;

namespace PXWeb
{
    public class RouteInstance
    {
        public static IRouteExtender RouteExtender { get; set; }
        public static IPxUrlProvider PxUrlProvider { get; set; }   
    }

    public interface ICacheService
    {
        T Get<T>(string cacheKey) where T : class;
        void Set(string cacheKey, object obj);
        void ClearCache();
    }

    public class InMemoryCache : ICacheService, IPxCache
    {
        static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(InMemoryCache));
        private bool _isEnabled = true;
        private int _cacheExpirationInMinutes;
        private HashSet<string> _cacheKeys = new HashSet<string>();
        private bool inCacheCleanProcess = false;

        private Object lockObject = new Object();

        public bool DefaultEnabled
        {
            get
            {
                return true;
            }
        }

        public InMemoryCache(int cacheExpirationInMinutes)
        {
            _cacheExpirationInMinutes = cacheExpirationInMinutes;
        }

        public T Get<T>(string cacheKey) where T : class
        {
            if (_coherenceChecker != null)
            {
                if (!_coherenceChecker())
                {
                    //Clear the cache if it is not coherent
                    Clear();
                    return null;
                }
            }

            lock (lockObject)
            {
                if (!_isEnabled) return null;
                return MemoryCache.Default.Get(cacheKey) as T;
            }
        }
        public void Set(string cacheKey, object obj)
        {
            lock (lockObject)
            {
                if (!_isEnabled) return;
                if (inCacheCleanProcess) return;

                if (!_cacheKeys.Contains(cacheKey))
                {
                    _cacheKeys.Add(cacheKey);
                }

                MemoryCache.Default.Set(cacheKey, obj, DateTime.Now.AddMinutes(_cacheExpirationInMinutes));
            }
        }

        public void ClearCache()
        {
            lock (lockObject)
            {
                if (inCacheCleanProcess) return;
                inCacheCleanProcess = true;

                try
                {
                    foreach (string cacheKey in _cacheKeys)
                    {
                        MemoryCache.Default.Remove(cacheKey);
                    }

                    _cacheKeys.Clear();
                }
                finally
                {
                    inCacheCleanProcess = false;
                    _logger.Info("Cache cleared");
                }
            }
        }

        public bool IsEnabled()
        {
            return _isEnabled;
        }

        public void Clear()
        {
            ClearCache();
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        private Func<bool> _coherenceChecker;
        public void SetCoherenceChecker(Func<bool> coherenceChecker)
        {
            _coherenceChecker = coherenceChecker;
        }
    }

    public class Global : System.Web.HttpApplication
    {

        static log4net.ILog _logger = log4net.LogManager.GetLogger("Global");
        private ICacheService _metaCacheService = null;
        private IPxCache _metaPxCache = null;
        private IPxCacheController _cacheController = null;

        public static void InitializeChartSettings(PCAxis.Chart.ChartSettings settings)
        {
            settings.AxisFontSize = Settings.Current.Features.Charts.Font.AxisSize;
            //settings.ChartType
            settings.Colors = Settings.Current.Features.Charts.Colors.ToList();
            settings.CurrentCulture = PCAxis.Web.Core.Management.LocalizationManager.CurrentCulture;
            settings.FontName = Settings.Current.Features.Charts.Font.Name;
            settings.Guidelines = ChartSettings.GuidelinesType.None;
            if (Settings.Current.Features.Charts.Guidelines.Horizontal) settings.Guidelines = ChartSettings.GuidelinesType.Horizontal;
            if (Settings.Current.Features.Charts.Guidelines.Vertical) settings.Guidelines |= ChartSettings.GuidelinesType.Vertical;
            settings.GuidelinesColor = Settings.Current.Features.Charts.Guidelines.Color;
            settings.Height = Settings.Current.Features.Charts.Height;
            settings.LabelOrientation = Settings.Current.Features.Charts.LabelOrientation;
            settings.LegendFontSize = Settings.Current.Features.Charts.Legend.FontSize;
            settings.LegendHeight = Settings.Current.Features.Charts.Legend.Height;
            settings.LineThickness = Settings.Current.Features.Charts.LineThickness;
            settings.Logotype = Settings.Current.Features.Charts.Logotype;            
            settings.ShowLegend = Settings.Current.Features.Charts.Legend.Visible;
            settings.TimeSortOrder = Settings.Current.Features.Charts.TimeSortOrder;
            //settings.Title = PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Meta.Title;
            settings.TitleFontSize = Settings.Current.Features.Charts.Font.TitleSize;
            settings.Width = Settings.Current.Features.Charts.Width;
            settings.ShowSource = Settings.Current.Features.Charts.ShowSource;
            settings.ShowLogo = Settings.Current.Features.Charts.ShowLogo;
            settings.BackgroundColorGraphs = Settings.Current.Features.Charts.BackgroundColorGraphs;
            settings.LineThicknessPhrame = Settings.Current.Features.Charts.LineThicknessPhrame;
            settings.LogotypePath = Settings.Current.General.Paths.ImagesPath;
            settings.LineColorPhrame = Settings.Current.Features.Charts.LineColorPhrame;
            settings.BackgroundColor = Settings.Current.Features.Charts.BackgroundColor;
            settings.BackgroundAlpha = Settings.Current.Features.Charts.BackgroundAlpha;
            settings.ChartAlpha = Settings.Current.Features.Charts.ChartAlpha;
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            _logger.Info(" === Application start ===");

            //Trigger reading of settings file
            Settings s = PXWeb.Settings.Current;

            //Trigger reading of databases
            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase("");

            PCAxis.Web.Controls.ChartManager.SettingsInitializer = InitializeChartSettings;

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CnmmDatabaseRoot"]))
            {
                CnmmDatabaseRootHelper.IsRooted = true;
                CnmmDatabaseRootHelper.DatabaseRoot = ConfigurationManager.AppSettings["CnmmDatabaseRoot"];
            }
            else
            {
                CnmmDatabaseRootHelper.IsRooted = false;
            }

            //Set if strict check of groupings shall be performed or not
            PCAxis.Paxiom.GroupRegistry.GetRegistry().Strict = PXWeb.Settings.Current.General.Global.StrictAggregationCheck;
            //Load aggregations
            PCAxis.Paxiom.GroupRegistry.GetRegistry().LoadGroupingsAsync();

            if (s.Features.General.ApiEnabled)
            {
                RouteManager.AddApiRoute();
            }
            
            if (ConfigurationManager.AppSettings["CacheServiceExpirationInMinutes"] != null)
            {
                int cacheServiceExpirationInMinutes = int.Parse(ConfigurationManager.AppSettings["CacheServiceExpirationInMinutes"]);

                if (cacheServiceExpirationInMinutes > 0)
                {
                    var cacheService = new InMemoryCache(cacheServiceExpirationInMinutes);
                    _metaCacheService = cacheService;
                    _metaPxCache = cacheService;
                }

                PXWeb.Management.PxContext.CacheService = _metaCacheService;
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RouteExtender"]))
            {
                RouteInstance.RouteExtender = Activator.CreateInstance(Type.GetType(ConfigurationManager.AppSettings["RouteExtender"])) as IRouteExtender;
                RouteInstance.RouteExtender.MetaCacheService = _metaCacheService;
                RouteInstance.RouteExtender.Db = PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DefaultDatabase;
                RouteInstance.RouteExtender.AddSavedQueryRoute(RouteTable.Routes);
                RouteInstance.RouteExtender.RegisterCustomRoutes(RouteTable.Routes);
                RouteInstance.RouteExtender.HomeSitePage = ConfigurationManager.AppSettings["HomeSitePage"] ?? "";
                RouteInstance.RouteExtender.DefaultRedirectPage = ConfigurationManager.AppSettings["DefaultRedirectPage"] ?? "";

                RouteInstance.PxUrlProvider = RouteInstance.RouteExtender.PxUrlProvider;
                PCAxis.Web.Core.Management.LinkManager.CreateLinkMethod = new PCAxis.Web.Core.Management.LinkManager.LinkMethod(RouteInstance.RouteExtender.CreateLink);
            }
            else
            {
                RouteManager.AddDefaultGotoRoute();
                RouteManager.AddSavedQueryRoute();
                RegisterRoutes(RouteTable.Routes);
                RouteInstance.PxUrlProvider = new PxUrlProvider();

                if (PXWeb.Settings.Current.Features.General.UserFriendlyUrlsEnabled)
                {
                    PCAxis.Web.Core.Management.LinkManager.CreateLinkMethod = new PCAxis.Web.Core.Management.LinkManager.LinkMethod(PXWeb.UserFriendlyLinkManager.CreateLink);
                }
            }

            //Initialize Index search
            SearchManager.Current.Initialize(PXWeb.Settings.Current.General.Paths.PxDatabasesPath, 
                                            new PCAxis.Search.GetMenuDelegate(PXWeb.Management.PxContext.GetMenuAndItem),
                                            PXWeb.Settings.Current.Features.Search.CacheTime,
                                            PXWeb.Settings.Current.Features.Search.DefaultOperator);

            PCAxis.Query.SavedQueryManager.StorageType = PXWeb.Settings.Current.Features.SavedQuery.StorageType;
            PCAxis.Query.SavedQueryManager.Reset();

            InitializeCacheController();

            if (PXWeb.Settings.Current.Features.General.BackgroundWorkerEnabled)
            {                
                //Start PX-Web background worker
                PxWebBackgroundWorker.Work(PXWeb.Settings.Current.Features.BackgroundWorker.SleepTime);
            }

        }

        protected void Session_Start(object sender, EventArgs e)
        {
           // InitializeChartSettings();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            MDC.Set("addr", Request.UserHostAddress);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            string errCode = "";

            Exception ex = Server.GetLastError();
            if (ex is System.Threading.ThreadAbortException)
            {
                return;
            }
            else if (ex is HttpException)
            {
                errCode = ((HttpException)ex).GetHttpCode().ToString();
            }

            _logger.Error(ex);
            //Check if the error is caused by illegal characters in parameter
            if (ex is PCAxis.Web.Core.Exceptions.InvalidQuerystringParameterException || ex.InnerException is PCAxis.Web.Core.Exceptions.InvalidQuerystringParameterException)
            {
                Server.Transfer("~/ErrorGeneral.aspx");
            }
            else if (!string.IsNullOrEmpty(errCode))
            {
                Server.Transfer("~/ErrorGeneral.aspx?errcode=" + errCode);
            }


        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Stop background worker
            PxWebBackgroundWorker.Run = false;
            ApplicationShutdownReason shutdownReason = System.Web.Hosting.HostingEnvironment.ShutdownReason;
            _logger.InfoFormat("ShutdownReason: {0}", shutdownReason);
            _logger.Info(" === Application end ===");
        }

        void RegisterRoutes(RouteCollection routes)
        {
            //    string routePrefix = ConfigurationManager.AppSettings["routePrefix"];
            //    if (routePrefix == null)
            //        throw new Exception("No route prefix set up in app config");

            //    routes.Add(new Route
            //    (
            //         routePrefix + "{language}/{*path}"
            //         , new SSDRouteHandler()
            //    ));

            

            RouteTable.Routes.MapPageRoute("DefaultRoute",
                                           PxUrl.PX_START + "/",
                                           "~/Default.aspx");
            RouteTable.Routes.MapPageRoute("LangRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/",
                                           "~/Default.aspx");
            RouteTable.Routes.MapPageRoute("DbRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/",
                                           "~/Menu.aspx");
            RouteTable.Routes.MapPageRoute("DbSearchRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" + 
                                            PxUrl.VIEW_SEARCH + "/",
                                           "~/Search.aspx");
            RouteTable.Routes.MapPageRoute("DbPathRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" +
                                           "{" + PxUrl.PATH_KEY + "}/",
                                           "~/Menu.aspx");
            RouteTable.Routes.MapPageRoute("SelectionRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" +
                                           "{" + PxUrl.PATH_KEY + "}/" +
                                           "{" + PxUrl.TABLE_KEY + "}/",
                                           "~/Selection.aspx");
            //RouteTable.Routes.MapPageRoute("GotoRoute",
            //                               PxUrl.PX_GOTO + "/" +
            //                               "{" + PxUrl.LANGUAGE_KEY + "}/" +
            //                               "{" + PxUrl.DB_KEY + "}/" +
            //                               "{" + PxUrl.TABLE_KEY + "}/",
            //                               "~/Goto.ashx");
            RouteTable.Routes.MapPageRoute("SelectionInformationRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" +
                                           "{" + PxUrl.PATH_KEY + "}/" +
                                           "{" + PxUrl.TABLE_KEY + "}/" +
                                           PxUrl.VIEW_INFORMATION_IDENTIFIER + "/",
                                           "~/InformationSelection.aspx");
            RouteTable.Routes.MapPageRoute("SelectionTipsRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" +
                                           "{" + PxUrl.PATH_KEY + "}/" +
                                           "{" + PxUrl.TABLE_KEY + "}/" +
                                           PxUrl.VIEW_TIPS_IDENTIFIER + "/",
                                           "~/MarkingTips.aspx");
            RouteTable.Routes.MapPageRoute("SelectionFootnotesRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" +
                                           "{" + PxUrl.PATH_KEY + "}/" +
                                           "{" + PxUrl.TABLE_KEY + "}/" +
                                           PxUrl.VIEW_FOOTNOTES_IDENTIFIER + "/",
                                           "~/FootnotesSelection.aspx");
            RouteTable.Routes.MapPageRoute("TablePresentationRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" +
                                           "{" + PxUrl.PATH_KEY + "}/" +
                                           "{" + PxUrl.TABLE_KEY + "}/" +
                                           PxUrl.VIEW_TABLE_IDENTIFIER + "/" +
                                           "{" + PxUrl.LAYOUT_KEY + "}/",
                                           "~/Table.aspx");
            RouteTable.Routes.MapPageRoute("ChartPresentationRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" +
                                           "{" + PxUrl.PATH_KEY + "}/" +
                                           "{" + PxUrl.TABLE_KEY + "}/" +
                                           PxUrl.VIEW_CHART_IDENTIFIER + "/" +
                                           "{" + PxUrl.LAYOUT_KEY + "}/",
                                           "~/Chart.aspx");
            RouteTable.Routes.MapPageRoute("InformationPresentationRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" +
                                           "{" + PxUrl.PATH_KEY + "}/" +
                                           "{" + PxUrl.TABLE_KEY + "}/" +
                                           PxUrl.VIEW_INFORMATION_IDENTIFIER + "/" +
                                           "{" + PxUrl.LAYOUT_KEY + "}/",
                                           "~/InformationPresentation.aspx");
            RouteTable.Routes.MapPageRoute("SortedTablePresentationRoute",
                                           PxUrl.PX_START + "/" +
                                           "{" + PxUrl.LANGUAGE_KEY + "}/" +
                                           "{" + PxUrl.DB_KEY + "}/" +
                                           "{" + PxUrl.PATH_KEY + "}/" +
                                           "{" + PxUrl.TABLE_KEY + "}/" +
                                           PxUrl.VIEW_SORTEDTABLE_IDENTIFIER + "/" +
                                           "{" + PxUrl.LAYOUT_KEY + "}/",
                                           "~/DataSort.aspx");
            
            RouteTable.Routes.MapHttpRoute(name: "CacheApi", routeTemplate: "api/admin/v1/{controller}");
            RouteTable.Routes.MapHttpRoute(name: "MenuApi", routeTemplate: "api/admin/v1/{controller}/{database}");
            RouteTable.Routes.MapHttpRoute(name: "DcatApi", routeTemplate: "api/admin/v1/{controller}/{databaseType}/{database}");

        }

        /// <summary>
        /// Initialize the cache controller that handles all of the PX caches
        /// </summary>
        private void InitializeCacheController()
        {
            //IPxCacheController controller;
            string strCacheController = ConfigurationManager.AppSettings["pxCacheController"]; // Do we have a customized cache controller?

            if (string.IsNullOrEmpty(strCacheController))
            {
                _cacheController = new PXWeb.Management.CacheController(); // Use the default cache controller
            }
            else
            {
                try
                {
                    var typeString = strCacheController;
                    var parts = typeString.Split(',');
                    var typeName = parts[0].Trim();
                    var assemblyName = parts[1].Trim();
                    _cacheController = (IPxCacheController)Activator.CreateInstance(assemblyName, typeName).Unwrap(); // Use the customized cache controller
                }
                catch (Exception)
                {
                    _cacheController = new PXWeb.Management.CacheController();
                }
            }

            List<IPxCache> lstCache = new List<IPxCache>();

            // Add all PX caches to the list
            lstCache.Add(PXWeb.Management.SavedQueryPaxiomCache.Current);
            lstCache.Add(PCAxis.Api.ApiCache.Current);

            if (_metaPxCache != null)
            {
                lstCache.Add(_metaPxCache);
            }

            _cacheController.Initialize(lstCache);
            PXWeb.Management.PxContext.CacheController = _cacheController;
        }
    }
}
