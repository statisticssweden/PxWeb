using System;
using System.Collections.Generic;
using PxWeb.Api2.Server.Models;

namespace PxWeb.Config.Api2
{
    public class IpRateLimitingConfigurationOptions
    {
        public List<GeneralRules> GeneralRules { get; set; } = new List<GeneralRules>();

    }
}
