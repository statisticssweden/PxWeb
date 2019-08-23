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
    /* For SubjectArea*/

    /// <summary>
    /// Holds the attributes for MenuSelection. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table is used to enable the presentation of any number of eligible levels above the table MainTable. The table acts as the entry point to the databases.
    /// All records in MenuSelection should also be in the corresponding MenuSelections for secondary languages (if any).
    /// </summary>
    public class MenuSelectionRow
    {
        private String mMenu;
        /// <summary>
        /// Code for relevant menu level. If LevelNo = 1, Menu should be filled with START. Code for subject areas may not exceed 20 characters.
        /// </summary>
        public String Menu
        {
            get { return mMenu; }
        }
        private String mSelection;
        /// <summary>
        /// The code for the nearest underlying eligible alternative in the relevant menu level. A menu can contain objects from different levels. Code for subject areas may not exceed 20 characters.
        /// </summary>
        public String Selection
        {
            get { return mSelection; }
        }
        private String mLevelNo;
        /// <summary>
        /// Number of menu level, where 1 refers to the highest level.
        /// A type of object should always have the same LevelNo.
        /// 
        /// The highest level number should be given in the table MetaAdm (see description in that table).
        /// </summary>
        public String LevelNo
        {
            get { return mLevelNo; }
        }
        private String mMetaId;
        /// <summary>
        /// MetaId can be used to link the information in this table to an external system.
        /// </summary>
        public String MetaId
        {
            get { return mMetaId; }
        }

        public Dictionary<string, MenuSelectionTexts> texts = new Dictionary<string, MenuSelectionTexts>();

        public MenuSelectionRow(DataRow myRow, SqlDbConfig_24 dbconf, StringCollection languageCodes)
        {
            this.mMenu = myRow[dbconf.MenuSelection.MenuCol.Label()].ToString();
            this.mSelection = myRow[dbconf.MenuSelection.SelectionCol.Label()].ToString();
            this.mLevelNo = myRow[dbconf.MenuSelection.LevelNoCol.Label()].ToString();
            this.mMetaId = myRow[dbconf.MenuSelection.MetaIdCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new MenuSelectionTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for MenuSelection  for one language.
    /// The table is used to enable the presentation of any number of eligible levels above the table MainTable. The table acts as the entry point to the databases.
    /// All records in MenuSelection should also be in the corresponding MenuSelections for secondary languages (if any).
    /// </summary>
    public class MenuSelectionTexts
    {
        private String mPresText;
        /// <summary>
        /// Presentation text for MenuSelection.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }
        private String mPresTextS;
        /// <summary>
        /// Short presentation text for MenuSelection.
        /// 
        /// If a short presentation text is not available, the field should be NULL.
        /// </summary>
        public String PresTextS
        {
            get { return mPresTextS; }
        }
        private String mDescription;
        /// <summary>
        /// Descriptive text for MenuSelection.
        /// 
        /// If a description is not available, the field should be NULL.
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }
        private String mSortCode;
        /// <summary>
        /// Sorting code to dictate the presentation order for the eligible alternatives on each level.
        /// 
        /// If there is no sorting code, the field should be NULL.
        /// </summary>
        public String SortCode
        {
            get { return mSortCode; }
        }
        private String mPresentation;
        /// <summary>
        /// Shows how a menu alternative can be used. Alternatives:
        /// 
        /// A = Active, visible and can be selected
        /// P = Passive, is visible but cannot be selected
        /// N = Not shown in the menu
        /// </summary>
        public String Presentation
        {
            get { return mPresentation; }
        }


        internal MenuSelectionTexts(DataRow myRow, SqlDbConfig_24 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.MenuSelectionLang2.PresTextCol.Label(languageCode)].ToString();
                this.mPresTextS = myRow[dbconf.MenuSelectionLang2.PresTextSCol.Label(languageCode)].ToString();
                this.mDescription = myRow[dbconf.MenuSelectionLang2.DescriptionCol.Label(languageCode)].ToString();
                this.mSortCode = myRow[dbconf.MenuSelectionLang2.SortCodeCol.Label(languageCode)].ToString();
                this.mPresentation = myRow[dbconf.MenuSelectionLang2.PresentationCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.MenuSelection.PresTextCol.Label()].ToString();
                this.mPresTextS = myRow[dbconf.MenuSelection.PresTextSCol.Label()].ToString();
                this.mDescription = myRow[dbconf.MenuSelection.DescriptionCol.Label()].ToString();
                this.mSortCode = myRow[dbconf.MenuSelection.SortCodeCol.Label()].ToString();
                this.mPresentation = myRow[dbconf.MenuSelection.PresentationCol.Label()].ToString();
            }
        }
    }

}
