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
using Microsoft.Extensions.Logging;

namespace PxWeb
{
    /// <summary>
    /// Implementation class for the API cache
    /// </summary>
    public class PxCache: IPxCache
    {
        /// <summary>
        /// Delegate that waits for something after the cache has been cleaned
        /// </summary>
        public delegate bool CacheReenabler();
        private Func<bool> _coherenceChecker;

        private ILogger<PxCache> _logger;
        private string _cacheLock = "lock";
        private MemoryCache _cache;
        private bool _enableCache;
        private TimeSpan _cacheTime;

        public PxCache(ILogger<PxCache> logger)
        {
            _logger = logger;
            _cache = new MemoryCache(new MemoryCacheOptions());
            _enableCache = true;
            _cacheTime = new TimeSpan(0, 1, 0); // Later to be read from appsettings
        }

        /// <summary>
        /// Fetches a cached object from the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? Get<T>(Object key)
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

            return (T) _cache.Get(key);
        }

        /// <summary>
        /// Stores a object in the cache
        /// </summary>
        /// <param name="data"></param>
        public void Set(object key, object value)
        {
            if (_cache.Get(key) is null)
            {
                _logger.LogDebug("Adding key={0} to Cache", key);

                lock (_cacheLock)
                {
                    if (_cache.Get(key) is null)
                    {
                        _cache.Set(key, value, _cacheTime);
                    }
                }
            }
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        private void ClearCache()
        {
            lock (_cacheLock)
            {
                _cache.Compact(1.0);
            }
        }

        public bool IsEnabled()
        {
            return _enableCache;
        }

        public void Clear()
        {
            _logger.LogInformation("Cache cleared started");
            ClearCache();
            _logger.LogInformation("Cache cleared finished");
        }

        public void Disable()
        {
            _logger.LogInformation("Cache disabled");
            _enableCache = false;
        }

        public void Enable()
        {
            _logger.LogInformation("Cache enabled");
            _enableCache = true;
        }

        public void SetCoherenceChecker(Func<bool> coherenceChecker)
        {
            _coherenceChecker = coherenceChecker;
        }
    }
}