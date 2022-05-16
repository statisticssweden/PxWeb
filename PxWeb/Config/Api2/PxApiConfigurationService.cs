using Microsoft.Extensions.Options;

namespace PxWeb.Config.Api2
{
    public class PxApiConfigurationService : IPxApiConfigurationService
    {
        private readonly PxApiConfigurationOptions _configOptions;

        public PxApiConfigurationService(IOptions<PxApiConfigurationOptions> configOptions)
        {
            _configOptions = configOptions.Value;
        }
        
        public PxApiConfigurationOptions GetConfiguration()
        {
            return _configOptions;
        }
    }
}
