using PCAxis.Menu;
using PX.Web.Interfaces.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCAxis.Api
{
    public class CnmmDatabaseApiRootHelper
    {
        private static string _databaseRoot = null;
        private static bool _isRooted;
        private static bool _isInitialized = false;
        public static string DatabaseRoot 
        { 
            get
            {
                if (!_isInitialized)
                {
                    ReadConfig();
                }

                return _databaseRoot;
            }
        }

        public static bool IsRooted 
        { 
            get
            { 
                if (!_isInitialized)
                {
                    ReadConfig();
                }

                return _isRooted; 
            }
        }

        public static bool CheckPath(string db, string[] nodePath, string language)
        {
            string lookupTableName = "pxapi_LookUpApiPathCache_" + language;
            var lookupTable = ApiCache.Current.Get<HashSet<string>>(lookupTableName);
            
            if (lookupTable is null)
            {
                lookupTable = PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[db].GetApiPathLookup(DatabaseRoot);
                ApiCache.Current.Set(lookupTableName, lookupTable, new TimeSpan(24, 0, 0)); //Store in cache for 24 hours
            }

            string path = string.Join("/", nodePath);

            return lookupTable.Contains(path.ToUpper());
        }

        private static void ReadConfig()
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CnmmDatabaseRoot"]))
            {
                _isRooted = true;
                _databaseRoot = ConfigurationManager.AppSettings["CnmmDatabaseRoot"];
            }
            else
            {
                _isRooted = false;
                _databaseRoot = "";
            }

            _isInitialized = true;
        }
    }
}
