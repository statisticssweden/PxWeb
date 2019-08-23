using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml.XPath;
using System.Globalization;

using PCAxis.Sql.DbConfig;

//This code is generated. Not anymore!!

namespace PCAxis.Sql.QueryLib_21
{

    #region class Contents

    /// <summary>
    /// Holds the attributes for Contents. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class ContentsRow
    {
        private String mMainTable;
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mContents;
        public String Contents
        {
            get { return mContents; }
        }
        private String mPresCode;
        public String PresCode
        {
            get { return mPresCode; }
        }
        private String mCopyright;
        public String Copyright
        {
            get { return mCopyright; }
        }
        private String mStatAuthority;
        public String StatAuthority
        {
            get { return mStatAuthority; }
        }
        private String mProducer;
        public String Producer
        {
            get { return mProducer; }
        }
        private String mLastUpdated;
        public String LastUpdated
        {
            get { return mLastUpdated; }
        }
        private String mPublished;
        public String Published
        {
            get { return mPublished; }
        }
        private String mPresDecimals;
        public String PresDecimals
        {
            get { return mPresDecimals; }
        }
        private String mPresCellsZero;
        public String PresCellsZero
        {
            get { return mPresCellsZero; }
        }
        private String mPresMissingLine;
        public String PresMissingLine
        {
            get { return mPresMissingLine; }
        }
        private String mAggregPossible;
        public String AggregPossible
        {
            get { return mAggregPossible; }
        }
        private String mStockFA;
        public String StockFA
        {
            get { return mStockFA; }
        }
        private String mCFPrices;
        public String CFPrices
        {
            get { return mCFPrices; }
        }
        private String mDayAdj;
        public String DayAdj
        {
            get { return mDayAdj; }
        }
        private String mSeasAdj;
        public String SeasAdj
        {
            get { return mSeasAdj; }
        }
        private String mFootnoteContents;
        public String FootnoteContents
        {
            get { return mFootnoteContents; }
        }
        private String mFootnoteTime;
        public String FootnoteTime
        {
            get { return mFootnoteTime; }
        }
        private String mFootnoteValue;
        public String FootnoteValue
        {
            get { return mFootnoteValue; }
        }
        private String mFootnoteVariable;
        public String FootnoteVariable
        {
            get { return mFootnoteVariable; }
        }
        private String mStoreNoChar;
        public String StoreNoChar
        {
            get { return mStoreNoChar; }
        }
        private String mStoreDecimals;
        public String StoreDecimals
        {
            get { return mStoreDecimals; }
        }
        private String mStoreFormat;
        public String StoreFormat
        {
            get { return mStoreFormat; }
        }
        private String mStoreColumnNo;
        public String StoreColumnNo
        {
            get { return mStoreColumnNo; }
        }

        public Dictionary<string, ContentsTexts> texts = new Dictionary<string, ContentsTexts>();

        public ContentsRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
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
            if (metaModel.metaVersionGE("2.1"))
            {
                this.mPresMissingLine = myRow[dbconf.Contents.PresMissingLineCol.Label()].ToString();
            }
            this.mAggregPossible = myRow[dbconf.Contents.AggregPossibleCol.Label()].ToString();
            this.mStockFA = myRow[dbconf.Contents.StockFACol.Label()].ToString();
            this.mCFPrices = myRow[dbconf.Contents.CFPricesCol.Label()].ToString();
            this.mDayAdj = myRow[dbconf.Contents.DayAdjCol.Label()].ToString();
            this.mSeasAdj = myRow[dbconf.Contents.SeasAdjCol.Label()].ToString();
            this.mFootnoteContents = myRow[dbconf.Contents.FootnoteContentsCol.Label()].ToString();
            this.mFootnoteTime = myRow[dbconf.Contents.FootnoteTimeCol.Label()].ToString();
            this.mFootnoteValue = myRow[dbconf.Contents.FootnoteValueCol.Label()].ToString();
            this.mFootnoteVariable = myRow[dbconf.Contents.FootnoteVariableCol.Label()].ToString();
            this.mStoreNoChar = myRow[dbconf.Contents.StoreNoCharCol.Label()].ToString();
            this.mStoreDecimals = myRow[dbconf.Contents.StoreDecimalsCol.Label()].ToString();
            this.mStoreFormat = myRow[dbconf.Contents.StoreFormatCol.Label()].ToString();
            this.mStoreColumnNo = myRow[dbconf.Contents.StoreColumnNoCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new ContentsTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for Contents  for one language.
    /// </summary>
    public class ContentsTexts
    {
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }
        private String mPresTextS;
        public String PresTextS
        {
            get { return mPresTextS; }
        }
        private String mUnit;
        public String Unit
        {
            get { return mUnit; }
        }
        private String mRefPeriod;
        public String RefPeriod
        {
            get { return mRefPeriod; }
        }
        private String mBasePeriod;
        public String BasePeriod
        {
            get { return mBasePeriod; }
        }


        internal ContentsTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
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


    #endregion class Contents

    #region class DataStorage

    /// <summary>
    /// Holds the attributes for DataStorage. (This entity is language independent.) 
    /// </summary>
    public class DataStorageRow
    {
        private String mProductId;
        public String ProductId
        {
            get { return mProductId; }
        }
        private String mServerName;
        public String ServerName
        {
            get { return mServerName; }
        }
        private String mDatabaseName;
        public String DatabaseName
        {
            get { return mDatabaseName; }
        }

        public DataStorageRow(DataRow myRow, SqlDbConfig_21 dbconf, IMetaVersionComparator metaModel)
        {
            this.mProductId = myRow[dbconf.DataStorage.ProductIdCol.Label()].ToString();
            this.mServerName = myRow[dbconf.DataStorage.ServerNameCol.Label()].ToString();
            this.mDatabaseName = myRow[dbconf.DataStorage.DatabaseNameCol.Label()].ToString();
        }

    }

    #endregion class DataStorage

    #region class Grouping

    /// <summary>
    /// Holds the attributes for Grouping. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class GroupingRow
    {
        private String mValuePool;
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mGrouping;
        public String Grouping
        {
            get { return mGrouping; }
        }
        private String mDescription;
        public String Description
        {
            get { return mDescription; }
        }
        private String mGroupPres;
        public String GroupPres
        {
            get { return mGroupPres; }
        }
        private String mGeoAreaNo;
        public String GeoAreaNo
        {
            get { return mGeoAreaNo; }
        }
        private String mKDBid;
        public String KDBid
        {
            get { return mKDBid; }
        }

        public Dictionary<string, GroupingTexts> texts = new Dictionary<string, GroupingTexts>();

        public GroupingRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mValuePool = myRow[dbconf.Grouping.ValuePoolCol.Label()].ToString();
            this.mGrouping = myRow[dbconf.Grouping.GroupingCol.Label()].ToString();
            this.mDescription = myRow[dbconf.Grouping.DescriptionCol.Label()].ToString();
            this.mGroupPres = myRow[dbconf.Grouping.GroupPresCol.Label()].ToString();
            this.mGeoAreaNo = myRow[dbconf.Grouping.GeoAreaNoCol.Label()].ToString();
            this.mKDBid = myRow[dbconf.Grouping.KDBidCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new GroupingTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for Grouping  for one language.
    /// </summary>
    public class GroupingTexts
    {
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }
        private String mSortCode;
        public String SortCode
        {
            get { return mSortCode; }
        }


        internal GroupingTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
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


    #endregion class Grouping

    #region class MainTable

    /// <summary>
    /// Holds the attributes for MainTable. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class MainTableRow
    {
        private String mMainTable;
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mTableStatus;
        public String TableStatus
        {
            get { return mTableStatus; }
        }
        private String mStatusEng;
        public String StatusEng
        {
            get { return mStatusEng; }
        }
        private String mTableId;
        public String TableId
        {
            get { return mTableId; }
        }
        private String mPresCategory;
        public String PresCategory
        {
            get { return mPresCategory; }
        }
        private String mSpecCharExists;
        public String SpecCharExists
        {
            get { return mSpecCharExists; }
        }
        private String mSubjectCode;
        public String SubjectCode
        {
            get { return mSubjectCode; }
        }
        private String mProductId;
        public String ProductId
        {
            get { return mProductId; }
        }
        private String mTimeScale;
        public String TimeScale
        {
            get { return mTimeScale; }
        }

        public Dictionary<string, MainTableTexts> texts = new Dictionary<string, MainTableTexts>();

        public MainTableRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mMainTable = myRow[dbconf.MainTable.MainTableCol.Label()].ToString();
            this.mTableStatus = myRow[dbconf.MainTable.TableStatusCol.Label()].ToString();
            if (metaModel.metaVersionLE("2.0"))
            {
                this.mStatusEng = myRow[dbconf.MainTable.StatusEngCol.Label()].ToString();
            }
            this.mTableId = myRow[dbconf.MainTable.TableIdCol.Label()].ToString();
            this.mPresCategory = myRow[dbconf.MainTable.PresCategoryCol.Label()].ToString();
            this.mSpecCharExists = myRow[dbconf.MainTable.SpecCharExistsCol.Label()].ToString();
            this.mSubjectCode = myRow[dbconf.MainTable.SubjectCodeCol.Label()].ToString();
            this.mProductId = myRow[dbconf.MainTable.ProductIdCol.Label()].ToString();
            this.mTimeScale = myRow[dbconf.MainTable.TimeScaleCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new MainTableTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for MainTable  for one language.
    /// </summary>
    public class MainTableTexts
    {
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }
        private String mPresTextS;
        public String PresTextS
        {
            get { return mPresTextS; }
        }
        private String mContentsVariable;
        public String ContentsVariable
        {
            get { return mContentsVariable; }
        }


        internal MainTableTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
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


    #endregion class MainTable

    #region class MainTablePerson

    /// <summary>
    /// Holds the attributes for MainTablePerson. (This entity is language independent.) 
    /// </summary>
    public class MainTablePersonRow
    {
        private String mMainTable;
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mPersonCode;
        public String PersonCode
        {
            get { return mPersonCode; }
        }
        private String mRolePerson;
        public String RolePerson
        {
            get { return mRolePerson; }
        }

        public MainTablePersonRow(DataRow myRow, SqlDbConfig_21 dbconf, IMetaVersionComparator metaModel)
        {
            this.mMainTable = myRow[dbconf.MainTablePerson.MainTableCol.Label()].ToString();
            this.mPersonCode = myRow[dbconf.MainTablePerson.PersonCodeCol.Label()].ToString();
            this.mRolePerson = myRow[dbconf.MainTablePerson.RolePersonCol.Label()].ToString();
        }

    }

    #endregion class MainTablePerson

    #region class MenuSelection
    /* For SubjectArea*/

    /// <summary>
    /// Holds the attributes for MenuSelection. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class MenuSelectionRow
    {
        private String mMenu;
        public String Menu
        {
            get { return mMenu; }
        }
        private String mSelection;
        public String Selection
        {
            get { return mSelection; }
        }
        private String mLevelNo;
        public String LevelNo
        {
            get { return mLevelNo; }
        }
        private String mInternalId;
        public String InternalId
        {
            get { return mInternalId; }
        }

        public Dictionary<string, MenuSelectionTexts> texts = new Dictionary<string, MenuSelectionTexts>();

        public MenuSelectionRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mMenu = myRow[dbconf.MenuSelection.MenuCol.Label()].ToString();
            this.mSelection = myRow[dbconf.MenuSelection.SelectionCol.Label()].ToString();
            this.mLevelNo = myRow[dbconf.MenuSelection.LevelNoCol.Label()].ToString();
            this.mInternalId = myRow[dbconf.MenuSelection.InternalIdCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new MenuSelectionTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for MenuSelection  for one language.
    /// </summary>
    public class MenuSelectionTexts
    {
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }
        private String mPresTextS;
        public String PresTextS
        {
            get { return mPresTextS; }
        }
        private String mDescription;
        public String Description
        {
            get { return mDescription; }
        }
        private String mSortCode;
        public String SortCode
        {
            get { return mSortCode; }
        }
        private String mPresentation;
        public String Presentation
        {
            get { return mPresentation; }
        }


        internal MenuSelectionTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
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


    #endregion class MenuSelection

    #region class MetaAdm

    /// <summary>
    /// Holds the attributes for MetaAdm. (This entity is language independent.) 
    /// </summary>
    public class MetaAdmRow
    {
        private String mProperty;
        public String Property
        {
            get { return mProperty; }
        }
        private String mValue;
        public String Value
        {
            get { return mValue; }
        }

        public MetaAdmRow(DataRow myRow, SqlDbConfig_21 dbconf, IMetaVersionComparator metaModel)
        {
            this.mProperty = myRow[dbconf.MetaAdm.PropertyCol.Label()].ToString();
            this.mValue = myRow[dbconf.MetaAdm.ValueCol.Label()].ToString();
        }

    }

    #endregion class MetaAdm

    #region class MetabaseInfo

    /// <summary>
    /// Holds the attributes for MetabaseInfo. (This entity is language independent.) 
    /// </summary>
    public class MetabaseInfoRow
    {
        private String mModel;
        public String Model
        {
            get { return mModel; }
        }
        private String mModelVersion;
        public String ModelVersion
        {
            get { return mModelVersion; }
        }
        private String mDatabaseRole;
        public String DatabaseRole
        {
            get { return mDatabaseRole; }
        }

        public MetabaseInfoRow(DataRow myRow, SqlDbConfig_21 dbconf, IMetaVersionComparator metaModel)
        {
            this.mModel = myRow[dbconf.MetabaseInfo.ModelCol.Label()].ToString();
            this.mModelVersion = myRow[dbconf.MetabaseInfo.ModelVersionCol.Label()].ToString();
            this.mDatabaseRole = myRow[dbconf.MetabaseInfo.DatabaseRoleCol.Label()].ToString();
        }

    }

    #endregion class MetabaseInfo

    #region class Organization

    /// <summary>
    /// Holds the attributes for Organization. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class OrganizationRow
    {
        private String mOrganizationCode;
        public String OrganizationCode
        {
            get { return mOrganizationCode; }
        }
        private String mWebAddress;
        public String WebAddress
        {
            get { return mWebAddress; }
        }
        private String mInternalId;
        public String InternalId
        {
            get { return mInternalId; }
        }

        public Dictionary<string, OrganizationTexts> texts = new Dictionary<string, OrganizationTexts>();

        public OrganizationRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mOrganizationCode = myRow[dbconf.Organization.OrganizationCodeCol.Label()].ToString();
            if (metaModel.metaVersionGE("2.1"))
            {
                this.mWebAddress = myRow[dbconf.Organization.WebAddressCol.Label()].ToString();
            }
            this.mInternalId = myRow[dbconf.Organization.InternalIdCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new OrganizationTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for Organization  for one language.
    /// </summary>
    public class OrganizationTexts
    {
        private String mOrganizationName;
        public String OrganizationName
        {
            get { return mOrganizationName; }
        }
        private String mDepartment;
        public String Department
        {
            get { return mDepartment; }
        }
        private String mUnit;
        public String Unit
        {
            get { return mUnit; }
        }


        internal OrganizationTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mOrganizationName = myRow[dbconf.OrganizationLang2.OrganizationNameCol.Label(languageCode)].ToString();
                this.mDepartment = myRow[dbconf.OrganizationLang2.DepartmentCol.Label(languageCode)].ToString();
                this.mUnit = myRow[dbconf.OrganizationLang2.UnitCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mOrganizationName = myRow[dbconf.Organization.OrganizationNameCol.Label()].ToString();
                this.mDepartment = myRow[dbconf.Organization.DepartmentCol.Label()].ToString();
                this.mUnit = myRow[dbconf.Organization.UnitCol.Label()].ToString();
            }


        }
    }


    #endregion class Organization

    #region class Person

    /// <summary>
    /// Holds the attributes for Person. (This entity is language independent.) 
    /// </summary>
    public class PersonRow
    {
        private String mPersonCode;
        public String PersonCode
        {
            get { return mPersonCode; }
        }
        private String mForename;
        public String Forename
        {
            get { return mForename; }
        }
        private String mSurname;
        public String Surname
        {
            get { return mSurname; }
        }
        private String mOrganizationCode;
        public String OrganizationCode
        {
            get { return mOrganizationCode; }
        }
        private String mPhonePrefix;
        public String PhonePrefix
        {
            get { return mPhonePrefix; }
        }
        private String mPhoneNo;
        public String PhoneNo
        {
            get { return mPhoneNo; }
        }
        private String mFaxNo;
        public String FaxNo
        {
            get { return mFaxNo; }
        }
        private String mEmail;
        public String Email
        {
            get { return mEmail; }
        }

        public PersonRow(DataRow myRow, SqlDbConfig_21 dbconf, IMetaVersionComparator metaModel)
        {
            this.mPersonCode = myRow[dbconf.Person.PersonCodeCol.Label()].ToString();
            this.mForename = myRow[dbconf.Person.ForenameCol.Label()].ToString();
            this.mSurname = myRow[dbconf.Person.SurnameCol.Label()].ToString();
            this.mOrganizationCode = myRow[dbconf.Person.OrganizationCodeCol.Label()].ToString();
            this.mPhonePrefix = myRow[dbconf.Person.PhonePrefixCol.Label()].ToString();
            this.mPhoneNo = myRow[dbconf.Person.PhoneNoCol.Label()].ToString();
            this.mFaxNo = myRow[dbconf.Person.FaxNoCol.Label()].ToString();
            this.mEmail = myRow[dbconf.Person.EmailCol.Label()].ToString();
        }

    }

    #endregion class Person

    #region class SpecialCharacter

    /// <summary>
    /// Holds the attributes for SpecialCharacter. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class SpecialCharacterRow
    {
        private String mCharacterType;
        public String CharacterType
        {
            get { return mCharacterType; }
        }
        private String mAggregPossible;
        public String AggregPossible
        {
            get { return mAggregPossible; }
        }
        private String mDataCellPres;
        public String DataCellPres
        {
            get { return mDataCellPres; }
        }
        private String mDataCellFilled;
        public String DataCellFilled
        {
            get { return mDataCellFilled; }
        }

        public Dictionary<string, SpecialCharacterTexts> texts = new Dictionary<string, SpecialCharacterTexts>();

        public SpecialCharacterRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mCharacterType = myRow[dbconf.SpecialCharacter.CharacterTypeCol.Label()].ToString();
            this.mAggregPossible = myRow[dbconf.SpecialCharacter.AggregPossibleCol.Label()].ToString();
            if (metaModel.metaVersionGE("2.1"))
            {
                this.mDataCellPres = myRow[dbconf.SpecialCharacter.DataCellPresCol.Label()].ToString();
            }
            if (metaModel.metaVersionGE("2.1"))
            {
                this.mDataCellFilled = myRow[dbconf.SpecialCharacter.DataCellFilledCol.Label()].ToString();
            }

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new SpecialCharacterTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for SpecialCharacter  for one language.
    /// </summary>
    public class SpecialCharacterTexts
    {
        private String mPresCharacter;
        public String PresCharacter
        {
            get { return mPresCharacter; }
        }
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }


        internal SpecialCharacterTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresCharacter = myRow[dbconf.SpecialCharacterLang2.PresCharacterCol.Label(languageCode)].ToString();
                this.mPresText = myRow[dbconf.SpecialCharacterLang2.PresTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresCharacter = myRow[dbconf.SpecialCharacter.PresCharacterCol.Label()].ToString();
                this.mPresText = myRow[dbconf.SpecialCharacter.PresTextCol.Label()].ToString();
            }


        }
    }


    #endregion class SpecialCharacter

    #region class SubTable

    /// <summary>
    /// Holds the attributes for SubTable. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class SubTableRow
    {
        private String mSubTable;
        public String SubTable
        {
            get { return mSubTable; }
        }
        private String mMainTable;
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mCleanTable;
        public String CleanTable
        {
            get { return mCleanTable; }
        }

        public Dictionary<string, SubTableTexts> texts = new Dictionary<string, SubTableTexts>();

        public SubTableRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mSubTable = myRow[dbconf.SubTable.SubTableCol.Label()].ToString();
            this.mMainTable = myRow[dbconf.SubTable.MainTableCol.Label()].ToString();
            this.mCleanTable = myRow[dbconf.SubTable.CleanTableCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new SubTableTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for SubTable  for one language.
    /// </summary>
    public class SubTableTexts
    {
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }


        internal SubTableTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.SubTableLang2.PresTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.SubTable.PresTextCol.Label()].ToString();
            }


        }
    }


    #endregion class SubTable

    #region class SubTableVariable

    /// <summary>
    /// Holds the attributes for SubTableVariable. (This entity is language independent.) 
    /// </summary>
    public class SubTableVariableRow
    {
        private String mMainTable;
        public String MainTable
        {
            get { return mMainTable; }
        }
        private String mSubTable;
        public String SubTable
        {
            get { return mSubTable; }
        }
        private String mVariable;
        public String Variable
        {
            get { return mVariable; }
        }
        private String mValueSet;
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mVariableType;
        public String VariableType
        {
            get { return mVariableType; }
        }
        private String mStoreColumnNo;
        public String StoreColumnNo
        {
            get { return mStoreColumnNo; }
        }

        public SubTableVariableRow(DataRow myRow, SqlDbConfig_21 dbconf, IMetaVersionComparator metaModel)
        {
            this.mMainTable = myRow[dbconf.SubTableVariable.MainTableCol.Label()].ToString();
            this.mSubTable = myRow[dbconf.SubTableVariable.SubTableCol.Label()].ToString();
            this.mVariable = myRow[dbconf.SubTableVariable.VariableCol.Label()].ToString();
            this.mValueSet = myRow[dbconf.SubTableVariable.ValueSetCol.Label()].ToString();
            this.mVariableType = myRow[dbconf.SubTableVariable.VariableTypeCol.Label()].ToString();
            this.mStoreColumnNo = myRow[dbconf.SubTableVariable.StoreColumnNoCol.Label()].ToString();
        }

    }

    #endregion class SubTableVariable

    #region class TextCatalog
    /*This table is not suitable for generated extractions such as Get...*/

    /// <summary>
    /// Holds the attributes for TextCatalog. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class TextCatalogRow
    {
        private String mTextCatalogNo;
        public String TextCatalogNo
        {
            get { return mTextCatalogNo; }
        }

        public Dictionary<string, TextCatalogTexts> texts = new Dictionary<string, TextCatalogTexts>();

        public TextCatalogRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mTextCatalogNo = myRow[dbconf.TextCatalog.TextCatalogNoCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new TextCatalogTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for TextCatalog  for one language.
    /// </summary>
    public class TextCatalogTexts
    {
        private String mTextType;
        public String TextType
        {
            get { return mTextType; }
        }
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }
        private String mDescription;
        public String Description
        {
            get { return mDescription; }
        }


        internal TextCatalogTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mTextType = myRow[dbconf.TextCatalogLang2.TextTypeCol.Label(languageCode)].ToString();
                this.mPresText = myRow[dbconf.TextCatalogLang2.PresTextCol.Label(languageCode)].ToString();
                this.mDescription = myRow[dbconf.TextCatalogLang2.DescriptionCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mTextType = myRow[dbconf.TextCatalog.TextTypeCol.Label()].ToString();
                this.mPresText = myRow[dbconf.TextCatalog.PresTextCol.Label()].ToString();
                this.mDescription = myRow[dbconf.TextCatalog.DescriptionCol.Label()].ToString();
            }


        }
    }


    #endregion class TextCatalog

    #region class TimeScale

    /// <summary>
    /// Holds the attributes for TimeScale. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class TimeScaleRow
    {
        private String mTimeScale;
        public String TimeScale
        {
            get { return mTimeScale; }
        }
        private String mTimeScalePres;
        public String TimeScalePres
        {
            get { return mTimeScalePres; }
        }
        private String mRegular;
        public String Regular
        {
            get { return mRegular; }
        }
        private String mTimeUnit;
        public String TimeUnit
        {
            get { return mTimeUnit; }
        }
        private String mFrequency;
        public String Frequency
        {
            get { return mFrequency; }
        }
        private String mStoreFormat;
        public String StoreFormat
        {
            get { return mStoreFormat; }
        }

        public Dictionary<string, TimeScaleTexts> texts = new Dictionary<string, TimeScaleTexts>();

        public TimeScaleRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mTimeScale = myRow[dbconf.TimeScale.TimeScaleCol.Label()].ToString();
            this.mTimeScalePres = myRow[dbconf.TimeScale.TimeScalePresCol.Label()].ToString();
            this.mRegular = myRow[dbconf.TimeScale.RegularCol.Label()].ToString();
            this.mTimeUnit = myRow[dbconf.TimeScale.TimeUnitCol.Label()].ToString();
            this.mFrequency = myRow[dbconf.TimeScale.FrequencyCol.Label()].ToString();
            this.mStoreFormat = myRow[dbconf.TimeScale.StoreFormatCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new TimeScaleTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for TimeScale  for one language.
    /// </summary>
    public class TimeScaleTexts
    {
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }


        internal TimeScaleTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.TimeScaleLang2.PresTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.TimeScale.PresTextCol.Label()].ToString();
            }


        }
    }


    #endregion class TimeScale

    #region class ValuePool

    /// <summary>
    /// Holds the attributes for ValuePool. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class ValuePoolRow
    {
        private String mValuePool;
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mDescription;
        public String Description
        {
            get { return mDescription; }
        }
        private String mValueTextExists;
        public String ValueTextExists
        {
            get { return mValueTextExists; }
        }
        private String mValuePres;
        public String ValuePres
        {
            get { return mValuePres; }
        }
        private String mKDBId;
        public String KDBId
        {
            get { return mKDBId; }
        }


        public Dictionary<string, ValuePoolTexts> texts = new Dictionary<string, ValuePoolTexts>();

        public ValuePoolRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mValuePool = myRow[dbconf.ValuePool.ValuePoolCol.Label()].ToString();
            this.mDescription = myRow[dbconf.ValuePool.DescriptionCol.Label()].ToString();
            this.mValueTextExists = myRow[dbconf.ValuePool.ValueTextExistsCol.Label()].ToString();
            this.mValuePres = myRow[dbconf.ValuePool.ValuePresCol.Label()].ToString();
            this.mKDBId = myRow[dbconf.ValuePool.KDBIdCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new ValuePoolTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for ValuePool  for one language.
    /// </summary>
    public class ValuePoolTexts
    {
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }

        private String mValuePoolLangDependent;
        public String ValuePoolLangDependent
        {
            get { return mValuePoolLangDependent; }
        }


        internal ValuePoolTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.ValuePoolLang2.PresTextCol.Label(languageCode)].ToString();
                if (metaModel.metaVersionGE("2.2"))
                {
                    this.mValuePoolLangDependent = myRow[dbconf.ValuePoolLang2.ValuePoolAliasCol.Label(languageCode)].ToString();
                }
                else
                {
                    this.mValuePoolLangDependent = myRow[dbconf.ValuePoolLang2.ValuePoolEngCol.Label(languageCode)].ToString();
                }
            }
            else
            {
                this.mPresText = myRow[dbconf.ValuePool.PresTextCol.Label()].ToString();
                this.mValuePoolLangDependent = myRow[dbconf.ValuePool.ValuePoolCol.Label()].ToString();
            }


        }
    }


    #endregion class ValuePool

    #region class ValueSet

    /// <summary>
    /// Holds the attributes for ValueSet. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class ValueSetRow
    {
        private String mValueSet;
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mElimination;
        public String Elimination
        {
            get { return mElimination; }
        }
        private String mValuePool;
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mValuePres;
        public String ValuePres
        {
            get { return mValuePres; }
        }
        private String mGeoAreaNo;
        public String GeoAreaNo
        {
            get { return mGeoAreaNo; }
        }
        private String mKDBId;
        public String KDBId
        {
            get { return mKDBId; }
        }
        private String mSortCodeExists;
        public String SortCodeExists
        {
            get { return mSortCodeExists; }
        }
        private String mFootnote;
        public String Footnote
        {
            get { return mFootnote; }
        }

        public Dictionary<string, ValueSetTexts> texts = new Dictionary<string, ValueSetTexts>();

        public ValueSetRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mValueSet = myRow[dbconf.ValueSet.ValueSetCol.Label()].ToString();
            this.mElimination = myRow[dbconf.ValueSet.EliminationCol.Label()].ToString();
            this.mValuePool = myRow[dbconf.ValueSet.ValuePoolCol.Label()].ToString();
            this.mValuePres = myRow[dbconf.ValueSet.ValuePresCol.Label()].ToString();
            this.mGeoAreaNo = myRow[dbconf.ValueSet.GeoAreaNoCol.Label()].ToString();
            this.mKDBId = myRow[dbconf.ValueSet.KDBIdCol.Label()].ToString();
            this.mSortCodeExists = myRow[dbconf.ValueSet.SortCodeExistsCol.Label()].ToString();
            this.mFootnote = myRow[dbconf.ValueSet.FootnoteCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new ValueSetTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for ValueSet  for one language.
    /// </summary>
    public class ValueSetTexts
    {
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }
        private String mDescription;
        public String Description
        {
            get { return mDescription; }
        }


        internal ValueSetTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                if (metaModel.metaVersionGE("2.1"))
                {
                    this.mPresText = myRow[dbconf.ValueSetLang2.PresTextCol.Label(languageCode)].ToString();
                }
                this.mDescription = myRow[dbconf.ValueSetLang2.DescriptionCol.Label(languageCode)].ToString();
            }
            else
            {
                if (metaModel.metaVersionGE("2.1"))
                {
                    this.mPresText = myRow[dbconf.ValueSet.PresTextCol.Label()].ToString();
                }
                this.mDescription = myRow[dbconf.ValueSet.DescriptionCol.Label()].ToString();
            }


        }
    }


    #endregion class ValueSet

    #region class Variable

    /// <summary>
    /// Holds the attributes for Variable. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class VariableRow
    {
        private String mVariable;
        public String Variable
        {
            get { return mVariable; }
        }
        private String mVariableInfo;
        public String VariableInfo
        {
            get { return mVariableInfo; }
        }
        private String mFootnote;
        public String Footnote
        {
            get { return mFootnote; }
        }

        public Dictionary<string, VariableTexts> texts = new Dictionary<string, VariableTexts>();

        public VariableRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mVariable = myRow[dbconf.Variable.VariableCol.Label()].ToString();
            this.mVariableInfo = myRow[dbconf.Variable.VariableInfoCol.Label()].ToString();
            this.mFootnote = myRow[dbconf.Variable.FootnoteCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new VariableTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for Variable  for one language.
    /// </summary>
    public class VariableTexts
    {
        private String mPresText;
        public String PresText
        {
            get { return mPresText; }
        }


        internal VariableTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mPresText = myRow[dbconf.VariableLang2.PresTextCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mPresText = myRow[dbconf.Variable.PresTextCol.Label()].ToString();
            }


        }
    }


    #endregion class Variable

    #region class VSGroup

    /// <summary>
    /// Holds the attributes for VSGroup. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class VSGroupRow
    {
        private String mValueSet;
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mGrouping;
        public String Grouping
        {
            get { return mGrouping; }
        }
        private String mGroupCode;
        public String GroupCode
        {
            get { return mGroupCode; }
        }
        private String mValueCode;
        public String ValueCode
        {
            get { return mValueCode; }
        }
        private String mValuePool;
        public String ValuePool
        {
            get { return mValuePool; }
        }

        public Dictionary<string, VSGroupTexts> texts = new Dictionary<string, VSGroupTexts>();

        public VSGroupRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mValueSet = myRow[dbconf.VSGroup.ValueSetCol.Label()].ToString();
            this.mGrouping = myRow[dbconf.VSGroup.GroupingCol.Label()].ToString();
            this.mGroupCode = myRow[dbconf.VSGroup.GroupCodeCol.Label()].ToString();
            this.mValueCode = myRow[dbconf.VSGroup.ValueCodeCol.Label()].ToString();
            this.mValuePool = myRow[dbconf.VSGroup.ValuePoolCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new VSGroupTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for VSGroup  for one language.
    /// </summary>
    public class VSGroupTexts
    {
        private String mSortCode;
        public String SortCode
        {
            get { return mSortCode; }
        }


        internal VSGroupTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mSortCode = myRow[dbconf.VSGroupLang2.SortCodeCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mSortCode = myRow[dbconf.VSGroup.SortCodeCol.Label()].ToString();
            }


        }
    }


    #endregion class VSGroup

    #region class VSValue

    /// <summary>
    /// Holds the attributes for VSValue. The language dependent parts are stored in the texts dictionary which is indexed by language code.
    /// </summary>
    public class VSValueRow
    {
        private String mValueSet;
        public String ValueSet
        {
            get { return mValueSet; }
        }
        private String mValuePool;
        public String ValuePool
        {
            get { return mValuePool; }
        }
        private String mValueCode;
        public String ValueCode
        {
            get { return mValueCode; }
        }

        public Dictionary<string, VSValueTexts> texts = new Dictionary<string, VSValueTexts>();

        public VSValueRow(DataRow myRow, SqlDbConfig_21 dbconf, StringCollection languageCodes, IMetaVersionComparator metaModel)
        {
            this.mValueSet = myRow[dbconf.VSValue.ValueSetCol.Label()].ToString();
            this.mValuePool = myRow[dbconf.VSValue.ValuePoolCol.Label()].ToString();
            this.mValueCode = myRow[dbconf.VSValue.ValueCodeCol.Label()].ToString();

            foreach (string languageCode in languageCodes)
            {
                texts.Add(languageCode, new VSValueTexts(myRow, dbconf, languageCode, metaModel));
            }

        }

    }

    /// <summary>
    /// Holds the language dependent attributes for VSValue  for one language.
    /// </summary>
    public class VSValueTexts
    {
        private String mSortCode;
        public String SortCode
        {
            get { return mSortCode; }
        }


        internal VSValueTexts(DataRow myRow, SqlDbConfig_21 dbconf, String languageCode, IMetaVersionComparator metaModel)
        {
            if (dbconf.isSecondaryLanguage(languageCode))
            {
                this.mSortCode = myRow[dbconf.VSValueLang2.SortCodeCol.Label(languageCode)].ToString();
            }
            else
            {
                this.mSortCode = myRow[dbconf.VSValue.SortCodeCol.Label()].ToString();
            }


        }
    }


    #endregion class VSValue

}
