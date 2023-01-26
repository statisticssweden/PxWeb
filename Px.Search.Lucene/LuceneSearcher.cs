using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Search.Lucene
{
    public class LuceneSearcher : ISearcher
    {
        private string _indexDirectory = "";
        private IndexReader _reader;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="indexDirectory">Index directory</param>
        public LuceneSearcher(string indexDirectory)
        {
            if (string.IsNullOrWhiteSpace(indexDirectory))
            {
                throw new ArgumentNullException("Index directory not defined for Lucene");
            }

            _indexDirectory = indexDirectory;
        }

        public IEnumerable<SearchResult> Find(string searchExpression, string language)
        {
            //Search the right index depending on the language and give back a search result.
            // See https://github.com/statisticssweden/Px.Search.Lucene/blob/main/Px.Search.Lucene/LuceneSearcher.cs
            throw new NotImplementedException();
        }
    }
}
