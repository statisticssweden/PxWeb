namespace PXWeb.API.Services
{
    public class CacheService : ICacheService
    {
        public void ClearCache()
        {
            Management.PxContext.CacheController.Clear();
        }
    }
}