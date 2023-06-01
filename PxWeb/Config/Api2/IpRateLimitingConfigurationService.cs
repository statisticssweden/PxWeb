using Microsoft.Extensions.Options;

namespace PxWeb.Config.Api2
{
    public class IpRateLimitingConfigurationService : IIpRateLimitingConfigurationService
    {
        private readonly IpRateLimitingConfigurationOptions _configOptions;

        public IpRateLimitingConfigurationService(IOptions<IpRateLimitingConfigurationOptions> configOptions)
        {
            _configOptions = configOptions.Value;
        }
        
        public IpRateLimitingConfigurationOptions GetConfiguration()
        {
            return  _configOptions;
        } 
    }
}
