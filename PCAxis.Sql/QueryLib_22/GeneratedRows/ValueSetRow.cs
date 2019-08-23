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
    /// Holds the attributes for ValueSet. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table describes the value sets that exist for the different value pools.
    /// </summary>
    public class ValueSetRow
    {
        private String mValueSet;
        /// <summary>
        /// Name of the stored value set\n.\nThe name should consist of the name of the value pool that the value set is linked to, plus a suffix. The suffix should always be used, even if there is only one value set for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.
        /// </summary>
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mElimination;
        /// <summary>
        /// Here it should be shown whether the variable can be eliminated or not.\nElimination means that the variable can be excluded when selecting the value when retrieving from the databases. The variable must in that case be able to assume a value, i.e. the sum of all integral values or another specific value, that is included in the value set. There are the following alternatives:\nN = No elimination value, i.e. the variable cannot be eliminated\nA = Elimination value is obtained by aggregation of all values in the value set\nValueCode = a selected value, included in the value set, that should be used at elimination.
        /// </summary>
        public String Elimination
        {
            get { return mElimination; }
        }
        private String mValuePool;
        /// <summary>
        /// Name of the value pool that the value set belongs to. See further description of the table ValuePool. 
        /// </summary>
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mValuePres;
        /// <summary>
        /// Used to show how the values in a value set should be presented when being retrieved. There are the following alternatives:\nA = Both code and short text should be presented\nB = Both code and long text should be presented\nK = Value code should be presented\nS = Short value text should be presented\nT = Long value text should be presented\nV = Presentation format is taken from the column ValuePres in the table ValuePool
        /// </summary>
        public String ValuePres
        {
            get { return mValuePres; }
        }
        private String mGeoAreaNo;
        /// <summary>
        /// Should contain the identification of a map that is suitable for the variable and the grouping. The field must be filled in if the column VariableType in the table SubTableVariable = G, otherwise the field is NULL.\nThe identification number should also be included in the table TextCatalog. For further information see description of TextCatalog. 
        /// </summary>
        public String GeoAreaNo
        {
            get { return mGeoAreaNo; }
        }
        private String mKDBId;
        /// <summary>
        /// 
        /// </summary>
        public String KDBId
        {
            get { return mKDBId; }
        }
        private String mSortCodeExists;
        /// <summary>
        /// Code showing whether there is a particular sorting order for the value set. Can be:\nY = Yes\nN = No\nIf SortCodeExists = Y, the sorting code must be in VSValue for all values included in the value set.\nIf SortCodeExists = N, the sorting code for the value pool is used (SortCode in the table Value).
        /// </summary>
        public String SortCodeExists
        {
            get { return mSortCodeExists; }
        }
        private String mFootnote;
        /// <summary>
        /// Shows whether there is a footnote linked to a value in the value set (FootNoteType 6). There are the following alternatives:\nB = Both obligatory and optional footnotes exist\nV = One or several optional footnotes exist.\nO = One or several obligatory footnotes exist\nS = There are no footnotes
        /// </summary>
        public String Footnote
        {
            get { return mFootnote; }
        }

        public Dictionary<string, ValueSetTexts> texts = new Dictionary<string, ValueSetTexts>();

        public ValueSetRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mValueSet = myRow[dbconf.ValueSet.ValueSetCol.Label()].ToString();
            this.mElimination = myRow[dbconf.ValueSet.EliminationCol.Label()].ToString();
            this.mValuePool = myRow[dbconf.ValueSet.ValuePoolCol.Label()].ToString();
            this.mValuePres = myRow[dbconf.ValueSet.ValuePresCol.Label()].ToString();
            this.mGeoAreaNo = myRow[dbconf.ValueSet.GeoAreaNoCol.Label()].ToString();
            this.mKDBId = myRow[dbconf.ValueSet.KDBIdCol.Label()].ToString();
            this.mSortCodeExists = myRow[dbconf.ValueSet.SortCodeExistsCol.Label()].ToString();
            this.mFootnote = myRow[dbconf.ValueSet.FootnoteCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new ValueSetTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for ValueSet  for one language.
    /// The table describes the value sets that exist for the different value pools.
    /// </summary>
    public class ValueSetTexts
    {
        private String mPresText;
        /// <summary>
        /// (Datamod vers 2.1, not yet impl. at Statistics Sweden)\nPresentation text for value set. Can be used, if needed, as presentation text for the variable in the retrieval programs. It will then be the the variable name in the px file. \nIf the field is not used, it should be NULL.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }
        private String mDescription;
        /// <summary>
        /// Description of the content of the value set.\nThe text should give a picture of the integral values, classes, aggregates and any totals, and should end with information on the number of values in the value set, including the total.\nText should begin with a capital letter.
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }


        internal ValueSetTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.ValueSetLang2.PresTextCol.Label(languageCode)].ToString();
                this.mDescription = myRow[dbconf.ValueSetLang2.DescriptionCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.ValueSet.PresTextCol.Label()].ToString();
                this.mDescription = myRow[dbconf.ValueSet.DescriptionCol.Label()].ToString();
            }
        }
    }

}
