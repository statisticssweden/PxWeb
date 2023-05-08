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
        public SearchResultContainer Find(string query, string language, int? pastdays, bool includediscontinued, int pageSize = 20, int pageNumber = 1 )
        {
            var searcher = _backend.GetSearcher(language);

            return searcher.Find(query, pageSize, pageNumber, pastdays, includediscontinued);
        }
        public SearchResultContainer FindTable(string tableId, string language)
        {
            var searcher = _backend.GetSearcher(language);

            return searcher.FindTable(tableId);
        }

    }
}
