using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the General.Databases settings
    /// </summary>
    internal class DatabasesSettings : IDatabasesSettings
    {
        #region "Private fields"
        /// <summary>
        /// Enabled PX-file databasese
        /// </summary>
        private List<string> _pxDatabases;
        /// <summary>
        /// Enabled CNMM databases
        /// </summary>
        private List<string> _cnmmDatabases;
        /// <summary>
        /// Dictionary with all possible PX-file databases. Key = directory name of the database
        /// </summary>
        private Dictionary<string, DatabaseInfo> _allPxDatabases;
        /// <summary>
        /// Dictionary with all possible CNMM databases. Key = Id of database in the SQL-configuration file.
        /// </summary>
        private Dictionary<string, DatabaseInfo> _allCnmmDatabases;
        /// <summary>
        /// Object to control loading of "All PX databases" in a multithreaded environment 
        /// </summary>
        private static object _allPxDbSettingLock = new Object();
        /// <summary>
        /// Object to control loading of "All CNMM databases" in a multithreaded environment 
        /// </summary>
        private static object _allCnmmDbSettingLock = new Object();
        /// <summary>
        /// Log
        /// </summary>
        private static log4net.ILog _logger;
        #endregion


        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databasesNode">XML-node for the General.Databases settings</param>
        public DatabasesSettings(XmlNode databasesNode)
        {
            string xpath;
            XmlNode node;

            _logger = log4net.LogManager.GetLogger(typeof(DatabasesSettings));

            _pxDatabases = new List<string>();
            xpath = "./pxDatabases";
            node = databasesNode.SelectSingleNode(xpath);
            xpath = ".//database";
            _pxDatabases = SettingsHelper.GetSettingValue(xpath, node);


            _cnmmDatabases = new List<string>();
            xpath = "./cnmmDatabases";
            node = databasesNode.SelectSingleNode(xpath);
            xpath = ".//database";
            _cnmmDatabases = SettingsHelper.GetSettingValue(xpath, node);

        }

        /// <summary>
        /// Save the General.Databases settings to the settings file
        /// </summary>
        /// <param name="databasesNode">XML-node for the General.Databases settings</param>
        public void Save(XmlNode databasesNode)
        {
            string xpath;

            xpath = "./pxDatabases";
            SettingsHelper.SetSettingValue(xpath, databasesNode, "database", PxDatabases);

            xpath = "./cnmmDatabases";
            SettingsHelper.SetSettingValue(xpath, databasesNode, "database", CnmmDatabases);
        }       
        #endregion

        #region Private methods
        /// <summary>
        /// Load all possible PX-file databases
        /// </summary>
        private void LoadAllPxDatabases()
        {
            //CNMM databases must be loaded before the PX-databases
            if (_allCnmmDatabases == null)
            {
                LoadAllCnmmDatabases();
            }
            
            if (_allPxDatabases == null)
                {
                    // Assure that only one thread at a time can load "All PX databases"
                    lock (_allPxDbSettingLock)
                    {
                        if (_allPxDatabases == null)
                        {
                            _logger.Info("Loading of PX databases started");
                            _allPxDatabases = new Dictionary<string, DatabaseInfo>();
                            string dbDir;

                            //if (System.IO.Path.IsPathRooted(PXWeb.Settings.Current.General.Paths.PxDatabasesPath))
                            //{
                            //    //Absolute path
                            //    dbDir = PXWeb.Settings.Current.General.Paths.PxDatabasesPath;
                            //}
                            //else
                            //{
                            //    //Relative path
                                dbDir = System.Web.HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath);
                            //}

                            DirectoryInfo rootDir = new DirectoryInfo(dbDir);

                            foreach (DirectoryInfo dir in rootDir.GetDirectories())
                            {
                                //Verify that this is not a CNMM database...
                                if (!_allCnmmDatabases.ContainsKey(dir.Name))
                                {
                                    DatabaseInfo dbi = new DatabaseInfo();

                                    //dbi.Type = DatabaseType.PX;
                                    dbi.Type = PCAxis.Web.Core.Enums.DatabaseType.PX;
                                    dbi.Id = dir.Name;

                                    //Check if database file has been generated
                                    FileInfo dbFile = new FileInfo(Path.Combine(dir.FullName, PXWeb.Settings.Current.General.Databases.PxDatabaseFilename));

                                    if (dbFile.Exists)
                                    {
                                        dbi.LastUpdated = dbFile.LastWriteTime;
                                        GetDatabaseLanguages(dbi, dbFile);
                                    }
                                    else
                                    {
                                        dbi.LastUpdated = DateTime.MinValue;
                                    }

                                    foreach (string lang in PXWeb.Settings.Current.General.Language.AllLanguages)
                                    {
                                        string name = "";

                                        //Check for Alias-file in the actual language
                                        string aliasFile = Path.Combine(dir.FullName, "Alias_" + lang + ".txt");
                                        if (File.Exists(aliasFile))
                                        {
                                            name = ReadAliasName(aliasFile);
                                        }

                                        // Check for Alias file in the fallback language
                                        if (name.Length == 0)
                                        {
                                            aliasFile = Path.Combine(dir.FullName, "Alias.txt");
                                            if (File.Exists(aliasFile))
                                            {
                                                name = ReadAliasName(aliasFile);
                                            }
                                        }

                                        // Take the directory name
                                        if (name.Length == 0)
                                        {
                                            name = dir.Name;
                                        }

                                        //Add Database name for the language
                                        dbi.AddName(lang, name);
                                    }

                                    _allPxDatabases.Add(dir.Name, dbi);
                                    _logger.InfoFormat("PX database {0} added", dbi.Id);
                                }
                            }
                            _logger.Info("Loading of PX databases ended");

                        }
                    }
                }


        }

        /// <summary>
        /// Get from Menu.xml file in which langauages the database has tables
        /// </summary>
        /// <param name="dbi">DatabasInfo object for the database</param>
        /// <param name="dbFile">FileInfo object for Menu.xml</param>
        private void GetDatabaseLanguages(DatabaseInfo dbi, FileInfo dbFile)
        {
            string lang;

            //Load the Menu.xml file
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(dbFile.FullName);
                                    
            //Find all langauage-nodes
            XmlNodeList nodeList = xdoc.SelectNodes("//Language");
            foreach (XmlNode node in nodeList)
            {
                lang = node.Attributes["lang"].Value;

                //Check if there are any tables (Link-elements) in this language
                XmlNodeList lst = node.SelectNodes(".//Link");
                if (lst.Count > 0)
                {
                    dbi.AddLanguage(lang);
                }
            }
        }

        /// <summary>
        /// Reads the alias-name from a alias file. Assumes only one row of text in the alias-file.
        /// </summary>
        /// <param name="path">Path to the alias file</param>
        /// <returns>The alias name. If no name could be read an empty string is returned.</returns>
        private string ReadAliasName(string path)
        {
            string name = "";

            using (StreamReader reader = new StreamReader(path, System.Text.Encoding.Default))
            {
                if (reader.Peek() != -1)
                {
                    name = reader.ReadLine();
                }
            }

            return name;
        }

        /// <summary>
        /// Load all possible CNMM databases
        /// </summary>
        private void LoadAllCnmmDatabases()
        {

            if (_allCnmmDatabases == null)
            {
                // Assure that only one thread at a time can load "All CNMM databases"
                lock (_allCnmmDbSettingLock)
                {
                    if (_allCnmmDatabases == null)
                    {
                        _logger.Info("Loading of CNMM databases started");

                        _allCnmmDatabases = new Dictionary<string, DatabaseInfo>();
                        string configFile = PCAxis.Sql.DbConfig.SqlDbConfigsStatic.ConfigPath;
                        string configPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), configFile);

                        if (!File.Exists(configPath))
                        {
                            _logger.Error("LoadAllCnmmDatabases : Could not read Sql-config file '" + configPath + "'");
                            return;
                        }

                        //Get databases directory
                        string dbDir = System.Web.HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath);
                        DirectoryInfo dir = new DirectoryInfo(dbDir);

                        //Load the Sql-settings file
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(configPath);

                        //Find all database-nodes
                        XmlNodeList nodeList = xdoc.SelectNodes("//Database");
                        foreach (XmlNode node in nodeList)
                        {
                            CnmmDatabaseInfo dbi = new CnmmDatabaseInfo();

                            //dbi.Type = DatabaseType.CNMM;
                            dbi.Type = PCAxis.Web.Core.Enums.DatabaseType.CNMM;
                            dbi.Id = node.Attributes["id"].Value;
                            //dbi.LastUpdated = DateTime.MinValue;

                            //Check if database file (.xml) has been generated
                            FileInfo dbFile = new FileInfo(Path.Combine(dir.FullName, dbi.Id + ".xml"));

                            if (dbFile.Exists)
                            {
                                dbi.LastUpdated = dbFile.LastWriteTime;
                            }
                            else
                            {
                                dbi.LastUpdated = DateTime.MinValue;
                            }

                            // TODO: Get description for every language...
                            foreach (string lang in PXWeb.Settings.Current.General.Language.AllLanguages)
                            {
                                XmlNode descNode = node.SelectSingleNode(String.Format("./Descriptions/Description[@lang='{0}']", lang));
                                if (descNode != null)
                                {
                                    dbi.AddName(lang,descNode.InnerText);
                                }
                                else
                                {
                                    descNode = node.SelectSingleNode("./Descriptions/Description");
                                    dbi.AddName(lang,descNode.InnerText);
                                }
                            }

                            // Get default language of the CNMM database
                            XmlNode deflangNode = node.SelectSingleNode("./Languages/Language[@main='true']");
                            if (deflangNode != null)
                            {
                                dbi.DefaultLanguage = deflangNode.Attributes["code"].Value;
                                dbi.AddLanguage(deflangNode.Attributes["code"].Value);
                            }

                            // Get the other languages of the CNMM database
                            XmlNodeList langNodes = node.SelectNodes("./Languages/Language[@main='false']");
                            foreach (XmlNode langNode in langNodes)
                            {
                                dbi.OtherLanguages.Add(langNode.Attributes["code"].Value);
                                dbi.AddLanguage(langNode.Attributes["code"].Value);
                            }
                            _allCnmmDatabases.Add(dbi.Id, dbi);
                            _logger.InfoFormat("CNMM database {0} added", dbi.Id);
                        }
                        _logger.Info("Loading of CNMM databases ended");
                    }
                }
            }

        }


        #endregion

        #region IDatabasesSettings Members

        public System.Collections.Generic.IEnumerable<string> PxDatabases
        {
            get { return _pxDatabases; }
        }

        public System.Collections.Generic.IEnumerable<string> CnmmDatabases
        {
            get { return _cnmmDatabases; }
        }

        public IEnumerable<DatabaseInfo> AllPxDatabases
        {
            get 
            {
                LoadAllPxDatabases();
                return _allPxDatabases.Values; 
            }
        }

        public IEnumerable<DatabaseInfo> AllCnmmDatabases
        {
            get 
            {
                LoadAllCnmmDatabases();
                return _allCnmmDatabases.Values; 
            }
        }

        public string PxDatabaseFilename { get { return "Menu.xml"; } }

        public DatabaseInfo GetPxDatabase(string id)
        {
            LoadAllPxDatabases();
            if (_allPxDatabases.ContainsKey(id))
            {
                return _allPxDatabases[id];
            }
            else
            {
                return null;
            }
        }

        public DatabaseInfo GetCnmmDatabase(string id)
        {
            LoadAllCnmmDatabases();
            if (_allCnmmDatabases.ContainsKey(id))
            {
                return _allCnmmDatabases[id];
            }
            else
            {
                return null;
            }
        }

        public DatabaseInfo GetDatabase(string id)
        {
            DatabaseInfo dbi = GetPxDatabase(id);

            if (dbi == null)
            {
                dbi = GetCnmmDatabase(id);
            }

            return dbi;
        }

        public void ResetDatabases()
        {
            _allCnmmDatabases = null;
            _allPxDatabases = null;
        }

        #endregion

    }
}
