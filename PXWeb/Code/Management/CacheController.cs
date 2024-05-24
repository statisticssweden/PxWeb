using PX.Web.Interfaces.Cache;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace PXWeb.Management
{
    /// <summary>
    /// Class that handles the PX-Caches
    /// </summary>
    public class CacheController : IPxCacheController
    {
        static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(CacheController));

        /// <summary>
        /// List of PX caches that shall be handled by the cache controller
        /// </summary>
        private List<IPxCache> _caches;

        /// <summary>
        /// List of times when we want to clear all PX caches automatically
        /// </summary>
        private List<string> _times;

        /// <summary>
        /// Initialize the cache controller
        /// </summary>
        /// <param name="lstCache"></param>
        public void Initialize(List<IPxCache> lstCache)
        {
            _caches = lstCache;
            InitializeSchedualClear();
        }

        /// <summary>
        /// Clear all caches
        /// </summary>
        public void Clear()
        {
            ClearCaches();
        }

        /// <summary>
        /// Clears cahe item with provided type
        /// </summary>
        /// <param name="type">Type of cache item to clear</param>
        public void Clear(Type type)
        {
            if (type != null && _caches != null)
            {
                _caches.Find(c => c.GetType().Equals(type))?.Clear();
            }
        }

        /// <summary>
        /// Set up the times when we want to clear all PX caches automatically
        /// </summary>
        private void InitializeSchedualClear()
        {
            _times = new List<string>();

            char[] separators = new char[] { ',', ';' }; // Comma and semicolon are allowed as separators
            string times = PXWeb.Settings.Current.Features.General.ClearCache;
            string[] parts = times.Split(separators);

            Regex checktime = new Regex(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            foreach (var time in parts)
            {
                if (checktime.IsMatch(time))
                {
                    _times.Add(time);
                }
            }

            _times.Sort((x, y) => string.Compare(x, y));
            SchedualNextClear();
        }

        /// <summary>
        /// Schedularer that scheduals the next clear of the the PX caches
        /// </summary>
        private void SchedualNextClear()
        {
            DateTime now = DateTime.Now;
            DateTime? t = null;
            foreach (var time in _times)
            {
                DateTime dt = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);
                if ((dt - now).Ticks > 0)
                {
                    t = dt;
                    break;
                }
            }

            if (!t.HasValue && _times.Count > 0)
            {
                t = DateTime.ParseExact(_times[0], "HH:mm", CultureInfo.InvariantCulture);
            }

            if (t.HasValue)
            {
                if (now > t.Value)
                {
                    t = t.Value.AddDays(1);
                }
                HttpRuntime.Cache.Add(Guid.NewGuid().ToString(), t.Value, null, t.Value, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, new CacheItemRemovedCallback(RemovedCallback));
                _logger.InfoFormat("Next scheduled clear at {0}", t.Value.ToString());
            }
        }

        /// <summary>
        /// Callback to clean the PX caches and also resechdual the next cleaning of the PX caches
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <param name="r"></param>
        private void RemovedCallback(String k, Object v, CacheItemRemovedReason r)
        {
            ClearCaches();
            SchedualNextClear();
        }

        /// <summary>
        /// Clear all the caches
        /// </summary>
        private void ClearCaches()
        {
            if (_caches != null)
            {
                _logger.InfoFormat("Clear all caches ({0})", _caches.Count.ToString());

                foreach (IPxCache cache in _caches)
                {
                    if (cache.DefaultEnabled)
                    {
                        cache.Clear();
                    }
                }
            }
        }
    }
}