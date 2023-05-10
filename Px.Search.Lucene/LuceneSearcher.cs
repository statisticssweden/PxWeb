using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Documents;
using Lucene.Net.Queries;
using System.Drawing.Printing;

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
        /// <param name="query"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pastdays"></param>
        /// <param name="includediscontinued"></param>
        /// <returns></returns>
        public SearchResultContainer Find(string? query, int pageSize, int pageNumber, int? pastdays, bool includediscontinued = false)
        {

            var skipRecords = pageSize * (pageNumber - 1);
            var searchResultContainer = new SearchResultContainer();
            var searchResultList = new List<SearchResult>();
            string[] fields = GetSearchFields();
            LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
            Query luceneQuery;
            QueryParser queryParser = new MultiFieldQueryParser(luceneVersion,
                                                       fields,
                                                       new StandardAnalyzer(luceneVersion));
            BooleanFilter filter = new BooleanFilter();
            queryParser.DefaultOperator = _defaultOperator;

            luceneQuery = string.IsNullOrEmpty(query) ? queryParser.Parse("*:*") : queryParser.Parse(query);

            if (!string.IsNullOrEmpty(pastdays.ToString()))
            {
                var pastDay = DateTools.DateToString(DateTime.Now.AddDays(-Convert.ToDouble(pastdays)), DateResolution.HOUR);
                filter.Add(new FilterClause(FieldCacheRangeFilter.NewStringRange(SearchConstants.SEARCH_FIELD_UPDATED, lowerVal: pastDay, includeLower: true, upperVal: null, includeUpper: false), Occur.MUST));
            }

            if (!includediscontinued)
            {
                var areaFilter = new TermsFilter(new Term(SearchConstants.SEARCH_FIELD_DISCONTINUED, "true"));
                filter.Add(new FilterClause(areaFilter, Occur.MUST_NOT));
            }

            TopDocs topDocs;
            topDocs = filter.Count() > 0 ? _indexSearcher.Search(luceneQuery, filter, skipRecords + pageSize)
                : _indexSearcher.Search(luceneQuery, skipRecords + pageSize);

            ScoreDoc[] scoreDocs = topDocs.ScoreDocs;
            var pages = (double)topDocs.TotalHits / (double)pageSize;
            int totalpages;

            totalpages = Convert.ToInt32(Math.Ceiling(pages));

            if (totalpages < pageNumber && totalpages != 0)
            {
                searchResultContainer.outOfRange = true;
                return searchResultContainer;
            }

            for (int i = skipRecords; i < topDocs.TotalHits; i++)
            {
                if (i > (skipRecords + pageSize) - 1)
                {
                    break;
                }
                Document doc = _indexSearcher.Doc(scoreDocs[i].Doc);
                searchResultList.Add(GetSearchResult(doc));
            }

            searchResultContainer.pageNumber = pageNumber;
            searchResultContainer.pageSize = pageSize;
            searchResultContainer.totalPages = totalpages;
            searchResultContainer.totalElements = topDocs.TotalHits;
            searchResultContainer.searchResults = searchResultList;

            return searchResultContainer;

        }
        /// <summary>
        /// Search in index on table id
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        public SearchResult FindTable(string tableId)
        {
            
            string[] field = new[] { SearchConstants.SEARCH_FIELD_SEARCHID };
            LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
            Query luceneQuery;
            QueryParser queryParser = new MultiFieldQueryParser(luceneVersion,
                                                       field,
                                                       new StandardAnalyzer(luceneVersion));
            queryParser.DefaultOperator = _defaultOperator;
            luceneQuery = queryParser.Parse(tableId);

            TopDocs topDocs;
            topDocs = _indexSearcher.Search(luceneQuery, 1);

            Document doc = _indexSearcher.Doc(topDocs.ScoreDocs[0].Doc);
            return GetSearchResult(doc);
                      
        }

        /// <summary>
        /// Get SearchResultList
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private SearchResult GetSearchResult(Document doc)
        {
            DateTime updated;
            bool discontinued;
            var searchResultList = new List<SearchResult>();

            var searchResult = new SearchResult(
                    doc.Get(SearchConstants.SEARCH_FIELD_DOCID),
                    doc.Get(SearchConstants.SEARCH_FIELD_TITLE),
                    doc.Get(SearchConstants.SEARCH_FIELD_CATEGORY),
                    doc.Get(SearchConstants.SEARCH_FIELD_FIRSTPERIOD),
                    doc.Get(SearchConstants.SEARCH_FIELD_LASTPERIOD),
                    doc.Get(SearchConstants.SEARCH_FIELD_VARIABLES).Split(" ")
                );
            searchResult.Description = doc.Get(SearchConstants.SEARCH_FIELD_DESCRIPTION);
            searchResult.SortCode = doc.Get(SearchConstants.SEARCH_FIELD_SORTCODE);
            if (DateTime.TryParse(doc.Get(SearchConstants.SEARCH_FIELD_UPDATED), out updated))
            {
                searchResult.Updated = updated;
            }
            if (bool.TryParse(doc.Get(SearchConstants.SEARCH_FIELD_DISCONTINUED), out discontinued))
            {
                searchResult.Discontinued = discontinued;
            }
            searchResult.Category = doc.Get(SearchConstants.SEARCH_FIELD_CATEGORY);
            searchResult.FirstPeriod = doc.Get(SearchConstants.SEARCH_FIELD_FIRSTPERIOD);
            searchResult.LastPeriod = doc.Get(SearchConstants.SEARCH_FIELD_LASTPERIOD);
            searchResult.Tags = doc.Get(SearchConstants.SEARCH_FIELD_TAGS).Split(" ");
            searchResult.Updated = String.IsNullOrEmpty(doc.Get(SearchConstants.SEARCH_FIELD_UPDATED)) ? null : DateTools.StringToDate(doc.Get(SearchConstants.SEARCH_FIELD_UPDATED));
            searchResult.Label = doc.Get(SearchConstants.SEARCH_FIELD_TITLE);
            

            return searchResult;


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
