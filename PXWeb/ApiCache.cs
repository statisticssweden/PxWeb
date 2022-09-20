using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web;
using System.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace PxWeb
{
    /// <summary>
    /// Implementation class for the API cache
    /// </summary>
    public class ApiCache: IPxCache<string, ResponseBucket>
    {
        /// <summary>
        /// Delegate that waits for something after the cache has been cleaned
        /// </summary>
        public delegate bool CacheReenabler();
        private Func<bool> _coherenceChecker;

        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(ApiCache));
        private static log4net.ILog _apiCacheLogger = log4net.LogManager.GetLogger("api-cache-logger");
        private static string CacheLock = "lock";
        private static bool _defaultCacheValue;
        private static DateTime _cachedStartTime;
        private static ApiCache _current;
        private static MemoryCache _cache;

        /// <summary>
        /// Get the (Singleton) ApiCache object
        /// </summary>
        public static ApiCache Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new ApiCache();
                }
                return _current;
            }
        }

        private ApiCache()
        {
            //Parse if cahce should be enabled
            string strCacheEnabled = ConfigurationManager.AppSettings["enableCache"];
            if (strCacheEnabled != null)
                _defaultCacheValue = bool.Parse(strCacheEnabled);
            else
                _defaultCacheValue = true;

            _cachedStartTime = DateTime.MinValue;

            _apiCacheLogger.DebugFormat("Settings.Current.EnableCache = {0}", Settings.Current.EnableCache);
            _cache = new MemoryCache(new MemoryCacheOptions());

        }

        /// <summary>
        /// Fetches a cached object from the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ResponseBucket Get(string key)
        {

            if (!IsEnabled())
            {
                return null;
            }

            //Check if the cache controller has set a coherence checker
            if (_coherenceChecker != null)
            {
                //Check that the cache is coherent
                if (!_coherenceChecker())
                {
                    //Clear the cache if it is not coherent
                    Clear();
                }
            }

            return _cache.Get(key) as ResponseBucket;
        }

        /// <summary>
        /// Stores a object in the cache
        /// </summary>
        /// <param name="data"></param>
        public void Set(string key, ResponseBucket data)
        {
            TimeSpan time = new TimeSpan(0, 0, 10); // Read from appsettings later

            //Check if caching is enabled
            if (!Settings.Current.EnableCache) return;

            if (_cache.Get(key) == null && data.CreationTime > _cachedStartTime)
            {
                if (_apiCacheLogger.IsDebugEnabled)
                {
                    _apiCacheLogger.DebugFormat("Adding key={0} to Cache", key);
                }

                lock (CacheLock)
                {
                    if (_cache.Get(key) == null)
                    {
                        _cache.Set(key, data, time);
                    }
                }
            }
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        private void ClearCache()
        {
            lock (CacheLock)
            {
                _cache.Compact(1.0);
            }
        }

        public bool IsEnabled()
        {
            return Settings.Current.EnableCache;
        }

        public void Clear()
        {
            _logger.Info("Cache cleared started");
            ClearCache();
            _logger.Info("Cache cleared finished");
        }

        public void Disable()
        {
            _logger.Info("Cache disabled");
            Settings.Current.EnableCache = false;
        }

        public void Enable()
        {
            if (_defaultCacheValue)
            {
                _logger.Info("Cache enabled");
                Settings.Current.EnableCache = true;
            }
        }

        public void SetCoherenceChecker(Func<bool> coherenceChecker)
        {
            _coherenceChecker = coherenceChecker;
        }
        public bool DefaultEnabled
        {
            get { return _defaultCacheValue; }
        }
    }
}