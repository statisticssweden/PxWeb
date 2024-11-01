using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXWeb.Code.API.Interfaces
{
    /// <summary>
    /// Represents a bulk registry for managing table bulk files.
    /// </summary>
    public interface IBulkRegistry
    {
        /// <summary>
        /// Sets the context for the bulk registry.
        /// </summary>
        /// <param name="context">The context to set.</param>
        /// <param name="language">The language for the current context.</param>
        void SetContext(string context, string language);

        /// <summary>
        /// Determines whether a table should be updated based on its last updated date.
        /// </summary>
        /// <param name="tableId">The ID of the table.</param>
        /// <param name="lastUpdated">The last updated date of the table.</param>
        /// <returns>True if the table should be updated, otherwise false.</returns>
        bool ShouldTableBeUpdated(string tableId, DateTime lastUpdated);

        /// <summary>
        /// Registers that a table bulk file has been updated.
        /// </summary>
        /// <param name="tableId">The ID of the table.</param>
        /// <param name="tableText">The presentation text of the table.</param>
        /// <param name="generationDate">The generation date of the bulk file.</param>
        void RegisterTableBulkFileUpdated(string tableId, string tableText, DateTime generationDate);

        /// <summary>
        /// Saves the changes made to the bulk registry.
        /// </summary>
        void Save();
    }
}
