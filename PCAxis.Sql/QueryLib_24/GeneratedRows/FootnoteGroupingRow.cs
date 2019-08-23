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
    /// Holds the attributes for FootnoteGrouping. (This entity is language independent.)
    /// 
    /// The table links footnotes to a grouping.
    /// </summary>
    public class FootnoteGroupingRow
    {
        private String mGrouping;
        /// <summary>
        /// Name of groupong
        /// See further in the description of the table Gruoping.
        /// </summary>
        public String Grouping
        {
            get { return mGrouping; }
        }
        private String mFootnoteNo;
        /// <summary>
        /// Number of the footnote.
        /// See further in the description of the table Footnote.
        /// </summary>
        public String FootnoteNo
        {
            get { return mFootnoteNo; }
        }

        public FootnoteGroupingRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mGrouping = myRow[dbconf.FootnoteGrouping.GroupingCol.Label()].ToString();
            this.mFootnoteNo = myRow[dbconf.FootnoteGrouping.FootnoteNoCol.Label()].ToString();
        }
    }
}
