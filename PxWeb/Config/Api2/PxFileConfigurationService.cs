using Microsoft.Extensions.Options;

namespace PxWeb.Config.Api2
{
    public class PxFileConfigurationService : IPxFileConfigurationService
    {
        private readonly PxFileConfigurationOptions _configOptions;

        public PxFileConfigurationService(IOptions<PxFileConfigurationOptions> configOptions)
        {
            _configOptions = configOptions.Value;
        }

        public PxFileConfigurationOptions GetConfiguration()
        {
            return _configOptions;
        }
    }
}
