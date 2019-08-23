using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using log4net;

//This code is generated. 

namespace PCAxis.Sql.DbConfig
{
    public class SqlDbConfig_23 : SqlDbConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlDbConfig_23));

        public Ccodes Codes;
        public DbKeywords Keywords;

        #region Fields
        public TblAttribute Attribute;
        public TblAttributeLang2 AttributeLang2;
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
        public TblFootnoteMaintTime FootnoteMaintTime;
        public TblFootnoteMaintValue FootnoteMaintValue;
        public TblFootnoteMenuSel FootnoteMenuSel;
        public TblFootnoteSubTable FootnoteSubTable;
        public TblFootnoteValue FootnoteValue;
        public TblFootnoteValueSetValue FootnoteValueSetValue;
        public TblFootnoteVariable FootnoteVariable;
        public TblGrouping Grouping;
        public TblGroupingLang2 GroupingLang2;
        public TblGroupingLevel GroupingLevel;
        public TblGroupingLevelLang2 GroupingLevelLang2;
        public TblLink Link;
        public TblLinkLang2 LinkLang2;
        public TblLinkMenuSelection LinkMenuSelection;
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
        public TblSecondaryLanguage SecondaryLanguage;
        public TblSpecialCharacter SpecialCharacter;
        public TblSpecialCharacterLang2 SpecialCharacterLang2;
        public TblSubTable SubTable;
        public TblSubTableLang2 SubTableLang2;
        public TblSubTableVariable SubTableVariable;
        public TblTextCatalog TextCatalog;
        public TblTextCatalogLang2 TextCatalogLang2;
        public TblTimeScale TimeScale;
        public TblTimeScaleLang2 TimeScaleLang2;
        public TblVSValue VSValue;
        public TblVSValueLang2 VSValueLang2;
        public TblValue Value;
        public TblValueLang2 ValueLang2;
        public TblValueGroup ValueGroup;
        public TblValueGroupLang2 ValueGroupLang2;
        public TblValuePool ValuePool;
        public TblValuePoolLang2 ValuePoolLang2;
        public TblValueSet ValueSet;
        public TblValueSetLang2 ValueSetLang2;
        public TblValueSetGrouping ValueSetGrouping;
        public TblVariable Variable;
        public TblVariableLang2 VariableLang2;
        #endregion Fields

        private void initStructs()
        {


            Attribute = new TblAttribute(this);

            AttributeLang2 = new TblAttributeLang2(this);

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

            FootnoteMaintTime = new TblFootnoteMaintTime(this);

            FootnoteMaintValue = new TblFootnoteMaintValue(this);

            FootnoteMenuSel = new TblFootnoteMenuSel(this);

            FootnoteSubTable = new TblFootnoteSubTable(this);

            FootnoteValue = new TblFootnoteValue(this);

            FootnoteValueSetValue = new TblFootnoteValueSetValue(this);

            FootnoteVariable = new TblFootnoteVariable(this);

            Grouping = new TblGrouping(this);

            GroupingLang2 = new TblGroupingLang2(this);

            GroupingLevel = new TblGroupingLevel(this);

            GroupingLevelLang2 = new TblGroupingLevelLang2(this);

            Link = new TblLink(this);

            LinkLang2 = new TblLinkLang2(this);

            LinkMenuSelection = new TblLinkMenuSelection(this);

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

            SecondaryLanguage = new TblSecondaryLanguage(this);

            SpecialCharacter = new TblSpecialCharacter(this);

            SpecialCharacterLang2 = new TblSpecialCharacterLang2(this);

            SubTable = new TblSubTable(this);

            SubTableLang2 = new TblSubTableLang2(this);

            SubTableVariable = new TblSubTableVariable(this);

            TextCatalog = new TblTextCatalog(this);

            TextCatalogLang2 = new TblTextCatalogLang2(this);

            TimeScale = new TblTimeScale(this);

            TimeScaleLang2 = new TblTimeScaleLang2(this);

            VSValue = new TblVSValue(this);

            VSValueLang2 = new TblVSValueLang2(this);

            Value = new TblValue(this);

            ValueLang2 = new TblValueLang2(this);

            ValueGroup = new TblValueGroup(this);

            ValueGroupLang2 = new TblValueGroupLang2(this);

            ValuePool = new TblValuePool(this);

            ValuePoolLang2 = new TblValuePoolLang2(this);

            ValueSet = new TblValueSet(this);

            ValueSetLang2 = new TblValueSetLang2(this);

            ValueSetGrouping = new TblValueSetGrouping(this);

            Variable = new TblVariable(this);

            VariableLang2 = new TblVariableLang2(this);
        }

        public SqlDbConfig_23(XmlReader xmlReader, XPathNavigator nav)
        : base(xmlReader, nav)
        {
            log.Debug("SqlDbConfig_23 called");

            this.initStructs();
            this.initCodesAndKeywords();
        }

        #region  structs

         

        /// <summary>
        /// The table contains information on the attribute on the observation values.
        /// 
        /// See further information in the separate document: Attributes in the Nordic SQL Data Model.
        /// </summary>
        public class TblAttribute : Tab
        {

            /// <summary>
            /// Name of the main table to which the attribute is linked.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of the attribute
            /// </summary>
            public Column4Parameterized AttributeCol;
            /// <summary>
            /// Name of the column. The length is set to accommodate future use of prefixes
            /// </summary>
            public Column4Parameterized AttributeColumnCol;
            /// <summary>
            /// Presentation text used by the retrieval interface.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// The attributes place in the data table column or the place within the column. The first attribute for a given main table has the value 1.
            /// </summary>
            public Column4Parameterized SequenceNoCol;
            /// <summary>
            /// Description of the attribute.
            /// </summary>
            public Column4Parameterized DescriptionCol;
            /// <summary>
            /// Name of the value set to which the values are linked.
            /// See further description in table ValueSet.
            /// 
            /// Can be null – for example if the attribute contains a comment.
            /// </summary>
            public Column4Parameterized ValueSetCol;
            /// <summary>
            /// Number of stored characters in the data column.
            /// </summary>
            public Column4Parameterized ColumnLengthCol;

            internal TblAttribute(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("Attribute","ATT"), config.ExtractTableName("Attribute","ATTRIBUTE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Attribute", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Attribute", "Attribute", "ATTRIBUTE");
                this.AttributeCol = new Column4Parameterized(tmpColumnName, this.Alias,"Attribute",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Attribute", "AttributeColumn", "ATTRIBUTECOLUMN");
                this.AttributeColumnCol = new Column4Parameterized(tmpColumnName, this.Alias,"AttributeColumn",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Attribute", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Attribute", "SequenceNo", "SEQUENCENO");
                this.SequenceNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"SequenceNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Attribute", "Description", "DESCRIPTION");
                this.DescriptionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Description",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Attribute", "ValueSet", "VALUESET");
                this.ValueSetCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueSet",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Attribute", "ColumnLength", "COLUMNLENGTH");
                this.ColumnLengthCol = new Column4Parameterized(tmpColumnName, this.Alias,"ColumnLength",config.GetDataProvider());

            }

        }

        public class TblAttributeLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of the main table to which the attribute is linked.
            /// </summary>
            public Lang2Column4Parameterized MainTableCol;

            /// <summary>
            /// Name of the attribute
            /// </summary>
            public Lang2Column4Parameterized AttributeCol;

            /// <summary>
            /// Presentation text used by the retrieval interface.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            /// <summary>
            /// Description of the attribute.
            /// </summary>
            public Lang2Column4Parameterized DescriptionCol;

            internal TblAttributeLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("AttributeLang2","AT2"), config.ExtractTableName("AttributeLang2","ATTRIBUTE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("AttributeLang2", "MainTable"  , "MAINTABLE");
                this.MainTableCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("AttributeLang2", "Attribute"  , "ATTRIBUTE");
                this.AttributeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("AttributeLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("AttributeLang2", "Description"  , "DESCRIPTION");
                this.DescriptionCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table contains information on the content of the data table(s).The content column's name is the same as the name of the corresponding data columns in the data table.
        /// </summary>
        public class TblContents : Tab
        {

            /// <summary>
            /// Name of the main table to which the content columns are linked. See further description in the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of the data columns in the data table.
            /// 
            /// A main table's content columns must have unique names within that main table but the same column name can occur in other main tables.
            /// 
            /// The name should be descriptive, max 20 characters, beginning with a capital letter and should only contain letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
            /// </summary>
            public Column4Parameterized ContentsCol;
            /// <summary>
            /// Presentation text used by the retrieval interface when selecting table data and which, after retrieval of a data table, forms the beginning of the table heading.
            /// 
            /// The presentation text should be unique within a main table.
            /// 
            /// The text should contain information on unit (if not obvious), base time, fixed/ current prices, calendar adjustment and seasonal adjustment.
            /// 
            /// The text should begin with a capital letter, should not contain the % symbol and should not be finished with a full-stop.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Short presentation text for content column. Used in retrieval interface, e.g. when selecting table data, or as the column name when table is presented, if the main table has several contents.
            /// 
            /// The text should begin with a capital letter, should not contain the % symbol and should not end with a full-stop.
            /// 
            /// If there is no text, the field should be NULL.
            /// </summary>
            public Column4Parameterized PresTextSCol;
            /// <summary>
            /// Presentation code.
            /// </summary>
            public Column4Parameterized PresCodeCol;
            /// <summary>
            /// Code for copyright ownership.
            /// 
            /// All content columns within a main table must belong to the same category, i.e. the same code should be in all the fields.
            /// </summary>
            public Column4Parameterized CopyrightCol;
            /// <summary>
            /// Code for the authority responsible for the statistics (statistical authority).
            /// 
            /// All content columns in a main table must belong to the same statistical authority.
            /// 
            /// Data is taken from the column OrganizationCode in the table Organization. For a more detailed description, see that table.
            /// </summary>
            public Column4Parameterized StatAuthorityCol;
            /// <summary>
            /// All content columns in a main table must have the same producer.
            /// 
            /// Data is taken from the column OrganizationCode in the table Organization. For a more detailed description, see that table.
            /// </summary>
            public Column4Parameterized ProducerCol;
            /// <summary>
            /// The date of the most recent update (incl. exact time) of the main table's data tables on the production server. The date remains unchanged when copying over to the external servers. The field is updated automatically by the programs used for loading and updating of data.
            /// 
            /// If there is no text, the field should be NULL.
            /// </summary>
            public Column4Parameterized LastUpdatedCol;
            /// <summary>
            /// The most recent date for the official release of data, i.e. the date and exact time for the transfer of the main table's data tables to the external servers. The field is updated automatically by the program used for the transfer.
            /// 
            /// If there is no text, the field should be NULL.
            /// </summary>
            public Column4Parameterized PublishedCol;
            /// <summary>
            /// Unit, e.g. number, currency, index. The given unit should apply to both the storage and the presentation. Details on unit (if not obvious) should also be written in text to the column PresText in the table Contents.
            /// 
            /// The unit can also be stored in the content column in the data table. This column is always called Unit and can contain different units for different values. In this case, %DataTable is written in the field unit in the table Contents.
            /// </summary>
            public Column4Parameterized UnitCol;
            /// <summary>
            /// The number of decimals that are to be presented in the table presentation when retrievals are made.
            /// </summary>
            public Column4Parameterized PresDecimalsCol;
            /// <summary>
            /// The data table, in principle, only stores cells with values that are not zero. Cells containing zero are only stored if another content column in the same data table has a value other than zero (0) in the corresponding cell. In this field, it should be stated how data cells that are not stored, i.e. cells for which data is missing in all content columns, should be presented.
            /// 
            /// Alternatives:
            /// 
            /// Y = Yes. The cells are assumed to contain zero. Presented as zero (0).
            /// N = No. Data cannot be given.
            /// 
            /// If PresCellsZero is 'N', the field PresMissingLine should be used to indicate how these cells should be presented. See description of PresMissingLine in Contents. See also the description of the table SpecialCharacter.
            /// </summary>
            public Column4Parameterized PresCellsZeroCol;
            /// <summary>
            /// The field is used by the retrieval programs for presenting cells that should not be presented as zero, i.e. when PresCellsZero in Contents is 'N'. The field can either be NULL or contain the identity for a special character. The identity must in that case exist in the column CharacterType in the table SpecialCharacter, and the character must exist in the column PresCharacter in this table. If PresMissingLine is NULL, DefaultCodeMissingLine in MetaAdm is used.
            /// 
            /// See also descriptions of: the column PresCellsZero in the table Contents,  the columns CharacterType and PresCharacter in the table SpecialCharacter and the table MetaAdm.
            /// </summary>
            public Column4Parameterized PresMissingLineCol;
            /// <summary>
            /// Shows if the content can be aggregated or not. Applies to all distributed variables for the content column. There are the following alternatives:
            /// 
            /// Y = Yes
            /// N = No
            /// 
            /// If AggregPossible = N, the possibility to both aggregate and group the data in the retrieval interface is eliminated.
            /// </summary>
            public Column4Parameterized AggregPossibleCol;
            /// <summary>
            /// RefPeriod relates to the time of measurement for the material. Written as text, i.e."31 December of previous year". Data is obligatory for stock material, i.e. when the field StockFA in Contents is "S".
            /// 
            /// If the reference time is not available, the field should be NULL.
            /// </summary>
            public Column4Parameterized RefPeriodCol;
            /// <summary>
            /// Shows whether the statistical material is of the type stock, flow or average. There are the following alternatives:
            /// 
            /// F = Flow. Measurement time refers to a specific period. The result describes events that occurred successively during the measurement period.
            /// A = Average. The result is made up of an average value of observation values at different measurement times.
            /// S = Stock. The measurement time refers to a specific point in time.
            /// X = Other
            /// </summary>
            public Column4Parameterized StockFACol;
            /// <summary>
            /// The base period when calculating an index or fixed prices, for example. In cases where data are not relevant, the field should be NULL.
            /// </summary>
            public Column4Parameterized BasePeriodCol;
            /// <summary>
            /// Current/fixed prices. Alternatives:
            /// F = Fixed prices.
            /// C = Current prices.
            /// 
            /// In cases where data are not relevant, the field should be NULL.
            /// </summary>
            public Column4Parameterized CFPricesCol;
            /// <summary>
            /// Shows whether the statistical material is calendar adjusted or not during the measurement period. There are the following alternatives:
            /// 
            /// Y = Yes
            /// N = No
            /// </summary>
            public Column4Parameterized DayAdjCol;
            /// <summary>
            /// Shows whether the statistical material is seasonally adjusted or not, i.e. adjusted for different periodical variations during the measurement period that may have affected the result. There are the following alternatives:
            /// 
            /// Y = Yes
            /// N = No
            /// </summary>
            public Column4Parameterized SeasAdjCol;
            /// <summary>
            /// Here the content column's (data columns) order of storage amongst themselves in the data table is shown.
            /// </summary>
            public Column4Parameterized StoreColumnNoCol;
            /// <summary>
            /// Specifies  the storage format for data cells for the content column. There are the following alternatives:
            /// C = Varchar. Can only be used to store unit information in the data tables. There must exist a column with name unit and the filed Contents.Unit should be set to ‘%Datatable’
            /// I = Integer. For numbers of size 2 147 483 647 to - 2 147 483 648.
            /// N = Numeric. For larger numbers and always when the material is stored with decimals.
            /// S = Smallint. For numbers of size 32 767 to - 32 768.
            /// 
            /// Also see the description of column StoreNoChar.
            /// </summary>
            public Column4Parameterized StoreFormatCol;
            /// <summary>
            /// Number of stored characters in the data column. Number of characters should be:
            /// 
            /// 2, if StoreFormat = S,
            /// 4, if StoreFormat = I,
            /// 2-17 (decimals included, decimal point excluded), if StoreFormat = N
            /// 1-30 characters, if StoreFormat = C.
            /// 
            /// Also see the description of column StoreFormat in the table Contents.
            /// </summary>
            public Column4Parameterized StoreNoCharCol;
            /// <summary>
            /// Number of stored decimals (0-15).
            /// 
            /// Data should be included in StoreNoChar if StoreFormat = N.
            /// </summary>
            public Column4Parameterized StoreDecimalsCol;
            /// <summary>
            /// MetaId can be used to link the information in this table to an external system.
            /// </summary>
            public Column4Parameterized MetaIdCol;

            internal TblContents(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("Contents","CNT"), config.ExtractTableName("Contents","CONTENTS"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Contents", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "Contents", "CONTENTS");
                this.ContentsCol = new Column4Parameterized(tmpColumnName, this.Alias,"Contents",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "PresTextS", "PRESTEXTS");
                this.PresTextSCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresTextS",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "PresCode", "PRESCODE");
                this.PresCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "Copyright", "COPYRIGHT");
                this.CopyrightCol = new Column4Parameterized(tmpColumnName, this.Alias,"Copyright",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "StatAuthority", "STATAUTHORITY");
                this.StatAuthorityCol = new Column4Parameterized(tmpColumnName, this.Alias,"StatAuthority",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "Producer", "PRODUCER");
                this.ProducerCol = new Column4Parameterized(tmpColumnName, this.Alias,"Producer",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "LastUpdated", "LASTUPDATED");
                this.LastUpdatedCol = new Column4Parameterized(tmpColumnName, this.Alias,"LastUpdated",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "Published", "PUBLISHED");
                this.PublishedCol = new Column4Parameterized(tmpColumnName, this.Alias,"Published",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "Unit", "UNIT");
                this.UnitCol = new Column4Parameterized(tmpColumnName, this.Alias,"Unit",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "PresDecimals", "PRESDECIMALS");
                this.PresDecimalsCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresDecimals",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "PresCellsZero", "PRESCELLSZERO");
                this.PresCellsZeroCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresCellsZero",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "PresMissingLine", "PRESMISSINGLINE");
                this.PresMissingLineCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresMissingLine",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "AggregPossible", "AGGREGPOSSIBLE");
                this.AggregPossibleCol = new Column4Parameterized(tmpColumnName, this.Alias,"AggregPossible",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "RefPeriod", "REFPERIOD");
                this.RefPeriodCol = new Column4Parameterized(tmpColumnName, this.Alias,"RefPeriod",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "StockFA", "STOCKFA");
                this.StockFACol = new Column4Parameterized(tmpColumnName, this.Alias,"StockFA",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "BasePeriod", "BASEPERIOD");
                this.BasePeriodCol = new Column4Parameterized(tmpColumnName, this.Alias,"BasePeriod",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "CFPrices", "CFPRICES");
                this.CFPricesCol = new Column4Parameterized(tmpColumnName, this.Alias,"CFPrices",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "DayAdj", "DAYADJ");
                this.DayAdjCol = new Column4Parameterized(tmpColumnName, this.Alias,"DayAdj",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "SeasAdj", "SEASADJ");
                this.SeasAdjCol = new Column4Parameterized(tmpColumnName, this.Alias,"SeasAdj",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "StoreColumnNo", "STORECOLUMNNO");
                this.StoreColumnNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"StoreColumnNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "StoreFormat", "STOREFORMAT");
                this.StoreFormatCol = new Column4Parameterized(tmpColumnName, this.Alias,"StoreFormat",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "StoreNoChar", "STORENOCHAR");
                this.StoreNoCharCol = new Column4Parameterized(tmpColumnName, this.Alias,"StoreNoChar",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "StoreDecimals", "STOREDECIMALS");
                this.StoreDecimalsCol = new Column4Parameterized(tmpColumnName, this.Alias,"StoreDecimals",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Contents", "MetaId", "METAID");
                this.MetaIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"MetaId",config.GetDataProvider());

            }

        }

        public class TblContentsLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of the main table to which the content columns are linked. See further description in the table MainTable.
            /// </summary>
            public Lang2Column4Parameterized MainTableCol;

            /// <summary>
            /// Name of the data columns in the data table.
            /// 
            /// A main table's content columns must have unique names within that main table but the same column name can occur in other main tables.
            /// 
            /// The name should be descriptive, max 20 characters, beginning with a capital letter and should only contain letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
            /// </summary>
            public Lang2Column4Parameterized ContentsCol;

            /// <summary>
            /// Presentation text used by the retrieval interface when selecting table data and which, after retrieval of a data table, forms the beginning of the table heading.
            /// 
            /// The presentation text should be unique within a main table.
            /// 
            /// The text should contain information on unit (if not obvious), base time, fixed/ current prices, calendar adjustment and seasonal adjustment.
            /// 
            /// The text should begin with a capital letter, should not contain the % symbol and should not be finished with a full-stop.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            /// <summary>
            /// Short presentation text for content column. Used in retrieval interface, e.g. when selecting table data, or as the column name when table is presented, if the main table has several contents.
            /// 
            /// The text should begin with a capital letter, should not contain the % symbol and should not end with a full-stop.
            /// 
            /// If there is no text, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized PresTextSCol;

            /// <summary>
            /// Unit, e.g. number, currency, index. The given unit should apply to both the storage and the presentation. Details on unit (if not obvious) should also be written in text to the column PresText in the table Contents.
            /// 
            /// The unit can also be stored in the content column in the data table. This column is always called Unit and can contain different units for different values. In this case, %DataTable is written in the field unit in the table Contents.
            /// </summary>
            public Lang2Column4Parameterized UnitCol;

            /// <summary>
            /// RefPeriod relates to the time of measurement for the material. Written as text, i.e."31 December of previous year". Data is obligatory for stock material, i.e. when the field StockFA in Contents is "S".
            /// 
            /// If the reference time is not available, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized RefPeriodCol;

            /// <summary>
            /// The base period when calculating an index or fixed prices, for example. In cases where data are not relevant, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized BasePeriodCol;

            internal TblContentsLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ContentsLang2","CN2"), config.ExtractTableName("ContentsLang2","CONTENTS_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "MainTable"  , "MAINTABLE");
                this.MainTableCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "Contents"  , "CONTENTS");
                this.ContentsCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "PresTextS"  , "PRESTEXTS");
                this.PresTextSCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "Unit"  , "UNIT");
                this.UnitCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "RefPeriod"  , "REFPERIOD");
                this.RefPeriodCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ContentsLang2", "BasePeriod"  , "BASEPERIOD");
                this.BasePeriodCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
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
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// The name of the content column that the point in time is linked to.
            /// See description in the table Contents.
            /// </summary>
            public Column4Parameterized ContentsCol;
            /// <summary>
            /// Code for point in time, i.e. 2003, 2003Q1, 2003M01.
            /// 
            /// Rules for how codes should be constructed are available in the column StoreFormat in the table TimeScale.
            /// </summary>
            public Column4Parameterized TimePeriodCol;

            internal TblContentsTime(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ContentsTime","CTM"), config.ExtractTableName("ContentsTime","CONTENTSTIME"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ContentsTime", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ContentsTime", "Contents", "CONTENTS");
                this.ContentsCol = new Column4Parameterized(tmpColumnName, this.Alias,"Contents",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ContentsTime", "TimePeriod", "TIMEPERIOD");
                this.TimePeriodCol = new Column4Parameterized(tmpColumnName, this.Alias,"TimePeriod",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table contains information on which database the data tables for the statistical product are stored in.
        /// </summary>
        public class TblDataStorage : Tab
        {

            /// <summary>
            /// Unique identifier for at product group.
            /// </summary>
            public Column4Parameterized ProductCodeCol;
            /// <summary>
            /// Name of the server where the database is situated.
            /// </summary>
            public Column4Parameterized ServerNameCol;
            /// <summary>
            /// Name of the database where the product's data tables are stored.
            /// </summary>
            public Column4Parameterized DatabaseNameCol;

            internal TblDataStorage(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("DataStorage","DST"), config.ExtractTableName("DataStorage","DATASTORAGE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("DataStorage", "ProductCode", "PRODUCTCODE");
                this.ProductCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"ProductCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("DataStorage", "ServerName", "SERVERNAME");
                this.ServerNameCol = new Column4Parameterized(tmpColumnName, this.Alias,"ServerName",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("DataStorage", "DatabaseName", "DATABASENAME");
                this.DatabaseNameCol = new Column4Parameterized(tmpColumnName, this.Alias,"DatabaseName",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table contains footnote texts and information on footnotes.
        /// </summary>
        public class TblFootnote : Tab
        {

            /// <summary>
            /// Serial number given automatically by the system. The most recently used footnote number is stored in the table MetaAdm.
            /// 
            /// See further in the description of the table MetaAdm.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;
            /// <summary>
            /// Code for the type of footnote. There are the following alternatives:
            /// 
            /// 1 = footnote on subject area
            /// 2 = footnote on content column
            /// 3 = footnote on variable + content column
            /// 4 = footnote on value/time + content column
            /// 5 = footnote on variable
            /// 6 = footnote on value
            /// 7 = footnote on main table
            /// 8 = footnote on sub-table
            /// 9 = footnote on value + main table
            /// A = footnote on statistical area (level 2)
            /// B = footnote on product (level 3)
            /// C = footnote on table group (level 4)
            /// Q = footnote on grouping
            /// </summary>
            public Column4Parameterized FootnoteTypeCol;
            /// <summary>
            /// Contains information on when the footnote should be shown in the outdata program, i.e. when content is selected for a table, when the table is presented or both.
            /// There are the following alternatives:
            /// 
            /// B = show both at selection and presentation
            /// P = show at presentation
            /// S = shown upon selection
            /// </summary>
            public Column4Parameterized ShowFootnoteCol;
            /// <summary>
            /// Code for whether the footnote is classified as "optional" or "mandatory".
            /// Alternatives:
            /// 
            /// O = optional
            /// M = mandatory
            /// </summary>
            public Column4Parameterized MandOptCol;
            /// <summary>
            /// Text in the footnote. Written as consecutive text, starting with a capital letter.
            /// 
            /// NB! Double quotation marks should not be used as this causes problems in PC-AXIS.
            /// </summary>
            public Column4Parameterized FootnoteTextCol;
            /// <summary>
            /// Special character or special characters to be associated with the footnote
            /// </summary>
            public Column4Parameterized PresCharacterCol;

            internal TblFootnote(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("Footnote","FNT"), config.ExtractTableName("Footnote","FOOTNOTE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Footnote", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Footnote", "FootnoteType", "FOOTNOTETYPE");
                this.FootnoteTypeCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteType",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Footnote", "ShowFootnote", "SHOWFOOTNOTE");
                this.ShowFootnoteCol = new Column4Parameterized(tmpColumnName, this.Alias,"ShowFootnote",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Footnote", "MandOpt", "MANDOPT");
                this.MandOptCol = new Column4Parameterized(tmpColumnName, this.Alias,"MandOpt",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Footnote", "FootnoteText", "FOOTNOTETEXT");
                this.FootnoteTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Footnote", "PresCharacter", "PRESCHARACTER");
                this.PresCharacterCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresCharacter",config.GetDataProvider());

            }

        }

        public class TblFootnoteLang2 : Lang2Tab
        {

            /// <summary>
            /// Serial number given automatically by the system. The most recently used footnote number is stored in the table MetaAdm.
            /// 
            /// See further in the description of the table MetaAdm.
            /// </summary>
            public Lang2Column4Parameterized FootnoteNoCol;

            /// <summary>
            /// Text in the footnote. Written as consecutive text, starting with a capital letter.
            /// 
            /// NB! Double quotation marks should not be used as this causes problems in PC-AXIS.
            /// </summary>
            public Lang2Column4Parameterized FootnoteTextCol;

            internal TblFootnoteLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteLang2","FN2"), config.ExtractTableName("FootnoteLang2","FOOTNOTE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteLang2", "FootnoteNo"  , "FOOTNOTENO");
                this.FootnoteNoCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("FootnoteLang2", "FootnoteText"  , "FOOTNOTETEXT");
                this.FootnoteTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table links footnotes to a point in time for a specific content column.
        /// </summary>
        public class TblFootnoteContTime : Tab
        {

            /// <summary>
            /// Name of main table.
            /// 
            /// See further in the description of the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of content column.
            /// 
            /// See further in the description of the table Contents.
            /// </summary>
            public Column4Parameterized ContentsCol;
            /// <summary>
            /// Point in time that the footnote relates to.
            /// 
            /// See descriptions in table TimeScale and ContentsTime.
            /// </summary>
            public Column4Parameterized TimePeriodCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;
            /// <summary>
            /// State whether the footnote is a cell footnote. A cell footnote is defined as a footnote that exists for at least two of the content column's variables, e.g. Region and Time or Region and Sex.
            /// Alternatives:
            /// Y = Yes, the footnote is a cell footnote
            /// N = No
            /// </summary>
            public Column4Parameterized CellnoteCol;

            internal TblFootnoteContTime(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteContTime","FCT"), config.ExtractTableName("FootnoteContTime","FOOTNOTECONTTIME"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "Contents", "CONTENTS");
                this.ContentsCol = new Column4Parameterized(tmpColumnName, this.Alias,"Contents",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "TimePeriod", "TIMEPERIOD");
                this.TimePeriodCol = new Column4Parameterized(tmpColumnName, this.Alias,"TimePeriod",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContTime", "Cellnote", "CELLNOTE");
                this.CellnoteCol = new Column4Parameterized(tmpColumnName, this.Alias,"Cellnote",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a value for a specific content column.
        /// </summary>
        public class TblFootnoteContValue : Tab
        {

            /// <summary>
            /// Name of main table.
            /// 
            /// See further in the description of the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of content column.
            /// 
            /// See further in the description of the table Contents.
            /// </summary>
            public Column4Parameterized ContentsCol;
            /// <summary>
            /// Name of variable.
            /// 
            /// See further in the description of the table Variable.
            /// </summary>
            public Column4Parameterized VariableCol;
            /// <summary>
            /// Name of value pool.
            /// 
            /// See further in the description of the table ValuePool.
            /// </summary>
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Code for the value that the footnote relates to.
            /// 
            /// See further in the description of the table Value.
            /// </summary>
            public Column4Parameterized ValueCodeCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;
            /// <summary>
            /// State whether the footnote is a cell footnote. A cell footnote is defined as a footnote that exists for at least two of the content column's variables, e.g. Region and Time or Region and Sex.
            /// Alternatives:
            /// Y = Yes, the footnote is a cell footnote
            /// N = No
            /// </summary>
            public Column4Parameterized CellnoteCol;

            internal TblFootnoteContValue(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteContValue","FCA"), config.ExtractTableName("FootnoteContValue","FOOTNOTECONTVALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "Contents", "CONTENTS");
                this.ContentsCol = new Column4Parameterized(tmpColumnName, this.Alias,"Contents",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "Variable", "VARIABLE");
                this.VariableCol = new Column4Parameterized(tmpColumnName, this.Alias,"Variable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContValue", "Cellnote", "CELLNOTE");
                this.CellnoteCol = new Column4Parameterized(tmpColumnName, this.Alias,"Cellnote",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a variable for a specific content column.
        /// </summary>
        public class TblFootnoteContVbl : Tab
        {

            /// <summary>
            /// Name of main table.
            /// 
            /// See further in the description of the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of content column.
            /// 
            /// See further in the description of the table Contents.
            /// </summary>
            public Column4Parameterized ContentsCol;
            /// <summary>
            /// Name of variable.
            /// 
            /// See further in the description of the table Variable.
            /// </summary>
            public Column4Parameterized VariableCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteContVbl(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteContVbl","FCB"), config.ExtractTableName("FootnoteContVbl","FOOTNOTECONTVBL"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteContVbl", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContVbl", "Contents", "CONTENTS");
                this.ContentsCol = new Column4Parameterized(tmpColumnName, this.Alias,"Contents",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContVbl", "Variable", "VARIABLE");
                this.VariableCol = new Column4Parameterized(tmpColumnName, this.Alias,"Variable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContVbl", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a content column.
        /// </summary>
        public class TblFootnoteContents : Tab
        {

            /// <summary>
            /// Name of main table.
            /// 
            /// See further in the description of the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of content column.
            /// 
            /// See further in the description of the table Contents.
            /// </summary>
            public Column4Parameterized ContentsCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteContents(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteContents","FCO"), config.ExtractTableName("FootnoteContents","FOOTNOTECONTENTS"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteContents", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContents", "Contents", "CONTENTS");
                this.ContentsCol = new Column4Parameterized(tmpColumnName, this.Alias,"Contents",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteContents", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a grouping.
        /// </summary>
        public class TblFootnoteGrouping : Tab
        {

            /// <summary>
            /// Name of groupong
            /// See further in the description of the table Gruoping.
            /// </summary>
            public Column4Parameterized GroupingCol;
            /// <summary>
            /// Number of the footnote.
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteGrouping(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteGrouping","FCO"), config.ExtractTableName("FootnoteGrouping","FOOTNOTEGROUPING"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteGrouping", "Grouping", "GROUPING");
                this.GroupingCol = new Column4Parameterized(tmpColumnName, this.Alias,"Grouping",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteGrouping", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a main table.
        /// 
        /// N.B this table can only be used as long as all contents of the main table has the same connections in the ContentsTime table. Footnotes that are linked in this way should have type = 9. This is the same type that is used for notes with FootnoteMaintValue. By using that type the footnote can be specified so that it is valid for the intersection of more than one column thereby assigning the footnote to a subset of the data.
        /// </summary>
        public class TblFootnoteMainTable : Tab
        {

            /// <summary>
            /// Name of main table.
            /// 
            /// See further in the description of the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteMainTable(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteMainTable","FMT"), config.ExtractTableName("FootnoteMainTable","FOOTNOTEMAINTABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteMainTable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteMainTable", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// Footnote for points in time or timeperiods for a main table.
        /// 
        /// N.B this table can only be used as long as all contents of the main table has the same connections in the ContentsTime table. Footnotes that are linked in this way should have type = 9. This is the same type that is used for notes with FootnoteMaintValue. By using that type the footnote can be specified so that it is valid for the intersection of more than one column thereby assigning the footnote to a subset of the data.
        /// </summary>
        public class TblFootnoteMaintTime : Tab
        {

            /// <summary>
            /// Name of main table
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Timeperiod
            /// </summary>
            public Column4Parameterized TimePeriodCol;
            /// <summary>
            /// Footnote number
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteMaintTime(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteMaintTime","FNM"), config.ExtractTableName("FootnoteMaintTime","FOOTNOTEMAINTTIME"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteMaintTime", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteMaintTime", "TimePeriod", "TIMEPERIOD");
                this.TimePeriodCol = new Column4Parameterized(tmpColumnName, this.Alias,"TimePeriod",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteMaintTime", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a value for a specific content column.
        /// </summary>
        public class TblFootnoteMaintValue : Tab
        {

            /// <summary>
            /// Name of main table.
            /// 
            /// See further in the description of the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of variable.
            /// 
            /// See further in the description of the table Variable.
            /// </summary>
            public Column4Parameterized VariableCol;
            /// <summary>
            /// Name of value pool.
            /// 
            /// See further in the description of the table ValuePool.
            /// </summary>
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Code for the value that the footnote relates to.
            /// 
            /// See further in the description of the table Value.
            /// </summary>
            public Column4Parameterized ValueCodeCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteMaintValue(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteMaintValue","FMV"), config.ExtractTableName("FootnoteMaintValue","FOOTNOTEMAINTVALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "Variable", "VARIABLE");
                this.VariableCol = new Column4Parameterized(tmpColumnName, this.Alias,"Variable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteMaintValue", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to all levels above the MainTable.
        /// </summary>
        public class TblFootnoteMenuSel : Tab
        {

            /// <summary>
            /// Code for relevant menu level.
            /// 
            /// See further in the description of the table MenuSelection.
            /// </summary>
            public Column4Parameterized MenuCol;
            /// <summary>
            /// Code for the nearest underlying eligible alternative in the relevant menu level.
            /// </summary>
            public Column4Parameterized SelectionCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteMenuSel(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteMenuSel","FMS"), config.ExtractTableName("FootnoteMenuSel","FOOTNOTEMENUSEL"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteMenuSel", "Menu", "MENU");
                this.MenuCol = new Column4Parameterized(tmpColumnName, this.Alias,"Menu",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteMenuSel", "Selection", "SELECTION");
                this.SelectionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Selection",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteMenuSel", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a main table.
        /// </summary>
        public class TblFootnoteSubTable : Tab
        {

            /// <summary>
            /// Name of main table.
            /// 
            /// See further in the description of the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of sub-table
            /// 
            /// See further in the description of the table SubTable.
            /// </summary>
            public Column4Parameterized SubTableCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteSubTable(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteSubTable","FST"), config.ExtractTableName("FootnoteSubTable","FOOTNOTESUBTABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteSubTable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteSubTable", "SubTable", "SUBTABLE");
                this.SubTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"SubTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteSubTable", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a value.
        /// </summary>
        public class TblFootnoteValue : Tab
        {

            /// <summary>
            /// Name of value pool.
            /// 
            /// See further in the description of the table ValuePool.
            /// </summary>
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Code for the value that the footnote relates to.
            /// 
            /// See further in the description of the table Value.
            /// </summary>
            public Column4Parameterized ValueCodeCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteValue(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteValue","FVL"), config.ExtractTableName("FootnoteValue","FOOTNOTEVALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteValue", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteValue", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteValue", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a valueset.
        /// </summary>
        public class TblFootnoteValueSetValue : Tab
        {

            /// <summary>
            /// Name of value pool.
            /// 
            /// See further in the description of the table ValuePool.
            /// </summary>
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Name of value set.
            /// 
            /// See further in the description of the table ValueSet.
            /// </summary>
            public Column4Parameterized ValueSetCol;
            /// <summary>
            /// Code for the value that the footnote relates to.
            /// 
            /// See further in the description of the table Value.
            /// </summary>
            public Column4Parameterized ValueCodeCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteValueSetValue(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteValueSetValue","FVS"), config.ExtractTableName("FootnoteValueSetValue","FOOTNOTEVALUESETVALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteValueSetValue", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteValueSetValue", "ValueSet", "VALUESET");
                this.ValueSetCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueSet",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteValueSetValue", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteValueSetValue", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links footnotes to a variable.
        /// </summary>
        public class TblFootnoteVariable : Tab
        {

            /// <summary>
            /// Name of the variable that the footnote relates to.
            /// 
            /// See further in the description of the table Variable.
            /// </summary>
            public Column4Parameterized VariableCol;
            /// <summary>
            /// Number of the footnote.
            /// 
            /// See further in the description of the table Footnote.
            /// </summary>
            public Column4Parameterized FootnoteNoCol;

            internal TblFootnoteVariable(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("FootnoteVariable","FVB"), config.ExtractTableName("FootnoteVariable","FOOTNOTEVARIABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("FootnoteVariable", "Variable", "VARIABLE");
                this.VariableCol = new Column4Parameterized(tmpColumnName, this.Alias,"Variable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("FootnoteVariable", "FootnoteNo", "FOOTNOTENO");
                this.FootnoteNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FootnoteNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table describes the groupings which exist in the database. It is used for the grouping of values for presentation purposes.
        /// </summary>
        public class TblGrouping : Tab
        {

            /// <summary>
            /// Name of grouping.
            /// 
            /// The name should consist of the name of the value pool that the grouping is linked to + a suffix. The suffix should always be used, even if there is only one grouping for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.
            /// 
            /// The name is written beginning with a capital letter.
            /// </summary>
            public Column4Parameterized GroupingCol;
            /// <summary>
            /// Name of the value pool that the grouping belongs to.
            /// 
            /// See further in the description of the table Organization.
            /// </summary>
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Presentation text for grouping.
            /// 
            /// Used in the retrieval interface when selecting a grouping under "Classification". The text should also be able to be used in PC-AXIS as a replacement for the usual variable text, when retrieving grouped material, in the stub or heading and in the title when presenting a table.
            /// 
            /// The text should be short and descriptive and begin with a capital letter.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Shows if the grouping is hierarchic or not. Can be:
            /// 
            /// N = No
            /// B = Balanced
            /// U = Unbalanced
            /// 
            /// For non-hierarchic groupings Hierarchy should always be N.
            /// In a balanced hierarchy all branches are the same length, i.e. with the same number of levels. In an unbalanced hierarchy the number of levels and the length of the levels can vary within the hierarchy.
            /// </summary>
            public Column4Parameterized HierarchyCol;
            /// <summary>
            /// Sorting code to enable the presentation of the groupings within a value pool in a logical order.
            /// 
            /// If there is no sorting code, the field should be NULL.
            /// </summary>
            public Column4Parameterized SortCodeCol;
            /// <summary>
            /// Code which indicates how a grouping should be presented, as an aggregated value, integral value or both. There are the following alternatives:
            /// 
            /// A = aggregated value should be shown
            /// I = integral value should be shown
            /// B = both aggregated and integral values should be shown .
            /// </summary>
            public Column4Parameterized GroupPresCol;
            /// <summary>
            /// Description of grouping. Should give an idea of how the grouping has been put together.
            /// 
            /// Written beginning with a capital letter.
            /// If a description is not available, the field should be NULL.
            /// </summary>
            public Column4Parameterized DescriptionCol;
            /// <summary>
            /// MetaId can be used to link the information in this table to an external system.
            /// </summary>
            public Column4Parameterized MetaIdCol;

            internal TblGrouping(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("Grouping","GRP"), config.ExtractTableName("Grouping","GROUPING"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Grouping", "Grouping", "GROUPING");
                this.GroupingCol = new Column4Parameterized(tmpColumnName, this.Alias,"Grouping",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Grouping", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Grouping", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Grouping", "Hierarchy", "HIERARCHY");
                this.HierarchyCol = new Column4Parameterized(tmpColumnName, this.Alias,"Hierarchy",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Grouping", "SortCode", "SORTCODE");
                this.SortCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"SortCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Grouping", "GroupPres", "GROUPPRES");
                this.GroupPresCol = new Column4Parameterized(tmpColumnName, this.Alias,"GroupPres",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Grouping", "Description", "DESCRIPTION");
                this.DescriptionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Description",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Grouping", "MetaId", "METAID");
                this.MetaIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"MetaId",config.GetDataProvider());

            }

        }

        public class TblGroupingLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of grouping.
            /// 
            /// The name should consist of the name of the value pool that the grouping is linked to + a suffix. The suffix should always be used, even if there is only one grouping for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.
            /// 
            /// The name is written beginning with a capital letter.
            /// </summary>
            public Lang2Column4Parameterized GroupingCol;

            /// <summary>
            /// Presentation text for grouping.
            /// 
            /// Used in the retrieval interface when selecting a grouping under "Classification". The text should also be able to be used in PC-AXIS as a replacement for the usual variable text, when retrieving grouped material, in the stub or heading and in the title when presenting a table.
            /// 
            /// The text should be short and descriptive and begin with a capital letter.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            /// <summary>
            /// Sorting code to enable the presentation of the groupings within a value pool in a logical order.
            /// 
            /// If there is no sorting code, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized SortCodeCol;

            internal TblGroupingLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("GroupingLang2","GR2"), config.ExtractTableName("GroupingLang2","GROUPING_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("GroupingLang2", "Grouping"  , "GROUPING");
                this.GroupingCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("GroupingLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("GroupingLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table describes the levels within a grouping.
        /// 
        /// The table has to exist for both hierarchical and non-hierarchical groupings, but does not have to be used for the non-hierarchical.
        /// </summary>
        public class TblGroupingLevel : Tab
        {

            /// <summary>
            /// Name of grouping.
            /// 
            /// See further in the description of the table Grouping.
            /// </summary>
            public Column4Parameterized GroupingCol;
            /// <summary>
            /// Number for sorting a level within a grouping. The highest level should always be 1.
            /// </summary>
            public Column4Parameterized LevelNoCol;
            /// <summary>
            /// The name of the level.
            /// </summary>
            public Column4Parameterized LevelTextCol;
            /// <summary>
            /// Should contain the identification of a map that is suitable for the variable and the grouping. The field must be filled in if the column VariableType in the table SubTableVariable = G, otherwise the field is NULL.
            /// 
            /// The identification number should also be included in the table TextCatalog. For further information see description of TextCatalog.
            /// </summary>
            public Column4Parameterized GeoAreaNoCol;

            internal TblGroupingLevel(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("GroupingLevel","GRP"), config.ExtractTableName("GroupingLevel","GROUPINGLEVEL"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("GroupingLevel", "Grouping", "GROUPING");
                this.GroupingCol = new Column4Parameterized(tmpColumnName, this.Alias,"Grouping",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("GroupingLevel", "LevelNo", "LEVELNO");
                this.LevelNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"LevelNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("GroupingLevel", "LevelText", "LEVELTEXT");
                this.LevelTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"LevelText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("GroupingLevel", "GeoAreaNo", "GEOAREANO");
                this.GeoAreaNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"GeoAreaNo",config.GetDataProvider());

            }

        }

        public class TblGroupingLevelLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of grouping.
            /// 
            /// See further in the description of the table Grouping.
            /// </summary>
            public Lang2Column4Parameterized GroupingCol;

            /// <summary>
            /// Number for sorting a level within a grouping. The highest level should always be 1.
            /// </summary>
            public Lang2Column4Parameterized LevelNoCol;

            /// <summary>
            /// The name of the level.
            /// </summary>
            public Lang2Column4Parameterized LevelTextCol;

            internal TblGroupingLevelLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("GroupingLevelLang2","GR2"), config.ExtractTableName("GroupingLevelLang2","GROUPINGLEVEL_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("GroupingLevelLang2", "Grouping"  , "GROUPING");
                this.GroupingCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("GroupingLevelLang2", "LevelNo"  , "LEVELNO");
                this.LevelNoCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("GroupingLevelLang2", "LevelText"  , "LEVELTEXT");
                this.LevelTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
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
            public Column4Parameterized LinkIdCol;
            /// <summary>
            /// Link written in HTML.
            /// </summary>
            public Column4Parameterized LinkCol;
            /// <summary>
            /// Describes the type of link. There are the following alternatives:
            /// 
            /// TableF = table ahead in the same database
            /// TableB = previous table in the same database
            /// TableRel = related table in the same database
            /// TableProc = table for percentage calculation
            /// Dok = documentation (internal or external)
            /// TableRelEx = related table in other countries
            /// Website = related web site
            /// Temasite = theme web site
            /// Analys = analysis
            /// 
            /// For the four first above mentioned alternatives the column LinkFormat should be M. In other cases LinkFormat should be U.
            /// </summary>
            public Column4Parameterized LinkTypeCol;
            /// <summary>
            /// Indicates whether the link is an Internet address outside databases (a URL) or a link to a master table in the database. The options are:
            /// 
            /// U = URL
            /// M = maintable
            /// </summary>
            public Column4Parameterized LinkFormatCol;
            /// <summary>
            /// Presentation text for the link, i.e. the text that is visible for the user in the Internet interface.
            /// </summary>
            public Column4Parameterized LinkTextCol;
            /// <summary>
            /// Presentation category for the link. There are three alternatives:
            /// 
            /// O = Public, i.e. link accessible for all users
            /// I = Internal, i.e. table is only accessible for internal users.
            /// P = Private, i.e. table is only accessible for certain internal users.
            /// 
            /// The code is always written with capital letters.
            /// </summary>
            public Column4Parameterized PresCategoryCol;
            /// <summary>
            /// Shows how the link should be presented in the Internet interface. There are three alternatives:
            /// 
            /// D = Presented directly
            /// I = Presented under an icon or similar
            /// B = Presented both directly and under an icon
            /// </summary>
            public Column4Parameterized LinkPresCol;
            /// <summary>
            /// Sorting code for the link. Makes it possible to dictate the order in which the links are presented for users in the Internet interface.
            /// If there is no sorting code, the field should be NULL.
            /// </summary>
            public Column4Parameterized SortCodeCol;
            /// <summary>
            /// Description of link.
            /// If a description is not available, the field should be NULL.
            /// </summary>
            public Column4Parameterized DescriptionCol;

            internal TblLink(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("Link","LNK"), config.ExtractTableName("Link","LINK"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Link", "LinkId", "LINKID");
                this.LinkIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"LinkId",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Link", "Link", "LINK");
                this.LinkCol = new Column4Parameterized(tmpColumnName, this.Alias,"Link",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Link", "LinkType", "LINKTYPE");
                this.LinkTypeCol = new Column4Parameterized(tmpColumnName, this.Alias,"LinkType",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Link", "LinkFormat", "LINKFORMAT");
                this.LinkFormatCol = new Column4Parameterized(tmpColumnName, this.Alias,"LinkFormat",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Link", "LinkText", "LINKTEXT");
                this.LinkTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"LinkText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Link", "PresCategory", "PRESCATEGORY");
                this.PresCategoryCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresCategory",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Link", "LinkPres", "LINKPRES");
                this.LinkPresCol = new Column4Parameterized(tmpColumnName, this.Alias,"LinkPres",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Link", "SortCode", "SORTCODE");
                this.SortCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"SortCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Link", "Description", "DESCRIPTION");
                this.DescriptionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Description",config.GetDataProvider());

            }

        }

        public class TblLinkLang2 : Lang2Tab
        {

            /// <summary>
            /// Identifying code for the link.
            /// </summary>
            public Lang2Column4Parameterized LinkIdCol;

            /// <summary>
            /// Link written in HTML.
            /// </summary>
            public Lang2Column4Parameterized LinkCol;

            /// <summary>
            /// Presentation text for the link, i.e. the text that is visible for the user in the Internet interface.
            /// </summary>
            public Lang2Column4Parameterized LinkTextCol;

            /// <summary>
            /// Sorting code for the link. Makes it possible to dictate the order in which the links are presented for users in the Internet interface.
            /// If there is no sorting code, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized SortCodeCol;

            /// <summary>
            /// Description of link.
            /// If a description is not available, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized DescriptionCol;

            internal TblLinkLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("LinkLang2","LN2"), config.ExtractTableName("LinkLang2","LINK_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("LinkLang2", "LinkId"  , "LINKID");
                this.LinkIdCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("LinkLang2", "Link"  , "LINK");
                this.LinkCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("LinkLang2", "LinkText"  , "LINKTEXT");
                this.LinkTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("LinkLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("LinkLang2", "Description"  , "DESCRIPTION");
                this.DescriptionCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table connects links (table Link) to menu selection (table MenuSelection).
        /// </summary>
        public class TblLinkMenuSelection : Tab
        {

            /// <summary>
            /// Code for relevant menu level.
            /// 
            /// See description of the table MenuSelection.
            /// </summary>
            public Column4Parameterized MenuCol;
            /// <summary>
            /// The code for the nearest underlying eligible alternative in the relevant menu level.
            /// </summary>
            public Column4Parameterized SelectionCol;
            /// <summary>
            /// Identifying code for the link.
            /// </summary>
            public Column4Parameterized LinkIdCol;

            internal TblLinkMenuSelection(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("LinkMenuSelection","LMS"), config.ExtractTableName("LinkMenuSelection","LINKMENUSELECTION"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("LinkMenuSelection", "Menu", "MENU");
                this.MenuCol = new Column4Parameterized(tmpColumnName, this.Alias,"Menu",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("LinkMenuSelection", "Selection", "SELECTION");
                this.SelectionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Selection",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("LinkMenuSelection", "LinkId", "LINKID");
                this.LinkIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"LinkId",config.GetDataProvider());

            }

        }

        /// <summary>
        /// This table is the central compilation point for material and contains general information for the data tables that are linked to the table.
        /// </summary>
        public class TblMainTable : Tab
        {

            /// <summary>
            /// Summarised names of the statistical material and its underlying sub-tables with stored data.
            /// 
            /// The name should be descriptive, max 20 characters, beginning with a capital letter and contain only letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
            /// </summary>
            public Column4Parameterized MainTableCol;
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
            public Column4Parameterized TableStatusCol;
            /// <summary>
            /// The descriptive text which is presented when selecting a table in the retrieval interface.
            /// 
            /// The text should be unique and contain information on all dividing variables, including time scale (at the end). Information on unit, base time, fixed/current prices, calendar-adjusted or seasonally adjusted should normally NOT be included, as this is given in the presentation text for Contents. There can however be exceptions, i.e. for main tables that contain the same information except that the calculations are based on different base times.
            /// 
            /// The text should begin with a capital letter, should not end with a full stop, and should not include the % symbol. It should not finish with a number that relates to something other than the point of time.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Presentation text in short. Only needs to be filled in if the main table has several content columns. The text comes from the main table's PresText but should not contain information on variable or time.
            /// 
            /// Used in the retrieval interface as an introduction to the table heading, where several contents are chosen. Also used as content information in a PC-AXIS file, in some cases several contents are chosen at the same time and put into the same file.
            /// 
            /// Should begin with a capital letter and not end with a full-stop. The text should not include a % symbol.
            /// 
            /// If a short presentation text is not available, the field should be NULL.
            /// </summary>
            public Column4Parameterized PresTextSCol;
            /// <summary>
            /// Can be used for main tables with several contents to specify a collective name for the content columns. The content variable is used as the general name ("type") which is in the TextCatalog. Used in the retrieval interface in the heading when table is presented, "...by industry, time and [content variable]"
            /// 
            /// If the field is not used, it should be NULL.
            /// </summary>
            public Column4Parameterized ContentsVariableCol;
            /// <summary>
            /// Unique identity for main table. Can be used in requests from the end users, etc.
            /// </summary>
            public Column4Parameterized TableIdCol;
            /// <summary>
            /// Presentation category for all sub-tables and content columns in the table. There are three alternatives:
            /// 
            /// O = Official, i.e. tables that are officially released and accessible to all users on the external servers or are available on the production server, with plans for official release. (Only tables with PresCategory = O are on the external servers)
            /// I = Internal, i.e. tables are only accessible for internal users.The table should never be published to the general public.
            /// P = Private, i.e. tables are only accessible for certain internal users.
            /// The code is also written with capital letters.
            /// </summary>
            public Column4Parameterized PresCategoryCol;
            /// <summary>
            /// States when a main table was first published. The value is optional.
            /// </summary>
            public Column4Parameterized FirstPublishedCol;
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
            public Column4Parameterized SpecCharExistsCol;
            /// <summary>
            /// Code for subject area, i.e. BE
            /// </summary>
            public Column4Parameterized SubjectCodeCol;
            /// <summary>
            /// MetaId can be used to link the information in this table to an external system.
            /// </summary>
            public Column4Parameterized MetaIdCol;
            /// <summary>
            /// Id for the product that corresponds to the table. The field is optional.
            /// </summary>
            public Column4Parameterized ProductCodeCol;
            /// <summary>
            /// Name on the time scale that is used in the material. See further description of the table TimeScale.
            /// </summary>
            public Column4Parameterized TimeScaleCol;

            internal TblMainTable(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("MainTable","MTA"), config.ExtractTableName("MainTable","MAINTABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MainTable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "TableStatus", "TABLESTATUS");
                this.TableStatusCol = new Column4Parameterized(tmpColumnName, this.Alias,"TableStatus",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "PresTextS", "PRESTEXTS");
                this.PresTextSCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresTextS",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "ContentsVariable", "CONTENTSVARIABLE");
                this.ContentsVariableCol = new Column4Parameterized(tmpColumnName, this.Alias,"ContentsVariable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "TableId", "TABLEID");
                this.TableIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"TableId",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "PresCategory", "PRESCATEGORY");
                this.PresCategoryCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresCategory",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "FirstPublished", "FIRSTPUBLISHED");
                this.FirstPublishedCol = new Column4Parameterized(tmpColumnName, this.Alias,"FirstPublished",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "SpecCharExists", "SPECCHAREXISTS");
                this.SpecCharExistsCol = new Column4Parameterized(tmpColumnName, this.Alias,"SpecCharExists",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "SubjectCode", "SUBJECTCODE");
                this.SubjectCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"SubjectCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "MetaId", "METAID");
                this.MetaIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"MetaId",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "ProductCode", "PRODUCTCODE");
                this.ProductCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"ProductCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTable", "TimeScale", "TIMESCALE");
                this.TimeScaleCol = new Column4Parameterized(tmpColumnName, this.Alias,"TimeScale",config.GetDataProvider());

            }

        }

        public class TblMainTableLang2 : Lang2Tab
        {

            /// <summary>
            /// Summarised names of the statistical material and its underlying sub-tables with stored data.
            /// 
            /// The name should be descriptive, max 20 characters, beginning with a capital letter and contain only letters (a-z) or numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
            /// </summary>
            public Lang2Column4Parameterized MainTableCol;

            /// <summary>
            /// The descriptive text which is presented when selecting a table in the retrieval interface.
            /// 
            /// The text should be unique and contain information on all dividing variables, including time scale (at the end). Information on unit, base time, fixed/current prices, calendar-adjusted or seasonally adjusted should normally NOT be included, as this is given in the presentation text for Contents. There can however be exceptions, i.e. for main tables that contain the same information except that the calculations are based on different base times.
            /// 
            /// The text should begin with a capital letter, should not end with a full stop, and should not include the % symbol. It should not finish with a number that relates to something other than the point of time.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            /// <summary>
            /// Presentation text in short. Only needs to be filled in if the main table has several content columns. The text comes from the main table's PresText but should not contain information on variable or time.
            /// 
            /// Used in the retrieval interface as an introduction to the table heading, where several contents are chosen. Also used as content information in a PC-AXIS file, in some cases several contents are chosen at the same time and put into the same file.
            /// 
            /// Should begin with a capital letter and not end with a full-stop. The text should not include a % symbol.
            /// 
            /// If a short presentation text is not available, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized PresTextSCol;

            /// <summary>
            /// Can be used for main tables with several contents to specify a collective name for the content columns. The content variable is used as the general name ("type") which is in the TextCatalog. Used in the retrieval interface in the heading when table is presented, "...by industry, time and [content variable]"
            /// 
            /// If the field is not used, it should be NULL.
            /// </summary>
            public Lang2Column4Parameterized ContentsVariableCol;

            internal TblMainTableLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("MainTableLang2","MT2"), config.ExtractTableName("MainTableLang2","MAINTABLE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "MainTable"  , "MAINTABLE");
                this.MainTableCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "PresTextS"  , "PRESTEXTS");
                this.PresTextSCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MainTableLang2", "ContentsVariable"  , "CONTENTSVARIABLE");
                this.ContentsVariableCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table links the person responsible, i.e. the contact person and person responsible for updating, to the main tables. An unlimited number of persons can be linked to a main table.
        /// </summary>
        public class TblMainTablePerson : Tab
        {

            /// <summary>
            /// Name of main table.
            /// 
            /// See further in the description of the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Code for the person responsible, contact person or person responsible for updating for the main table.
            /// 
            /// For description, see the table Person.
            /// </summary>
            public Column4Parameterized PersonCodeCol;
            /// <summary>
            /// Code that shows the role of responsible person, contact person and/or person responsible for updating. There are the following alternatives:
            /// 
            /// M = Main contact person (one person)
            /// C = Contact person (0 – several persons)
            /// U = Person responsible for updating (1 – several persons)
            /// I = Person responsible for international reporting (0 - 1 person).
            /// V = Person that verifies not yet published data  (0 – several persons)
            /// </summary>
            public Column4Parameterized RolePersonCol;

            internal TblMainTablePerson(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("MainTablePerson","MTP"), config.ExtractTableName("MainTablePerson","MAINTABLEPERSON"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MainTablePerson", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTablePerson", "PersonCode", "PERSONCODE");
                this.PersonCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"PersonCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTablePerson", "RolePerson", "ROLEPERSON");
                this.RolePersonCol = new Column4Parameterized(tmpColumnName, this.Alias,"RolePerson",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table links a grouping to a main table.
        /// </summary>
        public class TblMainTableVariableHierarchy : Tab
        {

            /// <summary>
            /// Name of main table.
            /// 
            /// See further in the description of the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of variable.
            /// 
            /// See further in the description of the table Variable.
            /// </summary>
            public Column4Parameterized VariableCol;
            /// <summary>
            /// Name of grouping.
            /// 
            /// See further in the description of the table Grouping.
            /// </summary>
            public Column4Parameterized GroupingCol;
            /// <summary>
            /// The number of open levels that will be shown at menu selection the first time the tree is displayed. Must be 0 if all levels shall be shown.
            /// </summary>
            public Column4Parameterized ShowLevelsCol;
            /// <summary>
            /// Shows if all levels shall be stored or not. Can be:
            /// 
            /// Y = Yes
            /// N = No
            /// </summary>
            public Column4Parameterized AllLevelsStoredCol;

            internal TblMainTableVariableHierarchy(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("MainTableVariableHierarchy","MTP"), config.ExtractTableName("MainTableVariableHierarchy","MAINTABLEVARIABLEHIERARCHY"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "Variable", "VARIABLE");
                this.VariableCol = new Column4Parameterized(tmpColumnName, this.Alias,"Variable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "Grouping", "GROUPING");
                this.GroupingCol = new Column4Parameterized(tmpColumnName, this.Alias,"Grouping",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "ShowLevels", "SHOWLEVELS");
                this.ShowLevelsCol = new Column4Parameterized(tmpColumnName, this.Alias,"ShowLevels",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MainTableVariableHierarchy", "AllLevelsStored", "ALLLEVELSSTORED");
                this.AllLevelsStoredCol = new Column4Parameterized(tmpColumnName, this.Alias,"AllLevelsStored",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table is used to enable the presentation of any number of eligible levels above the table MainTable. The table acts as the entry point to the databases.
        /// All records in MenuSelection should also be in the corresponding MenuSelections for secondary languages (if any).
        /// </summary>
        public class TblMenuSelection : Tab
        {

            /// <summary>
            /// Code for relevant menu level. If LevelNo = 1, Menu should be filled with START. Code for subject areas may not exceed 20 characters.
            /// </summary>
            public Column4Parameterized MenuCol;
            /// <summary>
            /// The code for the nearest underlying eligible alternative in the relevant menu level. A menu can contain objects from different levels. Code for subject areas may not exceed 20 characters.
            /// </summary>
            public Column4Parameterized SelectionCol;
            /// <summary>
            /// Presentation text for MenuSelection.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Short presentation text for MenuSelection.
            /// 
            /// If a short presentation text is not available, the field should be NULL.
            /// </summary>
            public Column4Parameterized PresTextSCol;
            /// <summary>
            /// Descriptive text for MenuSelection.
            /// 
            /// If a description is not available, the field should be NULL.
            /// </summary>
            public Column4Parameterized DescriptionCol;
            /// <summary>
            /// Number of menu level, where 1 refers to the highest level.
            /// A type of object should always have the same LevelNo.
            /// 
            /// The highest level number should be given in the table MetaAdm (see description in that table).
            /// </summary>
            public Column4Parameterized LevelNoCol;
            /// <summary>
            /// Sorting code to dictate the presentation order for the eligible alternatives on each level.
            /// 
            /// If there is no sorting code, the field should be NULL.
            /// </summary>
            public Column4Parameterized SortCodeCol;
            /// <summary>
            /// Shows how a menu alternative can be used. Alternatives:
            /// 
            /// A = Active, visible and can be selected
            /// P = Passive, is visible but cannot be selected
            /// N = Not shown in the menu
            /// </summary>
            public Column4Parameterized PresentationCol;
            /// <summary>
            /// MetaId can be used to link the information in this table to an external system.
            /// </summary>
            public Column4Parameterized MetaIdCol;

            internal TblMenuSelection(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("MenuSelection","MSL"), config.ExtractTableName("MenuSelection","MENUSELECTION"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MenuSelection", "Menu", "MENU");
                this.MenuCol = new Column4Parameterized(tmpColumnName, this.Alias,"Menu",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MenuSelection", "Selection", "SELECTION");
                this.SelectionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Selection",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MenuSelection", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MenuSelection", "PresTextS", "PRESTEXTS");
                this.PresTextSCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresTextS",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MenuSelection", "Description", "DESCRIPTION");
                this.DescriptionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Description",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MenuSelection", "LevelNo", "LEVELNO");
                this.LevelNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"LevelNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MenuSelection", "SortCode", "SORTCODE");
                this.SortCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"SortCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MenuSelection", "Presentation", "PRESENTATION");
                this.PresentationCol = new Column4Parameterized(tmpColumnName, this.Alias,"Presentation",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MenuSelection", "MetaId", "METAID");
                this.MetaIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"MetaId",config.GetDataProvider());

            }

        }

        public class TblMenuSelectionLang2 : Lang2Tab
        {

            /// <summary>
            /// Code for relevant menu level. If LevelNo = 1, Menu should be filled with START. Code for subject areas may not exceed 20 characters.
            /// </summary>
            public Lang2Column4Parameterized MenuCol;

            /// <summary>
            /// The code for the nearest underlying eligible alternative in the relevant menu level. A menu can contain objects from different levels. Code for subject areas may not exceed 20 characters.
            /// </summary>
            public Lang2Column4Parameterized SelectionCol;

            /// <summary>
            /// Presentation text for MenuSelection.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            /// <summary>
            /// Short presentation text for MenuSelection.
            /// 
            /// If a short presentation text is not available, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized PresTextSCol;

            /// <summary>
            /// Descriptive text for MenuSelection.
            /// 
            /// If a description is not available, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized DescriptionCol;

            /// <summary>
            /// Sorting code to dictate the presentation order for the eligible alternatives on each level.
            /// 
            /// If there is no sorting code, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized SortCodeCol;

            /// <summary>
            /// Shows how a menu alternative can be used. Alternatives:
            /// 
            /// A = Active, visible and can be selected
            /// P = Passive, is visible but cannot be selected
            /// N = Not shown in the menu
            /// </summary>
            public Lang2Column4Parameterized PresentationCol;

            internal TblMenuSelectionLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("MenuSelectionLang2","MS2"), config.ExtractTableName("MenuSelectionLang2","MENUSELECTION_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "Menu"  , "MENU");
                this.MenuCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "Selection"  , "SELECTION");
                this.SelectionCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "PresTextS"  , "PRESTEXTS");
                this.PresTextSCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "Description"  , "DESCRIPTION");
                this.DescriptionCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("MenuSelectionLang2", "Presentation"  , "PRESENTATION");
                this.PresentationCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table contains system variables and their values.
        /// </summary>
        public class TblMetaAdm : Tab
        {

            /// <summary>
            /// Name of system variable. There are the following alternatives:
            /// - LastFootnoteNo
            /// - MenuLevels
            /// - SpecCharSum
            /// - NoOfLanguage
            /// - Language1, Language2 and so on
            /// - Codepage1
            /// - DataNotAvailable
            /// - DataNoteSum
            /// - DataSymbolSum
            /// - DataSymbolNil
            /// - PxDataFormat
            /// - KeysUpperLimit
            /// - DefaultCodeMissingLine
            /// 
            /// NoOfLanguage indicates the number of languages that exists in the model for the meta database presentation texts, and that can be used by the retrieval interfaces. For each language there should be a line like: Language1, Language2 and so on. Language1 should always include the main language. See also the description of the column Value in this table.
            /// 
            /// For each language there should also be a row in the table TextCatalog. For further information, see this table.
            /// 
            /// Regarding DefaultCodeMissingLine see also descriptions in: PresCellsZero och PresMissingLine in Contents, and CharacterType and PresCharacter in SpecialCharacter.
            /// </summary>
            public Column4Parameterized PropertyCol;
            /// <summary>
            /// Value of system variable. Contains one value per property.
            /// 
            /// For the property LastFootnoteNo, Value should contain the last used footnote number in the table Footnote.
            /// 
            /// For the property MenuLevels, Value should contains the highest used level number in the table MenuSelection.
            /// 
            /// For the property SpecCharSum, Value should contain the highest acceptable value for character type in the table SpecCharacter.
            /// 
            /// For the property NoOfLanguage Value should contain the number of languages that exists in the metadata model.
            /// 
            /// For the property Language1 Value should contain the main language of the model. The code is written in three capital letters, i.e. SVE.
            /// 
            /// For the property  Language2 , Language3 etc., Value should contain the other languages of the model. The code is written in three capital letters, i.e. ENG, ESP. The code is used as a suffix in the extra tables that should exist in the meta database, i.e. SubTable_ENG, SubTable_ESP.
            /// 
            /// For the property Codepage1: The characters that can be used and  how they should be presented. Is used at creating the keyword Codepage in the px file and at converting to XML.
            /// Three different examples: iso-8859-1, windows-1251, big5.
            /// 
            /// For the property DataNotAvailable: The value that should be presented, if  the data cell contains NULL and NPM-character is missing. If the value exists in the table SpecialCharacter, it is used, otherwise the character in the table DataNotAvailable is used.  The value is a reference to CharacterType in the SpecialCharacter table.
            /// Example: .. (two dots).
            /// 
            /// For the property DataNoteSum: The value that should be presented after the sum, if data cells with different NPM marking is summarized. The value is a reference to CharacterType in the SpecialCharacter table.
            /// Example:  *
            /// 1A + 2B = 3*
            /// 
            /// For the property DataSymbolSum: The value that should be presented if data cells with different NPM character are summarized and no sum can be created. The value is a reference to CharacterType in the SpecialCharacter table.
            /// Example: N.A.
            /// . + .. = N.A.
            /// 
            /// For the property DataSymbolNil: the value that should be presented at absolute 0 (zero) in the table SpecialCharacter. The value is a reference to CharacterType in the SpecialCharacter table.
            /// Example: -
            /// 
            /// For the property PxDataFormat: Matrix = all retreivals should be stored in matrix format.
            /// Keysnn = retreivals with keys are remade
            /// nn &gt; read data cells *100 / presented number of data cells
            /// Example: 40
            /// (Default is Matrix.)
            /// 
            /// For the property KeysUpperLimit: Maximum number of data cells that the presented matrix may contain, if the retreival should be possible to do with Keys. If greater, the retreival is made in matrix format.
            /// Example: 1000000
            /// 
            /// For the property DefaultCodeMissingLine: The value that should be presented in data cells that are not stored. Is used if neither presentation with 0 or special character have been specified. The value is a reference to CharacterType in the SpecialCharacter table.
            /// 
            /// See also the description of the column Property in this table, and also the table TextCatalog.
            /// </summary>
            public Column4Parameterized ValueCol;
            /// <summary>
            /// Must contain a description of the property.
            /// </summary>
            public Column4Parameterized DescriptionCol;

            internal TblMetaAdm(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("MetaAdm","MAD"), config.ExtractTableName("MetaAdm","METAADM"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MetaAdm", "Property", "PROPERTY");
                this.PropertyCol = new Column4Parameterized(tmpColumnName, this.Alias,"Property",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MetaAdm", "Value", "VALUE");
                this.ValueCol = new Column4Parameterized(tmpColumnName, this.Alias,"Value",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MetaAdm", "Description", "DESCRIPTION");
                this.DescriptionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Description",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table contains information on the relevant data model,version and the database role.
        /// </summary>
        public class TblMetabaseInfo : Tab
        {

            /// <summary>
            /// This field can be used to give information about general characteristics of the database.
            /// E.g. if the data is on macro or micro level.
            /// </summary>
            public Column4Parameterized ModelCol;
            /// <summary>
            /// Version number for the metadata model that the metadata database uses.
            /// </summary>
            public Column4Parameterized ModelVersionCol;
            /// <summary>
            /// Role of database. Can be:
            /// 
            /// - Production
            /// - Operation
            /// - Test
            /// </summary>
            public Column4Parameterized DatabaseRoleCol;

            internal TblMetabaseInfo(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("MetabaseInfo","MBI"), config.ExtractTableName("MetabaseInfo","METABASEINFO"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("MetabaseInfo", "Model", "MODEL");
                this.ModelCol = new Column4Parameterized(tmpColumnName, this.Alias,"Model",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MetabaseInfo", "ModelVersion", "MODELVERSION");
                this.ModelVersionCol = new Column4Parameterized(tmpColumnName, this.Alias,"ModelVersion",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("MetabaseInfo", "DatabaseRole", "DATABASEROLE");
                this.DatabaseRoleCol = new Column4Parameterized(tmpColumnName, this.Alias,"DatabaseRole",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table contains information on the authorities and organizations that are responsible for or produce statistics.
        /// </summary>
        public class TblOrganization : Tab
        {

            /// <summary>
            /// Identification for the organization
            /// </summary>
            public Column4Parameterized OrganizationCodeCol;
            /// <summary>
            /// Name of authority/organisation in full text, including any official abbreviation in brackets, e.g. Statistics Sweden (SCB).
            /// 
            /// Text should begin with a capital letter.
            /// </summary>
            public Column4Parameterized OrganizationNameCol;
            /// <summary>
            /// Name of the department or equivalent that produces the statistics.
            /// </summary>
            public Column4Parameterized DepartmentCol;
            /// <summary>
            /// Name of unit or equivalent.
            /// </summary>
            public Column4Parameterized UnitCol;
            /// <summary>
            /// Internet address to the authority's/organisation's website. Written as, for example:
            /// www.scb.se
            /// 
            /// If Internet address is not available, the field should be NULL.
            /// </summary>
            public Column4Parameterized WebAddressCol;
            /// <summary>
            /// MetaId can be used to link the information in this table to an external system.
            /// </summary>
            public Column4Parameterized MetaIdCol;

            internal TblOrganization(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("Organization","ORG"), config.ExtractTableName("Organization","ORGANIZATION"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Organization", "OrganizationCode", "ORGANIZATIONCODE");
                this.OrganizationCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"OrganizationCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Organization", "OrganizationName", "ORGANIZATIONNAME");
                this.OrganizationNameCol = new Column4Parameterized(tmpColumnName, this.Alias,"OrganizationName",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Organization", "Department", "DEPARTMENT");
                this.DepartmentCol = new Column4Parameterized(tmpColumnName, this.Alias,"Department",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Organization", "Unit", "UNIT");
                this.UnitCol = new Column4Parameterized(tmpColumnName, this.Alias,"Unit",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Organization", "WebAddress", "WEBADDRESS");
                this.WebAddressCol = new Column4Parameterized(tmpColumnName, this.Alias,"WebAddress",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Organization", "MetaId", "METAID");
                this.MetaIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"MetaId",config.GetDataProvider());

            }

        }

        public class TblOrganizationLang2 : Lang2Tab
        {

            /// <summary>
            /// Identification for the organization
            /// </summary>
            public Lang2Column4Parameterized OrganizationCodeCol;

            /// <summary>
            /// Name of authority/organisation in full text, including any official abbreviation in brackets, e.g. Statistics Sweden (SCB).
            /// 
            /// Text should begin with a capital letter.
            /// </summary>
            public Lang2Column4Parameterized OrganizationNameCol;

            /// <summary>
            /// Name of the department or equivalent that produces the statistics.
            /// </summary>
            public Lang2Column4Parameterized DepartmentCol;

            /// <summary>
            /// Name of unit or equivalent.
            /// </summary>
            public Lang2Column4Parameterized UnitCol;

            /// <summary>
            /// Internet address to the authority's/organisation's website. Written as, for example:
            /// www.scb.se
            /// 
            /// If Internet address is not available, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized WebAddressCol;

            internal TblOrganizationLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("OrganizationLang2","OR2"), config.ExtractTableName("OrganizationLang2","ORGANIZATION_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("OrganizationLang2", "OrganizationCode"  , "ORGANIZATIONCODE");
                this.OrganizationCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("OrganizationLang2", "OrganizationName"  , "ORGANIZATIONNAME");
                this.OrganizationNameCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("OrganizationLang2", "Department"  , "DEPARTMENT");
                this.DepartmentCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("OrganizationLang2", "Unit"  , "UNIT");
                this.UnitCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("OrganizationLang2", "WebAddress"  , "WEBADDRESS");
                this.WebAddressCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table contains information on all persons (or alternatively groups) which are contact persons for content and/or responsible for updating statistics in the database.
        /// </summary>
        public class TblPerson : Tab
        {

            /// <summary>
            /// Identifying code for person (or group) responsible.
            /// </summary>
            public Column4Parameterized PersonCodeCol;
            /// <summary>
            /// Code for the organization or authority that produces and/or is responsible for the statistical material.
            /// 
            /// See further description of the table Organization.
            /// </summary>
            public Column4Parameterized OrganizationCodeCol;
            /// <summary>
            /// Responsible person's first name
            /// For groups, this data is not available and should therefore be NULL.
            /// </summary>
            public Column4Parameterized ForenameCol;
            /// <summary>
            /// Surname of the person responsible or name of the group responsible.
            /// </summary>
            public Column4Parameterized SurnameCol;
            /// <summary>
            /// Prefix for telephone number, so that the number is valid internationally.
            /// 
            /// E.g. for Swedish telephone numbers the prefix is +46.
            /// </summary>
            public Column4Parameterized PhonePrefixCol;
            /// <summary>
            /// Complete national telephone numbers, i.e. without international prefix.
            /// 
            /// Should be written as: national code, hyphen, then numbers in groups of two or three, divided by a space.
            /// </summary>
            public Column4Parameterized PhoneNoCol;
            /// <summary>
            /// Complete national fax machine numbers, i.e. without international prefix.
            /// 
            /// Should be written as: national code, hyphen, then numbers in groups of two or three, divided by a space.
            /// If there is no fax number, this field should be NULL.
            /// </summary>
            public Column4Parameterized FaxNoCol;
            /// <summary>
            /// E-mail address for person or group responsible, if available.
            /// If an e-mail address is not available, the field should be NULL.
            /// 
            /// Written with lower case letters.
            /// </summary>
            public Column4Parameterized EmailCol;

            internal TblPerson(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("Person","PRS"), config.ExtractTableName("Person","PERSON"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Person", "PersonCode", "PERSONCODE");
                this.PersonCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"PersonCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Person", "OrganizationCode", "ORGANIZATIONCODE");
                this.OrganizationCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"OrganizationCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Person", "Forename", "FORENAME");
                this.ForenameCol = new Column4Parameterized(tmpColumnName, this.Alias,"Forename",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Person", "Surname", "SURNAME");
                this.SurnameCol = new Column4Parameterized(tmpColumnName, this.Alias,"Surname",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Person", "PhonePrefix", "PHONEPREFIX");
                this.PhonePrefixCol = new Column4Parameterized(tmpColumnName, this.Alias,"PhonePrefix",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Person", "PhoneNo", "PHONENO");
                this.PhoneNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"PhoneNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Person", "FaxNo", "FAXNO");
                this.FaxNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"FaxNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Person", "Email", "EMAIL");
                this.EmailCol = new Column4Parameterized(tmpColumnName, this.Alias,"Email",config.GetDataProvider());

            }

        }

        /// <summary>
        /// Information about secondary languages (if any)
        /// </summary>
        public class TblSecondaryLanguage : Tab
        {

            /// <summary>
            /// Name of main table
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of secondary language.
            /// </summary>
            public Column4Parameterized LanguageCol;
            /// <summary>
            /// Code which shows whether all the table's presentation texts are translated to English or not. This column is necessary so that it is possible to determine from the retrieval interface whether the table will be shown in English or not.
            /// 
            /// Valid values:
            /// Y - the table is completely translated to the secondary language
            /// N - the table is not completely translated to the secondary language
            /// </summary>
            public Column4Parameterized CompletelyTranslatedCol;
            /// <summary>
            /// Shows if the secondary language is published or not.
            /// 
            /// Valid values:
            /// Y = yes
            /// N = no
            /// </summary>
            public Column4Parameterized PublishedCol;
            /// <summary>
            /// 
            /// </summary>
            public Column4Parameterized UserIdCol;
            /// <summary>
            /// 
            /// </summary>
            public Column4Parameterized LogDateCol;

            internal TblSecondaryLanguage(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("SecondaryLanguage","SLA"), config.ExtractTableName("SecondaryLanguage","SECONDARYLANGUAGE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SecondaryLanguage", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SecondaryLanguage", "Language", "LANGUAGE");
                this.LanguageCol = new Column4Parameterized(tmpColumnName, this.Alias,"Language",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SecondaryLanguage", "CompletelyTranslated", "COMPLETELYTRANSLATED");
                this.CompletelyTranslatedCol = new Column4Parameterized(tmpColumnName, this.Alias,"CompletelyTranslated",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SecondaryLanguage", "Published", "PUBLISHED");
                this.PublishedCol = new Column4Parameterized(tmpColumnName, this.Alias,"Published",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SecondaryLanguage", "UserId", "USERID");
                this.UserIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"UserId",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SecondaryLanguage", "LogDate", "LOGDATE");
                this.LogDateCol = new Column4Parameterized(tmpColumnName, this.Alias,"LogDate",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table contains information on the special characters that are used in the database's data tables. Special characters such as ....or - , can be used to show that data is missing, is not relevant or is too uncertain to be given.
        /// </summary>
        public class TblSpecialCharacter : Tab
        {

            /// <summary>
            /// Identifying code for the special character.
            /// 
            /// Given in the form of a number, from 1 upwards. The highest acceptable number is given in the table MetaAdm, which is 99 (see description in table MetaAdm).
            /// 
            /// If PresMissingLine in Contents contains the identity for a special character, this character must be represented here. See also descriptions of PresCellsZero and PresMissingLine in Contents, PresCharacter in SpecialCharacter and the table MetaAdm.
            /// </summary>
            public Column4Parameterized CharacterTypeCol;
            /// <summary>
            /// The special character as presented for the user when the table is presented when retrieved.
            /// </summary>
            public Column4Parameterized PresCharacterCol;
            /// <summary>
            /// Used to show whether the data cell with the special character can be aggregated or not. There are the following alternatives:
            /// 
            /// Y = Yes
            /// N = No
            /// 
            /// If AggregPossible = Y, the specific data cell, even if not shown, can be included in an aggregation.
            /// </summary>
            public Column4Parameterized AggregPossibleCol;
            /// <summary>
            /// Provides the retrieval programs with information concerning the presentation of a special character;  with data and special character or with special character only.
            /// 
            /// There are the following alternatives:
            /// Y = The data cell should be presented together with the special character
            /// N = The special character alone should be presented
            /// </summary>
            public Column4Parameterized DataCellPresCol;
            /// <summary>
            /// Shows whether the data cell must be filled in or not. There are the following alternatives:
            /// 
            /// V = Value must be filled in
            /// N = No, the data cell should not be  filled in but should be NULL
            /// F = Any, i.e. the data cell can be filled in or can be NULL.
            /// 0 = The data cell should contain 0 (zero) only
            /// </summary>
            public Column4Parameterized DataCellFilledCol;
            /// <summary>
            /// Explanation to what is written in PresCharacter.
            /// 
            /// If there is no presentation text, the field should be NULL.
            /// </summary>
            public Column4Parameterized PresTextCol;

            internal TblSpecialCharacter(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("SpecialCharacter","SPC"), config.ExtractTableName("SpecialCharacter","SPECIALCHARACTER"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "CharacterType", "CHARACTERTYPE");
                this.CharacterTypeCol = new Column4Parameterized(tmpColumnName, this.Alias,"CharacterType",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "PresCharacter", "PRESCHARACTER");
                this.PresCharacterCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresCharacter",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "AggregPossible", "AGGREGPOSSIBLE");
                this.AggregPossibleCol = new Column4Parameterized(tmpColumnName, this.Alias,"AggregPossible",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "DataCellPres", "DATACELLPRES");
                this.DataCellPresCol = new Column4Parameterized(tmpColumnName, this.Alias,"DataCellPres",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "DataCellFilled", "DATACELLFILLED");
                this.DataCellFilledCol = new Column4Parameterized(tmpColumnName, this.Alias,"DataCellFilled",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SpecialCharacter", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());

            }

        }

        public class TblSpecialCharacterLang2 : Lang2Tab
        {

            /// <summary>
            /// Identifying code for the special character.
            /// 
            /// Given in the form of a number, from 1 upwards. The highest acceptable number is given in the table MetaAdm, which is 99 (see description in table MetaAdm).
            /// 
            /// If PresMissingLine in Contents contains the identity for a special character, this character must be represented here. See also descriptions of PresCellsZero and PresMissingLine in Contents, PresCharacter in SpecialCharacter and the table MetaAdm.
            /// </summary>
            public Lang2Column4Parameterized CharacterTypeCol;

            /// <summary>
            /// The special character as presented for the user when the table is presented when retrieved.
            /// </summary>
            public Lang2Column4Parameterized PresCharacterCol;

            /// <summary>
            /// Explanation to what is written in PresCharacter.
            /// 
            /// If there is no presentation text, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            internal TblSpecialCharacterLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("SpecialCharacterLang2","SP2"), config.ExtractTableName("SpecialCharacterLang2","SPECIALCHARACTER_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SpecialCharacterLang2", "CharacterType"  , "CHARACTERTYPE");
                this.CharacterTypeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("SpecialCharacterLang2", "PresCharacter"  , "PRESCHARACTER");
                this.PresCharacterCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("SpecialCharacterLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table contains information on the sub-tables, reflecting the stored data tables, which are in the subject databases. The data tables are identified using the main table's name + sub-table's name.
        /// </summary>
        public class TblSubTable : Tab
        {

            /// <summary>
            /// The name of the main table, to which the sub-table is linked. See further description in the table MainTable.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of the sub-table. For material stored only in the sub-table, the field is left empty, i.e. a dash is written.
            /// Name of main table + name of sub-table together make up the name of the data table where the data is stored, if SubTable is not empty, in which case the name of the data table is made up of the name of the main table only.
            /// 
            /// NB. Make sure that the numbering is always included even if there is only one sub-table divided by region among the sub-tables that are linked to the relevant main table.
            /// </summary>
            public Column4Parameterized SubTableCol;
            /// <summary>
            /// Descriptive text that is used by the retrieval interface, i.e. when selecting a sub-level to a table or sub-table in the retrieval interface, if the main table has several sub-tables.
            /// 
            /// The text should be unique (there should not be two sub-tables with the same PresText) and should contain information on all the division variables, excluding totals. Information on timescale should be added at the end.
            /// 
            /// For data material that is only stored in a sub-table, the text should be the same as PresText in the table MainTable.
            /// 
            /// For data material that is divided up into different sub-tables, the main table’s presentation text should be used as a "model", which is supplemented with the information that differentiates the sub-tables.
            /// 
            /// The text should begin with a capital letter, should not end with a full stop, and should not include the % symbol.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Shows whether the sub-tables values can be aggregated or not.
            /// </summary>
            public Column4Parameterized CleanTableCol;

            internal TblSubTable(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("SubTable","STB"), config.ExtractTableName("SubTable","SUBTABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SubTable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SubTable", "SubTable", "SUBTABLE");
                this.SubTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"SubTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SubTable", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SubTable", "CleanTable", "CLEANTABLE");
                this.CleanTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"CleanTable",config.GetDataProvider());

            }

        }

        public class TblSubTableLang2 : Lang2Tab
        {

            /// <summary>
            /// The name of the main table, to which the sub-table is linked. See further description in the table MainTable.
            /// </summary>
            public Lang2Column4Parameterized MainTableCol;

            /// <summary>
            /// Name of the sub-table. For material stored only in the sub-table, the field is left empty, i.e. a dash is written.
            /// Name of main table + name of sub-table together make up the name of the data table where the data is stored, if SubTable is not empty, in which case the name of the data table is made up of the name of the main table only.
            /// 
            /// NB. Make sure that the numbering is always included even if there is only one sub-table divided by region among the sub-tables that are linked to the relevant main table.
            /// </summary>
            public Lang2Column4Parameterized SubTableCol;

            /// <summary>
            /// Descriptive text that is used by the retrieval interface, i.e. when selecting a sub-level to a table or sub-table in the retrieval interface, if the main table has several sub-tables.
            /// 
            /// The text should be unique (there should not be two sub-tables with the same PresText) and should contain information on all the division variables, excluding totals. Information on timescale should be added at the end.
            /// 
            /// For data material that is only stored in a sub-table, the text should be the same as PresText in the table MainTable.
            /// 
            /// For data material that is divided up into different sub-tables, the main table’s presentation text should be used as a "model", which is supplemented with the information that differentiates the sub-tables.
            /// 
            /// The text should begin with a capital letter, should not end with a full stop, and should not include the % symbol.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            internal TblSubTableLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("SubTableLang2","ST2"), config.ExtractTableName("SubTableLang2","SUBTABLE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SubTableLang2", "MainTable"  , "MAINTABLE");
                this.MainTableCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("SubTableLang2", "SubTable"  , "SUBTABLE");
                this.SubTableCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("SubTableLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table links variables with value sets in the sub-tables. The variable name is made up of the name for the corresponding metadata column in the data tables.
        /// </summary>
        public class TblSubTableVariable : Tab
        {

            /// <summary>
            /// The name of the main table to which the variable and the sub-table are linked. See further description in the MainTable table.
            /// </summary>
            public Column4Parameterized MainTableCol;
            /// <summary>
            /// Name of sub-table
            /// 
            /// Data can be missing, with a dash in its place.
            /// 
            /// See further description in the table SubTable.
            /// </summary>
            public Column4Parameterized SubTableCol;
            /// <summary>
            /// Variable name, which makes up the column name for metadata in the data table. See further description in the table Variable.
            /// </summary>
            public Column4Parameterized VariableCol;
            /// <summary>
            /// Name of value set. See further description in the table ValueSet.
            /// For rows with variable types V and G, the name of the value set must be filled in. For VariableType = T, the field is left empty, as there is no value set for the variable Time.
            /// </summary>
            public Column4Parameterized ValueSetCol;
            /// <summary>
            /// Code for type of variable. There are three alternatives:
            /// 
            /// - V = variable, i.e. dividing variable, not time.
            /// - G = geographical information for map program.
            /// - T = time.
            /// 
            /// If VariableType = G, the field GeoAreaNo in the tables ValueSet and Grouping should be filled in (however not yet implemented).
            /// </summary>
            public Column4Parameterized VariableTypeCol;
            /// <summary>
            /// The variable's column number in the data table.
            /// The variable Time should always be included and be the last column in the data table. If the material is divided by region, the variable Region should be the first column.
            /// Written as 1, 2, 3, etc.
            /// </summary>
            public Column4Parameterized StoreColumnNoCol;

            internal TblSubTableVariable(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("SubTableVariable","STV"), config.ExtractTableName("SubTableVariable","SUBTABLEVARIABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "MainTable", "MAINTABLE");
                this.MainTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"MainTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "SubTable", "SUBTABLE");
                this.SubTableCol = new Column4Parameterized(tmpColumnName, this.Alias,"SubTable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "Variable", "VARIABLE");
                this.VariableCol = new Column4Parameterized(tmpColumnName, this.Alias,"Variable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "ValueSet", "VALUESET");
                this.ValueSetCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueSet",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "VariableType", "VARIABLETYPE");
                this.VariableTypeCol = new Column4Parameterized(tmpColumnName, this.Alias,"VariableType",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("SubTableVariable", "StoreColumnNo", "STORECOLUMNNO");
                this.StoreColumnNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"StoreColumnNo",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table contains information on joint texts.
        /// </summary>
        public class TblTextCatalog : Tab
        {

            /// <summary>
            /// Identity of text.
            /// </summary>
            public Column4Parameterized TextCatalogNoCol;
            /// <summary>
            /// Type of text. The texts should be fixed for use in PC-AXIS.
            /// 
            /// Alternatives:
            /// - ContentsVariable
            /// - GeoAreaNo
            /// - Language1, Language2 (and so on)
            /// </summary>
            public Column4Parameterized TextTypeCol;
            /// <summary>
            /// The text that should be shown. Can be the name of a map file etc., or a language. The language should be written in the language it refers to, e.g. svenska, English, Espanol. See also the description of the table MetaAdm.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Description of text.
            /// 
            /// If a description is not available, the field should be NULL.
            /// </summary>
            public Column4Parameterized DescriptionCol;

            internal TblTextCatalog(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("TextCatalog","TXC"), config.ExtractTableName("TextCatalog","TEXTCATALOG"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("TextCatalog", "TextCatalogNo", "TEXTCATALOGNO");
                this.TextCatalogNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"TextCatalogNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("TextCatalog", "TextType", "TEXTTYPE");
                this.TextTypeCol = new Column4Parameterized(tmpColumnName, this.Alias,"TextType",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("TextCatalog", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("TextCatalog", "Description", "DESCRIPTION");
                this.DescriptionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Description",config.GetDataProvider());

            }

        }

        public class TblTextCatalogLang2 : Lang2Tab
        {

            /// <summary>
            /// Identity of text.
            /// </summary>
            public Lang2Column4Parameterized TextCatalogNoCol;

            /// <summary>
            /// Type of text. The texts should be fixed for use in PC-AXIS.
            /// 
            /// Alternatives:
            /// - ContentsVariable
            /// - GeoAreaNo
            /// - Language1, Language2 (and so on)
            /// </summary>
            public Lang2Column4Parameterized TextTypeCol;

            /// <summary>
            /// The text that should be shown. Can be the name of a map file etc., or a language. The language should be written in the language it refers to, e.g. svenska, English, Espanol. See also the description of the table MetaAdm.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            /// <summary>
            /// Description of text.
            /// 
            /// If a description is not available, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized DescriptionCol;

            internal TblTextCatalogLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("TextCatalogLang2","TX2"), config.ExtractTableName("TextCatalogLang2","TEXTCATALOG_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("TextCatalogLang2", "TextCatalogNo"  , "TEXTCATALOGNO");
                this.TextCatalogNoCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("TextCatalogLang2", "TextType"  , "TEXTTYPE");
                this.TextTypeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("TextCatalogLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("TextCatalogLang2", "Description"  , "DESCRIPTION");
                this.DescriptionCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table describes the timescales that exist in the database.
        /// </summary>
        public class TblTimeScale : Tab
        {

            /// <summary>
            /// Name of timescale, i.e.Year, Month, Quarter.
            /// Should not contain dash (applies for retrievals in PC-AXIS).
            /// </summary>
            public Column4Parameterized TimeScaleCol;
            /// <summary>
            /// Presentation text for timescale, i.e. year, month, quarter. Text is often the same as the name in the column TimeScale. Written in lower case.
            /// Presentation text used when selecting time when making a retrieval from databases.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Shows if the timescale should be presented in table heading instead of the word Time. Can be:
            /// 
            /// Y = Yes
            /// N = No
            /// </summary>
            public Column4Parameterized TimeScalePresCol;
            /// <summary>
            /// Shows if timescale is regular or not. Can be:
            /// 
            /// Y = Yes
            /// N = No
            /// 
            /// An example of an irregular timescale is an election year.
            /// 
            /// Data is primarily accompanying information when retrieving statistics to a file.
            /// </summary>
            public Column4Parameterized RegularCol;
            /// <summary>
            /// Code for TimeUnit. Used as accompanying information when retrieving a statistics file. The following alternatives are possible:
            /// 
            /// Q = quarter
            /// A = academic year
            /// M = month
            /// X = 3 years
            /// S = split year
            /// Y = year
            /// T= term
            /// </summary>
            public Column4Parameterized TimeUnitCol;
            /// <summary>
            /// Shows how many points in time the relevant timescale contains per calendar year, i.e.:
            /// 
            /// 1 for timescale year,
            /// 4 for timescale quarter,
            /// 12 for timescale month.
            /// 
            /// For irregular and regular timescales, where points in time do not occur consecutively (i.e. every other year), the field should be NULL.
            /// </summary>
            public Column4Parameterized FrequencyCol;
            /// <summary>
            /// Description of storage format for the point in time in the timescale. There are the following alternatives:
            /// 
            /// yyyy for timescales where TimeUnit = Y,
            /// yyyy-yyyy for timescales where TimeUnit = T,
            /// yyyy/yy for timescales where TimeUnit = A,
            /// yyyy/yyyy for timescales where TimeUnit = P,
            /// yyyyQq for timescales where TimeUnit = Q,
            /// yyyyMmm for timescales where TimeUnit = M.
            /// 
            /// For a description of time units, see column TimeUnit in the table TimeScale.
            /// </summary>
            public Column4Parameterized StoreFormatCol;

            internal TblTimeScale(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("TimeScale","TSC"), config.ExtractTableName("TimeScale","TIMESCALE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("TimeScale", "TimeScale", "TIMESCALE");
                this.TimeScaleCol = new Column4Parameterized(tmpColumnName, this.Alias,"TimeScale",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("TimeScale", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("TimeScale", "TimeScalePres", "TIMESCALEPRES");
                this.TimeScalePresCol = new Column4Parameterized(tmpColumnName, this.Alias,"TimeScalePres",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("TimeScale", "Regular", "REGULAR");
                this.RegularCol = new Column4Parameterized(tmpColumnName, this.Alias,"Regular",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("TimeScale", "TimeUnit", "TIMEUNIT");
                this.TimeUnitCol = new Column4Parameterized(tmpColumnName, this.Alias,"TimeUnit",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("TimeScale", "Frequency", "FREQUENCY");
                this.FrequencyCol = new Column4Parameterized(tmpColumnName, this.Alias,"Frequency",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("TimeScale", "StoreFormat", "STOREFORMAT");
                this.StoreFormatCol = new Column4Parameterized(tmpColumnName, this.Alias,"StoreFormat",config.GetDataProvider());

            }

        }

        public class TblTimeScaleLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of timescale, i.e.Year, Month, Quarter.
            /// Should not contain dash (applies for retrievals in PC-AXIS).
            /// </summary>
            public Lang2Column4Parameterized TimeScaleCol;

            /// <summary>
            /// Presentation text for timescale, i.e. year, month, quarter. Text is often the same as the name in the column TimeScale. Written in lower case.
            /// Presentation text used when selecting time when making a retrieval from databases.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            internal TblTimeScaleLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("TimeScaleLang2","TS2"), config.ExtractTableName("TimeScaleLang2","TIMESCALE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("TimeScaleLang2", "TimeScale"  , "TIMESCALE");
                this.TimeScaleCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("TimeScaleLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table links values for a value pool to a value set, for which data is stored in the data table.
        /// </summary>
        public class TblVSValue : Tab
        {

            /// <summary>
            /// Name of the value set to which the values are linked.
            /// 
            /// See further description in table ValueSet.
            /// </summary>
            public Column4Parameterized ValueSetCol;
            /// <summary>
            /// Name of the value pool to which the value set belongs.
            /// 
            /// See further description in table ValuePool.
            /// </summary>
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Code for the values that are linked to the value set.
            /// 
            /// See further description in table Value.
            /// </summary>
            public Column4Parameterized ValueCodeCol;
            /// <summary>
            /// Sorting code for values within the value set. Dictates the presentation order for the value set's values when retrieving from the database and presenting the table.
            /// 
            /// So that this sorting code can be applied, the field SortCodeExists in the table ValueSet must be filled with Y. If it is N, the sorting code in the table Value is used instead.
            /// 
            /// If there is no sorting code, the field should be NULL.
            /// </summary>
            public Column4Parameterized SortCodeCol;

            internal TblVSValue(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("VSValue","VVL"), config.ExtractTableName("VSValue","VSVALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("VSValue", "ValueSet", "VALUESET");
                this.ValueSetCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueSet",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("VSValue", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("VSValue", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("VSValue", "SortCode", "SORTCODE");
                this.SortCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"SortCode",config.GetDataProvider());

            }

        }

        public class TblVSValueLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of the value set to which the values are linked.
            /// 
            /// See further description in table ValueSet.
            /// </summary>
            public Lang2Column4Parameterized ValueSetCol;

            /// <summary>
            /// Name of the value pool to which the value set belongs.
            /// 
            /// See further description in table ValuePool.
            /// </summary>
            public Lang2Column4Parameterized ValuePoolCol;

            /// <summary>
            /// Code for the values that are linked to the value set.
            /// 
            /// See further description in table Value.
            /// </summary>
            public Lang2Column4Parameterized ValueCodeCol;

            /// <summary>
            /// Sorting code for values within the value set. Dictates the presentation order for the value set's values when retrieving from the database and presenting the table.
            /// 
            /// So that this sorting code can be applied, the field SortCodeExists in the table ValueSet must be filled with Y. If it is N, the sorting code in the table Value is used instead.
            /// 
            /// If there is no sorting code, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized SortCodeCol;

            internal TblVSValueLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("VSValueLang2","VV2"), config.ExtractTableName("VSValueLang2","VSVALUE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("VSValueLang2", "ValueSet"  , "VALUESET");
                this.ValueSetCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("VSValueLang2", "ValuePool"  , "VALUEPOOL");
                this.ValuePoolCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("VSValueLang2", "ValueCode"  , "VALUECODE");
                this.ValueCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("VSValueLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
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
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Code for value or group.
            /// 
            /// Value code should agree with the code in the corresponding classification or standard if there is one.
            /// 
            /// Because the value codes are stored in a metadata column for variables in the data table(s) and, because the width of the metadata column is decided by the number of characters in the longest value code, the code should not be longer than necessary to ensure that it does not take up more space in the data table than necessary. The value codes within a value set should also be roughly the same size.
            /// 
            /// Capitals and/or lower case letters can be used, the letters å, ä and ö are accepted. Special characters and dashes should be avoided because they can cause technical problems.
            /// </summary>
            public Column4Parameterized ValueCodeCol;
            /// <summary>
            /// Sorting code for values and groups, which decides in which order the value and group codes are to be presented when values and table presentation are selected when retrieved from the databases.
            /// 
            /// The sorting code should be the same as the ValueCode or be designed in such a way that the values can be presented in the desired order. The beginning of ValueTextL can be used so that the values will be presented in alphabetical order by the value text.
            /// 
            /// NB. Please note that the sorting code is also available in the tables VSValue, VSGroup and Grouping. See further descriptions for these.
            /// </summary>
            public Column4Parameterized SortCodeCol;
            /// <summary>
            /// Can be used to state the unit so that a value can have different units.
            /// 
            /// If the field is filled in with a unit, the column Unit in the table Contents should be filled with %Value. If the field is not filled in, it should be NULL. Then the column Unit in the table Contents is used instead to state the unit.
            /// 
            /// See also description of the table Contents.
            /// </summary>
            public Column4Parameterized UnitCol;
            /// <summary>
            /// Short presentation text for value and group.
            /// 
            /// To be visible in the retrieval interfaces, it requires that:
            /// - The field ValueTextExists in ValuePool is either S ('Short value text exists') or B ('Both short and long value text exists') and
            /// - The field ValuePres in ValuePool or ValueSet is either A ('Both code and short value text should be presented') or S ('Short value text should be presented').
            /// 
            /// The text is written in lower case letters, except for abbreviations etc.
            /// 
            /// See also descriptions of ValueTextExists in ValuePool and ValuePres in ValuePool and ValueSet.
            /// </summary>
            public Column4Parameterized ValueTextSCol;
            /// <summary>
            /// Value text, presentation text for value and group.
            /// 
            /// To be visible in the retrieval interface, the field ValueTextExists in the table ValuePool for the value's value pool must be L.
            /// 
            /// ValueText can be omitted if the values are to be presented only as codes. The field should then be NULL. There should be consistency with a value pool so that all the value pool's values are presented either with or without value texts.
            /// 
            /// The text is written in lower case, with the exception of abbreviations, etc.
            /// 
            /// See also descriptions of ValueTextExists in ValuePool and  ValuePres in ValuePool and ValueSet.
            /// </summary>
            public Column4Parameterized ValueTextLCol;
            /// <summary>
            /// MetaId can be used to link the information in this table to an external system.
            /// </summary>
            public Column4Parameterized MetaIdCol;
            /// <summary>
            /// Shows whether there is a footnote linked to the value (FootnoteType 6). There are the following alternatives:
            /// 
            /// B = Both obligatory and optional footnotes exist
            /// V = One or several optional footnotes exist.
            /// O = One or several obligatory footnotes exist
            /// N = There are no footnotes
            /// </summary>
            public Column4Parameterized FootnoteCol;

            internal TblValue(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("Value","VAL"), config.ExtractTableName("Value","VALUE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Value", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Value", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Value", "SortCode", "SORTCODE");
                this.SortCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"SortCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Value", "Unit", "UNIT");
                this.UnitCol = new Column4Parameterized(tmpColumnName, this.Alias,"Unit",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Value", "ValueTextS", "VALUETEXTS");
                this.ValueTextSCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueTextS",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Value", "ValueTextL", "VALUETEXTL");
                this.ValueTextLCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueTextL",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Value", "MetaId", "METAID");
                this.MetaIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"MetaId",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Value", "Footnote", "FOOTNOTE");
                this.FootnoteCol = new Column4Parameterized(tmpColumnName, this.Alias,"Footnote",config.GetDataProvider());

            }

        }

        public class TblValueLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of the value pool that the value belongs to. See further description of table ValuePool.
            /// </summary>
            public Lang2Column4Parameterized ValuePoolCol;

            /// <summary>
            /// Code for value or group.
            /// 
            /// Value code should agree with the code in the corresponding classification or standard if there is one.
            /// 
            /// Because the value codes are stored in a metadata column for variables in the data table(s) and, because the width of the metadata column is decided by the number of characters in the longest value code, the code should not be longer than necessary to ensure that it does not take up more space in the data table than necessary. The value codes within a value set should also be roughly the same size.
            /// 
            /// Capitals and/or lower case letters can be used, the letters å, ä and ö are accepted. Special characters and dashes should be avoided because they can cause technical problems.
            /// </summary>
            public Lang2Column4Parameterized ValueCodeCol;

            /// <summary>
            /// Sorting code for values and groups, which decides in which order the value and group codes are to be presented when values and table presentation are selected when retrieved from the databases.
            /// 
            /// The sorting code should be the same as the ValueCode or be designed in such a way that the values can be presented in the desired order. The beginning of ValueTextL can be used so that the values will be presented in alphabetical order by the value text.
            /// 
            /// NB. Please note that the sorting code is also available in the tables VSValue, VSGroup and Grouping. See further descriptions for these.
            /// </summary>
            public Lang2Column4Parameterized SortCodeCol;

            /// <summary>
            /// Can be used to state the unit so that a value can have different units.
            /// 
            /// If the field is filled in with a unit, the column Unit in the table Contents should be filled with %Value. If the field is not filled in, it should be NULL. Then the column Unit in the table Contents is used instead to state the unit.
            /// 
            /// See also description of the table Contents.
            /// </summary>
            public Lang2Column4Parameterized UnitCol;

            /// <summary>
            /// Short presentation text for value and group.
            /// 
            /// To be visible in the retrieval interfaces, it requires that:
            /// - The field ValueTextExists in ValuePool is either S ('Short value text exists') or B ('Both short and long value text exists') and
            /// - The field ValuePres in ValuePool or ValueSet is either A ('Both code and short value text should be presented') or S ('Short value text should be presented').
            /// 
            /// The text is written in lower case letters, except for abbreviations etc.
            /// 
            /// See also descriptions of ValueTextExists in ValuePool and ValuePres in ValuePool and ValueSet.
            /// </summary>
            public Lang2Column4Parameterized ValueTextSCol;

            /// <summary>
            /// Value text, presentation text for value and group.
            /// 
            /// To be visible in the retrieval interface, the field ValueTextExists in the table ValuePool for the value's value pool must be L.
            /// 
            /// ValueText can be omitted if the values are to be presented only as codes. The field should then be NULL. There should be consistency with a value pool so that all the value pool's values are presented either with or without value texts.
            /// 
            /// The text is written in lower case, with the exception of abbreviations, etc.
            /// 
            /// See also descriptions of ValueTextExists in ValuePool and  ValuePres in ValuePool and ValueSet.
            /// </summary>
            public Lang2Column4Parameterized ValueTextLCol;

            internal TblValueLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ValueLang2","VA2"), config.ExtractTableName("ValueLang2","VALUE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueLang2", "ValuePool"  , "VALUEPOOL");
                this.ValuePoolCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueLang2", "ValueCode"  , "VALUECODE");
                this.ValueCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueLang2", "Unit"  , "UNIT");
                this.UnitCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueLang2", "ValueTextS"  , "VALUETEXTS");
                this.ValueTextSCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueLang2", "ValueTextL"  , "VALUETEXTL");
                this.ValueTextLCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table link values/ value set with groups
        /// </summary>
        public class TblValueGroup : Tab
        {

            /// <summary>
            /// Name of grouping.
            /// 
            /// See further in the description of the table Grouping.
            /// </summary>
            public Column4Parameterized GroupingCol;
            /// <summary>
            /// 
            /// See further in the description of the table Value.
            /// </summary>
            public Column4Parameterized GroupCodeCol;
            /// <summary>
            /// Code for the value contained in the group. Retrieved from the table Value, column value code.
            /// See further in the description of the table Value
            /// Se beskrivning av tabellen Varde.
            /// </summary>
            public Column4Parameterized ValueCodeCol;
            /// <summary>
            /// The name of the value set, the grouping is attached.
            /// See further in the description of the table ValuePool
            /// 
            /// Se beskrivning av tabellen Vardeforrad.
            /// </summary>
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Indicates witch level the group code belongs to.
            /// </summary>
            public Column4Parameterized GroupLevelCol;
            /// <summary>
            /// Indicates witch level the value code belongs to.
            /// </summary>
            public Column4Parameterized ValueLevelCol;
            /// <summary>
            /// Code for sorting groups within a group, in order to present them in a logical order.
            /// 
            /// If any group within a grouping of a range is the sort code, all the teams have that. If the sort code is missing, the field shall be NULL.
            /// </summary>
            public Column4Parameterized SortCodeCol;

            internal TblValueGroup(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ValueGroup","VPL"), config.ExtractTableName("ValueGroup","VALUEGROUP"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueGroup", "Grouping", "GROUPING");
                this.GroupingCol = new Column4Parameterized(tmpColumnName, this.Alias,"Grouping",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueGroup", "GroupCode", "GROUPCODE");
                this.GroupCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"GroupCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueGroup", "ValueCode", "VALUECODE");
                this.ValueCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueCode",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueGroup", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueGroup", "GroupLevel", "GROUPLEVEL");
                this.GroupLevelCol = new Column4Parameterized(tmpColumnName, this.Alias,"GroupLevel",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueGroup", "ValueLevel", "VALUELEVEL");
                this.ValueLevelCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueLevel",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueGroup", "SortCode", "SORTCODE");
                this.SortCodeCol = new Column4Parameterized(tmpColumnName, this.Alias,"SortCode",config.GetDataProvider());

            }

        }

        public class TblValueGroupLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of grouping.
            /// 
            /// See further in the description of the table Grouping.
            /// </summary>
            public Lang2Column4Parameterized GroupingCol;

            /// <summary>
            /// 
            /// See further in the description of the table Value.
            /// </summary>
            public Lang2Column4Parameterized GroupCodeCol;

            /// <summary>
            /// Code for the value contained in the group. Retrieved from the table Value, column value code.
            /// See further in the description of the table Value
            /// Se beskrivning av tabellen Varde.
            /// </summary>
            public Lang2Column4Parameterized ValueCodeCol;

            /// <summary>
            /// Code for sorting groups within a group, in order to present them in a logical order.
            /// 
            /// If any group within a grouping of a range is the sort code, all the teams have that. If the sort code is missing, the field shall be NULL.
            /// </summary>
            public Lang2Column4Parameterized SortCodeCol;

            internal TblValueGroupLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ValueGroupLang2","VL2"), config.ExtractTableName("ValueGroupLang2","VALUEGROUP_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueGroupLang2", "Grouping"  , "GROUPING");
                this.GroupingCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueGroupLang2", "GroupCode"  , "GROUPCODE");
                this.GroupCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueGroupLang2", "ValueCode"  , "VALUECODE");
                this.ValueCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueGroupLang2", "SortCode"  , "SORTCODE");
                this.SortCodeCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table describes which value pools exist in the database. The value pool brings together all values and aggregates for a classification or a variation of a classification.
        /// </summary>
        public class TblValuePool : Tab
        {

            /// <summary>
            /// Name of value pool.
            /// 
            /// If there is only one variable belonging to a particular value pool, the variable and value pool should have the same name.
            /// 
            /// The name should begin with a capital letter.
            /// </summary>
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Can be used to give the valuepool an alternative name.
            /// </summary>
            public Column4Parameterized ValuePoolAliasCol;
            /// <summary>
            /// Presentation text for the value pool.
            /// 
            /// If there is no text, the field should be NULL.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Description of value pool.
            /// 
            /// Should also contain information on the principles used for sorting the value pool's values (i.e....sorting by particular principle,....sorting by value code).
            /// 
            /// Written beginning with a capital letter.
            /// </summary>
            public Column4Parameterized DescriptionCol;
            /// <summary>
            /// Here it is stated whether there are texts or not for the value pool's values, and whether they are in the table in ValueExtra. There are the following alternatives:
            /// 
            /// L = Long value text exists
            /// S = Short value text exists
            /// B = Both long and short value text exist
            /// N = No value texts for any values
            /// 
            /// 
            /// In the table Value (see descriptions of these columns) there are two columns for value texts, ValueTextS (for short texts) and ValueTextL (for long texts). If ValueTextExists = L, the value text is taken from column ValueTextL in the table Value. If ValueTextExists = S, the value text is taken from column ValueTextS in the table Value. If ValueTextExists = B, the value presentation is determined by what is specified in the field ValuePres in the tables ValuePool or ValueSet. If ValueTextExists = N, the values are presented only by a code in the retrieval interface.
            /// </summary>
            public Column4Parameterized ValueTextExistsCol;
            /// <summary>
            /// Here it is shown how the values should be presented after retrieval. There are the following alternatives:
            /// 
            /// A = Both code and short text should be presented
            /// B = Both code and long text should be presented
            /// C = Value code should be presented
            /// T = Long value text should be presented
            /// S = Short value text should be presented
            /// </summary>
            public Column4Parameterized ValuePresCol;
            /// <summary>
            /// MetaId can be used to link the information in this table to an external system.
            /// </summary>
            public Column4Parameterized MetaIdCol;

            internal TblValuePool(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ValuePool","VPL"), config.ExtractTableName("ValuePool","VALUEPOOL"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValuePool", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValuePool", "ValuePoolAlias", "VALUEPOOLALIAS");
                this.ValuePoolAliasCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePoolAlias",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValuePool", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValuePool", "Description", "DESCRIPTION");
                this.DescriptionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Description",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValuePool", "ValueTextExists", "VALUETEXTEXISTS");
                this.ValueTextExistsCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueTextExists",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValuePool", "ValuePres", "VALUEPRES");
                this.ValuePresCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePres",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValuePool", "MetaId", "METAID");
                this.MetaIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"MetaId",config.GetDataProvider());

            }

        }

        public class TblValuePoolLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of value pool.
            /// 
            /// If there is only one variable belonging to a particular value pool, the variable and value pool should have the same name.
            /// 
            /// The name should begin with a capital letter.
            /// </summary>
            public Lang2Column4Parameterized ValuePoolCol;

            /// <summary>
            /// Can be used to give the valuepool an alternative name.
            /// </summary>
            public Lang2Column4Parameterized ValuePoolAliasCol;

            /// <summary>
            /// Presentation text for the value pool.
            /// 
            /// If there is no text, the field should be NULL.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            internal TblValuePoolLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ValuePoolLang2","VP2"), config.ExtractTableName("ValuePoolLang2","VALUEPOOL_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValuePoolLang2", "ValuePool"  , "VALUEPOOL");
                this.ValuePoolCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValuePoolLang2", "ValuePoolAlias"  , "VALUEPOOLALIAS");
                this.ValuePoolAliasCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValuePoolLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table describes the value sets that exist for the different value pools.
        /// </summary>
        public class TblValueSet : Tab
        {

            /// <summary>
            /// Name of the stored value set.
            /// 
            /// The name should consist of the name of the value pool that the value set is linked to, plus a suffix. The suffix should always be used, even if there is only one value set for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.
            /// </summary>
            public Column4Parameterized ValueSetCol;
            /// <summary>
            /// Presentation text for value set. Can be used, if needed, as presentation text for the variable in the retrieval programs. It will then be the the variable name in the px file.
            /// 
            /// If the field is not used, it should be NULL.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Description of the content of the value set.
            /// 
            /// The text should give a picture of the integral values, classes, aggregates and any totals, and should end with information on the number of values in the value set, including the total.
            /// 
            /// Text should begin with a capital letter.
            /// </summary>
            public Column4Parameterized DescriptionCol;
            /// <summary>
            /// Here it should be shown whether the variable can be eliminated or not.
            /// 
            /// Elimination means that the variable can be excluded when selecting the value when retrieving from the databases. The variable must in that case be able to assume a value, i.e. the sum of all integral values or another specific value, that is included in the value set. There are the following alternatives:
            /// 
            /// N = No elimination value, i.e. the variable cannot be eliminated
            /// A = Elimination value is obtained by aggregation of all values in the value set
            /// ValueCode = a selected value, included in the value set, that should be used at elimination.
            /// </summary>
            public Column4Parameterized EliminationCol;
            /// <summary>
            /// Name of the value pool that the value set belongs to. See further description of the table ValuePool.
            /// </summary>
            public Column4Parameterized ValuePoolCol;
            /// <summary>
            /// Used to show how the values in a value set should be presented when being retrieved. There are the following alternatives:
            /// 
            /// A = Both code and short text should be presented
            /// B = Both code and long text should be presented
            /// K = Value code should be presented
            /// S = Short value text should be presented
            /// T = Long value text should be presented
            /// V = Presentation format is taken from the column ValuePres in the table ValuePool
            /// </summary>
            public Column4Parameterized ValuePresCol;
            /// <summary>
            /// Should contain the identification of a map that is suitable for the variable and the grouping. The field must be filled in if the column VariableType in the table SubTableVariable = G, otherwise the field is NULL.
            /// 
            /// The identification number should also be included in the table TextCatalog. For further information see description of TextCatalog.
            /// </summary>
            public Column4Parameterized GeoAreaNoCol;
            /// <summary>
            /// MetaId can be used to link the information in this table to an external system.
            /// </summary>
            public Column4Parameterized MetaIdCol;
            /// <summary>
            /// Code showing whether there is a particular sorting order for the value set. Can be:
            /// 
            /// Y = Yes
            /// N = No
            /// 
            /// If SortCodeExists = Y, the sorting code must be in VSValue for all values included in the value set.
            /// If SortCodeExists = N, the sorting code for the value pool is used (SortCode in the table Value).
            /// </summary>
            public Column4Parameterized SortCodeExistsCol;
            /// <summary>
            /// Shows whether there is a footnote linked to a value in the value set (FootNoteType 6). There are the following alternatives:
            /// 
            /// B = Both obligatory and optional footnotes exist
            /// V = One or several optional footnotes exist.
            /// O = One or several obligatory footnotes exist
            /// N = There are no footnotes
            /// </summary>
            public Column4Parameterized FootnoteCol;

            internal TblValueSet(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ValueSet","VST"), config.ExtractTableName("ValueSet","VALUESET"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueSet", "ValueSet", "VALUESET");
                this.ValueSetCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueSet",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSet", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSet", "Description", "DESCRIPTION");
                this.DescriptionCol = new Column4Parameterized(tmpColumnName, this.Alias,"Description",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSet", "Elimination", "ELIMINATION");
                this.EliminationCol = new Column4Parameterized(tmpColumnName, this.Alias,"Elimination",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSet", "ValuePool", "VALUEPOOL");
                this.ValuePoolCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePool",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSet", "ValuePres", "VALUEPRES");
                this.ValuePresCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValuePres",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSet", "GeoAreaNo", "GEOAREANO");
                this.GeoAreaNoCol = new Column4Parameterized(tmpColumnName, this.Alias,"GeoAreaNo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSet", "MetaId", "METAID");
                this.MetaIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"MetaId",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSet", "SortCodeExists", "SORTCODEEXISTS");
                this.SortCodeExistsCol = new Column4Parameterized(tmpColumnName, this.Alias,"SortCodeExists",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSet", "Footnote", "FOOTNOTE");
                this.FootnoteCol = new Column4Parameterized(tmpColumnName, this.Alias,"Footnote",config.GetDataProvider());

            }

        }

        public class TblValueSetLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of the stored value set.
            /// 
            /// The name should consist of the name of the value pool that the value set is linked to, plus a suffix. The suffix should always be used, even if there is only one value set for a value pool. The suffix can be distinguished by a short text beginning with a capital letter, or a single capital letter or number.
            /// </summary>
            public Lang2Column4Parameterized ValueSetCol;

            /// <summary>
            /// Presentation text for value set. Can be used, if needed, as presentation text for the variable in the retrieval programs. It will then be the the variable name in the px file.
            /// 
            /// If the field is not used, it should be NULL.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            /// <summary>
            /// Description of the content of the value set.
            /// 
            /// The text should give a picture of the integral values, classes, aggregates and any totals, and should end with information on the number of values in the value set, including the total.
            /// 
            /// Text should begin with a capital letter.
            /// </summary>
            public Lang2Column4Parameterized DescriptionCol;

            internal TblValueSetLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ValueSetLang2","VS2"), config.ExtractTableName("ValueSetLang2","VALUESET_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueSetLang2", "ValueSet"  , "VALUESET");
                this.ValueSetCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueSetLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("ValueSetLang2", "Description"  , "DESCRIPTION");
                this.DescriptionCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
            }

        }


        /// <summary>
        /// The table connects value set to grouping
        /// </summary>
        public class TblValueSetGrouping : Tab
        {

            /// <summary>
            /// Name of the stored value set.
            /// 
            /// See description of table ValueSet.
            /// </summary>
            public Column4Parameterized ValueSetCol;
            /// <summary>
            /// Name of grouping.
            /// 
            /// See further in the description of the table Grouping.
            /// </summary>
            public Column4Parameterized GroupingCol;

            internal TblValueSetGrouping(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("ValueSetGrouping","VBL"), config.ExtractTableName("ValueSetGrouping","VALUESETGROUPING"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("ValueSetGrouping", "ValueSet", "VALUESET");
                this.ValueSetCol = new Column4Parameterized(tmpColumnName, this.Alias,"ValueSet",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("ValueSetGrouping", "Grouping", "GROUPING");
                this.GroupingCol = new Column4Parameterized(tmpColumnName, this.Alias,"Grouping",config.GetDataProvider());

            }

        }

        /// <summary>
        /// The table contains the distributed statistical variables in the database.
        /// </summary>
        public class TblVariable : Tab
        {

            /// <summary>
            /// Name of distributed statistical variable. Name of metadata column for the variable in the data table.
            /// 
            /// The variable name must be unique within a main table.
            /// 
            /// The name should be descriptive, i.e. have an obvious link to the presentation text, consist of a maximum of 20 characters, begin with a capital letter and should only contains letters (a-z) and numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
            /// </summary>
            public Column4Parameterized VariableCol;
            /// <summary>
            /// Presentation text for a variable. Used in the retrieval interface when selecting variables or values and in the heading text when the table is presented after retrieval.
            /// 
            /// The entire text should be written in lower case letters, with the exception of abbreviations, etc.
            /// </summary>
            public Column4Parameterized PresTextCol;
            /// <summary>
            /// Descriptive information on variables, primarily for internal use, to facilitate the selection of a variable when drawing up new tables.
            /// 
            /// If there is no text, the field should be NULL.
            /// </summary>
            public Column4Parameterized VariableInfoCol;
            /// <summary>
            /// MetaId can be used to link the information in this table to an external system.
            /// </summary>
            public Column4Parameterized MetaIdCol;
            /// <summary>
            /// Shows whether there is a footnote linked to the variable (FootnoteType 5). There are the following alternatives:
            /// 
            /// B = Both obligatory and optional footnotes exist
            /// V = One or several optional footnotes exist.
            /// O = One or several obligatory footnotes exist
            /// S = There are no footnotes
            /// </summary>
            public Column4Parameterized FootnoteCol;

            internal TblVariable(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("Variable","VBL"), config.ExtractTableName("Variable","VARIABLE"), config.MetaOwner)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("Variable", "Variable", "VARIABLE");
                this.VariableCol = new Column4Parameterized(tmpColumnName, this.Alias,"Variable",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Variable", "PresText", "PRESTEXT");
                this.PresTextCol = new Column4Parameterized(tmpColumnName, this.Alias,"PresText",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Variable", "VariableInfo", "VARIABLEINFO");
                this.VariableInfoCol = new Column4Parameterized(tmpColumnName, this.Alias,"VariableInfo",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Variable", "MetaId", "METAID");
                this.MetaIdCol = new Column4Parameterized(tmpColumnName, this.Alias,"MetaId",config.GetDataProvider());
                tmpColumnName = config.ExtractColumnName("Variable", "Footnote", "FOOTNOTE");
                this.FootnoteCol = new Column4Parameterized(tmpColumnName, this.Alias,"Footnote",config.GetDataProvider());

            }

        }

        public class TblVariableLang2 : Lang2Tab
        {

            /// <summary>
            /// Name of distributed statistical variable. Name of metadata column for the variable in the data table.
            /// 
            /// The variable name must be unique within a main table.
            /// 
            /// The name should be descriptive, i.e. have an obvious link to the presentation text, consist of a maximum of 20 characters, begin with a capital letter and should only contains letters (a-z) and numbers. The name can also be broken down using the standard "PascalCase",  i.e. CapitalsAndLowerCase.
            /// </summary>
            public Lang2Column4Parameterized VariableCol;

            /// <summary>
            /// Presentation text for a variable. Used in the retrieval interface when selecting variables or values and in the heading text when the table is presented after retrieval.
            /// 
            /// The entire text should be written in lower case letters, with the exception of abbreviations, etc.
            /// </summary>
            public Lang2Column4Parameterized PresTextCol;

            internal TblVariableLang2(SqlDbConfig_23 config)
            : base(config.ExtractAliasName("VariableLang2","VB2"), config.ExtractTableName("VariableLang2","VARIABLE_"), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                string tmpColumnName ="";
                tmpColumnName = config.ExtractColumnName("VariableLang2", "Variable"  , "VARIABLE");
                this.VariableCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
                tmpColumnName = config.ExtractColumnName("VariableLang2", "PresText"  , "PRESTEXT");
                this.PresTextCol = new Lang2Column4Parameterized(tmpColumnName, this.Alias, this.Suffixes);
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
            Codes.RoleMain = ExtractCode("RoleMain", "M");
            Codes.RoleContact = ExtractCode("RoleContact", "C");
            Codes.RoleUpdate = ExtractCode("RoleUpdate", "U");
            Codes.RoleVerify = ExtractCode("RoleVerify", "V");
            Codes.RoleInternationalReporting = ExtractCode("RoleInternationalReporting", "V");
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
            public String RoleMain;
            public String RoleContact;
            public String RoleUpdate;
            public String RoleVerify;
            public String RoleInternationalReporting;
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
