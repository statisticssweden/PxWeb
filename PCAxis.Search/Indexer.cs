using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using PCAxis.Menu;
using PCAxis.Paxiom;
using Lucene.Net.Documents;
using PCAxis.Web.Core.Enums;
using PCAxis.Paxiom.Extensions;

namespace PCAxis.Search
{
    /// <summary>
    /// Class for creating and updating a search index.
    /// Encapsulates a Lucene.Net IndexWriter.
    /// </summary>
    public class Indexer
    {
        #region "Private fields"

        private string _indexDirectory;
        private GetMenuDelegate _menuMethod;
        private string _database;
        private string _language;
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(Indexer));

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="indexDirectory">Index directory</param>
        /// <param name="menuMethod">Delegate method to get the Menu</param>
        /// <param name="database">Database id</param>
        /// <param name="langyuage">Language</param>
        public Indexer(string indexDirectory, GetMenuDelegate menuMethod, string database, string language)
        {
            _indexDirectory = indexDirectory;
            _menuMethod = menuMethod;
            _database = database;
            _language = language;
        }

        #region "Public methods"

        /// <summary>
        /// Create index
        /// </summary>
        public bool CreateIndex()
        {
            try
            {


                using (IndexWriter writer = CreateIndexWriter(true))
                {
                    if (writer == null)
                    {
                        return false;
                    }

                    // Set field length to max to be able to handle very large valuesets
                    writer.SetMaxFieldLength(int.MaxValue);

                    ItemSelection node = null;

                    //if (_database == "ssd")
                    //{
                    //    //nodeId = "START__KU";
                    //    node = new ItemSelection();
                    //    //node.Menu = "START";
                    //    //node.Selection = "KU";
                    //    node.Menu = "HA0201";
                    //    node.Selection = "HA0201B";
                    //}

                    // Get database
                    PCAxis.Menu.Item itm;
                    PCAxis.Menu.PxMenuBase db = _menuMethod(_database, node, _language, out itm);
                    if (db == null)
                    {
                        _logger.Error("Failed to access database '" + _database + "'. Creation of search index aborted.");
                        writer.Rollback(); 
                        _logger.Error("Rollback of '" + _database + "' done");
                        return false;
                    }

                    PCAxis.Web.Core.Enums.DatabaseType dbType;
                    if (db is PCAxis.Menu.Implementations.XmlMenu)
                    {
                        dbType = DatabaseType.PX;
                    }
                    else
                    {
                        dbType = DatabaseType.CNMM;
                    }

                    if (db.RootItem != null)
                    {
                        foreach (var item in db.RootItem.SubItems)
                        {
                            if (item is PCAxis.Menu.PxMenuItem)
                            {
                                TraverseDatabase(dbType, item as PxMenuItem, writer, "/" + item.ID.Selection);
                            }
                            else if (item is PCAxis.Menu.TableLink)
                            {
                                IndexTable(dbType, (TableLink)item, "/" + item.ID.Menu, writer);
                            }
                        }
                    }

                    writer.Optimize();
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
        }

        /// <summary>
        /// Update index for the tables specified in the list.
        /// Partial update of search index is only possible for CNMM databases.
        /// </summary>
        /// <param name="tableList">List with TableUpdate objects that represents the tables that should be updated in the index</param>
        public bool UpdateIndex(List<TableUpdate> tableList)
        {
            ItemSelection node = null;
            PCAxis.Menu.Item currentTable;
            string[] pathParts;
            string title;
            string menu, selection;
            DateTime published = DateTime.MinValue;
            bool doUpdate;

            using (IndexWriter writer = CreateIndexWriter(false))
            {
                if (writer == null)
                {
                    return false;
                }

                foreach (TableUpdate table in tableList)
                {
                    doUpdate = false;
                    PXModel model = PxModelManager.Current.GetModel(DatabaseType.CNMM, _database, _language, table.Id);

                    // Get default value for title
                    title = model.Meta.Title;

                    // Get table title from _menuMethod
                    // table.Path is supposed to have the following format: path/path/path
                    // Example: BE/BE0101/BE0101A
                    pathParts = table.Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                    if (pathParts.Length > 1)
                    {
                        menu = pathParts[pathParts.Length - 1];
                        selection = table.Id;

                        node = new ItemSelection(menu, selection);
                        PCAxis.Menu.PxMenuBase db = _menuMethod(_database, node, _language, out currentTable);

                        if (currentTable != null)
                        {
                            if (currentTable is TableLink)
                            {
                                doUpdate = true;
                                // Get table title from the menu method
                                if (!string.IsNullOrEmpty(currentTable.Text))
                                {
                                    title = currentTable.Text;
                                }
                                if (((TableLink)currentTable).Published != null)
                                {
                                    published = (DateTime)((TableLink)currentTable).Published;
                                }
                            }
                        }
                    }

                    if (doUpdate)
                    {
                        UpdatePaxiomDocument(writer, _database, table.Id, table.Path, table.Id, title, published, model.Meta);
                        _logger.Info("Search index " + _database + " - " + _language + " updated table " + table.Id);
                    }
                }

                writer.Optimize();
            }

            return true;
        }

        #endregion


        #region "Private methods"

        /// <summary>
        /// Recursively traverse the database to add all tables as Document objects into the index
        /// </summary>
        /// <param name="itm">Current node in database to add Document objects for</param>
        /// <param name="writer">IndexWriter object</param>
        /// <param name="path">Path within the database for this node</param>
        private void TraverseDatabase(PCAxis.Web.Core.Enums.DatabaseType dbType, PxMenuItem itm, IndexWriter writer, string path)
        {
            PCAxis.Menu.Item newItem;
            PCAxis.Menu.PxMenuBase db = _menuMethod(_database, itm.ID, _language, out newItem);
            PxMenuItem m = (PxMenuItem)newItem;

            if (m == null)
            {
                return;
            }

            foreach (var item in m.SubItems)
            {
                if (item is PxMenuItem)
                {
                    TraverseDatabase(dbType, item as PxMenuItem, writer, path + "/" + item.ID.Selection);
                }
                else if (item is TableLink)
                {
                    IndexTable(dbType, (TableLink)item, path, writer);
                }
            }
        }


        /// <summary>
        /// Add table to search index
        /// </summary>
        /// <param name="dbType">Type of database</param>
        /// <param name="item">TableLink object representing the table</param>
        /// <param name="path">Path to table within database</param>
        /// <param name="writer">IndexWriter object</param>
        private void IndexTable(PCAxis.Web.Core.Enums.DatabaseType dbType, TableLink item, string path, IndexWriter writer)
        {
            item.ID.Selection = CleanTableId(item.ID);

            PXModel model = PxModelManager.Current.GetModel(dbType, _database, _language, item.ID);

            if (model != null)
            {
                string id;
                string tablePath;
                string table = "";
                string title = "";
                DateTime published = DateTime.MinValue;

                if (dbType == DatabaseType.PX)
                {
                    char[] sep = { '\\' };
                    string[] parts = item.ID.Selection.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    StringBuilder pxPath = new StringBuilder();

                    // PX database
                    id = item.ID.Selection;

                    for (int i = 0; i < parts.Length - 1; i++)
                    {
                        if (i > 0)
                        {
                            pxPath.Append("/");
                        }
                        pxPath.Append(parts[i]);
                    }
                    tablePath = pxPath.ToString();
                    table = parts.Last();
                    title = item.Text;
                    if (((TableLink)item).Published != null)
                    {
                        published = (DateTime)((TableLink)item).Published;
                    }
                }
                else
                {
                    // CNMM database
                    id = item.ID.Selection;
                    tablePath = path;
                    table = item.ID.Selection;
                    title = item.Text;
                    if (((TableLink)item).Published != null)
                    {
                        published = (DateTime)((TableLink)item).Published;
                    }
                }
                AddPaxiomDocument(writer, _database, id, tablePath, table, title, published, model.Meta);
            }
        }

        /// <summary>
        /// Get table id without database name
        /// Example: If node.Selection = databaseid:tableid then tableid will be returned
        /// </summary>
        /// <param name="node">node representing the table</param>
        /// <returns>Table id as a string</returns>
        private string CleanTableId(ItemSelection node)
        {
            int index = node.Selection.IndexOf(":");

            if ((index > -1) && (node.Selection.Length > index))
            {
                return node.Selection.Substring(index + 1);
            }
            else
            {
                return node.Selection;
            }
        }


        /// <summary>
        /// Get Lucene.Net IndexWriter object 
        /// </summary>
        /// <param name="createIndex">If index shall be created (true) or updated (false)</param>
        /// <returns>IndexWriter object. If the Index directory is locked, null is returned</returns>
        private IndexWriter CreateIndexWriter(bool createIndex)
        {
            FSDirectory fsDir = FSDirectory.Open(_indexDirectory);
            
            if (IndexWriter.IsLocked(fsDir))
            {
                _logger.Error("Index directory " + _indexDirectory + " is locked - cannot write index");
                return null;
            }

            IndexWriter writer = new IndexWriter(fsDir, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), createIndex, IndexWriter.MaxFieldLength.LIMITED);
            return writer;
        }

        /// <summary>
        /// Add Paxiom Document (table) to index
        /// </summary>
        /// <param name="writer">IndexWriter object</param>
        /// <param name="database">Database id</param>
        /// <param name="id">Id of document (table)</param>
        /// <param name="path">Path to table within database</param>
        /// <param name="meta">PXMeta object</param>
        /// <returns>Document object representing the added table</returns>
        private Document AddPaxiomDocument(IndexWriter writer, string database, string id, string path, string table, string title, DateTime published, PXMeta meta)
        {
            Document doc = GetDocument(database, id, path, table, title, published, meta);

            writer.AddDocument(doc);

            return doc;
        }

        /// <summary>
        /// Update Paxiom Document (table) in index
        /// </summary>
        /// <param name="writer">IndexWriter object</param>
        /// <param name="database">Database id</param>
        /// <param name="id">Id of document (table)</param>
        /// <param name="path">Path to table within database</param>
        /// <param name="meta">PXMeta object</param>
        /// <returns>Document object representing the updated table</returns>
        private Document UpdatePaxiomDocument(IndexWriter writer, string database, string id, string path, string table, string title, DateTime published, PXMeta meta)
        {
            Document doc = GetDocument(database, id, path, table, title, published, meta);

            writer.UpdateDocument(new Term(SearchConstants.SEARCH_FIELD_DOCID, doc.Get(SearchConstants.SEARCH_FIELD_DOCID)), doc);

            return doc;
        }


        /// <summary>
        /// Get Document object representing the table
        /// </summary>
        /// <param name="database">Database id</param>
        /// <param name="id">Id of document (table)</param>
        /// <param name="path">Path to table within database</param>
        /// <param name="path">Table</param>
        /// <param name="meta">PXMeta object</param>
        /// <returns>Document object representing the table</returns>
        private Document GetDocument(string database, string id, string path, string table, string title, DateTime published, PXMeta meta)
        {
            Document doc = new Document();

            if (meta != null)
            {
                if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(table) || string.IsNullOrEmpty(database) || string.IsNullOrEmpty(meta.Title) || string.IsNullOrEmpty(meta.Matrix) || meta.Variables.Count == 0)
                {
                    return doc;
                }
               
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_DOCID, id, Field.Store.YES, Field.Index.NOT_ANALYZED)); // Used as id when updating a document - NOT searchable!!!
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_SEARCHID, id, Field.Store.NO, Field.Index.ANALYZED)); // Used for finding a document by id - will be used for generating URL from just the tableid - Searchable!!!
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_PATH, path, Field.Store.YES, Field.Index.NO));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_TABLE, table, Field.Store.YES, Field.Index.NO));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_DATABASE, database, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_PUBLISHED, published.DateTimeToPxDateString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_MATRIX, meta.Matrix, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_TITLE, title, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_VARIABLES, string.Join(" ", (from v in meta.Variables select v.Name).ToArray()), Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_PERIOD, meta.GetTimeValues(), Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_VALUES, meta.GetAllValues(), Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_CODES, meta.GetAllCodes(), Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_GROUPINGS, meta.GetAllGroupings(), Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_GROUPINGCODES, meta.GetAllGroupingCodes(), Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_VALUESETS, meta.GetAllValuesets(), Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_VALUESETCODES, meta.GetAllValuesetCodes(), Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field(SearchConstants.SEARCH_FIELD_TABLEID, meta.TableID == null?meta.Matrix:meta.TableID, Field.Store.YES, Field.Index.ANALYZED));
                if (!string.IsNullOrEmpty(meta.Synonyms))
                {
                    doc.Add(new Field(SearchConstants.SEARCH_FIELD_SYNONYMS, meta.Synonyms, Field.Store.NO, Field.Index.ANALYZED));
                }
                              
            }

            return doc;
        }

        #endregion
    }
}
