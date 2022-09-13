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

namespace Px.Search
{
    public class Indexer
    {
        private IDataSource _source;
        private ISearchBackend _backend;
        public Indexer(IDataSource dataSource, ISearchBackend backend)
        {
            _source = dataSource;
            _backend = backend;
        }
        
        /// <summary>
        /// Creates or recreates a search index for the database
        /// </summary>
        /// <param name="languages">list of languages codes that the search index will be able to be searched for</param>
        public void IndexDatabase(List<string> languages)
        {
            bool selectionExisits;
            var index = _backend.GetIndex();
            foreach (var language in languages) {
                index.BeginWrite(language);
                
                //Get the root item from the database
                var item = _source.CreateMenu("", language, out selectionExisits);
                if (selectionExisits) {
                     
                    if (item == null)
                    {
                        //TODO throw Exception?
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

            if (m == null)
            {
                return;
            }

            //TODO make sure that the loop works as intended. That is that it looks for all sub leveles and call itself.
            foreach (var item in m.RootItem.SubItems)
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


        /// <summary>
        /// Updates the entries in the search index for the list of specified tables.
        /// </summary>
        /// <param name="tables">List of tables that the search index</param>
        /// <param name="languages">list of languages codes that the search index will be able to be searched for</param>
        public void UpdateTableEntries(List<string> tables, List<string> languages)
        {
            var index = _backend.GetIndex();

            //for each combination of table and language and within a index.BeginUpdate(language) and index.BeginUpdate(language)
            //call IndexTable
        }

        private void IndexTable(string id, string language, IIndex index)
        {
            //Create a Builder from backand
            //Call BuildForSelection
            //Extract metadata 
            //call AddEntry on the index
            throw new NotImplementedException();
        }
    }
}
