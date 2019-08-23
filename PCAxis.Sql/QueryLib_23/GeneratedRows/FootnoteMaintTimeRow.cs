using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. 

namespace PCAxis.Sql.QueryLib_23
{

    /// <summary>
    /// Holds the attributes for FootnoteMaintTime. (This entity is language independent.)
    /// 
    /// Footnote for points in time or timeperiods for a main table.
    /// 
    /// N.B this table can only be used as long as all contents of the main table has the same connections in the ContentsTime table. Footnotes that are linked in this way should have type = 9. This is the same type that is used for notes with FootnoteMaintValue. By using that type the footnote can be specified so that it is valid for the intersection of more than one column thereby assigning the footnote to a subset of the data.
    /// </summary>
    public class FootnoteMaintTimeRow
    {
        private String mMainTable;
        /// <summary>
        /// Name of main table
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mTimePeriod;
        /// <summary>
        /// Timeperiod
        /// </summary>
        public String TimePeriod
        {
            get { return mTimePeriod; }
        }
        private String mFootnoteNo;
        /// <summary>
        /// Footnote number
        /// </summary>
        public String FootnoteNo
        {
            get { return mFootnoteNo; }
        }

        public FootnoteMaintTimeRow(DataRow myRow, SqlDbConfig_23 dbconf)
        {
            this.mMainTable = myRow[dbconf.FootnoteMaintTime.MainTableCol.Label()].ToString();
            this.mTimePeriod = myRow[dbconf.FootnoteMaintTime.TimePeriodCol.Label()].ToString();
            this.mFootnoteNo = myRow[dbconf.FootnoteMaintTime.FootnoteNoCol.Label()].ToString();
        }
    }
}
