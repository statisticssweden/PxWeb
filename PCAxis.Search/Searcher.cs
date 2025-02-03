using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using PCAxis.Paxiom.Extensions;

namespace PCAxis.Search
{
    /// <summary>
    /// Class for searching in a index.
    /// Encapsulates a Lucene.Net IndexSearcher.
    /// </summary>
    public class Searcher
    {
        private string _indexDirectory;
        private IndexSearcher _indexSearcher;
        private DateTime _creationTime;
        private static QueryParser.Operator _defaultOperator = QueryParser.Operator.OR;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="indexDirectory">Directory to search</param>
        public Searcher(string indexDirectory)
        {
            _indexDirectory = indexDirectory;
            _creationTime = DateTime.Now; 

            FSDirectory fsDir = FSDirectory.Open(_indexDirectory);

            try
            {
                _indexSearcher = new IndexSearcher(fsDir, true); //Read-only = true
            }
            catch (Exception)
            {
                throw ;
            }
        }

        public List<SearchResultItem> Search(string text, string filter = "", int resultListLength = 250 )
        {
            List<SearchResultItem> searchResult = new List<SearchResultItem>();
            string[] fields = GetSearchFields(filter);

            QueryParser qp = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30,
                                                       fields,
                                                       new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));

            qp.DefaultOperator = _defaultOperator;

            Lucene.Net.Search.Query q = qp.Parse(text);
            TopDocs topDocs = _indexSearcher.Search(q, resultListLength);
            //hits = topDocs.TotalHits;
            foreach (var d in topDocs.ScoreDocs)
            {
                Document doc = _indexSearcher.Doc(d.Doc);
                searchResult.Add(new SearchResultItem()
                {
                    Path = doc.Get(SearchConstants.SEARCH_FIELD_PATH),
                    Table = doc.Get(SearchConstants.SEARCH_FIELD_TABLE),
                    Title = doc.Get(SearchConstants.SEARCH_FIELD_TITLE),
                    Score = d.Score,
                    Published = GetPublished(doc)
                });
            }

            return searchResult;
        }

        /// <summary>
        /// The time the Searcher was created
        /// </summary>
        public DateTime CreationTime
        {
            get
            {
                return _creationTime;
            }
        }

        /// <summary>
        /// Default operator AND/OR that will be used when more than 1 word is specified in a search querie
        /// </summary>
        public static QueryParser.Operator DefaultOperator 
        {
            get
            { 
                return _defaultOperator;
            }
            set
            {
                _defaultOperator = value;
            }
        }

        /// <summary>
        /// Get fields in index to search in
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string[] GetSearchFields(string filter)
        {
            string[] fields;


            if (string.IsNullOrEmpty(filter))
            {
                // Default fields
                fields = new[] { SearchConstants.SEARCH_FIELD_SEARCHID, 
                                 SearchConstants.SEARCH_FIELD_TITLE, 
                                 SearchConstants.SEARCH_FIELD_VALUES, 
                                 SearchConstants.SEARCH_FIELD_CODES, 
                                 SearchConstants.SEARCH_FIELD_MATRIX, 
                                 SearchConstants.SEARCH_FIELD_VARIABLES, 
                                 SearchConstants.SEARCH_FIELD_PERIOD, 
                                 SearchConstants.SEARCH_FIELD_GROUPINGS, 
                                 SearchConstants.SEARCH_FIELD_GROUPINGCODES, 
                                 SearchConstants.SEARCH_FIELD_VALUESETS, 
                                 SearchConstants.SEARCH_FIELD_VALUESETCODES,
                                 SearchConstants.SEARCH_FIELD_SYNONYMS };
            }
            else
            {
                // Get fields from filter
                fields = filter.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return fields;
        }

        private DateTime GetPublished(Document doc)
        {
            DateTime published = DateTime.MinValue;
            string publishedStr = doc.Get(SearchConstants.SEARCH_FIELD_PUBLISHED);

            if (!string.IsNullOrEmpty(publishedStr))
            {
                if (PxDate.IsPxDate(publishedStr))
                {
                    published = publishedStr.PxDateStringToDateTime();
                }
            }

            return published;
        }
    }
}
