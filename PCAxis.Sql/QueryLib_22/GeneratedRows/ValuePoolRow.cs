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
    /// Holds the attributes for ValuePool. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table describes which value pools exist in the macro database. The value pool brings together all values and aggregates for a classification or a variation of a classification.
    /// </summary>
    public class ValuePoolRow
    {
        private String mValuePool;
        /// <summary>
        /// Name of value pool.\nAt statistics \n,\n There should be a value pool for every classification or variation of a classification. The name should correspond to the name in the Classification Database (KDB), if the value pool/classification is included there. The name should be descriptive. A suffix which states the version/variation/year can also be used, i.e.SNI92BR, SUN2000.\nIf there is only one variable belonging to a particular value pool, the variable and value pool should have the same name.\nThe name should begin with a capital letter.
        /// </summary>
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mDescription;
        /// <summary>
        /// Description of value pool.\nShould also contain information on the principles used for sorting the value pool's values (i.e....sorting by particular principle,....sorting by value code).\nWritten beginning with a capital letter.
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }
        private String mValueTextExists;
        /// <summary>
        /// Here it is stated whether there are texts or not for the value pool's values, and whether they are in the table in ValueExtra. There are the following alternatives:\nL = Long value text exists\nS = Short value text exists\nB = Both long and short value text exist\nN = No value texts for any values\nX = All values are in ValueExtra\nIn the table Value (see descriptions of these columns) there are two columns for value texts, ValueTextS (for short texts) and ValueTextL (for long texts). If ValueTextExists = L, the value text is taken from column ValueTextL in the table Value. If ValueTextExists = S, the value text is taken from column ValueTextS in the table Value. If ValueTextExists = B, the value presentation is determined by what is specified in the field ValuePres in the tables ValuePool or ValueSet. If ValueTextExists = N, the values are presented only by a code in the retrieval interface. If ValueTextExists = X, the value texts are taken from table ValueExtra (see further description of this).
        /// </summary>
        public String ValueTextExists
        {
            get { return mValueTextExists; }
        }
        private String mValuePres;
        /// <summary>
        /// Here it is shown how the values should be presented after retrieval. There are the following alternatives:\nA = Both code and short text should be presented\nB = Both code and long text should be presented\nC = Value code should be presented\nT = Long value text should be presented\nS = Short value text should be presented
        /// </summary>
        public String ValuePres
        {
            get { return mValuePres; }
        }
        private String mKDBId;
        /// <summary>
        /// 
        /// </summary>
        public String KDBId
        {
            get { return mKDBId; }
        }

        public Dictionary<string, ValuePoolTexts> texts = new Dictionary<string, ValuePoolTexts>();

        public ValuePoolRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mValuePool = myRow[dbconf.ValuePool.ValuePoolCol.Label()].ToString();
            this.mDescription = myRow[dbconf.ValuePool.DescriptionCol.Label()].ToString();
            this.mValueTextExists = myRow[dbconf.ValuePool.ValueTextExistsCol.Label()].ToString();
            this.mValuePres = myRow[dbconf.ValuePool.ValuePresCol.Label()].ToString();
            this.mKDBId = myRow[dbconf.ValuePool.KDBIdCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new ValuePoolTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for ValuePool  for one language.
    /// The table describes which value pools exist in the macro database. The value pool brings together all values and aggregates for a classification or a variation of a classification.
    /// </summary>
    public class ValuePoolTexts
    {
        private String mValuePoolAlias;
        /// <summary>
        /// 
        /// </summary>
        public String ValuePoolAlias
        {
            get { return mValuePoolAlias; }
        }
        private String mPresText;
        /// <summary>
        /// At statistics \n,\n The field is currently not used. Should until further notice be NULL.\nIs planned to be used for presentation texts for value pools.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }


        internal ValuePoolTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
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
