using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Search
{
    /// <summary>
    /// Default implementation of SearchIndex class
    /// </summary>
    public class DefaultSearchIndex : ISearchIndex
    {
        public List<TableUpdate> GetUpdatedTables(DateTime dateFrom, string database, string language)
        {
            List<TableUpdate> lst = new List<TableUpdate>();

            // Return empty list
            return lst;
        }
    }
}
