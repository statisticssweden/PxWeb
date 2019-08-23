using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using log4net;

//This code is generated. 

namespace PCAxis.Sql.DbConfig
{
    public class SqlDbConfig_21 : SqlDbConfig
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(SqlDbConfig_21));

        public Ccodes Codes;
        public DbKeywords Keywords;

        #region Fields
        public TblContents Contents;
        public TblContentsLang2 ContentsLang2;
        public TblContentsTime ContentsTime;
        public TblColumnCode ColumnCode;
        public TblColumnCodeLang2 ColumnCodeLang2;
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
        public TblFootnoteValueSet FootnoteValueSet;
        public TblFootnoteVariable FootnoteVariable;
        public TblGrouping Grouping;
        public TblGroupingLang2 GroupingLang2;
        public TblGroupingLevel GroupingLevel;
        public TblGroupingLevelLang2 GroupingLevelLang2;
        public TblLink Link;
        public TblLinkLang2 LinkLang2;
        public TblLinkMenuSel LinkMenuSel;
        public TblLinkMenuSel LinkMenuSelection; ////hack for menu: this is the 2.3 name for LinkMenuSel.  
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
        public TblValuePool ValuePool;
        public TblValuePoolLang2 ValuePoolLang2;
        public TblValueSet ValueSet;
        public TblValueSetLang2 ValueSetLang2;
        public TblValueSetGrouping ValueSetGrouping;
        public TblVariable Variable;
        public TblVariableLang2 VariableLang2;
        public TblVSGroup VSGroup;
        public TblVSGroupLang2 VSGroupLang2;
        public TblVSValue VSValue;
        public TblVSValueLang2 VSValueLang2;
        #endregion Fields

        private void initStructs()
        {


            Contents = new TblContents("Contents", this);

            ContentsLang2 = new TblContentsLang2("ContentsLang2", this);

            ContentsTime = new TblContentsTime("ContentsTime", this);

            ColumnCode = new TblColumnCode("ColumnCode", this);

            ColumnCodeLang2 = new TblColumnCodeLang2("ColumnCodeLang2", this);

            DataStorage = new TblDataStorage("DataStorage", this);

            Footnote = new TblFootnote("Footnote", this);

            FootnoteLang2 = new TblFootnoteLang2("FootnoteLang2", this);

            FootnoteContTime = new TblFootnoteContTime("FootnoteContTime", this);

            FootnoteContValue = new TblFootnoteContValue("FootnoteContValue", this);

            FootnoteContVbl = new TblFootnoteContVbl("FootnoteContVbl", this);

            FootnoteContents = new TblFootnoteContents("FootnoteContents", this);

            if (String.Compare(this.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
            {
                FootnoteGrouping = new TblFootnoteGrouping("FootnoteGrouping", this);
            }

            FootnoteMainTable = new TblFootnoteMainTable("FootnoteMainTable", this);

            if (String.Compare(this.MetaModel, "2.0", false, CultureInfo.InvariantCulture) > 0)
            {
                FootnoteMaintValue = new TblFootnoteMaintValue("FootnoteMaintValue", this);
            }

            FootnoteMenuSel = new TblFootnoteMenuSel("FootnoteMenuSel", this);

            FootnoteSubTable = new TblFootnoteSubTable("FootnoteSubTable", this);

            FootnoteValue = new TblFootnoteValue("FootnoteValue", this);

            if (String.Compare(this.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
            {
                FootnoteValueSet = new TblFootnoteValueSet("FootnoteValueSet", this);
            }

            FootnoteVariable = new TblFootnoteVariable("FootnoteVariable", this);

            Grouping = new TblGrouping("Grouping", this);

            GroupingLang2 = new TblGroupingLang2("GroupingLang2", this);

            if (String.Compare(this.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
            {
                GroupingLevel = new TblGroupingLevel("GroupingLevel", this);
            }

            if (String.Compare(this.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
            {
                GroupingLevelLang2 = new TblGroupingLevelLang2("GroupingLevelLang2", this);
            }

            Link = new TblLink("Link", this);

            LinkLang2 = new TblLinkLang2("LinkLang2", this);

            LinkMenuSel = new TblLinkMenuSel("LinkMenuSel", this);

            LinkMenuSelection = LinkMenuSel;

            MainTable = new TblMainTable("MainTable", this);

            MainTableLang2 = new TblMainTableLang2("MainTableLang2", this);

            MainTablePerson = new TblMainTablePerson("MainTablePerson", this);

            if (String.Compare(this.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
            {
                MainTableVariableHierarchy = new TblMainTableVariableHierarchy("MainTableVariableHierarchy", this);
            }

            MenuSelection = new TblMenuSelection("MenuSelection", this);

            MenuSelectionLang2 = new TblMenuSelectionLang2("MenuSelectionLang2", this);

            MetaAdm = new TblMetaAdm("MetaAdm", this);

            MetabaseInfo = new TblMetabaseInfo("MetabaseInfo", this);

            Organization = new TblOrganization("Organization", this);

            OrganizationLang2 = new TblOrganizationLang2("OrganizationLang2", this);

            Person = new TblPerson("Person", this);

            SpecialCharacter = new TblSpecialCharacter("SpecialCharacter", this);

            SpecialCharacterLang2 = new TblSpecialCharacterLang2("SpecialCharacterLang2", this);

            SubTable = new TblSubTable("SubTable", this);

            SubTableLang2 = new TblSubTableLang2("SubTableLang2", this);

            SubTableVariable = new TblSubTableVariable("SubTableVariable", this);

            TextCatalog = new TblTextCatalog("TextCatalog", this);

            TextCatalogLang2 = new TblTextCatalogLang2("TextCatalogLang2", this);

            TimeScale = new TblTimeScale("TimeScale", this);

            TimeScaleLang2 = new TblTimeScaleLang2("TimeScaleLang2", this);

            Value = new TblValue("Value", this);

            ValueLang2 = new TblValueLang2("ValueLang2", this);

            ValueExtra = new TblValueExtra("ValueExtra", this);

            ValueExtraLang2 = new TblValueExtraLang2("ValueExtraLang2", this);

            if (String.Compare(this.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
            {
                ValueGroup = new TblValueGroup("ValueGroup", this);
            }

            ValuePool = new TblValuePool("ValuePool", this);

            ValuePoolLang2 = new TblValuePoolLang2("ValuePoolLang2", this);

            ValueSet = new TblValueSet("ValueSet", this);

            ValueSetLang2 = new TblValueSetLang2("ValueSetLang2", this);

            if (String.Compare(this.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
            {
                ValueSetGrouping = new TblValueSetGrouping("ValueSetGrouping", this);
            }

            Variable = new TblVariable("Variable", this);

            VariableLang2 = new TblVariableLang2("VariableLang2", this);

            VSGroup = new TblVSGroup("VSGroup", this);

            VSGroupLang2 = new TblVSGroupLang2("VSGroupLang2", this);

            VSValue = new TblVSValue("VSValue", this);

            VSValueLang2 = new TblVSValueLang2("VSValueLang2", this);
        }

        public SqlDbConfig_21(XmlReader xmlReader, XPathNavigator nav)
        : base(xmlReader, nav)
        {

            log.Debug("SqlDbConfig_21 called");

            this.initStructs();
            this.initCodesAndKeywords();
        }

        #region  structs

         

        public class TblContents : Tab
        {
            public String MainTable;
            public String Contents;
            public String PresText;
            public String PresTextS;
            public String PresCode;
            public String Copyright;
            public String StatAuthority;
            public String Producer;
            public String LastUpdated;
            public String Published;
            public String Unit;
            public String PresDecimals;
            public String PresCellsZero;
            public String PresMissingLine;
            public String AggregPossible;
            public String RefPeriod;
            public String StockFA;
            public String BasePeriod;
            public String CFPrices;
            public String DayAdj;
            public String SeasAdj;
            public String FootnoteContents;
            public String FootnoteTime;
            public String FootnoteValue;
            public String FootnoteVariable;
            public String StoreNoChar;
            public String StoreDecimals;
            public String StoreFormat;
            public String StoreColumnNo;

            public Col MainTableCol;
            public Col ContentsCol;
            public Col PresTextCol;
            public Col PresTextSCol;
            public Col PresCodeCol;
            public Col CopyrightCol;
            public Col StatAuthorityCol;
            public Col ProducerCol;
            public Col LastUpdatedCol;
            public Col PublishedCol;
            public Col UnitCol;
            public Col PresDecimalsCol;
            public Col PresCellsZeroCol;
            public Col PresMissingLineCol;
            public Col AggregPossibleCol;
            public Col RefPeriodCol;
            public Col StockFACol;
            public Col BasePeriodCol;
            public Col CFPricesCol;
            public Col DayAdjCol;
            public Col SeasAdjCol;
            public Col FootnoteContentsCol;
            public Col FootnoteTimeCol;
            public Col FootnoteValueCol;
            public Col FootnoteVariableCol;
            public Col StoreNoCharCol;
            public Col StoreDecimalsCol;
            public Col StoreFormatCol;
            public Col StoreColumnNoCol;

            internal TblContents(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.Contents = config.ExtractColumnName(modelName, "Contents");
                this.ContentsCol = new Col(this.Contents, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);
                this.PresTextS = config.ExtractColumnName(modelName, "PresTextS");
                this.PresTextSCol = new Col(this.PresTextS, this.Alias);
                this.PresCode = config.ExtractColumnName(modelName, "PresCode");
                this.PresCodeCol = new Col(this.PresCode, this.Alias);
                this.Copyright = config.ExtractColumnName(modelName, "Copyright");
                this.CopyrightCol = new Col(this.Copyright, this.Alias);
                this.StatAuthority = config.ExtractColumnName(modelName, "StatAuthority");
                this.StatAuthorityCol = new Col(this.StatAuthority, this.Alias);
                this.Producer = config.ExtractColumnName(modelName, "Producer");
                this.ProducerCol = new Col(this.Producer, this.Alias);
                this.LastUpdated = config.ExtractColumnName(modelName, "LastUpdated");
                this.LastUpdatedCol = new Col(this.LastUpdated, this.Alias);
                this.Published = config.ExtractColumnName(modelName, "Published");
                this.PublishedCol = new Col(this.Published, this.Alias);
                this.Unit = config.ExtractColumnName(modelName, "Unit");
                this.UnitCol = new Col(this.Unit, this.Alias);
                this.PresDecimals = config.ExtractColumnName(modelName, "PresDecimals");
                this.PresDecimalsCol = new Col(this.PresDecimals, this.Alias);
                this.PresCellsZero = config.ExtractColumnName(modelName, "PresCellsZero");
                this.PresCellsZeroCol = new Col(this.PresCellsZero, this.Alias);
                if (String.Compare(config.MetaModel, "2.0", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.PresMissingLine = config.ExtractColumnName(modelName, "PresMissingLine");
                    this.PresMissingLineCol = new Col(this.PresMissingLine, this.Alias);
                }
                this.AggregPossible = config.ExtractColumnName(modelName, "AggregPossible");
                this.AggregPossibleCol = new Col(this.AggregPossible, this.Alias);
                this.RefPeriod = config.ExtractColumnName(modelName, "RefPeriod");
                this.RefPeriodCol = new Col(this.RefPeriod, this.Alias);
                this.StockFA = config.ExtractColumnName(modelName, "StockFA");
                this.StockFACol = new Col(this.StockFA, this.Alias);
                this.BasePeriod = config.ExtractColumnName(modelName, "BasePeriod");
                this.BasePeriodCol = new Col(this.BasePeriod, this.Alias);
                this.CFPrices = config.ExtractColumnName(modelName, "CFPrices");
                this.CFPricesCol = new Col(this.CFPrices, this.Alias);
                this.DayAdj = config.ExtractColumnName(modelName, "DayAdj");
                this.DayAdjCol = new Col(this.DayAdj, this.Alias);
                this.SeasAdj = config.ExtractColumnName(modelName, "SeasAdj");
                this.SeasAdjCol = new Col(this.SeasAdj, this.Alias);
                this.FootnoteContents = config.ExtractColumnName(modelName, "FootnoteContents");
                this.FootnoteContentsCol = new Col(this.FootnoteContents, this.Alias);
                this.FootnoteTime = config.ExtractColumnName(modelName, "FootnoteTime");
                this.FootnoteTimeCol = new Col(this.FootnoteTime, this.Alias);
                this.FootnoteValue = config.ExtractColumnName(modelName, "FootnoteValue");
                this.FootnoteValueCol = new Col(this.FootnoteValue, this.Alias);
                this.FootnoteVariable = config.ExtractColumnName(modelName, "FootnoteVariable");
                this.FootnoteVariableCol = new Col(this.FootnoteVariable, this.Alias);
                this.StoreNoChar = config.ExtractColumnName(modelName, "StoreNoChar");
                this.StoreNoCharCol = new Col(this.StoreNoChar, this.Alias);
                this.StoreDecimals = config.ExtractColumnName(modelName, "StoreDecimals");
                this.StoreDecimalsCol = new Col(this.StoreDecimals, this.Alias);
                this.StoreFormat = config.ExtractColumnName(modelName, "StoreFormat");
                this.StoreFormatCol = new Col(this.StoreFormat, this.Alias);
                this.StoreColumnNo = config.ExtractColumnName(modelName, "StoreColumnNo");
                this.StoreColumnNoCol = new Col(this.StoreColumnNo, this.Alias);

            }

        }

        public class TblContentsLang2 : Lang2Tab
        {
            public String MainTable;
            public String Contents;
            public String BasePeriod;
            public String PresText;
            public String PresTextS;
            public String RefPeriod;
            public String Unit;

            public Lang2Col MainTableCol;
            public Lang2Col ContentsCol;
            public Lang2Col BasePeriodCol;
            public Lang2Col PresTextCol;
            public Lang2Col PresTextSCol;
            public Lang2Col RefPeriodCol;
            public Lang2Col UnitCol;

            internal TblContentsLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Lang2Col(this.MainTable, this.Alias, this.Suffixes);
                this.Contents = config.ExtractColumnName(modelName, "Contents");
                this.ContentsCol = new Lang2Col(this.Contents, this.Alias, this.Suffixes);
                this.BasePeriod = config.ExtractColumnName(modelName, "BasePeriod");
                this.BasePeriodCol = new Lang2Col(this.BasePeriod, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
                this.PresTextS = config.ExtractColumnName(modelName, "PresTextS");
                this.PresTextSCol = new Lang2Col(this.PresTextS, this.Alias, this.Suffixes);
                this.RefPeriod = config.ExtractColumnName(modelName, "RefPeriod");
                this.RefPeriodCol = new Lang2Col(this.RefPeriod, this.Alias, this.Suffixes);
                this.Unit = config.ExtractColumnName(modelName, "Unit");
                this.UnitCol = new Lang2Col(this.Unit, this.Alias, this.Suffixes);
            }

        }


        public class TblContentsTime : Tab
        {
            public String MainTable;
            public String Contents;
            public String TimePeriod;

            public Col MainTableCol;
            public Col ContentsCol;
            public Col TimePeriodCol;

            internal TblContentsTime(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.Contents = config.ExtractColumnName(modelName, "Contents");
                this.ContentsCol = new Col(this.Contents, this.Alias);
                this.TimePeriod = config.ExtractColumnName(modelName, "TimePeriod");
                this.TimePeriodCol = new Col(this.TimePeriod, this.Alias);

            }

        }

        public class TblColumnCode : Tab
        {
            public String MetaTable;
            public String ColumnName;
            public String Code;
            public String PresText;

            public Col MetaTableCol;
            public Col ColumnNameCol;
            public Col CodeCol;
            public Col PresTextCol;

            internal TblColumnCode(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MetaTable = config.ExtractColumnName(modelName, "MetaTable");
                this.MetaTableCol = new Col(this.MetaTable, this.Alias);
                this.ColumnName = config.ExtractColumnName(modelName, "ColumnName");
                this.ColumnNameCol = new Col(this.ColumnName, this.Alias);
                this.Code = config.ExtractColumnName(modelName, "Code");
                this.CodeCol = new Col(this.Code, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);

            }

        }

        public class TblColumnCodeLang2 : Lang2Tab
        {
            public String MetaTable;
            public String ColumnName;
            public String Code;
            public String CodeEng;
            public String PresText;

            public Lang2Col MetaTableCol;
            public Lang2Col ColumnNameCol;
            public Lang2Col CodeCol;
            public Lang2Col CodeEngCol;
            public Lang2Col PresTextCol;

            internal TblColumnCodeLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.MetaTable = config.ExtractColumnName(modelName, "MetaTable");
                this.MetaTableCol = new Lang2Col(this.MetaTable, this.Alias, this.Suffixes);
                this.ColumnName = config.ExtractColumnName(modelName, "ColumnName");
                this.ColumnNameCol = new Lang2Col(this.ColumnName, this.Alias, this.Suffixes);
                this.Code = config.ExtractColumnName(modelName, "Code");
                this.CodeCol = new Lang2Col(this.Code, this.Alias, this.Suffixes);
                this.CodeEng = config.ExtractColumnName(modelName, "CodeEng");
                this.CodeEngCol = new Lang2Col(this.CodeEng, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
            }

        }


        public class TblDataStorage : Tab
        {
            public String ProductId;
            public String ServerName;
            public String DatabaseName;

            public Col ProductIdCol;
            public Col ServerNameCol;
            public Col DatabaseNameCol;

            internal TblDataStorage(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ProductId = config.ExtractColumnName(modelName, "ProductId");
                this.ProductIdCol = new Col(this.ProductId, this.Alias);
                this.ServerName = config.ExtractColumnName(modelName, "ServerName");
                this.ServerNameCol = new Col(this.ServerName, this.Alias);
                this.DatabaseName = config.ExtractColumnName(modelName, "DatabaseName");
                this.DatabaseNameCol = new Col(this.DatabaseName, this.Alias);

            }

        }

        public class TblFootnote : Tab
        {
            public String FootnoteNo;
            public String FootnoteType;
            public String ShowFootnote;
            public String MandOpt;
            public String GeneralSpecific;
            public String FootnoteText;

            public Col FootnoteNoCol;
            public Col FootnoteTypeCol;
            public Col ShowFootnoteCol;
            public Col MandOptCol;
            public Col GeneralSpecificCol;
            public Col FootnoteTextCol;

            internal TblFootnote(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);
                this.FootnoteType = config.ExtractColumnName(modelName, "FootnoteType");
                this.FootnoteTypeCol = new Col(this.FootnoteType, this.Alias);
                this.ShowFootnote = config.ExtractColumnName(modelName, "ShowFootnote");
                this.ShowFootnoteCol = new Col(this.ShowFootnote, this.Alias);
                this.MandOpt = config.ExtractColumnName(modelName, "MandOpt");
                this.MandOptCol = new Col(this.MandOpt, this.Alias);
                if (String.Compare(config.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.GeneralSpecific = config.ExtractColumnName(modelName, "GeneralSpecific");
                    this.GeneralSpecificCol = new Col(this.GeneralSpecific, this.Alias);
                }
                this.FootnoteText = config.ExtractColumnName(modelName, "FootnoteText");
                this.FootnoteTextCol = new Col(this.FootnoteText, this.Alias);

            }

        }

        public class TblFootnoteLang2 : Lang2Tab
        {
            public String FootnoteNo;
            public String FootnoteText;

            public Lang2Col FootnoteNoCol;
            public Lang2Col FootnoteTextCol;

            internal TblFootnoteLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Lang2Col(this.FootnoteNo, this.Alias, this.Suffixes);
                this.FootnoteText = config.ExtractColumnName(modelName, "FootnoteText");
                this.FootnoteTextCol = new Lang2Col(this.FootnoteText, this.Alias, this.Suffixes);
            }

        }


        public class TblFootnoteContTime : Tab
        {
            public String MainTable;
            public String Contents;
            public String TimePeriod;
            public String FootnoteNo;
            public String Cellnote;

            public Col MainTableCol;
            public Col ContentsCol;
            public Col TimePeriodCol;
            public Col FootnoteNoCol;
            public Col CellnoteCol;

            internal TblFootnoteContTime(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.Contents = config.ExtractColumnName(modelName, "Contents");
                this.ContentsCol = new Col(this.Contents, this.Alias);
                this.TimePeriod = config.ExtractColumnName(modelName, "TimePeriod");
                this.TimePeriodCol = new Col(this.TimePeriod, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);
                this.Cellnote = config.ExtractColumnName(modelName, "Cellnote");
                this.CellnoteCol = new Col(this.Cellnote, this.Alias);

            }

        }

        public class TblFootnoteContValue : Tab
        {
            public String MainTable;
            public String Contents;
            public String Variable;
            public String ValuePool;
            public String ValueCode;
            public String FootnoteNo;
            public String Cellnote;

            public Col MainTableCol;
            public Col ContentsCol;
            public Col VariableCol;
            public Col ValuePoolCol;
            public Col ValueCodeCol;
            public Col FootnoteNoCol;
            public Col CellnoteCol;

            internal TblFootnoteContValue(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.Contents = config.ExtractColumnName(modelName, "Contents");
                this.ContentsCol = new Col(this.Contents, this.Alias);
                this.Variable = config.ExtractColumnName(modelName, "Variable");
                this.VariableCol = new Col(this.Variable, this.Alias);
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Col(this.ValueCode, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);
                this.Cellnote = config.ExtractColumnName(modelName, "Cellnote");
                this.CellnoteCol = new Col(this.Cellnote, this.Alias);

            }

        }

        public class TblFootnoteContVbl : Tab
        {
            public String MainTable;
            public String Contents;
            public String Variable;
            public String FootnoteNo;

            public Col MainTableCol;
            public Col ContentsCol;
            public Col VariableCol;
            public Col FootnoteNoCol;

            internal TblFootnoteContVbl(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.Contents = config.ExtractColumnName(modelName, "Contents");
                this.ContentsCol = new Col(this.Contents, this.Alias);
                this.Variable = config.ExtractColumnName(modelName, "Variable");
                this.VariableCol = new Col(this.Variable, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblFootnoteContents : Tab
        {
            public String MainTable;
            public String Contents;
            public String FootnoteNo;

            public Col MainTableCol;
            public Col ContentsCol;
            public Col FootnoteNoCol;

            internal TblFootnoteContents(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.Contents = config.ExtractColumnName(modelName, "Contents");
                this.ContentsCol = new Col(this.Contents, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblFootnoteGrouping : Tab
        {
            public String Grouping;
            public String FootnoteNo;

            public Col GroupingCol;
            public Col FootnoteNoCol;

            internal TblFootnoteGrouping(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Col(this.Grouping, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblFootnoteMainTable : Tab
        {
            public String MainTable;
            public String FootnoteNo;

            public Col MainTableCol;
            public Col FootnoteNoCol;

            internal TblFootnoteMainTable(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblFootnoteMaintValue : Tab
        {
            public String MainTable;
            public String Variable;
            public String ValuePool;
            public String ValueCode;
            public String FootnoteNo;

            public Col MainTableCol;
            public Col VariableCol;
            public Col ValuePoolCol;
            public Col ValueCodeCol;
            public Col FootnoteNoCol;

            internal TblFootnoteMaintValue(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.Variable = config.ExtractColumnName(modelName, "Variable");
                this.VariableCol = new Col(this.Variable, this.Alias);
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Col(this.ValueCode, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblFootnoteMenuSel : Tab
        {
            public String Menu;
            public String Selection;
            public String FootnoteNo;

            public Col MenuCol;
            public Col SelectionCol;
            public Col FootnoteNoCol;

            internal TblFootnoteMenuSel(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Menu = config.ExtractColumnName(modelName, "Menu");
                this.MenuCol = new Col(this.Menu, this.Alias);
                this.Selection = config.ExtractColumnName(modelName, "Selection");
                this.SelectionCol = new Col(this.Selection, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblFootnoteSubTable : Tab
        {
            public String MainTable;
            public String SubTable;
            public String FootnoteNo;

            public Col MainTableCol;
            public Col SubTableCol;
            public Col FootnoteNoCol;

            internal TblFootnoteSubTable(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.SubTable = config.ExtractColumnName(modelName, "SubTable");
                this.SubTableCol = new Col(this.SubTable, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblFootnoteValue : Tab
        {
            public String ValuePool;
            public String ValueCode;
            public String FootnoteNo;

            public Col ValuePoolCol;
            public Col ValueCodeCol;
            public Col FootnoteNoCol;

            internal TblFootnoteValue(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Col(this.ValueCode, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblFootnoteValueSet : Tab
        {
            public String ValueSet;
            public String ValueCode;
            public String FootnoteNo;

            public Col ValueSetCol;
            public Col ValueCodeCol;
            public Col FootnoteNoCol;

            internal TblFootnoteValueSet(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValueSet = config.ExtractColumnName(modelName, "ValueSet");
                this.ValueSetCol = new Col(this.ValueSet, this.Alias);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Col(this.ValueCode, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblFootnoteVariable : Tab
        {
            public String Variable;
            public String FootnoteNo;

            public Col VariableCol;
            public Col FootnoteNoCol;

            internal TblFootnoteVariable(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Variable = config.ExtractColumnName(modelName, "Variable");
                this.VariableCol = new Col(this.Variable, this.Alias);
                this.FootnoteNo = config.ExtractColumnName(modelName, "FootnoteNo");
                this.FootnoteNoCol = new Col(this.FootnoteNo, this.Alias);

            }

        }

        public class TblGrouping : Tab
        {
            public String ValuePool;
            public String Grouping;
            public String PresText;
            public String Description;
            public String GroupPres;
            public String GeoAreaNo;
            public String Hierarchy;
            public String KDBid;
            public String SortCode;

            public Col ValuePoolCol;
            public Col GroupingCol;
            public Col PresTextCol;
            public Col DescriptionCol;
            public Col GroupPresCol;
            public Col GeoAreaNoCol;
            public Col HierarchyCol;
            public Col KDBidCol;
            public Col SortCodeCol;

            internal TblGrouping(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Col(this.Grouping, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Col(this.Description, this.Alias);
                this.GroupPres = config.ExtractColumnName(modelName, "GroupPres");
                this.GroupPresCol = new Col(this.GroupPres, this.Alias);
                if (String.Compare(config.MetaModel, "2.1", false, CultureInfo.InvariantCulture) <= 0)
                {
                    this.GeoAreaNo = config.ExtractColumnName(modelName, "GeoAreaNo");
                    this.GeoAreaNoCol = new Col(this.GeoAreaNo, this.Alias);
                }
                if (String.Compare(config.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.Hierarchy = config.ExtractColumnName(modelName, "Hierarchy");
                    this.HierarchyCol = new Col(this.Hierarchy, this.Alias);
                }
                this.KDBid = config.ExtractColumnName(modelName, "KDBid");
                this.KDBidCol = new Col(this.KDBid, this.Alias);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Col(this.SortCode, this.Alias);

            }

        }

        public class TblGroupingLang2 : Lang2Tab
        {
            public String ValuePool;
            public String Grouping;
            public String Description;
            public String PresText;
            public String SortCode;

            public Lang2Col ValuePoolCol;
            public Lang2Col GroupingCol;
            public Lang2Col DescriptionCol;
            public Lang2Col PresTextCol;
            public Lang2Col SortCodeCol;

            internal TblGroupingLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Lang2Col(this.ValuePool, this.Alias, this.Suffixes);
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Lang2Col(this.Grouping, this.Alias, this.Suffixes);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Lang2Col(this.Description, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Lang2Col(this.SortCode, this.Alias, this.Suffixes);
            }

        }


        public class TblGroupingLevel : Tab
        {
            public String Grouping;
            public String Level;
            public String LevelText;
            public String GeoAreaNo;

            public Col GroupingCol;
            public Col LevelCol;
            public Col LevelTextCol;
            public Col GeoAreaNoCol;

            internal TblGroupingLevel(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Col(this.Grouping, this.Alias);
                this.Level = config.ExtractColumnName(modelName, "Level");
                this.LevelCol = new Col(this.Level, this.Alias);
                this.LevelText = config.ExtractColumnName(modelName, "LevelText");
                this.LevelTextCol = new Col(this.LevelText, this.Alias);
                this.GeoAreaNo = config.ExtractColumnName(modelName, "GeoAreaNo");
                this.GeoAreaNoCol = new Col(this.GeoAreaNo, this.Alias);

            }

        }

        public class TblGroupingLevelLang2 : Lang2Tab
        {
            public String Grouping;
            public String Level;
            public String LevelText;

            public Lang2Col GroupingCol;
            public Lang2Col LevelCol;
            public Lang2Col LevelTextCol;

            internal TblGroupingLevelLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Lang2Col(this.Grouping, this.Alias, this.Suffixes);
                this.Level = config.ExtractColumnName(modelName, "Level");
                this.LevelCol = new Lang2Col(this.Level, this.Alias, this.Suffixes);
                this.LevelText = config.ExtractColumnName(modelName, "LevelText");
                this.LevelTextCol = new Lang2Col(this.LevelText, this.Alias, this.Suffixes);
            }

        }


        public class TblLink : Tab
        {
            public String LinkId;
            public String Link;
            public String LinkType;
            public String LinkFormat;
            public String LinkText;
            public String PresCategory;
            public String LinkPres;
            public String SortCode;
            public String Description;

            public Col LinkIdCol;
            public Col LinkCol;
            public Col LinkTypeCol;
            public Col LinkFormatCol;
            public Col LinkTextCol;
            public Col PresCategoryCol;
            public Col LinkPresCol;
            public Col SortCodeCol;
            public Col DescriptionCol;

            internal TblLink(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.LinkId = config.ExtractColumnName(modelName, "LinkId");
                this.LinkIdCol = new Col(this.LinkId, this.Alias);
                this.Link = config.ExtractColumnName(modelName, "Link");
                this.LinkCol = new Col(this.Link, this.Alias);
                if (String.Compare(config.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.LinkType = config.ExtractColumnName(modelName, "LinkType");
                    this.LinkTypeCol = new Col(this.LinkType, this.Alias);
                }
                if (String.Compare(config.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.LinkFormat = config.ExtractColumnName(modelName, "LinkFormat");
                    this.LinkFormatCol = new Col(this.LinkFormat, this.Alias);
                }
                this.LinkText = config.ExtractColumnName(modelName, "LinkText");
                this.LinkTextCol = new Col(this.LinkText, this.Alias);
                this.PresCategory = config.ExtractColumnName(modelName, "PresCategory");
                this.PresCategoryCol = new Col(this.PresCategory, this.Alias);
                this.LinkPres = config.ExtractColumnName(modelName, "LinkPres");
                this.LinkPresCol = new Col(this.LinkPres, this.Alias);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Col(this.SortCode, this.Alias);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Col(this.Description, this.Alias);

            }

        }

        public class TblLinkLang2 : Lang2Tab
        {
            public String LinkId;
            public String Link;
            public String LinkText;
            public String SortCode;
            public String Description;

            public Lang2Col LinkIdCol;
            public Lang2Col LinkCol;
            public Lang2Col LinkTextCol;
            public Lang2Col SortCodeCol;
            public Lang2Col DescriptionCol;

            internal TblLinkLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.LinkId = config.ExtractColumnName(modelName, "LinkId");
                this.LinkIdCol = new Lang2Col(this.LinkId, this.Alias, this.Suffixes);
                this.Link = config.ExtractColumnName(modelName, "Link");
                this.LinkCol = new Lang2Col(this.Link, this.Alias, this.Suffixes);
                this.LinkText = config.ExtractColumnName(modelName, "LinkText");
                this.LinkTextCol = new Lang2Col(this.LinkText, this.Alias, this.Suffixes);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Lang2Col(this.SortCode, this.Alias, this.Suffixes);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Lang2Col(this.Description, this.Alias, this.Suffixes);
            }

        }


        public class TblLinkMenuSel : Tab
        {
            public String Menu;
            public String Selection;
            public String LinkId;

            public Col MenuCol;
            public Col SelectionCol;
            public Col LinkIdCol;

            internal TblLinkMenuSel(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Menu = config.ExtractColumnName(modelName, "Menu");
                this.MenuCol = new Col(this.Menu, this.Alias);
                this.Selection = config.ExtractColumnName(modelName, "Selection");
                this.SelectionCol = new Col(this.Selection, this.Alias);
                this.LinkId = config.ExtractColumnName(modelName, "LinkId");
                this.LinkIdCol = new Col(this.LinkId, this.Alias);

            }

        }

        public class TblMainTable : Tab
        {
            public String MainTable;
            public String TableStatus;
            public String PresText;
            public String PresTextS;
            public String ContentsVariable;
            public String TableId;
            public String PresCategory;
            public String SpecCharExists;
            public String StatusEng;
            public String SubjectCode;
            public String ProductId;
            public String TimeScale;

            public Col MainTableCol;
            public Col TableStatusCol;
            public Col PresTextCol;
            public Col PresTextSCol;
            public Col ContentsVariableCol;
            public Col TableIdCol;
            public Col PresCategoryCol;
            public Col SpecCharExistsCol;
            public Col StatusEngCol;
            public Col SubjectCodeCol;
            public Col ProductIdCol;
            public Col TimeScaleCol;

            internal TblMainTable(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.TableStatus = config.ExtractColumnName(modelName, "TableStatus");
                this.TableStatusCol = new Col(this.TableStatus, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);
                this.PresTextS = config.ExtractColumnName(modelName, "PresTextS");
                this.PresTextSCol = new Col(this.PresTextS, this.Alias);
                this.ContentsVariable = config.ExtractColumnName(modelName, "ContentsVariable");
                this.ContentsVariableCol = new Col(this.ContentsVariable, this.Alias);
                this.TableId = config.ExtractColumnName(modelName, "TableId");
                this.TableIdCol = new Col(this.TableId, this.Alias);
                this.PresCategory = config.ExtractColumnName(modelName, "PresCategory");
                this.PresCategoryCol = new Col(this.PresCategory, this.Alias);
                this.SpecCharExists = config.ExtractColumnName(modelName, "SpecCharExists");
                this.SpecCharExistsCol = new Col(this.SpecCharExists, this.Alias);
                if (String.Compare(config.MetaModel, "2.0", false, CultureInfo.InvariantCulture) <= 0)
                {
                    this.StatusEng = config.ExtractColumnName(modelName, "StatusEng");
                    this.StatusEngCol = new Col(this.StatusEng, this.Alias);
                }
                this.SubjectCode = config.ExtractColumnName(modelName, "SubjectCode");
                this.SubjectCodeCol = new Col(this.SubjectCode, this.Alias);
                this.ProductId = config.ExtractColumnName(modelName, "ProductId");
                this.ProductIdCol = new Col(this.ProductId, this.Alias);
                this.TimeScale = config.ExtractColumnName(modelName, "TimeScale");
                this.TimeScaleCol = new Col(this.TimeScale, this.Alias);

            }

        }

        public class TblMainTableLang2 : Lang2Tab
        {
            public String MainTable;
            public String Status;
            public String Published;
            public String PresText;
            public String PresTextS;
            public String ContentsVariable;

            public Lang2Col MainTableCol;
            public Lang2Col StatusCol;
            public Lang2Col PublishedCol;
            public Lang2Col PresTextCol;
            public Lang2Col PresTextSCol;
            public Lang2Col ContentsVariableCol;

            internal TblMainTableLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Lang2Col(this.MainTable, this.Alias, this.Suffixes);
                if (String.Compare(config.MetaModel, "2.0", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.Status = config.ExtractColumnName(modelName, "Status");
                    this.StatusCol = new Lang2Col(this.Status, this.Alias, this.Suffixes);
                }
                if (String.Compare(config.MetaModel, "2.0", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.Published = config.ExtractColumnName(modelName, "Published");
                    this.PublishedCol = new Lang2Col(this.Published, this.Alias, this.Suffixes);
                }
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
                this.PresTextS = config.ExtractColumnName(modelName, "PresTextS");
                this.PresTextSCol = new Lang2Col(this.PresTextS, this.Alias, this.Suffixes);
                this.ContentsVariable = config.ExtractColumnName(modelName, "ContentsVariable");
                this.ContentsVariableCol = new Lang2Col(this.ContentsVariable, this.Alias, this.Suffixes);
            }

        }


        public class TblMainTablePerson : Tab
        {
            public String MainTable;
            public String PersonCode;
            public String RolePerson;

            public Col MainTableCol;
            public Col PersonCodeCol;
            public Col RolePersonCol;

            internal TblMainTablePerson(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.PersonCode = config.ExtractColumnName(modelName, "PersonCode");
                this.PersonCodeCol = new Col(this.PersonCode, this.Alias);
                this.RolePerson = config.ExtractColumnName(modelName, "RolePerson");
                this.RolePersonCol = new Col(this.RolePerson, this.Alias);

            }

        }

        public class TblMainTableVariableHierarchy : Tab
        {
            public String MainTable;
            public String Variable;
            public String Grouping;
            public String ShowLevels;
            public String AllLevelsStored;

            public Col MainTableCol;
            public Col VariableCol;
            public Col GroupingCol;
            public Col ShowLevelsCol;
            public Col AllLevelsStoredCol;

            internal TblMainTableVariableHierarchy(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.Variable = config.ExtractColumnName(modelName, "Variable");
                this.VariableCol = new Col(this.Variable, this.Alias);
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Col(this.Grouping, this.Alias);
                this.ShowLevels = config.ExtractColumnName(modelName, "ShowLevels");
                this.ShowLevelsCol = new Col(this.ShowLevels, this.Alias);
                this.AllLevelsStored = config.ExtractColumnName(modelName, "AllLevelsStored");
                this.AllLevelsStoredCol = new Col(this.AllLevelsStored, this.Alias);

            }

        }

        public class TblMenuSelection : Tab
        {
            public String Menu;
            public String Selection;
            public String PresText;
            public String PresTextS;
            public String Description;
            public String LevelNo;
            public String SortCode;
            public String Presentation;
            public String InternalId;

            public Col MenuCol;
            public Col SelectionCol;
            public Col PresTextCol;
            public Col PresTextSCol;
            public Col DescriptionCol;
            public Col LevelNoCol;
            public Col SortCodeCol;
            public Col PresentationCol;
            public Col InternalIdCol;

            internal TblMenuSelection(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Menu = config.ExtractColumnName(modelName, "Menu");
                this.MenuCol = new Col(this.Menu, this.Alias);
                this.Selection = config.ExtractColumnName(modelName, "Selection");
                this.SelectionCol = new Col(this.Selection, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);
                this.PresTextS = config.ExtractColumnName(modelName, "PresTextS");
                this.PresTextSCol = new Col(this.PresTextS, this.Alias);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Col(this.Description, this.Alias);
                this.LevelNo = config.ExtractColumnName(modelName, "LevelNo");
                this.LevelNoCol = new Col(this.LevelNo, this.Alias);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Col(this.SortCode, this.Alias);
                this.Presentation = config.ExtractColumnName(modelName, "Presentation");
                this.PresentationCol = new Col(this.Presentation, this.Alias);
                this.InternalId = config.ExtractColumnName(modelName, "InternalId");
                this.InternalIdCol = new Col(this.InternalId, this.Alias);

            }

        }

        public class TblMenuSelectionLang2 : Lang2Tab
        {
            public String Menu;
            public String Selection;
            public String PresText;
            public String PresTextS;
            public String Description;
            public String SortCode;
            public String Presentation;

            public Lang2Col MenuCol;
            public Lang2Col SelectionCol;
            public Lang2Col PresTextCol;
            public Lang2Col PresTextSCol;
            public Lang2Col DescriptionCol;
            public Lang2Col SortCodeCol;
            public Lang2Col PresentationCol;

            internal TblMenuSelectionLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.Menu = config.ExtractColumnName(modelName, "Menu");
                this.MenuCol = new Lang2Col(this.Menu, this.Alias, this.Suffixes);
                this.Selection = config.ExtractColumnName(modelName, "Selection");
                this.SelectionCol = new Lang2Col(this.Selection, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
                this.PresTextS = config.ExtractColumnName(modelName, "PresTextS");
                this.PresTextSCol = new Lang2Col(this.PresTextS, this.Alias, this.Suffixes);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Lang2Col(this.Description, this.Alias, this.Suffixes);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Lang2Col(this.SortCode, this.Alias, this.Suffixes);
                this.Presentation = config.ExtractColumnName(modelName, "Presentation");
                this.PresentationCol = new Lang2Col(this.Presentation, this.Alias, this.Suffixes);
            }

        }


        public class TblMetaAdm : Tab
        {
            public String Property;
            public String Value;
            public String Description;

            public Col PropertyCol;
            public Col ValueCol;
            public Col DescriptionCol;

            internal TblMetaAdm(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Property = config.ExtractColumnName(modelName, "Property");
                this.PropertyCol = new Col(this.Property, this.Alias);
                this.Value = config.ExtractColumnName(modelName, "Value");
                this.ValueCol = new Col(this.Value, this.Alias);
                if (String.Compare(config.MetaModel, "2.0", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.Description = config.ExtractColumnName(modelName, "Description");
                    this.DescriptionCol = new Col(this.Description, this.Alias);
                }

            }

        }

        public class TblMetabaseInfo : Tab
        {
            public String Model;
            public String ModelVersion;
            public String DatabaseRole;

            public Col ModelCol;
            public Col ModelVersionCol;
            public Col DatabaseRoleCol;

            internal TblMetabaseInfo(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Model = config.ExtractColumnName(modelName, "Model");
                this.ModelCol = new Col(this.Model, this.Alias);
                this.ModelVersion = config.ExtractColumnName(modelName, "ModelVersion");
                this.ModelVersionCol = new Col(this.ModelVersion, this.Alias);
                this.DatabaseRole = config.ExtractColumnName(modelName, "DatabaseRole");
                this.DatabaseRoleCol = new Col(this.DatabaseRole, this.Alias);

            }

        }

        public class TblOrganization : Tab
        {
            public String OrganizationCode;
            public String OrganizationName;
            public String Department;
            public String Unit;
            public String WebAddress;
            public String InternalId;

            public Col OrganizationCodeCol;
            public Col OrganizationNameCol;
            public Col DepartmentCol;
            public Col UnitCol;
            public Col WebAddressCol;
            public Col InternalIdCol;

            internal TblOrganization(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.OrganizationCode = config.ExtractColumnName(modelName, "OrganizationCode");
                this.OrganizationCodeCol = new Col(this.OrganizationCode, this.Alias);
                this.OrganizationName = config.ExtractColumnName(modelName, "OrganizationName");
                this.OrganizationNameCol = new Col(this.OrganizationName, this.Alias);
                this.Department = config.ExtractColumnName(modelName, "Department");
                this.DepartmentCol = new Col(this.Department, this.Alias);
                this.Unit = config.ExtractColumnName(modelName, "Unit");
                this.UnitCol = new Col(this.Unit, this.Alias);
                this.WebAddress = config.ExtractColumnName(modelName, "WebAddress");
                this.WebAddressCol = new Col(this.WebAddress, this.Alias);
                this.InternalId = config.ExtractColumnName(modelName, "InternalId");
                this.InternalIdCol = new Col(this.InternalId, this.Alias);

            }

        }

        public class TblOrganizationLang2 : Lang2Tab
        {
            public String OrganizationCode;
            public String OrganizationName;
            public String Department;
            public String Unit;
            public String WebAddress;

            public Lang2Col OrganizationCodeCol;
            public Lang2Col OrganizationNameCol;
            public Lang2Col DepartmentCol;
            public Lang2Col UnitCol;
            public Lang2Col WebAddressCol;

            internal TblOrganizationLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.OrganizationCode = config.ExtractColumnName(modelName, "OrganizationCode");
                this.OrganizationCodeCol = new Lang2Col(this.OrganizationCode, this.Alias, this.Suffixes);
                this.OrganizationName = config.ExtractColumnName(modelName, "OrganizationName");
                this.OrganizationNameCol = new Lang2Col(this.OrganizationName, this.Alias, this.Suffixes);
                this.Department = config.ExtractColumnName(modelName, "Department");
                this.DepartmentCol = new Lang2Col(this.Department, this.Alias, this.Suffixes);
                this.Unit = config.ExtractColumnName(modelName, "Unit");
                this.UnitCol = new Lang2Col(this.Unit, this.Alias, this.Suffixes);
                if (String.Compare(config.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.WebAddress = config.ExtractColumnName(modelName, "WebAddress");
                    this.WebAddressCol = new Lang2Col(this.WebAddress, this.Alias, this.Suffixes);
                }
            }

        }


        public class TblPerson : Tab
        {
            public String PersonCode;
            public String Forename;
            public String Surname;
            public String OrganizationCode;
            public String PhonePrefix;
            public String PhoneNo;
            public String FaxNo;
            public String Email;

            public Col PersonCodeCol;
            public Col ForenameCol;
            public Col SurnameCol;
            public Col OrganizationCodeCol;
            public Col PhonePrefixCol;
            public Col PhoneNoCol;
            public Col FaxNoCol;
            public Col EmailCol;

            internal TblPerson(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.PersonCode = config.ExtractColumnName(modelName, "PersonCode");
                this.PersonCodeCol = new Col(this.PersonCode, this.Alias);
                this.Forename = config.ExtractColumnName(modelName, "Forename");
                this.ForenameCol = new Col(this.Forename, this.Alias);
                this.Surname = config.ExtractColumnName(modelName, "Surname");
                this.SurnameCol = new Col(this.Surname, this.Alias);
                this.OrganizationCode = config.ExtractColumnName(modelName, "OrganizationCode");
                this.OrganizationCodeCol = new Col(this.OrganizationCode, this.Alias);
                this.PhonePrefix = config.ExtractColumnName(modelName, "PhonePrefix");
                this.PhonePrefixCol = new Col(this.PhonePrefix, this.Alias);
                this.PhoneNo = config.ExtractColumnName(modelName, "PhoneNo");
                this.PhoneNoCol = new Col(this.PhoneNo, this.Alias);
                this.FaxNo = config.ExtractColumnName(modelName, "FaxNo");
                this.FaxNoCol = new Col(this.FaxNo, this.Alias);
                this.Email = config.ExtractColumnName(modelName, "Email");
                this.EmailCol = new Col(this.Email, this.Alias);

            }

        }

        public class TblSpecialCharacter : Tab
        {
            public String CharacterType;
            public String PresCharacter;
            public String AggregPossible;
            public String DataCellPres;
            public String DataCellFilled;
            public String PresText;

            public Col CharacterTypeCol;
            public Col PresCharacterCol;
            public Col AggregPossibleCol;
            public Col DataCellPresCol;
            public Col DataCellFilledCol;
            public Col PresTextCol;

            internal TblSpecialCharacter(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.CharacterType = config.ExtractColumnName(modelName, "CharacterType");
                this.CharacterTypeCol = new Col(this.CharacterType, this.Alias);
                this.PresCharacter = config.ExtractColumnName(modelName, "PresCharacter");
                this.PresCharacterCol = new Col(this.PresCharacter, this.Alias);
                this.AggregPossible = config.ExtractColumnName(modelName, "AggregPossible");
                this.AggregPossibleCol = new Col(this.AggregPossible, this.Alias);
                if (String.Compare(config.MetaModel, "2.0", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.DataCellPres = config.ExtractColumnName(modelName, "DataCellPres");
                    this.DataCellPresCol = new Col(this.DataCellPres, this.Alias);
                }
                this.DataCellFilled = config.ExtractColumnName(modelName, "DataCellFilled");
                this.DataCellFilledCol = new Col(this.DataCellFilled, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);

            }

        }

        public class TblSpecialCharacterLang2 : Lang2Tab
        {
            public String CharacterType;
            public String PresCharacter;
            public String PresText;

            public Lang2Col CharacterTypeCol;
            public Lang2Col PresCharacterCol;
            public Lang2Col PresTextCol;

            internal TblSpecialCharacterLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.CharacterType = config.ExtractColumnName(modelName, "CharacterType");
                this.CharacterTypeCol = new Lang2Col(this.CharacterType, this.Alias, this.Suffixes);
                this.PresCharacter = config.ExtractColumnName(modelName, "PresCharacter");
                this.PresCharacterCol = new Lang2Col(this.PresCharacter, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
            }

        }


        public class TblSubTable : Tab
        {
            public String SubTable;
            public String MainTable;
            public String PresText;
            public String CleanTable;

            public Col SubTableCol;
            public Col MainTableCol;
            public Col PresTextCol;
            public Col CleanTableCol;

            internal TblSubTable(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.SubTable = config.ExtractColumnName(modelName, "SubTable");
                this.SubTableCol = new Col(this.SubTable, this.Alias);
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);
                this.CleanTable = config.ExtractColumnName(modelName, "CleanTable");
                this.CleanTableCol = new Col(this.CleanTable, this.Alias);

            }

        }

        public class TblSubTableLang2 : Lang2Tab
        {
            public String SubTable;
            public String MainTable;
            public String PresText;

            public Lang2Col SubTableCol;
            public Lang2Col MainTableCol;
            public Lang2Col PresTextCol;

            internal TblSubTableLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.SubTable = config.ExtractColumnName(modelName, "SubTable");
                this.SubTableCol = new Lang2Col(this.SubTable, this.Alias, this.Suffixes);
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Lang2Col(this.MainTable, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
            }

        }


        public class TblSubTableVariable : Tab
        {
            public String MainTable;
            public String SubTable;
            public String Variable;
            public String ValueSet;
            public String VariableType;
            public String StoreColumnNo;

            public Col MainTableCol;
            public Col SubTableCol;
            public Col VariableCol;
            public Col ValueSetCol;
            public Col VariableTypeCol;
            public Col StoreColumnNoCol;

            internal TblSubTableVariable(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.MainTable = config.ExtractColumnName(modelName, "MainTable");
                this.MainTableCol = new Col(this.MainTable, this.Alias);
                this.SubTable = config.ExtractColumnName(modelName, "SubTable");
                this.SubTableCol = new Col(this.SubTable, this.Alias);
                this.Variable = config.ExtractColumnName(modelName, "Variable");
                this.VariableCol = new Col(this.Variable, this.Alias);
                this.ValueSet = config.ExtractColumnName(modelName, "ValueSet");
                this.ValueSetCol = new Col(this.ValueSet, this.Alias);
                this.VariableType = config.ExtractColumnName(modelName, "VariableType");
                this.VariableTypeCol = new Col(this.VariableType, this.Alias);
                this.StoreColumnNo = config.ExtractColumnName(modelName, "StoreColumnNo");
                this.StoreColumnNoCol = new Col(this.StoreColumnNo, this.Alias);

            }

        }

        public class TblTextCatalog : Tab
        {
            public String TextCatalogNo;
            public String TextType;
            public String PresText;
            public String Description;

            public Col TextCatalogNoCol;
            public Col TextTypeCol;
            public Col PresTextCol;
            public Col DescriptionCol;

            internal TblTextCatalog(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.TextCatalogNo = config.ExtractColumnName(modelName, "TextCatalogNo");
                this.TextCatalogNoCol = new Col(this.TextCatalogNo, this.Alias);
                this.TextType = config.ExtractColumnName(modelName, "TextType");
                this.TextTypeCol = new Col(this.TextType, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Col(this.Description, this.Alias);

            }

        }

        public class TblTextCatalogLang2 : Lang2Tab
        {
            public String TextCatalogNo;
            public String TextType;
            public String PresText;
            public String Description;

            public Lang2Col TextCatalogNoCol;
            public Lang2Col TextTypeCol;
            public Lang2Col PresTextCol;
            public Lang2Col DescriptionCol;

            internal TblTextCatalogLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.TextCatalogNo = config.ExtractColumnName(modelName, "TextCatalogNo");
                this.TextCatalogNoCol = new Lang2Col(this.TextCatalogNo, this.Alias, this.Suffixes);
                this.TextType = config.ExtractColumnName(modelName, "TextType");
                this.TextTypeCol = new Lang2Col(this.TextType, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Lang2Col(this.Description, this.Alias, this.Suffixes);
            }

        }


        public class TblTimeScale : Tab
        {
            public String TimeScale;
            public String PresText;
            public String TimeScalePres;
            public String Regular;
            public String TimeUnit;
            public String Frequency;
            public String StoreFormat;

            public Col TimeScaleCol;
            public Col PresTextCol;
            public Col TimeScalePresCol;
            public Col RegularCol;
            public Col TimeUnitCol;
            public Col FrequencyCol;
            public Col StoreFormatCol;

            internal TblTimeScale(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.TimeScale = config.ExtractColumnName(modelName, "TimeScale");
                this.TimeScaleCol = new Col(this.TimeScale, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);
                this.TimeScalePres = config.ExtractColumnName(modelName, "TimeScalePres");
                this.TimeScalePresCol = new Col(this.TimeScalePres, this.Alias);
                this.Regular = config.ExtractColumnName(modelName, "Regular");
                this.RegularCol = new Col(this.Regular, this.Alias);
                this.TimeUnit = config.ExtractColumnName(modelName, "TimeUnit");
                this.TimeUnitCol = new Col(this.TimeUnit, this.Alias);
                this.Frequency = config.ExtractColumnName(modelName, "Frequency");
                this.FrequencyCol = new Col(this.Frequency, this.Alias);
                this.StoreFormat = config.ExtractColumnName(modelName, "StoreFormat");
                this.StoreFormatCol = new Col(this.StoreFormat, this.Alias);

            }

        }

        public class TblTimeScaleLang2 : Lang2Tab
        {
            public String TimeScale;
            public String PresText;

            public Lang2Col TimeScaleCol;
            public Lang2Col PresTextCol;

            internal TblTimeScaleLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.TimeScale = config.ExtractColumnName(modelName, "TimeScale");
                this.TimeScaleCol = new Lang2Col(this.TimeScale, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
            }

        }


        public class TblValue : Tab
        {
            public String ValuePool;
            public String ValueCode;
            public String SortCode;
            public String ValueTextS;
            public String ValueTextL;
            public String Footnote;

            public Col ValuePoolCol;
            public Col ValueCodeCol;
            public Col SortCodeCol;
            public Col ValueTextSCol;
            public Col ValueTextLCol;
            public Col FootnoteCol;

            internal TblValue(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Col(this.ValueCode, this.Alias);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Col(this.SortCode, this.Alias);
                this.ValueTextS = config.ExtractColumnName(modelName, "ValueTextS");
                this.ValueTextSCol = new Col(this.ValueTextS, this.Alias);
                this.ValueTextL = config.ExtractColumnName(modelName, "ValueTextL");
                this.ValueTextLCol = new Col(this.ValueTextL, this.Alias);
                this.Footnote = config.ExtractColumnName(modelName, "Footnote");
                this.FootnoteCol = new Col(this.Footnote, this.Alias);

            }

        }

        public class TblValueLang2 : Lang2Tab
        {
            public String ValuePool;
            public String ValueCode;
            public String SortCode;
            public String ValueTextS;
            public String ValueTextL;

            public Lang2Col ValuePoolCol;
            public Lang2Col ValueCodeCol;
            public Lang2Col SortCodeCol;
            public Lang2Col ValueTextSCol;
            public Lang2Col ValueTextLCol;

            internal TblValueLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Lang2Col(this.ValuePool, this.Alias, this.Suffixes);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Lang2Col(this.ValueCode, this.Alias, this.Suffixes);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Lang2Col(this.SortCode, this.Alias, this.Suffixes);
                this.ValueTextS = config.ExtractColumnName(modelName, "ValueTextS");
                this.ValueTextSCol = new Lang2Col(this.ValueTextS, this.Alias, this.Suffixes);
                this.ValueTextL = config.ExtractColumnName(modelName, "ValueTextL");
                this.ValueTextLCol = new Lang2Col(this.ValueTextL, this.Alias, this.Suffixes);
            }

        }


        public class TblValueExtra : Tab
        {
            public String ValuePool;
            public String ValueCode;
            public String Unit;
            public String ValueTextX1;
            public String ValueTextX2;
            public String ValueTextX3;
            public String ValueTextX4;

            public Col ValuePoolCol;
            public Col ValueCodeCol;
            public Col UnitCol;
            public Col ValueTextX1Col;
            public Col ValueTextX2Col;
            public Col ValueTextX3Col;
            public Col ValueTextX4Col;

            internal TblValueExtra(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Col(this.ValueCode, this.Alias);
                this.Unit = config.ExtractColumnName(modelName, "Unit");
                this.UnitCol = new Col(this.Unit, this.Alias);
                this.ValueTextX1 = config.ExtractColumnName(modelName, "ValueTextX1");
                this.ValueTextX1Col = new Col(this.ValueTextX1, this.Alias);
                this.ValueTextX2 = config.ExtractColumnName(modelName, "ValueTextX2");
                this.ValueTextX2Col = new Col(this.ValueTextX2, this.Alias);
                this.ValueTextX3 = config.ExtractColumnName(modelName, "ValueTextX3");
                this.ValueTextX3Col = new Col(this.ValueTextX3, this.Alias);
                this.ValueTextX4 = config.ExtractColumnName(modelName, "ValueTextX4");
                this.ValueTextX4Col = new Col(this.ValueTextX4, this.Alias);

            }

        }

        public class TblValueExtraLang2 : Lang2Tab
        {
            public String ValuePool;
            public String ValueCode;
            public String Unit;
            public String ValueTextX1;
            public String ValueTextX2;
            public String ValueTextX3;
            public String ValueTextX4;

            public Lang2Col ValuePoolCol;
            public Lang2Col ValueCodeCol;
            public Lang2Col UnitCol;
            public Lang2Col ValueTextX1Col;
            public Lang2Col ValueTextX2Col;
            public Lang2Col ValueTextX3Col;
            public Lang2Col ValueTextX4Col;

            internal TblValueExtraLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Lang2Col(this.ValuePool, this.Alias, this.Suffixes);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Lang2Col(this.ValueCode, this.Alias, this.Suffixes);
                this.Unit = config.ExtractColumnName(modelName, "Unit");
                this.UnitCol = new Lang2Col(this.Unit, this.Alias, this.Suffixes);
                this.ValueTextX1 = config.ExtractColumnName(modelName, "ValueTextX1");
                this.ValueTextX1Col = new Lang2Col(this.ValueTextX1, this.Alias, this.Suffixes);
                this.ValueTextX2 = config.ExtractColumnName(modelName, "ValueTextX2");
                this.ValueTextX2Col = new Lang2Col(this.ValueTextX2, this.Alias, this.Suffixes);
                this.ValueTextX3 = config.ExtractColumnName(modelName, "ValueTextX3");
                this.ValueTextX3Col = new Lang2Col(this.ValueTextX3, this.Alias, this.Suffixes);
                this.ValueTextX4 = config.ExtractColumnName(modelName, "ValueTextX4");
                this.ValueTextX4Col = new Lang2Col(this.ValueTextX4, this.Alias, this.Suffixes);
            }

        }


        public class TblValueGroup : Tab
        {
            public String Grouping;
            public String GroupCode;
            public String ValuePool;
            public String ValueCode;
            public String GroupLevel;
            public String ValueLevel;
            public String SortCode;

            public Col GroupingCol;
            public Col GroupCodeCol;
            public Col ValuePoolCol;
            public Col ValueCodeCol;
            public Col GroupLevelCol;
            public Col ValueLevelCol;
            public Col SortCodeCol;

            internal TblValueGroup(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Col(this.Grouping, this.Alias);
                this.GroupCode = config.ExtractColumnName(modelName, "GroupCode");
                this.GroupCodeCol = new Col(this.GroupCode, this.Alias);
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Col(this.ValueCode, this.Alias);
                this.GroupLevel = config.ExtractColumnName(modelName, "GroupLevel");
                this.GroupLevelCol = new Col(this.GroupLevel, this.Alias);
                this.ValueLevel = config.ExtractColumnName(modelName, "ValueLevel");
                this.ValueLevelCol = new Col(this.ValueLevel, this.Alias);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Col(this.SortCode, this.Alias);

            }

        }

        public class TblValuePool : Tab
        {
            public String ValuePool;
            public String PresText;
            public String Description;
            public String ValueTextExists;
            public String ValuePres;
            public String KDBId;

            public Col ValuePoolCol;
            public Col PresTextCol;
            public Col DescriptionCol;
            public Col ValueTextExistsCol;
            public Col ValuePresCol;
            public Col KDBIdCol;

            internal TblValuePool(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Col(this.Description, this.Alias);
                this.ValueTextExists = config.ExtractColumnName(modelName, "ValueTextExists");
                this.ValueTextExistsCol = new Col(this.ValueTextExists, this.Alias);
                this.ValuePres = config.ExtractColumnName(modelName, "ValuePres");
                this.ValuePresCol = new Col(this.ValuePres, this.Alias);
                this.KDBId = config.ExtractColumnName(modelName, "KDBId");
                this.KDBIdCol = new Col(this.KDBId, this.Alias);

            }

        }

        public class TblValuePoolLang2 : Lang2Tab
        {
            public String ValuePool;
            public String ValuePoolAlias;
            public String ValuePoolEng;
            public String PresText;

            public Lang2Col ValuePoolCol;
            public Lang2Col ValuePoolAliasCol;
            public Lang2Col ValuePoolEngCol;
            public Lang2Col PresTextCol;

            internal TblValuePoolLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Lang2Col(this.ValuePool, this.Alias, this.Suffixes);
                if (String.Compare(config.MetaModel, "2.1", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.ValuePoolAlias = config.ExtractColumnName(modelName, "ValuePoolAlias");
                    this.ValuePoolAliasCol = new Lang2Col(this.ValuePoolAlias, this.Alias, this.Suffixes);
                }
                if (String.Compare(config.MetaModel, "2.1", false, CultureInfo.InvariantCulture) <= 0)
                {
                    this.ValuePoolEng = config.ExtractColumnName(modelName, "ValuePoolEng");
                    this.ValuePoolEngCol = new Lang2Col(this.ValuePoolEng, this.Alias, this.Suffixes);
                }
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
            }

        }


        public class TblValueSet : Tab
        {
            public String ValueSet;
            public String PresText;
            public String Description;
            public String Elimination;
            public String ValuePool;
            public String ValuePres;
            public String GeoAreaNo;
            public String KDBId;
            public String SortCodeExists;
            public String Footnote;

            public Col ValueSetCol;
            public Col PresTextCol;
            public Col DescriptionCol;
            public Col EliminationCol;
            public Col ValuePoolCol;
            public Col ValuePresCol;
            public Col GeoAreaNoCol;
            public Col KDBIdCol;
            public Col SortCodeExistsCol;
            public Col FootnoteCol;

            internal TblValueSet(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValueSet = config.ExtractColumnName(modelName, "ValueSet");
                this.ValueSetCol = new Col(this.ValueSet, this.Alias);
                if (String.Compare(config.MetaModel, "2.0", false, CultureInfo.InvariantCulture) > 0)
                {
                    this.PresText = config.ExtractColumnName(modelName, "PresText");
                    this.PresTextCol = new Col(this.PresText, this.Alias);
                }
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Col(this.Description, this.Alias);
                this.Elimination = config.ExtractColumnName(modelName, "Elimination");
                this.EliminationCol = new Col(this.Elimination, this.Alias);
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.ValuePres = config.ExtractColumnName(modelName, "ValuePres");
                this.ValuePresCol = new Col(this.ValuePres, this.Alias);
                this.GeoAreaNo = config.ExtractColumnName(modelName, "GeoAreaNo");
                this.GeoAreaNoCol = new Col(this.GeoAreaNo, this.Alias);
                this.KDBId = config.ExtractColumnName(modelName, "KDBId");
                this.KDBIdCol = new Col(this.KDBId, this.Alias);
                this.SortCodeExists = config.ExtractColumnName(modelName, "SortCodeExists");
                this.SortCodeExistsCol = new Col(this.SortCodeExists, this.Alias);
                this.Footnote = config.ExtractColumnName(modelName, "Footnote");
                this.FootnoteCol = new Col(this.Footnote, this.Alias);

            }

        }

        public class TblValueSetLang2 : Lang2Tab
        {
            public String ValueSet;
            public String PresText;
            public String Description;

            public Lang2Col ValueSetCol;
            public Lang2Col PresTextCol;
            public Lang2Col DescriptionCol;

            internal TblValueSetLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.ValueSet = config.ExtractColumnName(modelName, "ValueSet");
                this.ValueSetCol = new Lang2Col(this.ValueSet, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
                this.Description = config.ExtractColumnName(modelName, "Description");
                this.DescriptionCol = new Lang2Col(this.Description, this.Alias, this.Suffixes);
            }

        }


        public class TblValueSetGrouping : Tab
        {
            public String ValueSet;
            public String Grouping;

            public Col ValueSetCol;
            public Col GroupingCol;

            internal TblValueSetGrouping(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValueSet = config.ExtractColumnName(modelName, "ValueSet");
                this.ValueSetCol = new Col(this.ValueSet, this.Alias);
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Col(this.Grouping, this.Alias);

            }

        }

        public class TblVariable : Tab
        {
            public String Variable;
            public String PresText;
            public String VariableInfo;
            public String Footnote;

            public Col VariableCol;
            public Col PresTextCol;
            public Col VariableInfoCol;
            public Col FootnoteCol;

            internal TblVariable(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.Variable = config.ExtractColumnName(modelName, "Variable");
                this.VariableCol = new Col(this.Variable, this.Alias);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Col(this.PresText, this.Alias);
                this.VariableInfo = config.ExtractColumnName(modelName, "VariableInfo");
                this.VariableInfoCol = new Col(this.VariableInfo, this.Alias);
                this.Footnote = config.ExtractColumnName(modelName, "Footnote");
                this.FootnoteCol = new Col(this.Footnote, this.Alias);

            }

        }

        public class TblVariableLang2 : Lang2Tab
        {
            public String Variable;
            public String PresText;

            public Lang2Col VariableCol;
            public Lang2Col PresTextCol;

            internal TblVariableLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.Variable = config.ExtractColumnName(modelName, "Variable");
                this.VariableCol = new Lang2Col(this.Variable, this.Alias, this.Suffixes);
                this.PresText = config.ExtractColumnName(modelName, "PresText");
                this.PresTextCol = new Lang2Col(this.PresText, this.Alias, this.Suffixes);
            }

        }


        public class TblVSGroup : Tab
        {
            public String ValueSet;
            public String Grouping;
            public String GroupCode;
            public String ValueCode;
            public String ValuePool;
            public String SortCode;

            public Col ValueSetCol;
            public Col GroupingCol;
            public Col GroupCodeCol;
            public Col ValueCodeCol;
            public Col ValuePoolCol;
            public Col SortCodeCol;

            internal TblVSGroup(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValueSet = config.ExtractColumnName(modelName, "ValueSet");
                this.ValueSetCol = new Col(this.ValueSet, this.Alias);
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Col(this.Grouping, this.Alias);
                this.GroupCode = config.ExtractColumnName(modelName, "GroupCode");
                this.GroupCodeCol = new Col(this.GroupCode, this.Alias);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Col(this.ValueCode, this.Alias);
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Col(this.SortCode, this.Alias);

            }

        }

        public class TblVSGroupLang2 : Lang2Tab
        {
            public String ValueSet;
            public String Grouping;
            public String GroupCode;
            public String ValueCode;
            public String ValuePool;
            public String SortCode;

            public Lang2Col ValueSetCol;
            public Lang2Col GroupingCol;
            public Lang2Col GroupCodeCol;
            public Lang2Col ValueCodeCol;
            public Lang2Col ValuePoolCol;
            public Lang2Col SortCodeCol;

            internal TblVSGroupLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.ValueSet = config.ExtractColumnName(modelName, "ValueSet");
                this.ValueSetCol = new Lang2Col(this.ValueSet, this.Alias, this.Suffixes);
                this.Grouping = config.ExtractColumnName(modelName, "Grouping");
                this.GroupingCol = new Lang2Col(this.Grouping, this.Alias, this.Suffixes);
                this.GroupCode = config.ExtractColumnName(modelName, "GroupCode");
                this.GroupCodeCol = new Lang2Col(this.GroupCode, this.Alias, this.Suffixes);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Lang2Col(this.ValueCode, this.Alias, this.Suffixes);
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Lang2Col(this.ValuePool, this.Alias, this.Suffixes);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Lang2Col(this.SortCode, this.Alias, this.Suffixes);
            }

        }


        public class TblVSValue : Tab
        {
            public String ValueSet;
            public String ValuePool;
            public String ValueCode;
            public String SortCode;

            public Col ValueSetCol;
            public Col ValuePoolCol;
            public Col ValueCodeCol;
            public Col SortCodeCol;

            internal TblVSValue(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner)
            {
                this.ValueSet = config.ExtractColumnName(modelName, "ValueSet");
                this.ValueSetCol = new Col(this.ValueSet, this.Alias);
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Col(this.ValuePool, this.Alias);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Col(this.ValueCode, this.Alias);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Col(this.SortCode, this.Alias);

            }

        }

        public class TblVSValueLang2 : Lang2Tab
        {
            public String ValueSet;
            public String ValuePool;
            public String ValueCode;
            public String SortCode;

            public Lang2Col ValueSetCol;
            public Lang2Col ValuePoolCol;
            public Lang2Col ValueCodeCol;
            public Lang2Col SortCodeCol;

            internal TblVSValueLang2(string modelName, SqlDbConfig_21 config)
            : base(config.ExtractAliasName(modelName), config.ExtractTableName(modelName), config.MetaOwner, config.MetaSuffixByLanguage)
            {
                this.ValueSet = config.ExtractColumnName(modelName, "ValueSet");
                this.ValueSetCol = new Lang2Col(this.ValueSet, this.Alias, this.Suffixes);
                this.ValuePool = config.ExtractColumnName(modelName, "ValuePool");
                this.ValuePoolCol = new Lang2Col(this.ValuePool, this.Alias, this.Suffixes);
                this.ValueCode = config.ExtractColumnName(modelName, "ValueCode");
                this.ValueCodeCol = new Lang2Col(this.ValueCode, this.Alias, this.Suffixes);
                this.SortCode = config.ExtractColumnName(modelName, "SortCode");
                this.SortCodeCol = new Lang2Col(this.SortCode, this.Alias, this.Suffixes);
            }

        }


        #endregion  structs


        private void initCodesAndKeywords()
        {

            #region Codes

            Codes = new Ccodes();

            Codes.CFPricesC = ExtractCode("CFPricesC");
            Codes.CFPricesF = ExtractCode("CFPricesF");
            Codes.StockFAS = ExtractCode("StockFAS");
            Codes.StockFAF = ExtractCode("StockFAF");
            Codes.StockFAA = ExtractCode("StockFAA");
            Codes.Copyright1 = ExtractCode("Copyright1");
            Codes.Copyright2 = ExtractCode("Copyright2");
            Codes.Copyright3 = ExtractCode("Copyright3");
            Codes.DataCellFilledOptional = ExtractCode("DataCellFilledOptional");
            Codes.DataCellFilledValue = ExtractCode("DataCellFilledValue");
            Codes.DataCellFilledZero = ExtractCode("DataCellFilledZero");
            Codes.VariableTypeC = ExtractCode("VariableTypeC");
            Codes.VariableTypeT = ExtractCode("VariableTypeT");
            Codes.VariableTypeG = ExtractCode("VariableTypeG");
            Codes.FootnoteM = ExtractCode("FootnoteM");
            Codes.FootnoteR = ExtractCode("FootnoteR");
            Codes.FootnoteB = ExtractCode("FootnoteB");
            Codes.FootnoteN = ExtractCode("FootnoteN");
            Codes.LinkPresDirect = ExtractCode("LinkPresDirect");
            Codes.LinkPresIndirect = ExtractCode("LinkPresIndirect");
            Codes.LinkPresBoth = ExtractCode("LinkPresBoth");
            Codes.PresCategoryInternal = ExtractCode("PresCategoryInternal");
            Codes.PresCategoryPrivate = ExtractCode("PresCategoryPrivate");
            Codes.PresCategoryPublic = ExtractCode("PresCategoryPublic");
            Codes.PresActive = ExtractCode("PresActive");
            Codes.PresPassive = ExtractCode("PresPassive");
            Codes.PresNo = ExtractCode("PresNo");
            Codes.TimeUnitA = ExtractCode("TimeUnitA");
            Codes.TimeUnitH = ExtractCode("TimeUnitH");
            Codes.TimeUnitQ = ExtractCode("TimeUnitQ");
            Codes.TimeUnitM = ExtractCode("TimeUnitM");
            Codes.TimeUnitW = ExtractCode("TimeUnitW");
            Codes.ValueTextExistsS = ExtractCode("ValueTextExistsS");
            Codes.ValueTextExistsL = ExtractCode("ValueTextExistsL");
            Codes.ValueTextExistsB = ExtractCode("ValueTextExistsB");
            Codes.ValueTextExistsN = ExtractCode("ValueTextExistsN");
            Codes.ValueTextExistsX = ExtractCode("ValueTextExistsX");
            Codes.ValuePresC = ExtractCode("ValuePresC");
            Codes.ValuePresT = ExtractCode("ValuePresT");
            Codes.ValuePresB = ExtractCode("ValuePresB");
            Codes.ValuePresA = ExtractCode("ValuePresA");
            Codes.ValuePresS = ExtractCode("ValuePresS");
            Codes.ValuePresV = ExtractCode("ValuePresV");
            Codes.EliminationA = ExtractCode("EliminationA");
            Codes.EliminationN = ExtractCode("EliminationN");
            Codes.GroupPresA = ExtractCode("GroupPresA");
            Codes.GroupPresI = ExtractCode("GroupPresI");
            Codes.GroupPresB = ExtractCode("GroupPresB");
            Codes.SpecialSignColumnY = ExtractCode("SpecialSignColumnY");
            Codes.SpecialSignColumnN = ExtractCode("SpecialSignColumnN");
            Codes.SpecialSignColumnE = ExtractCode("SpecialSignColumnE");
            Codes.Yes = ExtractCode("Yes");
            Codes.No = ExtractCode("No");
            Codes.StatusAll = ExtractCode("StatusAll");
            Codes.StatusEmpty = ExtractCode("StatusEmpty");
            Codes.StatusNew = ExtractCode("StatusNew");
            Codes.StatusMeta = ExtractCode("StatusMeta");
            Codes.StatusOrder = ExtractCode("StatusOrder");
            Codes.StatusUpdate = ExtractCode("StatusUpdate");
            Codes.StatusEng = ExtractCode("StatusEng");
            Codes.StatusTranslated = ExtractCode("StatusTranslated");
            Codes.RoleHead = ExtractCode("RoleHead");
            Codes.RoleContact = ExtractCode("RoleContact");
            Codes.RoleUpdate = ExtractCode("RoleUpdate");
            Codes.RoleVerify = ExtractCode("RoleVerify");
            Codes.FootnoteShowS = ExtractCode("FootnoteShowS");
            Codes.FootnoteShowP = ExtractCode("FootnoteShowP");
            Codes.FootnoteShowB = ExtractCode("FootnoteShowB");

            #endregion Codes


            #region Keywords

            Keywords = new DbKeywords();

            Keywords.ContentVariable = ExtractKeyword("ContentVariable");
            Keywords.MenuLevels = ExtractKeyword("MenuLevels");
            Keywords.Macro = ExtractKeyword("Macro");
            if (FileHasKeyword("PXCodepage"))
            {
                Keywords.Optional_PXCodepage = ExtractKeyword("PXCodepage");
            }
            if (FileHasKeyword("PXDescriptionDefault"))
            {
                Keywords.Optional_PXDescriptionDefault = ExtractKeyword("PXDescriptionDefault");
            }
            if (FileHasKeyword("AllwaysUseMaintablePrestextSInDynamicTitle"))
            {
                Keywords.Optional_AllwaysUseMaintablePrestextSInDynamicTitle = ExtractKeyword("AllwaysUseMaintablePrestextSInDynamicTitle");
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
            public String Optional_PXCodepage;
            public String Optional_PXDescriptionDefault;
            public String Optional_AllwaysUseMaintablePrestextSInDynamicTitle;
            public String Optional_PXCharset;
            public String Optional_PXAxisVersion;
        }
    }
}
