using System;
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
            // TODO: Authentication

            var statusCode = HttpStatusCode.NoContent;
            try {
                _service.ClearCache(getCacheItemType(type));
                _logger.Info($"Cleared cache item for: {type}");

            } catch(Exception e) {
                _logger.Error($"Unhandled exception occurred while trying to clear cache item with type: {type}", e);
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Request.CreateResponse(statusCode);
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