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

        public static bool CheckPath(string db, string[] NodePath, string language)
        {
            string lookupTableName = "pxapi_LookUpApiPathCache_" + language;
            //var lookupTable = ApiCache.Current.Fetch().Get<Dictionary<string, ItemSelection>>(lookupTableName);
            //if (lookupTable is null)
            //{
            //    lookupTable = _itemSelectionResolverFactory.GetMenuLookup(language);
            //    _pxCache.Set(lookupTableName, lookupTable);
            //}
            HashSet<string> lookupTable = PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[db].GetApiPathLookup(language, DatabaseRoot);

            return true;

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
