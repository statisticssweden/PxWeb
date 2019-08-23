using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query
{
    public class SavedQuery
    {
        [JsonProperty("sources", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<TableSource> Sources { get; set; }
        [JsonProperty("workflow", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<WorkStep> Workflow { get; set; }
        [JsonProperty("output", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Output Output { get; set; }
        [JsonProperty("stats", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public QueryStats Stats { get; set; }
        [JsonProperty("safe", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Safe { get; set; }
        [JsonProperty("timeDependent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool TimeDependent { get; set; }
        public string LoadedQueryName { get; set; }

        public SavedQuery()
        {
            Sources = new List<TableSource>();
            Workflow = new List<WorkStep>();
            this.Output = new Output();
            Stats = new QueryStats();
            Safe = false;
            TimeDependent = false;
        }
    }

    public class Output
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("params", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, string> Params { get; private set; }

        public Output()
        {
            Params = new Dictionary<string, string>();
        }
    }

    public class QueryStats
    {
        [JsonProperty("creator", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Creator { get; set; }
        [JsonProperty("removeable", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Removeable { get; set; }
        [JsonProperty("runs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long RunCounter { get; set; }
        [JsonProperty("fails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long FailCounter { get; set; }
        [JsonProperty("lastrunned", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime LastExecuted { get; set; }

        public QueryStats()
        {
            Removeable = true;
        }
    }
}
