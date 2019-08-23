using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PCAxis.Paxiom;
using PCAxis.PlugIn;
using System.Data;
using PCAxis.Sql.QueryLib_24;
using PCAxis.Sql.DbConfig; // ReadSqlDbConfig;
using PCAxis.Sql.Pxs;
using log4net;
using PCAxis.PlugIn.Sql;
using System.Linq;

namespace PCAxis.Sql.Parser_24
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
 

    public class PXSqlMeta_24 : PCAxis.Sql.Parser.PXSqlMeta
    {
        #region constants
        private const string mPresTextOptionCode = "CODE";
        private const string mPresTextOptionText = "TEXT";
        private const string mPresTextOptionBoth = "BOTH";
        private const string mContVariableName = "CONTENTS";
        internal const string mContVariableCode = "ContentsCode";
        //private const string PaxiomElimNo = "N0";
        //private const string PaxiomElimYes = "YES";
        //private const string SelectCounter = "COUNT";

        #endregion

        #region members
        private static readonly ILog log = LogManager.GetLogger(typeof(PXSqlMeta_24));

        private PXSqlAttributes mAttributes;
        internal PXSqlAttributes Attributes
        {
            get { return mAttributes; }
        }        

        private PxsQuery mPxsFile;
        internal PxsQuery PxsFile
        {
            get { return mPxsFile; }
        }
        internal PXSqlNpm mPxsqlNpm;
       
        private string mPxsSubTableId;
                      
        /// <summary>
        /// The ID of the Contents dimention
        /// </summary>
        internal string ContensCode
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
        internal PXSqlMaintable MainTable
        {
            get { return mMainTable; }
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
        }

        private List<PXSqlVariable> mStubs;
        public List<PXSqlVariable> Stubs
        {
            get { return mStubs; }
        }

        private List<PXSqlVariable> mHeadings;
        public List<PXSqlVariable> Headings
        {
            get { return mHeadings; }
        }

        private Dictionary<string, PXSqlContent> mContents;
        public Dictionary<string, PXSqlContent> Contents
        {
            get { return mContents; }
        }

        private PXSqlVariable mSqlVariable;
        /// <summary>
        /// All variables in DB
        /// </summary>
        private PXSqlVariables mVariables;
        /// <summary>
        /// All variables in DB
        /// </summary>
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


       
        private SqlDbConfig_24 mConfig;
        public SqlDbConfig_24 Config
        {
            get { return mConfig; }
        }
        
        //jfi:Hei piv, jeg la denne her. Er det ok?
        public bool SpecCharExists
        {
            get { return MainTable.SpecCharExists.Equals(mMetaQuery.DB.Codes.Yes); }
        }

        
        private string mDataTablesPrefix;
        public string DataTablesPrefix { get { return mDataTablesPrefix; } }


        private DataSet mTimeInfoTbl;
        private DataRowCollection mTimeInfo;


        private PXSqlNotes theNotes;

        internal PXSqlNotes TheNotes
        {
            get { return theNotes; }
        }
        

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
        public PXSqlMeta_24(string mainTableId, string preferredLang, bool getAllLangs, SqlDbConfig config, InfoForDbConnection selectedDbInfo, PCAxis.Sql.Parser.Instancemodus aModus, bool useTempTables)
            : base(config, selectedDbInfo, aModus, false)
        {
            log.Info("PXSqlMeta(string mainTableId(=" + mainTableId + "), StringCollection desiredLanguages, SqlDbConfig config, Instancemodus aModus(=" + aModus.ToString() + "))");
            this.mMainTableId = mainTableId;
            this.mConfig = (SqlDbConfig_24) config;
            mMetaQuery = new MetaQuery(this.mConfig, this.SelectedDbInfo, useTempTables);
            SetLanguageCodesNoPxs(preferredLang, getAllLangs);
            BuildMeta();
        }



        public PXSqlMeta_24(PxsQuery mPxsObject, string preferredLang, SqlDbConfig config, InfoForDbConnection selectedDbInfo, PCAxis.Sql.Parser.Instancemodus aModus, bool useTempTables)
            : base(config, selectedDbInfo, aModus, true)

        {
            log.Debug("PXSqlMeta(PxsQuery mPxsObject, SqlDbConfig config, Instancemodus aModus");
           

            //disse er trukket hit for å kunne kjøre med String hovedtabellId.
            this.mMainTableId = mPxsObject.Query.TableSource;

            //TODO; denne burde kunne fjernes
            this.mPxsSubTableId = mPxsObject.Query.SubTable;
            this.mConfig = (SqlDbConfig_24) config;

            mMetaQuery = new MetaQuery(this.mConfig, this.SelectedDbInfo, useTempTables);

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
                Dictionary<string, SubTableVariableRow> fromDB = this.MetaQuery.GetSubTableVariableRowskeyVariable(this.mMainTableId, mPxsObject.Query.SubTable,false);
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

            mPXMetaAdmValues = new PXMetaAdmValues(mMetaQuery); 

            if (inPresentationModus)
            {
                mPxsqlNpm = new PXSqlNpm(this); //trenger valium dette
            }

            


            SetMainTable(); 

            //mSubTables = new PXSqlSubTables(mMetaQuery.GetSubTableRows(mMainTableId), mPxsSubTableId);
            mSubTables = new PXSqlSubTables(mMetaQuery.GetSubTableRows(mMainTableId,false), mPxsFile, this);


            

            SetVariables();//ok for pxs == null, men denne kan også hente ut valueSet
            SetContents(); //må skrives om, men kall til metaQ OK. Bør kanskje legge inn contents som
            SetHeadingAndStub();

            SetAttributes();

            // hmm petros bruke TIMEVAL nøkkelord for å bestemme om en variabel er tid, men timeval
            // er ikke obligatorisk og har ingen mening i "valg modus".  Johannes trenger å vite om en 
            //variabel er tid for å danne korrekt pxs.
            //     if (instancemodus == Instancemodus.presentation)
            //     {
           // SetTimeVal(); Not used. Defined in PXSqlVariableTime
            SetPaxiomMap();//ok for pxs == null, men jeg forstår ikke helt hva den gjør.
            mEliminatedVariablesExist = CheckEliminatedVariables();//ok for pxs == null
            
            theNotes = new PXSqlNotes(this,mMainTableId, this.inPresentationModus);
            mDataTablesPrefix = mMetaQuery.GetDataTablesPrefix(mMainTable.ProductCode);
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

 


        private void SetInstanceModus()
        {
            if (! (this.inPresentationModus || this.inSelectionModus) )
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
            DataStorageRow tmpDataStoreageRow = mMetaQuery.GetDataStorageRow(altIBasen.ProductCode);
            MenuSelectionRow tmpMenuSelectionRow = mMetaQuery.GetMenuSelectionRow("START", altIBasen.SubjectCode);

            mMainTable = new PXSqlMaintable(altIBasen, tmpDataStoreageRow, tmpMenuSelectionRow, this);
        }





        private List<PXSqlContent> GetSortedContentsList(string mMainTableId, bool contructedFromPxs, BasicValueType[] contentsInPxs)
        {
            List<PXSqlContent> myOut = new List<PXSqlContent>();
            Dictionary<string, ContentsRow> altIBasen = mMetaQuery.GetContentsRows(mMainTableId,false);


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

   private void SetAttributes()
   {
       mAttributes = new PXSqlAttributes(this);
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

                sortedTimeValues.Add(row[mMetaQuery.DB.ContentsTime.TimePeriodCol.PureColumnName()].ToString());
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
        /// Get the IDs of the Variables In  Output Order
        /// </summary>
        /// <returns></returns>
        internal List<string> GetVariableIDsInOutputOrder()
        {
            List<string> myOut = new List<string>();

            foreach (PXSqlVariable var in this.Stubs)
            {
                myOut.Add(var.Name);
            }

            foreach (PXSqlVariable var in this.Headings)
            {
                myOut.Add(var.Name);
            }
            return myOut;
        }

        
        /// <summary>
        /// Get the IDs of the Variables In Reverse Output Order
        /// </summary>
        /// <returns></returns>
        internal List<string> GetVariableIDsInReverseOutputOrder()
        {
            List<string> myOut = new List<string>();

            myOut = GetVariableIDsInOutputOrder();
            myOut.Reverse();
            return myOut;
        }


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

