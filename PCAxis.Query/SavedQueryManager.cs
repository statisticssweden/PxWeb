using PCAxis.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Hosting;
using System.Runtime.Caching;

namespace PCAxis.Query
{
    public class SavedQueryManager
    {

        public static SavedQueryManager Current { get; private set; }

        public const string SAVE_PARAMETER_PATH = "path"; //Key for path parameter. Specifies the directory where the query will be saved
        public const string SAVE_PARAMETER_FILENAME = "filename"; //Key for filename parameter. Specifies the filename for the saved query

        #region "Constructors"

        static SavedQueryManager()
        {
            CreateManager();
        }

        public virtual SavedQuery Load(string name)
        {
            return null;
        }

        public virtual string Save(SavedQuery query, Dictionary<string, string> parameters = null, int? id = null)
        {
            return null;
        }

        public virtual bool MarkAsRunned(string name)
        {
            return false;
        }

        public virtual bool MarkAsFailed(string name)
        {
            return false;
        }


        public static void Reset()
        {
            CreateManager();
        }

        public static SavedQueryStorageType StorageType { get; set; }

        private static void CreateManager()
        {
            if (StorageType == SavedQueryStorageType.File)
            {
                Current = new FileSavedQueryManager();
            }
            else if (StorageType == SavedQueryStorageType.Database)
            {
                Current = new DatabaseSavedQueryManager();
            }
            else
            {
                Current = new FileSavedQueryManager();
            }
        }
    }


    public class FileSavedQueryManager : SavedQueryManager
    {
        private MemoryCache _cache;
        private int _cacheTime = 1;

        public FileSavedQueryManager()
        {
            _cache = new MemoryCache("Saved Query Cache");
        }

        #endregion

        #region "public methods"


        public override SavedQuery Load(string name)
        {

            if (_cache.Contains(name))
            {
                var bucket = _cache[name] as SavedQueryCacheBucket;
                return bucket.Query;
            }

            SavedQuery sq = LoadSavedQuery(name);

            AddToCache(name, sq);

            return sq;
        }

        public override string Save(SavedQuery sq, Dictionary<string, string> parameters = null, int? id = null)
        {
            string name = "";

            if (parameters != null && parameters.ContainsKey(SAVE_PARAMETER_FILENAME) && !string.IsNullOrWhiteSpace(parameters[SAVE_PARAMETER_FILENAME]))
            {
                name = parameters[SAVE_PARAMETER_FILENAME];
            }
            else
            {
                name = Guid.NewGuid().ToString();
            }

            string pathName = name;

            if (!pathName.EndsWith(".pxsq"))
            {
                pathName = pathName + ".pxsq";
            }

            if (parameters != null && parameters.ContainsKey(SAVE_PARAMETER_PATH) && !string.IsNullOrWhiteSpace(parameters[SAVE_PARAMETER_PATH]))
            {
                pathName = System.IO.Path.Combine(parameters[SAVE_PARAMETER_PATH], pathName);
            }

            if (SaveSavedQuery(pathName, sq))
            {
                AddToCache(pathName, sq);
                return name;
            }

            return null;
        }

        public override bool MarkAsRunned(string name)
        {
            SavedQuery sq = Load(name);
            if (sq != null)
            {
                sq.Stats.LastExecuted = DateTime.Now;
                sq.Stats.RunCounter++;
                var bucket = _cache[name] as SavedQueryCacheBucket;
                bucket.IsModified = true;
                return true;
            }
            return false;
        }

        public override bool MarkAsFailed(string name)
        {
            SavedQuery sq = Load(name);
            if (sq != null)
            {
                sq.Stats.LastExecuted = DateTime.Now;
                sq.Stats.RunCounter++;
                sq.Stats.FailCounter++;
                var bucket = _cache[name] as SavedQueryCacheBucket;
                bucket.IsModified = true;
                return true;
            }
            return false;
        }

        #endregion

        #region "private methods"

        /// <summary>
        /// Load a saved Query from disk
        /// </summary>
        /// <param name="path">Path to the saved query</param>
        /// <returns></returns>
        private static SavedQuery LoadSavedQuery(string path)
        {
            if (File.Exists(path))
            {
                string query = File.ReadAllText(path);
                SavedQuery sq = JsonHelper.Deserialize<SavedQuery>(query) as SavedQuery;
                return sq;
            }

            return null;
        }

        /// <summary>
        /// Save query
        /// </summary>
        /// <param name="path">Path + file name</param>
        /// <param name="sq">SavedQuery object</param>
        /// <returns></returns>
        private static bool SaveSavedQuery(string path, SavedQuery sq)
        {
            try
            {
                //string path = HostingEnvironment.MapPath(@"~/App_Data/queries/" + name);

                using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, sq);
                }
            }
            catch (Exception)
            {
                //TODO Logg error
                return false;
            }

            return true;
        }

        private void AddToCache(string name, SavedQuery sq)
        {
            _cache.Set(name, new SavedQueryCacheBucket() { Query = sq },
                new CacheItemPolicy()
                {
                    RemovedCallback = new CacheEntryRemovedCallback(CacheRemovedCallback),
                    SlidingExpiration = new TimeSpan(0, _cacheTime, 0)
                });
        }

        private void CacheRemovedCallback(CacheEntryRemovedArguments args)
        {
            var bucket = args.CacheItem.Value as SavedQueryCacheBucket;
            if (bucket.IsModified)
            {
                var name = args.CacheItem.Key;
                var sq = bucket.Query;

                SaveSavedQuery(name, sq);
            }

        }

        #endregion

        /// <summary>
        /// Internal class for the cache
        /// </summary>
        private class SavedQueryCacheBucket
        {
            public bool IsModified { get; set; }
            public SavedQuery Query { get; set; }

            public SavedQueryCacheBucket()
            {
                IsModified = false;
            }
        }
    }


    public class DatabaseSavedQueryManager : SavedQueryManager
    {

        public ISavedQueryDatabaseAccessor _da;

        public DatabaseSavedQueryManager()
        {
            if (string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["SavedQueryDataAccessor"]))
            {
                throw new System.Configuration.ConfigurationErrorsException("AppSetting SavedQueryDataAccessor not set in config file");
            }

            _da = Activator.CreateInstance(Type.GetType(System.Configuration.ConfigurationManager.AppSettings["SavedQueryDataAccessor"])) as ISavedQueryDatabaseAccessor;
        }

        public override SavedQuery Load(string name)
        {
            int id;

            //Convert savedquery name to integer
            if (!int.TryParse(name, out id)) return null;

            return _da.Load(id);

        }

        public override string Save(SavedQuery query, Dictionary<string, string> parameters = null, int? id = null)
        {
            if (id == null)
            {
                return _da.Save(query).ToString();
            }
            else
            {
                return _da.Save(query, id.Value).ToString();
            }
        }

        public override bool MarkAsRunned(string name)
        {
            int id;

            //Convert savedquery name to integer
            if (!int.TryParse(name, out id)) return false;

            return _da.MarkAsRunned(id);
        }

        public override bool MarkAsFailed(string name)
        {
            int id;

            //Convert savedquery name to integer
            if (!int.TryParse(name, out id)) return false;

            return _da.MarkAsFailed(id);
        }
    }
}
