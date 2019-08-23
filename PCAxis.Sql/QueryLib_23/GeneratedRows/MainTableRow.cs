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
    /// Holds the attributes for MainTable. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// This table is the central compilation point for material and contains general information for the data tables that are linked to the table.
    /// </summary>
    public class MainTableRow
    {
        private String mMainTable;
        /// <summary>
        /// Summarised names of the statistical material and its underlying sub-tables with stored data.
        /// 
        /// The name should be descriptive, max 20 characters, beginning with a capital letter and contain only letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mTableStatus;
        /// <summary>
        /// Code for the table's status, giving information on where the table fits into the production and publishing processes. Table status decides whether a table can be seen or not in the indata and outdata programs.
        /// 
        /// There are the following alternatives:
        /// M = Only metadata input
        /// E = The table is new and empty data tables have been created
        /// U = The table is currently being updated and retrievals cannot be done
        /// N = The table is loaded with new, but not yet officially released data, only accessible for product staff
        /// O = The table is ready for official release, only accessible for product staff, locked for updating
        /// D = The table is not updated but is accessible to everybody
        /// A = The table is still updating and the table is accessible to all (with authorisation according to PresCategory)
        /// </summary>
        public String TableStatus
        {
            get { return mTableStatus; }
        }
        private String mTableId;
        /// <summary>
        /// Unique identity for main table. Can be used in requests from the end users, etc.
        /// </summary>
        public String TableId
        {
            get { return mTableId; }
        }
        private String mPresCategory;
        /// <summary>
        /// Presentation category for all sub-tables and content columns in the table. There are three alternatives:
        /// 
        /// O = Official, i.e. tables that are officially released and accessible to all users on the external servers or are available on the production server, with plans for official release. (Only tables with PresCategory = O are on the external servers)
        /// I = Internal, i.e. tables are only accessible for internal users.The table should never be published to the general public.
        /// P = Private, i.e. tables are only accessible for certain internal users.
        /// The code is also written with capital letters.
        /// </summary>
        public String PresCategory
        {
            get { return mPresCategory; }
        }
        private String mFirstPublished;
        /// <summary>
        /// States when a main table was first published. The value is optional.
        /// </summary>
        public String FirstPublished
        {
            get { return mFirstPublished; }
        }
        private String mSpecCharExists;
        /// <summary>
        /// Specifies if a column for special symbols exists in the data table (s) and if it should be used at retrieval.
        /// There are the following alternatives:
        /// 
        /// Y = Yes, column for special symbols exists in the database, wich should be used at retrieval
        /// E = Yes, column for special symbols exists in the database, but is not used at retrieval
        /// N = No, column for special symbols does not exist in the database
        /// 
        /// If SpecCharExists =Y, there is an extra content column for all content columns in the data table. These special columns have the same names as the content column they belong to, with the suffix X. The format is varchar(2), i.e. the same as CharacterType in the table SpecialCharacter.
        /// </summary>
        public String SpecCharExists
        {
            get { return mSpecCharExists; }
        }
        private String mSubjectCode;
        /// <summary>
        /// Code for subject area, i.e. BE
        /// </summary>
        public String SubjectCode
        {
            get { return mSubjectCode; }
        }
        private String mMetaId;
        /// <summary>
        /// MetaId can be used to link the information in this table to an external system.
        /// </summary>
        public String MetaId
        {
            get { return mMetaId; }
        }
        private String mProductCode;
        /// <summary>
        /// Id for the product that corresponds to the table. The field is optional.
        /// </summary>
        public String ProductCode
        {
            get { return mProductCode; }
        }
        private String mTimeScale;
        /// <summary>
        /// Name on the time scale that is used in the material. See further description of the table TimeScale.
        /// </summary>
        public String TimeScale
        {
            get { return mTimeScale; }
        }

        public Dictionary<string, MainTableTexts> texts = new Dictionary<string, MainTableTexts>();

        public MainTableRow(DataRow myRow, SqlDbConfig_23 dbconf, StringCollection languageCodes)
        {
            this.mMainTable = myRow[dbconf.MainTable.MainTableCol.Label()].ToString();
            this.mTableStatus = myRow[dbconf.MainTable.TableStatusCol.Label()].ToString();
            this.mTableId = myRow[dbconf.MainTable.TableIdCol.Label()].ToString();
            this.mPresCategory = myRow[dbconf.MainTable.PresCategoryCol.Label()].ToString();
            this.mFirstPublished = myRow[dbconf.MainTable.FirstPublishedCol.Label()].ToString();
            this.mSpecCharExists = myRow[dbconf.MainTable.SpecCharExistsCol.Label()].ToString();
            this.mSubjectCode = myRow[dbconf.MainTable.SubjectCodeCol.Label()].ToString();
            this.mMetaId = myRow[dbconf.MainTable.MetaIdCol.Label()].ToString();
            this.mProductCode = myRow[dbconf.MainTable.ProductCodeCol.Label()].ToString();
            this.mTimeScale = myRow[dbconf.MainTable.TimeScaleCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new MainTableTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for MainTable  for one language.
    /// This table is the central compilation point for material and contains general information for the data tables that are linked to the table.
    /// </summary>
    public class MainTableTexts
    {
        private String mPresText;
        /// <summary>
        /// The descriptive text which is presented when selecting a table in the retrieval interface.
        /// 
        /// The text should be unique and contain information on all dividing variables, including time scale (at the end). Information on unit, base time, fixed/current prices, calendar-adjusted or seasonally adjusted should normally NOT be included, as this is given in the presentation text for Contents. There can however be exceptions, i.e. for main tables that contain the same information except that the calculations are based on different base times.
        /// 
        /// The text should begin with a capital letter, should not end with a full stop, and should not include the % symbol. It should not finish with a number that relates to something other than the point of time.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }
        private String mPresTextS;
        /// <summary>
        /// Presentation text in short. Only needs to be filled in if the main table has several content columns. The text comes from the main table's PresText but should not contain information on variable or time.
        /// 
        /// Used in the retrieval interface as an introduction to the table heading, where several contents are chosen. Also used as content information in a PC-AXIS file, in some cases several contents are chosen at the same time and put into the same file.
        /// 
        /// Should begin with a capital letter and not end with a full-stop. The text should not include a % symbol.
        /// 
        /// If a short presentation text is not available, the field should be NULL.
        /// </summary>
        public String PresTextS
        {
            get { return mPresTextS; }
        }
        private String mContentsVariable;
        /// <summary>
        /// Can be used for main tables with several contents to specify a collective name for the content columns. The content variable is used as the general name ("type") which is in the TextCatalog. Used in the retrieval interface in the heading when table is presented, "...by industry, time and [content variable]"
        /// 
        /// If the field is not used, it should be NULL.
        /// </summary>
        public String ContentsVariable
        {
            get { return mContentsVariable; }
        }


        internal MainTableTexts(DataRow myRow, SqlDbConfig_23 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.MainTableLang2.PresTextCol.Label(languageCode)].ToString();
                this.mPresTextS = myRow[dbconf.MainTableLang2.PresTextSCol.Label(languageCode)].ToString();
                this.mContentsVariable = myRow[dbconf.MainTableLang2.ContentsVariableCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.MainTable.PresTextCol.Label()].ToString();
                this.mPresTextS = myRow[dbconf.MainTable.PresTextSCol.Label()].ToString();
                this.mContentsVariable = myRow[dbconf.MainTable.ContentsVariableCol.Label()].ToString();
            }
        }
    }

}
