using System;
using System.Collections.Generic;

namespace PXWeb
{
    /// <summary>
    /// Class for holding information about a database
    /// </summary>
    public class DatabaseInfo
    {
        /// <summary>
        /// Dictionary containing the name of the database in different languages, key = language identifier (en = english)
        /// </summary>
        private Dictionary<string, string> _dbName;

        /// <summary>
        /// List containing the languages of the database
        /// </summary>
        private List<string> _languages;

        public DatabaseInfo()
        {
            _dbName = new Dictionary<string, string>();
            _languages = new List<string>();
        }


        /// <summary>
        /// Returns the name of the database in the specified language
        /// </summary>
        /// <param name="lang">Language</param>
        /// <returns>The name of the database</returns>
        public string GetDatabaseName(String lang)
        {
            if (_dbName.ContainsKey(lang))
            {
                return _dbName[lang];
            }
            else if (_dbName.ContainsKey(PXWeb.Settings.Current.General.Language.DefaultLanguage))
            {
                return _dbName[PXWeb.Settings.Current.General.Language.DefaultLanguage];
            }
            else
            {
                return Id;
            }
        }

        /// <summary>
        /// Add name for the specified language
        /// </summary>
        /// <param name="lang">Language</param>
        /// <param name="name">Name</param>
        public void AddName(string lang, string name)
        {
            if (_dbName.ContainsKey(lang))
            {
                _dbName[lang] = name;
            }
            else
            {
                _dbName.Add(lang, name);
            }
        }

        /// <summary>
        /// Add language for the database (meaning the database has at least one table in the specified language)
        /// </summary>
        /// <param name="lang">Language code</param>
        public void AddLanguage(string lang)
        {
            if (!_languages.Contains(lang))
            {
                _languages.Add(lang);
            }
        }

        /// <summary>
        /// Has the database tables in the specified language?
        /// </summary>
        /// <param name="lang">The language code</param>
        /// <returns>True if the database has tables in the specified language, else false</returns>
        public bool HasLanguage(string lang)
        {
            return _languages.Contains(lang);
        }

        /// <summary>
        /// Id of the database.
        /// If database type is PX, Id equals the folder name (directory) of the database.
        /// If database type is CNMM, Id equals id of the database in the SQL-configuration file
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Type of database
        /// </summary>
        public PCAxis.Web.Core.Enums.DatabaseType Type { get; set; }

        /// <summary>
        /// The last time the database was updated (Menu.xml was generated for a PX-file database)
        /// LastUpdated = DateTime.MinValue means that the database (file) has not been generated
        /// </summary>
        public DateTime LastUpdated { get; set; }

    }
}
