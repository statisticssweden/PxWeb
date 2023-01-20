using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public class SearchResult : TableInformation
    {
        public float Score { get; set; }

        public SearchResult(string id)
        {
            Id = id;
        }
    }
}
