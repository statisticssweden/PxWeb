using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public interface ISearcher
    {
        SearchResultContainer Find(string? query, int pageSize, int pageNumber, int? pastdays, bool includediscontinued = false);
        SearchResult FindTable(string tableId);
    }
}
