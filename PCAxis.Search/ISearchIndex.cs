using System;
using System.Collections.Generic;

namespace PCAxis.Search
{
    /// <summary>
    /// Interface for search index functionality
    /// </summary>
    public interface ISearchIndex
    {
        /// <summary>
        /// Get list of tables that shall be updated in the search index
        /// </summary>
        /// <param name="dateFrom">Date to check from (table metadata must have been changed after this date)</param>
        /// <param name="database">Database id</param>
        /// <param name="language">Language</param>
        /// <returns>List with TableUpdate objects representing the tables to update in the search index</returns>
        List<TableUpdate> GetUpdatedTables(DateTime dateFrom, string database, string language);
    }
}
