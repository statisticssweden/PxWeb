using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PXWeb.API
{
    /// <summary>
    /// Cotroller for PxWeb cache manipulation
    /// </summary>
    public class CacheController : ApiController
    {
        // TODO: Inject
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(CacheController));
        private Services.ICacheService _service;

        public CacheController(Services.ICacheService service)
        {
            _service = service;
        }

        public enum CacheType
        {
            ApiCache,
            InMemoryCache,
            SavedQueryPaxiomCache
        }

        /// <summary>
        /// Method to clear specific cache item
        /// </summary>
        /// <param name="type">Type of cache item to be cleared</param>
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] CacheType type)
        {
            var statusCode = HttpStatusCode.NoContent;
            try {
                if (IsAuthenticated())
                {
                    _service.ClearCache(getCacheItemType(type));
                    _logger.Info($"Cleared cache with key type: {type}");
                }
                else
                {
                    statusCode = HttpStatusCode.Forbidden;
                }
            } catch(Exception e) {
                _logger.Error(e);
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Request.CreateResponse(statusCode);
        }

        public bool IsAuthenticated()
        {
            const string KEYNAME = "APIKey";
            var key = Environment.GetEnvironmentVariable(KEYNAME);
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("APIKey is not set to environment variables.");
            }
            bool isAuthenticated = Request.Headers.Contains(KEYNAME) && Request.Headers.GetValues(KEYNAME).First().Equals(key);
            return isAuthenticated;
        }

        private Type getCacheItemType(CacheType typeParameter)
        {
            Type cacheItemType = null;
            switch (typeParameter) {
                case CacheType.ApiCache:
                    cacheItemType = typeof(PCAxis.Api.ApiCache);
                break;
                case CacheType.InMemoryCache:
                    cacheItemType = typeof(InMemoryCache);
                    break;
                case CacheType.SavedQueryPaxiomCache:
                    cacheItemType = typeof(Management.SavedQueryPaxiomCache);
                    break;
            }
            return cacheItemType;
        }
    }
}