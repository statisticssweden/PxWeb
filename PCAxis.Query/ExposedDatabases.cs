using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using PCAxis.Paxiom.Configuration;
using System.Collections.Concurrent;

namespace PCAxis.Query
{
    /// <summary>
    /// Class for handling the databases that are exposed via the API
    /// </summary>
    public static class ExposedDatabases
    {
        /// <summary>
        /// Database configurations ordered by language and id
        /// </summary>
        public static ConcurrentDictionary<string, Dictionary<string, DbConfig>> DatabaseConfigurations { get; set; }

        static ExposedDatabases()
        {
            Load();
        }

        /// <summary>
        /// Load configuration file
        /// </summary>
        private static void Load()
        {
            DatabaseConfigurations = new ConcurrentDictionary<string, Dictionary<string, DbConfig>>();

            var databaseXML = XDocument.Load(AppSettingsHelper.GetAppSettingsPath("dbmetaFile")); //ConfigurationManager.AppSettings["dbmetaFile"]);
            foreach (var databaseSetElement in databaseXML.Element("Databases").Elements("DatabaseSet"))
            {
                var language = (string)databaseSetElement.Attribute("language");
                DatabaseConfigurations.GetOrAdd(language, new Dictionary<string, DbConfig>());

                foreach (var databaseElement in databaseSetElement.Elements("Database"))
                {
                    var dbConfig = new DbConfig{
                        Type = (string)databaseElement.Attribute("type"),
                        Name = (string)databaseElement.Element("Name"),
                        RootPath = (string)databaseElement.Attribute("rootPath")
                    };
                    DatabaseConfigurations[language].Add((string)databaseElement.Attribute("id"), dbConfig);
                }
            }
        }

        /// <summary>
        /// Reload configuration file
        /// </summary>
        public static void Reload()
        {
            Load();
        }
    }
}
