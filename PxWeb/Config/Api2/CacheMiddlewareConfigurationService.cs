using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace PxWeb.Config.Api2
{
    public class CacheMiddlewareConfigurationService : ICacheMiddlewareConfigurationService
    {
        private readonly CacheMiddlewareConfigurationOptions _cacheMiddlewareConfigurationOptions;
        public CacheMiddlewareConfigurationService(IOptions<CacheMiddlewareConfigurationOptions> cacheMiddlewareConfigurationOptions)
        {
            _cacheMiddlewareConfigurationOptions = cacheMiddlewareConfigurationOptions.Value;
        }

        public CacheMiddlewareConfigurationOptions GetConfiguration()
        {
            return _cacheMiddlewareConfigurationOptions;
        }
    }
}
