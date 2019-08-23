using System;
using System.Collections.Generic;

using PCAxis.Paxiom;

using PCAxis.Sql.Pxs;
using PCAxis.Sql.DbConfig;

using log4net;
using System.Collections.Specialized;
using PCAxis.Sql.Parser;

namespace PCAxis.PlugIn.Sql
{
    
    /// <summary>
    /// This builder may be used (setPath)  with 2 types of parameters: A maintableId or a pxs-object( or a file that may be read into a pxs-object).
    /// buildForSelection with a maintableId means extract "all" metadata.
    /// not true: buildForSelection with a pxs-object means extract "all" metadata for the maintable in the pxs and mark as selected the sub-cube defined the pxs.
    /// does not work: buildForPresentation(null) with a maintableId means extract "all" metadata and data.
    /// buildForPresentation(null) with a pxs-object means extract the metadata and data for the sub-cube given by the pxs.
    /// buildForP(sel[]) (requires that buildForSel has been called () means create a pxs from _model+sel[] and do buildForPresentation(null) with a pxs-object
    /// what is supposed to happen if setPath is called more than once?
    /// </summary>
    public class PXSQLBuilder : PCAxis.Paxiom.PXModelBuilderAdapter, IDisposable, PCAxis.PlugIn.IPlugIn
    {
        
#if DEBUG
        private static readonly ILog logTime = LogManager.GetLogger("LogTime");
#endif

        private static readonly ILog log = LogManager.GetLogger(typeof(PXSQLBuilder));

        //this must be a parameter
        private PlugIn.IPlugInHost _host;

        private bool _haveDoneBuildForPresentation = false;
        private bool _haveDoneBuildForSelection = false;

        private string dbConfPath = String.Empty;

        private SqlDbConfig dbConf;

        public PXSqlMeta mPXSqlMeta;


        private bool hasSetUSer = false;
        private bool hasSetDbId = false;
        private string _user = "";
        private string _password = "";
        private string _connectionString;
        private string _dbId;
        private bool _hasCalledSetPath = false;

        /// setPath will fill maintableId or pxs ( and set the other to null, dont know if setPath may be called more than once)
        private string maintableId;
        private PxsQuery pxs;

        public PXSQLBuilder()
        {
            log.Debug("PXSQLBuilder() called");

        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void SetPath(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ApplicationException("SetPath(string path) called with path Null or Empty");
            }

            log.Debug("doing SetPath(string path) with path=\"" + path + "\"");

            Dictionary<string, Object> args = new Dictionary<string, Object>();
            if (System.IO.File.Exists(path))
            {
                args.Add(PXSqlKeywords.PXS_FILEPATH, path);

            }
            else if (path.Contains("\\"))
            {
                //throws exception with path expanded (  ..\..\ replaced by C:\mydir )
                throw new System.IO.FileNotFoundException("Can't find file: " + (new System.IO.FileInfo(path)).FullName);
            }
            else
            {
                args.Add(PXSqlKeywords.MAINTABLE_ID, path);

            }

            m_path = path;
            SetPath(args);
        }

        public void SetPath(Dictionary<string, Object> argumentDictionary)
        {
            log.Debug("doing SetPath(Dictionary<string, Object> argumentDictionary)");

            if (!(argumentDictionary.ContainsKey(PXSqlKeywords.PXS_QUERY_OBJECT) || argumentDictionary.ContainsKey(PXSqlKeywords.PXS_FILEPATH) || argumentDictionary.ContainsKey(PXSqlKeywords.MAINTABLE_ID)))
            {
                throw new ApplicationException("BUG: argumentDictionary must contain key: PXSqlKeywords.PXS_QUERY_OBJECT, PXSqlKeywords.PXS_FILEPATH or PXSqlKeywords.MAINTABLE_ID");
            }

            bool dbIdPartOfMaintableId = false;
            if (argumentDictionary.ContainsKey(PXSqlKeywords.PXS_QUERY_OBJECT))
            {
                maintableId = null;
                pxs = (PxsQuery)argumentDictionary[PXSqlKeywords.PXS_QUERY_OBJECT];

            }
            else if (argumentDictionary.ContainsKey(PXSqlKeywords.PXS_FILEPATH))
            {
                maintableId = null;
                string path = (string)argumentDictionary[PXSqlKeywords.PXS_FILEPATH];
                pxs = new PCAxis.Sql.Pxs.PxsQuery(path, m_preferredLanguage);

            }
            else
            { //maintableId
                pxs = null;

                string tmpMaintableId = (string)argumentDictionary[PXSqlKeywords.MAINTABLE_ID];
                log.Debug("tmpMaintableId = " + tmpMaintableId);
                if (tmpMaintableId.Contains(":"))
                {

                    _dbId = tmpMaintableId.Split(':')[0];
                    dbIdPartOfMaintableId = true;
                    if (_dbId != "")
                    {
                        hasSetDbId = true;
                    }
                    else
                    {
                        hasSetDbId = false;
                    }
                    maintableId = tmpMaintableId.Split(':')[1];
                }
                else
                {
                    hasSetDbId = false;
                    maintableId = tmpMaintableId;
                }

            }

            if (!dbIdPartOfMaintableId)
            {

                if (argumentDictionary.ContainsKey(PXSqlKeywords.DATABASE_ID))
                {
                    _dbId = (string)argumentDictionary[PXSqlKeywords.DATABASE_ID];
                    hasSetDbId = true;
                }
                else
                {
                    hasSetDbId = false;
                }
            }

            log.Debug("Leaving setPath: hasSetDbId =" + hasSetDbId.ToString());
            _hasCalledSetPath = true;

        }

        public override void SetUserCredentials(string userName, string password)
        {
            if (hasSetUSer)
            {
                throw new ApplicationException("BUG: can't SetUserCredentials twice.");
            }
            hasSetUSer = true;
            _user = userName;
            _password = password;
        }

        /// <summary>
        /// Builds the Paxiom model used for selection
        /// </summary>
        /// <returns></returns>
        public override bool BuildForSelection()
        {
            #if DEBUG
               System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
               stopWatch.Start();
               logTime.DebugFormat("Start "+System.Reflection.MethodBase.GetCurrentMethod().Name+". Maintable={0}", maintableId);
            #endif

            log.Debug("Start BuildForSelection");

            if (!_hasCalledSetPath)
            {
                throw new ApplicationException("BUG: SetPath must be called before BuildForSelection()");
            }

            try
            {
                setDbConfig();
                //log.Debug("Database: id = " + dbConf.Database.id + " Description = " + dbConf.Database.Description);
                log.Debug("Database: id = " + dbConf.Database.id + " Description preferred language = " + dbConf.GetDescription(m_preferredLanguage));

                log.Debug("_preferredLanguage:" + m_preferredLanguage + " _readAllLanguages:" + m_readAllLanguages.ToString());
                if (String.IsNullOrEmpty(maintableId))
                {
                    mPXSqlMeta = PXSqlMeta.GetPXSqlMeta(pxs, m_preferredLanguage, dbConf, GetInfoForDbConnection(), PCAxis.Sql.Parser.Instancemodus.selection, false);
                }
                else
                {
                    mPXSqlMeta = PXSqlMeta.GetPXSqlMeta(maintableId, m_preferredLanguage, m_readAllLanguages, dbConf, GetInfoForDbConnection(), PCAxis.Sql.Parser.Instancemodus.selection, false);
                }
                m_parser = PXSqlParser.GetPXSqlParser(mPXSqlMeta);

                // this.m_model.Meta.CreateTitle();
                base.BuildForSelection();

                if (!DoNotApplyCurrentValueSet)
                {
                    ((PXSqlParser)m_parser).SetCurrentValueSets(this.Model.Meta);
                }

                _haveDoneBuildForSelection = true;
                Model.Meta.SetPreferredLanguage(m_preferredLanguage);
            }

            catch (Exception e)
            {
                errorHandler(e, "Error in BuildForSelection");
                throw;
            }
            log.Debug("Done BuildForSelection");

            #if DEBUG
               stopWatch.Stop();
               logTime.DebugFormat(System.Reflection.MethodBase.GetCurrentMethod().Name+" Done in ms = {0}", stopWatch.ElapsedMilliseconds);
            #endif

            return true;
        }
       
        public override void ApplyValueSet(String variableCode, ValueSetInfo valueSet)
        { //Implements IPXModelBuilder.ApplyValueSet

            base.Model.Meta.Variables.GetByCode(variableCode).RecreateValues();// per inge values for variable must be deleted before created for new valueset.
            //alter state of mPXSqlMeta:
            if (valueSet == null)
            {
                mPXSqlMeta.ApplyValueSet(variableCode, null);
            }
            else
            {
                mPXSqlMeta.ApplyValueSet(variableCode, valueSet.ID);
            }

            //send new state to paxiom:
            m_parser = PXSqlParserForCodelists.GetPXSqlParserForCodelists(mPXSqlMeta, variableCode);
            base.BuildForSelection();
            // piv 04.02.2010
            base.Model.Meta.Variables.GetByCode(variableCode).CurrentValueSet = valueSet;
            base.Model.Meta.Variables.GetByCode(variableCode).CurrentGrouping = null;
        }
        public override void ApplyValueSet(String subTable)
        {
            if (this.mPXSqlMeta.CNMMVersion.Equals("2.3"))
            {
                string variableID;
                string valueSetID;
                Dictionary<string, PCAxis.Sql.QueryLib_23.SubTableVariableRow> mySubTableVariableRows = ((PCAxis.Sql.Parser_23.PXSqlMeta_23)this.mPXSqlMeta).MetaQuery.GetSubTableVariableRowskeyVariable(maintableId, subTable, true);
                foreach (KeyValuePair<string, PCAxis.Sql.QueryLib_23.SubTableVariableRow> mySubTableVariableRow in mySubTableVariableRows)
                {
                    if (mySubTableVariableRow.Value.VariableType != ((SqlDbConfig_23)dbConf).Codes.VariableTypeT)
                    {
                        variableID = mySubTableVariableRow.Value.Variable;
                        valueSetID = mySubTableVariableRow.Value.ValueSet;
                        base.Model.Meta.Variables.GetByCode(variableID).RecreateValues();// per inge values for variable must be deleted before created for new valueset.
                        //alter state of mPXSqlMeta:
                        mPXSqlMeta.ApplyValueSet(variableID, valueSetID);
                        //send new state to paxiom:
                        m_parser = PXSqlParserForCodelists.GetPXSqlParserForCodelists(mPXSqlMeta, variableID);
                        base.BuildForSelection();
                        base.Model.Meta.Variables.GetByCode(variableID).CurrentValueSet = base.Model.Meta.Variables.GetByCode(variableID).GetValuesetById(valueSetID);
                    }
                }

            }
            else if (this.mPXSqlMeta.CNMMVersion.Equals("2.4"))
            {
                string variableID;
                string valueSetID;
                Dictionary<string, PCAxis.Sql.QueryLib_24.SubTableVariableRow> mySubTableVariableRows = ((PCAxis.Sql.Parser_24.PXSqlMeta_24)this.mPXSqlMeta).MetaQuery.GetSubTableVariableRowskeyVariable(maintableId, subTable, true);
                foreach (KeyValuePair<string, PCAxis.Sql.QueryLib_24.SubTableVariableRow> mySubTableVariableRow in mySubTableVariableRows)
                {
                    if (mySubTableVariableRow.Value.VariableType != ((SqlDbConfig_24)dbConf).Codes.VariableTypeT)
                    {
                        variableID = mySubTableVariableRow.Value.Variable;
                        valueSetID = mySubTableVariableRow.Value.ValueSet;
                        base.Model.Meta.Variables.GetByCode(variableID).RecreateValues();// per inge values for variable must be deleted before created for new valueset.
                        //alter state of mPXSqlMeta:
                        mPXSqlMeta.ApplyValueSet(variableID, valueSetID);
                        //send new state to paxiom:
                        m_parser = PXSqlParserForCodelists.GetPXSqlParserForCodelists(mPXSqlMeta, variableID);
                        base.BuildForSelection();
                        base.Model.Meta.Variables.GetByCode(variableID).CurrentValueSet = base.Model.Meta.Variables.GetByCode(variableID).GetValuesetById(valueSetID);
                    }
                }

            }
            else
            {
                throw new NotImplementedException("Only implemented for version 2.3");
            }
        }


        public override void ApplyGrouping(string variableCode, GroupingInfo groupInfo, GroupingIncludesType include)
        {
            //JFI: ingen recreateValues?
            Variable paxiomVariable = base.Model.Meta.Variables.GetByCode(variableCode);

            //alter state of mPXSqlMeta, and change paxiomVariable (except sendeing codelists):
            mPXSqlMeta.ApplyGrouping(paxiomVariable, variableCode, groupInfo.ID, include);
            paxiomVariable.CurrentGrouping.GroupPres = include;

            //send the codelist:
            m_parser = PXSqlParserForCodelists.GetPXSqlParserForCodelists(mPXSqlMeta, variableCode);
            base.BuildForSelection();
            //JFI: Ingen .CurrentValueSet= ?

        }



        /// <summary>
        /// Builds the Paxiom model used for presentation
        /// </summary>
        /// <param name="selections">Selections of parameters used to specify what data that should be presented</param>
        /// <returns></returns>
        public override bool BuildForPresentation(PCAxis.Paxiom.Selection[] selections)
        {
            #if DEBUG
               System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
               stopWatch.Start();
               logTime.DebugFormat("Start " + System.Reflection.MethodBase.GetCurrentMethod().Name + ". Maintable={0}", maintableId);
            #endif

            log.Debug("Start BuildForPresentation");
            if (!_hasCalledSetPath)
            {
                throw new ApplicationException("BUG: SetPath must be called before BuildForPresentation.");
            }

            if (_haveDoneBuildForPresentation)
            {
                throw new ApplicationException("Hmm  BuildForPresentation has allready been called, what is the policy on this?");
            }
            try
            {
                setDbConfig();
                //m_parser = PXSqlParser.GetPXSqlParser(mPXSqlMeta);

                if (!_haveDoneBuildForSelection)
                {
                    // a WS or batch. selection is irrelevat and should be null. 

                    if (String.IsNullOrEmpty(maintableId))
                    {
                        mPXSqlMeta = PXSqlMeta.GetPXSqlMeta(pxs, m_preferredLanguage, dbConf, GetInfoForDbConnection(), PCAxis.Sql.Parser.Instancemodus.presentation, true);
                    }
                    else
                    {
                        mPXSqlMeta = PXSqlMeta.GetPXSqlMeta(maintableId, m_preferredLanguage, m_readAllLanguages, dbConf, GetInfoForDbConnection(), PCAxis.Sql.Parser.Instancemodus.presentation, true);
                    }
                }
                else
                {
                    //Not in use
                    //PxSQLEliniationProvider.ApplyEliminationIfSupported(selections, dbConf, GetInfoForDbConnection(), this.Model.Meta);

                    // a GUI: need to create a new pxs with the selections made in GUI
                    // and ajust the _parser and reread meta


                    PxsQuery tmpPxs = new PxsQuery(base.m_model.Meta, getLanguagesPxsQueryCreation(), selections, mPXSqlMeta.GetInfoFromPxSqlMeta2PxsQuery());

                    //for debugging: TODO Kun for test.
                    //tmpPxs.WriteToFile("aaapxs.xml");

                    mPXSqlMeta = PXSqlMeta.GetPXSqlMeta(tmpPxs, m_preferredLanguage, dbConf, GetInfoForDbConnection(), PCAxis.Sql.Parser.Instancemodus.presentation, true);
                    base.m_model.Meta = new PCAxis.Paxiom.PXMeta();

                }
                m_parser = PXSqlParser.GetPXSqlParser(mPXSqlMeta);
                //   this.m_model.Meta.CreateTitle();
                base.BuildForSelection();

                this.SetMatrixSize();

                if (!mPXSqlMeta.MainTableContainsOnlyMetaData())
                {
                    using (PXSqlData mPXSqlData = PXSqlData.GetPXSqlData(mPXSqlMeta, dbConf))
                    {
#if DEBUG
                        logTime.DebugFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " before CreateMatrix in ms = {0}", stopWatch.ElapsedMilliseconds);
#endif
                        m_model.Data.Write(mPXSqlData.CreateMatrix(), 0, mPXSqlData.MatrixSize - 1);

                        if(mPXSqlData.DataNoteCellEntries.Count >0)
                        {
                            m_model.Data.WriteDataNoteCells(mPXSqlData.DataCellNotes, 0, mPXSqlData.MatrixSize - 1);
                            m_model.Data.UseDataCellMatrix= true;
                        }

                        m_parser = PXSqlParseMetaPostData.GetPXSqlParseMetaPostData(mPXSqlMeta, mPXSqlData);

                        base.BuildForSelection();
                    }
                }
                else
                {
                    //lager juksedata
                    m_model.Data.Write(new double[m_model.Data.MatrixSize], 0, m_model.Data.MatrixSize - 1);
                    m_parser = new PXSqlParserForDataCellNote(new Dictionary<string, string>());
                    base.BuildForSelection();
                }
                m_model.Meta.Prune();
                _haveDoneBuildForSelection = true;
                _haveDoneBuildForPresentation = true;
                m_model.Meta.SetPreferredLanguage(m_preferredLanguage);

                this.m_model.IsComplete = true;
            }
            catch (Exception e)
            {
                errorHandler(e, "Error in BuildForPresentation");
                throw;
            }
            log.Debug("Done BuildForPresentation");
            #if DEBUG
               stopWatch.Stop();
               logTime.DebugFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " Done in ms = {0}", stopWatch.ElapsedMilliseconds);
            #endif
            return true;
        }
       
        private StringCollection getLanguagesPxsQueryCreation()
        {
            StringCollection langs = new StringCollection();
            if (m_readAllLanguages)
            {
                langs = dbConf.GetAllLanguages();
            }
            else if (!String.IsNullOrEmpty(m_preferredLanguage))
            {

                langs.Add(m_preferredLanguage);
            }
            else
            {
                throw new ApplicationException("Client BUG: String.IsNullOrEmpty(_preferredLanguage) && (!_readAllLanguages)");

            }
            return langs;
        }


        #region finders
        protected override PCAxis.Paxiom.Variable FindVariable(PCAxis.Paxiom.PXMeta meta, string findId)
        {
            return meta.Variables.GetByCode(findId);
        }
        protected override PCAxis.Paxiom.Variable FindVariable(PCAxis.Paxiom.PXMeta meta, string findId, int lang)
        {
            return meta.Variables.GetByCode(findId);
        }

        protected override PCAxis.Paxiom.Value FindValue(PCAxis.Paxiom.Variable variable, string findId)
        {
            return variable.Values.GetByCode(findId);
        }
        #endregion finders


        /// <summary>
        /// Hmm, some lucky keywords are given special tretment, accessing Paxiom.PXMeta directly. The others are just passed on to the normal set meta
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="subkey"></param>
        /// <param name="values"></param>
        /// <param name="meta"></param>
        /// <param name="isDefaultLanguage"></param>
        protected override void SetMeta(string keyword, string subkey, System.Collections.Specialized.StringCollection values, PCAxis.Paxiom.PXMeta meta, bool isDefaultLanguage)
        {
            switch (keyword)
            {
                //case PXKeywords.VARIABLE_NAME:
                case "VARIABLENAME":
                    SetVariableName(subkey, values[0], meta);
                    break;

                case PXKeywords.GROUPING_ID:
                    CreateGroupingInfo(subkey, values, meta);

                    break;
                case PXKeywords.GROUPING_GROUPPRES:
                    SetGroupingInfoGroupPres(subkey, values, meta);
                    break;
                case PXKeywords.GROUPING_NAME:
                    SetGroupingInfoNames(subkey, values, meta);
                    break;
                case PXKeywords.ELIMINATION:
                    SetElimination(subkey, values[0], meta);
                    break;

                default:
                    base.SetMeta(keyword, subkey, values, meta, isDefaultLanguage);
                    break;
            }

        }


        private void SetVariableName(string variablecode, string variablename, PCAxis.Paxiom.PXMeta meta)
        {
            Variable v;
            v = FindVariable(meta, variablecode);
            if (v == null)
                return;
            v.Name = variablename;
        }



        ///<summary>
        /// Creates GroupingInfo's for the cube
        ///</summary>
        ///<param name="groupingInfoIds">id for the GroupingInfo</param>
        /// <param name="variablecode">name of the variable that has 
        /// the given GropuingInfo</param>
        /// <param name="meta"></param>
        /// <remarks></remarks>
        private void CreateGroupingInfo(string variablecode, System.Collections.Specialized.StringCollection groupingInfoIds, PCAxis.Paxiom.PXMeta meta)
        {
            Variable v;
            v = FindVariable(meta, variablecode);
            if (v == null)
            {
                log.Debug("Can't find variablecode:\"" + variablecode + "\"");
                return;
            }


            GroupingInfo grInfo;
            for (int i = 0; i < groupingInfoIds.Count; i++)
            {

                grInfo = new GroupingInfo(groupingInfoIds[i]);

                v.AddGrouping(grInfo);
            }
        }



        /// <summary>
        /// Sets the groupinginfo GroupPres for a variable
        /// </summary>
        /// <param name="groupingInfoGroupPres">names of the groupingInfo</param>
        /// <param name="variablecode">name of the variable that has the given grouping</param>
        /// <param name="meta"></param>
        /// <remarks></remarks>
        private void SetGroupingInfoGroupPres(string variablecode, System.Collections.Specialized.StringCollection groupingInfoGroupPres, PCAxis.Paxiom.PXMeta meta)
        {

            Variable paxiomVariable;
            paxiomVariable = FindVariable(meta, variablecode);
            if (paxiomVariable == null)
            {

                log.Debug("SetGroupingInfoNames: Can't find variablecode:\"" + variablecode + "\"");
                return;
            }
            log.Debug("SetGroupingInfoNames: for variablecode:\"" + variablecode + "\"");

            if (paxiomVariable.Groupings.Count != groupingInfoGroupPres.Count)
            {
                throw new ApplicationException("Number of names differ from number of value sets");
            }

            PCAxis.Paxiom.GroupingIncludesType aggregationType;


            for (int i = 0; i < groupingInfoGroupPres.Count; i++)
            {
                log.Debug("groupingInfoGroupPres[i]" + groupingInfoGroupPres[i]);

                switch (groupingInfoGroupPres[i])
                {
                    case "SingleValues":
                        aggregationType = GroupingIncludesType.SingleValues;
                        break;

                    case "AggregatedValues":
                        aggregationType = GroupingIncludesType.AggregatedValues;
                        break;

                    default:
                        aggregationType = GroupingIncludesType.All;
                        break;
                }

                paxiomVariable.Groupings[i].GroupPres = aggregationType;
            }

        }


        /// <summary>
        /// Sets the groupinginfo names for a variable
        /// </summary>
        /// <param name="groupingInfoNames">names of the groupingInfo</param>
        /// <param name="variablecode">name of the variable that has the given grouping</param>
        /// <param name="meta"></param>
        /// <remarks></remarks>
        private void SetGroupingInfoNames(string variablecode, System.Collections.Specialized.StringCollection groupingInfoNames, PCAxis.Paxiom.PXMeta meta)
        {

            Variable paxiomVariable;
            paxiomVariable = FindVariable(meta, variablecode);
            if (paxiomVariable == null)
            {

                log.Debug("SetGroupingInfoNames: Can't find variablecode:\"" + variablecode + "\"");
                return;
            }
            log.Debug("SetGroupingInfoNames: for variablecode:\"" + variablecode + "\"");

            if (paxiomVariable.Groupings.Count != groupingInfoNames.Count)
            {
                throw new ApplicationException("Number of names differ from number of value sets");
            }
            for (int i = 0; i < groupingInfoNames.Count; i++)
            {
                log.Debug("groupingInfoNames[i]" + groupingInfoNames[i]);
                paxiomVariable.Groupings[i].Name = groupingInfoNames[i];
            }

        }



        private void SetElimination(string variablename, string elimination, PCAxis.Paxiom.PXMeta meta)
        {
            PCAxis.Paxiom.Variable v;
            v = FindVariable(meta, variablename);
            if (v == null)
            {
                this.Warnings.Add(new BuilderMessage(ErrorCodes.ELIMINATION));
                return;
            }
            v.SetElimination(elimination);
            if (elimination != "YES")
            {
                v.SetEliminationValue(elimination);
            }
        }

        /// <summary>
        /// Sets the Matrix Size of _model based on info from _model.
        /// </summary>
        private void SetMatrixSize()
        {
            //The real number of columns in the entire data matrix includes unselected data
            int lDataColumnLength = 1;
            //The real number of rows in the entire data matrix includes unselected data
            int lDataRowLength = 1;
            foreach (Variable var in m_model.Meta.Heading)
            {
                lDataColumnLength *= var.Values.Count;
            }

            foreach (Variable var in m_model.Meta.Stub)
            {
                //lDataRowLength *= var.Values.Count;
                lDataRowLength *= Math.Max(1, var.Values.Count);
            }

            m_model.Data.SetMatrixSize(lDataRowLength, lDataColumnLength);
        }

        /// <summary>
        /// gets a SqlDbConfig instance (with any user/passeword given) the first time it is called. 
        /// </summary>
        private void setDbConfig()
        {
            if (!hasSetDbId)
            {
                dbConf = SqlDbConfigsStatic.DefaultDatabase;
            }
            else
            {
                log.Debug("Using explisit dbId= \"" + _dbId + "\"");
                if (!SqlDbConfigsStatic.DataBases.ContainsKey(_dbId))
                {
                    throw new ApplicationException("Can't find databaseId =\"" + _dbId + "\"");
                }
                dbConf = SqlDbConfigsStatic.DataBases[_dbId];
            }
        }


        private InfoForDbConnection GetInfoForDbConnection()
        {
            if (_connectionString != null)
            {
                log.Debug("Using explicit connection string.");
                return dbConf.GetInfoForDbConnection(_connectionString);
            }

            return dbConf.GetInfoForDbConnection(_user, _password);
        }

        #region "Plugin implementation"


        public string Description
        {
            get { return "Model builder from SQL-databases"; }

        }

        public System.Guid Id
        {
            //TODO; change this (it is the same as PXFILEBuilder)
            get { return new Guid("b572c18f-a5bd-4297-9a85-7e67b54cdf84"); }
        }

        public string Name
        {
            get { return "PXSQLBuilder"; }
        }

        public void Initialize(PlugIn.IPlugInHost host, Dictionary<string, string> config)
        {
            _host = host;
            throw new Exception("The method or operation is not implemented.");
        }

        public void Terminate()
        {
            throw new Exception("The method or operation is not implemented.");
        }




        #endregion

        /// <summary>
        /// Calls dispose
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        private void errorHandler(Exception e, string message)
        {
            log.Error(message, e);
            try
            {
                this.Dispose();
            }
            catch (Exception f)
            {

                log.Error("There is also an exception in dispose", f);
            }
        }



        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            if (mPXSqlMeta != null)
            {
                mPXSqlMeta.Dispose();
            }
        }


    }
}
