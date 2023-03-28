using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Lucene.Net.Documents;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using System.IO;
using System.Security.Policy;
using Lucene.Net.Search;
using static System.Net.WebRequestMethods;

namespace Px.Search.Lucene
{

    //TODO look at https://github.com/statisticssweden/Px.Search.Lucene/blob/main/Px.Search.Lucene/LuceneIndexer.cs for inspiration

    public class LuceneIndex : IIndex
    {
        private string _indexDirectoryBase = "";
        private string _indexDirectoryCurrent = "";
        private IndexWriter? _writer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="indexDirectory">Index directory</param>
        public LuceneIndex(string indexDirectory)
        {
            if (string.IsNullOrWhiteSpace(indexDirectory))
            {
                throw new ArgumentNullException("Index directory not defined for Lucene");
            }

            _indexDirectoryBase = indexDirectory;
        }

        /// <summary>
        /// Get Lucene.Net IndexWriter object
        /// </summary>
        /// <param name="create">
        /// If true, the existing index will be overwritten
        /// If false, the existing index will be appended
        /// </param>
        /// <returns>IndexWriter object. If the Index directory is locked, null is returned</returns>
        private IndexWriter CreateIndexWriter(bool create)
        {
            FSDirectory fsDir = FSDirectory.Open(_indexDirectoryCurrent);
            if (IndexWriter.IsLocked(fsDir))
            {
                return null;
            }

            LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
            Analyzer analyzer = new StandardAnalyzer(luceneVersion);

            IndexWriterConfig config = new IndexWriterConfig(luceneVersion, analyzer)
            {
                // Overwrite or append existing index
                OpenMode = create ? OpenMode.CREATE : OpenMode.CREATE_OR_APPEND 
            };

            IndexWriter writer = new IndexWriter(fsDir, config);

            return writer;
        }

        public void BeginUpdate(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException("Language not specified");
            }

            _indexDirectoryCurrent = Path.Combine(_indexDirectoryBase, language);
            _writer = CreateIndexWriter(false);

            if (_writer == null)
            {
                throw new Exception("Could not create IndexWriter. Index directory may be locked by another IndexWriter");
            }
        }

        public void BeginWrite(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException("Language not specified");
            }

            _indexDirectoryCurrent = Path.Combine(_indexDirectoryBase, language);
            _writer = CreateIndexWriter(true);  

            if (_writer == null)
            {
                throw new Exception("Could not create IndexWriter. Index directory may be locked by another IndexWriter");
            }
        }

        public void EndUpdate(string language)
        {
            EndWrite(language);
        }

        public void EndWrite(string language)
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }
        }

        public void AddEntry(TableInformation tbl, PXMeta meta)
        {
            Document doc = GetDocument(tbl, meta);
            if (_writer != null)
            {
                _writer.AddDocument(doc);
            }
        }

        public void UpdateEntry(TableInformation tbl, PXMeta meta)
        {
            Document doc = GetDocument(tbl, meta);
            if (_writer != null)
            {
                _writer.UpdateDocument(new Term(SearchConstants.SEARCH_FIELD_DOCID, doc.Get(SearchConstants.SEARCH_FIELD_DOCID)), doc);
            }
        }

        public void RemoveEntry(string id)
        {
            //check if document exists, if true deletes existing
            var searchQuery = new TermQuery(new Term(SearchConstants.SEARCH_FIELD_DOCID, id));
            if (_writer != null)
            {
                _writer.DeleteDocuments(searchQuery);
            }
        }


        /// <summary>
        /// Get Document object representing the table
        /// </summary>
        /// <param name="tbl">TableInformation object</param>
        /// <param name="meta">PxMeta object</param>
        /// <returns>Document object representing the table</returns>
        private Document GetDocument(TableInformation tbl, PXMeta meta)
        {
            Document doc = new Document();
            DateTime updated2;
            string strUpdated = "";

            if (tbl != null && meta != null)
            {
                if (string.IsNullOrEmpty(tbl.Label) || string.IsNullOrEmpty(meta.Matrix) || meta.Variables.Count == 0)
                {
                    return doc;
                }

                if (tbl.Updated != null)
                {
                    updated2 = tbl.Updated.Value;
                    strUpdated = DateTools.DateToString(updated2, DateResolution.SECOND);
                    //DateTime d1 = Convert.ToDateTime(updated2);
                    //doc.Add(new Field("Registered_Date", DateTools.DateToString(d1, DateTools.Resolution.SECOND), Field.Store.YES, Field.Index.ANALYZED));
                }

                doc.Add(new StringField(SearchConstants.SEARCH_FIELD_DOCID, tbl.Id, Field.Store.YES)); // Used as id when updating a document - NOT searchable!!!
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_SEARCHID, tbl.Id, Field.Store.NO)); // Used for finding a document by id - will be used for generating URL from just the tableid - Searchable!!!
                doc.Add(new StringField(SearchConstants.SEARCH_FIELD_UPDATED, strUpdated, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_MATRIX, meta.Matrix, Field.Store.YES)); 
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_TITLE, tbl.Label, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_DESCRIPTION, tbl.Description, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_SORTCODE, tbl.SortCode, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_CATEGORY, tbl.Category, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_FIRSTPERIOD, tbl.FirstPeriod, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_LASTPERIOD, tbl.LastPeriod, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_VARIABLES, string.Join(" ", tbl.VariableNames), Field.Store.YES)); 
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_PERIOD, meta.GetTimeValues(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_VALUES, meta.GetAllValues(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_CODES, meta.GetAllCodes(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_GROUPINGS, meta.GetAllGroupings(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_GROUPINGCODES, meta.GetAllGroupingCodes(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_VALUESETS, meta.GetAllValuesets(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_VALUESETCODES, meta.GetAllValuesetCodes(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_DISCONTINUED, tbl.Discontinued == null ? "false" : tbl.Discontinued.ToString(), Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_TAGS, GetAllTags(tbl.Tags), Field.Store.YES));
                if (!string.IsNullOrEmpty(meta.Synonyms))
                {
                    doc.Add(new TextField(SearchConstants.SEARCH_FIELD_SYNONYMS, meta.Synonyms, Field.Store.NO));
                }
            }

            return doc;
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.Rollback();
                _writer = null;
            }
        }

        public static string GetAllTags(string[] tags)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string tag in tags)
            {
                builder.Append(tag);
                builder.Append(" ");
            }
            return builder.ToString();
        }
    }
}
