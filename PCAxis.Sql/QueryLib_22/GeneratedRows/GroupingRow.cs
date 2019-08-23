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
    /// Holds the attributes for Grouping. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table describes the groupings which exist in the macro database. Used for the grouping of values for presentation purposes.
    /// </summary>
    public class GroupingRow
    {
        private String mGrouping;
        /// <summary>
        /// Name of grouping.\nThe name should consist of the name of the value pool that the grouping is linked to + a suffix. The suffix should always be used, even if there is only one grouping for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.\nThe name is written beginning with a capital letter.
        /// </summary>
        public String Grouping
        {
            get { return mGrouping; }
        }
        private String mValuePool;
        /// <summary>
        /// Name of the value pool that the grouping belongs to.\nSee further in the description of the table Organization.
        /// </summary>
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mHierarchy;
        /// <summary>
        /// Shows if the grouping is hierarchic or not. Can be:\nN = No\nB = Balanced\nU = Unbalanced\nFor non hierarchic groupings Hierarchy should always be N. \nIn a balanced hierarchy all branches are the same length, i.e. with the same number of levels. In an unbalanced hierarchy the number of levels and the length of the levels can vary within the hierarchy. \nAt statistics \n,\n Not yet implemented.
        /// </summary>
        public String Hierarchy
        {
            get { return mHierarchy; }
        }
        private String mGroupPres;
        /// <summary>
        /// Code which indicates how a grouping should be presented, as an aggregated value, integral value or both. There are the following alternatives:\nA = aggregated value should be shown\nI = integral value should be shown\nB = both aggregated and integral values should be shown\nAt statistics \n,\n Not yet implemented.
        /// </summary>
        public String GroupPres
        {
            get { return mGroupPres; }
        }
        private String mDescription;
        /// <summary>
        /// Description of grouping. Should give an idea of how the grouping has been put together.\nWritten beginning with a capital letter.\nIf a description is not available, the field should be NULL.
        /// </summary>
        public String Description
        {
            get { return mDescription; }
        }
        private String mKDBid;
        /// <summary>
        /// At statistics \n,\n The field is currently not used. For the present, should be NULL.\nIt is planned that this will contain the name of the corresponding classification in the Classification Database (KDB).
        /// </summary>
        public String KDBid
        {
            get { return mKDBid; }
        }

        public Dictionary<string, GroupingTexts> texts = new Dictionary<string, GroupingTexts>();

        public GroupingRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mGrouping = myRow[dbconf.Grouping.GroupingCol.Label()].ToString();
            this.mValuePool = myRow[dbconf.Grouping.ValuePoolCol.Label()].ToString();
            this.mHierarchy = myRow[dbconf.Grouping.HierarchyCol.Label()].ToString();
            this.mGroupPres = myRow[dbconf.Grouping.GroupPresCol.Label()].ToString();
            this.mDescription = myRow[dbconf.Grouping.DescriptionCol.Label()].ToString();
            this.mKDBid = myRow[dbconf.Grouping.KDBidCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new GroupingTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for Grouping  for one language.
    /// The table describes the groupings which exist in the macro database. Used for the grouping of values for presentation purposes.
    /// </summary>
    public class GroupingTexts
    {
        private String mPresText;
        /// <summary>
        /// 
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }
        private String mSortCode;
        /// <summary>
        /// Sorting code to enable the presentation of the groupings within a value pool in a logical order.\nIf there is no sorting code, the field should be NULL.\nAt statistics \n,\n Not yet implemented.
        /// </summary>
        public String SortCode
        {
            get { return mSortCode; }
        }


        internal GroupingTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.GroupingLang2.PresTextCol.Label(languageCode)].ToString();
                this.mSortCode = myRow[dbconf.GroupingLang2.SortCodeCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.Grouping.PresTextCol.Label()].ToString();
                this.mSortCode = myRow[dbconf.Grouping.SortCodeCol.Label()].ToString();
            }
        }
    }

}
