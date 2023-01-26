using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace PxWeb.Config.Api2
{
    public class AdminProtectionConfigurationService : IAdminProtectionConfigurationService
    {
        private readonly AdminProtectionConfigurationOptions _adminProtectionOptions;
        public AdminProtectionConfigurationService(IOptions<AdminProtectionConfigurationOptions> adminProtectionOptions)
        {
            _adminProtectionOptions = adminProtectionOptions.Value;
        }

        public AdminProtectionConfigurationOptions GetConfiguration()
        {
            return _adminProtectionOptions;
        }
    }
}
