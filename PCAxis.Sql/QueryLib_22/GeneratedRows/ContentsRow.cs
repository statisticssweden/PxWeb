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
    /// Holds the attributes for Contents. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// The table contains information on the content of the data table(s).The content column's name is the same as the name of the corresponding data columns in the data table.
    /// </summary>
    public class ContentsRow
    {
        private String mMainTable;
        /// <summary>
        /// Name of the main table to which the content columns are linked. See further description in the table MainTable. 
        /// </summary>
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mContents;
        /// <summary>
        /// Name of the data columns in the data table.\nA main table's content columns must have unique names within that main table but the same column name can occur in other main tables.\nThe name should be descriptive, max 20 characters, beginning with a capital letter and should only contain letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
        /// </summary>
        public String Contents
        {
            get { return mContents; }
        }
        private String mPresCode;
        /// <summary>
        /// Presentation code\nAt statistics \n,\n Consists of the main table's ProductId (i.e. code for the product) + one letter + one number or letter. The code should be unique, i.e. the same presentation code should only occur once in the meta database.
        /// </summary>
        public String PresCode
        {
            get { return mPresCode; }
        }
        private String mCopyright;
        /// <summary>
        /// Here it should be shown whether material is included in \n's Official Statistics (SOS) or not, and which rules apply to the distribution of the statistics (copyright). All content columns within a main table must belong to the same category, i.e. the same code should be in all the fields. There are the following alternatives:\n1 = included in \n's Official Statistics (no copyright)\n2 = not included in \n's Official Statistics (no copyright)\n3 = not included in \n's Official Statistics (copyright)
        /// </summary>
        public String Copyright
        {
            get { return mCopyright; }
        }
        private String mStatAuthority;
        /// <summary>
        /// Code for the authority responsible for the statistics (statistical authority).\n \nAll content columns in a main table must belong to the same statistical authority.\nData is taken from the column OrganizationCode in the table Organization. For a more detailed description, see this table.
        /// </summary>
        public String StatAuthority
        {
            get { return mStatAuthority; }
        }
        private String mProducer;
        /// <summary>
        /// At statistics \n,\n Code for the authority/organisation that is responsible for the production of the statistics.\nAll content columns in a main table must have the same producer.\nWhere Statistics Sweden produces the statistics, the official abbreviation of the unit within Statistics Sweden that is responsible for the production should be given, i.e. SCB/BV/BE.\nWhere responsibility for the production lies outside Statistics Sweden, the abbreviation for the relevant authority/organisation should be given.\nData is taken from the column OrganizationCode in the table Organization. For a more detailed description, see this table.
        /// </summary>
        public String Producer
        {
            get { return mProducer; }
        }
        private String mLastUpdated;
        /// <summary>
        /// The date of the most recent update (incl. exact time) of the main table's data tables on the production server. The date remains unchanged when copying over to the external servers. The field is updated automatically by the programs used for loading and updating of data.\nIf there is no text, the field should be NULL.
        /// </summary>
        public String LastUpdated
        {
            get { return mLastUpdated; }
        }
        private String mPublished;
        /// <summary>
        /// The most recent date for the official release of data, i.e. the date and exact time for the transfer of the main table's data tables to the external servers. The field is updated automatically by the program used for the transfer.\nIf there is no text, the field should be NULL.
        /// </summary>
        public String Published
        {
            get { return mPublished; }
        }
        private String mPresDecimals;
        /// <summary>
        /// The number of decimals that are to be presented in the table presentation when retrievals are made.
        /// </summary>
        public String PresDecimals
        {
            get { return mPresDecimals; }
        }
        private String mPresCellsZero;
        /// <summary>
        /// The data table, in principle, only stores cells with values that are not zero. Cells containing zero are only stored if another content column in the same data table has a value other than zero (0) in the corresponding cell. In this field, it should be stated how data cells that are not stored, i.e. cells for which data is missing in all content columns, should be presented. Alternatives:\nY = Yes. The cells are assumed to contain zero. Presented as zero (0).\nN = No. Data cannot be given.\nIf PresCellsZero is 'N', the field PresMissingLine should be used to indicate how these cells should be presented. See description of PresMissingLine in Contents. See also the description of the table SpecialCharacter. 
        /// </summary>
        public String PresCellsZero
        {
            get { return mPresCellsZero; }
        }
        private String mPresMissingLine;
        /// <summary>
        /// (Datamod vers 2.1, not yet impl at Statistics Sweden)\nThe field is used by the retrieval programs for presenting cells that should not be presented as zero, i.e. when PresCellsZero in Contents is 'N'. The field can either be NULL or contain the identity for a special character. The identity must in that case exist in the column CharacterType in the table SpecialCharacter, and the character must exist in the column PresCharacter in this table. If PresMissingLine is NULL, DefaultCodeMissingLine in MetaAdm is used. \nSee also descriptions of: the column PresCellsZero in the table Contents,  the columns CharacterType and PresCharacter in the table SpecialCharacter and the table MetaAdm. 
        /// </summary>
        public String PresMissingLine
        {
            get { return mPresMissingLine; }
        }
        private String mAggregPossible;
        /// <summary>
        /// Shows if the content can be aggregated or not. Applies to all distributed variables for the content column. There are the following alternatives:\nY = Yes\nN = No\nIf AggregPossible = N, the possibility to both aggregate and group the data in the retrieval interface is eliminated.
        /// </summary>
        public String AggregPossible
        {
            get { return mAggregPossible; }
        }
        private String mStockFA;
        /// <summary>
        /// Shows whether the statistical material is of the type stock, flow or average. There are the following alternatives:\nF = Flow. Measurement time refers to a specific period. The result describes events that occurred successively during the measurement period.\nA = Average. The result is made up of an average value of observation values at different measurement times.\nS = Stock. The measurement time refers to a specific point in time.\nX = Other
        /// </summary>
        public String StockFA
        {
            get { return mStockFA; }
        }
        private String mCFPrices;
        /// <summary>
        /// Current/fixed prices. Alternatives:\nF = Fixed prices.\nC = Current prices.\nIn cases where data are not relevant, the field should be NULL. 
        /// </summary>
        public String CFPrices
        {
            get { return mCFPrices; }
        }
        private String mDayAdj;
        /// <summary>
        /// Shows whether the statistical material is calendar adjusted or not during the measurement period. There are the following alternatives:\nY = Yes\nN = No
        /// </summary>
        public String DayAdj
        {
            get { return mDayAdj; }
        }
        private String mSeasAdj;
        /// <summary>
        /// Shows whether the statistical material is seasonally adjusted or not, i.e. adjusted for different periodical variations during the measurement period that may have affected the result. There are the following alternatives:\nY = Yes \nN = No
        /// </summary>
        public String SeasAdj
        {
            get { return mSeasAdj; }
        }
        private String mStoreNoChar;
        /// <summary>
        /// Number of stored characters in the data column. Number of characters should be:\n2, if StoreFormat = S,\n4, if StoreFormat = I,\n2-17 (decimals included, decimal point excluded), if StoreFormat = N\n1-30 characters, if StoreFormat = C.\nAlso see the description of column StoreFormat in the table Contents.
        /// </summary>
        public String StoreNoChar
        {
            get { return mStoreNoChar; }
        }
        private String mStoreDecimals;
        /// <summary>
        /// Number of stored decimals (0-15).\nData should be included in StoreNoChar if StoreFormat = N.
        /// </summary>
        public String StoreDecimals
        {
            get { return mStoreDecimals; }
        }
        private String mStoreFormat;
        /// <summary>
        /// Here the storage format for data cells for the content column is shown. There are the following alternatives:\nC = Varchar. For storage of special characters.\nI = Integer. For numbers of size 2 147 483 647 to - 2 147 483 648.\nN = Numeric. For larger numbers and always when the material is stored with decimals.\nS = Smallint. For numbers of size 32 767 to - 32 768.\nAlso see the description of column StoreNoChar in the table Contents.
        /// </summary>
        public String StoreFormat
        {
            get { return mStoreFormat; }
        }
        private String mStoreColumnNo;
        /// <summary>
        /// Here the content column's (data columns) order of storage amongst themselves in the data table is shown.
        /// </summary>
        public String StoreColumnNo
        {
            get { return mStoreColumnNo; }
        }

        public Dictionary<string, ContentsTexts> texts = new Dictionary<string, ContentsTexts>();

        public ContentsRow(DataRow myRow, SqlDbConfig_22 dbconf, StringCollection languageCodes)
        {
            this.mMainTable = myRow[dbconf.Contents.MainTableCol.Label()].ToString();
            this.mContents = myRow[dbconf.Contents.ContentsCol.Label()].ToString();
            this.mPresCode = myRow[dbconf.Contents.PresCodeCol.Label()].ToString();
            this.mCopyright = myRow[dbconf.Contents.CopyrightCol.Label()].ToString();
            this.mStatAuthority = myRow[dbconf.Contents.StatAuthorityCol.Label()].ToString();
            this.mProducer = myRow[dbconf.Contents.ProducerCol.Label()].ToString();
            this.mLastUpdated = myRow[dbconf.Contents.LastUpdatedCol.Label()] == DBNull.Value ? "" : Convert.ToDateTime(myRow[dbconf.Contents.LastUpdatedCol.Label()]).ToString("yyyyMMdd HH:mm");
            this.mPublished = myRow[dbconf.Contents.PublishedCol.Label()].ToString();
            this.mPresDecimals = myRow[dbconf.Contents.PresDecimalsCol.Label()].ToString();
            this.mPresCellsZero = myRow[dbconf.Contents.PresCellsZeroCol.Label()].ToString();
            this.mPresMissingLine = myRow[dbconf.Contents.PresMissingLineCol.Label()].ToString();
            this.mAggregPossible = myRow[dbconf.Contents.AggregPossibleCol.Label()].ToString();
            this.mStockFA = myRow[dbconf.Contents.StockFACol.Label()].ToString();
            this.mCFPrices = myRow[dbconf.Contents.CFPricesCol.Label()].ToString();
            this.mDayAdj = myRow[dbconf.Contents.DayAdjCol.Label()].ToString();
            this.mSeasAdj = myRow[dbconf.Contents.SeasAdjCol.Label()].ToString();
            this.mStoreNoChar = myRow[dbconf.Contents.StoreNoCharCol.Label()].ToString();
            this.mStoreDecimals = myRow[dbconf.Contents.StoreDecimalsCol.Label()].ToString();
            this.mStoreFormat = myRow[dbconf.Contents.StoreFormatCol.Label()].ToString();
            this.mStoreColumnNo = myRow[dbconf.Contents.StoreColumnNoCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new ContentsTexts(myRow, dbconf, languageCode));
            }

        }
    }

    /// <summary>
    /// Holds the language dependent attributes for Contents  for one language.
    /// The table contains information on the content of the data table(s).The content column's name is the same as the name of the corresponding data columns in the data table.
    /// </summary>
    public class ContentsTexts
    {
        private String mPresText;
        /// <summary>
        /// Presentation text used by the retrieval interface when selecting table data and which, after retrieval of a data table, forms the beginning of the table heading.\nThe presentation text should be unique within a main table.\nAt statistics \n,\n For material within a statistical product with a universally established abbreviation, i.e. AKU, KPI, the short name should be included in the presentation text in brackets.\nFor material that refers only to one point in time (i.e. FoB90), the current point in time should be given in the presentation text.\nThe text should contain information on unit (if not obvious), base time, fixed/ current prices, calendar adjustment and seasonal adjustment.\nThe text should begin with a capital letter, should not contain the % symbol and should not be finished with a full-stop.
        /// </summary>
        public String PresText
        {
            get { return mPresText; }
        }
        private String mPresTextS;
        /// <summary>
        /// Short presentation text for content column. Used in retrieval interface, e.g. when selecting table data, or as the column name when table is presented, if the main table has several contents.\nThe text should begin with a capital letter, should not contain the % symbol and should not end with a full-stop. \nIf there is no text, the field should be NULL.
        /// </summary>
        public String PresTextS
        {
            get { return mPresTextS; }
        }
        private String mUnit;
        /// <summary>
        /// Unit, e.g. number, currency, index. The given unit should apply to both the storage and the presentation. Details on unit (if not obvious) should also be written in text to the column PresText in the table Contents.\nDetails on unit  can also be stored in the table ValueExtra, column Unit, for material with variables with long value texts. Then the unit can be different for different values. In this case, %ValueExtra is written in the field unit in the table Contents.\nThe unit can also be stored in the content column in the data table. This column is always called Unit and can contain different units for different values. In this case, %DataTable is written in the field unit in the table Contents.
        /// </summary>
        public String Unit
        {
            get { return mUnit; }
        }
        private String mRefPeriod;
        /// <summary>
        /// RefPeriod relates to the time of measurement for the material. Written as text, i.e."31 December of previous year". Data is obligatory for stock material, i.e. when the field StockFA in Contents is "S".\nIf the reference time is not available, the field should be NULL.
        /// </summary>
        public String RefPeriod
        {
            get { return mRefPeriod; }
        }
        private String mBasePeriod;
        /// <summary>
        /// The base period when calculating an index or fixed prices, for example. In cases where data are not relevant, the field should be NULL.
        /// </summary>
        public String BasePeriod
        {
            get { return mBasePeriod; }
        }


        internal ContentsTexts(DataRow myRow, SqlDbConfig_22 dbconf, String languageCode)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.ContentsLang2.PresTextCol.Label(languageCode)].ToString();
                this.mPresTextS = myRow[dbconf.ContentsLang2.PresTextSCol.Label(languageCode)].ToString();
                this.mUnit = myRow[dbconf.ContentsLang2.UnitCol.Label(languageCode)].ToString();
                this.mRefPeriod = myRow[dbconf.ContentsLang2.RefPeriodCol.Label(languageCode)].ToString();
                this.mBasePeriod = myRow[dbconf.ContentsLang2.BasePeriodCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.Contents.PresTextCol.Label()].ToString();
                this.mPresTextS = myRow[dbconf.Contents.PresTextSCol.Label()].ToString();
                this.mUnit = myRow[dbconf.Contents.UnitCol.Label()].ToString();
                this.mRefPeriod = myRow[dbconf.Contents.RefPeriodCol.Label()].ToString();
                this.mBasePeriod = myRow[dbconf.Contents.BasePeriodCol.Label()].ToString();
            }
        }
    }

}
