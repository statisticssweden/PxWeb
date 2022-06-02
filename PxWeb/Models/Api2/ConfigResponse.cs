using System.Collections.Generic;

namespace PxWeb.Models.Api2
{
    public class ConfigResponse
    {
        public string ApiVersion { get; set; }

        public List<Language> Languages { get; set; }

        public string DefaultLanguage { get; set; }

        public int MaxDataCells { get; set; }

        public int MaxCalls { get; set; }

        public int TimeWindow { get; set; }

        public List<SourceReference> SourceReferences { get; set; }

        public string License { get; set; }
        
        public List<Feature> Features { get; set; }

        public Cors Cors { get; set; }
    }
}