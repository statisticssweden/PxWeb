using Px.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCAxis.Menu;
using System.Net.Http;
using System.Data;
using System.IO;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Extensions;
using Microsoft.Extensions.Logging;
using PCAxis.Paxiom.Localization;


namespace Px.Search
{
    public class Indexer
    {
        private IDataSource _source;
        private ISearchBackend _backend;
        private ILogger _logger;

        public Indexer(IDataSource dataSource, ISearchBackend backend, ILogger logger)
        {
            _source = dataSource;
            _backend = backend;
            _logger = logger;   
        }
        
        /// <summary>
        /// Creates or recreates a search index for the database
        /// </summary>
        /// <param name="languages">list of languages codes that the search index will be able to be searched for</param>
        public void IndexDatabase(List<string> languages)
        {
            bool selectionExisits;
            using (var index = _backend.GetIndex())
            {
                foreach (var language in languages)
                {
                    index.BeginWrite(language);

                    //Get the root item from the database
                    var item = _source.CreateMenu("", language, out selectionExisits);
                    if (selectionExisits)
                    {

                        if (item == null)
                        {
                            _logger.LogError("IndexDatabase : Could not get root level for database");
                            return;
                        }

                        if (item != null && item is PxMenuItem)
                        {
                            TraverseDatabase(item.ID.Selection, language, index);
                        }
                    }
                    index.EndWrite(language);
                }
            }
        }


        /// <summary>
        /// Traverses the database and looks for tables to add in the index.
        /// </summary>
        /// <param name="id">current node id</param>
        /// <param name="language">current processing language</param>
        /// <param name="index">the index to use</param>
        private void TraverseDatabase(string id, string language, IIndex index)
        {
            bool exists;
            Item? item;

            try
            {
                item = _source.CreateMenu(id, language, out exists);
            }
            catch (Exception ex)
            {
                _logger.LogError($"TraverseDatabase : Could not CreateMenu for id {id} for language {language}", ex);
                return; 
            }

            if (item == null || !exists)
            {
                _logger.LogError($"TraverseDatabase : Could not get database level with id {id} for language {language}");
                return;
            }

            if (item is PxMenuItem)
            {
                foreach (var subitem in ((PxMenuItem)item).SubItems)
                {
                    if (subitem is PxMenuItem)
                    {
                        TraverseDatabase(subitem.ID.Selection, language, index);
                    }
                    else if (subitem is TableLink)
                    {
                        IndexTable(((TableLink)subitem).TableId, (TableLink)subitem, language, index);
                    }
                }
            }

        }


        /// <summary>
        /// Updates the entries in the search index for the list of specified tables.
        /// </summary>
        /// <param name="tables">List of tables that the search index</param>
        /// <param name="languages">list of languages codes that the search index will be able to be searched for</param>
        public void UpdateTableEntries(List<string> tables, List<string> languages)
        {
            bool exists;

            using (var index = _backend.GetIndex())
            {
                foreach (var language in languages)
                {
                    index.BeginUpdate(language);

                    foreach (var table in tables)
                    {
                        Item? item = _source.CreateMenu(table, language, out exists);

                        if (exists && item != null && item is TableLink)
                        {
                            UpdateTable(table, (TableLink)item, language, index);
                        }
                        else
                        {
                            index.RemoveEntry(table);
                        }
                    }

                    index.EndUpdate(language);
                }
            }
        }

        private void IndexTable(string id, TableLink tblLink, string language, IIndex index)
        {
            IPXModelBuilder? builder = _source.CreateBuilder(id, language);

            if (builder != null)
            {
                try
                {
                    builder.BuildForSelection();
                    var model = builder.Model;
                    TableInformation tbl = GetTableInformation(id, tblLink, model.Meta);

                    index.AddEntry(tbl, model.Meta);
                }
                catch (Exception)
                {
                    _logger.LogError($"IndexTable : Could not build table with id {id} for language {language}");
                }
            }
            else
            {
                _logger.LogError($"IndexTable : Could not build table with id {id} for language {language}");
            }

        }
        private void UpdateTable(string id, TableLink tblLink, string language, IIndex index)
        {
            IPXModelBuilder? builder = _source.CreateBuilder(id, language);

            if (builder != null)
            {
                try
                {
                    builder.BuildForSelection();
                    var model = builder.Model;
                    TableInformation tbl = GetTableInformation(id, tblLink, model.Meta);

                    index.UpdateEntry(tbl, model.Meta);
                }
                catch (Exception)
                {
                    _logger.LogError($"UpdateTable : Could not build table with id {id} for language {language}");
                }
            }
            else
            {
                _logger.LogError($"UpdateTable : Could not build table with id {id} for language {language}");
            }
        }

        private TableInformation GetTableInformation(string id, TableLink tblLink, PXMeta meta)
        {
            TableInformation tbl = new TableInformation(id, tblLink.Text, GetCategory(tblLink), meta.GetFirstTimeValue(), meta.GetLastTimeValue(), (from v in meta.Variables select v.Name).ToArray());
            tbl.Description = tblLink.Description;
            tbl.SortCode = tblLink.SortCode;
            tbl.Updated = tblLink.LastUpdated;
            tbl.Discontinued = null; // TODO: Implement later

            return tbl; 
        }

        private string GetCategory(TableLink tblLink)
        {
            switch (tblLink.Category)
            {
                case PresCategory.NotSet:
                    return "";
                case PresCategory.Official:
                    return "public";
                case PresCategory.Internal:
                    return "internal";
                case PresCategory.Private:
                    return "private";
                default:
                    return "";
            }
        }
    }
}
