using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace PXWeb.API
{
    /// <summary>
    /// Cotroller for PxWeb cache manipulation
    /// </summary>
    public class CacheController : ApiController
    {
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(CacheController));
        private static string _apiKey;
        const string _keyName = "APIKey";

        public CacheController()
        {
            _apiKey = Environment.GetEnvironmentVariable(_keyName);
            if(string.IsNullOrEmpty(_apiKey))
            {
                _logger.Error("APIKey is not set to environment variables.");
            }
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
        public IHttpActionResult Delete([FromUri] CacheType type)
        {
            // TODO: Authentication

            _apiKey = Environment.GetEnvironmentVariable(_keyName);
            if (string.IsNullOrEmpty(_apiKey))
            {
                _logger.Error("APIKey is not set to environment variables.");
                return InternalServerError();
            }

            if (!Request.Headers.Contains(_keyName) || !Request.Headers.GetValues(_apiKey).First().Equals(_keyName))
            {
                return Unauthorized();
            }

            // TODO: Implement method to clear only cache corresponding the key parameter

            Management.PxContext.CacheController.Clear(getCacheItemType(type));

            return StatusCode(t.HttpStatusCode.NoContent);
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