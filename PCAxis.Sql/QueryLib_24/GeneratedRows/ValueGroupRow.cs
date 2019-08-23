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
    /// Holds the attributes for ValueGroup. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table link values/ value set with groups
    /// </summary>
    public class ValueGroupRow
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
        private String mGroupCode;
        /// <summary>
        /// 
        /// See further in the description of the table Value.
        /// </summary>
        public String GroupCode
        {
            get { return mGroupCode; }
        }
        private String mValueCode;
        /// <summary>
        /// Code for the value contained in the group. Retrieved from the table Value, column value code.
        /// See further in the description of the table Value
        /// Se beskrivning av tabellen Varde.
        /// </summary>
        public String ValueCode
        {
            get { return mValueCode; }
        }
        private String mValuePool;
        /// <summary>
        /// The name of the value set, the grouping is attached.
        /// See further in the description of the table ValuePool
        /// 
        /// Se beskrivning av tabellen Vardeforrad.
        /// </summary>
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mGroupLevel;
        /// <summary>
        /// Indicates witch level the group code belongs to.
        /// </summary>
        public String GroupLevel
        {
            get { return mGroupLevel; }
        }
        private String mValueLevel;
        /// <summary>
        /// Indicates witch level the value code belongs to.
        /// </summary>
        public String ValueLevel
        {
            get { return mValueLevel; }
        }

        public Dictionary<string, ValueGroupTexts> texts = new Dictionary<string, ValueGroupTexts>();

        public ValueGroupRow(DataRow myRow, SqlDbConfig_24 dbconf, StringCollection languageCodes)
        {
            this.mGrouping = myRow[dbconf.ValueGroup.GroupingCol.Label()].ToString();
            this.mGroupCode = myRow[dbconf.ValueGroup.GroupCodeCol.Label()].ToString();
            this.mValueCode = myRow[dbconf.ValueGroup.ValueCodeCol.Label()].ToString();
            this.mValuePool = myRow[dbconf.ValueGroup.ValuePoolCol.Label()].ToString();
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
    /// The table link values/ value set with groups
    /// </summary>
    public class ValueGroupTexts
    {
        private String mSortCode;
        /// <summary>
        /// Code for sorting groups within a group, in order to present them in a logical order.
        /// 
        /// If any group within a grouping of a range is the sort code, all the teams have that. If the sort code is missing, the field shall be NULL.
        /// </summary>
        public String SortCode
        {
            get { return mSortCode; }
        }


        internal ValueGroupTexts(DataRow myRow, SqlDbConfig_24 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mSortCode = myRow[dbconf.ValueGroupLang2.SortCodeCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mSortCode = myRow[dbconf.ValueGroup.SortCodeCol.Label()].ToString();
            }
        }
    }

}
