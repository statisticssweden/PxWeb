using System;

namespace PXWeb.API.Services
{
    public class CacheService : ICacheService
    {
        public void ClearCache(Type type)
        {
            Management.PxContext.CacheController.Clear(type);
        }
    }
}