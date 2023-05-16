using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using log4net;
using System.IO;
using PCAxis.Paxiom;
using PCAxis.Search;
using PCAxis.Paxiom.Extensions;
using Px.Rdf;

namespace PXWeb.BackgroundWorker
{
    /// <summary>
    /// Describes the different status the background worker can have
    /// </summary>
    public enum StatusType
    {
        Running,
        ShuttingDown,
        Stopped
    }

    /// <summary>
    /// Class for doing background work in PX-Web
    /// </summary>
    public class PxWebBackgroundWorker
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PxWebBackgroundWorker));
        private static Thread _worker;
        
        /// <summary>
        /// Start work in a separate thread
        /// </summary>
        public static void Work(int sleepTime)
        {
            _run = true;
            _sleepTime = sleepTime;
            if (_worker == null)
            {
                _worker = new Thread(new ThreadStart(DoWork));
                _logger.Info("PX-Web background worker has been successfully started");
                _worker.Start();
            }
        }

        /// <summary>
        /// Wake up worker thread if it is sleeping
        /// </summary>
        public static void WakeUp()
        {
            if (_worker != null)
            {
                if (_worker.ThreadState == ThreadState.WaitSleepJoin)
                {
                    _worker.Interrupt();
                }
            }
        }

        /// <summary>
        /// Stop worker thread
        /// </summary>
        public static void Stop()
        {
            _run = false;

            if (_worker != null)
            {
                // Wake up if it is asleep
                if (_worker.ThreadState == ThreadState.WaitSleepJoin)
                {
                    _worker.Interrupt();
                }
            }
        }

        /// <summary>
        /// Restart worker thread
        /// </summary>
        public static void Restart()
        {
            _run = true;

            Work(_sleepTime);
        }

        #region "Public properties"

        private static bool _run = true;
        /// <summary>
        /// If worker shall run or not
        /// </summary>
        public static bool Run {
            get
            {
                return _run;
            }
            set
            { 
                _run = value; 
            }
        }

        private static int _sleepTime = 1;
        /// <summary>
        /// Time to sleep between work iterations in seconds
        /// </summary>
        public static int SleepTime { 
            get 
            { 
                return _sleepTime; 
            } 
            set 
            { 
                _sleepTime = value;
                if (_sleepTime == 0)
                {
                    _sleepTime = 10;
                }
            } 
        }

        private static string _activity;
        /// <summary>
        /// Status telling what the worker is doing right now
        /// </summary>
        public static string CurrentActivity { 
            get 
            {
                return _activity; 
            } 
            set 
            {
                _activity = value; 
            } 
        }

        /// <summary>
        /// Current status of the background worker process
        /// </summary>
        public static StatusType Status
        {
            get
            {
                if (_worker == null)
                {
                    return StatusType.Stopped;
                }
                else if (_run == false)
                {
                    return StatusType.ShuttingDown;
                }
                else
                {
                    return StatusType.Running;
                }
            }
        }

        #endregion

        /// <summary>
        /// Perform backround work
        /// </summary>
        private static void DoWork()
        {
            while (_run == true)
            {
                _logger.Info("PX-Web background worker iteration started");
                
                try
                {
                    if (_run == true)
                    {
                        CheckStatusOfDatabases();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("Error when checking indexes: " + ex.Message);
                }

                // In the future: Check for work configured in Web.config


                try
                {
                    if (_run == true)
                    {
                        _activity = "Sleeping";
                        _logger.Info("PX-Web background worker iteration ended - going to sleep");
                        Thread.Sleep(_sleepTime * 1000);
                    }
                }
                catch (ThreadInterruptedException e)
                {
                    _logger.Info("PX-Web background worker has been awakened");
                }
            }
            _worker = null;
            _activity = "-";
            _logger.Info("PX-Web background worker has been stopped");
        }

        /// <summary>
        /// Check if any search index wants to be indexed
        /// </summary>
        private static void CheckStatusOfDatabases()
        {
            try
            {
                _activity = "Checking databases";

                string path = GetDatabasePath();

                if (string.IsNullOrEmpty(path))
                {
                    return;
                }

                // All databases with settings must have a folder in the PX-databases folder containing a database.config file
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (DirectoryInfo dbDir in dir.GetDirectories())
                {
                    HandleSearchIndex(dbDir.Name);
                    HandleDcat(dbDir.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Check if search index shall be created for database (SearchIndex.Status = Waiting). If so create the search index
        /// </summary>
        /// <param name="database">Database id</param>
        public static void HandleSearchIndex(string database)
        {
            if (_run == false)
            {
                return;
            }
            
            PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);
            PXWeb.SearchIndexSettings searchIndex = (PXWeb.SearchIndexSettings)db.SearchIndex;

            _logger.InfoFormat("HandleSearchIndex called for database:{0}", database);
            if (searchIndex.Status == SearchIndexStatusType.WaitingCreate)
            {
                CreateSearchIndex(database);
            }
            else if (searchIndex.Status == SearchIndexStatusType.WaitingUpdate)
            {
                UpdateSearchIndex(database);
            }
            else
            {
                _logger.InfoFormat("No action taken for  database:{0}", database);
            }

        }

        /// <summary>
        /// Check if dcat file shall be created for database (Dcat.FileStatus = WaitingCreate). If so create the dcat file
        /// </summary>
        /// <param name="database">Database id</param>
        public static void HandleDcat(string database)
        {
            if (_run == false)
            {
                return;
            }

            PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);
            PXWeb.DcatSettings dcat = (PXWeb.DcatSettings)db.Dcat;

            _logger.InfoFormat("HandleDcat called for database:{0}", database);
            if (dcat.FileStatus == DcatStatusType.WaitingCreate)
            {
                CreateDcatFile(dcat.Database);
            }
            else
            {
                _logger.InfoFormat("No action taken for  database:{0}", database);
            }

        }

        private static string firstTwo(string s)
        {
            return s.Substring(0, 2);
        }

        /// <summary>
        /// Create new dcat file
        /// </summary>
        /// <param name="database"></param>
        private static void CreateDcatFile(string database)
        {
            _activity = "Creating dcat file for the " + database + " database";
            PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);
            PXWeb.DcatSettings dcat = (PXWeb.DcatSettings)db.Dcat;

            DcatStatusType startStatus = dcat.FileStatus;

            List<string> languages = new List<string>();
            string preferredLanguage = firstTwo(Settings.Current.General.Language.DefaultLanguage);
            foreach (LanguageSettings ls in Settings.Current.General.Language.SiteLanguages)
            {
                languages.Add(firstTwo(ls.Name));
            }
            string themeMapping = System.Web.Hosting.HostingEnvironment.MapPath("~/TMapping.json");
            string dbType = dcat.DatabaseType;
            string dbid;
            IFetcher fetcher;
            string databasepath = GetDatabasePath();

            string savePath = databasepath + dcat.Database + "/dcat-ap.xml";
            switch (dbType)
            {
                case "PX":
                    dbid = databasepath + dcat.Database + "/Menu.xml";
                    string localThemeMapping = databasepath + dcat.Database + "/TMapping.json";
                    if (File.Exists(localThemeMapping)) themeMapping = localThemeMapping;
                    fetcher = new PXFetcher(databasepath);
                    break;
                case "CNMM":
                    dbid = dcat.Database;
                    fetcher = new CNMMFetcher();
                    break;
                default:
                    return;
            }

            RdfSettings settings = new RdfSettings
            {
                BaseUri = dcat.BaseURI,

                BaseApiUrl = dcat.BaseApiUrl,

                PreferredLanguage = preferredLanguage,

                Languages = languages,

                CatalogTitle = dcat.CatalogTitle,
                CatalogDescription = dcat.CatalogDescription,

                PublisherName = dcat.Publisher,
                DBid = dbid,
                Fetcher = fetcher,
                LandingPageUrl = dcat.LandingPageUrl,
                License = dcat.License,
                ThemeMapping = themeMapping
            };

            try
            {
                dcat.FileStatus = DcatStatusType.Creating;
                db.Save();

                XML.WriteToFile(savePath, settings);

                _logger.Info("Dcat-file for the '" + database + "' database was created successfully");
                dcat.FileStatus = DcatStatusType.Created;
                dcat.FileUpdated = DateTime.Now.ToString(PXConstant.PXDATEFORMAT);
                db.Save();
            }
            catch (Exception ex)
            {
                _logger.Error("Error when creating dcat-file for the '" + database + "' database : " + ex.Message);
                dcat.FileStatus = startStatus;
                db.Save();
            }

            // Force reload of database settings
            db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);
        }

        /// <summary>
        /// Create new search index
        /// </summary>
        /// <param name="database"></param>
        private static void CreateSearchIndex(string database)
        {
            _activity = "Creating search index for the " + database + " database";

            bool success = true;
            PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);
            PXWeb.SearchIndexSettings searchIndex = (PXWeb.SearchIndexSettings)db.SearchIndex;

            // Get start values. If something goes wrong we may need to restore these values
            SearchIndexStatusType startStatus = searchIndex.Status;
            string startUpdated = searchIndex.IndexUpdated;

            searchIndex.Status = SearchIndexStatusType.Indexing;
            db.Save();

            try
            {
                foreach (LanguageSettings lang in PXWeb.Settings.Current.General.Language.SiteLanguages)
                {
                    DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(database);
                    if (dbi.HasLanguage(lang.Name))
                    {
                        if (SearchManager.Current.CreateIndex(database, lang.Name))
                        {
                            _logger.Info("Successfully created the " + database + " - " + lang.Name + " search index");
                        }
                        else
                        {
                            _logger.Error("Failed to create the " + database + " - " + lang.Name + " search index");
                            //success = false;
                            //return;
                        }
                    }
                }

                if (success)
                {
                    searchIndex.Status = SearchIndexStatusType.Indexed;
                    searchIndex.IndexUpdated = DateTime.Now.ToString(PXConstant.PXDATEFORMAT);
                    _logger.Info("Search index was successfully created for the '" + database + "' database");
                }
                else
                {
                    // Restore values...
                    searchIndex.Status = startStatus;
                    searchIndex.IndexUpdated = startUpdated;
                    _logger.Error("Failed to create search index for the '" + database + "' database");
                }

                db.Save();
            }
            catch (Exception ex)
            {
                _logger.Error("Error when creating search index for the '" + database + "' database : " + ex.Message);
                _logger.Error("Details", ex);

                // Restore values...
                searchIndex.Status = startStatus;
                searchIndex.IndexUpdated = startUpdated;
                db.Save();
            }

            // Force reload of database settings
            db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);
        }

        /// <summary>
        /// Update search index
        /// </summary>
        /// <param name="database"></param>
        private static void UpdateSearchIndex(string database)
        {
            _activity = "Updating search index for the " + database + " database";

            bool success = true;
            PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);
            PXWeb.SearchIndexSettings searchIndex = (PXWeb.SearchIndexSettings)db.SearchIndex;
            DateTime dateFrom;

            // Get start values. If something goes wrong we may need to restore these values
            SearchIndexStatusType startStatus = searchIndex.Status;
            string startUpdated = searchIndex.IndexUpdated;

            if (PxDate.IsPxDate(searchIndex.IndexUpdated))
            {
                dateFrom = PxDate.PxDateStringToDateTime(searchIndex.IndexUpdated);
            }
            else
            {
                dateFrom = DateTime.Now;
            }

            try
            {
                searchIndex.Status = SearchIndexStatusType.Indexing;
                db.Save();

                foreach (LanguageSettings lang in PXWeb.Settings.Current.General.Language.SiteLanguages)
                {
                    List<PCAxis.Search.TableUpdate> lst = searchIndex.UpdateMethod.GetUpdatedTables(dateFrom, "ssd_extern_test", lang.Name);

                    if (lst.Count > 0)
                    {
                        if (SearchManager.Current.UpdateIndex(database, lang.Name, lst))
                        {
                            _logger.Info("Successfully updated the " + database + " - " + lang.Name + " search index");
                        }
                        else
                        {
                            _logger.Error("Failed to update the " + database + " - " + lang.Name + " search index");
                            success = false;
                            return;
                        }
                    }
                }

                if (success)
                {
                    searchIndex.Status = SearchIndexStatusType.Indexed;
                    searchIndex.IndexUpdated = DateTime.Now.ToString(PXConstant.PXDATEFORMAT);
                    _logger.Info("Search index was successfully updated for the '" + database + "' database");
                }
                else
                {
                    // Restore values...
                    searchIndex.Status = startStatus;
                    searchIndex.IndexUpdated = startUpdated;
                    _logger.Error("Failed to update search index for the '" + database + "' database");
                }

                db.Save();
            }
            catch (Exception ex)
            {
                _logger.Error("Error when updating the search index for the '" + database + "' database : " + ex.Message);

                // Restore values...
                searchIndex.Status = startStatus;
                searchIndex.IndexUpdated = startUpdated;
                db.Save();
            }

            // Force reload of database settings
            db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);

        }

        /// <summary>
        /// Get the path to the databases directory
        /// </summary>
        /// <returns>Path to the databasese directory. If the path illegal in some way a empty string is returned</returns>
        private static string GetDatabasePath()
        {
            string path;

            if (string.IsNullOrEmpty(PXWeb.Settings.Current.General.Paths.PxDatabasesPath))
            {
                return "";
            }

            path = System.Web.Hosting.HostingEnvironment.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath);

            if (!Directory.Exists(path))
            {
                return "";
            }

            return path;
        }
    }
}