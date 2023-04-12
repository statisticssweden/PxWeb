using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public class SearchResultContainer
    {
        public IEnumerable<SearchResult> searchResults;
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalElements { get; set; }
        public int totalPages { get; set; }
        public bool outOfRange { get; set; }

    }
}
