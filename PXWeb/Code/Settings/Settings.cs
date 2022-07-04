using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace PXWeb
{
    /// <summary>
    /// Root class for reading and writing system settings in PX-Web. Implemented as a threadsafe Singleton object.
    /// </summary>
    public class Settings
    {
        #region "Private fields"

        /// <summary>
        /// The Settings object for PX-Web
        /// </summary>
        private static volatile Settings _settings;

        /// <summary>
        /// Object to control creation of the _settings object in a multithreaded environment 
        /// </summary>
        private static object currentSettingsLock = new Object();

        /// <summary>
        /// Settings object used from the administration
        /// </summary>
        private static volatile Settings _newSettings;

        /// <summary>
        /// Object to control creation of the _newSettings object in a multithreaded environment 
        /// </summary>
        private static object newSettingsLock = new Object();

        /// <summary>
        /// Path to the settings file
        /// </summary>
        private static string _path;

        /// <summary>
        /// General settings
        /// </summary>
        private GeneralSettings _generalSettings;

        /// <summary>
        /// Menu settings
        /// </summary>
        private MenuSettings _menuSettings;

        /// <summary>
        /// Selection settings
        /// </summary>
        private SelectionSettings _selectionSettings;

        /// <summary>
        /// Presentation settings
        /// </summary>
        private PresentationSettings _presentationSettings;

        /// <summary>
        /// Features settings
        /// </summary>
        private FeaturesSettings _featuresSettings;

        /// <summary>
        /// Navigation settings
        /// </summary>
        private NavigationSettings _navigationSettings;


        /// <summary>
        /// Dcat settings
        /// </summary>
        private DcatSettings _dcatSettings;

        /// <summary>
        /// Dictionary with settings per database 
        /// </summary>
        private Dictionary<string, IDatabaseSettings> _databases;

        /// <summary>
        /// Dictionary with times when the database settings files were updated.
        /// Used to determine if the settings for a database must be reloaded or not (read the database.config file again).
        /// </summary>
        private Dictionary<string, DateTime> _dbFileUpdate;

        /// <summary>
        /// Log object
        /// </summary>
        private static log4net.ILog _logger;
        
        #endregion

        #region "Public static methods"

        /// <summary>
        /// Called from Administration when settings are being saved. Performs a threadsafe instatiation of the NewSettings object.
        /// </summary>
        public static bool BeginUpdate()
        {
            if (_newSettings != null)
            {
                //Somebody else is saving right now...
                return false;
            }
            else
            {
                // Assure that only one thread at a time can instantiate the _newSettings object
                lock (newSettingsLock)
                {
                    if (_newSettings == null)
                    {
                        _newSettings = new Settings(GetSettingsPath());
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Save settings to the settings file
        /// </summary>
        public static void Save()
        {
            string xpath;
            XmlNode node;

            //Load the existing settings file
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(_path);

            xpath = "/settings/general";
            node = xdoc.SelectSingleNode(xpath);
            ((GeneralSettings)_newSettings.General).Save(node);

            xpath = "/settings/menu";
            node = xdoc.SelectSingleNode(xpath);
            ((MenuSettings)_newSettings.Menu).Save(node);

            xpath = "/settings/selection";
            node = xdoc.SelectSingleNode(xpath);
            ((SelectionSettings)_newSettings.Selection).Save(node);

            xpath = "/settings/presentation";
            node = xdoc.SelectSingleNode(xpath);
            ((PresentationSettings)_newSettings.Presentation).Save(node);

            xpath = "/settings/features";
            //node = xdoc.SelectSingleNode(xpath);
            node = SettingsHelper.GetNode(xdoc, xpath);
            ((FeaturesSettings)_newSettings.Features).Save(node);

            xpath = "/settings/navigation";
            node = xdoc.SelectSingleNode(xpath);
            ((NavigationSettings)_newSettings.Navigation).Save(node);

            xpath = "/settings/dcat";
            node = xdoc.SelectSingleNode(xpath);
            ((DcatSettings)_newSettings.Dcat).Save(node);

            xdoc.Save(_path);
            _logger.Info("Settings-file was successfully saved");

            //Expose new settings to PX-Web
            _settings = _newSettings;

            _settings.UpdateCoreSettings();
        }

        /// <summary>
        /// Load settings per database
        /// </summary>
        public void LoadDatabaseSettings()
        {
            string path = GetDatabasePath();

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            _databases.Clear();

            // All databases with settings must have a folder in the PX-databases folder containing a database.config file
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (DirectoryInfo dbDir in dir.GetDirectories())
            {
                LoadDatabaseSettingsFile(path, dbDir.Name);
            }

            // If a selected database does not have a database folder then load the default database settings (only possible for CNMM databases...)
            foreach (string db in _generalSettings.Databases.CnmmDatabases)
            {
                if (!_databases.ContainsKey(db))
                {
                    LoadDatabaseSettingsFile(path, db);
                }
            }

            
        }


        /// <summary>   
        /// Called from administration when settings have been saved
        /// </summary>
        public static void EndUpdate()
        {
            _newSettings = null;
        }

        /// <summary>
        /// Some database settings can be changed by another system. This function checks if the database.config file has been updated (by another system) 
        /// since it was loaded by PX-Web. If so the database settings are reloaded. NOTE!!! This method should only be called to by administrative
        /// functionality such as the administration tool!
        /// </summary>
        /// <param name="dbId">Database id</param>
        /// <returns>Settings for the specified database</returns>
        public IDatabaseSettings GetDatabase(string dbId)
        {
            if (_dbFileUpdate.ContainsKey(dbId))
            {
                string filePath = Path.Combine(GetDatabasePath(), dbId + "\\database.config");

                if (System.IO.File.Exists(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    if (_dbFileUpdate.ContainsKey(dbId))
                    {
                        if (file.LastWriteTime > _dbFileUpdate[dbId])
                        {
                            // The database.config file has been updated (maybe by another system) since the database settings was loaded by PX-Web 
                            // - Reload the database settings!
                            LoadDatabaseSettingsFile(GetDatabasePath(), dbId);
                        }
                    }
                }
            }

            //// The asked for database has not yet been loaded - Load it! 
            //if (!_databases.ContainsKey(dbId))
            //{
            //    LoadDatabaseSettingsFile(GetDatabasePath(), dbId);
            //}

            //return _databases[dbId];

            return GetDatabaseSettings(dbId);
        }

        /// <summary>
        /// Get the database settings for the specified database
        /// </summary>
        /// <param name="dbId">Database id</param>
        /// <returns>Settings for the specified database</returns>
        public IDatabaseSettings GetDatabaseSettings(string dbId)
        {
            // The asked for database has not yet been loaded - Load it! 
            if (!_databases.ContainsKey(dbId))
            {
                LoadDatabaseSettingsFile(GetDatabasePath(), dbId);
            }

            return _databases[dbId];
        }

        #endregion
        
        #region "Private/Protected methods"

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="path">Path to the settings file</param>
        protected Settings(string path)
        {
            _logger = log4net.LogManager.GetLogger(typeof(Settings));

            if (string.IsNullOrEmpty(path))
            {
                _logger.Error("Settings-file not specified");
                throw new System.Exception();
            }
            if (!File.Exists(path))
            {
                _logger.Error("Settings-file '" + path + "' does not exist");
                throw new System.Exception();
            }

            _path = path;
            _databases = new Dictionary<string, IDatabaseSettings>();
            _dbFileUpdate = new Dictionary<string, DateTime>();

            LoadSettings();
            LoadDatabaseSettings();
        }

        /// <summary>
        /// Read the settings file and load the settings
        /// </summary>
        /// <returns>True if the settings was loaded successfully, else false</returns>
        private bool LoadSettings()
        {
            string xpath;
            XmlNode node;

            try
            {
                _logger.Info("Load settings file (settings.config) started");

                //Load settings-file
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(_path);

                xpath = "/settings/general";
                node = xdoc.SelectSingleNode(xpath);
                _generalSettings = new GeneralSettings(node);

                xpath = "/settings/menu";
                node = xdoc.SelectSingleNode(xpath);
                _menuSettings = new MenuSettings(node);

                xpath = "/settings/selection";
                node = xdoc.SelectSingleNode(xpath);
                _selectionSettings = new SelectionSettings(node);

                xpath = "/settings/presentation";
                node = xdoc.SelectSingleNode(xpath);
                _presentationSettings = new PresentationSettings(node);

                xpath = "/settings/features";
                //node = xdoc.SelectSingleNode(xpath);
                node = SettingsHelper.GetNode(xdoc, xpath);
                _featuresSettings = new FeaturesSettings(node);

                xpath = "/settings/navigation";
                //node = SettingsHelper.GetNode(xdoc, xpath);
                node = xdoc.SelectSingleNode(xpath);
                _navigationSettings = new NavigationSettings(node);

                xpath = "/settings/dcat";
                //node = SettingsHelper.GetNode(xdoc, xpath);
                node = xdoc.SelectSingleNode(xpath);
                _dcatSettings = new DcatSettings(node);

            }
            catch (System.Exception ex)
            {
                _logger.Error("::LoadSettings() : Error when loading settings : " + ex.Message);
                return false;
            }

            _logger.Info("Settings-file '" + _path + "' was loaded successfully");
            return true;
        }

        /// <summary>
        /// Load database settings
        /// </summary>
        /// <param name="dbDir">Directory where the databases are located</param>
        /// <param name="dbId">Id of the database</param>
        private void LoadDatabaseSettingsFile(string dbDir, string dbId)
        {
            string filePath;
            DatabaseSettings dbSettings;

            filePath = Path.Combine(dbDir, dbId + "\\database.config");

            try
            {
                dbSettings = new DatabaseSettings(filePath);
            }
            catch (Exception)
            {
                return;
            }

            lock (_databases)
            {

                if (_databases.ContainsKey(dbId))
                {
                    _databases.Remove(dbId);
                }
                _databases.Add(dbId, dbSettings);

                // Store information about when the database settings files were last updated
                if (System.IO.File.Exists(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    if (_dbFileUpdate.ContainsKey(dbId))
                    {
                        _dbFileUpdate.Remove(dbId);
                    }
                    _dbFileUpdate.Add(dbId, file.LastWriteTime);
                }
            }
        }
        

        /// <summary>
        /// Get the path to the databases directory
        /// </summary>
        /// <returns>Path to the databasese directory. If the path illegal in some way a empty string is returned</returns>
        private string GetDatabasePath()
        {
            string path;

            if (string.IsNullOrEmpty(_generalSettings.Paths.PxDatabasesPath))
            {
                return "";
            }

            //path = System.Web.HttpContext.Current.Server.MapPath(_generalSettings.Paths.PxDatabasesPath);
            path = System.Web.Hosting.HostingEnvironment.MapPath(_generalSettings.Paths.PxDatabasesPath);

            if (!Directory.Exists(path))
            {
                return "";
            }

            return path;
        }

        /// <summary>
        /// Updates settings that in the stand-alone version of the web-components are loaded from web.config
        /// </summary>
        private void UpdateCoreSettings()
        {
            // Override Default language
            PCAxis.Web.Core.Management.LocalizationManager.DefaultLanguage = _generalSettings.Language.DefaultLanguage ?? "en";
            // Override Language path
            if (System.IO.Path.IsPathRooted(_generalSettings.Paths.LanguagesPath))
            {
                PCAxis.Paxiom.Localization.PxResourceReader.LanguagePath = _generalSettings.Paths.LanguagesPath;
            }
            else
            {
                PCAxis.Paxiom.Localization.PxResourceReader.LanguagePath = System.Web.HttpContext.Current.Server.MapPath(_generalSettings.Paths.LanguagesPath);
            }
            // Override Images path
            //PCAxis.Web.Controls.Configuration.Paths.ImagesPath = System.Web.HttpContext.Current.Server.MapPath(_generalSettings.Paths.ImagesPath);
            PCAxis.Web.Controls.Configuration.Paths.ImagesPath = _generalSettings.Paths.ImagesPath;
            // Overide path to the aggregation files
            PCAxis.Paxiom.GroupRegistry.GroupingsPath = System.Web.HttpContext.Current.Server.MapPath(_generalSettings.Paths.PxAggregationsPath);

            // Override General.Global settings:
            PCAxis.Paxiom.Settings.Numbers.SecrecyOption = _generalSettings.Global.SecrecyOption;

            // Rounding rule
            switch (_generalSettings.Global.RoundingRule)
            {
                case PCAxis.Paxiom.RoundingType.BankersRounding:
                case PCAxis.Paxiom.RoundingType.None:
                    PCAxis.Paxiom.Settings.Numbers.RoundingRule = MidpointRounding.ToEven;
                    break;
                case PCAxis.Paxiom.RoundingType.RoundUp:
                    PCAxis.Paxiom.Settings.Numbers.RoundingRule = MidpointRounding.AwayFromZero;
                    break;
                default:
                    PCAxis.Paxiom.Settings.Numbers.RoundingRule = MidpointRounding.ToEven;
                    break;
            }

            // Symbols
            PCAxis.Paxiom.Settings.DataSymbols.SymbolNIL = _generalSettings.Global.DataSymbolNil;
            PCAxis.Paxiom.Settings.DataSymbols.set_Symbol(1, _generalSettings.Global.Symbol1);
            PCAxis.Paxiom.Settings.DataSymbols.set_Symbol(2, _generalSettings.Global.Symbol2);
            PCAxis.Paxiom.Settings.DataSymbols.set_Symbol(3, _generalSettings.Global.Symbol3);
            PCAxis.Paxiom.Settings.DataSymbols.set_Symbol(4, _generalSettings.Global.Symbol4);
            PCAxis.Paxiom.Settings.DataSymbols.set_Symbol(5, _generalSettings.Global.Symbol5);
            PCAxis.Paxiom.Settings.DataSymbols.set_Symbol(6, _generalSettings.Global.Symbol6);
            PCAxis.Paxiom.Settings.DataSymbols.set_Symbol(7, _generalSettings.Global.DataSymbolSum);

            // Get language specific settings
            PCAxis.Paxiom.Settings.LocaleSettings langSettings;

            foreach (LanguageSettings lang in _generalSettings.Language.SiteLanguages)
            {
                langSettings = PCAxis.Paxiom.Settings.GetLocale(lang.Name);
                langSettings.DateFormat = lang.DateFormat;
                langSettings.DecimalSeparator = lang.DecimalSeparator;
                langSettings.ThousandSeparator = lang.ThousandSeparator;
            }

            PCAxis.Paxiom.Settings.DataNotes.Placment = _generalSettings.Global.DataNotePlacement;
            PCAxis.Paxiom.Settings.Metadata.RemoveSingleContent = _generalSettings.Global.RemoveSingleContent;

            PCAxis.Paxiom.Settings.Files.CompleteInfoFile = _generalSettings.FileFormats.Excel.InformationLevel;            

            switch (_generalSettings.FileFormats.FileBaseName)
            {
                case PCAxis.Paxiom.FileBaseNameType.Matrix:
                    PCAxis.Paxiom.Settings.Files.FileBaseName = PCAxis.Paxiom.FileBaseNameType.Matrix;
                    break;
                case PCAxis.Paxiom.FileBaseNameType.TableID:
                    PCAxis.Paxiom.Settings.Files.FileBaseName = PCAxis.Paxiom.FileBaseNameType.TableID;
                    break;
                default:
                    PCAxis.Paxiom.Settings.Files.FileBaseName = PCAxis.Paxiom.FileBaseNameType.Matrix;
                    break;
            }

            if (_generalSettings.FileFormats.Excel.DoubleColumn)
            {
                PCAxis.Paxiom.Settings.Files.DoubleColumnFile = PCAxis.Paxiom.DoubleColumnType.AlwaysDoubleColumns;
            }
            else
            {
                PCAxis.Paxiom.Settings.Files.DoubleColumnFile = PCAxis.Paxiom.DoubleColumnType.NoDoubleColumns;
            }

            PCAxis.Api.Settings.Current.MaxValues = _featuresSettings.Api.MaxValuesReturned;
            PCAxis.Api.Settings.Current.LimiterRequests = _featuresSettings.Api.LimiterRequests;
            PCAxis.Api.Settings.Current.LimiterTimeSpan = _featuresSettings.Api.LimiterTimespan;
            PCAxis.Api.Settings.Current.EnableCORS = _featuresSettings.Api.EnableCORS;
            PCAxis.Api.Settings.Current.EnableCache = _featuresSettings.Api.EnableCache;
            PCAxis.Api.Settings.Current.ClearCache = _featuresSettings.General.ClearCache;
            PCAxis.Api.Settings.Current.FetchCellLimit = _featuresSettings.Api.FetchCellLimit;
            //Cell limit
            //todo: maria PCAxis.Api.Settings.Current.CellLimit = _featuresSettings.General.CellLimit;

        }

        /// <summary>
        /// Retutns path to the settings file
        /// </summary>
        /// <returns></returns>
        private static string GetSettingsPath()
        {
            //TODO: Get path from web.config
            return System.Web.HttpContext.Current.Server.MapPath("~/setting.config");
        }

        #endregion

        #region "Public properties"

        /// <summary>
        /// Get the Settings object (PX-Web)
        /// </summary>
        public static Settings Current
        {
            get
            {
                //// TODO: Remove...
                //_settings = null;

                if (_settings == null)
                {
                    // Assure that only one thread at a time can instantiate the _settings object
                    lock (currentSettingsLock)
                    {
                        if (_settings == null)
                        {
                            _settings = new Settings(GetSettingsPath());
                            _settings.UpdateCoreSettings();
                        }
                    }
                }

                return _settings;
            }
        }

        /// <summary>
        /// Get the Settings object (PX-Web Administration)
        /// </summary>
        public static Settings NewSettings
        {
            get
            {
                return _newSettings;
            }
        }

        /// <summary>
        /// General settings
        /// </summary>
        public IGeneralSettings General { get { return _generalSettings; } }

        /// <summary>
        /// Menu settings
        /// </summary>
        public IMenuSettings Menu { get { return _menuSettings; } }

        /// <summary>
        /// Selection settings
        /// </summary>
        public ISelectionSettings Selection { get { return _selectionSettings; } }

        /// <summary>
        /// Presentation settings
        /// </summary>
        public IPresentationSettings Presentation { get { return _presentationSettings; } }

        /// <summary>
        /// Presentation settings
        /// </summary>
        public INavigationSettings Navigation { get { return _navigationSettings; } }

        /// <summary>
        /// Dcat settings
        /// </summary>
        public IDcatSettings Dcat { get { return _dcatSettings; } }

        /// <summary>
        /// Features settings
        /// </summary>
        public IFeaturesSettings Features { get { return _featuresSettings;  } }

        /// <summary>
        /// Dictionary with settings per database
        /// </summary>
        public Dictionary<string, IDatabaseSettings> Database { get { return _databases; } }

        #endregion

    }
}