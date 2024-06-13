using PCAxis.Menu;
using PCAxis.Paxiom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXWeb.Code.API.Interfaces
{
    /// <summary>
    /// Represents a service for retrieving tables.
    /// </summary>
    public interface ITableService
    {
        /// <summary>
        /// Gets all tables from the specified database and language.
        /// </summary>
        /// <param name="database">The name of the database.</param>
        /// <param name="language">The language of the tables.</param>
        /// <returns>A list of table links.</returns>
        List<TableLink> GetAllTables(string database, string language);

        PXModel GetTableModel(string database, string selection, string language);
    }
}
