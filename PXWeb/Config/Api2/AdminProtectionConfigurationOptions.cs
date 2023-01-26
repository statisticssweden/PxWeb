using System.Collections.Generic;

namespace PxWeb.Config.Api2
{
    public class AdminProtectionConfigurationOptions
    {
        public List<string> IpWhitelist { get; set; } = new List<string>();
    }
}
