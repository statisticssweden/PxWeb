using System.Collections.Generic;

namespace PxWeb.Models.Api2
{
    public class ConfigResponse
    {


        public string ApiVersion { get; set; }

        public IEnumerable<Language> Languages { get; set; }

        public Language DefaultLanguage { get; set; }

        public int MaxDataCells { get; set; }

        public int MaxCalls { get; set; }

        public int TimeWindow { get; set; }

        public IEnumerable<SourceReference> SourceReferences { get; set; }

        public string Licens { get; set; }

        public IEnumerable<Feature> Features { get; set; }

    }
}