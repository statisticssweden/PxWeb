using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public class SearchResult : TableInformation
    {
        public SearchResult(string id, string label, string category, string firstPeriod, string lastPeriod, string[] variableNames) : base(id, label, category, firstPeriod, lastPeriod, variableNames)
        {
        }

        public float Score { get; set; }

     }
}
