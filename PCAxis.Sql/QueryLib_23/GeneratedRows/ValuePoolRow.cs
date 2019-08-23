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
    /// Holds the attributes for ValuePool. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table describes which value pools exist in the database. The value pool brings together all values and aggregates for a classification or a variation of a classification.
    /// </summary>
    public class ValuePoolRow
    {
        private String mValuePool;
        /// <summary>
        /// Name of value pool.
        /// 
        /// If there is only one variable belonging to a particular value pool, the variable and value pool should have the same name.
        /// 
        /// The name should begin with a capital letter.
        /// </summary>
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mDescription;
        /// <summary>
        /// Description of value pool.
        /// 
        /// Should also contain information on the principles used for sorting the value pool's values (i.e....sorting by particular principle,....sorting by value code).
        /// 
        /// Written beginning with a capital letter.
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }
        private String mValueTextExists;
        /// <summary>
        /// Here it is stated whether there are texts or not for the value pool's values, and whether they are in the table in ValueExtra. There are the following alternatives:
        /// 
        /// L = Long value text exists
        /// S = Short value text exists
        /// B = Both long and short value text exist
        /// N = No value texts for any values
        /// 
        /// 
        /// In the table Value (see descriptions of these columns) there are two columns for value texts, ValueTextS (for short texts) and ValueTextL (for long texts). If ValueTextExists = L, the value text is taken from column ValueTextL in the table Value. If ValueTextExists = S, the value text is taken from column ValueTextS in the table Value. If ValueTextExists = B, the value presentation is determined by what is specified in the field ValuePres in the tables ValuePool or ValueSet. If ValueTextExists = N, the values are presented only by a code in the retrieval interface.
        /// </summary>
        public String ValueTextExists
        {
            get { return mValueTextExists; }
        }
        private String mValuePres;
        /// <summary>
        /// Here it is shown how the values should be presented after retrieval. There are the following alternatives:
        /// 
        /// A = Both code and short text should be presented
        /// B = Both code and long text should be presented
        /// C = Value code should be presented
        /// T = Long value text should be presented
        /// S = Short value text should be presented
        /// </summary>
        public String ValuePres
        {
            get { return mValuePres; }
        }
        private String mMetaId;
        /// <summary>
        /// MetaId can be used to link the information in this table to an external system.
        /// </summary>
        public String MetaId
        {
            get { return mMetaId; }
        }

        public Dictionary<string, ValuePoolTexts> texts = new Dictionary<string, ValuePoolTexts>();

        public ValuePoolRow(DataRow myRow, SqlDbConfig_23 dbconf, StringCollection languageCodes)
        {
            this.mValuePool = myRow[dbconf.ValuePool.ValuePoolCol.Label()].ToString();
            this.mDescription = myRow[dbconf.ValuePool.DescriptionCol.Label()].ToString();
            this.mValueTextExists = myRow[dbconf.ValuePool.ValueTextExistsCol.Label()].ToString();
            this.mValuePres = myRow[dbconf.ValuePool.ValuePresCol.Label()].ToString();
            this.mMetaId = myRow[dbconf.ValuePool.MetaIdCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new ValuePoolTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for ValuePool  for one language.
    /// The table describes which value pools exist in the database. The value pool brings together all values and aggregates for a classification or a variation of a classification.
    /// </summary>
    public class ValuePoolTexts
    {
        private String mValuePoolAlias;
        /// <summary>
        /// Can be used to give the valuepool an alternative name.
        /// </summary>
        public String ValuePoolAlias
        {
            get { return mValuePoolAlias; }
        }
        private String mPresText;
        /// <summary>
        /// Presentation text for the value pool.
        /// 
        /// If there is no text, the field should be NULL.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }


        internal ValuePoolTexts(DataRow myRow, SqlDbConfig_23 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mValuePoolAlias = myRow[dbconf.ValuePoolLang2.ValuePoolAliasCol.Label(languageCode)].ToString();
                this.mPresText = myRow[dbconf.ValuePoolLang2.PresTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mValuePoolAlias = myRow[dbconf.ValuePool.ValuePoolAliasCol.Label()].ToString();
                this.mPresText = myRow[dbconf.ValuePool.PresTextCol.Label()].ToString();
            }
        }
    }

}
