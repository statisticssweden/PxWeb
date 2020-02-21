using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCAxis.Query
{
    /// <summary>
    /// Describes possible output format filetypes
    /// </summary>
    public enum SavedQueryOutputFormatType
    {
        /// <summary>
        /// Get result as PX-file
        /// </summary>
        px,
        /// <summary>
        /// Get result as Excel-file
        /// </summary>
        xlsx,
        /// <summary>
        /// Get result as Excel-file with double column
        /// </summary>
        xlsx_doublecolumn,
        /// <summary>
        /// Get result as default csv-file 
        /// </summary>
        csv,
        /// <summary>
        /// Get result as tabseparated csv-file without heading
        /// </summary>
        csv_tab,
        /// <summary>
        /// Get result as tabseparated csv-file with heading
        /// </summary>
        csv_tabhead,
        /// <summary>
        /// Get result as commaseparated csv-file without heading
        /// </summary>
        csv_comma,
        /// <summary>
        /// Get result as commaseparated csv-file with heading
        /// </summary>
        csv_commahead,
        /// <summary>
        /// Get result as spaceseparated csv-file without heading
        /// </summary>
        csv_space,
        /// <summary>
        /// Get result as spaceseparated csv-file with heading
        /// </summary>
        csv_spacehead,
        /// <summary>
        /// Get result as semicolonseparated csv-file without heading
        /// </summary>
        csv_semicolon,
        /// <summary>
        /// Get result as semicolonseparated csv-file with heading
        /// </summary>
        csv_semicolonhead,
        /// <summary>
        /// Get result as json-stat-file
        /// </summary>
        json_stat,
		/// <summary>
		/// Get result as json-stat2-file
		/// </summary>
		json_stat2,
		/// <summary>
		/// Get result as json-file
		/// </summary>
		json,
        /// <summary>
        /// Get result as html5-table
        /// </summary>
        html5_table,
        /// <summary>
        /// Get result as a relation table (txt)
        /// </summary>
        relational_table
    }
}
