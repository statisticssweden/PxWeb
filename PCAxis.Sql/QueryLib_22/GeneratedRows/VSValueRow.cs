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
    /// Holds the attributes for VSValue. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table links values (for a value pool) to a value set, for which data is stored in the data table.
    /// </summary>
    public class VSValueRow
    {
        private String mValueSet;
        /// <summary>
        /// Name of the value set to which the values are linked.\nSee further description in table ValueSet.
        /// </summary>
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mValuePool;
        /// <summary>
        /// Name of the value pool to which the value set belongs.\nSee further description in table ValuePool.
        /// </summary>
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mValueCode;
        /// <summary>
        /// Code for the values that are linked to the value set.\nSee further description in table Value.
        /// </summary>
        public String ValueCode
        {
            get { return mValueCode; }
        }

        public Dictionary<string, VSValueTexts> texts = new Dictionary<string, VSValueTexts>();

        public VSValueRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mValueSet = myRow[dbconf.VSValue.ValueSetCol.Label()].ToString();
            this.mValuePool = myRow[dbconf.VSValue.ValuePoolCol.Label()].ToString();
            this.mValueCode = myRow[dbconf.VSValue.ValueCodeCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new VSValueTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for VSValue  for one language.
    /// The table links values (for a value pool) to a value set, for which data is stored in the data table.
    /// </summary>
    public class VSValueTexts
    {
        private String mSortCode;
        /// <summary>
        /// Sorting code for values within the value set. Dictates the presentation order for the value set's values when retrieving from the database and presenting the table.\nSo that this sorting code can be applied, the field SortCodeExists in the table ValueSet must be filled with Y. If it is N, the sorting code in the table Value is used instead.\nIf there is no sorting code, the field should be NULL.
        /// </summary>
        public String SortCode
        {
            get { return mSortCode; }
        }


        internal VSValueTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mSortCode = myRow[dbconf.VSValueLang2.SortCodeCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mSortCode = myRow[dbconf.VSValue.SortCodeCol.Label()].ToString();
            }
        }
    }

}
