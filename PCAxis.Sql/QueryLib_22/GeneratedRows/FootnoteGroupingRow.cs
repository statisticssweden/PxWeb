using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_22
{

    /// <summary>
    /// Holds the attributes for FootnoteGrouping. (This entity is language independent.) 
    /// 
    /// The table links footnotes to a grouping. \nAt statistics \n,\n Not yet implemented. 
    /// </summary>
    public class FootnoteGroupingRow
    {
        private String mGrouping;
        /// <summary>
        /// Namn på den variabel, som fotnoten avser.\nSe beskrivning av tabellen Variabel.
        /// </summary>
        public String Grouping
        {
            get { return mGrouping; }
        }
        private String mFootnoteNo;
        /// <summary>
        /// Fotnotens nummer.\nSe beskrivning av tabellen Fotnot. 
        /// </summary>
        public String FootnoteNo
        {
            get { return mFootnoteNo; }
        }

        public FootnoteGroupingRow(DataRow myRow, SqlDbConfig_22 dbconf)
        {
            this.mGrouping = myRow[dbconf.FootnoteGrouping.GroupingCol.Label()].ToString();
            this.mFootnoteNo = myRow[dbconf.FootnoteGrouping.FootnoteNoCol.Label()].ToString();
        }
    }
}
