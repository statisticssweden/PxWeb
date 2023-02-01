using Px.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search
{
    public class Searcher
    {
        private IDataSource _source;
        private ISearchBackend _backend;
        //private ILogger _logger;

        public Searcher(IDataSource dataSource, ISearchBackend backend)
        {
            _source = dataSource;
            _backend = backend;
        }
        public IEnumerable<SearchResult> Find(string searchExpression, string language, int pageSize = 20, int pageNumber = 1)
        {
            var searcher = _backend.GetSearcher(language);

            return searcher.Find(searchExpression, pageSize, pageNumber);
        }
    }
}
