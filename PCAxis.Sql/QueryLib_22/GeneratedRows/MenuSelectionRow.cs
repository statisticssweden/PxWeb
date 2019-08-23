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
    /* For SubjectArea*/

    /// <summary>
    /// Holds the attributes for MenuSelection. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table is used to enable the presentation of any number of eligible levels above the table MainTable. The table acts as the entry point to the databases. \nAt statistics \n,\n There are the subject area, statistics area (are planned to be introduced at a later stage), product, table group and main table. All records in MenuSelection should also be in MenuSelection_Eng.
    /// </summary>
    public class MenuSelectionRow
    {
        private String mMenu;
        /// <summary>
        /// Code for relevant menu level. If LevelNo = 1, Menu should be filled with START. Code for subject areas may not exceed 20 characters. \nAt statistics \n,\n \nExample of menu codes for subject areas: AM, BO.\nExample for menu codes for products: AM0401, BO0101.\nExample for menu codes for table groups: AM0401A, AM0401B.
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
        /// Number of menu level, where 1 refers to the highest level.\nA type of object should always have the same LevelNo.\nAt statistics \n,\n \n1 = Subject area\n3 = Product\n4 = Table group\n5 = Main table\nThe highest level number should be given in the table MetaAdm (see description in this table).
        /// </summary>
        public String LevelNo
        {
            get { return mLevelNo; }
        }
        private String mInternalId;
        /// <summary>
        /// At statistics \n,\nIdentifying code for the Product database (PDB). Should be filled in for:\n- Subject areas, i.e. when LevelNo is 1. InternalId should then be three digits. The code is called AmnesomradeId in PDB.\n- Products, i.e. when LevelNo is 3. InternalId should then be four digits. The code is called ProduktId in PDB. \nFor other levels the field should be NULL. 
        /// </summary>
        public String InternalId
        {
            get { return mInternalId; }
        }

        public Dictionary<string, MenuSelectionTexts> texts = new Dictionary<string, MenuSelectionTexts>();

        public MenuSelectionRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mMenu = myRow[dbconf.MenuSelection.MenuCol.Label()].ToString();
            this.mSelection = myRow[dbconf.MenuSelection.SelectionCol.Label()].ToString();
            this.mLevelNo = myRow[dbconf.MenuSelection.LevelNoCol.Label()].ToString();
            this.mInternalId = myRow[dbconf.MenuSelection.InternalIdCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new MenuSelectionTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for MenuSelection  for one language.
    /// The table is used to enable the presentation of any number of eligible levels above the table MainTable. The table acts as the entry point to the databases. \nAt statistics \n,\n There are the subject area, statistics area (are planned to be introduced at a later stage), product, table group and main table. All records in MenuSelection should also be in MenuSelection_Eng.
    /// </summary>
    public class MenuSelectionTexts
    {
        private String mPresText;
        /// <summary>
        /// Presentation text for MenuSelection.\nAt statistics \n,\n should be filled in for all levels apart from the lowest level, main table, where PresText should be NULL.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }
        private String mPresTextS;
        /// <summary>
        /// Short presentation text for MenuSelection.\nIf a short presentation text is not available, the field should be NULL.
        /// </summary>
        public String PresTextS
        {
            get { return mPresTextS; }
        }
        private String mDescription;
        /// <summary>
        /// Descriptive text for MenuSelection.\nIf a description is not available, the field should be NULL.
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }
        private String mSortCode;
        /// <summary>
        /// Sorting code to dictate the presentation order for the eligible alternatives on each level.\nIf there is no sorting code, the field should be NULL.
        /// </summary>
        public String SortCode
        {
            get { return mSortCode; }
        }
        private String mPresentation;
        /// <summary>
        /// Shows how a menu alternative can be used. Alternatives: \nA = Active, visible and can be selected\nP = Passive, is visible but cannot be selected\nN = Not shown in the menu  
        /// </summary>
        public String Presentation
        {
            get { return mPresentation; }
        }


        internal MenuSelectionTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
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
