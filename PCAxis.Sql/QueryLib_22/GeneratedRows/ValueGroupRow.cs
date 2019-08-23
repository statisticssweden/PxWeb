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
    /// Holds the attributes for ValueGroup. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// Tabellen kopplar ihop värden/värdemängder med grupperingar. 
    /// </summary>
    public class ValueGroupRow
    {
        private String mGrouping;
        /// <summary>
        /// Namn på grupperingen. \nSe vidare beskrivningen av tabellen Gruppering.
        /// </summary>
        public String Grouping
        {
            get { return mGrouping; }
        }
        private String mGroupCode;
        /// <summary>
        /// Kod för grupp. Hämtas från tabellen Varde, kolumnen Vardekod.\nSe beskrivning av tabellen Varde.
        /// </summary>
        public String GroupCode
        {
            get { return mGroupCode; }
        }
        private String mValueCode;
        /// <summary>
        /// Kod för värde som ingår i grupp. Hämtas från tabellen Varde, kolumnen Vardekod.\nSe beskrivning av tabellen Varde.
        /// </summary>
        public String ValueCode
        {
            get { return mValueCode; }
        }
        private String mGroupLevel;
        /// <summary>
        /// Indicates wich level the group code belongs to. 
        /// </summary>
        public String GroupLevel
        {
            get { return mGroupLevel; }
        }
        private String mValueLevel;
        /// <summary>
        /// Indicates wich level the value code belongs to. 
        /// </summary>
        public String ValueLevel
        {
            get { return mValueLevel; }
        }

        public Dictionary<string, ValueGroupTexts> texts = new Dictionary<string, ValueGroupTexts>();

        public ValueGroupRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mGrouping = myRow[dbconf.ValueGroup.GroupingCol.Label()].ToString();
            this.mGroupCode = myRow[dbconf.ValueGroup.GroupCodeCol.Label()].ToString();
            this.mValueCode = myRow[dbconf.ValueGroup.ValueCodeCol.Label()].ToString();
            this.mGroupLevel = myRow[dbconf.ValueGroup.GroupLevelCol.Label()].ToString();
            this.mValueLevel = myRow[dbconf.ValueGroup.ValueLevelCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new ValueGroupTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for ValueGroup  for one language.
    /// Tabellen kopplar ihop värden/värdemängder med grupperingar. 
    /// </summary>
    public class ValueGroupTexts
    {
        private String mValuePool;
        /// <summary>
        /// Namn på det värdeförråd, som grupperingen är kopplad till. \nSe beskrivning av tabellen Vardeforrad.
        /// </summary>
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mSortCode;
        /// <summary>
        /// Kod för sortering av grupper inom en gruppering, för att kunna presentera dem i en logisk ordning. \nOm någon grupp inom en gruppering för en värdemängd har sorteringskod, ska samtliga grupper ha det. \n sorteringskod saknas, skall fältet vara NULL.
        /// </summary>
        public String SortCode
        {
            get { return mSortCode; }
        }


        internal ValueGroupTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mValuePool = myRow[dbconf.ValueGroupLang2.ValuePoolCol.Label(languageCode)].ToString();
                this.mSortCode = myRow[dbconf.ValueGroupLang2.SortCodeCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mValuePool = myRow[dbconf.ValueGroup.ValuePoolCol.Label()].ToString();
                this.mSortCode = myRow[dbconf.ValueGroup.SortCodeCol.Label()].ToString();
            }
        }
    }

}
