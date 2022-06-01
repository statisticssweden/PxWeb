using System;
using System.Collections.Generic;
using PxWeb.Models.Api2;

namespace PxWeb.Config.Api2
{
    public class PxApiConfigurationOptions
    {
        public string DataSource { get; set; } = String.Empty;
        public string ApiVersion { get; set; } = "2.0";
        public List<Language> Languages { get; set; } = new List<Language>();
        public string DefaultLanguage { get; set; } = String.Empty;
        public int MaxDataCells { get; set; } = 1;
        public int MaxCalls { get; set; } = 1;
        public int TimeWindow { get; set; } = 1;
        public List<Feature> Features { get; set; } = new List<Feature>();
        public string License { get; set; }
        public List<SourceReference> SourceReferences { get; set; }
        public Cors Cors { get; set; } }
}
