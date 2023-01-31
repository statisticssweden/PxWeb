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
using static System.Net.Mime.MediaTypeNames;
using Lucene.Net.Documents;
using Microsoft.AspNetCore.Http;
using static Lucene.Net.Util.Fst.Util;

namespace Px.Search.Lucene
{
    public class LuceneSearcher : ISearcher
    {
        private IndexSearcher _indexSearcher;
        private static Operator _defaultOperator = Operator.OR;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="indexDirectory">Index directory</param>
        public LuceneSearcher(string indexDirectory, string language)
        {
            if (string.IsNullOrWhiteSpace(indexDirectory))
            {
                throw new ArgumentNullException("Index directory not defined for Lucene");
            }
          
            FSDirectory fsDir = FSDirectory.Open(Path.Combine(indexDirectory, language));

            IndexReader reader = DirectoryReader.Open(fsDir);
            _indexSearcher = new IndexSearcher(reader);
        }
        /// <summary>
        /// Search the right index depending on the language and give back a search result
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="language"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public IEnumerable<SearchResult> Find(string searchExpression, string language, int pageSize, int pageNumber)
        {
            // See https://github.com/statisticssweden/Px.Search.Lucene/blob/main/Px.Search.Lucene/LuceneSearcher.cs

            var skipRecords = pageSize * (pageNumber - 1);
            List<SearchResult> searchResultList = new List<SearchResult>();
            string[] fields = GetSearchFields();
            LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;

            QueryParser qp = new MultiFieldQueryParser(luceneVersion,
                                                       fields,
                                                       new StandardAnalyzer(luceneVersion));
            qp.DefaultOperator = _defaultOperator;
            Query q = qp.Parse(searchExpression);
            TopDocs topDocs = _indexSearcher.Search(q,skipRecords+pageSize);
            ScoreDoc[] scoreDocs = topDocs.ScoreDocs;

            for (int i = skipRecords; i < topDocs.TotalHits; i++)
            {
                if (i > (skipRecords + pageSize) - 1)
                {
                    break;
                }
                Document doc = _indexSearcher.Doc(scoreDocs[i].Doc);
                var searchResult = new SearchResult(
                    doc.Get(SearchConstants.SEARCH_FIELD_DOCID),
                    doc.Get(SearchConstants.SEARCH_FIELD_TITLE),
                    doc.Get(SearchConstants.SEARCH_FIELD_CATEGORY),
                    doc.Get(SearchConstants.SEARCH_FIELD_FIRSTPERIOD),
                    doc.Get(SearchConstants.SEARCH_FIELD_LASTPERIOD),
                    doc.Get(SearchConstants.SEARCH_FIELD_VARIABLES).Split(" ")
                );
                searchResult.Score= scoreDocs[i].Score; 
                searchResultList.Add(searchResult);
            }

            return searchResultList;
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
                                SearchConstants.SEARCH_FIELD_SEARCHID,                                
                                SearchConstants.SEARCH_FIELD_UPDATED,
                                SearchConstants.SEARCH_FIELD_MATRIX,
                                SearchConstants.SEARCH_FIELD_TITLE,
                                SearchConstants.SEARCH_FIELD_DESCRIPTION,
                                SearchConstants.SEARCH_FIELD_SORTCODE,
                                SearchConstants.SEARCH_FIELD_CATEGORY,
                                SearchConstants.SEARCH_FIELD_FIRSTPERIOD,
                                SearchConstants.SEARCH_FIELD_LASTPERIOD,
                                SearchConstants.SEARCH_FIELD_VARIABLES,
                                SearchConstants.SEARCH_FIELD_PERIOD,
                                SearchConstants.SEARCH_FIELD_VALUES,
                                SearchConstants.SEARCH_FIELD_CODES,
                                SearchConstants.SEARCH_FIELD_GROUPINGS,
                                SearchConstants.SEARCH_FIELD_GROUPINGCODES,
                                SearchConstants.SEARCH_FIELD_VALUESETS,
                                SearchConstants.SEARCH_FIELD_VALUESETCODES,
                                SearchConstants.SEARCH_FIELD_DISCONTINUED,
                                SearchConstants.SEARCH_FIELD_TAGS
            };

            return fields;
        }
       
    }
}
