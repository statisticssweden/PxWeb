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
        public Indexer(IDataSource dataSource)
        {
            _source = dataSource;
        }

        public void IndexDatabase(List<string> languages)
        {
            bool selectionExisits;
            foreach (var language in languages) {
                var item = _source.CreateMenu("", language, out selectionExisits);
                if (selectionExisits) {
                     
                    if (item == null)
                    {
                        //TODO throw Exception?
                        return;
                    }

                    if (item.CurrentItem != null && item.CurrentItem is PxMenuItem)
                    {
                        TraverseDatebase(item.CurrentItem.ID.Selection, language);
                    }
                }
            }
        }

        public void UpdateTableEntries(List<string> tables, List<string> languages)
        {

        }

        private void TraverseDatebase(string id, string language)
        {
            bool exists;
            PxMenuBase m = _source.CreateMenu(id, language, out exists);

            if (m == null)
            {
                return;
            }

            foreach (var item in m.RootItem.SubItems)
            {
                if (item is PxMenuItem)
                {
                    TraverseDatebase(item.ID.Selection, language);
                }
                else if (item is TableLink)
                {
                    IndexTable((TableLink)item, language);
                }
            }
        }

        private void IndexTable(TableLink item, string language)
        {
            throw new NotImplementedException();
        }
    }
}
