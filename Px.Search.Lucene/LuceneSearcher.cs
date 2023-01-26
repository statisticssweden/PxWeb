using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net;
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
        private IndexSearcher _indexSearcher;

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
            FSDirectory fsDir = FSDirectory.Open(indexDirectory);

            IndexReader reader = DirectoryReader.Open(fsDir);
            _indexSearcher = new IndexSearcher(reader);

            _indexDirectory = indexDirectory;
        }

        public IEnumerable<SearchResult> Find(string searchExpression, string language)
        {
            //Search the right index depending on the language and give back a search result.
            // See https://github.com/statisticssweden/Px.Search.Lucene/blob/main/Px.Search.Lucene/LuceneSearcher.cs

            //List<SearchResultItem> searchResult = new List<SearchResultItem>();
            string[] fields = GetSearchFields();
            LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;

            QueryParser qp = new MultiFieldQueryParser(luceneVersion,
                                                       fields,
                                                       new StandardAnalyzer(luceneVersion));
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get fields in index to search in
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string[] GetSearchFields()
        {
            string[] fields;

            // Default fields
            fields = new[] { SearchConstants.SEARCH_FIELD_DOCID,
                                SearchConstants.SEARCH_FIELD_UPDATED,
                                SearchConstants.SEARCH_FIELD_MATRIX,
                                SearchConstants.SEARCH_FIELD_TITLE,
                                SearchConstants.SEARCH_FIELD_DESCRIPTION,
                                SearchConstants.SEARCH_FIELD_SORTCODE,
                                SearchConstants.SEARCH_FIELD_CATEGORY,
                                SearchConstants.SEARCH_FIELD_FIRSTPERIOD,
                                SearchConstants.SEARCH_FIELD_LASTPERIOD,
                                SearchConstants.SEARCH_FIELD_VARIABLES,
                                SearchConstants.SEARCH_FIELD_DISCONTINUED,
                                SearchConstants.SEARCH_FIELD_TAGS
            };

            return fields;
        }
    }
}
