using System;
using System.Data;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;

namespace PCAxis.Query
{
    public class MetaDb
    {
        [JsonProperty("dbid")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
