using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Search
{
    /// <summary>
    /// Holds information about a table update in a search index
    /// </summary>
    public class TableUpdate
    {
        /// <summary>
        /// Table path within its database
        /// Table Path is supposed to have the following format: path/path/path/table
        /// Example: BE/BE0101/BE0101A/BefolkningNy
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Table id
        /// </summary>
        public string Id { get; set; }
    }
}
