using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml;
using System.Configuration;
using log4net;
using PCAxis.Sql.DbConfig;

namespace PCAxis.Sql.DbConfig 
{
    public static class SqlDbConfigsStatic
    {
       
        private static SqlDbConfigs _config;
        
        private static SqlDbConfig _defaultDatabase;
        public static SqlDbConfig DefaultDatabase
        {
            get { return _defaultDatabase; }
        }

        private static Dictionary<string, string> _databasesDescription;
        public static Dictionary<string, string> DatabasesDescription
        {
            get { return _databasesDescription; }
        }

        private static Dictionary<string,SqlDbConfig> _dataBases;
        public static Dictionary<string, SqlDbConfig> DataBases
        {
            get {return _dataBases;}
        }
          public static Dictionary<string, string> getAvailableDatabases()  
          {  
              return _databasesDescription;  
          }  


        static SqlDbConfigsStatic()
        {
            string configPath = ConfigPath;
            if (String.IsNullOrEmpty(configPath) ) {
                throw new ConfigurationErrorsException("Hullo: cant find an entry for dbconfigFile in AppSettings");
            }
            _config = new SqlDbConfigs(configPath);
            _databasesDescription = _config.DatabaselistDescById;
          
            _dataBases = _config.Databases;
            _defaultDatabase = _dataBases[_config.DefaultDbId];
        }


        public static string ConfigPath
        {
            get
            {
                string p = ConfigurationManager.AppSettings["dbconfigFile"];
                if (System.IO.Path.IsPathRooted(p))
                {
                    return p;
                }

                string orgBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                string noBinBaseDir = orgBaseDir.Replace("\\bin\\", "\\");

                string p2 = System.IO.Path.Combine(orgBaseDir , p);
                if ((new System.IO.FileInfo(p2)).Exists)
                {
                    return System.IO.Path.GetFullPath((new Uri(p2)).LocalPath);
                }


               //removing any "\bin"
                string p3 = System.IO.Path.Combine(noBinBaseDir, p);
                if ((new System.IO.FileInfo(p3)).Exists)
                {
                    return System.IO.Path.GetFullPath((new Uri(p3)).LocalPath);
                }


                // Uses p2 to get old exception (with any bin).
                return System.IO.Path.GetFullPath((new Uri(p2)).LocalPath);
            }
        }

        /// <summary>
        /// The id of the default database, which is the first in the config-file.  Note: PXWeb is not required to show all databases, so it need not be present in PXWeb.
        /// </summary>
        public static string DefaultDbId
        {
            get { return _config.DefaultDbId; }
        }


        /// <summary>
        /// Resets/replaces the connectionsstring.
        /// </summary>
        /// <param name="databaseIDstring">the database which should change connectionstring </param>
        /// <param name="newConnectionstring">connectionstring with default user and password</param>
        public static void ResetConnectionString(string databaseIDstring, string newConnectionstring)
        {
            if (!SqlDbConfigsStatic._dataBases.ContainsKey(databaseIDstring))
            {
                throw new ApplicationException("Can't find databaseId =\"" + databaseIDstring + "\"");
            }
            SqlDbConfigsStatic._dataBases[databaseIDstring].ResetConnectionString(newConnectionstring);
        }
    }
}
