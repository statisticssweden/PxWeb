using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query
{
    public class TableSource
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("default", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Default { get; set; }
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("dbid", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string DatabaseId { get; set; }
        [JsonProperty("sourceType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SourceIdType { get; set; }
        [JsonProperty("source", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Source { get; set; }
        [JsonProperty("lang", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Language { get; set; }
        [JsonProperty("queries", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<Query> Quieries { get; set; }

        public TableSource()
        {
            Quieries = new List<Query>();
        }

    }
}
