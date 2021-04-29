using System;
using System.Web.Http;

namespace PXWeb.API
{
    /// <summary>
    /// Cotroller for PxWeb cache manipulation
    /// </summary>
    public class CacheController : ApiController
    {
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
        public void Delete([FromUri] CacheType type)
        {
            // TODO: Authentication

            // TODO: Implement method to clear only cache corresponding the key parameter

            Management.PxContext.CacheController.Clear(getCacheItemType(type));
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