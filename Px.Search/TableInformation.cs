using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public class TableInformation
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string SortCode { get; set; }
        public DateTime? Updated { get; set; }
        public bool? Discontinued { get; set; }
        public string Category { get; set; }
        public string FirstPeriod { get; set; }
        public string LastPeriod { get; set; }
        public string[] VariableNames { get; set; }
        public string[] Tags { get; set; }
    }
}
