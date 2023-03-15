using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web.Caching;
using System.Web;
using System.Configuration;
using PX.Web.Interfaces.Cache;

namespace PCAxis.Api
{
    /// <summary>
    /// Implementation class for the API cache
    /// </summary>
    public class ApiCache : IPxCache
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

        }

        /// <summary>
        /// Creates a key for the API cache
        /// </summary>
        /// <param name="rawKey"></param>
        /// <returns></returns>
        public static string CreateKey(string rawKey)
        {
            byte[] buffer = System.Web.HttpContext.Current.Request.ContentEncoding.GetBytes(rawKey);
            SHA1CryptoServiceProvider cryptoTransformSHA1 =
            new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(
                cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return "pxapi_" + hash;
        }

        /// <summary>
        /// Fetches a cached ResponseBucket object from the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ResponseBucket Fetch(string key)
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
          
            return System.Web.HttpRuntime.Cache[key] as ResponseBucket;
        }

        /// <summary>
        /// Stores a ResponseBucket object in the cache
        /// </summary>
        /// <param name="data"></param>
        public void Store(ResponseBucket data, TimeSpan ?time = null)
        {
            if (time == null)
            {
                time = new TimeSpan(0, 2, 0);
            }
            
            //Check if caching is enabled
            if (!Settings.Current.EnableCache) return;

            if (System.Web.HttpRuntime.Cache[data.Key] == null && data.CreationTime > _cachedStartTime)
            {
                if (_apiCacheLogger.IsDebugEnabled)
                {
                    _apiCacheLogger.DebugFormat("Adding key={0} to Cache", data.Key);
                }

                lock (CacheLock)
                {
                    if (System.Web.HttpRuntime.Cache[data.Key] == null)
                    {
                        System.Web.HttpRuntime.Cache.Add(data.Key, data, null, System.Web.Caching.Cache.NoAbsoluteExpiration, time.Value, System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                }
            }
        }

        /// <summary>
        /// Fetches a cached object from the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (!IsEnabled())
            {
                return default;
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

            return (T)System.Web.HttpRuntime.Cache[key];
        }

        /// <summary>
        /// Stores a object in the cache for a specified time
        /// </summary>
        /// <param name="data"></param>
        public void Set(string key, object value, TimeSpan lifetime)
        {
            if (System.Web.HttpRuntime.Cache[key] is null)
            {
                if (_apiCacheLogger.IsDebugEnabled)
                {
                    _apiCacheLogger.DebugFormat("Adding key={0} to Cache", key);
                }
                lock (CacheLock)
                {
                    if (System.Web.HttpRuntime.Cache[key] is null)
                    {
                        System.Web.HttpRuntime.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, lifetime, System.Web.Caching.CacheItemPriority.Normal, null);
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

                IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    string key = enumerator.Key.ToString();
                    if (key.StartsWith("pxapi_"))
                    {
                        HttpRuntime.Cache.Remove(key);
                    }
                }
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
