using PCAxis.Paxiom;
using PX.Web.Interfaces.Cache;
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Web;

namespace PXWeb.Management
{
    /// <summary>
    /// Class for caching Paxiom objects for saved queries
    /// </summary>
    public class SavedQueryPaxiomCache : IPxCache
    {

        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(SavedQueryPaxiomCache));
        private static log4net.ILog _paxiomCacheLogger = log4net.LogManager.GetLogger("saved-query-paxiom-cache-logger");


        /// <summary>
        /// If the cache is enabled or not
        /// </summary>
        private bool _cacheEnabled = true;

        /// <summary>
        /// Memory cache holding the Paxiom objects
        /// </summary>
        private MemoryCache _cache;

        /// <summary>
        /// Function that do conerence checking on the cache
        /// </summary>
        private Func<bool> _coherenceChecker;

        /// <summary>
        /// The time when the cache was started or reenabled
        /// </summary>
        private DateTime _cacheStartTime;

        /// <summary>
        /// Used to create a mutual-exclusion and thread-safe lock when cleaning the cache  
        /// </summary>
        private static string _cacheLock = "lock";
        
        private static SavedQueryPaxiomCache _current;

        /// <summary>
        /// Get the (Singleton) Saved Query Paxiom Cache object
        /// </summary>
        public static SavedQueryPaxiomCache Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new SavedQueryPaxiomCache();
                }
                return _current;
            }
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private SavedQueryPaxiomCache()
        {
            Initialize();
        }

        #region "Private methods"

        /// <summary>
        /// Initialize the Cache
        /// </summary>
        private void Initialize()
        {
            _cacheEnabled = PXWeb.Settings.Current.Features.SavedQuery.EnableCache; // Get setting from setting.config
            _cache = new MemoryCache("Saved Query Paxiom Cache");
            _cacheStartTime = DateTime.MinValue;
            
            _paxiomCacheLogger.DebugFormat("PXWeb.Settings.Current.Features.SavedQuery.EnableCache = {0}", PXWeb.Settings.Current.Features.SavedQuery.EnableCache);
            
        }

      
        private void ClearCache()
        {
            lock (_cacheLock)
            {
                _cache.ToList().ForEach(a => _cache.Remove(a.Key));
                _logger.Info("Cache cleared");

            }
        }

      

        #endregion

        #region "Public methods"

        /// <summary>
        /// Add model to cache
        /// </summary>
        /// <param name="key">Saved Query ID</param>
        /// <param name="model">Paxiom object</param>
        /// <param name="timeStamp">Time when the Paxiom object was created</param>
        /// <returns></returns>
        public void Store(string key, PXModel model, DateTime timeStamp)
        {
            StoreObject(key, model, timeStamp);
        }

        private const string QueryModelKeySuffix = "_queryModel";

        /// <summary>
        /// Adds a query model to the cache
        /// </summary>
        /// <param name="key">Saved Query ID</param>
        /// <param name="queryModel">Table query object</param>
        /// <param name="timeStamp">Time when the table query object was created</param>
        public void StoreQueryModel(string key, PCAxis.Query.TableQuery queryModel, DateTime timeStamp)
        {
            var queryModelCacheKey = key + QueryModelKeySuffix;
            StoreObject(queryModelCacheKey, queryModel, timeStamp);
        }

        private void StoreObject(string key, object obj, DateTime timeStamp)
        {
            if (!_cacheEnabled) return;
            if (_cache.Contains(key)) return;

            if (timeStamp > _cacheStartTime) // Verify that the cache has not been reenabled since the paxiom object was created. If so the paxiom may be obsolete and shall not be stored.
            {
                if (_paxiomCacheLogger.IsDebugEnabled)
                {
                    _paxiomCacheLogger.DebugFormat("Adding key={0} to Cache", key);
                }
                lock (_cacheLock)
                {
                    if (!_cache.Contains(key))
                    {
                        _cache.Add(key, obj,
                        new CacheItemPolicy()
                        {
                            SlidingExpiration = new TimeSpan(0, PXWeb.Settings.Current.Features.SavedQuery.CacheTime, 0),
                            Priority = CacheItemPriority.Default
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Fetches a cached Paxiom object from the cache
        /// </summary>
        /// <param name="key">Saved Query Id</param>
        /// <param name="createCopy">
        /// If a copy of the Paxiom object shall be created or if the actual object shall be returned. 
        /// If the saved query has presentation on screen then a copy shall be created.
        /// </param>
        /// <returns>PXModel object</returns>
        public PXModel Fetch(string key, bool createCopy = false)
        {
         
            if (!_cacheEnabled) return null;

            if (_coherenceChecker != null)
            {
                if(!_coherenceChecker())
                {
                    Clear();
                }
            }

            if (!_cache.Contains(key)) return null;

            PXModel model = (PXModel)_cache[key];

            if (createCopy)
            {
                PXModel newModel = model.CreateCopy();
                PXData newData = model.Data.CreateCopy();
                PaxiomUtil.SetData(newModel, newData);

                return newModel;
            }
            else
            {
                return model;
            }
        }

        /// <summary>
        /// Fetches a cached query model object from the cache
        /// </summary>
        /// <param name="key">Saved Query Id</param>
        /// <returns>Table query object</returns>
        public PCAxis.Query.TableQuery FetchQueryModel(string key, bool createCopy = false)
        {
            var queryModelCacheKey = key + QueryModelKeySuffix;

            if (!_cacheEnabled) return null;

            if (_coherenceChecker != null)
            {
                if (!_coherenceChecker())
                {
                    Clear();
                }
            }

            if (!_cache.Contains(queryModelCacheKey)) return null;
            
            var queryModel = (PCAxis.Query.TableQuery)_cache[queryModelCacheKey];
            if (createCopy)
            {
                PCAxis.Query.TableQuery newModel = queryModel.CreateCopy();
                

                return newModel;
            }
            else
            {
                return queryModel;
            }
           
        }

        /// <summary>
        /// Reset the cache totally
        /// </summary>
        public void Reset()
        {
            Initialize();
        }

        #endregion

        #region "IPxCache implementation"

        public bool IsEnabled()
        {
            return _cacheEnabled;
        }

        public void Clear()
        {
            ClearCache();
        }

        public void Disable()
        {
            _logger.Info("Cache disabled");
            _cacheEnabled = false;
        }

        public void Enable()
        {
            if (DefaultEnabled)
            {
                _logger.Info("Cache enabled");
                _cacheEnabled = true;
            }
        }

        public void SetCoherenceChecker(Func<bool> coherenceChecker)
        {
            _coherenceChecker = coherenceChecker;
        }

        public bool DefaultEnabled
        {
            get { return PXWeb.Settings.Current.Features.SavedQuery.EnableCache; }
        }

        #endregion
    }
}