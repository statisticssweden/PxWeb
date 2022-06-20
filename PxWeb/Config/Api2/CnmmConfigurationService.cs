using Microsoft.Extensions.Options;

namespace PxWeb.Config.Api2
{
    public class CnmmConfigurationService : ICnmmConfigurationService
    {
        private readonly CnmmConfigurationOptions _configOptions;

        public CnmmConfigurationService(IOptions<CnmmConfigurationOptions> configOptions)
        {
            _configOptions = configOptions.Value;
        }

        public CnmmConfigurationOptions GetConfiguration()
        {
            return _configOptions;
        }
    }
}
