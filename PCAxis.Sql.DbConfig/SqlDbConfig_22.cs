using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using log4net;

//This code is generated. 

namespace PCAxis.Sql.DbConfig
{
    public class SqlDbConfig_22 : SqlDbConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlDbConfig_22));

        public Ccodes Codes;
        public DbKeywords Keywords;

        #region Fields
        public TblContents Contents;
        public TblContentsLang2 ContentsLang2;
        public TblContentsTime ContentsTime;
        public TblDataStorage DataStorage;
        public TblFootnote Footnote;
        public TblFootnoteLang2 FootnoteLang2;
        public TblFootnoteContTime FootnoteContTime;
        public TblFootnoteContValue FootnoteContValue;
        public TblFootnoteContVbl FootnoteContVbl;
        public TblFootnoteContents FootnoteContents;
        public TblFootnoteGrouping FootnoteGrouping;
        public TblFootnoteMainTable FootnoteMainTable;
        public TblFootnoteMaintValue FootnoteMaintValue;
        public TblFootnoteMenuSel FootnoteMenuSel;
        public TblFootnoteSubTable FootnoteSubTable;
        public TblFootnoteValue FootnoteValue;
        public TblFootnoteVariable FootnoteVariable;
        public TblGrouping Grouping;
        public TblGroupingLang2 GroupingLang2;
        public TblGroupingLevel GroupingLevel;
        public TblGroupingLevelLang2 GroupingLevelLang2;
        public TblLink Link;
        public TblLinkLang2 LinkLang2;
        public TblLinkMenuSel LinkMenuSel;
        public TblLinkMenuSel LinkMenuSelection;  ////hack for menu: this is the 2.3 name for LinkMenuSel.  
        public TblMainTable MainTable;
        public TblMainTableLang2 MainTableLang2;
        public TblMainTablePerson MainTablePerson;
        public TblMainTableVariableHierarchy MainTableVariableHierarchy;
        public TblMenuSelection MenuSelection;
        public TblMenuSelectionLang2 MenuSelectionLang2;
        public TblMetaAdm MetaAdm;
        public TblMetabaseInfo MetabaseInfo;
        public TblOrganization Organization;
        public TblOrganizationLang2 OrganizationLang2;
        public TblPerson Person;
        public TblSpecialCharacter SpecialCharacter;
        public TblSpecialCharacterLang2 SpecialCharacterLang2;
        public TblSubTable SubTable;
        public TblSubTableLang2 SubTableLang2;
        public TblSubTableVariable SubTableVariable;
        public TblTextCatalog TextCatalog;
        public TblTextCatalogLang2 TextCatalogLang2;
        public TblTimeScale TimeScale;
        public TblTimeScaleLang2 TimeScaleLang2;
        public TblValue Value;
        public TblValueLang2 ValueLang2;
        public TblValueExtra ValueExtra;
        public TblValueExtraLang2 ValueExtraLang2;
        public TblValueGroup ValueGroup;
        public TblValueGroupLang2 ValueGroupLang2;
        public TblValuePool ValuePool;
        public TblValuePoolLang2 ValuePoolLang2;
        public TblValueSet ValueSet;
        public TblValueSetLang2 ValueSetLang2;
        public TblValueSetGrouping ValueSetGrouping;
        public TblVariable Variable;
        public TblVariableLang2 VariableLang2;
        public TblVSValue VSValue;
        public TblVSValueLang2 VSValueLang2;
        #endregion Fields

        private void initStructs()
        {


            Contents = new TblContents(this);

            ContentsLang2 = new TblContentsLang2(this);

            ContentsTime = new TblContentsTime(this);

            DataStorage = new TblDataStorage(this);

            Footnote = new TblFootnote(this);

            FootnoteLang2 = new TblFootnoteLang2(this);

            FootnoteContTime = new TblFootnoteContTime(this);

            FootnoteContValue = new TblFootnoteContValue(this);

            FootnoteContVbl = new TblFootnoteContVbl(this);

            FootnoteContents = new TblFootnoteContents(this);

            FootnoteGrouping = new TblFootnoteGrouping(this);

            FootnoteMainTable = new TblFootnoteMainTable(this);

            FootnoteMaintValue = new TblFootnoteMaintValue(this);

            FootnoteMenuSel = new TblFootnoteMenuSel(this);

            FootnoteSubTable = new TblFootnoteSubTable(this);

            FootnoteValue = new TblFootnoteValue(this);

            FootnoteVariable = new TblFootnoteVariable(this);

            Grouping = new TblGrouping(this);

            GroupingLang2 = new TblGroupingLang2(this);

            GroupingLevel = new TblGroupingLevel(this);

            GroupingLevelLang2 = new TblGroupingLevelLang2(this);

            Link = new TblLink(this);

            LinkLang2 = new TblLinkLang2(this);

            LinkMenuSel = new TblLinkMenuSel(this);

            LinkMenuSelection = LinkMenuSel;

            MainTable = new TblMainTable(this);

            MainTableLang2 = new TblMainTableLang2(this);

            MainTablePerson = new TblMainTablePerson(this);

            MainTableVariableHierarchy = new TblMainTableVariableHierarchy(this);

            MenuSelection = new TblMenuSelection(this);

            MenuSelectionLang2 = new TblMenuSelectionLang2(this);

            MetaAdm = new TblMetaAdm(this);

            MetabaseInfo = new TblMetabaseInfo(this);

            Organization = new TblOrganization(this);

            OrganizationLang2 = new TblOrganizationLang2(this);

            Person = new TblPerson(this);

            SpecialCharacter = new TblSpecialCharacter(this);

            SpecialCharacterLang2 = new TblSpecialCharacterLang2(this);

            SubTable = new TblSubTable(this);

            SubTableLang2 = new TblSubTableLang2(this);

            SubTableVariable = new TblSubTableVariable(this);

            TextCatalog = new TblTextCatalog(this);

            TextCatalogLang2 = new TblTextCatalogLang2(this);

            TimeScale = new TblTimeScale(this);

            TimeScaleLang2 = new TblTimeScaleLang2(this);

            Value = new TblValue(this);

            ValueLang2 = new TblValueLang2(this);

            ValueExtra = new TblValueExtra(this);

            ValueExtraLang2 = new TblValueExtraLang2(this);

            ValueGroup = new TblValueGroup(this);

            ValueGroupLang2 = new TblValueGroupLang2(this);

            ValuePool = new TblValuePool(this);

            ValuePoolLang2 = new TblValuePoolLang2(this);

            ValueSet = new TblValueSet(this);

            ValueSetLang2 = new TblValueSetLang2(this);

            ValueSetGrouping = new TblValueSetGrouping(this);

            Variable = new TblVariable(this);

            VariableLang2 = new TblVariableLang2(this);

            VSValue = new TblVSValue(this);

            VSValueLang2 = new TblVSValueLang2(this);
        }

        public SqlDbConfig_22(XmlReader xmlReader, XPathNavigator nav)
        : base(xmlReader, nav)
        {
            log.Debug("SqlDbConfig_22 called");

            this.initStructs();
            this.initCodesAndKeywords();
        }

        #region  structs

         

        /// <summary>
        /// The table contains information on the content of the data table(s).The content column's name is the same as the name of the corresponding data columns in the data table.
        /// </summary>
        public class TblContents : Tab
        {

            /// <summary>
            /// Name of the main table to which the content columns are linked. See further description in the table MainTable. 
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Name of the data columns in the data table.\nA main table's content columns must have unique names within that main table but the same column name can occur in other main tables.\nThe name should be descriptive, max 20 characters, beginning with a capital letter and should only contain letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
            /// </summary>
            public Col ContentsCol;
            /// <summary>
            /// Presentation text used by the retrieval interface when selecting table data and which, after retrieval of a data table, forms the beginning of the table heading.\nThe presentation text should be unique within a main table.\nAt statistics \n,\n For material within a statistical product with a universally established abbreviation, i.e. AKU, KPI, the short name should be included in the presentation text in brackets.\nFor material that refers only to one point in time (i.e. FoB90), the current point in time should be given in the presentation text.\nThe text should contain information on unit (if not obvious), base time, fixed/ current prices, calendar adjustment and seasonal adjustment.\nThe text should begin with a capital letter, should not contain the % symbol and should not be finished with a full-stop.
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Short presentation text for content column. Used in retrieval interface, e.g. when selecting table data, or as the column name when table is presented, if the main table has several contents.\nThe text should begin with a capital letter, should not contain the % symbol and should not end with a full-stop. \nIf there is no text, the field should be NULL.
            /// </summary>
            public Col PresTextSCol;
            /// <summary>
            /// Presentation code\nAt statistics \n,\n Consists of the main table's ProductId (i.e. code for the product) + one letter + one number or letter. The code should be unique, i.e. the same presentation code should only occur once in the meta database.
            /// </summary>
            public Col PresCodeCol;
            /// <summary>
            /// Here it should be shown whether material is included in \n's Official Statistics (SOS) or not, and which rules apply to the distribution of the statistics (copyright). All content columns within a main table must belong to the same category, i.e. the same code should be in all the fields. There are the following alternatives:\n1 = included in \n's Official Statistics (no copyright)\n2 = not included in \n's Official Statistics (no copyright)\n3 = not included in \n's Official Statistics (copyright)
            /// </summary>
            public Col CopyrightCol;
            /// <summary>
            /// Code for the authority responsible for the statistics (statistical authority).\n \nAll content columns in a main table must belong to the same statistical authority.\nData is taken from the column OrganizationCode in the table Organization. For a more detailed description, see this table.
            /// </summary>
            public Col StatAuthorityCol;
            /// <summary>
            /// At statistics \n,\n Code for the authority/organisation that is responsible for the production of the statistics.\nAll content columns in a main table must have the same producer.\nWhere Statistics Sweden produces the statistics, the official abbreviation of the unit within Statistics Sweden that is responsible for the production should be given, i.e. SCB/BV/BE.\nWhere responsibility for the production lies outside Statistics Sweden, the abbreviation for the relevant authority/organisation should be given.\nData is taken from the column OrganizationCode in the table Organization. For a more detailed description, see this table.
            /// </summary>
            public Col ProducerCol;
            /// <summary>
            /// The date of the most recent update (incl. exact time) of the main table's data tables on the production server. The date remains unchanged when copying over to the external servers. The field is updated automatically by the programs used for loading and updating of data.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Col LastUpdatedCol;
            /// <summary>
            /// The most recent date for the official release of data, i.e. the date and exact time for the transfer of the main table's data tables to the external servers. The field is updated automatically by the program used for the transfer.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Col PublishedCol;
            /// <summary>
            /// Unit, e.g. number, currency, index. The given unit should apply to both the storage and the presentation. Details on unit (if not obvious) should also be written in text to the column PresText in the table Contents.\nDetails on unit  can also be stored in the table ValueExtra, column Unit, for material with variables with long value texts. Then the unit can be different for different values. In this case, %ValueExtra is written in the field unit in the table Contents.\nThe unit can also be stored in the content column in the data table. This column is always called Unit and can contain different units for different values. In this case, %DataTable is written in the field unit in the table Contents.
            /// </summary>
            public Col UnitCol;
            /// <summary>
            /// The number of decimals that are to be presented in the table presentation when retrievals are made.
            /// </summary>
            public Col PresDecimalsCol;
            /// <summary>
            /// The data table, in principle, only stores cells with values that are not zero. Cells containing zero are only stored if another content column in the same data table has a value other than zero (0) in the corresponding cell. In this field, it should be stated how data cells that are not stored, i.e. cells for which data is missing in all content columns, should be presented. Alternatives:\nY = Yes. The cells are assumed to contain zero. Presented as zero (0).\nN = No. Data cannot be given.\nIf PresCellsZero is 'N', the field PresMissingLine should be used to indicate how these cells should be presented. See description of PresMissingLine in Contents. See also the description of the table SpecialCharacter. 
            /// </summary>
            public Col PresCellsZeroCol;
            /// <summary>
            /// (Datamod vers 2.1, not yet impl at Statistics Sweden)\nThe field is used by the retrieval programs for presenting cells that should not be presented as zero, i.e. when PresCellsZero in Contents is 'N'. The field can either be NULL or contain the identity for a special character. The identity must in that case exist in the column CharacterType in the table SpecialCharacter, and the character must exist in the column PresCharacter in this table. If PresMissingLine is NULL, DefaultCodeMissingLine in MetaAdm is used. \nSee also descriptions of: the column PresCellsZero in the table Contents,  the columns CharacterType and PresCharacter in the table SpecialCharacter and the table MetaAdm. 
            /// </summary>
            public Col PresMissingLineCol;
            /// <summary>
            /// Shows if the content can be aggregated or not. Applies to all distributed variables for the content column. There are the following alternatives:\nY = Yes\nN = No\nIf AggregPossible = N, the possibility to both aggregate and group the data in the retrieval interface is eliminated.
            /// </summary>
            public Col AggregPossibleCol;
            /// <summary>
            /// RefPeriod relates to the time of measurement for the material. Written as text, i.e."31 December of previous year". Data is obligatory for stock material, i.e. when the field StockFA in Contents is "S".\nIf the reference time is not available, the field should be NULL.
            /// </summary>
            public Col RefPeriodCol;
            /// <summary>
            /// Shows whether the statistical material is of the type stock, flow or average. There are the following alternatives:\nF = Flow. Measurement time refers to a specific period. The result describes events that occurred successively during the measurement period.\nA = Average. The result is made up of an average value of observation values at different measurement times.\nS = Stock. The measurement time refers to a specific point in time.\nX = Other
            /// </summary>
            public Col StockFACol;
            /// <summary>
            /// The base period when calculating an index or fixed prices, for example. In cases where data are not relevant, the field should be NULL.
            /// </summary>
            public Col BasePeriodCol;
            /// <summary>
            /// Current/fixed prices. Alternatives:\nF = Fixed prices.\nC = Current prices.\nIn cases where data are not relevant, the field should be NULL. 
            /// </summary>
            public Col CFPricesCol;
            /// <summary>
            /// Shows whether the statistical material is calendar adjusted or not during the measurement period. There are the following alternatives:\nY = Yes\nN = No
            /// </summary>
            public Col DayAdjCol;
            /// <summary>
            /// Shows whether the statistical material is seasonally adjusted or not, i.e. adjusted for different periodical variations during the measurement period that may have affected the result. There are the following alternatives:\nY = Yes \nN = No
            /// </summary>
            public Col SeasAdjCol;
            /// <summary>
            /// Number of stored characters in the data column. Number of characters should be:\n2, if StoreFormat = S,\n4, if StoreFormat = I,\n2-17 (decimals included, decimal point excluded), if StoreFormat = N\n1-30 characters, if StoreFormat = C.\nAlso see the description of column StoreFormat in the table Contents.
            /// </summary>
            public Col StoreNoCharCol;
            /// <summary>
            /// Number of stored decimals (0-15).\nData should be included in StoreNoChar if StoreFormat = N.
            /// </summary>
            public Col StoreDecimalsCol;
            /// <summary>
            /// Here the storage format for data cells for the content column is shown. There are the following alternatives:\nC = Varchar. For storage of special characters.\nI = Integer. For numbers of size 2 147 483 647 to - 2 147 483 648.\nN = Numeric. For larger numbers and always when the material is stored with decimals.\nS = Smallint. For numbers of size 32 767 to - 32 768.\nAlso see the description of column StoreNoChar in the table Contents.
            /// </summary>
            public Col StoreFormatCol;
            /// <summary>
            /// Here the content column's (data columns) order of storage amongst themselves in the data table is shown.
            /// </summary>
            public Col StoreColumnNoCol;

            internal TblContents(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("Contents","CNT"), config.ExtractTableName("Contents","CONTENTS"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Contents", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "Contents", "CONTENTS");
                this.ContentsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "PresTextS", "PRESTEXTS");
                this.PresTextSCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "PresCode", "PRESCODE");
                this.PresCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "Copyright", "COPYRIGHT");
                this.CopyrightCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "StatAuthority", "STATAUTHORITY");
                this.StatAuthorityCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "Producer", "PRODUCER");
                this.ProducerCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "LastUpdated", "LASTUPDATED");
                this.LastUpdatedCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "Published", "PUBLISHED");
                this.PublishedCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "Unit", "UNIT");
                this.UnitCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "PresDecimals", "PRESDECIMALS");
                this.PresDecimalsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "PresCellsZero", "PRESCELLSZERO");
                this.PresCellsZeroCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "PresMissingLine", "PRESMISSINGLINE");
                this.PresMissingLineCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "AggregPossible", "AGGREGPOSSIBLE");
                this.AggregPossibleCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "RefPeriod", "REFPERIOD");
                this.RefPeriodCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "StockFA", "STOCKFA");
                this.StockFACol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "BasePeriod", "BASEPERIOD");
                this.BasePeriodCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "CFPrices", "CFPRICES");
                this.CFPricesCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "DayAdj", "DAYADJ");
                this.DayAdjCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "SeasAdj", "SEASADJ");
                this.SeasAdjCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "StoreNoChar", "STORENOCHAR");
                this.StoreNoCharCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "StoreDecimals", "STOREDECIMALS");
                this.StoreDecimalsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "StoreFormat", "STOREFORMAT");
                this.StoreFormatCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Contents", "StoreColumnNo", "STORECOLUMNNO");
                this.StoreColumnNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblContentsLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of main table.\nSee further in the description of the table MainTable.
            /// </summary>
            public Lang2Col MainTableCol;

            /// <summary>
            /// Name of content column. \nSee further in the description of the table Contents.
            /// </summary>
            public Lang2Col ContentsCol;

            /// <summary>
            /// English presentation text for base period.\nIf there is no base period, the field should be NULL.\nFor a description of BasePeriod, see table Contents.
            /// </summary>
            public Lang2Col BasePeriodCol;

            /// <summary>
            /// English presentation text for content column.\nFor a description of PresText, see thetable Contents.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextCol;

            /// <summary>
            /// Short English presentation texts for content column.\nFor a description of PresTextS, see the table Contents.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextSCol;

            /// <summary>
            /// English presentation text for reference period.\nFor a description of RefPeriod, see the table Contents.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col RefPeriodCol;

            /// <summary>
            /// English presentation text for unit.\nFor a description of Unit, see the table Contents.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col UnitCol;

            internal TblContentsLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ContentsLang2","CN2"), config.ExtractTableName("ContentsLang2","CONTENTS_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "MainTable"  , "MAINTABLE");
                this.MainTableCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "Contents"  , "CONTENTS");
                this.ContentsCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "BasePeriod"  , "BASEPERIOD");
                this.BasePeriodCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "PresTextS"  , "PRESTEXTS");
                this.PresTextSCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "RefPeriod"  , "REFPERIOD");
                this.RefPeriodCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "Unit"  , "UNIT");
                this.UnitCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table links points in time for which data is stored, to a content column.
        /// </summary>
        public class TblContentsTime : Tab
        {

            /// <summary>
            /// The name of the main table that the point in time in the content column is linked to. See further description in the table MainTable.
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// The name of the content column that the point in time is linked to.\nSee description in the table Contents. 
            /// </summary>
            public Col ContentsCol;
            /// <summary>
            /// Code for point in time, i.e. 2003, 2003Q1, 2003M01. \nAt statistics \n,\n the year should always be written in full (accepted years are 1749 - 2050).\nRules for how codes should be constructed are available in the column StoreFormat in the table TimeScale.
            /// </summary>
            public Col TimePeriodCol;

            internal TblContentsTime(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ContentsTime","CTM"), config.ExtractTableName("ContentsTime","CONTENTSTIME"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ContentsTime", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ContentsTime", "Contents", "CONTENTS");
                this.ContentsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ContentsTime", "TimePeriod", "TIMEPERIOD");
                this.TimePeriodCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table contains information on which database the data tables for the statistical product are stored in. At Statistics Sweden, the table will contain all products including those that still do not have any tables in the database. 
        /// </summary>
        public class TblDataStorage : Tab
        {

            /// <summary>
            /// At statistics \n,\n An identifying code for a statistical product, which can be linked to the Product Database (PDB). 
            /// </summary>
            public Col ProductIdCol;
            /// <summary>
            /// Name of the server where the database is situated.
            /// </summary>
            public Col ServerNameCol;
            /// <summary>
            /// Name of the database where the product's data tables are stored.
            /// </summary>
            public Col DatabaseNameCol;

            internal TblDataStorage(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("DataStorage","DST"), config.ExtractTableName("DataStorage","DATASTORAGE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("DataStorage", "ProductId", "PRODUCTID");
                this.ProductIdCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("DataStorage", "ServerName", "SERVERNAME");
                this.ServerNameCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("DataStorage", "DatabaseName", "DATABASENAME");
                this.DatabaseNameCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table contains footnote texts and information on footnotes.  (In the future, it could also contain links to documents, publications, etc. on Statistics Sweden's website or the website of another statistical authority).
        /// </summary>
        public class TblFootnote : Tab
        {

            /// <summary>
            /// Serial number given automatically by the system. The most recently used footnote number is stored in the table MetaAdm.\nSee further in the description of the table MetaAdm.
            /// </summary>
            public Col FootnoteNoCol;
            /// <summary>
            /// Code for the type of footnote. There are the following alternatives:\n1 = footnote on subject area\n2 = footnote on content column\n3 = footnote on variable + content column\n4 = footnote on value/time + content column\n5 = footnote on variable\n6 = footnote on value\n7 = footnote on main table\n8 = footnote on sub-table\n9 = footnote on value + main table\nA = footnote on statistical area (level 2)\nB = footnote on product (level 3)\nC = footnote on table group (level 4)\nQ = footnote on grouping
            /// </summary>
            public Col FootnoteTypeCol;
            /// <summary>
            /// Contains information on when the footnote should be shown in the outdata program, i.e. when content is selected for a table, when the table is presented or both.\nThere are the following alternatives:\nB = shown upon both selection and presentation\nP = shown upon presentation\nS = shown upon selection
            /// </summary>
            public Col ShowFootnoteCol;
            /// <summary>
            /// Code for whether the footnote is classified as "voluntary" or "obligatory".\nAlternatives:\nO = optional\nM = mandatory
            /// </summary>
            public Col MandOptCol;
            /// <summary>
            /// Text in the footnote. Written as consecutive text, starting with a capital letter.\nAt statistics \n,\n MacroMeta currently allows footnote texts of a maximum of 700 characters long.\nNOT YET IMPLEMENTED.\nThe footnote texts can be edited using HTML tags to insert a new row, bold or italic text. Only the following is allowed:\n<b> Bold </b> Bold text\n<I> Italic </I> Italic text\n<BR> New row\nThe text can also contain links to documents, publications, etc. on Statistics Sweden's website or the website of another statistical authority. The link is written in HTML format. Example: \n <a href=http://www.scb.se> </a>See Statistics Sweden's website for more information!</a>\nNB! Double quotation marks should not be used as this causes problems in PC-AXIS.
            /// </summary>
            public Col FootnoteTextCol;

            internal TblFootnote(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("Footnote","FNT"), config.ExtractTableName("Footnote","FOOTNOTE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Footnote", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Footnote", "FootnoteType", "FOOTNOTETYPE");
                this.FootnoteTypeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Footnote", "ShowFootnote", "SHOWFOOTNOTE");
                this.ShowFootnoteCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Footnote", "MandOpt", "MANDOPT");
                this.MandOptCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Footnote", "FootnoteText", "FOOTNOTETEXT");
                this.FootnoteTextCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblFootnoteLang2 : Lang2Tab
        {

            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Lang2Col FootnoteNoCol;

            /// <summary>
            /// English footnote text.
            /// </summary>
            public Lang2Col FootnoteTextCol;

            internal TblFootnoteLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteLang2","FN2"), config.ExtractTableName("FootnoteLang2","FOOTNOTE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteLang2", "FootnoteNo"  , "FOOTNOTENO");
                this.FootnoteNoCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("FootnoteLang2", "FootnoteText"  , "FOOTNOTETEXT");
                this.FootnoteTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table links footnotes to a point in time for a specific content column.
        /// </summary>
        public class TblFootnoteContTime : Tab
        {

            /// <summary>
            /// Name of main table.\nSee further in the description of the table MainTable.
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Name of content column.\nSee further in the description of the table Contents.
            /// </summary>
            public Col ContentsCol;
            /// <summary>
            /// Point in time that the footnote relates to.\nSee descriptions in table TimeScale and ContentsTime.
            /// </summary>
            public Col TimePeriodCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;
            /// <summary>
            /// State whether the footnote is a cell footnote. A cell footnote is defined as a footnote that exists for at least two of the content column's variables, e.g. Region and Time or Region and Sex.\nAlternatives:\nY = Yes, the footnote is a cell footnote\nN = No\nAt statistics \n,\n Not yet implemented. 
            /// </summary>
            public Col CellnoteCol;

            internal TblFootnoteContTime(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteContTime","FCT"), config.ExtractTableName("FootnoteContTime","FOOTNOTECONTTIME"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "Contents", "CONTENTS");
                this.ContentsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "TimePeriod", "TIMEPERIOD");
                this.TimePeriodCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "Cellnote", "CELLNOTE");
                this.CellnoteCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to a value for a specific content column.
        /// </summary>
        public class TblFootnoteContValue : Tab
        {

            /// <summary>
            /// Name of main table. \nSee further in the description of the table MainTable. 
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Name of content column. \nSee further in the description of the table Contents. 
            /// </summary>
            public Col ContentsCol;
            /// <summary>
            /// Name of variable.\nSee further in the description of the table Variable.
            /// </summary>
            public Col VariableCol;
            /// <summary>
            /// Name of value pool.\nSee further in the description of the table ValuePool.
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// Code for the value that the footnote relates to.\nSee further in the description of the table Value.
            /// </summary>
            public Col ValueCodeCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;
            /// <summary>
            /// State whether the footnote is a cell footnote. A cell footnote is defined as a footnote that exists for at least two of the content column's variables, e.g. Region and Time or Region and Sex. \nAlternatives:\nY = Yes, the footnote is a cell footnote\nN = No\nAt statistics \n,\n Not yet implemented.
            /// </summary>
            public Col CellnoteCol;

            internal TblFootnoteContValue(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteContValue","FCA"), config.ExtractTableName("FootnoteContValue","FOOTNOTECONTVALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "Contents", "CONTENTS");
                this.ContentsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "Variable", "VARIABLE");
                this.VariableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "Cellnote", "CELLNOTE");
                this.CellnoteCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to a variable for a specific content column.
        /// </summary>
        public class TblFootnoteContVbl : Tab
        {

            /// <summary>
            /// Name of main table. \nSee further in the description of the table MainTable. 
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Name of content column. \nSee further in the description of the table Contents. 
            /// </summary>
            public Col ContentsCol;
            /// <summary>
            /// Name of variable. \nSee further in the description of the table Variable.
            /// </summary>
            public Col VariableCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;

            internal TblFootnoteContVbl(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteContVbl","FCB"), config.ExtractTableName("FootnoteContVbl","FOOTNOTECONTVBL"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteContVbl", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContVbl", "Contents", "CONTENTS");
                this.ContentsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContVbl", "Variable", "VARIABLE");
                this.VariableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContVbl", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to a content column.
        /// </summary>
        public class TblFootnoteContents : Tab
        {

            /// <summary>
            /// Name of main table.\nSee further in the description of the table MainTable.
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Name of content column.\nSee further in the description of the table Contents.
            /// </summary>
            public Col ContentsCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;

            internal TblFootnoteContents(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteContents","FCO"), config.ExtractTableName("FootnoteContents","FOOTNOTECONTENTS"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteContents", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContents", "Contents", "CONTENTS");
                this.ContentsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteContents", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to a grouping. \nAt statistics \n,\n Not yet implemented. 
        /// </summary>
        public class TblFootnoteGrouping : Tab
        {

            /// <summary>
            /// Namn på den variabel, som fotnoten avser.\nSe beskrivning av tabellen Variabel.
            /// </summary>
            public Col GroupingCol;
            /// <summary>
            /// Fotnotens nummer.\nSe beskrivning av tabellen Fotnot. 
            /// </summary>
            public Col FootnoteNoCol;

            internal TblFootnoteGrouping(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteGrouping","FCO"), config.ExtractTableName("FootnoteGrouping","FOOTNOTEGROUPING"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteGrouping", "Grouping", "GROUPING");
                this.GroupingCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteGrouping", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to a main table. \nAt statistics \n,\n Not yet implemented. 
        /// </summary>
        public class TblFootnoteMainTable : Tab
        {

            /// <summary>
            /// Name of main table.\nSee further in the description of the table MainTable. 
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;

            internal TblFootnoteMainTable(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteMainTable","FMT"), config.ExtractTableName("FootnoteMainTable","FOOTNOTEMAINTABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteMainTable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteMainTable", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to a value for a specific content column.
        /// </summary>
        public class TblFootnoteMaintValue : Tab
        {

            /// <summary>
            /// Name of main table. \nSee further in the description of the table MainTable. 
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Name of variable.\nSee further in the description of the table Variable.
            /// </summary>
            public Col VariableCol;
            /// <summary>
            /// Name of value pool.\nSee further in the description of the table ValuePool.
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// Code for the value that the footnote relates to.\nSee further in the description of the table Value.
            /// </summary>
            public Col ValueCodeCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;

            internal TblFootnoteMaintValue(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteMaintValue","FMV"), config.ExtractTableName("FootnoteMaintValue","FOOTNOTEMAINTVALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "Variable", "VARIABLE");
                this.VariableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to all levels above the MainTable.
        /// </summary>
        public class TblFootnoteMenuSel : Tab
        {

            /// <summary>
            /// Code for relevant menu level.\nSee further in the description of the table MenuSelection. 
            /// </summary>
            public Col MenuCol;
            /// <summary>
            /// Code for the nearest underlying eligible alternative in the relevant menu level.
            /// </summary>
            public Col SelectionCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;

            internal TblFootnoteMenuSel(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteMenuSel","FMS"), config.ExtractTableName("FootnoteMenuSel","FOOTNOTEMENUSEL"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteMenuSel", "Menu", "MENU");
                this.MenuCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteMenuSel", "Selection", "SELECTION");
                this.SelectionCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteMenuSel", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to a main table. \nAt statistics \n,\n Not yet implemented.
        /// </summary>
        public class TblFootnoteSubTable : Tab
        {

            /// <summary>
            /// Name of main table.\nSee further in the description of the table MainTable. 
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Name of sub-table \nSee further in the description of the table SubTable.
            /// </summary>
            public Col SubTableCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;

            internal TblFootnoteSubTable(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteSubTable","FST"), config.ExtractTableName("FootnoteSubTable","FOOTNOTESUBTABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteSubTable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteSubTable", "SubTable", "SUBTABLE");
                this.SubTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteSubTable", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to a value.
        /// </summary>
        public class TblFootnoteValue : Tab
        {

            /// <summary>
            /// Name of value pool.\nSee further in the description of the table ValuePool.
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// Code for the value that the footnote relates to.\nSee further in the description of the table Value.
            /// </summary>
            public Col ValueCodeCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;

            internal TblFootnoteValue(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteValue","FVL"), config.ExtractTableName("FootnoteValue","FOOTNOTEVALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteValue", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteValue", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteValue", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links footnotes to a variable.
        /// </summary>
        public class TblFootnoteVariable : Tab
        {

            /// <summary>
            /// Name of the variable that the footnote relates to.\nSee further in the description of the table Variable. 
            /// </summary>
            public Col VariableCol;
            /// <summary>
            /// Number of the footnote.\nSee further in the description of the table Footnote. 
            /// </summary>
            public Col FootnoteNoCol;

            internal TblFootnoteVariable(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("FootnoteVariable","FVB"), config.ExtractTableName("FootnoteVariable","FOOTNOTEVARIABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteVariable", "Variable", "VARIABLE");
                this.VariableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("FootnoteVariable", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table describes the groupings which exist in the macro database. Used for the grouping of values for presentation purposes.
        /// </summary>
        public class TblGrouping : Tab
        {

            /// <summary>
            /// Name of grouping.\nThe name should consist of the name of the value pool that the grouping is linked to + a suffix. The suffix should always be used, even if there is only one grouping for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.\nThe name is written beginning with a capital letter.
            /// </summary>
            public Col GroupingCol;
            /// <summary>
            /// Name of the value pool that the grouping belongs to.\nSee further in the description of the table Organization.
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// 
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Shows if the grouping is hierarchic or not. Can be:\nN = No\nB = Balanced\nU = Unbalanced\nFor non hierarchic groupings Hierarchy should always be N. \nIn a balanced hierarchy all branches are the same length, i.e. with the same number of levels. In an unbalanced hierarchy the number of levels and the length of the levels can vary within the hierarchy. \nAt statistics \n,\n Not yet implemented.
            /// </summary>
            public Col HierarchyCol;
            /// <summary>
            /// Sorting code to enable the presentation of the groupings within a value pool in a logical order.\nIf there is no sorting code, the field should be NULL.\nAt statistics \n,\n Not yet implemented.
            /// </summary>
            public Col SortCodeCol;
            /// <summary>
            /// Code which indicates how a grouping should be presented, as an aggregated value, integral value or both. There are the following alternatives:\nA = aggregated value should be shown\nI = integral value should be shown\nB = both aggregated and integral values should be shown\nAt statistics \n,\n Not yet implemented.
            /// </summary>
            public Col GroupPresCol;
            /// <summary>
            /// Description of grouping. Should give an idea of how the grouping has been put together.\nWritten beginning with a capital letter.\nIf a description is not available, the field should be NULL.
            /// </summary>
            public Col DescriptionCol;
            /// <summary>
            /// At statistics \n,\n The field is currently not used. For the present, should be NULL.\nIt is planned that this will contain the name of the corresponding classification in the Classification Database (KDB).
            /// </summary>
            public Col KDBidCol;

            internal TblGrouping(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("Grouping","GRP"), config.ExtractTableName("Grouping","GROUPING"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Grouping", "Grouping", "GROUPING");
                this.GroupingCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Grouping", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Grouping", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Grouping", "Hierarchy", "HIERARCHY");
                this.HierarchyCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Grouping", "SortCode", "SORTCODE");
                this.SortCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Grouping", "GroupPres", "GROUPPRES");
                this.GroupPresCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Grouping", "Description", "DESCRIPTION");
                this.DescriptionCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Grouping", "KDBid", "KDBID");
                this.KDBidCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblGroupingLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of grouping.\nSee further in the description of the table Grouping.
            /// </summary>
            public Lang2Col GroupingCol;

            /// <summary>
            /// Name of value pool.\nSee further in the description of the table ValuePool.
            /// </summary>
            public Lang2Col ValuePoolCol;

            /// <summary>
            /// English presentation text for the grouping.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextCol;

            /// <summary>
            /// Sorting code to enable presentation of groupings within a value pool in a logical order by the English names.\nIf there is no text, the field should be NULL.\nAt statistics \n,\n Not yet implemented.
            /// </summary>
            public Lang2Col SortCodeCol;

            internal TblGroupingLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("GroupingLang2","GR2"), config.ExtractTableName("GroupingLang2","GROUPING_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("GroupingLang2", "Grouping"  , "GROUPING");
                this.GroupingCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("GroupingLang2", "ValuePool"  , "VALUEPOOL");
                this.ValuePoolCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("GroupingLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("GroupingLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table describes the levels within a grouping. \nThe table has to exist for both hierarcical and non hierarcical groupings, but does not have to be used for the non hierarcical. 
        /// </summary>
        public class TblGroupingLevel : Tab
        {

            /// <summary>
            /// Variabelns namn.\nSe beskrivning av tabellen Variabel.
            /// </summary>
            public Col GroupingCol;
            /// <summary>
            /// Number for sorting a level within a grouping. The highest level should always be 1. 
            /// </summary>
            public Col LevelCol;
            /// <summary>
            /// The name of the level.
            /// </summary>
            public Col LevelTextCol;
            /// <summary>
            /// Should contain the identification of a map that is suitable for the variable and the grouping. The field must be filled in if the column VariableType in the table SubTableVariable = G, otherwise the field is NULL. \nThe identification number should also be included in the table TextCatalog. For further information see description of TextCatalog. 
            /// </summary>
            public Col GeoAreaNoCol;

            internal TblGroupingLevel(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("GroupingLevel","GRP"), config.ExtractTableName("GroupingLevel","GROUPINGLEVEL"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("GroupingLevel", "Grouping", "GROUPING");
                this.GroupingCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("GroupingLevel", "Level", "LEVEL");
                this.LevelCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("GroupingLevel", "LevelText", "LEVELTEXT");
                this.LevelTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("GroupingLevel", "GeoAreaNo", "GEOAREANO");
                this.GeoAreaNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblGroupingLevelLang2 : Lang2Tab
        {

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col GroupingCol;

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col LevelCol;

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col LevelTextCol;

            internal TblGroupingLevelLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("GroupingLevelLang2","GR2"), config.ExtractTableName("GroupingLevelLang2","GROUPINGLEVEL_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("GroupingLevelLang2", "Grouping"  , "GROUPING");
                this.GroupingCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("GroupingLevelLang2", "Level"  , "LEVEL");
                this.LevelCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("GroupingLevelLang2", "LevelText"  , "LEVELTEXT");
                this.LevelTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table contains one or several links to menu levels in the table MenuSelection. 
        /// </summary>
        public class TblLink : Tab
        {

            /// <summary>
            /// Identifying code for the link.
            /// </summary>
            public Col LinkIdCol;
            /// <summary>
            /// Link written in HTML.
            /// </summary>
            public Col LinkCol;
            /// <summary>
            /// Describes the type of link. There are the following alternatives:\nTableF = table ahead in the same database \nTableB = previous table in the same database \nTableRel = related table in the same database\nTableProc = table for percentage calculation\nDok = dokumentation (internal or external) \nTableRelEx = related table in other countries\nWebsite = related web site\nTemasite = theme web site\nAnalys = analysis
            /// </summary>
            public Col LinkTypeCol;
            /// <summary>
            /// U = URL\nM = Maintable 
            /// </summary>
            public Col LinkFormatCol;
            /// <summary>
            /// Presentation text for the link, i.e. the text that is visible for the user in the Internet interface.
            /// </summary>
            public Col LinkTextCol;
            /// <summary>
            /// Presentation category for the link. There are three alternatives:\nO = Public, i.e. link accessible for all users\nI = Internal, i.e. table is only accessible for internal Statistics Sweden users.\nP = Private, i.e. table is only accessible for certain internal users.\nThe code is always written with capital letters.
            /// </summary>
            public Col PresCategoryCol;
            /// <summary>
            /// Shows how the link should be presented in the Internet interface. There are three alternatives:\nD = Presented dictately\nI = Presented under an icon or similar\nB = Presented both dictately and under an icon
            /// </summary>
            public Col LinkPresCol;
            /// <summary>
            /// Sorting code for the link. Makes it possible to dictate the order in which the links are presented for users in the Internet interface.\nIf there is no sorting code, the field should be NULL.
            /// </summary>
            public Col SortCodeCol;
            /// <summary>
            /// Description of link.\nIf a description is not available, the field should be NULL.
            /// </summary>
            public Col DescriptionCol;

            internal TblLink(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("Link","LNK"), config.ExtractTableName("Link","LINK"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Link", "LinkId", "LINKID");
                this.LinkIdCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Link", "Link", "LINK");
                this.LinkCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Link", "LinkType", "LINKTYPE");
                this.LinkTypeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Link", "LinkFormat", "LINKFORMAT");
                this.LinkFormatCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Link", "LinkText", "LINKTEXT");
                this.LinkTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Link", "PresCategory", "PRESCATEGORY");
                this.PresCategoryCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Link", "LinkPres", "LINKPRES");
                this.LinkPresCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Link", "SortCode", "SORTCODE");
                this.SortCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Link", "Description", "DESCRIPTION");
                this.DescriptionCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblLinkLang2 : Lang2Tab
        {

            /// <summary>
            /// Identifying code for the link.
            /// </summary>
            public Lang2Col LinkIdCol;

            /// <summary>
            /// Link written in HTML.
            /// </summary>
            public Lang2Col LinkCol;

            /// <summary>
            /// The link's English presentation text, i.e. the text that is visible for users in the English version of the Internet interface.
            /// </summary>
            public Lang2Col LinkTextCol;

            /// <summary>
            /// Sorting code for the link. Makes it possible to dictate the order in which the links are presented for users in the retrieval interface.\nIf there is no sorting code, the field should be NULL.
            /// </summary>
            public Lang2Col SortCodeCol;

            /// <summary>
            /// Description of link.\nIf a description is not available, the field should be NULL.
            /// </summary>
            public Lang2Col DescriptionCol;

            internal TblLinkLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("LinkLang2","LN2"), config.ExtractTableName("LinkLang2","LINK_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("LinkLang2", "LinkId"  , "LINKID");
                this.LinkIdCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("LinkLang2", "Link"  , "LINK");
                this.LinkCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("LinkLang2", "LinkText"  , "LINKTEXT");
                this.LinkTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("LinkLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("LinkLang2", "Description"  , "DESCRIPTION");
                this.DescriptionCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public class TblLinkMenuSel : Tab
        {

            /// <summary>
            /// 
            /// </summary>
            public Col MenuCol;
            /// <summary>
            /// 
            /// </summary>
            public Col SelectionCol;
            /// <summary>
            /// 
            /// </summary>
            public Col LinkIdCol;

            internal TblLinkMenuSel(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("LinkMenuSel","LMS"), config.ExtractTableName("LinkMenuSel","LINKMENUSEL"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("LinkMenuSel", "Menu", "MENU");
                this.MenuCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("LinkMenuSel", "Selection", "SELECTION");
                this.SelectionCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("LinkMenuSel", "LinkId", "LINKID");
                this.LinkIdCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// This table is the central compilation point for material and contains general information for the data tables that are linked to the table.
        /// </summary>
        public class TblMainTable : Tab
        {

            /// <summary>
            /// Summarised names of the statistical material and its underlying sub-tables with stored data.\nThe name should be descriptive, max 20 characters, beginning with a capital letter and contain only letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Code for the table's status, giving information on where the table fits into the production and publishing processes. Table status decides whether a table can be seen or not in the indata and outdata programs.\nThere are the following alternatives:\nM = Only metadata input\nE = The table is new and empty data tables have been created\nU = The table is currently being updated and retrievals cannot be done\nN = The table is loaded with new, but not yet officially released data, only accessible for product staff\nO = The table is ready for official release, only accessible for product staff, locked for updating\nA = The table is accessible to all (with authorisation according to PresCategory) 
            /// </summary>
            public Col TableStatusCol;
            /// <summary>
            /// The descriptive text which is presented when selecting a table in the retrieval interface.\nAt statistics \n,\n If an established abbreviation for the statistical product exists, i.e. AKU or KPI, this should be included in the presentation text.\nThe text should be unique and contain information on all dividing variables, including time scale (at the end). Information on unit, base time, fixed/current prices, calendar-adjusted or seasonally adjusted should normally NOT be included, as this is given in the presentation text for Contents. There can however be exceptions, i.e. for main tables that contain the same information except that the calculations are based on different base times.\nThe text should begin with a capital letter, should not end with a full stop, and should not include the % symbol. It should not finish with a number that relates to something other than the point of time.
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Presentation text in short. Only needs to be filled in if the main table has several content columns. The text comes from the main table's PresText but should not contain information on variable or time.\nUsed in the retrieval interface as an introduction to the table heading, where several contents are chosen. Also used as content information in a PC-AXIS file, in some cases several contents are chosen at the same time and put into the same file.\nShould begin with a capital letter and not end with a full-stop. The text should not include a % symbol.\nIf a short presentation text is not available, the field should be NULL.
            /// </summary>
            public Col PresTextSCol;
            /// <summary>
            /// Can be used for main tables with several contents to specify a collective name for the content columns. The content variable is used as the general name ("type") which is in the TextCatalog. Used in the retrieval interface in the heading when table is presented, "...by industry, time and [content variable]"\nIf the field is not used, it should be NULL.\nAt statistics \n,\n Not yet implemented.
            /// </summary>
            public Col ContentsVariableCol;
            /// <summary>
            /// Unique identity for main table. Can be used in requests from the end users, etc.\nAt statistics \n,\n TabellId is not yet implemented but it is planned later to be created automatically when a new main table is set up according to the formula one letter + 3 digits. Is for now empty, i.e. filled in with a dash.
            /// </summary>
            public Col TableIdCol;
            /// <summary>
            /// Presentation category for all sub-tables and content columns in the table. There are three alternatives:\nO = Official, i.e. tables that are officially released and accessible to all users on the external servers or are available on the production server, with plans for official release. (Only tables with PresCategory = O are on the external servers)\nI = Internal, i.e. tables are only accessible for internal users at Statistics Sweden and are only available on the production server. The table should never be published on the Internet.\nP - Private, i.e. tables are only accessible for certain internal users and are only available on the production server. The tables should never be published on the Internet.\nThe code is also written with capital letters.
            /// </summary>
            public Col PresCategoryCol;
            /// <summary>
            /// Specifies if a column for special symbols exists in the data table (s) and if it should be used at retrieval.\nThere are the following alternatives:\nY = Yes, column for special symbols exists in the database, wich should be used at retrieval\nE = Yes, column for special symbols exists in the database, but is not used at retrieval\nN = No, column for special symbols does not exist in the database\nIf SpecCharExists =Y, there is an extra content column for all content columns in the data table. These special columns have the same names as the content column they belong to, with the suffix X. The format is varchar(2), i.e. the same as CharacterType in the table SpecialCharacter.\nAt statistics \n,\n Not yet implemented. 
            /// </summary>
            public Col SpecCharExistsCol;
            /// <summary>
            /// Code for subject area, i.e. BE
            /// </summary>
            public Col SubjectCodeCol;
            /// <summary>
            /// At statistics \n,\n An identifying code for a statistical product, which can be linked to the Product Database (PDB). ProductId is obligatory at Statistics Sweden, otherwise it is voluntary.\nConsists of two capital letters + 4 numbers.
            /// </summary>
            public Col ProductIdCol;
            /// <summary>
            /// Name on the time scale that is used in the material. See further description of the table TimeScale.
            /// </summary>
            public Col TimeScaleCol;

            internal TblMainTable(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("MainTable","MTA"), config.ExtractTableName("MainTable","MAINTABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MainTable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "TableStatus", "TABLESTATUS");
                this.TableStatusCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "PresTextS", "PRESTEXTS");
                this.PresTextSCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "ContentsVariable", "CONTENTSVARIABLE");
                this.ContentsVariableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "TableId", "TABLEID");
                this.TableIdCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "PresCategory", "PRESCATEGORY");
                this.PresCategoryCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "SpecCharExists", "SPECCHAREXISTS");
                this.SpecCharExistsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "SubjectCode", "SUBJECTCODE");
                this.SubjectCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "ProductId", "PRODUCTID");
                this.ProductIdCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTable", "TimeScale", "TIMESCALE");
                this.TimeScaleCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblMainTableLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of main table. \nSee further in the description of the table MainTable. 
            /// </summary>
            public Lang2Col MainTableCol;

            /// <summary>
            /// Code which shows whether all the table's presentation texts are translated to English or not. This column is necessary so that it is possible to determine from the retrieval interface whether the table will be shown in English or not. There are the following alternatives:\nN = the table is not completely translated to English\nT = the table is completely translated to English\nCode T is given to main tables for which all the English tables’ presentation texts are filled in.
            /// </summary>
            public Lang2Col StatusCol;

            /// <summary>
            /// Code which shows if the table's English texts are copied to the external servers or not. The column is used by the indata program. There are the following alternatives:\nY = the table's texts are officially released in English, i.e. copied to the external servers\nN = the table's texts are not officially released in English.
            /// </summary>
            public Lang2Col PublishedCol;

            /// <summary>
            /// English presentation text for the main table.\nFor a description of PresText, see table MainTable.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextCol;

            /// <summary>
            /// Short English presentation text for the main table.\nFor a description of PresTextS, see table MainTable.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextSCol;

            /// <summary>
            /// Can be used for main tables with several contents to specify a collective name for the content columns. See further in the description of the table MainTable. \nIf the field is not used, it should be NULL.\nAt statistics \n,\n Not yet implemented.
            /// </summary>
            public Lang2Col ContentsVariableCol;

            internal TblMainTableLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("MainTableLang2","MT2"), config.ExtractTableName("MainTableLang2","MAINTABLE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "MainTable"  , "MAINTABLE");
                this.MainTableCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "Status"  , "STATUS");
                this.StatusCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "Published"  , "PUBLISHED");
                this.PublishedCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "PresTextS"  , "PRESTEXTS");
                this.PresTextSCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "ContentsVariable"  , "CONTENTSVARIABLE");
                this.ContentsVariableCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table links the person responsible, i.e. the contact person and person responsible for updating, to the main tables. An unlimited number of persons can be linked to a main table.
        /// </summary>
        public class TblMainTablePerson : Tab
        {

            /// <summary>
            /// Name of main table.\nSee further in the description of the table MainTable. 
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Code for the person responsible, contact person or person responsible for updating for the main table.\nFor description, see the table Person. 
            /// </summary>
            public Col PersonCodeCol;
            /// <summary>
            /// Code that shows the role of responsible person, contact person and/or person responsible for updating. There are the following alternatives:\nM = Main contact person (one person)\nC = Contact person (0 – several persons)\nU = Person responsible for updating (1 – several persons)\nI = Person responsible for international reporting (0 - 1 person). \nAt statistics \n,\n This code is not in use\nV = Person that verifies not yet published data  (0 – several persons)\nAt statistics \n,\n The maximum number of contact persons allowed at present is two and two persons responsible for updating. 
            /// </summary>
            public Col RolePersonCol;

            internal TblMainTablePerson(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("MainTablePerson","MTP"), config.ExtractTableName("MainTablePerson","MAINTABLEPERSON"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MainTablePerson", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTablePerson", "PersonCode", "PERSONCODE");
                this.PersonCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTablePerson", "RolePerson", "ROLEPERSON");
                this.RolePersonCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table links a grouping to a main table. 
        /// </summary>
        public class TblMainTableVariableHierarchy : Tab
        {

            /// <summary>
            /// Name of main table. \nSee further in the description of the table MainTable. 
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Name of variable. \nSee further in the description of the table Variable. 
            /// </summary>
            public Col VariableCol;
            /// <summary>
            /// Name of grouping. \nSee further in the description of the table Grouping. 
            /// </summary>
            public Col GroupingCol;
            /// <summary>
            /// The number of open levels that will be shown at menu selection the first time the tree is displayed. Must be 0 if all levels shall be shown.
            /// </summary>
            public Col ShowLevelsCol;
            /// <summary>
            /// Shows if all levels shall be stored or not. Can be:\nY = Yes\nN = No
            /// </summary>
            public Col AllLevelsStoredCol;

            internal TblMainTableVariableHierarchy(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("MainTableVariableHierarchy","MTP"), config.ExtractTableName("MainTableVariableHierarchy","MAINTABLEVARIABLEHIERARCHY"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "Variable", "VARIABLE");
                this.VariableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "Grouping", "GROUPING");
                this.GroupingCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "ShowLevels", "SHOWLEVELS");
                this.ShowLevelsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "AllLevelsStored", "ALLLEVELSSTORED");
                this.AllLevelsStoredCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table is used to enable the presentation of any number of eligible levels above the table MainTable. The table acts as the entry point to the databases. \nAt statistics \n,\n There are the subject area, statistics area (are planned to be introduced at a later stage), product, table group and main table. All records in MenuSelection should also be in MenuSelection_Eng.
        /// </summary>
        public class TblMenuSelection : Tab
        {

            /// <summary>
            /// Code for relevant menu level. If LevelNo = 1, Menu should be filled with START. Code for subject areas may not exceed 20 characters. \nAt statistics \n,\n \nExample of menu codes for subject areas: AM, BO.\nExample for menu codes for products: AM0401, BO0101.\nExample for menu codes for table groups: AM0401A, AM0401B.
            /// </summary>
            public Col MenuCol;
            /// <summary>
            /// The code for the nearest underlying eligible alternative in the relevant menu level. A menu can contain objects from different levels. Code for subject areas may not exceed 20 characters. 
            /// </summary>
            public Col SelectionCol;
            /// <summary>
            /// Presentation text for MenuSelection.\nAt statistics \n,\n should be filled in for all levels apart from the lowest level, main table, where PresText should be NULL.
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Short presentation text for MenuSelection.\nIf a short presentation text is not available, the field should be NULL.
            /// </summary>
            public Col PresTextSCol;
            /// <summary>
            /// Descriptive text for MenuSelection.\nIf a description is not available, the field should be NULL.
            /// </summary>
            public Col DescriptionCol;
            /// <summary>
            /// Number of menu level, where 1 refers to the highest level.\nA type of object should always have the same LevelNo.\nAt statistics \n,\n \n1 = Subject area\n3 = Product\n4 = Table group\n5 = Main table\nThe highest level number should be given in the table MetaAdm (see description in this table).
            /// </summary>
            public Col LevelNoCol;
            /// <summary>
            /// Sorting code to dictate the presentation order for the eligible alternatives on each level.\nIf there is no sorting code, the field should be NULL.
            /// </summary>
            public Col SortCodeCol;
            /// <summary>
            /// Shows how a menu alternative can be used. Alternatives: \nA = Active, visible and can be selected\nP = Passive, is visible but cannot be selected\nN = Not shown in the menu  
            /// </summary>
            public Col PresentationCol;
            /// <summary>
            /// At statistics \n,\nIdentifying code for the Product database (PDB). Should be filled in for:\n- Subject areas, i.e. when LevelNo is 1. InternalId should then be three digits. The code is called AmnesomradeId in PDB.\n- Products, i.e. when LevelNo is 3. InternalId should then be four digits. The code is called ProduktId in PDB. \nFor other levels the field should be NULL. 
            /// </summary>
            public Col InternalIdCol;

            internal TblMenuSelection(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("MenuSelection","MSL"), config.ExtractTableName("MenuSelection","MENUSELECTION"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MenuSelection", "Menu", "MENU");
                this.MenuCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MenuSelection", "Selection", "SELECTION");
                this.SelectionCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MenuSelection", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MenuSelection", "PresTextS", "PRESTEXTS");
                this.PresTextSCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MenuSelection", "Description", "DESCRIPTION");
                this.DescriptionCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MenuSelection", "LevelNo", "LEVELNO");
                this.LevelNoCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MenuSelection", "SortCode", "SORTCODE");
                this.SortCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MenuSelection", "Presentation", "PRESENTATION");
                this.PresentationCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MenuSelection", "InternalId", "INTERNALID");
                this.InternalIdCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblMenuSelectionLang2 : Lang2Tab
        {

            /// <summary>
            /// Code for relevant menu level. \nSee further in the description of the table MenuSelection. 
            /// </summary>
            public Lang2Col MenuCol;

            /// <summary>
            /// Code for the nearest underlying eligible alternative in the relevant menu level. \nSee further in the description of the table MenuSelection. 
            /// </summary>
            public Lang2Col SelectionCol;

            /// <summary>
            /// English presentation text for MenuSelection.\nSee further in the description of the table MenuSelection.
            /// </summary>
            public Lang2Col PresTextCol;

            /// <summary>
            /// Short English presentation text for MenuSelection.\nIf a short presentation text is not available, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextSCol;

            /// <summary>
            /// English descriptive text for MenuSelection.\nIf a description is not available, the field should be NULL.
            /// </summary>
            public Lang2Col DescriptionCol;

            /// <summary>
            /// Sorting code to dictate the presentation order for the eligible alternatives on every level.\nIf there is no sorting code, the field should be NULL.
            /// </summary>
            public Lang2Col SortCodeCol;

            /// <summary>
            /// Shows how a menu alternative can be used. Alternatives:\nA = Active, visible and can be selected\nP = Passive, is visible but cannot be selected\nN = Not shown in the menu  \nAt statistics \n,\n If related main tables are not translated into English, i.e. if Status in MainTable_Eng = ’N’, Presentation in MenuSelection_Eng should be ’N’ för both product and table group. 
            /// </summary>
            public Lang2Col PresentationCol;

            internal TblMenuSelectionLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("MenuSelectionLang2","MS2"), config.ExtractTableName("MenuSelectionLang2","MENUSELECTION_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "Menu"  , "MENU");
                this.MenuCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "Selection"  , "SELECTION");
                this.SelectionCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "PresTextS"  , "PRESTEXTS");
                this.PresTextSCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "Description"  , "DESCRIPTION");
                this.DescriptionCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "Presentation"  , "PRESENTATION");
                this.PresentationCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table contains system variables and their values.
        /// </summary>
        public class TblMetaAdm : Tab
        {

            /// <summary>
            /// At statistics \n,\n Name of system variable. There are the following alternatives:\n- LastFootnoteNo\n- MenuLevels\n- SpecCharSum\n- NoOfLanguage \n- Language1, Language2 osv.  \n- Codepage1\n- DataNotAvailable \n- DataNoteSum \n- DataSymbolSum \n- DataSymbolNil \n- PxDataFormat \n- KeysUpperLimit \n- DefaultCodeMissingLine \nNoOfLanguage indicates the number of languages that exists in the model for the meta database presentation texts, and that can be used by the retrieval interfaces. For each language there should be a line like:  Language1, Language2 and so on. Language1 should always include the main language. See also the description of the column Value in this table.\nFor each language there should also be a row in the table TextCatalog. For further information, see this table. \nRegarding DefaultCodeMissingLine see also descriptions in: PresCellsZero och PresMissingLine in Contents, and CharacterType and PresCharacter in SpecialCharacter.
            /// </summary>
            public Col PropertyCol;
            /// <summary>
            /// Value of system variable. Contains one value per property.\nFor the property LastFootnoteNo, Value should contain the last used footnote number in the table Footnote.\nFor the property MenuLevels, Value should contains the highest used level number in the table MenuSelection.\nFor the property SpecCharSum, Value should contain the highest acceptable value for character type in the table SpecCharacter.\nFor the property NoOfLanguage Value should contain the number of languages that exists in the metadata model. \nFor the property Language1 Value should contain the main language of the model. The code is written in three capital letters, i.e. SVE.  \nFor the property  Language2 , Language3 etc., Value should contain the other languages of the model. The code is written in three capital letters, i.e. ENG, ESP. The code is used as a suffix in the extra tables that should exist in the meta database, i.e. SubTable_ENG, SubTable_ESP. \nFor the property Codepage1: The characters that can be used and  how they should be presented. Is used at creating the keyword Codepage in the px file and at converting to XML. \nThree different examples: iso-8859-1, windows-1251, big5.\nFor the property DataNotAvailable: The value that should be presented, if  the data cell contains NULL and NPM-character is missing. If the value exists in the table SpecialCharacter, it is used, otherwise the character in the table DataNotAvailable is used. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nExample: .. (two dots).  \nFor the property DataNoteSum: The value that should be presented after the sum, if data cells with different NPM marking is summarized. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nExample:  * \n1A + 2B = 3* \nFor the property DataSymbolSum: The value that should be presented if data cells with different NPM character are summarized and no sum can be created. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nExample: N.A. \n. + .. = N.A.\nFor the property DataSymbolNil: the value that should be presented at absolute 0 (zero) in the table SpecialCharacter. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nExample: -\nFor the property PxDataFormat: Matrix = all retreivals should be stored in matrix format.\nKeysnn = retreivals with keys are remade \nnn > read data cells *100 / presented number of data cells\nExample: 40\n(Default is Matrix.)\nFor the property KeysUpperLimit: Maximum number of data cells that the presented matrix may contain, if the retreival should be possible to do with Keys. If greater, the retreival is made in matrix format.\nExample: 1000000\nFor the property DefaultCodeMissingLine: The value that should be presented in data cells that are not stored. Is used if neither presentation with 0 or special character have been specified. \nThe value is a reference to CharacterType in the SpecialCharacter table.\nSee also the description of the column Property in this table, and also the table TextCatalog.
            /// </summary>
            public Col ValueCol;
            /// <summary>
            /// Description of the property for internal use
            /// </summary>
            public Col DescriptionCol;

            internal TblMetaAdm(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("MetaAdm","MAD"), config.ExtractTableName("MetaAdm","METAADM"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MetaAdm", "Property", "PROPERTY");
                this.PropertyCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MetaAdm", "Value", "VALUE");
                this.ValueCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MetaAdm", "Description", "DESCRIPTION");
                this.DescriptionCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table contains information on the relevant data model, macro/micro, version and the database role (see further description of respective columns).\nUsed by the indata program, which via its .ini file carries out a check at the beginning that the program is run against the right version of the metadata model. (By right version, it is meant the version that the respective program is adapted for and that it can manage with the right result). If the program is run against the wrong version of the data model, an error message is received and the program is interrupted.
        /// </summary>
        public class TblMetabaseInfo : Tab
        {

            /// <summary>
            /// Macro or micro.
            /// </summary>
            public Col ModelCol;
            /// <summary>
            /// Version number for the metadata model that the metadata database uses.
            /// </summary>
            public Col ModelVersionCol;
            /// <summary>
            /// Role of database. Can be:\n- Production\n- Operation\n- Text
            /// </summary>
            public Col DatabaseRoleCol;

            internal TblMetabaseInfo(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("MetabaseInfo","MBI"), config.ExtractTableName("MetabaseInfo","METABASEINFO"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MetabaseInfo", "Model", "MODEL");
                this.ModelCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MetabaseInfo", "ModelVersion", "MODELVERSION");
                this.ModelVersionCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("MetabaseInfo", "DatabaseRole", "DATABASEROLE");
                this.DatabaseRoleCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table contains information on the authorities and organisations that are responsible for or produce statistics, which are stored in the Macro or Micro databases.
        /// </summary>
        public class TblOrganization : Tab
        {

            /// <summary>
            /// Code for the organisation or authority that produces and/or is responsible for the statistics material. \nThe field is used both to show the organisation responsible for the statistics and the producer of the statistics in the table Contents.\nAt statistics sweden,\n For every authority/organisation that is responsible for some statistical material in the databases, there should be a "main item" in the table Organization, where the OrganizationCode consists of an abbreviation of the authority's/organisation's name, official or unofficial (i.e. SCB or KI). Used in the field StatAuthority in Contents.\nWhere Statistics Sweden is the producer of the statistics, there should be an item for every function within Statistics Sweden that produces statistics in the databases. In this case, OrganizationCode should be an abbreviation, which, in addition to "SCB", should also contain the department or unit name, divided by a forward slash, i.e.SCB/BV/BE. Used in the field Producer in Contents.\nWhere the statistics are produced by another statistical authority, the authority's/organisation's abbreviated name should be used to give the producer in the field Producer in Contents. In this case, the fields Department and Unit in Organization should be NULL.\nThe code is written in capital letters.
            /// </summary>
            public Col OrganizationCodeCol;
            /// <summary>
            /// Name of authority/organisation in full text, including any official abbreviation in brackets, i.e. Statistics Sweden (SCB).\nText should begin with a capital letter.
            /// </summary>
            public Col OrganizationNameCol;
            /// <summary>
            /// Name of the department or equivalent within Statistics Sweden that produces the statistics.\nOnly filled in for statistics producers within Statistics Sweden, otherwise the field should be NULL.
            /// </summary>
            public Col DepartmentCol;
            /// <summary>
            /// Name of unit or equivalent within Statistics Sweden that produces the statistics.\nOnly filled in for statistics producers within Statistics Sweden, otherwise the field should be NULL.
            /// </summary>
            public Col UnitCol;
            /// <summary>
            /// Internet address to the authority's/organisation's website. Written as, for example:\nwww.scb.se\nIf Internet address is not available, the field should be NULL.
            /// </summary>
            public Col WebAddressCol;
            /// <summary>
            /// Identifying code for the Product database (PDB). \nAt statistics \n,\n Not yet implemented. Will later be obligatory to fill in at Statistics Sweden.
            /// </summary>
            public Col InternalIdCol;

            internal TblOrganization(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("Organization","ORG"), config.ExtractTableName("Organization","ORGANIZATION"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Organization", "OrganizationCode", "ORGANIZATIONCODE");
                this.OrganizationCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Organization", "OrganizationName", "ORGANIZATIONNAME");
                this.OrganizationNameCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Organization", "Department", "DEPARTMENT");
                this.DepartmentCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Organization", "Unit", "UNIT");
                this.UnitCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Organization", "WebAddress", "WEBADDRESS");
                this.WebAddressCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Organization", "InternalId", "INTERNALID");
                this.InternalIdCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblOrganizationLang2 : Lang2Tab
        {

            /// <summary>
            /// Code for the organisation or authority that produces and/or is responsible for the statistics material. \nSee description of the table Organization.
            /// </summary>
            public Lang2Col OrganizationCodeCol;

            /// <summary>
            /// English name for authority/organisation in full text, including any official abbreviation in brackets.\nText should begin with a capital letter.\nIf English text is not available, the field should be NULL.
            /// </summary>
            public Lang2Col OrganizationNameCol;

            /// <summary>
            /// English name of the department or equivalent within Statistics Sweden that produces the statistics.\nOnly filled in for statistics producers within Statistics Sweden, otherwise the field should be NULL.
            /// </summary>
            public Lang2Col DepartmentCol;

            /// <summary>
            /// English name of unit within Statistics Sweden that produces the statistics or equivalent.\nOnly filled in for statistics producers within Statistics Sweden, otherwise the field should be NULL.
            /// </summary>
            public Lang2Col UnitCol;

            internal TblOrganizationLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("OrganizationLang2","OR2"), config.ExtractTableName("OrganizationLang2","ORGANIZATION_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("OrganizationLang2", "OrganizationCode"  , "ORGANIZATIONCODE");
                this.OrganizationCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("OrganizationLang2", "OrganizationName"  , "ORGANIZATIONNAME");
                this.OrganizationNameCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("OrganizationLang2", "Department"  , "DEPARTMENT");
                this.DepartmentCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("OrganizationLang2", "Unit"  , "UNIT");
                this.UnitCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table contains information on all persons (or alternatively groups) which are contact persons for content and/or responsible for updating statistics in the macro database.
        /// </summary>
        public class TblPerson : Tab
        {

            /// <summary>
            /// Identifying code for person (or group) responsible. \nAt statistics \n,\n For persons within Statistics Sweden, the person's log-on code. For persons within other statistical authorities, the code consists of an abbreviation for the authority, a hyphen and the person's initials. If the same initials occur for several persons within the same authority, they can be differentiated by a number at the end.
            /// </summary>
            public Col PersonCodeCol;
            /// <summary>
            /// Responsible person's first name \nFor groups, this data is not available and should therefore be NULL.
            /// </summary>
            public Col ForenameCol;
            /// <summary>
            /// Surname of the person responsible or name of the group responsible.
            /// </summary>
            public Col SurnameCol;
            /// <summary>
            /// Code for the organisation or authority that produces and/or is responsible for the statistical material. \nSee further description of the table Organization.
            /// </summary>
            public Col OrganizationCodeCol;
            /// <summary>
            /// Prefix for telephone number, so that the number is valid internationally.\nAt statistics \n,\n For Swedish telephone numbers, write:  +46.
            /// </summary>
            public Col PhonePrefixCol;
            /// <summary>
            /// Complete national telephone numbers, i.e. without international prefix.\nShould be written as: national code, hyphen, then numbers in groups of two or three, divided by a space.
            /// </summary>
            public Col PhoneNoCol;
            /// <summary>
            /// Complete national fax machine numbers, i.e. without international prefix.\nShould be written as: national code, hyphen, then numbers in groups of two or three, divided by a space.\nIf there is no fax number, this field should be NULL.
            /// </summary>
            public Col FaxNoCol;
            /// <summary>
            /// E-mail address for person or group responsible, if available.\nIf an e-mail address is not available, the field should be NULL.\nWritten with lower case letters.
            /// </summary>
            public Col EmailCol;

            internal TblPerson(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("Person","PRS"), config.ExtractTableName("Person","PERSON"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Person", "PersonCode", "PERSONCODE");
                this.PersonCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Person", "Forename", "FORENAME");
                this.ForenameCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Person", "Surname", "SURNAME");
                this.SurnameCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Person", "OrganizationCode", "ORGANIZATIONCODE");
                this.OrganizationCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Person", "PhonePrefix", "PHONEPREFIX");
                this.PhonePrefixCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Person", "PhoneNo", "PHONENO");
                this.PhoneNoCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Person", "FaxNo", "FAXNO");
                this.FaxNoCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Person", "Email", "EMAIL");
                this.EmailCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table contains information on the special characters that are used in the database's data tables. Special characters such as ....or - , can be used to show that data is missing, is not relevant or is too uncertain to be given. \nAt statistics \n,\n Not yet implemented. 
        /// </summary>
        public class TblSpecialCharacter : Tab
        {

            /// <summary>
            /// Identifying code for the special character. \nGiven in the form of a number, from 1 upwards. The highest acceptable number is given in the table MetaAdm, which is 99 (see description in table MetaAdm).\n(Datamod vers 2.1, not yet impl. at Statistics Sweden):\nIf PresMissingLine in Contents contains the identity for a special character, this character must be represented here. See also descriptions of PresCellsZero and PresMissingLine in Contents, PresCharacter in SpecialCharacter and the table MetaAdm. 
            /// </summary>
            public Col CharacterTypeCol;
            /// <summary>
            /// The special character as presented for the user when the table is presented when retrieved.
            /// </summary>
            public Col PresCharacterCol;
            /// <summary>
            /// Used to show whether the data cell with the special character can be aggregated or not. There are the following alternatives:\nY = Yes\nN = No\nIf AggregPossible = Y, the specific data cell, even if not shown, can be included in an aggregation.
            /// </summary>
            public Col AggregPossibleCol;
            /// <summary>
            /// Provides the retrieval programs with information concerning the presentation of a special character;  with data and special character or with special character only. \nThere are the following alternatives: \nY = The data cell should be presented together with the special character\nN = The special character alone should be presented
            /// </summary>
            public Col DataCellPresCol;
            /// <summary>
            /// Shows whether the data cell must be filled in or not. There are the following alternatives:\nV = Value must be filled in\nN = No, the data cell should not be  filled in but should be NULL\nF = Any, i.e. the data cell can be filled in or can be NULL.\n0 = The data cell should contain 0 (zero) only
            /// </summary>
            public Col DataCellFilledCol;
            /// <summary>
            /// Explanation to what is written in PresCharacter.\nIf there is no presentation text, the field should be NULL.
            /// </summary>
            public Col PresTextCol;

            internal TblSpecialCharacter(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("SpecialCharacter","SPC"), config.ExtractTableName("SpecialCharacter","SPECIALCHARACTER"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "CharacterType", "CHARACTERTYPE");
                this.CharacterTypeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "PresCharacter", "PRESCHARACTER");
                this.PresCharacterCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "AggregPossible", "AGGREGPOSSIBLE");
                this.AggregPossibleCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "DataCellPres", "DATACELLPRES");
                this.DataCellPresCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "DataCellFilled", "DATACELLFILLED");
                this.DataCellFilledCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblSpecialCharacterLang2 : Lang2Tab
        {

            /// <summary>
            /// Identifying code for the special character. \nGiven in the form of a number, from 1 upwards.
            /// </summary>
            public Lang2Col CharacterTypeCol;

            /// <summary>
            /// The special character as presented for the user when the table is presented when retrieved.\nIf the field is not filled in, it should be NULL.
            /// </summary>
            public Lang2Col PresCharacterCol;

            /// <summary>
            /// English presentation text for special character.\nIf there is no English presentation text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextCol;

            internal TblSpecialCharacterLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("SpecialCharacterLang2","SP2"), config.ExtractTableName("SpecialCharacterLang2","SPECIALCHARACTER_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SpecialCharacterLang2", "CharacterType"  , "CHARACTERTYPE");
                this.CharacterTypeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("SpecialCharacterLang2", "PresCharacter"  , "PRESCHARACTER");
                this.PresCharacterCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("SpecialCharacterLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table contains information on the sub-tables, reflecting the stored data tables, which are in the subject databases.  The data tables are identified using the main table's name + sub-table's name.
        /// </summary>
        public class TblSubTable : Tab
        {

            /// <summary>
            /// Name of the sub-table. For material stored only in the sub-table, the field is left empty, i.e. a dash is written.\nName of main table + name of sub-table together make up the name of the data table where the data is stored, if SubTable is not empty, in which case the name of the data table is made up of the name of the main table only.\nAt statistics \n,\n For material that is divided into several sub-tables, the following rules for naming apply:\n- For material that is not divided regionally, a consecutive numbering for the sub-tables is used that is linked to a specific main table, i.e.01, 02, etc.\n- For material that is divided regionally, the sub-tables are given the name:\nM (for sub-tables divided by municipality)\nC (for sub-tables divided by county)\nR (for sub-tables containing the whole country only)\n...followed by a consecutive numbering within each region value set for the sub-tables, which are linked to the relevant main table, i.e. M1, M2, C1, C2.\nNB. Make sure that the numbering is always included even if there is only one sub-table divided by region among the sub-tables that are linked to the relevant main table.
            /// </summary>
            public Col SubTableCol;
            /// <summary>
            /// The name of the main table, to which the sub-table is linked. See further description in the table MainTable.
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Descriptive text that is used by the retrieval interface, i.e. when selecting a sub-level to a table or sub-table in the retrieval interface, if the main table has several sub-tables.\nAt statistics \n,\n If an established abbreviation for the statistical product exists, i.e. AKU or KPI, this should be included in the presentation text.\nThe text should be unique (there should not be two sub-tables with the same PresText) and should contain information on all the division variables, excluding totals. Information on timescale should be added at the end.\nFor data material that is only stored in a sub-table, the text should be the same as PresText in the table MainTable.\nFor data material that is divided up into different sub-tables, the main table’s presentation text should be used as a "model", which is supplemented with the information that differentiates the sub-tables.\nThe text should begin with a capital letter, should not end with a full stop, and should not include the % symbol.
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Shows whether the sub-tables values can be aggregated or not.\nAt statistics \n,\n No longer in use. From June 1998, CleanTable = X was set for all new sub-tables, which should be written in. For older tables, the following codes can appear:\n- Y = yes, i.e. the sub-table values can be aggregated.\n- N = no, i.e. the sub-tables values cannot be aggregated.
            /// </summary>
            public Col CleanTableCol;

            internal TblSubTable(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("SubTable","STB"), config.ExtractTableName("SubTable","SUBTABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SubTable", "SubTable", "SUBTABLE");
                this.SubTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SubTable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SubTable", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SubTable", "CleanTable", "CLEANTABLE");
                this.CleanTableCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblSubTableLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of sub-table\nSee further in the description of the table SubTable.
            /// </summary>
            public Lang2Col SubTableCol;

            /// <summary>
            /// Name of main table.\nSee further in the description of the table MainTable.
            /// </summary>
            public Lang2Col MainTableCol;

            /// <summary>
            /// English presentation text for sub-table.\nIf there is no English presentation text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextCol;

            internal TblSubTableLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("SubTableLang2","ST2"), config.ExtractTableName("SubTableLang2","SUBTABLE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SubTableLang2", "SubTable"  , "SUBTABLE");
                this.SubTableCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("SubTableLang2", "MainTable"  , "MAINTABLE");
                this.MainTableCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("SubTableLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table links variables with value sets in the sub-tables.  The variable name is made up of the name for the corresponding metadata column in the data tables.
        /// </summary>
        public class TblSubTableVariable : Tab
        {

            /// <summary>
            /// The name of the main table to which the variable and the sub-table are linked. See further description in the MainTable table. 
            /// </summary>
            public Col MainTableCol;
            /// <summary>
            /// Name of sub-table\nData can be missing, with a dash in its place.\nSee further description in the table SubTable.
            /// </summary>
            public Col SubTableCol;
            /// <summary>
            /// Variable name, which makes up the column name for metadata in the data table. See further description in the table Variable.
            /// </summary>
            public Col VariableCol;
            /// <summary>
            /// Name of value set. See further description in the table ValueSet.\nFor rows with variable types V and G, the name of the value set must be filled in. For VariableType = T, the field is left empty, as there is no value set for the variable Time.
            /// </summary>
            public Col ValueSetCol;
            /// <summary>
            /// Code for type of variable. There are three alternatives:\n- V = variable, i.e. dividing variable, not time.\n- G = geographical information for map program. \nAt statistics \n,\n Not yet implemented.\n- T = time.\nIf VariableType = G, the field GeoAreaNo in the tables ValueSet and Grouping should be filled in (however not yet implemented).
            /// </summary>
            public Col VariableTypeCol;
            /// <summary>
            /// The variable's column number in the data table.\nThe variable Time should always be included and be the last column in the data table. If the material is divided by region, the variable Region should be the first column.\nWritten as 1, 2, 3, etc.
            /// </summary>
            public Col StoreColumnNoCol;

            internal TblSubTableVariable(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("SubTableVariable","STV"), config.ExtractTableName("SubTableVariable","SUBTABLEVARIABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "SubTable", "SUBTABLE");
                this.SubTableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "Variable", "VARIABLE");
                this.VariableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "ValueSet", "VALUESET");
                this.ValueSetCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "VariableType", "VARIABLETYPE");
                this.VariableTypeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "StoreColumnNo", "STORECOLUMNNO");
                this.StoreColumnNoCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table contains information on joint texts.
        /// </summary>
        public class TblTextCatalog : Tab
        {

            /// <summary>
            /// Identity of text.\nAt statistics \n,\n Is written as a run number: 1, 2, 3 etc.
            /// </summary>
            public Col TextCatalogNoCol;
            /// <summary>
            /// Type of text. The texts should be fixed for use in PC-AXIS.\nAlternatives:\n- ContentsVariable\n- GeoAreaNo\n- Language1, Language2 osv. (Datamod vers 2.1, not yet impl. at Ststistics \n)
            /// </summary>
            public Col TextTypeCol;
            /// <summary>
            /// The text that should be shown. Can be the name of a map file etc., or a language (datamod vers 2.1, not yet impl. at Statistics Sweden). The language should be written in the language it refers to, e.g. svenska, English, Espanol. See also the description of the table MetaAdm. 
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Description of text.\nIf a description is not available, the field should be NULL.
            /// </summary>
            public Col DescriptionCol;

            internal TblTextCatalog(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("TextCatalog","TXC"), config.ExtractTableName("TextCatalog","TEXTCATALOG"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("TextCatalog", "TextCatalogNo", "TEXTCATALOGNO");
                this.TextCatalogNoCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("TextCatalog", "TextType", "TEXTTYPE");
                this.TextTypeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("TextCatalog", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("TextCatalog", "Description", "DESCRIPTION");
                this.DescriptionCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblTextCatalogLang2 : Lang2Tab
        {

            /// <summary>
            /// Identity of text.\nAt statistics \n,\n Is written as a run number: 1, 2, 3 etc.
            /// </summary>
            public Lang2Col TextCatalogNoCol;

            /// <summary>
            /// Type of text. See further in the description of the table TextCatalog. 
            /// </summary>
            public Lang2Col TextTypeCol;

            /// <summary>
            /// English presentation text for TextCatalog.\nFor a description of PresText, see table TextCatalog.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextCol;

            /// <summary>
            /// Description of text.\nIf a description is not available, the field should be NULL.
            /// </summary>
            public Lang2Col DescriptionCol;

            internal TblTextCatalogLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("TextCatalogLang2","TX2"), config.ExtractTableName("TextCatalogLang2","TEXTCATALOG_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("TextCatalogLang2", "TextCatalogNo"  , "TEXTCATALOGNO");
                this.TextCatalogNoCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("TextCatalogLang2", "TextType"  , "TEXTTYPE");
                this.TextTypeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("TextCatalogLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("TextCatalogLang2", "Description"  , "DESCRIPTION");
                this.DescriptionCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table describes the timescales that exist in the macro database.
        /// </summary>
        public class TblTimeScale : Tab
        {

            /// <summary>
            /// Name of timescale, i.e.Year, Month, Quarter.\nShould not contain dash (applies for retrievals in PC-AXIS).
            /// </summary>
            public Col TimeScaleCol;
            /// <summary>
            /// Presentation text for timescale, i.e. year, month, quarter. Text is often the same as the name in the column TimeScale. Written in lower case.\nPresentation text used when selecting time when making a retrieval from databases.
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Shows if the timescale should be presented in table heading instead of the word Time. Can be:\nY = Yes\nN = No\nAt statistics \n,\n Not yet implemented. Is currently"N" for all timescales.
            /// </summary>
            public Col TimeScalePresCol;
            /// <summary>
            /// Shows if timescale is regular or not. Can be:\nY = Yes\nN = No\nAn example of an irregular timescale is an election year.\nData is primarily accompanying information when retrieving statistics to a file.
            /// </summary>
            public Col RegularCol;
            /// <summary>
            /// Code for TimeUnit. Used as accompanying information when retrieving a statistics file. The following alternatives are possible:\nQ = quarter\nA = academic year\nM = month\nX = 3 years\nS = split year\nY = year
            /// </summary>
            public Col TimeUnitCol;
            /// <summary>
            /// Shows how many points in time the relevant timescale contains per calendar year, i.e.:\n1 for timescale year,\n4 for timescale quarter,\n12 for timescale month.\nFor irregular and regular timescales, where points in time do not occur consecutively (i.e. every other year), the field should be NULL.
            /// </summary>
            public Col FrequencyCol;
            /// <summary>
            /// Description of storage format for the point in time in the timescale. There are the following alternatives: \nyyyy for timescales where TimeUnit = Y,\nyyyy-yyyy for timescales where TimeUnit = T,\nyyyy/yy for timescales where TimeUnit = A,\nyyyy/yyyy for timescales where TimeUnit = P,\nyyyyQq for timescales where TimeUnit = Q,\nyyyyMmm for timescales where TimeUnit = M.\nFor a description of time units, see column TimeUnit in the table TimeScale.
            /// </summary>
            public Col StoreFormatCol;

            internal TblTimeScale(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("TimeScale","TSC"), config.ExtractTableName("TimeScale","TIMESCALE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("TimeScale", "TimeScale", "TIMESCALE");
                this.TimeScaleCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("TimeScale", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("TimeScale", "TimeScalePres", "TIMESCALEPRES");
                this.TimeScalePresCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("TimeScale", "Regular", "REGULAR");
                this.RegularCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("TimeScale", "TimeUnit", "TIMEUNIT");
                this.TimeUnitCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("TimeScale", "Frequency", "FREQUENCY");
                this.FrequencyCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("TimeScale", "StoreFormat", "STOREFORMAT");
                this.StoreFormatCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblTimeScaleLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of timescale. Regular Swedish name.\nSee description of the table TimeScale.
            /// </summary>
            public Lang2Col TimeScaleCol;

            /// <summary>
            /// English presentation text for timescale.\nSee description of the table TimeScale.\nIf there is no English presentation text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextCol;

            internal TblTimeScaleLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("TimeScaleLang2","TS2"), config.ExtractTableName("TimeScaleLang2","TIMESCALE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("TimeScaleLang2", "TimeScale"  , "TIMESCALE");
                this.TimeScaleCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("TimeScaleLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table describes the values in the value pool.
        /// </summary>
        public class TblValue : Tab
        {

            /// <summary>
            /// Name of the value pool that the value belongs to. See further description of table ValuePool.
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// Code for value or group.\nValue code should agree with the code in the corresponding classification or standard if there is one.\nBecause the value codes are stored in a metadata column for variables in the data table(s) and, because the width of the metadata column is decided by the number of characters in the longest value code, the code should not be longer than necessary to ensure that it does not take up more space in the data table than necessary. The value codes within a value set should also be roughly the same size.\nCapitals and/or lower case letters can be used, the letters å, ä and ö are accepted. Special characters and dashes should be avoided because they can cause technical problems.
            /// </summary>
            public Col ValueCodeCol;
            /// <summary>
            /// Sorting code for values and groups, which decides in which order the value and group codes are to be presented when values and table presentation are selected when retrieved from the databases.\nThe sorting code should be the same as the ValueCode or be designed in such a way that the values can be presented in the desired order. The beginning of ValueTextL can be used so that the values will be presented in alphabetical order by the value text.\nNB. Please note that the sorting code is also available in the tables VSValue, VSGroup and Grouping. See further descriptions for these.
            /// </summary>
            public Col SortCodeCol;
            /// <summary>
            /// Short presentation text for value and group. \nTo be visible in the retrieval interfaces, it requires that:\n- The field ValueTextExists in ValuePool is either S ('Short value text exists') or B ('Both short and long value text exists') and \n- The field ValuePres in ValuePool or ValueSet is either A ('Both code and short value text should be presented') or S ('Short value text should be presented').\nThe text is written in lower case letters, except for abbreviations etc. \nSee also descriptions of ValueTextExists in ValuePool and ValuePres in ValuePool and ValueSet. 
            /// </summary>
            public Col ValueTextSCol;
            /// <summary>
            /// Value text, presentation text for value and group.\nTo be visible in the retrieval interface, the field ValueTextExists in the table ValuePool for the value's value pool must be L.\nValueText can be omitted if the values are to be presented only as codes. The field should then be NULL. There should be consistency with a value pool so that all the value pool's values are presented either with or without value texts.\nThe text is written in lower case, with the exception of abbreviations, etc.\nSee also descriptions of ValueTextExists in ValuePool and  ValuePres in ValuePool and ValueSet. 
            /// </summary>
            public Col ValueTextLCol;
            /// <summary>
            /// Shows whether there is a footnote linked to the value (FootnoteType 6). There are the following alternatives:\nB = Both obligatory and optional footnotes exist\nV = One or several optional footnotes exist.\nO = One or several obligatory footnotes exist\nN = There are no footnotes
            /// </summary>
            public Col FootnoteCol;

            internal TblValue(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("Value","VAL"), config.ExtractTableName("Value","VALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Value", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Value", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Value", "SortCode", "SORTCODE");
                this.SortCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Value", "ValueTextS", "VALUETEXTS");
                this.ValueTextSCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Value", "ValueTextL", "VALUETEXTL");
                this.ValueTextLCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Value", "Footnote", "FOOTNOTE");
                this.FootnoteCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblValueLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of value pool.\nDescription of the table ValuePool.
            /// </summary>
            public Lang2Col ValuePoolCol;

            /// <summary>
            /// Code for value.\nSee description of the table Value. 
            /// </summary>
            public Lang2Col ValueCodeCol;

            /// <summary>
            /// Sorting code for English presentation texts for values. Can be used for sorting alphabetically by the English text.\nIf there is no text, the field should be NULL.\nIf a specific English sorting code is not necessary, the same sorting code as for the Swedish presentation text is used (SortCode in Value).
            /// </summary>
            public Lang2Col SortCodeCol;

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col ValueTextSCol;

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col ValueTextLCol;

            internal TblValueLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValueLang2","VA2"), config.ExtractTableName("ValueLang2","VALUE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueLang2", "ValuePool"  , "VALUEPOOL");
                this.ValuePoolCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueLang2", "ValueCode"  , "VALUECODE");
                this.ValueCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueLang2", "ValueTextS"  , "VALUETEXTS");
                this.ValueTextSCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueLang2", "ValueTextL"  , "VALUETEXTL");
                this.ValueTextLCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table exists to make it possible to present presentation texts for values that are longer than 250 characters and to be able to have different units for different values. The texts can be divided up into four fields of a maximum of 255 characters. Only one variable per main table may have value texts in ValueExtra.
        /// </summary>
        public class TblValueExtra : Tab
        {

            /// <summary>
            /// Name of value pool.\nSee further in the description of the table ValuePool.
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// Code for value.\nSee description of the table Value.
            /// </summary>
            public Col ValueCodeCol;
            /// <summary>
            /// Can be used to state the unit so that a value can have different units.\nIf the field is filled in with a unit, the column Unit in the table Contents should be filled with %ValueExtra. If the field is not filled in, it should be NULL. Then the column Unit in the table Contents is used instead to state the unit.\nSee also description of the table Contents.
            /// </summary>
            public Col UnitCol;
            /// <summary>
            /// Extra long presentation texts for value. Can contain a maximum of 255 characters. If the text is longer, the field ValueTextX2 is used to continue. The text should then be divided between complete words, before the space (the space carries on in the next column).\nIf there is no text, the field should be NULL.
            /// </summary>
            public Col ValueTextX1Col;
            /// <summary>
            /// Continuation of extra long presentation text for value. Can contain a maximum of 255 characters. If the text is longer, the field ValueTextX3 is used to continue. For information on division of text, see column ValueTextX1.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Col ValueTextX2Col;
            /// <summary>
            /// Continuation of extra long presentation text for value. Can contain a maximum of 255 characters. If the text is longer, the field ValueTextX4 is used to continue. For information on division of text, see column ValueTextX1.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Col ValueTextX3Col;
            /// <summary>
            /// Continuation of extra long presentation text for value. Can contain a maximum of 255 characters.  The maximum length for a presentation text is 1020 characters. If the text is longer, it should be shortened by cutting off the end.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Col ValueTextX4Col;

            internal TblValueExtra(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValueExtra","VXT"), config.ExtractTableName("ValueExtra","VALUEEXTRA"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueExtra", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueExtra", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueExtra", "Unit", "UNIT");
                this.UnitCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueExtra", "ValueTextX1", "VALUETEXTX1");
                this.ValueTextX1Col = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueExtra", "ValueTextX2", "VALUETEXTX2");
                this.ValueTextX2Col = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueExtra", "ValueTextX3", "VALUETEXTX3");
                this.ValueTextX3Col = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueExtra", "ValueTextX4", "VALUETEXTX4");
                this.ValueTextX4Col = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblValueExtraLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of value pool.\nSee description of the table ValuePool.
            /// </summary>
            public Lang2Col ValuePoolCol;

            /// <summary>
            /// Code for value.\nSee description of the table Value.
            /// </summary>
            public Lang2Col ValueCodeCol;

            /// <summary>
            /// English presentation text for unit.\nIf there is no unit, the field should be NULL.\nFor a description of Unit, see the table ValueExtra.
            /// </summary>
            public Lang2Col UnitCol;

            /// <summary>
            /// English presentation text for extra long value texts.\nFor a description of ValueTextX1, see the table ValueExtra.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col ValueTextX1Col;

            /// <summary>
            /// English presentation text for extra long value texts.\nFor a description of ValueTextX2, see the table ValueExtra.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col ValueTextX2Col;

            /// <summary>
            /// English presentation text for extra long value texts. \nFor a description of ValueTextX3, see the table ValueExtra.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col ValueTextX3Col;

            /// <summary>
            /// English presentation text for extra long value texts. \nFor a description of ValueTextX4, see the table ValueExtra.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col ValueTextX4Col;

            internal TblValueExtraLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValueExtraLang2","VX2"), config.ExtractTableName("ValueExtraLang2","VALUEEXTRA_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueExtraLang2", "ValuePool"  , "VALUEPOOL");
                this.ValuePoolCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueExtraLang2", "ValueCode"  , "VALUECODE");
                this.ValueCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueExtraLang2", "Unit"  , "UNIT");
                this.UnitCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueExtraLang2", "ValueTextX1"  , "VALUETEXTX1");
                this.ValueTextX1Col = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueExtraLang2", "ValueTextX2"  , "VALUETEXTX2");
                this.ValueTextX2Col = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueExtraLang2", "ValueTextX3"  , "VALUETEXTX3");
                this.ValueTextX3Col = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueExtraLang2", "ValueTextX4"  , "VALUETEXTX4");
                this.ValueTextX4Col = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// Tabellen kopplar ihop värden/värdemängder med grupperingar. 
        /// </summary>
        public class TblValueGroup : Tab
        {

            /// <summary>
            /// Namn på grupperingen. \nSe vidare beskrivningen av tabellen Gruppering.
            /// </summary>
            public Col GroupingCol;
            /// <summary>
            /// Kod för grupp. Hämtas från tabellen Varde, kolumnen Vardekod.\nSe beskrivning av tabellen Varde.
            /// </summary>
            public Col GroupCodeCol;
            /// <summary>
            /// Kod för värde som ingår i grupp. Hämtas från tabellen Varde, kolumnen Vardekod.\nSe beskrivning av tabellen Varde.
            /// </summary>
            public Col ValueCodeCol;
            /// <summary>
            /// Namn på det värdeförråd, som grupperingen är kopplad till. \nSe beskrivning av tabellen Vardeforrad.
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// Indicates wich level the group code belongs to. 
            /// </summary>
            public Col GroupLevelCol;
            /// <summary>
            /// Indicates wich level the value code belongs to. 
            /// </summary>
            public Col ValueLevelCol;
            /// <summary>
            /// Kod för sortering av grupper inom en gruppering, för att kunna presentera dem i en logisk ordning. \nOm någon grupp inom en gruppering för en värdemängd har sorteringskod, ska samtliga grupper ha det. \n sorteringskod saknas, skall fältet vara NULL.
            /// </summary>
            public Col SortCodeCol;

            internal TblValueGroup(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValueGroup","VPL"), config.ExtractTableName("ValueGroup","VALUEGROUP"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueGroup", "Grouping", "GROUPING");
                this.GroupingCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueGroup", "GroupCode", "GROUPCODE");
                this.GroupCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueGroup", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueGroup", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueGroup", "GroupLevel", "GROUPLEVEL");
                this.GroupLevelCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueGroup", "ValueLevel", "VALUELEVEL");
                this.ValueLevelCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueGroup", "SortCode", "SORTCODE");
                this.SortCodeCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblValueGroupLang2 : Lang2Tab
        {

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col GroupingCol;

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col GroupCodeCol;

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col ValueCodeCol;

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col ValuePoolCol;

            /// <summary>
            /// 
            /// </summary>
            public Lang2Col SortCodeCol;

            internal TblValueGroupLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValueGroupLang2","VL2"), config.ExtractTableName("ValueGroupLang2","VALUEGROUP_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueGroupLang2", "Grouping"  , "GROUPING");
                this.GroupingCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueGroupLang2", "GroupCode"  , "GROUPCODE");
                this.GroupCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueGroupLang2", "ValueCode"  , "VALUECODE");
                this.ValueCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueGroupLang2", "ValuePool"  , "VALUEPOOL");
                this.ValuePoolCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueGroupLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table describes which value pools exist in the macro database. The value pool brings together all values and aggregates for a classification or a variation of a classification.
        /// </summary>
        public class TblValuePool : Tab
        {

            /// <summary>
            /// Name of value pool.\nAt statistics \n,\n There should be a value pool for every classification or variation of a classification. The name should correspond to the name in the Classification Database (KDB), if the value pool/classification is included there. The name should be descriptive. A suffix which states the version/variation/year can also be used, i.e.SNI92BR, SUN2000.\nIf there is only one variable belonging to a particular value pool, the variable and value pool should have the same name.\nThe name should begin with a capital letter.
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// 
            /// </summary>
            public Col ValuePoolAliasCol;
            /// <summary>
            /// At statistics \n,\n The field is currently not used. Should until further notice be NULL.\nIs planned to be used for presentation texts for value pools.
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Description of value pool.\nShould also contain information on the principles used for sorting the value pool's values (i.e....sorting by particular principle,....sorting by value code).\nWritten beginning with a capital letter.
            /// </summary>
            public Col DescriptionCol;
            /// <summary>
            /// Here it is stated whether there are texts or not for the value pool's values, and whether they are in the table in ValueExtra. There are the following alternatives:\nL = Long value text exists\nS = Short value text exists\nB = Both long and short value text exist\nN = No value texts for any values\nX = All values are in ValueExtra\nIn the table Value (see descriptions of these columns) there are two columns for value texts, ValueTextS (for short texts) and ValueTextL (for long texts). If ValueTextExists = L, the value text is taken from column ValueTextL in the table Value. If ValueTextExists = S, the value text is taken from column ValueTextS in the table Value. If ValueTextExists = B, the value presentation is determined by what is specified in the field ValuePres in the tables ValuePool or ValueSet. If ValueTextExists = N, the values are presented only by a code in the retrieval interface. If ValueTextExists = X, the value texts are taken from table ValueExtra (see further description of this).
            /// </summary>
            public Col ValueTextExistsCol;
            /// <summary>
            /// Here it is shown how the values should be presented after retrieval. There are the following alternatives:\nA = Both code and short text should be presented\nB = Both code and long text should be presented\nC = Value code should be presented\nT = Long value text should be presented\nS = Short value text should be presented
            /// </summary>
            public Col ValuePresCol;
            /// <summary>
            /// 
            /// </summary>
            public Col KDBIdCol;

            internal TblValuePool(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValuePool","VPL"), config.ExtractTableName("ValuePool","VALUEPOOL"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValuePool", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValuePool", "ValuePoolAlias", "VALUEPOOL");
                this.ValuePoolAliasCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValuePool", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValuePool", "Description", "DESCRIPTION");
                this.DescriptionCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValuePool", "ValueTextExists", "VALUETEXTEXISTS");
                this.ValueTextExistsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValuePool", "ValuePres", "VALUEPRES");
                this.ValuePresCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValuePool", "KDBId", "KDBID");
                this.KDBIdCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblValuePoolLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of value pool.\nSee description of the table ValuePool.
            /// </summary>
            public Lang2Col ValuePoolCol;

            /// <summary>
            /// English name of the value pool.\nIs used in the keyword Domain in the px-file.
            /// </summary>
            public Lang2Col ValuePoolAliasCol;

            /// <summary>
            /// English presentation text for the value pool.\nIf there is no text, the field should be NULL.\nAt statistics \n,\n Not in use.
            /// </summary>
            public Lang2Col PresTextCol;

            internal TblValuePoolLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValuePoolLang2","VP2"), config.ExtractTableName("ValuePoolLang2","VALUEPOOL_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValuePoolLang2", "ValuePool"  , "VALUEPOOL");
                this.ValuePoolCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValuePoolLang2", "ValuePoolAlias"  , "VALUEPOOLALIAS");
                this.ValuePoolAliasCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValuePoolLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table describes the value sets that exist for the different value pools.
        /// </summary>
        public class TblValueSet : Tab
        {

            /// <summary>
            /// Name of the stored value set\n.\nThe name should consist of the name of the value pool that the value set is linked to, plus a suffix. The suffix should always be used, even if there is only one value set for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.
            /// </summary>
            public Col ValueSetCol;
            /// <summary>
            /// (Datamod vers 2.1, not yet impl. at Statistics Sweden)\nPresentation text for value set. Can be used, if needed, as presentation text for the variable in the retrieval programs. It will then be the the variable name in the px file. \nIf the field is not used, it should be NULL.
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Description of the content of the value set.\nThe text should give a picture of the integral values, classes, aggregates and any totals, and should end with information on the number of values in the value set, including the total.\nText should begin with a capital letter.
            /// </summary>
            public Col DescriptionCol;
            /// <summary>
            /// Here it should be shown whether the variable can be eliminated or not.\nElimination means that the variable can be excluded when selecting the value when retrieving from the databases. The variable must in that case be able to assume a value, i.e. the sum of all integral values or another specific value, that is included in the value set. There are the following alternatives:\nN = No elimination value, i.e. the variable cannot be eliminated\nA = Elimination value is obtained by aggregation of all values in the value set\nValueCode = a selected value, included in the value set, that should be used at elimination.
            /// </summary>
            public Col EliminationCol;
            /// <summary>
            /// Name of the value pool that the value set belongs to. See further description of the table ValuePool. 
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// Used to show how the values in a value set should be presented when being retrieved. There are the following alternatives:\nA = Both code and short text should be presented\nB = Both code and long text should be presented\nK = Value code should be presented\nS = Short value text should be presented\nT = Long value text should be presented\nV = Presentation format is taken from the column ValuePres in the table ValuePool
            /// </summary>
            public Col ValuePresCol;
            /// <summary>
            /// Should contain the identification of a map that is suitable for the variable and the grouping. The field must be filled in if the column VariableType in the table SubTableVariable = G, otherwise the field is NULL.\nThe identification number should also be included in the table TextCatalog. For further information see description of TextCatalog. 
            /// </summary>
            public Col GeoAreaNoCol;
            /// <summary>
            /// 
            /// </summary>
            public Col KDBIdCol;
            /// <summary>
            /// Code showing whether there is a particular sorting order for the value set. Can be:\nY = Yes\nN = No\nIf SortCodeExists = Y, the sorting code must be in VSValue for all values included in the value set.\nIf SortCodeExists = N, the sorting code for the value pool is used (SortCode in the table Value).
            /// </summary>
            public Col SortCodeExistsCol;
            /// <summary>
            /// Shows whether there is a footnote linked to a value in the value set (FootNoteType 6). There are the following alternatives:\nB = Both obligatory and optional footnotes exist\nV = One or several optional footnotes exist.\nO = One or several obligatory footnotes exist\nS = There are no footnotes
            /// </summary>
            public Col FootnoteCol;

            internal TblValueSet(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValueSet","VST"), config.ExtractTableName("ValueSet","VALUESET"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueSet", "ValueSet", "VALUESET");
                this.ValueSetCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSet", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSet", "Description", "DESCRIPTION");
                this.DescriptionCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSet", "Elimination", "ELIMINATION");
                this.EliminationCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSet", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSet", "ValuePres", "VALUEPRES");
                this.ValuePresCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSet", "GeoAreaNo", "GEOAREANO");
                this.GeoAreaNoCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSet", "KDBId", "KDBID");
                this.KDBIdCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSet", "SortCodeExists", "SORTCODEEXISTS");
                this.SortCodeExistsCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSet", "Footnote", "FOOTNOTE");
                this.FootnoteCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblValueSetLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of the stored value sets.\nSee description of table ValueSet. 
            /// </summary>
            public Lang2Col ValueSetCol;

            /// <summary>
            /// Prestext
            /// </summary>
            public Lang2Col PresTextCol;

            /// <summary>
            /// Description of the value set's contents in English.\nFor more information on Description, see table ValueSet.
            /// </summary>
            public Lang2Col DescriptionCol;

            internal TblValueSetLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValueSetLang2","VS2"), config.ExtractTableName("ValueSetLang2","VALUESET_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueSetLang2", "ValueSet"  , "VALUESET");
                this.ValueSetCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueSetLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueSetLang2", "Description"  , "DESCRIPTION");
                this.DescriptionCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table connects value set to grouping. 
        /// </summary>
        public class TblValueSetGrouping : Tab
        {

            /// <summary>
            /// Name of the stored value set\n.\nSee description of table ValueSet. 
            /// </summary>
            public Col ValueSetCol;
            /// <summary>
            /// Name of grouping.\nSee further in the description of the table Grouping.
            /// </summary>
            public Col GroupingCol;

            internal TblValueSetGrouping(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("ValueSetGrouping","VBL"), config.ExtractTableName("ValueSetGrouping","VALUESETGROUPING"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueSetGrouping", "ValueSet", "VALUESET");
                this.ValueSetCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("ValueSetGrouping", "Grouping", "GROUPING");
                this.GroupingCol = new Col(tmpColumnName, this.Alias);

            }

        }

        /// <summary>
        /// The table contains the distributed statistical variables in the macro database.
        /// </summary>
        public class TblVariable : Tab
        {

            /// <summary>
            /// Name of distributed statistical variable. Name of metadata column for the variable in the data table.\nThe variable name must be unique within a main table.\nThe name should be descriptive, i.e. have an obvious link to the presentation text, consist of a maximum of 20 characters, begin with a capital letter and should only contains letters (a-z) and numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
            /// </summary>
            public Col VariableCol;
            /// <summary>
            /// Presentation text for a variable. Used in the retrieval interface when selecting variables or values and in the heading text when the table is presented after retrieval.\nThe entire text should be written in lower case letters, with the exception of abbreviations, etc.
            /// </summary>
            public Col PresTextCol;
            /// <summary>
            /// Descriptive information on variables, primarily for internal use, to facilitate the selection of a variable when drawing up new tables.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Col VariableInfoCol;
            /// <summary>
            /// Shows whether there is a footnote linked to the variable (FootnoteType 5). There are the following alternatives:\nB = Both obligatory and optional footnotes exist\nV = One or several optional footnotes exist.\nO = One or several obligatory footnotes exist\nS = There are no footnotes
            /// </summary>
            public Col FootnoteCol;

            internal TblVariable(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("Variable","VBL"), config.ExtractTableName("Variable","VARIABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Variable", "Variable", "VARIABLE");
                this.VariableCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Variable", "PresText", "PRESTEXT");
                this.PresTextCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Variable", "VariableInfo", "VARIABLEINFO");
                this.VariableInfoCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("Variable", "Footnote", "FOOTNOTE");
                this.FootnoteCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblVariableLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of variable.\nSee further in the description of the table Variable.
            /// </summary>
            public Lang2Col VariableCol;

            /// <summary>
            /// English presentation text for a variable.\nFor a description of PresText, see table Variable.\nIf there is no text, the field should be NULL.
            /// </summary>
            public Lang2Col PresTextCol;

            internal TblVariableLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("VariableLang2","VB2"), config.ExtractTableName("VariableLang2","VARIABLE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("VariableLang2", "Variable"  , "VARIABLE");
                this.VariableCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("VariableLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table links values (for a value pool) to a value set, for which data is stored in the data table.
        /// </summary>
        public class TblVSValue : Tab
        {

            /// <summary>
            /// Name of the value set to which the values are linked.\nSee further description in table ValueSet.
            /// </summary>
            public Col ValueSetCol;
            /// <summary>
            /// Name of the value pool to which the value set belongs.\nSee further description in table ValuePool.
            /// </summary>
            public Col ValuePoolCol;
            /// <summary>
            /// Code for the values that are linked to the value set.\nSee further description in table Value.
            /// </summary>
            public Col ValueCodeCol;
            /// <summary>
            /// Sorting code for values within the value set. Dictates the presentation order for the value set's values when retrieving from the database and presenting the table.\nSo that this sorting code can be applied, the field SortCodeExists in the table ValueSet must be filled with Y. If it is N, the sorting code in the table Value is used instead.\nIf there is no sorting code, the field should be NULL.
            /// </summary>
            public Col SortCodeCol;

            internal TblVSValue(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("VSValue","VVL"), config.ExtractTableName("VSValue","VSVALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("VSValue", "ValueSet", "VALUESET");
                this.ValueSetCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("VSValue", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("VSValue", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Col(tmpColumnName, this.Alias);
                tmpColumnName = config.ExtractColumnName("VSValue", "SortCode", "SORTCODE");
                this.SortCodeCol = new Col(tmpColumnName, this.Alias);

            }

        }

        public class TblVSValueLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of value set.\nSee description of  table ValueSet. 
            /// </summary>
            public Lang2Col ValueSetCol;

            /// <summary>
            /// Name of value pool.\nSee description of the table ValuePool.
            /// </summary>
            public Lang2Col ValuePoolCol;

            /// <summary>
            /// Code for the values that are linked to the value set.\nSee further description in table Value.
            /// </summary>
            public Lang2Col ValueCodeCol;

            /// <summary>
            /// Sorting code for English presentation texts for the values in the value set. Can be used for sorting alphabetically by the English text within a value set.\nIf there is a sorting code for VSValue, for the Swedish value texts, there should also be a sorting code here, even if it is the same.\nIf there is no sorting code, the field should be NULL.
            /// </summary>
            public Lang2Col SortCodeCol;

            internal TblVSValueLang2(SqlDbConfig_22 config)
            : base(config.ExtractAliasName("VSValueLang2","VV2"), config.ExtractTableName("VSValueLang2","VSVALUE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("VSValueLang2", "ValueSet"  , "VALUESET");
                this.ValueSetCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("VSValueLang2", "ValuePool"  , "VALUEPOOL");
                this.ValuePoolCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("VSValueLang2", "ValueCode"  , "VALUECODE");
                this.ValueCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("VSValueLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Col(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        #endregion  structs


        private void initCodesAndKeywords()
        {

            #region Codes

            Codes = new Ccodes();

            Codes.CFPricesC = ExtractCode("CFPricesC", "C");
            Codes.CFPricesF = ExtractCode("CFPricesF", "F");
            Codes.StockFAS = ExtractCode("StockFAS", "S");
            Codes.StockFAF = ExtractCode("StockFAF", "F");
            Codes.StockFAA = ExtractCode("StockFAA", "A");
            Codes.Copyright1 = ExtractCode("Copyright1", "1");
            Codes.Copyright2 = ExtractCode("Copyright2", "2");
            Codes.Copyright3 = ExtractCode("Copyright3", "3");
            Codes.DataCellFilledOptional = ExtractCode("DataCellFilledOptional", "F");
            Codes.DataCellFilledValue = ExtractCode("DataCellFilledValue", "V");
            Codes.DataCellFilledZero = ExtractCode("DataCellFilledZero", "0");
            Codes.VariableTypeC = ExtractCode("VariableTypeC", "C");
            Codes.VariableTypeT = ExtractCode("VariableTypeT", "T");
            Codes.VariableTypeG = ExtractCode("VariableTypeG", "G");
            Codes.FootnoteM = ExtractCode("FootnoteM", "M");
            Codes.FootnoteR = ExtractCode("FootnoteR", "R");
            Codes.FootnoteB = ExtractCode("FootnoteB", "B");
            Codes.FootnoteN = ExtractCode("FootnoteN", "N");
            Codes.LinkFormatURL = ExtractCode("LinkFormatURL", "U");
            Codes.LinkFormatMainTable = ExtractCode("LinkFormatMainTable", "M");
            Codes.LinkTypeTableF = ExtractCode("LinkTypeTableF", "TableF");
            Codes.LinkTypeTableB = ExtractCode("LinkTypeTableB", "TableB");
            Codes.LinkTypeTableRel = ExtractCode("LinkTypeTableRel", "TableRel");
            Codes.LinkTypeTableProc = ExtractCode("LinkTypeTableProc", "TableProc");
            Codes.LinkTypeDok = ExtractCode("LinkTypeDok", "Dok");
            Codes.LinkTypeTableRelEx = ExtractCode("LinkTypeTableRelEx", "TableRelEx");
            Codes.LinkTypeWebsite = ExtractCode("LinkTypeWebsite", "Website");
            Codes.LinkTypeTemasite = ExtractCode("LinkTypeTemasite", "Temasite");
            Codes.LinkTypeAnalys = ExtractCode("LinkTypeAnalys", "Analys");
            Codes.LinkPresDirect = ExtractCode("LinkPresDirect", "D");
            Codes.LinkPresIndirect = ExtractCode("LinkPresIndirect", "I");
            Codes.LinkPresBoth = ExtractCode("LinkPresBoth", "B");
            Codes.PresCategoryInternal = ExtractCode("PresCategoryInternal", "I");
            Codes.PresCategoryPrivate = ExtractCode("PresCategoryPrivate", "P");
            Codes.PresCategoryPublic = ExtractCode("PresCategoryPublic", "O");
            Codes.PresActive = ExtractCode("PresActive", "A");
            Codes.PresPassive = ExtractCode("PresPassive", "P");
            Codes.PresNo = ExtractCode("PresNo", "N");
            Codes.TimeUnitA = ExtractCode("TimeUnitA", "Y");
            Codes.TimeUnitH = ExtractCode("TimeUnitH", "H");
            Codes.TimeUnitQ = ExtractCode("TimeUnitQ", "Q");
            Codes.TimeUnitM = ExtractCode("TimeUnitM", "M");
            Codes.TimeUnitW = ExtractCode("TimeUnitW", "W");
            Codes.ValueTextExistsS = ExtractCode("ValueTextExistsS", "S");
            Codes.ValueTextExistsL = ExtractCode("ValueTextExistsL", "L");
            Codes.ValueTextExistsB = ExtractCode("ValueTextExistsB", "B");
            Codes.ValueTextExistsN = ExtractCode("ValueTextExistsN", "N");
            Codes.ValueTextExistsX = ExtractCode("ValueTextExistsX", "X");
            Codes.ValuePresC = ExtractCode("ValuePresC", "C");
            Codes.ValuePresT = ExtractCode("ValuePresT", "T");
            Codes.ValuePresB = ExtractCode("ValuePresB", "B");
            Codes.ValuePresA = ExtractCode("ValuePresA", "A");
            Codes.ValuePresS = ExtractCode("ValuePresS", "S");
            Codes.ValuePresV = ExtractCode("ValuePresV", "V");
            Codes.EliminationA = ExtractCode("EliminationA", "A");
            Codes.EliminationN = ExtractCode("EliminationN", "N");
            Codes.GroupPresA = ExtractCode("GroupPresA", "A");
            Codes.GroupPresI = ExtractCode("GroupPresI", "I");
            Codes.GroupPresB = ExtractCode("GroupPresB", "B");
            Codes.HierarchyNon = ExtractCode("HierarchyNon", "N");
            Codes.HierarchyBalanced = ExtractCode("HierarchyBalanced", "B");
            Codes.HierarchyUnbalanced = ExtractCode("HierarchyUnbalanced", "U");
            Codes.SpecialSignColumnY = ExtractCode("SpecialSignColumnY", "Y");
            Codes.SpecialSignColumnN = ExtractCode("SpecialSignColumnN", "N");
            Codes.SpecialSignColumnE = ExtractCode("SpecialSignColumnE", "E");
            Codes.Yes = ExtractCode("Yes", "Y");
            Codes.No = ExtractCode("No", "N");
            Codes.StatusAll = ExtractCode("StatusAll", "A");
            Codes.StatusEmpty = ExtractCode("StatusEmpty", "E");
            Codes.StatusNew = ExtractCode("StatusNew", "N");
            Codes.StatusMeta = ExtractCode("StatusMeta", "M");
            Codes.StatusOrder = ExtractCode("StatusOrder", "O");
            Codes.StatusUpdate = ExtractCode("StatusUpdate", "U");
            Codes.StatusEng = ExtractCode("StatusEng", "T");
            Codes.StatusTranslated = ExtractCode("StatusTranslated", "T");
            Codes.RoleHead = ExtractCode("RoleHead", "H");
            Codes.RoleContact = ExtractCode("RoleContact", "C");
            Codes.RoleUpdate = ExtractCode("RoleUpdate", "U");
            Codes.RoleVerify = ExtractCode("RoleVerify", "V");
            Codes.FootnoteShowS = ExtractCode("FootnoteShowS", "S");
            Codes.FootnoteShowP = ExtractCode("FootnoteShowP", "P");
            Codes.FootnoteShowB = ExtractCode("FootnoteShowB", "B");

            #endregion Codes


            #region Keywords

            Keywords = new DbKeywords();

            Keywords.ContentVariable = ExtractKeyword("ContentVariable", "CONTENTVARIABLE");
            Keywords.MenuLevels = ExtractKeyword("MenuLevels", "MENULEVELS");
            Keywords.Macro = ExtractKeyword("Macro", "MACRO");
            Keywords.DataNotAvailable = ExtractKeyword("DataNotAvailable", "DATANOTAVAILABLE");
            Keywords.DataNoteSum = ExtractKeyword("DataNoteSum", "DATANOTESUM");
            Keywords.DataSymbolNIL = ExtractKeyword("DataSymbolNIL", "DATASYMBOLNIL");
            Keywords.DataSymbolSum = ExtractKeyword("DataSymbolSum", "DATASYMBOLSUM");
            Keywords.DefaultCodeMissingLine = ExtractKeyword("DefaultCodeMissingLine", "DEFAULTCODEMISSINGLINE");
            if (FileHasKeyword("AllwaysUseMaintablePrestextSInDynamicTitle"))
            {
                Keywords.Optional_AllwaysUseMaintablePrestextSInDynamicTitle = ExtractKeyword("AllwaysUseMaintablePrestextSInDynamicTitle");
            }
            if (FileHasKeyword("PXCodepage"))
            {
                Keywords.Optional_PXCodepage = ExtractKeyword("PXCodepage");
            }
            if (FileHasKeyword("PXDescriptionDefault"))
            {
                Keywords.Optional_PXDescriptionDefault = ExtractKeyword("PXDescriptionDefault");
            }
            if (FileHasKeyword("PXCharset"))
            {
                Keywords.Optional_PXCharset = ExtractKeyword("PXCharset");
            }
            if (FileHasKeyword("PXAxisVersion"))
            {
                Keywords.Optional_PXAxisVersion = ExtractKeyword("PXAxisVersion");
            }

            #endregion Keywords

        }

        public struct Ccodes
        {
            public String CFPricesC;
            public String CFPricesF;
            public String StockFAS;
            public String StockFAF;
            public String StockFAA;
            public String Copyright1;
            public String Copyright2;
            public String Copyright3;
            public String DataCellFilledOptional;
            public String DataCellFilledValue;
            public String DataCellFilledZero;
            public String VariableTypeC;
            public String VariableTypeT;
            public String VariableTypeG;
            public String FootnoteM;
            public String FootnoteR;
            public String FootnoteB;
            public String FootnoteN;
            public String LinkFormatURL;
            public String LinkFormatMainTable;
            public String LinkTypeTableF;
            public String LinkTypeTableB;
            public String LinkTypeTableRel;
            public String LinkTypeTableProc;
            public String LinkTypeDok;
            public String LinkTypeTableRelEx;
            public String LinkTypeWebsite;
            public String LinkTypeTemasite;
            public String LinkTypeAnalys;
            public String LinkPresDirect;
            public String LinkPresIndirect;
            public String LinkPresBoth;
            public String PresCategoryInternal;
            public String PresCategoryPrivate;
            public String PresCategoryPublic;
            public String PresActive;
            public String PresPassive;
            public String PresNo;
            public String TimeUnitA;
            public String TimeUnitH;
            public String TimeUnitQ;
            public String TimeUnitM;
            public String TimeUnitW;
            public String ValueTextExistsS;
            public String ValueTextExistsL;
            public String ValueTextExistsB;
            public String ValueTextExistsN;
            public String ValueTextExistsX;
            public String ValuePresC;
            public String ValuePresT;
            public String ValuePresB;
            public String ValuePresA;
            public String ValuePresS;
            public String ValuePresV;
            public String EliminationA;
            public String EliminationN;
            public String GroupPresA;
            public String GroupPresI;
            public String GroupPresB;
            public String HierarchyNon;
            public String HierarchyBalanced;
            public String HierarchyUnbalanced;
            public String SpecialSignColumnY;
            public String SpecialSignColumnN;
            public String SpecialSignColumnE;
            public String Yes;
            public String No;
            public String StatusAll;
            public String StatusEmpty;
            public String StatusNew;
            public String StatusMeta;
            public String StatusOrder;
            public String StatusUpdate;
            public String StatusEng;
            public String StatusTranslated;
            public String RoleHead;
            public String RoleContact;
            public String RoleUpdate;
            public String RoleVerify;
            public String FootnoteShowS;
            public String FootnoteShowP;
            public String FootnoteShowB;
        }

        public struct DbKeywords
        {
            public String ContentVariable;
            public String MenuLevels;
            public String Macro;
            public String DataNotAvailable;
            public String DataNoteSum;
            public String DataSymbolNIL;
            public String DataSymbolSum;
            public String DefaultCodeMissingLine;
            public String Optional_AllwaysUseMaintablePrestextSInDynamicTitle;
            public String Optional_PXCodepage;
            public String Optional_PXDescriptionDefault;
            public String Optional_PXCharset;
            public String Optional_PXAxisVersion;
        }
    }
}
