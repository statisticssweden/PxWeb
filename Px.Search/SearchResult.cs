using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public class SearchResult
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public DateTime Updated { get; set; }
        public string[] Tags { get; set; }
        public string Category { get; set; }
        public string Source { get; set; }
        public float Score { get; set; }

        public SearchResult(string id)
        {
            Id = id;
        }
    }
}
