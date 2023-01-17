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

                        if (item.CurrentItem != null && item.CurrentItem is PxMenuItem)
                        {
                            TraverseDatabase(item.CurrentItem.ID.Selection, language, index);
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

            PxMenuBase m = _source.CreateMenu(id, language, out exists);

            if (m == null || !exists || m.CurrentItem == null)
            {
                _logger.LogError($"TraverseDatabase : Could not get database level with id {id} for language {language}");
                return;
            }

            if (m.CurrentItem is PxMenuItem)
            {
                foreach (var item in ((PxMenuItem)(m.CurrentItem)).SubItems)
                {
                    if (item is PxMenuItem)
                    {
                        TraverseDatabase(item.ID.Selection, language, index);
                    }
                    else if (item is TableLink)
                    {
                        IndexTable(item.ID.Selection, language, index);
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
            using (var index = _backend.GetIndex())
            {
                //for each combination of table and language and within a index.BeginUpdate(language) and index.BeginUpdate(language)
                //call UpdateTable(id, lang, index)
                foreach (var language in languages)
                {
                    index.BeginUpdate(language);

                    foreach (var table in tables)
                    {
                        UpdateTable(table, language, index);
                    }

                    index.EndUpdate(language);
                }
            }
        }

        private void IndexTable(string id, string language, IIndex index)
        {
            IPXModelBuilder builder = _source.CreateBuilder(id, language);

            if (builder != null)
            {
                try
                {
                    builder.BuildForSelection();
                    var model = builder.Model;

                    DateTime updated = model.Meta.GetLastUpdated().PxDateStringToDateTime();
                    string[] tags = new string[] { };

                    index.AddEntry(id, updated, null, tags, model.Meta);
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
        private void UpdateTable(string id, string language, IIndex index)
        {
            IPXModelBuilder builder = _source.CreateBuilder(id, language);

            if (builder != null)
            {
                try
                {
                    builder.BuildForSelection();
                    var model = builder.Model;

                    DateTime updated = model.Meta.GetLastUpdated().PxDateStringToDateTime();
                    string[] tags = new string[] { };

                    index.UpdateEntry(id, updated, null, tags, model.Meta);
                    return;
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

            index.RemoveEntry(id);
            return;

        }

    }
}
