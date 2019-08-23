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
    /// Holds the attributes for Attribute. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains information on the attribute on the observation values.
    /// 
    /// See further information in the separate document: Attributes in the Nordic SQL Data Model.
    /// </summary>
    public class AttributeRow
    {
        private String mMainTable;
        /// <summary>
        /// Name of the main table to which the attribute is linked.
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mAttribute;
        /// <summary>
        /// Name of the attribute
        /// </summary>
        public String Attribute
        {
            get { return mAttribute; }
        }
        private String mAttributeColumn;
        /// <summary>
        /// Name of the column. The length is set to accommodate future use of prefixes
        /// </summary>
        public String AttributeColumn
        {
            get { return mAttributeColumn; }
        }
        private String mSequenceNo;
        /// <summary>
        /// The attributes place in the data table column or the place within the column. The first attribute for a given main table has the value 1.
        /// </summary>
        public String SequenceNo
        {
            get { return mSequenceNo; }
        }
        private String mValueSet;
        /// <summary>
        /// Name of the value set to which the values are linked.
        /// See further description in table ValueSet.
        /// 
        /// Can be null – for example if the attribute contains a comment.
        /// </summary>
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mColumnLength;
        /// <summary>
        /// Number of stored characters in the data column.
        /// </summary>
        public String ColumnLength
        {
            get { return mColumnLength; }
        }

        public Dictionary<string, AttributeTexts> texts = new Dictionary<string, AttributeTexts>();

        public AttributeRow(DataRow myRow, SqlDbConfig_24 dbconf, StringCollection languageCodes)
        {
            this.mMainTable = myRow[dbconf.Attribute.MainTableCol.Label()].ToString();
            this.mAttribute = myRow[dbconf.Attribute.AttributeCol.Label()].ToString();
            this.mAttributeColumn = myRow[dbconf.Attribute.AttributeColumnCol.Label()].ToString();
            this.mSequenceNo = myRow[dbconf.Attribute.SequenceNoCol.Label()].ToString();
            this.mValueSet = myRow[dbconf.Attribute.ValueSetCol.Label()].ToString();
            this.mColumnLength = myRow[dbconf.Attribute.ColumnLengthCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new AttributeTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for Attribute  for one language.
    /// The table contains information on the attribute on the observation values.
    /// 
    /// See further information in the separate document: Attributes in the Nordic SQL Data Model.
    /// </summary>
    public class AttributeTexts
    {
        private String mPresText;
        /// <summary>
        /// Presentation text used by the retrieval interface.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }
        private String mDescription;
        /// <summary>
        /// Description of the attribute.
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }


        internal AttributeTexts(DataRow myRow, SqlDbConfig_24 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.AttributeLang2.PresTextCol.Label(languageCode)].ToString();
                this.mDescription = myRow[dbconf.AttributeLang2.DescriptionCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.Attribute.PresTextCol.Label()].ToString();
                this.mDescription = myRow[dbconf.Attribute.DescriptionCol.Label()].ToString();
            }
        }
    }

}
