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
    /// Holds the attributes for FootnoteValueSetValue. (This entity is language independent.)
    /// 
    /// The table links footnotes to a valueset.
    /// </summary>
    public class FootnoteValueSetValueRow
    {
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
        private String mValueSet;
        /// <summary>
        /// Name of value set.
        /// 
        /// See further in the description of the table ValueSet.
        /// </summary>
        public String ValueSet
        {
            get { return mValueSet; }
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

        public FootnoteValueSetValueRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mValuePool = myRow[dbconf.FootnoteValueSetValue.ValuePoolCol.Label()].ToString();
            this.mValueSet = myRow[dbconf.FootnoteValueSetValue.ValueSetCol.Label()].ToString();
            this.mValueCode = myRow[dbconf.FootnoteValueSetValue.ValueCodeCol.Label()].ToString();
            this.mFootnoteNo = myRow[dbconf.FootnoteValueSetValue.FootnoteNoCol.Label()].ToString();
        }
    }
}
