using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PCAxis.Paxiom;
using PCAxis.PlugIn;
using System.Data;
using PCAxis.Sql.QueryLib_21;
using PCAxis.Sql.DbConfig; // ReadSqlDbConfig;
using PCAxis.Sql.Pxs;
using log4net;
using PCAxis.PlugIn.Sql;

namespace PCAxis.Sql.Parser_21
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    //public enum TimeOption
    //{
    //    selected = 0,
    //    Onlylast = 1,
    //    movingWindow = 2,
    //    fixedStartMovingLast = 3

    //}
 

    public class PXSqlMeta_21 :PCAxis.Sql.Parser.PXSqlMeta
    {
        #region constants
        private const string mPresTextOptionCode = "CODE";
        private const string mPresTextOptionText = "TEXT";
        private const string mPresTextOptionBoth = "BOTH";
        private const string mContVariableName = "CONTENTS";
        public const string mContVariableCode = "ContentsCode";
        //private const string PaxiomElimNo = "N0";
        //private const string PaxiomElimYes = "YES";
        //private const string SelectCounter = "COUNT";

        #endregion

        #region members
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlMeta_21));

        private StringCollection showFootnotesCodes;
        private PxsQuery mPxsFile;
        public PxsQuery PxsFile
        {
            get { return mPxsFile; }
            //set { mPxsFile = value; }
        }
        internal PXSqlNpm mPxsqlNpm;
        // private string mPxsMainTableId; bruk   mMainTableId;
        private string mPxsSubTableId;
        //private Texts mTexts; 
        private string mMetaModelVersion;
        public string MetaModelVersion { get { return mMetaModelVersion; } }
        //private string instanceModus = "P"; // presentation;
       

        public string ContensCode
        {
            get
            {
                if (!ConstructedFromPxs)
                {
                    return mContVariableCode;
                }
                else
                {
                    return mPxsFile.Query.Contents.code;
                }
            }
        }
       
        
    
        

        private PXMetaAdmValues mPXMetaAdmValues;

        public PXMetaAdmValues PXMetaAdmValues
        {
            get { return mPXMetaAdmValues; }
        }



     

        private readonly string mMainTableId;

        private PXSqlMaintable mMainTable;
        public PXSqlMaintable MainTable
        {
            get { return mMainTable; }
            //set { mMainTable = value; }
        }

        
        private PXSqlSubTables mSubTables;
        public PXSqlSubTables SubTables
        {
            get { return mSubTables; }
        }

        private PXSqlThingsThatWouldBeTheSameInAllPXSqlContent couldHaveBeenByMainTableOnly;
        internal PXSqlThingsThatWouldBeTheSameInAllPXSqlContent CouldHaveBeenByMainTableOnly
        {
            get { return couldHaveBeenByMainTableOnly; }
        }

        private string mFirstContents;
        public string FirstContents
        {
            get { return mFirstContents; }
            //set { mFirstContents = value; }
        }
        private List<PXSqlVariable> mStubs;
        public List<PXSqlVariable> Stubs
        {
            get { return mStubs; }
            //set { mStubs = value; }
        }
        private List<PXSqlVariable> mHeadings;
        public List<PXSqlVariable> Headings
        {
            get { return mHeadings; }
            //set { mHeadings = value; }
        }

        private Dictionary<string, PXSqlContent> mContents;
        public Dictionary<string, PXSqlContent> Contents
        {
            get { return mContents; }
            //set { mContents = value; }
        }
        private PXSqlVariable mSqlVariable;
        /// <summary>
        /// All variables in DB
        /// </summary>
        //private Dictionary<string, PXSqlVariable> mVariables;
        private PXSqlVariables mVariables;
        /// <summary>
        /// All variables in DB
        /// </summary>
        //public Dictionary<string, PXSqlVariable> Variables {
        public PXSqlVariables Variables
        {
            get { return mVariables; }
        }

        private PXSqlVariablesClassification mVariablesClassification;
        /// <summary>
        /// All (selected/used or not) Classificationvariables from DB for this maintable.
        /// </summary>
        public PXSqlVariablesClassification VariablesClassification
        {
            get { return mVariablesClassification; }
        }
        private PXSqlVariable mTimeVariable;
        public PXSqlVariable TimeVariable
        {
            get { return mTimeVariable; }
        }
        private PXSqlVariable mContentsVariable;
        public PXSqlVariable ContentsVariable
        {
            get { return mContentsVariable; }
        }
        
        

        private PXSqlDecimalStuff mDecimalHandler = new PXSqlDecimalStuff();
        internal PXSqlDecimalStuff DecimalHandler
        {
            get { return mDecimalHandler; }
        }



        private PXSqlValue mValue;
        
        private Dictionary<string, string> mContentsVariablePresText = new Dictionary<string, string>();
        public Dictionary<string, string> ContentsVariablePresText
        {
            get { return mContentsVariablePresText; }
        }
        private MetaQuery mMetaQuery;
        public MetaQuery MetaQuery
        {
            get { return mMetaQuery; }
        }
        private bool mEliminatedVariablesExist;
        public bool EliminatedVariablesExist
        {
            get { return mEliminatedVariablesExist; }
        }


        private PaxiomNotes mPaxiomNotes;
        public PaxiomNotes PaxiomNotes
        {
            get { return mPaxiomNotes; }
        }

        private PXSqlMenuSelNotes mMenuSelNotes;
        public PXSqlMenuSelNotes MenuSelNotes
        {
            get { return mMenuSelNotes; }
        }
        private PXSqlMainTablesValueNotes mMaintValueNotes;
        public PXSqlMainTablesValueNotes MaintValueNotes
        {
            get { return mMaintValueNotes; }
        }
        private PXSqlValueNotes mValueNotes;
        public PXSqlValueNotes ValueNotes
        {
            get { return mValueNotes; }
        }
        private PXSqlContValueNotes mContValueNotes;
        public PXSqlContValueNotes ContValueNotes
        {
            get { return mContValueNotes; }
        }
        private PXSqlContTimeNotes mContTimeNotes;
        public PXSqlContTimeNotes ContTimeNotes
        {
            get { return mContTimeNotes; }
        }
        private PXSqlContentsNotes mContentsNotes;
        public PXSqlContentsNotes ContentsNotes
        {
            get { return mContentsNotes; }
        }
        private PXSqlContVblNotes mContVblNotes;
        public PXSqlContVblNotes ContVblNotes
        {
            get { return mContVblNotes; }
        }
        private PXSqlVariableNotes mVariableNotes;
        public PXSqlVariableNotes VariableNotes
        {
            get { return mVariableNotes; }
        }
        private PXSqlCellNotes mCellNotes;
        public PXSqlCellNotes CellNotes
        {
            get { return mCellNotes; }
        }
        private SqlDbConfig_21 mConfig;
        public SqlDbConfig_21 Config
        {
            get { return mConfig; }
        }
        
        //jfi:Hei piv, jeg la denne her. Er det ok?
        public bool SpecCharExists
        {
            get { return MainTable.SpecCharExists.Equals(mMetaQuery.DB.Codes.Yes); }
        }

        /// <summary>
        /// True if one or more variable uses grouping
        /// </summary>
        public bool HasGrouping
        {
            get
            {
                foreach (PXSqlVariable var in mVariables.Values)
                {
                    if (var.UsesGrouping)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        private string mDataTablesPrefix;
        public string DataTablesPrefix { get { return mDataTablesPrefix; } }


        private DataSet mTimeInfoTbl;
        private DataRowCollection mTimeInfo;
        // private List<RelevantFootNotesRow> mRelevantFootNootes;


        #endregion
        #region Constructor

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainTableId"></param>
        /// <param name="preferredLang">The code ("no","en",...) of the language the client wants as main language in paxiom. May be null or empty, indicating the client dont care, in which case a "random" language is choosen.</param>
        /// <param name="getAllLangs"></param>
        /// <param name="config"></param>
        /// <param name="selectedDbInfo"></param>
        /// <param name="aModus"></param>
        public PXSqlMeta_21(string mainTableId, string preferredLang, bool getAllLangs, SqlDbConfig config, InfoForDbConnection selectedDbInfo, PCAxis.Sql.Parser.Instancemodus aModus)
            : base(config, selectedDbInfo, aModus, false)
        {
            log.Info("PXSqlMeta(string mainTableId(=" + mainTableId + "), StringCollection desiredLanguages, SqlDbConfig config, Instancemodus aModus(=" + aModus.ToString() + "))");
            this.mMainTableId = mainTableId;
            this.mConfig = (SqlDbConfig_21) config;
            mMetaQuery = new MetaQuery(this.mConfig, this.SelectedDbInfo);
            SetLanguageCodesNoPxs(preferredLang, getAllLangs);
            BuildMeta();
        }



        public PXSqlMeta_21(PxsQuery mPxsObject, string preferredLang, SqlDbConfig config, InfoForDbConnection selectedDbInfo, PCAxis.Sql.Parser.Instancemodus aModus)
            : base(config, selectedDbInfo, aModus, true)

        {
            log.Debug("PXSqlMeta(PxsQuery mPxsObject, SqlDbConfig config, Instancemodus aModus");
           

            //disse er trukket hit for å kunne kjøre med String hovedtabellId.
            this.mMainTableId = mPxsObject.Query.TableSource;

            //TODO; denne burde kunne fjernes
            this.mPxsSubTableId = mPxsObject.Query.SubTable;
            this.mConfig = (SqlDbConfig_21) config;

            mMetaQuery = new MetaQuery(this.mConfig, this.SelectedDbInfo);

            this.mPxsFile = this.rearrangePxsQuery(mPxsObject);

            SetLanguageCodesFromPxs(preferredLang);
            BuildMeta(); // 
        }

        /// <summary>
        /// Moves information from PxsObject.Query.SubTable to mPxsObject.Query.Variables[XXX].SelectedValueset.
        /// So that we do not need to look both places in the rest of the code.
        /// </summary>
        /// <param name="mPxsObject"></param>
        /// <returns></returns>
        private PxsQuery rearrangePxsQuery(PxsQuery mPxsObject)
        {

            if (!String.IsNullOrEmpty(mPxsObject.Query.SubTable))
            {

                Dictionary<string, string> valuesetIdByVariableId = new Dictionary<string, string>();
                Dictionary<string, SubTableVariableRow> fromDB = this.MetaQuery.GetSubTableVariableRowskeyVariable(this.mMainTableId, mPxsObject.Query.SubTable);
                foreach (string variableId in fromDB.Keys)
                {
                    if (String.IsNullOrEmpty(fromDB[variableId].ValueSet))
                    {
                        continue;  //assuming (I know :-) time
                    }
                    valuesetIdByVariableId.Add(variableId, fromDB[variableId].ValueSet);


                }
                mPxsObject.SetSelectedValuesetsAndNullSubtable(valuesetIdByVariableId);
            }
            else
            {

                foreach (var pxsVariable in mPxsObject.Query.Variables)
                {
                    if (String.IsNullOrEmpty(pxsVariable.SelectedValueset))
                    {
                        if (pxsVariable.Values.Items.Length > 0)
                        {

                            pxsVariable.SelectedValueset = PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS;
                        }
                    }
                }
                
            }
            
            return mPxsObject;
        }


        #endregion
        #region BuildMeta
        private void BuildMeta()
        {

            SetInstanceModus(); 
            
            mMetaQuery.LanguageCodes = LanguageCodes; // instanced above, now just set language
            SetMetaModelVersion();     // sjekker ikke mot pxs
            if (inPresentationModus)
            {
                mPxsqlNpm = new PXSqlNpm(this); //trenger valium dette
            }

            mPXMetaAdmValues = new PXMetaAdmValues(mMetaQuery.GetMetaAdmAllRows(), mMetaQuery.DB);


            SetMainTable(); 

            //mSubTables = new PXSqlSubTables(mMetaQuery.GetSubTableRows(mMainTableId), mPxsSubTableId);
            mSubTables = new PXSqlSubTables(mMetaQuery.GetSubTableRows(mMainTableId), mPxsFile, this);


            

            SetVariables();//ok for pxs == null, men denne kan også hente ut valueSet
            SetContents(); //må skrives om, men kall til metaQ OK. Bør kanskje legge inn contents som
            SetHeadingAndStub();
            
           

            // hmm petros bruke TIMEVAL nøkkelord for å bestemme om en variabel er tid, men timeval
            // er ikke obligatorisk og har ingen mening i "valg modus".  Johannes trenger å vite om en 
            //variabel er tid for å danne korrekt pxs.
            //     if (instancemodus == Instancemodus.presentation)
            //     {
           // SetTimeVal(); Not used. Defined in PXSqlVariableTime
            SetPaxiomMap();//ok for pxs == null, men jeg forstår ikke helt hva den gjør.
            mEliminatedVariablesExist = CheckEliminatedVariables();//ok for pxs == null
            SetFootNotes();            //  //ok for pxs == null trur eg                      
            mDataTablesPrefix = mMetaQuery.GetDataTablesPrefix(mMainTable.ProductId);
            CheckPxs();

        }
        #endregion
        #region Instanciate DataModel structs
        //private void instDataSets() // brukes ikke lenger.  Datasett leses i setRutiner
        //{

        //    //can't find any use of Menu yet but it will be need for "parse all meta"
        //    // MenuSel
        //    //mMenuSelTbl = mMetaQuery.GetMenuSelectionById("START", mPXSqlMainTable.SubjectCode );
        //    //mMenuSel = mMenuSelTbl.Tables[0].Rows;


        #endregion
        #region Setroutines

        /// <summary>
        /// mMetaModelVersion to 0. 
        /// The following is no longer true:
        /// Sets the mMetaModelVersion to that of the pxs-file and checks that this is consistent with 
        /// the db. If not an exception is thrown
        /// </summary>
        private void SetMetaModelVersion()
        {

            //mMetaModelVersion = 0;
            mMetaModelVersion = mMetaQuery.MetaModelVersion;
           //mMetaModelVersion = "2.0";  //TESTTESTTEST
            /*
            decimal.Parse(mPxsFile.Information.PxsVersion, new System.Globalization.CultureInfo("en-GB").NumberFormat);
            if (mMetaModelVersion > mMetaQuery.MetaModelVersion) {
                string message = "The pxs-file requires meta model version " + mMetaModelVersion +
                    " , but the database is only version " + mMetaQuery.MetaModelVersion;
                System.Console.WriteLine("Exception kommentert ut:" + message);
                //throw new PCAxis.Sql.Exceptions.DbMetaVersionTooLowException(message);
            }
             */
        }


        private void SetInstanceModus()
        {
            if (this.inPresentationModus)
            {
                showFootnotesCodes = new StringCollection();
                showFootnotesCodes.Add(mConfig.Codes.FootnoteShowP);
                showFootnotesCodes.Add(mConfig.Codes.FootnoteShowB);
            }
            else if (this.inSelectionModus)
            {
                showFootnotesCodes = new StringCollection();
                showFootnotesCodes.Add(mConfig.Codes.FootnoteShowS);
                showFootnotesCodes.Add(mConfig.Codes.FootnoteShowB);
            }
            else
            {
                throw new NotImplementedException("BUG");
            }
        }

        /// <summary>
        /// Sets mLanguageCodes and mMainLanguageCode and mHasLanguage 
        /// </summary>
        /// 
        private StringCollection GetLanguagesForMainTable()
        {
            return mMetaQuery.GetLanguagesForMainTable(mMainTableId, mConfig.GetAllLanguages());

        }
        private void SetLanguageCodesFromPxs(string preferredLang)
        {
            StringCollection languagesInMaintable = GetLanguagesForMainTable();
            mLanguageCodes = new StringCollection();



            if (mPxsFile.Query.Languages.Items.Length > 0)
            {
                mHasLanguage = true;
                log.Debug(mPxsFile.Query.Languages.Items[0].GetType().ToString());

                if ((mPxsFile.Query.Languages.Items[0].GetType().ToString().Equals("PCAxis.Sql.Pxs.allType")))
                {
                    mLanguageCodes = languagesInMaintable;

                }
                else
                { //i.e. pxs contains a list of languages

                    foreach (myLanguageType PxslangCode in mPxsFile.Query.Languages.Items)
                    {
                        if (languagesInMaintable.Contains(PxslangCode.Value))
                        {
                            mLanguageCodes.Add(PxslangCode.Value);
                        }
                        else
                        {
                            log.Warn("Database does not contain language:\"" + PxslangCode.Value + "\". Getting all languages!");
                            mLanguageCodes = languagesInMaintable;
                            break;
                        }
                    }
                }
                //    if (mLanguageCodes.Count < 1) {
                //        throw new PCAxis.Sql.Exceptions.DbPxsMismatchException(5);
                //    }
                //    if (mMainLanguageCode == null) {
                //        mMainLanguageCode = mLanguageCodes[0];
                //    }
                //}
            }
            else
            {
                log.Debug("Pxs has no languages, mPxsFile.Query.Languages.Items.Length > 0. Getting all");
                mLanguageCodes = languagesInMaintable;
            }

            //mMainLanguageCode
            if ((!String.IsNullOrEmpty(preferredLang)) && mLanguageCodes.Contains(preferredLang))
            {
                mMainLanguageCode = preferredLang;
            }
            else
            {
                log.Warn("Can't return preferred language.");
                mMainLanguageCode = mLanguageCodes[0];
            }
            mHasLanguage = true;
        }



        private void SetLanguageCodesNoPxs(string preferredLang, bool aGetAllLangs)
        {
            bool getAllLangs = aGetAllLangs;
            StringCollection languagesInMaintable = GetLanguagesForMainTable();
            mLanguageCodes = new StringCollection();



            #region bad parameters
            if (String.IsNullOrEmpty(preferredLang) && !getAllLangs)
            {
                getAllLangs = true;
                log.Debug("Setting getAllLangs = true. ( String.IsNullOrEmpty(preferredLang) && ! getAllLangs ) = true) ");
            }

            if ((!String.IsNullOrEmpty(preferredLang)) && (!languagesInMaintable.Contains(preferredLang)))
            {
                String languageList = "";
                foreach (string nLang in languagesInMaintable)
                {
                    languageList += "\"" + nLang + "\",";
                }
                languageList = languageList.TrimEnd(',');
                log.Warn("throw new PCAxis.Sql.Exceptions.DbPxsMismatchException(47, langCodesfromMenu, languageList);");
                preferredLang = null;
                getAllLangs = true;
            }
            #endregion bad parameters

            if (getAllLangs)
            {
                mLanguageCodes = languagesInMaintable;
                if (String.IsNullOrEmpty(preferredLang))
                {
                    mMainLanguageCode = mLanguageCodes[0];
                }
                else
                {
                    mMainLanguageCode = preferredLang;
                }
            }
            else
            {
                mMainLanguageCode = preferredLang;
                mLanguageCodes.Add(preferredLang);
            }

            mHasLanguage = true;
            log.Debug("SetLanguageCodesNoPxs: mMainLanguageCode:" + mMainLanguageCode);
            foreach (string nLang in mLanguageCodes)
            {
                log.Debug("SetLanguageCodesNoPxs:  mLanguageCodes:" + nLang);
            }
        }




        private void SetMainTable()
        {

            MainTableRow altIBasen = mMetaQuery.GetMainTableRow(mMainTableId);
            DataStorageRow tmpDataStoreageRow = mMetaQuery.GetDataStorageRow(altIBasen.ProductId);
            MenuSelectionRow tmpMenuSelectionRow = mMetaQuery.GetMenuSelectionRow("START", altIBasen.SubjectCode);

            mMainTable = new PXSqlMaintable(altIBasen, tmpDataStoreageRow, tmpMenuSelectionRow, this);
        }





        private List<PXSqlContent> GetSortedContentsList(string mMainTableId, bool contructedFromPxs, BasicValueType[] contentsInPxs)
        {
            List<PXSqlContent> myOut = new List<PXSqlContent>();
            Dictionary<string, ContentsRow> altIBasen = mMetaQuery.GetContentsRows(mMainTableId);


            //side effect:
            ContentsRow someContentsRow = null;
            foreach (ContentsRow tmpRow in altIBasen.Values)
            { //just want any one of the rows
                someContentsRow = tmpRow;
                break;
            }

            couldHaveBeenByMainTableOnly = new PXSqlThingsThatWouldBeTheSameInAllPXSqlContent(someContentsRow, this, mConfig);

            PXSqlContact contact = new PXSqlContact(this, this.MainTable.MainTable);


            PXSqlContent mContent = null;
            if (this.ConstructedFromPxs)
            {
                int documentOrder = 0; 
                foreach (BasicValueType contents in contentsInPxs)
                {
                    if (altIBasen.ContainsKey(contents.code))
                    {
                        mContent = new PXSqlContent(altIBasen[contents.code], this, mConfig, contact);
                        mContent.SortOrder = documentOrder;
                        myOut.Add(mContent);
                        documentOrder++; 
                    }
                }

            }
            else
            {
                foreach (KeyValuePair<string, ContentsRow> cont in altIBasen)
                {
                    mContent = new PXSqlContent(cont.Value, this, mConfig, contact);
                    mContent.SortOrder = int.Parse(cont.Value.StoreColumnNo);
                    myOut.Add(mContent);
                }
            }

            if (myOut.Count < 1)
            {
                throw new ApplicationException("No contents to process");
            }
            myOut.Sort();
            return myOut;
        }



        private void SetContents()
        {
            BasicValueType[] contentsInPxs = new BasicValueType[0];
            if (this.ConstructedFromPxs)
            {
                contentsInPxs = mPxsFile.Query.Contents.Content;
            }
            List<PXSqlContent> mTempContentsList = GetSortedContentsList(mMainTableId, this.ConstructedFromPxs, contentsInPxs);

            mFirstContents = mTempContentsList[0].Contents;



            this.mContents = new Dictionary<string, PXSqlContent>();
            foreach (PXSqlContent sortedCont in mTempContentsList)
            {
                sortedCont.AdjustPresDecimalsToCommonDecimals(this.mDecimalHandler.ShowDecimals);
                mContents.Add(sortedCont.Contents, sortedCont);
            }



            PxSqlValues mValues = new PxSqlValues();
            int counter = 0;
            foreach (PXSqlContent content in mContents.Values)
            {
                mValue = new PXSqlValue(content,counter);
                mValues.Add(mValue.ValueCode, mValue);
                counter++;
            }


            mSqlVariable = new PXSqlVariableContents(mContVariableCode, this);
            mSqlVariable.Values = mValues;

            mVariables.Add(mSqlVariable.Name, mSqlVariable);
            mContentsVariable = mSqlVariable;


        }


        private void SetVariables()
        {
            //mVariables = new Dictionary<string, PXSqlVariable>();
            mVariables = new PXSqlVariables(this);
            mVariablesClassification = new PXSqlVariablesClassification();
            // Get all the variables connected to the selected maintable
            foreach (MainTableVariableRow aTVRow in mMetaQuery.GetMainTableVariableRows(mMainTableId))
            {
                if (aTVRow.VariableType == mConfig.Codes.VariableTypeT)
                {
                    mTimeVariable = new PXSqlVariableTime(aTVRow, this);
                    mSqlVariable = mTimeVariable;
                    mVariables.Add(mSqlVariable.Name, mSqlVariable);

                }
                else
                { // Should be changed so that variabletype is set separate.

                    if (!mVariables.ContainsKey(aTVRow.Variable))
                    {
                        mSqlVariable = new PXSqlVariableClassification(aTVRow, this);
                        mVariablesClassification.Add(mSqlVariable.Name, (PXSqlVariableClassification)mSqlVariable);
                        mVariables.Add(mSqlVariable.Name, mSqlVariable);
                    }
                }
                mSqlVariable.VariableType = aTVRow.VariableType;
               
            }

       
        }

        private void CheckPxs()
        {
            if (this.ConstructedFromPxs)
            {
                foreach (PQVariable selVar in mPxsFile.Query.Variables)
                {
                    if (!mVariables.ContainsKey(selVar.code))  //Should this be considered as an error? YES
                        throw new PCAxis.Sql.Exceptions.DbPxsMismatchException(7, selVar.code);

                }

                if (mPxsFile.Query.Time.ToString().Length < 1)
                {
                    throw new PCAxis.Sql.Exceptions.PxsException(8);
                }
                if (!mTimeVariable.Name.Equals(mPxsFile.Query.Time.code))
                {
                    throw new PCAxis.Sql.Exceptions.PxsException(9);
                }
            }
        }

      
       

        private void SetHeadingAndStub()
        {
            if (this.ConstructedFromPxs)
            {
                mVariables.setStubHeadPxs();
            }
            else
            {
                mVariables.setStubHeadDefault();
            }
            this.mStubs = mVariables.GetStubSorted();
            this.mHeadings = mVariables.GetHeadingSorted();
        }

    


      

        //TODO; flytt denne, tror kanskje heller ikke alt er sagt om GeoArea, det kan settes en dullion steder
        private void SetPaxiomMap()
        {
            foreach (KeyValuePair<string, PXSqlVariable> var in mVariables)
            {
                //if (!var.Value.IsContentVariable && !var.Value.IsTimevariable) {
                if (var.Value.VariableType == mConfig.Codes.VariableTypeG  && var.Value.isSelected && var.Value.ValueSets != null)
                {
                    List<string> GeoAreaValues = new List<string>();
                    string GeoAreaValue;
                    bool GeoAreaIsEqual = true;
                    int NumberOfValueSets;
                    NumberOfValueSets = var.Value.ValueSets.Count;

                    foreach (KeyValuePair<string, PXSqlValueSet> vs in var.Value.ValueSets)
                    {
                        if (vs.Key.Equals(PCAxis.PlugIn.Sql.PXSqlKeywords.FICTIONAL_ID_ALLVALUESETS)) continue;
                        GeoAreaValues.Add(vs.Value.GeoAreaNo);
                    }
                    if (NumberOfValueSets > 1)
                    {
                        GeoAreaIsEqual = CompareListValues(GeoAreaValues);
                    }
                    if (GeoAreaIsEqual)
                    {
                        GeoAreaValue = GeoAreaValues[0];
                        if (GeoAreaValue.Length > 0)
                        {
                            TextCatalogRow myTextCatalogRow = mMetaQuery.GetTextCatalogRow(int.Parse(GeoAreaValue));
                            //foreach (string langCode in LanguageCodes) {
                            var.Value.PaxiomMap = myTextCatalogRow.texts[MainLanguageCode].PresText;
                            // }
                        }
                    }
                }
            }
        }


   


        private void SetMainTableNotes(RelevantFootNotesRow footNoteRow)
        {
            // MainTable allways maps to Table in paxiom 

            mPaxiomNotes.addTableNote(new PXSqlNote(footNoteRow, this));
        }

        private void SetSubTableNotes(RelevantFootNotesRow footNoteRow)
        {
            // SubTable allways maps to Table in paxiom 
            if (mSubTables.ContainsKey(footNoteRow.SubTable))
            {
                if (mSubTables[footNoteRow.SubTable].IsSelected)
                {
                    mPaxiomNotes.addTableNote( new PXSqlNote(footNoteRow, this) );
                }

            }
        }

        private void SetVariableNotes(RelevantFootNotesRow footNoteRow)
        {

            if (mVariables.ContainsKey(footNoteRow.Variable))
            {
                if (mVariables[footNoteRow.Variable].isSelected)
                {
                    PXSqlNote mNote;
                    mNote = new PXSqlNote(footNoteRow, this);
                    mVariableNotes.Add(mNote);
                    //  mPXSqlVariables[footNoteRow.Variable].FootNotesVariable.Add(footNoteRow);
                }
            }
        }

        private void SetValueNotes(RelevantFootNotesRow footNoteRow)
        {

            if (mVariables.ContainsKey(footNoteRow.Variable))
            {
                if (mVariables[footNoteRow.Variable].isSelected)
                {
                    if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                    {
                        if (footNoteRow.ValuePool == mVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].ValuePool)
                        {
                            PXSqlNote mNote;
                            mNote = new PXSqlNote(footNoteRow, this);
                            mValueNotes.Add(mNote);
                            //          mPXSqlVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].FootNotesValue.Add(footNoteRow);
                        }
                    }
                }
            }
        }
        private void SetMaintValueNotes(RelevantFootNotesRow footNoteRow)
        {

            if (mVariables.ContainsKey(footNoteRow.Variable))
            {
                if (mVariables[footNoteRow.Variable].isSelected)
                {
                    if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                    {
                        if (footNoteRow.ValuePool == mVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].ValuePool)
                        {
                            PXSqlNote mNote;
                            mNote = new PXSqlNote(footNoteRow, this);
                            mMaintValueNotes.Add(mNote);
                            //            mPXSqlVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].FootNotesValue.Add(footNoteRow);
                        }
                    }
                }
            }
        }


        private void SetContentsNotes(RelevantFootNotesRow footNoteRow)
        {
            // Contents

            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                PXSqlNote mNote;
                mNote = new PXSqlNote(footNoteRow, this);
                mContentsNotes.Add(mNote);
                //  mPXSqlContents[footNoteRow.Contents].FootNotesContents.Add(footNoteRow);
            }
        }

        private void SetContVblNotes(RelevantFootNotesRow footNoteRow)
        {
            // ContentsVariable

            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                if (mVariables.ContainsKey(footNoteRow.Variable))
                {
                    if (mVariables[footNoteRow.Variable].isSelected)
                    {
                        PXSqlNote mNote;
                        mNote = new PXSqlNote(footNoteRow, this);
                        mContVblNotes.Add(mNote);
                        //  mPXSqlContents[footNoteRow.Contents].FootNotesContVariable.Add(footNoteRow);
                    }
                }
            }
        }

        private void SetContValueNotes(RelevantFootNotesRow footNoteRow)
        {
            // ContentsVariable
            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                if (mVariables.ContainsKey(footNoteRow.Variable))
                {
                    if (mVariables[footNoteRow.Variable].isSelected)
                    {
                        if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                            if (footNoteRow.ValuePool == mVariables[footNoteRow.Variable].Values[footNoteRow.ValueCode].ValuePool)
                            {
                                PXSqlNote mNote;
                                mNote = new PXSqlNote(footNoteRow, this);
                                mContValueNotes.Add(mNote);
                                //    mPXSqlContents[footNoteRow.Contents].FootNotesContValue.Add(footNoteRow);
                            }
                    }
                }
            }
        }

        private void SetContTimeNotes(RelevantFootNotesRow footNoteRow)
        {
            // ContentsVariable

            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                if (mTimeVariable.Values.ContainsKey(footNoteRow.TimePeriod))
                {
                    PXSqlNote mNote;
                    mNote = new PXSqlNote(footNoteRow, this);
                    mContTimeNotes.Add(mNote);
                    //  mPXSqlContents[footNoteRow.Contents].FootNotesContTime.Add(footNoteRow);
                }
            }
        }

        private void SetCellNotesNotes(RelevantFootNotesRow footNoteRow)
        {
            // ContentsVariable
            PXSqlNote mNote;
            if (mContents.ContainsKey(footNoteRow.Contents))
            {
                if (mVariables.ContainsKey(footNoteRow.Variable))
                {
                    if (mVariables[footNoteRow.Variable].isSelected)
                    {
                        if (mVariables[footNoteRow.Variable].Values.ContainsKey(footNoteRow.ValueCode))
                        {
                            if (mTimeVariable.Values.ContainsKey(footNoteRow.TimePeriod))
                            {
                                mNote = new PXSqlNote(footNoteRow, this);
                                mCellNotes.Add(mNote);
                            }

                        }
                    }


                }
            }
        }








        private void SetFootNotes()
        {

            /* the CNMM has many footnote types,  Paxiom has fewer.
             * "Upgrade" from "VALUE"  or from "Variable" to "Table" where possible*/
       
            
            mMenuSelNotes = new PXSqlMenuSelNotes();
            mMaintValueNotes = new PXSqlMainTablesValueNotes();
            mValueNotes = new PXSqlValueNotes();
            mContValueNotes = new PXSqlContValueNotes();
            mContTimeNotes = new PXSqlContTimeNotes();
            mContentsNotes = new PXSqlContentsNotes();
            mContVblNotes = new PXSqlContVblNotes();
            mVariableNotes = new PXSqlVariableNotes();
            mCellNotes = new PXSqlCellNotes();

            mPaxiomNotes = new PaxiomNotes();

            if (this.MainTable.ContainsOnlyMetaData)
            {
                mPaxiomNotes.addTableNote(new PXSqlNote(this));
            }
            foreach (RelevantFootNotesRow footNoteRow in MetaQuery.GetRelevantFoonotes(mMainTableId))
            {
                if (!this.showFootnotesCodes.Contains(footNoteRow.ShowFootNote))
                {
                    continue;
                }
                // MainTableNotes
                switch (footNoteRow.FootNoteType)
                {
                    case PXSqlNoteType.MainTable:
                        SetMainTableNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.SubTable:
                        SetSubTableNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.Contents:
                        SetContentsNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.Variable:
                        SetVariableNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.Value:
                        SetValueNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.MainTableValue:
                        SetMaintValueNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.ContentsVbl:
                        SetContVblNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.ContentsValue:
                        SetContValueNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.ContentsTime:
                        SetContTimeNotes(footNoteRow);
                        break;
                    case PXSqlNoteType.CellNote:
                        SetCellNotesNotes(footNoteRow);
                        break;

                }
            }


            //TODO; refactor: Make new class for mVariableNotes and mContentsNotes type data 
            #region mVariableNotes
            // rearranges the note according to notenumbe
            Dictionary<String, List<PXSqlNote>> varNotesByNoteNo = GetNotesByNoteNo(mVariableNotes);

            StringCollection outputVariables = this.mVariables.GetSelectedClassificationVarableIds();

            foreach (String noteno in varNotesByNoteNo.Keys)
            {
                StringCollection varsWhichHasANote = new StringCollection();
                foreach (PXSqlNote anote in varNotesByNoteNo[noteno])
                {
                    if (!varsWhichHasANote.Contains(anote.Variable))
                    {
                        varsWhichHasANote.Add(anote.Variable);
                    }
                }

                
                if (containsAll(outputVariables, varsWhichHasANote))
                {
                    mPaxiomNotes.addTableNote(varNotesByNoteNo[noteno][0]);
                } else
                {
                    mPaxiomNotes.addVariableNotes(varNotesByNoteNo[noteno]);
                }

            }
            #endregion


            #region mContentsNotes
            // rearranges the note according to notenumbe
            Dictionary<String, List<PXSqlNote>> conentsNotesByNoteNo = GetNotesByNoteNo(mContentsNotes);


            StringCollection outputContentsValues = new StringCollection(); 

            foreach (String contentsValue in this.Contents.Keys) {
                outputContentsValues.Add(contentsValue);
            }

            foreach (String noteno in conentsNotesByNoteNo.Keys)
            {
                StringCollection usedValues = new StringCollection();
                foreach (PXSqlNote anote in conentsNotesByNoteNo[noteno])
                {
                    if (!usedValues.Contains(anote.Contents))
                    {
                        usedValues.Add(anote.Contents);
                    }
                }


                

                if (containsAll(outputContentsValues, usedValues))
                {
                    mPaxiomNotes.addTableNote(conentsNotesByNoteNo[noteno][0]);
                } else
                {
                    mPaxiomNotes.addContentsNotes(conentsNotesByNoteNo[noteno]);
                }

            }
            #endregion



        }

        private Dictionary<String, List<PXSqlNote>> GetNotesByNoteNo(List<PXSqlNote> longNoteList)
        {
            Dictionary<String, List<PXSqlNote>> NotesByNoteNo = new Dictionary<string, List<PXSqlNote>>();
            foreach (PXSqlNote note in longNoteList)
            {
                List<PXSqlNote> tmp;
                if (NotesByNoteNo.ContainsKey(note.FootNoteNo))
                {
                    tmp = NotesByNoteNo[note.FootNoteNo];
                } else
                {
                    tmp = new List<PXSqlNote>();
                }
                tmp.Add(note);
                NotesByNoteNo[note.FootNoteNo] = tmp;
            }
            return NotesByNoteNo;
        }

        
        #endregion

        #region tja
        // OBS OBS denne funker bare for noen
        public StringCollection GetDataTableNames()
        {
            StringCollection subTablesString = new StringCollection();
            foreach (string subtable in mSubTables.GetKeysOfSelectedSubTables())
            {
                subTablesString.Add(DataTablesPrefix + mMainTableId + subtable);
            }
            return subTablesString;
        }





        #endregion







        #region helproutines
  
        //private string CheckMultValuesetElim(List<string> ElimValues)
        //{
        //    int numberOfElimA = 0;
        //    int numberOfElimN = 0;
        //    int numberOfElimOtherCode = 0;
        //    string elimOtherCode = "";
        //    foreach (string elimval in ElimValues)
        //    {
        //        if (elimval == mConfig.Codes.EliminationA)
        //        {
        //            numberOfElimA++;
        //        }
        //        else if (elimval == mConfig.Codes.EliminationN || String.IsNullOrEmpty(elimval))
        //        {
        //            numberOfElimN++;
        //        }
        //        else
        //        {
        //            numberOfElimOtherCode++;
        //            elimOtherCode = elimval;
        //        }

        //    }
        //    if ((numberOfElimA == ElimValues.Count - 1) && (numberOfElimOtherCode == 1))
        //    {
        //        return elimOtherCode;
        //    }
        //    else
        //    {
        //        return mConfig.Codes.EliminationN;
        //    }
        //}
        //private string ConvertToDbPresTextOption(string pxsPresTextOption) {
        //    switch (pxsPresTextOption) {
        //        case "Text":
        //            return mConfig.Codes.ValuePresT;
        //        case "Code":
        //            return mConfig.Codes.ValuePresC;
        //        case "Both":
        //            return mConfig.Codes.ValuePresB;
        //        default:
        //            return mConfig.Codes.ValuePresB;
        //    }
        //}


        private ArrayList TimeValueCodesSorted()
        {
            ArrayList sortedTimeValues = new ArrayList();
            mTimeInfoTbl = mMetaQuery.GetAllTimeValues(mMainTableId, "asc");
            mTimeInfo = mTimeInfoTbl.Tables[0].Rows;
            foreach (DataRow row in mTimeInfo)
            {

                sortedTimeValues.Add(row[mMetaQuery.DB.ContentsTime.TimePeriod].ToString());
            }
            sortedTimeValues.Sort();
            return sortedTimeValues;
        }

        /// <summary>
        /// true if one or more variables is eliminated by sum.
        /// </summary>
        /// <returns></returns>
        private bool CheckEliminatedVariables()
        {
            bool mElimExist = false;
            foreach (KeyValuePair<string, PXSqlVariable> var in mVariables)
            {
                if (var.Value.Values.Count < 1)
                {
                    mElimExist = true;
                }
            }
            return mElimExist;
        }

        #endregion



        /// <summary>
        /// Loads the InfoFromPxSqlMeta2PxsQuery with SelectedValuesetId and CurrentGroupingId for all Classificationvariable in db 
        /// </summary>
        /// <returns></returns>
        override public InfoFromPxSqlMeta2PxsQuery GetInfoFromPxSqlMeta2PxsQuery()
        {

            InfoFromPxSqlMeta2PxsQuery myOut = new InfoFromPxSqlMeta2PxsQuery();

            foreach (PXSqlVariableClassification clVar in this.VariablesClassification.Values)
            {

                myOut.AddSelectedValuesetId(clVar.Name, clVar.SelectedValueset);
                myOut.AddCurrentGroupingId(clVar.Name, clVar.CurrentGroupingId);
            }

            return myOut;
        }

 

        /// <summary>Finds the variable and passes to call on to it</summary>
        /// <param name="paxiomVariable">The paxiom Varable, but both this and the name?? </param>
        /// <param name="variableCode">The variable to which the new Grouping should be applied</param>
        /// <param name="groupingId">The id of the new grouping</param>
        /// <param name="include">Emun value inducating what the new codelist should include:parents,childern or both </param>
        override internal void ApplyGrouping(Variable paxiomVariable, string variableCode, string groupingId, GroupingIncludesType include)
        {
            if (mVariablesClassification.ContainsKey(variableCode))
            {
                mVariablesClassification[variableCode].ApplyGrouping(paxiomVariable, groupingId, include);
            } else
            {
                throw new ApplicationException("BUG");
            }
        }

        /// <summary>Finds the variable and passes to call on</summary>
        /// <param name="variableCode">The variable to which the new ValueSet should be applied</param>
        /// <param name="valueSetId">The id of the new ValueSet</param>
        override internal void ApplyValueSet(string variableCode, string valueSetId)
        {
            if (mVariablesClassification.ContainsKey(variableCode))
            {
                mVariablesClassification[variableCode].ApplyValueSet(valueSetId);
            }
            else
            {
                throw new ApplicationException("BUG");
            }
        }


        override public bool MainTableContainsOnlyMetaData(){
          return this.MainTable.ContainsOnlyMetaData;
        }

 


        #region "IPlugIn implementation"
        public Guid Id
        {
            get
            {
                //return new Guid("c6fd3c04-c14d-42ce-8271-e151a0bfdc8e");
                return new Guid("c6fd3c04-c14d-42ce-8271-e151a0bfdc8d"); //pxfileparser sin
            }

        }
        public string Name
        {
            get
            {
                return "PxSqlParser";
            }

        }
        public string Description
        {
            get
            {
                return "This plugin reads data and meta data from a SQL database an populate the Axiom model";
            }

        }
        public void Initialize(IPlugInHost host, Dictionary<string, string> configuration)
        {
        }
        public void Terminate()
        {
            this.Dispose();
        }
        #endregion
        #region IDisposable implemenatation
        override public void Dispose()
        {
            this.mMetaQuery.Dispose();
        }
        #endregion
    }
}

