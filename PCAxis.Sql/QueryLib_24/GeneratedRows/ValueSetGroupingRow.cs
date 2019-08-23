using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_24
{

    /// <summary>
    /// Holds the attributes for ValueSetGrouping. (This entity is language independent.)
    /// 
    /// The table connects value set to grouping
    /// </summary>
    public class ValueSetGroupingRow
    {
        private String mValueSet;
        /// <summary>
        /// Name of the stored value set.
        /// 
        /// See description of table ValueSet.
        /// </summary>
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mGrouping;
        /// <summary>
        /// Name of grouping.
        /// 
        /// See further in the description of the table Grouping.
        /// </summary>
        public String Grouping
        {
            get { return mGrouping; }
        }

        public ValueSetGroupingRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mValueSet = myRow[dbconf.ValueSetGrouping.ValueSetCol.Label()].ToString();
            this.mGrouping = myRow[dbconf.ValueSetGrouping.GroupingCol.Label()].ToString();
        }
    }
}
