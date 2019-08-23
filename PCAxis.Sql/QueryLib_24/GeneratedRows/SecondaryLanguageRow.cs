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
    /// Holds the attributes for SecondaryLanguage. (This entity is language independent.)
    /// 
    /// Information about secondary languages (if any)
    /// </summary>
    public class SecondaryLanguageRow
    {
        private String mMainTable;
        /// <summary>
        /// Name of main table
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mLanguage;
        /// <summary>
        /// Name of secondary language.
        /// </summary>
        public String Language
        {
            get { return mLanguage; }
        }
        private String mCompletelyTranslated;
        /// <summary>
        /// Code which shows whether all the table's presentation texts are translated to English or not. This column is necessary so that it is possible to determine from the retrieval interface whether the table will be shown in English or not.
        /// 
        /// Valid values:
        /// Y - the table is completely translated to the secondary language
        /// N - the table is not completely translated to the secondary language
        /// </summary>
        public String CompletelyTranslated
        {
            get { return mCompletelyTranslated; }
        }
        private String mPublished;
        /// <summary>
        /// Shows if the secondary language is published or not.
        /// 
        /// Valid values:
        /// Y = yes
        /// N = no
        /// </summary>
        public String Published
        {
            get { return mPublished; }
        }
        private String mUserId;
        /// <summary>
        /// 
        /// </summary>
        public String UserId
        {
            get { return mUserId; }
        }
        private String mLogDate;
        /// <summary>
        /// 
        /// </summary>
        public String LogDate
        {
            get { return mLogDate; }
        }

        public SecondaryLanguageRow(DataRow myRow, SqlDbConfig_24 dbconf)
        {
            this.mMainTable = myRow[dbconf.SecondaryLanguage.MainTableCol.Label()].ToString();
            this.mLanguage = myRow[dbconf.SecondaryLanguage.LanguageCol.Label()].ToString();
            this.mCompletelyTranslated = myRow[dbconf.SecondaryLanguage.CompletelyTranslatedCol.Label()].ToString();
            this.mPublished = myRow[dbconf.SecondaryLanguage.PublishedCol.Label()].ToString();
            this.mUserId = myRow[dbconf.SecondaryLanguage.UserIdCol.Label()].ToString();
            this.mLogDate = myRow[dbconf.SecondaryLanguage.LogDateCol.Label()].ToString();
        }
    }
}
