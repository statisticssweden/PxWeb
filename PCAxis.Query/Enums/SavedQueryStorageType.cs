using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query
{
    /// <summary>
    /// Describes where saved queries are stored
    /// </summary>
    public enum SavedQueryStorageType
    {
        /// <summary>
        /// Saved queries are stored as files
        /// </summary>
        File,
        /// <summary>
        /// Saved queries are stored in a database
        /// </summary>
        Database
    }
}
