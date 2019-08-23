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
    /// Holds the attributes for SpecialCharacter. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains information on the special characters that are used in the database's data tables. Special characters such as ....or - , can be used to show that data is missing, is not relevant or is too uncertain to be given.
    /// </summary>
    public class SpecialCharacterRow
    {
        private String mCharacterType;
        /// <summary>
        /// Identifying code for the special character.
        /// 
        /// Given in the form of a number, from 1 upwards. The highest acceptable number is given in the table MetaAdm, which is 99 (see description in table MetaAdm).
        /// 
        /// If PresMissingLine in Contents contains the identity for a special character, this character must be represented here. See also descriptions of PresCellsZero and PresMissingLine in Contents, PresCharacter in SpecialCharacter and the table MetaAdm.
        /// </summary>
        public String CharacterType
        {
            get { return mCharacterType; }
        }
        private String mAggregPossible;
        /// <summary>
        /// Used to show whether the data cell with the special character can be aggregated or not. There are the following alternatives:
        /// 
        /// Y = Yes
        /// N = No
        /// 
        /// If AggregPossible = Y, the specific data cell, even if not shown, can be included in an aggregation.
        /// </summary>
        public String AggregPossible
        {
            get { return mAggregPossible; }
        }
        private String mDataCellPres;
        /// <summary>
        /// Provides the retrieval programs with information concerning the presentation of a special character;  with data and special character or with special character only.
        /// 
        /// There are the following alternatives:
        /// Y = The data cell should be presented together with the special character
        /// N = The special character alone should be presented
        /// </summary>
        public String DataCellPres
        {
            get { return mDataCellPres; }
        }
        private String mDataCellFilled;
        /// <summary>
        /// Shows whether the data cell must be filled in or not. There are the following alternatives:
        /// 
        /// V = Value must be filled in
        /// N = No, the data cell should not be  filled in but should be NULL
        /// F = Any, i.e. the data cell can be filled in or can be NULL.
        /// 0 = The data cell should contain 0 (zero) only
        /// </summary>
        public String DataCellFilled
        {
            get { return mDataCellFilled; }
        }

        public Dictionary<string, SpecialCharacterTexts> texts = new Dictionary<string, SpecialCharacterTexts>();

        public SpecialCharacterRow(DataRow myRow, SqlDbConfig_24 dbconf, StringCollection languageCodes)
        {
            this.mCharacterType = myRow[dbconf.SpecialCharacter.CharacterTypeCol.Label()].ToString();
            this.mAggregPossible = myRow[dbconf.SpecialCharacter.AggregPossibleCol.Label()].ToString();
            this.mDataCellPres = myRow[dbconf.SpecialCharacter.DataCellPresCol.Label()].ToString();
            this.mDataCellFilled = myRow[dbconf.SpecialCharacter.DataCellFilledCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new SpecialCharacterTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for SpecialCharacter  for one language.
    /// The table contains information on the special characters that are used in the database's data tables. Special characters such as ....or - , can be used to show that data is missing, is not relevant or is too uncertain to be given.
    /// </summary>
    public class SpecialCharacterTexts
    {
        private String mPresCharacter;
        /// <summary>
        /// The special character as presented for the user when the table is presented when retrieved.
        /// </summary>
        public String PresCharacter
        {
            get { return mPresCharacter; }
        }
        private String mPresText;
        /// <summary>
        /// Explanation to what is written in PresCharacter.
        /// 
        /// If there is no presentation text, the field should be NULL.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }


        internal SpecialCharacterTexts(DataRow myRow, SqlDbConfig_24 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresCharacter = myRow[dbconf.SpecialCharacterLang2.PresCharacterCol.Label(languageCode)].ToString();
                this.mPresText = myRow[dbconf.SpecialCharacterLang2.PresTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresCharacter = myRow[dbconf.SpecialCharacter.PresCharacterCol.Label()].ToString();
                this.mPresText = myRow[dbconf.SpecialCharacter.PresTextCol.Label()].ToString();
            }
        }
    }

}
