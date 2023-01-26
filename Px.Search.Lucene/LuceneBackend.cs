using Px.Search.Lucene.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search.Lucene
{
    public class LuceneBackend : ISearchBackend
    {

        private readonly ILuceneConfigurationService _luceneConfigurationService;

        public LuceneBackend(ILuceneConfigurationService luceneConfigurationService)
        {
            _luceneConfigurationService = luceneConfigurationService;   
        }

        public IIndex GetIndex()
        {
            string path = _luceneConfigurationService.GetIndexDirectoryPath();
            return new LuceneIndex(path);
        }

        public ISearcher GetSearcher()
        {
            string path = _luceneConfigurationService.GetIndexDirectoryPath();
            return new LuceneSearcher(path);
        }

 

    }
}
