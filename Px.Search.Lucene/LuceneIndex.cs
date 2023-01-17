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
        private IndexWriter _writer;

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
        /// <returns>IndexWriter object. If the Index directory is locked, null is returned</returns>
        private IndexWriter CreateIndexWriter()
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
                OpenMode = OpenMode.CREATE_OR_APPEND // Creates a new index if one does not exist, otherwise it opens the index and documents will be appended.
            };

            IndexWriter writer = new IndexWriter(fsDir, config);

            return writer;
        }

        public void BeginUpdate(string language)
        {
            BeginWrite(language);
        }

        public void BeginWrite(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException("Language not specified");
            }

            _indexDirectoryCurrent = Path.Combine(_indexDirectoryBase, language);
            _writer = CreateIndexWriter();  
        }

        public void EndUpdate(string language)
        {
            EndWrite(language);
        }

        public void EndWrite(string language)
        {
            _writer.Dispose();
            _writer = null;
        }

        public void AddEntry(string id, DateTime? updated, bool? discontinued, string[] tags, PXMeta meta)
        {
            Document doc = GetDocument(id, updated, discontinued, tags, meta);
            _writer.AddDocument(doc);
        }

        public void UpdateEntry(string id, DateTime? updated, bool? discontinued, string[] tags, PXMeta meta)
        {
            Document doc = GetDocument(id, updated, discontinued, tags, meta);
            _writer.UpdateDocument(new Term(SearchConstants.SEARCH_FIELD_DOCID, doc.Get(SearchConstants.SEARCH_FIELD_DOCID)), doc);
        }

        public void RemoveEntry(string id)
        {
            //check if document exists, if true deletes existing
            var searchQuery = new TermQuery(new Term(SearchConstants.SEARCH_FIELD_DOCID, id));
            _writer.DeleteDocuments(searchQuery);   
        }

        /// <summary>
        /// Get Document object representing the table
        /// </summary>
        /// <param name="id">Id of document (table)</param>
        /// <param name="updated">Time the table was last updated</param>
        /// <param name="discontinued">If the table is discontinued</param>
        /// <param name="tags">Table tags</param>
        /// <param name="meta">PXMeta object</param>
        /// <returns>Document object representing the table</returns>
        private Document GetDocument(string id, DateTime? updated, bool? discontinued, string[] tags, PXMeta meta)
        {
            Document doc = new Document();
            DateTime updated2;
            string strUpdated = "";

            if (meta != null)
            {
                //if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(table) || string.IsNullOrEmpty(database) || string.IsNullOrEmpty(meta.Title) || string.IsNullOrEmpty(meta.Matrix) || meta.Variables.Count == 0)
                //{
                //    return doc;
                //}
                if (string.IsNullOrEmpty(meta.Title) || string.IsNullOrEmpty(meta.Matrix) || meta.Variables.Count == 0)
                {
                    return doc;
                }

                if (updated != null)
                {
                    updated2 = updated.Value;
                    strUpdated = updated2.DateTimeToPxDateString();
                }

                doc.Add(new StringField(SearchConstants.SEARCH_FIELD_DOCID, id, Field.Store.YES)); // Used as id when updating a document - NOT searchable!!!
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_SEARCHID, id, Field.Store.NO)); // Used for finding a document by id - will be used for generating URL from just the tableid - Searchable!!!
                //doc.Add(new StoredField(SearchConstants.SEARCH_FIELD_PATH, path));
                //doc.Add(new StoredField(SearchConstants.SEARCH_FIELD_TABLE, table));
                //doc.Add(new StringField(SearchConstants.SEARCH_FIELD_DATABASE, database, Field.Store.YES));
                doc.Add(new StringField(SearchConstants.SEARCH_FIELD_PUBLISHED, strUpdated, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_MATRIX, meta.Matrix, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_TITLE, meta.Title, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_VARIABLES, string.Join(" ", (from v in meta.Variables select v.Name).ToArray()), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_PERIOD, meta.GetTimeValues(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_VALUES, meta.GetAllValues(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_CODES, meta.GetAllCodes(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_GROUPINGS, meta.GetAllGroupings(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_GROUPINGCODES, meta.GetAllGroupingCodes(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_VALUESETS, meta.GetAllValuesets(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_VALUESETCODES, meta.GetAllValuesetCodes(), Field.Store.NO));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_TABLEID, meta.TableID == null ? meta.Matrix : meta.TableID, Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_DISCONTINUED, discontinued == null ? "False" : discontinued.ToString(), Field.Store.YES));
                doc.Add(new TextField(SearchConstants.SEARCH_FIELD_TAGS, GetAllTags(tags), Field.Store.YES));
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
