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
    /// Holds the attributes for Value. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table describes the values in the value pool.
    /// </summary>
    public class ValueRow
    {
        private String mValuePool;
        /// <summary>
        /// Name of the value pool that the value belongs to. See further description of table ValuePool.
        /// </summary>
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mValueCode;
        /// <summary>
        /// Code for value or group.
        /// 
        /// Value code should agree with the code in the corresponding classification or standard if there is one.
        /// 
        /// Because the value codes are stored in a metadata column for variables in the data table(s) and, because the width of the metadata column is decided by the number of characters in the longest value code, the code should not be longer than necessary to ensure that it does not take up more space in the data table than necessary. The value codes within a value set should also be roughly the same size.
        /// 
        /// Capitals and/or lower case letters can be used, the letters å, ä and ö are accepted. Special characters and dashes should be avoided because they can cause technical problems.
        /// </summary>
        public String ValueCode
        {
            get { return mValueCode; }
        }
        private String mMetaId;
        /// <summary>
        /// MetaId can be used to link the information in this table to an external system.
        /// </summary>
        public String MetaId
        {
            get { return mMetaId; }
        }
        private String mFootnote;
        /// <summary>
        /// Shows whether there is a footnote linked to the value (FootnoteType 6). There are the following alternatives:
        /// 
        /// B = Both obligatory and optional footnotes exist
        /// V = One or several optional footnotes exist.
        /// O = One or several obligatory footnotes exist
        /// N = There are no footnotes
        /// </summary>
        public String Footnote
        {
            get { return mFootnote; }
        }

        public Dictionary<string, ValueTexts> texts = new Dictionary<string, ValueTexts>();

        public ValueRow(DataRow myRow, SqlDbConfig_24 dbconf, StringCollection languageCodes)
        {
            this.mValuePool = myRow[dbconf.Value.ValuePoolCol.Label()].ToString();
            this.mValueCode = myRow[dbconf.Value.ValueCodeCol.Label()].ToString();
            this.mMetaId = myRow[dbconf.Value.MetaIdCol.Label()].ToString();
            this.mFootnote = myRow[dbconf.Value.FootnoteCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new ValueTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for Value  for one language.
    /// The table describes the values in the value pool.
    /// </summary>
    public class ValueTexts
    {
        private String mSortCode;
        /// <summary>
        /// Sorting code for values and groups, which decides in which order the value and group codes are to be presented when values and table presentation are selected when retrieved from the databases.
        /// 
        /// The sorting code should be the same as the ValueCode or be designed in such a way that the values can be presented in the desired order. The beginning of ValueTextL can be used so that the values will be presented in alphabetical order by the value text.
        /// 
        /// NB. Please note that the sorting code is also available in the tables VSValue, VSGroup and Grouping. See further descriptions for these.
        /// </summary>
        public String SortCode
        {
            get { return mSortCode; }
        }
        private String mUnit;
        /// <summary>
        /// Can be used to state the unit so that a value can have different units.
        /// 
        /// If the field is filled in with a unit, the column Unit in the table Contents should be filled with %Value. If the field is not filled in, it should be NULL. Then the column Unit in the table Contents is used instead to state the unit.
        /// 
        /// See also description of the table Contents.
        /// </summary>
        public String Unit
        {
            get { return mUnit; }
        }
        private String mValueTextS;
        /// <summary>
        /// Short presentation text for value and group.
        /// 
        /// To be visible in the retrieval interfaces, it requires that:
        /// - The field ValueTextExists in ValuePool is either S ('Short value text exists') or B ('Both short and long value text exists') and
        /// - The field ValuePres in ValuePool or ValueSet is either A ('Both code and short value text should be presented') or S ('Short value text should be presented').
        /// 
        /// The text is written in lower case letters, except for abbreviations etc.
        /// 
        /// See also descriptions of ValueTextExists in ValuePool and ValuePres in ValuePool and ValueSet.
        /// </summary>
        public String ValueTextS
        {
            get { return mValueTextS; }
        }
        private String mValueTextL;
        /// <summary>
        /// Value text, presentation text for value and group.
        /// 
        /// To be visible in the retrieval interface, the field ValueTextExists in the table ValuePool for the value's value pool must be L.
        /// 
        /// ValueText can be omitted if the values are to be presented only as codes. The field should then be NULL. There should be consistency with a value pool so that all the value pool's values are presented either with or without value texts.
        /// 
        /// The text is written in lower case, with the exception of abbreviations, etc.
        /// 
        /// See also descriptions of ValueTextExists in ValuePool and  ValuePres in ValuePool and ValueSet.
        /// </summary>
        public String ValueTextL
        {
            get { return mValueTextL; }
        }


        internal ValueTexts(DataRow myRow, SqlDbConfig_24 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mSortCode = myRow[dbconf.ValueLang2.SortCodeCol.Label(languageCode)].ToString();
                this.mUnit = myRow[dbconf.ValueLang2.UnitCol.Label(languageCode)].ToString();
                this.mValueTextS = myRow[dbconf.ValueLang2.ValueTextSCol.Label(languageCode)].ToString();
                this.mValueTextL = myRow[dbconf.ValueLang2.ValueTextLCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mSortCode = myRow[dbconf.Value.SortCodeCol.Label()].ToString();
                this.mUnit = myRow[dbconf.Value.UnitCol.Label()].ToString();
                this.mValueTextS = myRow[dbconf.Value.ValueTextSCol.Label()].ToString();
                this.mValueTextL = myRow[dbconf.Value.ValueTextLCol.Label()].ToString();
            }
        }
    }

}
