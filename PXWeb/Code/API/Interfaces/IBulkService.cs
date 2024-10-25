using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXWeb.Code.API.Interfaces
{
    /// <summary>
    /// Represents a service for creating bulk files for a database.
    /// </summary>
    public interface IBulkService
    {
        /// <summary>
        /// Creates bulk files for the specified database and language.
        /// </summary>
        /// <param name="database">The name of the database.</param>
        /// <param name="language">The language of the bulk files.</param>
        /// <returns><c>true</c> if the bulk files are created successfully; otherwise, <c>false</c>.</returns>
        bool CreateBulkFilesForDatabase(string database);
    }
}
