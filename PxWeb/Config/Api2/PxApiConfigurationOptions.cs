using System;
using System.Collections.Generic;
using PxWeb.Api2.Server.Models;

namespace PxWeb.Config.Api2
{
    public class PxApiConfigurationOptions
    {
        public string ApiVersion { get; set; } = "2.0";
        public List<Language> Languages { get; set; } = new List<Language>();
        public string DefaultLanguage { get; set; } = String.Empty;
        public int MaxDataCells { get; set; } = 1;
        public int MaxCalls { get; set; } = 1;
        public int TimeWindow { get; set; } = 1;
        public List<ApiFeature> Features { get; set; } = new List<ApiFeature>();
        public string License { get; set; }
        public List<SourceReference> SourceReferences { get; set; }
        public Cors Cors { get; set; }
        public int CacheTime { get; set; } = 5;
        public int PageSize { get; set; }
        public string BaseURL { get; set; } = String.Empty;
    }
}
