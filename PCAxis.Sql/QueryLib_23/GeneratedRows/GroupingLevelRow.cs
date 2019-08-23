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
    /// Holds the attributes for GroupingLevel. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table describes the levels within a grouping.
    /// 
    /// The table has to exist for both hierarchical and non-hierarchical groupings, but does not have to be used for the non-hierarchical.
    /// </summary>
    public class GroupingLevelRow
    {
        private String mGrouping;
        /// <summary>
        /// Name of grouping.
        /// 
        /// See further in the description of the table Grouping.
        /// </summary>
        public String Grouping
        {
            get { return mGrouping; }
        }
        private String mLevelNo;
        /// <summary>
        /// Number for sorting a level within a grouping. The highest level should always be 1.
        /// </summary>
        public String LevelNo
        {
            get { return mLevelNo; }
        }
        private String mGeoAreaNo;
        /// <summary>
        /// Should contain the identification of a map that is suitable for the variable and the grouping. The field must be filled in if the column VariableType in the table SubTableVariable = G, otherwise the field is NULL.
        /// 
        /// The identification number should also be included in the table TextCatalog. For further information see description of TextCatalog.
        /// </summary>
        public String GeoAreaNo
        {
            get { return mGeoAreaNo; }
        }

        public Dictionary<string, GroupingLevelTexts> texts = new Dictionary<string, GroupingLevelTexts>();

        public GroupingLevelRow(DataRow myRow, SqlDbConfig_23 dbconf, StringCollection languageCodes)
        {
            this.mGrouping = myRow[dbconf.GroupingLevel.GroupingCol.Label()].ToString();
            this.mLevelNo = myRow[dbconf.GroupingLevel.LevelNoCol.Label()].ToString();
            this.mGeoAreaNo = myRow[dbconf.GroupingLevel.GeoAreaNoCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new GroupingLevelTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for GroupingLevel  for one language.
    /// The table describes the levels within a grouping.
    /// 
    /// The table has to exist for both hierarchical and non-hierarchical groupings, but does not have to be used for the non-hierarchical.
    /// </summary>
    public class GroupingLevelTexts
    {
        private String mLevelText;
        /// <summary>
        /// The name of the level.
        /// </summary>
        public String LevelText
        {
            get { return mLevelText; }
        }


        internal GroupingLevelTexts(DataRow myRow, SqlDbConfig_23 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mLevelText = myRow[dbconf.GroupingLevelLang2.LevelTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mLevelText = myRow[dbconf.GroupingLevel.LevelTextCol.Label()].ToString();
            }
        }
    }

}
