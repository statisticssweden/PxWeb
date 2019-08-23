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
    /// Holds the attributes for FootnoteContTime. (This entity is language independent.)
    /// 
    /// The table links footnotes to a point in time for a specific content column.
    /// </summary>
    public class FootnoteContTimeRow
    {
        private String mMainTable;
        /// <summary>
        /// Name of main table.
        /// 
        /// See further in the description of the table MainTable.
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mContents;
        /// <summary>
        /// Name of content column.
        /// 
        /// See further in the description of the table Contents.
        /// </summary>
        public String Contents
        {
            get { return mContents; }
        }
        private String mTimePeriod;
        /// <summary>
        /// Point in time that the footnote relates to.
        /// 
        /// See descriptions in table TimeScale and ContentsTime.
        /// </summary>
        public String TimePeriod
        {
            get { return mTimePeriod; }
        }
        private String mFootnoteNo;
        /// <summary>
        /// Number of the footnote.
        /// 
        /// See further in the description of the table Footnote.
        /// </summary>
        public String FootnoteNo
        {
            get { return mFootnoteNo; }
        }
        private String mCellnote;
        /// <summary>
        /// State whether the footnote is a cell footnote. A cell footnote is defined as a footnote that exists for at least two of the content column's variables, e.g. Region and Time or Region and Sex.
        /// Alternatives:
        /// Y = Yes, the footnote is a cell footnote
        /// N = No
        /// </summary>
        public String Cellnote
        {
            get { return mCellnote; }
        }

        public FootnoteContTimeRow(DataRow myRow, SqlDbConfig_23 dbconf)
        {
            this.mMainTable = myRow[dbconf.FootnoteContTime.MainTableCol.Label()].ToString();
            this.mContents = myRow[dbconf.FootnoteContTime.ContentsCol.Label()].ToString();
            this.mTimePeriod = myRow[dbconf.FootnoteContTime.TimePeriodCol.Label()].ToString();
            this.mFootnoteNo = myRow[dbconf.FootnoteContTime.FootnoteNoCol.Label()].ToString();
            this.mCellnote = myRow[dbconf.FootnoteContTime.CellnoteCol.Label()].ToString();
        }
    }
}
