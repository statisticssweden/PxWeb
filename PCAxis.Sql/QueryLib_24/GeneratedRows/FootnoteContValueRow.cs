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
    /// Holds the attributes for FootnoteContValue. (This entity is language independent.)
    /// 
    /// The table links footnotes to a value for a specific content column.
    /// </summary>
    public class FootnoteContValueRow
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
        private String mVariable;
        /// <summary>
        /// Name of variable.
        /// 
        /// See further in the description of the table Variable.
        /// </summary>
        public String Variable
        {
            get { return mVariable; }
        }
        private String mValuePool;
        /// <summary>
        /// Name of value pool.
        /// 
        /// See further in the description of the table ValuePool.
        /// </summary>
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mValueCode;
        /// <summary>
        /// Code for the value that the footnote relates to.
        /// 
        /// See further in the description of the table Value.
        /// </summary>
        public String ValueCode
        {
            get { return mValueCode; }
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

        public FootnoteContValueRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mMainTable = myRow[dbconf.FootnoteContValue.MainTableCol.Label()].ToString();
            this.mContents = myRow[dbconf.FootnoteContValue.ContentsCol.Label()].ToString();
            this.mVariable = myRow[dbconf.FootnoteContValue.VariableCol.Label()].ToString();
            this.mValuePool = myRow[dbconf.FootnoteContValue.ValuePoolCol.Label()].ToString();
            this.mValueCode = myRow[dbconf.FootnoteContValue.ValueCodeCol.Label()].ToString();
            this.mFootnoteNo = myRow[dbconf.FootnoteContValue.FootnoteNoCol.Label()].ToString();
            this.mCellnote = myRow[dbconf.FootnoteContValue.CellnoteCol.Label()].ToString();
        }
    }
}
