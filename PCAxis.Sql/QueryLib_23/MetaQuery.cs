using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using System.Data; //For DataSet-objects.
using System.Reflection; //For MethodBase.GetCurrentMethod().

using System.Globalization;// for CultureInfo


using PCAxis.Sql.DbClient; //For executing SQLs.
using PCAxis.Sql.DbConfig; // ReadSqlDbConfig;


using log4net;
using PCAxis.Sql.Exceptions;



namespace PCAxis.Sql.QueryLib_23
{
    /// <summary>
    /// "SQL library" with all meta queries.
    /// </summary>
    public partial class MetaQuery
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MetaQuery));
#if DEBUG
        private static readonly log4net.ILog logTime = log4net.LogManager.GetLogger("LogTime");
#endif


        #region Properties and member variables


        private readonly PxSqlCommand mSqlCommand;

        private StringCollection mLanguageCodes;
        public StringCollection LanguageCodes
        {
            get { return mLanguageCodes; }
            set { mLanguageCodes = value; }
        }



        // *** Config/language settings from XMLs ***



        private readonly SqlDbConfig_23 mDbConfig;
        public SqlDbConfig_23 DB
        {
            get { return mDbConfig; }
        }

        private readonly InfoForDbConnection mSelectedDbInfo;

        public string mMd;

        /// <summary>
        /// mDbConfig.MetatablesSchema
        /// </summary>
        public string Md
        {
            get { return mMd; }
        }

        #endregion


        public MetaQuery(SqlDbConfig_23 config, InfoForDbConnection selectedDbInfo)
        {
            this.mDbConfig = config;
            this.mSelectedDbInfo = selectedDbInfo;
            this.mMd = mDbConfig.MetatablesSchema;
            this.mSqlCommand = this.GetPxSqlCommand();
            this.mLanguageCodes = null;

            CompareCNMMVersionSources();
        }


        public PxSqlCommand GetPxSqlCommand()
        {
            return new PxSqlCommandForTempTables(mSelectedDbInfo.DataBaseType, mSelectedDbInfo.DataProvider, mSelectedDbInfo.ConnectionString);
        }



        #region Methods



        public string GetDataTablesPrefix(string aProductId)
        {
            string myOut = this.GetDataStorageRow(aProductId).DatabaseName + ".";
            myOut += mSqlCommand.getExtraDotForDatatables();
            return myOut;
        }


       

        
        /// <summary>Gets the languages for which the maintable exists in the database.
        /// </summary>
        /// <param name="mainTableId">The maintable</param>
        /// <param name="dbLanguages">All the languages in the database(from dbconfig-xmlfila)</param>
        /// <returns>the languages for which the maintable exists in the database.</returns>
        public StringCollection GetLanguagesForMainTable(string mainTableId, StringCollection dbLanguages)
        {
            StringCollection maintableLanguages = new StringCollection();
            maintableLanguages.Add(DB.MainLanguage.code);
            
            Dictionary<string, SecondaryLanguageRow> SLs = this.GetSecondaryLanguageRowsbyLanguage(mainTableId, true);
                foreach (String dbLang in dbLanguages)
                {
                    if (DB.isSecondaryLanguage(dbLang))
                    {
                        if (SLs.ContainsKey(dbLang))
                        {
                            if (SLs[dbLang].CompletelyTranslated.Equals(DB.Codes.Yes))
                            {
                                maintableLanguages.Add(dbLang);
                            }
                        }

                    }
                }
            
            return maintableLanguages;
        }


        public List<MainTableVariableRow> GetMainTableVariableRows(string aMainTable)
        {
            List<MainTableVariableRow> myOut = new List<MainTableVariableRow>();


            string sqlString = MainTableVariableRow.sqlString(DB);

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = DB.SubTableVariable.MainTableCol.GetStringParameter(aMainTable);
            

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count < 1)
            {
                throw new DbPxsMismatchException(35, " MainTable = " + aMainTable);
            }
            foreach (DataRow sqlRow in myRows)
            {
                MainTableVariableRow outRow = new MainTableVariableRow(sqlRow, DB);
                myOut.Add(outRow);
            }
            return myOut;
        }


        //public DataSet GetValueSet(string MainTable, string Variable)
        //blir
        //(alternativt kunne man kjøre en sql først får å finne ValueSet id'ene og så slå dem opp)
        public List<ValueSetRow> GetValueSetRows2(string MainTable, string Variable)
        {


            string sqlString = "SELECT DISTINCT " +
            DB.ValueSet.ValueSetCol.ForSelect() + ", ";

            sqlString += DB.ValueSet.PresTextCol.ForSelect() + ", ";
        
            sqlString += DB.ValueSet.DescriptionCol.ForSelect() + ", " +
            DB.ValueSet.EliminationCol.ForSelect() + ", " +
            DB.ValueSet.ValuePoolCol.ForSelect() + ", " +
            DB.ValueSet.ValuePresCol.ForSelect() + ", " +
            DB.ValueSet.GeoAreaNoCol.ForSelect() + ", " +
            DB.ValueSet.MetaIdCol.ForSelect() + ", " +
            DB.ValueSet.SortCodeExistsCol.ForSelect() + ", " +
            DB.ValueSet.FootnoteCol.ForSelect();
            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
  
                        sqlString += ", " + DB.ValueSetLang2.PresTextCol.ForSelectWithFallback(langCode, DB.ValueSet.PresTextCol) + " ";

                    sqlString += ", " + DB.ValueSetLang2.DescriptionCol.ForSelectWithFallback(langCode, DB.ValueSet.DescriptionCol) + " ";
                }
            }
            sqlString += " /" + "*** SQLID:  GetValueSetRow2_01 ***" + "/";
            sqlString += " FROM " + DB.SubTableVariable.GetNameAndAlias() +
              " JOIN " + DB.ValueSet.GetNameAndAlias() +
              " ON " + DB.SubTableVariable.ValueSetCol.Is(DB.ValueSet.ValueSetCol);

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN " + DB.ValueSetLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.ValueSet.ValueSetCol.Is(DB.ValueSetLang2.ValueSetCol, langCode);
                }
            }

            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is() +
                           " AND " + DB.SubTableVariable.VariableCol.Is();


            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = DB.SubTableVariable.MainTableCol.GetStringParameter(MainTable);
            parameters[1] = DB.SubTableVariable.VariableCol.GetStringParameter(Variable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);

            DataRowCollection myRows = ds.Tables[0].Rows;

            //if (myRows.Count < 1) {
            //    throw new StrangeMetaDataException("No rows for " + " MainTable = " + MainTable + " Variable = " + Variable);
            //} 


            List<ValueSetRow> myOut = new List<ValueSetRow>();
            foreach (DataRow myRow in myRows)
            {
                myOut.Add(new ValueSetRow(myRow, DB, mLanguageCodes));
            }
            return myOut;
        }









        public DataSet GetValueWildCardBySubTable(string aMainTable, string aVariable, string aSubtable, string aWildCard)
        {
            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;
            string sqlWildCard = aWildCard.Replace("*", "%").Replace("?", "_");

            int noParams = 3;


            // Get the contents-table data.
            // Standard (ANSI) SQL for this query.
            //
            // SELECT DISTINCT STV.Variable, VAL.ValueCode, VAL.ValueTextS, VAL.ValueTextL /*** SQLID: GetValueWildCardBySubTable_01 ***/
            // FROM MetaData.SubtableVariable STV
            // JOIN MetaData.ValueSet VST ON (STV.ValueSet = VST.ValueSet)
            // JOIN MetaData.VSValue  VVL ON (VST.ValueSet = VVL.ValueSet)
            // JOIN MetaData.Value    VAL ON (VVL.ValuePool = VAL.ValuePool AND VVL.ValueCode = VAL.ValueCode)
            // WHERE STV.MainTable = '<mainTable>' AND STV.Variable  = '<variable>'
            //   AND STV.SubTable  = '<subtable>'
            //   AND VVL.ValueCode LIKE '<sqlWildCard>'
            // ORDER BY SORTCODE
            //
            string sqlString =
                "SELECT DISTINCT " + DB.SubTableVariable.VariableCol.Id() +
                "," + DB.Value.ValueCodeCol.Id() +
                "," + DB.Value.ValueTextSCol.Id() +
                "," + DB.Value.ValueTextLCol.Id() +
                "," + DB.Value.SortCodeCol.Id() +
                "/*** SQLID: " + currentMethod + "_01 ***/ " +
                "FROM " + DB.SubTableVariable.GetNameAndAlias() +
                " JOIN " + DB.ValueSet.GetNameAndAlias() + " ON (" + DB.SubTableVariable.ValueSetCol.Is(DB.ValueSet.ValueSetCol) + ")" +
                " JOIN " + DB.VSValue.GetNameAndAlias() + " ON (" + DB.ValueSet.ValueSetCol.Is(DB.VSValue.ValueSetCol) + ")" +
                " JOIN " + DB.Value.GetNameAndAlias() + " ON (" + DB.VSValue.ValuePoolCol.Is(DB.Value.ValuePoolCol) +
                                                         " AND " + DB.VSValue.ValueCodeCol.Is(DB.Value.ValueCodeCol) + ")" +
                " WHERE " + DB.SubTableVariable.MainTableCol.Is() +
                  " AND " + DB.SubTableVariable.VariableCol.Is();

            if (aSubtable != null)
            {
                sqlString = sqlString + " AND " + DB.SubTableVariable.SubTableCol.Is();
                noParams++;
            }

            sqlString = sqlString + " AND " + DB.VSValue.ValueCodeCol.Like();
            sqlString = sqlString + " ORDER BY " + DB.Value.SortCodeCol.Id();


            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[noParams];
            parameters[0] = DB.SubTableVariable.MainTableCol.GetStringParameter(aMainTable);
            parameters[1] = DB.SubTableVariable.VariableCol.GetStringParameter(aVariable);
            if (aSubtable != null)
            {
                parameters[2] = DB.SubTableVariable.SubTableCol.GetStringParameter(aSubtable);
                parameters[3] = DB.VSValue.ValueCodeCol.GetStringParameter(sqlWildCard);
            }
            else
            {
                parameters[2] = DB.VSValue.ValueCodeCol.GetStringParameter(sqlWildCard);
            }

           
            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString, parameters);
        }





        //flyttet hit fra fra_master2MetaQuery
        public ValueRow2HMDictionary GetValueRowDictionary(string mainTable, StringCollection subTables, string variable, StringCollection someValueCodes, string valueExtraExists)
        {
            #region example
            //
            //SELECT bx.Vardekod, bz.Vardemangd, bx.Language, bx.VardeTextL, bx.VardeTextK  --, bx.SortCodeValue, bz.SortCodeVsValue 
            //FROM
            //  (SELECT 'NOR' as Language, v.Vardekod, v.VardeForrad, v.VardeTextL, v.VardeTextK, v.SortKod AS SortCodeValue
            //   FROM statmeta.varde v
            //  UNION
            //   SELECT 'ENG' as Language, ve.Vardekod, ve.Vardeforrad, ve.VardeTextL, ve.VardeTextK, ve.SortKod AS SortCodeValue
            //   FROM statmeta.varde_eng ve
            //  ) bx
            //  JOIN (SELECT Vardemangd, Vardeforrad, Vardekod, SortKod AS SortCodeVsValue
            //        FROM statmeta.vmvarde) bz 
            //  ON (bx.Vardeforrad = bz.Vardeforrad AND bx.Vardekod = bz.Vardekod)
            //  JOIN statmeta.deltabellvariabel dv 
            //  ON (bz.Vardemangd = dv.Vardemangd)
            //WHERE dv.huvudtabell = 'Rd0001Aa'
            //  AND dv.deltabell IN ('Kommun1')
            //  AND dv.variabel = 'Region'
            //  AND ((bx.vardekod IN ('0101','0102','0118')) OR (bx.vardekod IN ('0120','0121','0128')))
            //ORDER BY language, SortCodeVsValue, SortCodeValue, Vardekod

            //SELECT bx.Vardekod, bz.Vardemangd, bx.Language, bx.VardeTextL, bx.VardeTextK  --, bx.SortCodeValue, bz.SortCodeVsValue 
            //FROM
            //  (SELECT 'NOR' as Language, v.Vardekod, v.VardeForrad, 
            //          CONCAT(vx.VardeTextX1, CONCAT(vx.VardeTextX2, CONCAT(vx.VardeTextX3, vx.VardeTextX4))) AS VardeTextL, 
            //          NULL AS VardeTextK, SortKod AS SortCodeValue
            //   FROM statmeta.varde v
            //   JOIN statmeta.vardeextra vx ON (vx.Vardeforrad = v.Vardeforrad AND vx.Vardekod = v.Vardekod)
            //  UNION
            //   SELECT 'ENG' as Language, ve.Vardekod, ve.Vardeforrad, 
            //          CONCAT(vxe.VardeTextX1, CONCAT(vxe.VardeTextX2, CONCAT(vxe.VardeTextX3, vxe.VardeTextX4))) AS VardeTextL, 
            //          NULL AS VardeTextK, SortKod AS SortCodeValue
            //   FROM statmeta.varde_eng ve
            //   JOIN statmeta.vardeextra_eng vxe ON (vxe.Vardeforrad = ve.Vardeforrad AND vxe.Vardekod = ve.Vardekod)
            //  ) bx
            //  JOIN (SELECT Vardemangd, Vardeforrad, Vardekod, SortKod AS SortCodeVsValue
            //        FROM statmeta.vmvarde) bz ON (bx.Vardeforrad = bz.Vardeforrad AND bx.Vardekod = bz.Vardekod)
            //  JOIN statmeta.deltabellvariabel dv ON (bz.Vardemangd = dv.Vardemangd)
            //WHERE dv.huvudtabell = 'SalgProdInd08'
            //  AND dv.deltabell IN ('2')
            //  AND dv.variabel = 'ProdCom'
            //  AND ((bx.vardekod IN ('22.21.30.70','22.21.30.90','24.10.31.30')) OR (bx.vardekod IN ('24.10.32.10','24.10.35.30','24.32.10.40')))
            //ORDER BY language, SortCodeVsValue, SortCodeValue, Vardekod

            //SqlDbConfig dbconf = DB;  
            // piv added DISTINCT
            #endregion example

#if DEBUG
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            logTime.Info("Start " + System.Reflection.MethodBase.GetCurrentMethod().Name + " bad");
#endif

            //TODO: Creating tempTable should be kept for reuse in the datapart
            string myVariableNumber = "007";
            string tempTabId = mSqlCommand.MakeTempTableJustValues(variable, myVariableNumber, DB.Database.Connection.useTemporaryTables, someValueCodes);


            string sqlString = "SELECT bx." + DB.Value.ValueCodeCol.PureColumnName() + ", bz." + DB.Value.ValuePoolCol.PureColumnName() + ", bz." + DB.VSValue.ValueSetCol.PureColumnName()
                             + ", bx.Language, bx." + DB.Value.ValueTextLCol.PureColumnName() + ", bx." + DB.Value.ValueTextSCol.PureColumnName()
                                + ", bx.SortCodeValue, bz.SortCodeVsValue";
            // sqlString += ", bx.SortCodeValue, bz.SortCodeVsValue";
            sqlString += " FROM (";
            #region language section
            int LangCounter = 0;
            
            //TODO; reduserte denne mye, kan man bruke noe generert nå, mon tro
           
            foreach (String langCode in mLanguageCodes)
            {
                if (LangCounter > 0)
                {
                    sqlString += " UNION ";
                }
                if (!DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " SELECT '" + langCode + "' AS Language"
                               + ", " + DB.Value.ValueCodeCol.Id() + " AS " + DB.Value.ValueCodeCol.PureColumnName()
                               + ", " + DB.Value.ValuePoolCol.Id() + " AS " + DB.Value.ValuePoolCol.PureColumnName();


                   
                        sqlString += ", " + DB.Value.ValueTextLCol.Id() + " AS " + DB.Value.ValueTextLCol.PureColumnName()
                                   + ", " + DB.Value.ValueTextSCol.Id() + " AS " + DB.Value.ValueTextSCol.PureColumnName();
                    

                    sqlString += ", " + DB.Value.SortCodeCol.Id() + " AS SortCodeValue"
                               + " FROM " + DB.MetaOwner + DB.Value.TableName + " " + DB.Value.Alias;

                    
                }
                else
                {
                    sqlString += " SELECT '" + langCode + "' AS Language"
                               + ", " + DB.ValueLang2.ValueCodeCol.Id(langCode) + " AS " + DB.Value.ValueCodeCol.PureColumnName()
                               + ", " + DB.ValueLang2.ValuePoolCol.Id(langCode) + " AS " + DB.Value.ValuePoolCol.PureColumnName();

                    
                        sqlString += ", " + DB.ValueLang2.ValueTextLCol.Id(langCode) + " AS " + DB.Value.ValueTextLCol.PureColumnName()
                                   + ", " + DB.ValueLang2.ValueTextSCol.Id(langCode) + " AS " + DB.Value.ValueTextSCol.PureColumnName();
                   

                    sqlString += ", " + DB.ValueLang2.SortCodeCol.Id(langCode) + " AS SortCodeValue"
                               + " FROM " + DB.ValueLang2.GetNameAndAlias(langCode);

                   
                }
                LangCounter++;
            }
            sqlString += ") bx";
            #endregion language section

            sqlString += " JOIN (SELECT " + DB.VSValue.ValueSetCol.PureColumnName() + " AS " + DB.VSValue.ValueSetCol.PureColumnName()
                       + ", " + DB.VSValue.ValuePoolCol.PureColumnName() + " AS " + DB.Value.ValuePoolCol.PureColumnName()
                       + ", " + DB.VSValue.ValueCodeCol.PureColumnName() + " AS " + DB.Value.ValueCodeCol.PureColumnName()
                       + ", " + DB.VSValue.SortCodeCol.PureColumnName() + " AS SortCodeVsValue"
                       + " FROM " + DB.MetaOwner + DB.VSValue.TableName
                       + ") bz ON (bx." + DB.Value.ValuePoolCol.PureColumnName() + " = bz." + DB.Value.ValuePoolCol.PureColumnName() + " AND bx." + DB.Value.ValueCodeCol.PureColumnName() + " = bz." + DB.Value.ValueCodeCol.PureColumnName() + ")";
            sqlString += " JOIN " + DB.MetaOwner + DB.SubTableVariable.TableName + " " + DB.SubTableVariable.Alias
                       + " ON (bz." + DB.VSValue.ValueSetCol.PureColumnName() + " = " + DB.SubTableVariable.ValueSetCol.Id() + ")";
            sqlString += " JOIN " + tempTabId
                + " ON  bx." + DB.VSValue.ValueCodeCol.PureColumnName() + "=" + tempTabId + ".Group" + myVariableNumber + " ";


            #region where section

            
            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is();
            int numberOfDbParameters = 1;
            
            if (subTables != null)
            {
                sqlString += " AND " + DB.SubTableVariable.SubTableCol.In(subTables);
                numberOfDbParameters += subTables.Count;
            }

            sqlString += " AND " + DB.SubTableVariable.VariableCol.Is();
            numberOfDbParameters++;

 
            #endregion where section

            sqlString += " ORDER BY Language, SortCodeVsValue, SortCodeValue, bx." + DB.Value.ValueCodeCol.PureColumnName();
           

            #region parameters to where section
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[numberOfDbParameters];
            int DbParameterIndex = 0;
            parameters[DbParameterIndex] = DB.SubTableVariable.MainTableCol.GetStringParameter(mainTable);
            DbParameterIndex++;

            // If extraction from only one subtable, this paragraph must be included
            if (subTables != null)
            {
                Array.Copy(DB.SubTableVariable.SubTableCol.GetStringParameters(subTables), 0, parameters, DbParameterIndex, subTables.Count);
                DbParameterIndex += subTables.Count;
            }

            parameters[DbParameterIndex] = DB.SubTableVariable.VariableCol.GetStringParameter(variable);
            DbParameterIndex++;
            

            #endregion parameters to where section

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataTable myTable = ds.Tables[0];
            

            ValueRow2HMDictionary myOut = new ValueRow2HMDictionary(myTable, DB, this);

            //This temptable should be reused and not deleted, but for now ...

            if (!DB.UseTemporaryTables)
            {

                try
                {

                    mSqlCommand.ExecuteNonQuery("DROP TABLE " + tempTabId);
                }
                catch (Exception e)
                {
                    log.Error("DROP TABLE failed:", e);
                    //don't want to stop because of this... 
                }

            }
            else if (mSqlCommand.getProgramMustTruncAndDropTempTable())
            {
                try
                {
                    mSqlCommand.ExecuteNonQuery("TRUNCATE TABLE " + tempTabId);
                    mSqlCommand.ExecuteNonQuery("DROP TABLE " + tempTabId);
                }
                catch (Exception e)
                {
                    log.Error("TRUNC or DROP TABLE failed:", e);
                    //don't want to stop because of this... 
                }
            }


#if DEBUG
            stopWatch.Stop();
            logTime.InfoFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " Done in ms = {0}", stopWatch.ElapsedMilliseconds);
#endif
            return myOut;
        }

        //mod for flere deltabeller.  Dersom det ikke er spesifiert verdier fra pxs, så må deltabeller spesifiseres selv om alle er valgt, da det kan finnes verdier i verdiforrådet som ikke hører til tabellen.
        public ValueRow2HMDictionary GetValueRowDictionary(string mainTable, StringCollection subTables, string variable, string valueExtraExists)
        {
            #region example
            //
            //SELECT Vardekod, Vardemangd, Language, VardeTextL, VardeTextK   --, SortCodeValue, SortCodeVsValue 
            //FROM (
            //SELECT bx.Vardekod, bz.Vardemangd, bx.Language, bx.VardeTextL, bx.VardeTextK, bx.SortCodeValue, bz.SortCodeVsValue FROM
            //       (SELECT 'NOR' as Language, Vardekod, VardeForrad, VardeTextL, VardeTextK, SortKod AS SortCodeValue FROM statmeta.varde) bx
            //  JOIN (SELECT Vardemangd, Vardeforrad, Vardekod, SortKod AS SortCodeVsValue FROM statmeta.vmvarde) bz
            //    ON (bx.Vardeforrad = bz.Vardeforrad AND bx.Vardekod = bz.Vardekod)
            //  JOIN statmeta.deltabellvariabel dv
            //    ON (bz.Vardemangd = dv.Vardemangd)
            //WHERE dv.huvudtabell = 'Rd0002AaGr'
            //  AND dv.deltabell IN ('Grunnk1')
            //  AND dv.variabel = 'Region'
            //UNION
            //SELECT bx.Vardekod, bz.Vardemangd, bx.Language, bx.VardeTextL, bx.VardeTextK, bx.SortCodeValue, bz.SortCodeVsValue FROM
            //       (SELECT 'ENG' as Language, Vardekod, VardeForrad, VardeTextL, VardeTextK, SortKod AS SortCodeValue FROM statmeta.varde_eng) bx
            //  JOIN (SELECT Vardemangd, Vardeforrad, Vardekod, SortKod AS SortCodeVsValue FROM statmeta.vmvarde) bz
            //    ON (bx.Vardeforrad = bz.Vardeforrad AND bx.Vardekod = bz.Vardekod)
            //  JOIN statmeta.deltabellvariabel dv
            //    ON (bz.Vardemangd = dv.Vardemangd)
            //WHERE dv.huvudtabell = 'Rd0002AaGr'
            //  AND dv.deltabell IN ('Grunnk1')
            //  AND dv.variabel = 'Region'
            //)
            //ORDER BY language, SortCodeVsValue, SortCodeValue, Vardekod
            //;

            //SELECT Vardekod, Vardemangd, Language, VardeTextL, VardeTextK   --, SortCodeValue, SortCodeVsValue 
            //FROM (
            //SELECT bx.Vardekod, bz.Vardemangd, bx.Language, bx.VardeTextL, bx.VardeTextK, bx.SortCodeValue, bz.SortCodeVsValue FROM
            //       (SELECT 'NOR' as Language, v.Vardekod, v.VardeForrad, 
            //        CONCAT(vx.VardeTextX1, CONCAT(vx.VardeTextX2, CONCAT(vx.VardeTextX3, vx.VardeTextX4))) AS VardeTextL, 
            //        NULL AS VardeTextK, v.SortKod AS SortCodeValue 
            //        FROM statmeta.varde v
            //        JOIN statmeta.vardeextra vx ON (vx.Vardeforrad = v.Vardeforrad AND vx.Vardekod = v.Vardekod)
            //       ) bx
            //  JOIN (SELECT Vardemangd, Vardeforrad, Vardekod, SortKod AS SortCodeVsValue FROM statmeta.vmvarde) bz
            //    ON (bx.Vardeforrad = bz.Vardeforrad AND bx.Vardekod = bz.Vardekod)
            //  JOIN statmeta.deltabellvariabel dv
            //    ON (bz.Vardemangd = dv.Vardemangd)
            //WHERE dv.huvudtabell = 'SalgProdInd08'
            //  AND dv.deltabell IN ('2')
            //  AND dv.variabel = 'ProdCom'
            //UNION
            //SELECT bx.Vardekod, bz.Vardemangd, bx.Language, bx.VardeTextL, bx.VardeTextK, bx.SortCodeValue, bz.SortCodeVsValue FROM
            //       (SELECT 'ENG' as Language, ve.Vardekod, ve.VardeForrad, 
            //        CONCAT(vxe.VardeTextX1, CONCAT(vxe.VardeTextX2, CONCAT(vxe.VardeTextX3, vxe.VardeTextX4))) AS VardeTextL, 
            //        NULL AS VardeTextK, 
            //        ve.SortKod AS SortCodeValue 
            //        FROM statmeta.varde_eng ve
            //        JOIN statmeta.vardeextra_eng vxe ON (vxe.Vardeforrad = ve.Vardeforrad AND vxe.Vardekod = ve.Vardekod)
            //      ) bx
            //  JOIN (SELECT Vardemangd, Vardeforrad, Vardekod, SortKod AS SortCodeVsValue FROM statmeta.vmvarde) bz
            //    ON (bx.Vardeforrad = bz.Vardeforrad AND bx.Vardekod = bz.Vardekod)
            //  JOIN statmeta.deltabellvariabel dv
            //    ON (bz.Vardemangd = dv.Vardemangd)
            //WHERE dv.huvudtabell = 'SalgProdInd08'
            //  AND dv.deltabell IN ('2')
            //  AND dv.variabel = 'ProdCom'
            //)
            //ORDER BY language, SortCodeVsValue, SortCodeValue, Vardekod
            //;

            //SqlDbConfig dbconf = DB;  
            // piv added DISTINCT
            #endregion example

            
            int numberOfDbParameters = mLanguageCodes.Count * (2 + subTables.Count);
            int DbParameterIndex = 0;

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[numberOfDbParameters];


            string sqlString = "SELECT bw." + DB.Value.ValueCodeCol.PureColumnName() + ", bw." + DB.Value.ValuePoolCol.PureColumnName() + ", bw." + DB.VSValue.ValueSetCol.PureColumnName()
                 + ", bw.Language, bw." + DB.Value.ValueTextLCol.PureColumnName() + ", bw." + DB.Value.ValueTextSCol.PureColumnName()
                    + ", bw.SortCodeValue, bw.SortCodeVsValue";
            sqlString += " FROM (";
            #region language section
            int LangCounter = 0;
            foreach (String langCode in mLanguageCodes)
            {
                if (LangCounter > 0)
                {
                    sqlString += " UNION ";
                }

                sqlString += "SELECT bx." + DB.Value.ValueCodeCol.PureColumnName() + ", bz." + DB.Value.ValuePoolCol.PureColumnName() + ", bz." + DB.VSValue.ValueSetCol.PureColumnName()
                                 + ", bx.Language, bx." + DB.Value.ValueTextLCol.PureColumnName() + ", bx." + DB.Value.ValueTextSCol.PureColumnName()
                                 + ", bx.SortCodeValue, bz.SortCodeVsValue FROM (";

                if (!DB.isSecondaryLanguage(langCode))
                {
                    #region primary language
                    sqlString += " SELECT '" + langCode + "' AS Language"
                               + ", " + DB.Value.ValueCodeCol.Id() + " AS " + DB.Value.ValueCodeCol.PureColumnName()
                               + ", " + DB.Value.ValuePoolCol.Id() + " AS " + DB.Value.ValuePoolCol.PureColumnName();

                    
                        sqlString += ", " + DB.Value.ValueTextLCol.Id() + " AS " + DB.Value.ValueTextLCol.PureColumnName()
                                   + ", " + DB.Value.ValueTextSCol.Id() + " AS " + DB.Value.ValueTextSCol.PureColumnName();
                   

                    sqlString += ", " + DB.Value.SortCodeCol.Id() + " AS SortCodeValue"
                               + " FROM " + DB.Value.GetNameAndAlias();

                   
                    #endregion primary language
                }
                else
                {
                    #region secondary language
                    sqlString += " SELECT '" + langCode + "' AS Language"
                               + ", " + DB.ValueLang2.ValueCodeCol.Id(langCode) + " AS " + DB.Value.ValueCodeCol.PureColumnName()
                               + ", " + DB.ValueLang2.ValuePoolCol.Id(langCode) + " AS " + DB.Value.ValuePoolCol.PureColumnName();

                    
                        sqlString += ", " + DB.ValueLang2.ValueTextLCol.Id(langCode) + " AS " + DB.Value.ValueTextLCol.PureColumnName()
                                   + ", " + DB.ValueLang2.ValueTextSCol.Id(langCode) + " AS " + DB.Value.ValueTextSCol.PureColumnName();
                    

                    sqlString += ", " + DB.ValueLang2.SortCodeCol.Id(langCode) + " AS SortCodeValue"
                               + " FROM " + DB.ValueLang2.GetNameAndAlias(langCode);

                    
                    #endregion secondary language
                }

                sqlString += ") bx"
                           + " JOIN (SELECT " + DB.VSValue.ValueSetCol.PureColumnName() + " AS " + DB.VSValue.ValueSetCol.PureColumnName()
                           + ", " + DB.VSValue.ValuePoolCol.PureColumnName() + " AS " + DB.Value.ValuePoolCol.PureColumnName()
                           + ", " + DB.VSValue.ValueCodeCol.PureColumnName() + " AS " + DB.Value.ValueCodeCol.PureColumnName()
                           + ", " + DB.VSValue.SortCodeCol.PureColumnName() + " AS SortCodeVsValue"
                           + " FROM " + DB.MetaOwner + DB.VSValue.TableName + ") bz"
                           + " ON (bx." + DB.Value.ValuePoolCol.PureColumnName() + " = bz." + DB.Value.ValuePoolCol.PureColumnName() + " AND bx." + DB.Value.ValueCodeCol.PureColumnName() + " = bz." + DB.Value.ValueCodeCol.PureColumnName() + ")"
                           + " JOIN " + DB.SubTableVariable.GetNameAndAlias()
                           + " ON (bz." + DB.VSValue.ValueSetCol.PureColumnName() + " = " + DB.SubTableVariable.ValueSetCol.Id() + ")"
                           + " WHERE " + DB.SubTableVariable.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable"+langCode));



                parameters[DbParameterIndex] = mSqlCommand.GetStringParameter("aMainTable" + langCode, mainTable);
                DbParameterIndex++;

                // If extraction from only one subtable, this paragraph must be included
                if (subTables.Count > 0)
                {
                    string tmpNameBase="";
                    sqlString += " AND " + DB.SubTableVariable.SubTableCol.Id() + " IN (";
                    for (int counter = 0; counter < subTables.Count; counter++)
                    {
                        tmpNameBase= "aSubTable" + langCode + (counter + 1);
                        sqlString += mSqlCommand.GetParameterRef(tmpNameBase) + " ,";
                        parameters[DbParameterIndex] = mSqlCommand.GetStringParameter(tmpNameBase, subTables[counter]);
                        DbParameterIndex++;
                    }
                    sqlString = sqlString.TrimEnd(',');
                    sqlString += ")";

                }

                sqlString += " AND " + DB.SubTableVariable.VariableCol.Is(mSqlCommand.GetParameterRef("aVariable" + langCode));
                parameters[DbParameterIndex] = mSqlCommand.GetStringParameter("aVariable" + langCode, variable);
                DbParameterIndex++;

                LangCounter++;
            }
            #endregion language section
            sqlString += " ) bw  ORDER BY Language, SortCodeVsValue, SortCodeValue, " + DB.Value.ValueCodeCol.PureColumnName();

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            //DataRowCollection myRows = ds.Tables[0].Rows;
            DataTable myTable = ds.Tables[0];
           
            ValueRow2HMDictionary myOut = new ValueRow2HMDictionary(myTable, DB, this);
            return myOut;
        }
      

        // newnewnew
        //flyttet hit fra fra_master2MetaQuery
        public List<ValueRowHM> GetValueRowsByValuePool(string valuePool, StringCollection someValueCodes, string valueExtraExists)
        {

            string joinString = "";

            string sqlString = "SELECT " + DB.Value.ValuePoolCol.ForSelect() + " ," +
                          DB.Value.ValueCodeCol.ForSelect() + " ," + DB.Value.MetaIdCol.ForSelect();
            foreach (String langCode in mLanguageCodes)
            {
                if (!DB.isSecondaryLanguage(langCode))
                {
                        sqlString += ", " + DB.Value.ValueTextSCol.ForSelect();
                        sqlString += ", " + DB.Value.ValueTextLCol.ForSelect();
                        sqlString += ", " + DB.Value.SortCodeCol.ForSelect();
                }
                else
                {

                    joinString += " LEFT JOIN " + DB.ValueLang2.GetNameAndAlias(langCode);
                    joinString += " ON ( " + DB.ValueLang2.ValuePoolCol.Id(langCode) + " = " + DB.Value.ValuePoolCol.Id() +
                                  " AND " + DB.ValueLang2.ValueCodeCol.Id(langCode) + " = " + DB.Value.ValueCodeCol.Id() + ")";

 
                        sqlString += ", " + GetColumnSelectionString(DB.ValueLang2.Alias + DB.GetMetaSuffix(langCode), DB.ValueLang2.ValueTextSCol.PureColumnName());
                        sqlString += ", " + GetColumnSelectionString(DB.ValueLang2.Alias + DB.GetMetaSuffix(langCode), DB.ValueLang2.ValueTextLCol.PureColumnName());

                   

                    sqlString += ", " + GetColumnSelectionString(DB.ValueLang2.Alias + DB.GetMetaSuffix(langCode), DB.ValueLang2.SortCodeCol.PureColumnName());
                }
            }

            sqlString += " FROM " + DB.Value.GetNameAndAlias();


            sqlString += joinString;

            sqlString += " WHERE " + DB.Value.ValuePoolCol.Is();

            if (someValueCodes.Count > 0)
            {
                sqlString += " AND " + DB.Value.ValueCodeCol.In(someValueCodes);
            }

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1 + someValueCodes.Count];
            parameters[0] = DB.Value.ValuePoolCol.GetStringParameter(valuePool);
            
            if (someValueCodes.Count > 0)
            {
                Array.Copy(DB.Value.ValueCodeCol.GetStringParameters(someValueCodes), 0, parameters, 1, someValueCodes.Count);
            }

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;


            List<ValueRowHM> myOut = new List<ValueRowHM>();
            foreach (DataRow myRow in myRows)
            {
                ValueRowHM tmpRow = new ValueRowHM(myRow, DB, LanguageCodes, 1);

                myOut.Add(tmpRow);
            }

            return myOut;
        }
        //end  newnewnew


        private string GetColumnSelectionString(string columnPrefix, string columnName)
        {
            return columnPrefix + "." + columnName + " AS " + columnPrefix + "_" + columnName;

        }


        public int GetNumberOfValuesInValueSetById(string aValueSet)
        {

            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;


            // 11.11.2008 ThH: CHANGED TO:
            // SELECT COUNT(*) AS COUNT 
            // FROM MetaData.VSValue VVL
            // JOIN MetaData.ValueSet VST ON VST.ValueSet = VVL.ValueSet
            // WHERE VST.ValueSet = '<ValueSet>'
            //
            string sqlString =
                "SELECT COUNT(*) AS COUNT " +
                " FROM " + DB.VSValue.GetNameAndAlias() +
                " JOIN " + DB.ValueSet.GetNameAndAlias() +
                " ON " + DB.ValueSet.Alias + "." + DB.VSValue.ValueSetCol.PureColumnName() + " = " + DB.VSValue.ValueSetCol.Id() +   ////Bug?:ValueSet og  VSValue  ON " + DB.ValueSet.Alias + "." + DB.VSValue.ValueSet + " = " 
                " WHERE " + DB.ValueSet.ValueSetCol.Is();

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = DB.ValueSet.ValueSetCol.GetStringParameter(aValueSet);
            DataSet mMetaSet = mSqlCommand.ExecuteSelect(sqlString, parameters);
            return int.Parse((mMetaSet.Tables[0].Rows[0]["COUNT"]).ToString());
        }

        
        /// <summary>
        /// Logs a line if Database and configfile disagrees on CNMM version 
        /// </summary>
        private void CompareCNMMVersionSources()
        {
            string versionFromConfig = DB.MetaModel;


            MetabaseInfoRow tmp2 = GetMetabaseInfoRow(DB.Keywords.Macro);

            string versionFromDb = tmp2.ModelVersion;
            if (versionFromConfig != versionFromDb)
            {
                string message = "Meta version is " + versionFromDb + " in database but " +
                    versionFromConfig + " in config file. They must be equal";
                log.Warn(message);
            }
            
        }

     


        //flyttet over fra fra_master2MetaQuery. Denne tabellen er ond

        //This assumes that there is just one row which has the given aTextType(is not a Prim.Key)
        // and that the (int aTextCatalogNo) (which is the PK) will be used when this is not the case
        public TextCatalogRow GetTextCatalogRow()
        {
            string aTextType = DB.Keywords.ContentVariable;
           
            string sqlString = GetTextCatalog_SQLString_NoWhere();

            sqlString += " WHERE " + DB.TextCatalog.TextTypeCol.IsUppered();

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];

            parameters[0] = DB.TextCatalog.TextTypeCol.GetStringParameter(aTextType);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);

            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36, " TextType = " + aTextType);
            }

            TextCatalogRow myOut = new TextCatalogRow(myRows[0], DB, mLanguageCodes);
            return myOut;
        }

        //flyttet over fra fra_master2MetaQuery. Denne tabellen er ond
        public TextCatalogRow GetTextCatalogRow(int aTextCatalogNo)
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = GetTextCatalog_SQLString_NoWhere();

            sqlString += " WHERE " + DB.TextCatalog.TextCatalogNoCol.Is();

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = DB.TextCatalog.TextCatalogNoCol.GetStringParameter(aTextCatalogNo.ToString());


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36, " TextCatalogNo = " + aTextCatalogNo);
            }

            TextCatalogRow myOut = new TextCatalogRow(myRows[0], DB, mLanguageCodes);
            return myOut;
        }


        //     public List<RelevantFootNotesRow> GetRelevantFoonotes(string mainTable)
        //     {
        //         string sqlString = "select bx.MainTable, bx.FootNoteNo, bx.FootNoteType, bx.Contents, bx.Variable, bx.ValuePool, bx.ValueCode," +
        //        "bx.TimePeriod, bx.Subtable," + DB.Footnote.Alias + "." + DB.Footnote.MandOpt + " as MandOpt," + DB.Footnote.Alias + "." + DB.Footnote.ShowFootnote + " as ShowFootNotes," +
        //        DB.Footnote.Alias + "." + DB.Footnote.FootnoteText + " as " + DB.Footnote.Alias + "_" + DB.Footnote.FootnoteText;
        //         foreach (String langCode in mLanguageCodes)
        //         {
        //             if (DB.isSecondaryLanguage(langCode))
        //             {
        //                 sqlString += ", CASE WHEN " + DB.FootnoteLang2.Alias + DB.GetMetaSuffix(langCode) + "." + DB.FootnoteLang2.FootnoteText + " IS NOT NULL THEN " +
        //                     DB.FootnoteLang2.Alias + DB.GetMetaSuffix(langCode) + "." + DB.FootnoteLang2.FootnoteText +
        //                     " ELSE " + DB.Footnote.Alias + "." + DB.Footnote.FootnoteText + " END " +
        //                     " AS " + DB.FootnoteLang2.Alias + DB.GetMetaSuffix(langCode) + "_" + DB.FootnoteLang2.FootnoteText;
        //             }
        //         }
        // sqlString += " from ( " +
        //   "(select fi." + DB.Contents.MainTable + " as MainTable , fi." + DB.FootnoteContents.FootnoteNo + " as FootNoteNo, '2' as FootNoteType, fi." + DB.FootnoteContents.Contents + " as Contents, '*' as Variable, '*' as ValuePool, '*' as Valuecode," +
        //           "'*' as TimePeriod, '*' as Subtable " +
        //    "from " + Md + DB.FootnoteContents.TableName + " fi) " +
        // "union " +
        //   "(select fi." + DB.FootnoteContVbl.MainTable + " as MainTable, fi." + DB.FootnoteContVbl.FootnoteNo + " as  FootNoteNo, '3' as FootNoteType, fi." + DB.FootnoteContVbl.Contents + " as  Contents, fi." + DB.FootnoteContVbl.Variable + " as  Variable, '*' as ValuePool, '*' as Valuecode, " +
        //           "'*' as TimePeriod, '*' as Subtable " +
        //    "from " + Md + DB.FootnoteContVbl.TableName + " fi) " +
        // "union " +
        //   "(select fi." + DB.FootnoteContValue.MainTable + ", fi." + DB.FootnoteContValue.FootnoteNo + ", '4' as FootNoteType, fi." + DB.FootnoteContValue.Contents + " as Contents, fi." + DB.FootnoteContValue.Variable + " as Variable, fi." + DB.FootnoteContValue.ValuePool + " as ValuePool, fi." + DB.FootnoteContValue.ValueCode + " as ValueCode, " +
        //           "'*' as TimePeriod, '*' as SubTable " +
        //    "from " + Md + DB.FootnoteContValue.TableName + " fi " +
        //    "where  NVL(fi." + DB.FootnoteContValue.Cellnote + ", 'XX') <> '" + DB.Codes.Yes + "') " +
        // "union " +
        //   "(select fi." + DB.FootnoteContTime.MainTable + " as MainTable,fi." + DB.FootnoteContTime.FootnoteNo + " as FootNoteNo, '41' as FootNoteType, fi." + DB.FootnoteContTime.Contents + " as Contents, '*' as Variable, '*' as ValuePool, '*' as ValueCode," +
        //           "fi." + DB.FootnoteContTime.TimePeriod + " as TimePeriod, '*' as SubTable " +
        //    "from " + Md + DB.FootnoteContTime.TableName + " fi " +
        //    "where  NVL(fi." + DB.FootnoteContTime.Cellnote + ", 'XX') <> '" + DB.Codes.Yes + "') " +
        // "union " +
        //   "(select fih." + DB.FootnoteContValue.MainTable + ", fih." + DB.FootnoteContValue.FootnoteNo + " as FootNoteNo, '999' as FootNoteType, fih." + DB.FootnoteContValue.Contents + " as Contents, fih." + DB.FootnoteContValue.Variable + " Variable, fih." + DB.FootnoteContValue.ValuePool + " as ValuePool, fih." + DB.FootnoteContValue.ValueCode + " as ValueCode, " +
        //           "fit." + DB.FootnoteContTime.TimePeriod + " as TimePeriod, '*' as SubTable " +
        //    "from " + Md + DB.FootnoteContValue.TableName + " fih, " + Md + DB.FootnoteContTime.TableName + " fit " +
        //    "where  fih." + DB.FootnoteContValue.Cellnote + " = '" + DB.Codes.Yes + "' " +
        //      "and  fit." + DB.FootnoteContTime.Cellnote + " = '" + DB.Codes.Yes + "' " +
        //      "and  fit." + DB.FootnoteContTime.FootnoteNo + " = fih." + DB.FootnoteContValue.FootnoteNo + ") " +
        // "union " +
        //   "(select distinct dvt." + DB.SubTableVariable.MainTable + " as MainTable, fi." + DB.FootnoteVariable.FootnoteNo + " as  FootNoteNo, '5' as FootNoteType, '*' as Contents, fi." + DB.FootnoteContVbl.Variable + " as Variable, '*' as ValuePool, '*' as ValueCode, " +
        //           "'*' as TimePeriod, '*' as SubTable " +
        //    "from  " + Md + DB.FootnoteVariable.TableName + " fi," + Md + DB.SubTableVariable.TableName + " dvt " +
        //    "where  dvt." + DB.SubTableVariable.Variable + "= fi." + DB.FootnoteVariable.Variable + ") " +
        // "union " +
        //   "(select distinct dvt." + DB.SubTableVariable.MainTable + " as MainTable, fi." + DB.FootnoteValue.FootnoteNo + " as  FootNoteNo, '6' as FootNoteType, '*' as Contents, dvt." + DB.SubTableVariable.Variable + " as Variable, fi." + DB.FootnoteValue.ValuePool + " as ValuePool, fi." + DB.FootnoteValue.ValueCode + " as ValueCode, " +
        //           "'*' as TimePeriod, '*' as SubTable " +
        //    "from " + Md + DB.FootnoteValue.TableName + " fi, " + Md + DB.SubTableVariable.TableName + " dvt, " + Md + DB.VSValue.TableName + " vmv " +
        //    "where vmv." + DB.ValueSet.TableName + " = dvt. " + DB.ValueSet.ValueSet +
        //      " and  vmv." + DB.VSValue.ValuePool + " = fi." + DB.FootnoteValue.ValuePool +
        //      " and  vmv." + DB.VSValue.ValueCode + " = fi." + DB.FootnoteValue.ValueCode + ") " +
        // "union " +
        //   "(select fi." + DB.FootnoteMainTable.MainTable + " as MainTable, fi." + DB.FootnoteMainTable.FootnoteNo + " as FootNoteNo , '7' as FootNoteType, '*' as Contents, '*' as Variable, '*' as ValuePool, '*' as ValueCode," +
        //           "'*' as TimeStamp , '*' as SubTable " +
        //    "from " + Md + DB.FootnoteMainTable.TableName + " fi) " +
        // "union " +
        //   "(select fi." + DB.FootnoteSubTable.MainTable + " as MainTable, " + DB.FootnoteSubTable.FootnoteNo + " as FootNote, '8' as FootNoteType, '*' as Contents, '*' as Variable, '*' as ValuePool, '*' as ValueCode," +
        //           "'*' as TimePeriod, " + "fi." + DB.FootnoteSubTable.SubTable + " as SubTable " +
        //    "from " + Md + DB.FootnoteSubTable.TableName + " fi) " +
        // "union " +
        //   "(select distinct fi." + DB.FootnoteMaintValue.MainTable + " as MainTable, fi." + DB.FootnoteMaintValue.FootnoteNo + " as FootNoteNo, '9' as FootNoteType, '*' as Contents, dvt." + DB.SubTableVariable.Variable + ", fi." + DB.FootnoteMaintValue.ValuePool + " as ValuePool , fi." + DB.FootnoteMaintValue.ValueCode + " as ValueCode," +
        //           "'*' as TimePeriod, '*' as SubTable " +
        //    "from " + Md + DB.FootnoteMaintValue.TableName + " fi, " + Md + DB.SubTableVariable.TableName + " dvt," + Md + DB.VSValue.TableName + " vmv " +
        //    "where  dvt." + DB.SubTableVariable.Variable + "= fi." + DB.FootnoteMaintValue.Variable +
        //      " and  vmv." + DB.VSValue.ValueSet + " = dvt." + DB.SubTableVariable.ValueSet +
        //      " and  vmv." + DB.VSValue.ValuePool + " = fi." + DB.FootnoteMaintValue.ValuePool +
        //      " and  vmv." + DB.VSValue.ValueCode + " = fi." + DB.FootnoteMaintValue.ValueCode + ")" +
        // ") bx," +
        //  Md + DB.Footnote.TableName + " " + DB.Footnote.Alias + " ";
        //foreach (String langCode in mLanguageCodes)
        //    {
        //       if (DB.isSecondaryLanguage(langCode))
        //          {
        //              sqlString += " LEFT JOIN " + DB.MetaOwner + DB.FootnoteLang2.TableName + DB.GetMetaSuffix(langCode) + " " + DB.FootnoteLang2.Alias + DB.GetMetaSuffix(langCode);
        //              sqlString += " ON " + DB.Footnote.Alias + "." + DB.Footnote.FootnoteNo + " = " + DB.FootnoteLang2.Alias + DB.GetMetaSuffix(langCode) + "." + DB.FootnoteLang2.FootnoteNo;
        //           }
        //      }


        //sqlString += " where bx.MainTable = '" + mainTable + "' " +
        //   "and "+ DB.Footnote.Alias + "." + DB.Footnote.FootnoteNo + " = bx.FootNoteNo " +
        // "order by FootNoteType, Contents, Variable, ValuePool, ValueCode, Timeperiod, SubTable, FootNoteNo";

        //         DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
        //         DataRowCollection myRows = ds.Tables[0].Rows;
        //         List<RelevantFootNotesRow> myOut = new List<RelevantFootNotesRow>();
        //         foreach (DataRow myRow in myRows)
        //         {
        //             myOut.Add(new RelevantFootNotesRow(myRow,mDbConfig,mLanguageCodes,mMetaModelVersion));
        //         }
        //         return myOut;
        //     }

        public List<RelevantFootNotesRow> GetRelevantFoonotes(string mainTable)
        {
            string sqlString = "select bx.MainTable, bx.FootNoteNo, bx.FootNoteType, bx.Contents, bx.Variable, bx.ValuePool, bx.ValueCode," +
           "bx.TimePeriod, bx.Subtable," + DB.Footnote.MandOptCol.Id() + " as MandOpt," + DB.Footnote.ShowFootnoteCol.Id() + " as ShowFootNotes," +
            DB.Footnote.PresCharacterCol.Id() + " as PresCharacter," +
           DB.Footnote.FootnoteTextCol.ForSelect() ;
            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.FootnoteLang2.FootnoteTextCol.ForSelectWithFallback(langCode, DB.Footnote.FootnoteTextCol);
                }
            }
            sqlString += " from ( " +
              "(select fi." + DB.FootnoteContents.MainTableCol.PureColumnName() + " as MainTable , fi." + DB.FootnoteContents.FootnoteNoCol.PureColumnName() + " as FootNoteNo, '2' as FootNoteType, fi." + DB.FootnoteContents.ContentsCol.PureColumnName() + " as Contents, '*' as Variable, '*' as ValuePool, '*' as Valuecode," +
                      "'*' as TimePeriod, '*' as Subtable " +
               "from " + Md + DB.FootnoteContents.TableName + " fi) " +
            "union " +
              "(select fi." + DB.FootnoteContVbl.MainTableCol.PureColumnName() + " as MainTable, fi." + DB.FootnoteContVbl.FootnoteNoCol.PureColumnName() + " as  FootNoteNo, '3' as FootNoteType, fi." + DB.FootnoteContVbl.ContentsCol.PureColumnName() + " as  Contents, fi." + DB.FootnoteContVbl.VariableCol.PureColumnName() + " as  Variable, '*' as ValuePool, '*' as Valuecode, " +
                      "'*' as TimePeriod, '*' as Subtable " +
               "from " + Md + DB.FootnoteContVbl.TableName + " fi) " +
           // "union " +
           //   "(select fi." + DB.FootnoteContValue.MainTableCol.PureColumnName() + ", fi." + DB.FootnoteContValue.FootnoteNoCol.PureColumnName() + ", '4' as FootNoteType, fi." + DB.FootnoteContValue.ContentsCol.PureColumnName() + " as Contents, fi." + DB.FootnoteContValue.VariableCol.PureColumnName() + " as Variable, fi." + DB.FootnoteContValue.ValuePoolCol.PureColumnName() + " as ValuePool, fi." + DB.FootnoteContValue.ValueCodeCol.PureColumnName() + " as ValueCode, " +
           //           "'*' as TimePeriod, '*' as SubTable " +
           //    "from " + Md + DB.FootnoteContValue.TableName + " fi " +
           //    "where  COALESCE(fi." + DB.FootnoteContValue.CellnoteCol.PureColumnName() + ", 'XX') <> '" + DB.Codes.Yes + "') " +
           // "union " +
           //   "(select fi." + DB.FootnoteContTime.MainTableCol.PureColumnName() + " as MainTable,fi." + DB.FootnoteContTime.FootnoteNoCol.PureColumnName() + " as FootNoteNo, '41' as FootNoteType, fi." + DB.FootnoteContTime.ContentsCol.PureColumnName() + " as Contents, '*' as Variable, '*' as ValuePool, '*' as ValueCode," +
           //           "fi." + DB.FootnoteContTime.TimePeriodCol.PureColumnName() + " as TimePeriod, '*' as SubTable " +
           //    "from " + Md + DB.FootnoteContTime.TableName + " fi " +
           //    "where  COALESCE(fi." + DB.FootnoteContTime.CellnoteCol.PureColumnName() + ", 'XX') <> '" + DB.Codes.Yes + "') " +
           // "union " +
           //   "(select fih." + DB.FootnoteContValue.MainTableCol.PureColumnName() + ", fih." + DB.FootnoteContValue.FootnoteNoCol.PureColumnName() + " as FootNoteNo, '999' as FootNoteType, fih." + DB.FootnoteContValue.ContentsCol.PureColumnName() + " as Contents, fih." + DB.FootnoteContValue.VariableCol.PureColumnName() + " Variable, fih." + DB.FootnoteContValue.ValuePoolCol.PureColumnName() + " as ValuePool, fih." + DB.FootnoteContValue.ValueCodeCol.PureColumnName() + " as ValueCode, " +
           //           "fit." + DB.FootnoteContTime.TimePeriodCol.PureColumnName() + " as TimePeriod, '*' as SubTable " +
           //    "from " + Md + DB.FootnoteContValue.TableName + " fih, " + Md + DB.FootnoteContTime.TableName + " fit " +
           //    "where  fih." + DB.FootnoteContValue.CellnoteCol.PureColumnName() + " = '" + DB.Codes.Yes + "' " +
           //      "and  fit." + DB.FootnoteContTime.CellnoteCol.PureColumnName() + " = '" + DB.Codes.Yes + "' " +
           //      "and  fit." + DB.FootnoteContTime.FootnoteNoCol.PureColumnName() + " = fih." + DB.FootnoteContValue.FootnoteNoCol.PureColumnName() + ") " +
            "union " +
              "(select distinct dvt." + DB.SubTableVariable.MainTableCol.PureColumnName() + " as MainTable, fi." + DB.FootnoteVariable.FootnoteNoCol.PureColumnName() + " as  FootNoteNo, '5' as FootNoteType, '*' as Contents, fi." + DB.FootnoteContVbl.VariableCol.PureColumnName() + " as Variable, '*' as ValuePool, '*' as ValueCode, " +
                      "'*' as TimePeriod, '*' as SubTable " +
               "from  " + Md + DB.FootnoteVariable.TableName + " fi," + Md + DB.SubTableVariable.TableName + " dvt " +
               "where  dvt." + DB.SubTableVariable.VariableCol.PureColumnName() + "= fi." + DB.FootnoteVariable.VariableCol.PureColumnName() + ") " +
            "union " +
              "(select distinct dvt." + DB.SubTableVariable.MainTableCol.PureColumnName() + " as MainTable, fi." + DB.FootnoteValue.FootnoteNoCol.PureColumnName() + " as  FootNoteNo, '6' as FootNoteType, '*' as Contents, dvt." + DB.SubTableVariable.VariableCol.PureColumnName() + " as Variable, fi." + DB.FootnoteValue.ValuePoolCol.PureColumnName() + " as ValuePool, fi." + DB.FootnoteValue.ValueCodeCol.PureColumnName() + " as ValueCode, " +
                      "'*' as TimePeriod, '*' as SubTable " +
               "from " + Md + DB.FootnoteValue.TableName + " fi, " + Md + DB.SubTableVariable.TableName + " dvt, " + Md + DB.VSValue.TableName + " vmv " +
               "where vmv." + DB.VSValue.ValueSetCol.PureColumnName() + " = dvt. " + DB.ValueSet.ValueSetCol.PureColumnName() +
                 " and  vmv." + DB.VSValue.ValuePoolCol.PureColumnName() + " = fi." + DB.FootnoteValue.ValuePoolCol.PureColumnName() +
                 " and  vmv." + DB.VSValue.ValueCodeCol.PureColumnName() + " = fi." + DB.FootnoteValue.ValueCodeCol.PureColumnName() + ") " +
            "union " +
              "(select fi." + DB.FootnoteMainTable.MainTableCol.PureColumnName() + " as MainTable, fi." + DB.FootnoteMainTable.FootnoteNoCol.PureColumnName() + " as FootNoteNo , '7' as FootNoteType, '*' as Contents, '*' as Variable, '*' as ValuePool, '*' as ValueCode," +
                      "'*' as TimeStamp , '*' as SubTable " +
               "from " + Md + DB.FootnoteMainTable.TableName + " fi) " +
            "union " +
              "(select fi." + DB.FootnoteSubTable.MainTableCol.PureColumnName() + " as MainTable, " + DB.FootnoteSubTable.FootnoteNoCol.PureColumnName() + " as FootNote, '8' as FootNoteType, '*' as Contents, '*' as Variable, '*' as ValuePool, '*' as ValueCode," +
                      "'*' as TimePeriod, " + "fi." + DB.FootnoteSubTable.SubTableCol.PureColumnName() + " as SubTable " +
               "from " + Md + DB.FootnoteSubTable.TableName + " fi) ";
      
                sqlString += "union " +
                  "(select distinct fi." + DB.FootnoteMaintValue.MainTableCol.PureColumnName() + " as MainTable, fi." + DB.FootnoteMaintValue.FootnoteNoCol.PureColumnName() + " as FootNoteNo, '9' as FootNoteType, '*' as Contents, dvt." + DB.SubTableVariable.VariableCol.PureColumnName() + ", fi." + DB.FootnoteMaintValue.ValuePoolCol.PureColumnName() + " as ValuePool , fi." + DB.FootnoteMaintValue.ValueCodeCol.PureColumnName() + " as ValueCode," +
                          "'*' as TimePeriod, '*' as SubTable " +
                   "from " + Md + DB.FootnoteMaintValue.TableName + " fi, " + Md + DB.SubTableVariable.TableName + " dvt," + Md + DB.VSValue.TableName + " vmv " +
                   "where  dvt." + DB.SubTableVariable.VariableCol.PureColumnName() + "= fi." + DB.FootnoteMaintValue.VariableCol.PureColumnName() +
                     " and  vmv." + DB.VSValue.ValueSetCol.PureColumnName() + " = dvt." + DB.SubTableVariable.ValueSetCol.PureColumnName() +
                     " and  vmv." + DB.VSValue.ValuePoolCol.PureColumnName() + " = fi." + DB.FootnoteMaintValue.ValuePoolCol.PureColumnName() +
                     " and  vmv." + DB.VSValue.ValueCodeCol.PureColumnName() + " = fi." + DB.FootnoteMaintValue.ValueCodeCol.PureColumnName() + ")";
         
            sqlString += ") bx," +
             DB.Footnote.GetNameAndAlias();
            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN " + DB.FootnoteLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Footnote.FootnoteNoCol.Is(DB.FootnoteLang2.FootnoteNoCol, langCode);
                }
            }


            sqlString += " where bx.MainTable = " + mSqlCommand.GetParameterRef("aMainTable") + " " +
               "and " + DB.Footnote.FootnoteNoCol.Id() + " = bx.FootNoteNo " +
            "order by bx.FootNoteType, bx.Contents, bx.Variable, bx.ValuePool, bx.ValueCode, bx.Timeperiod, bx.SubTable, bx.FootNoteNo";


            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];

            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", mainTable);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            DataRowCollection myRows = ds.Tables[0].Rows;
            List<RelevantFootNotesRow> myOut = new List<RelevantFootNotesRow>();
            foreach (DataRow myRow in myRows)
            {
                myOut.Add(new RelevantFootNotesRow(myRow, mDbConfig, mLanguageCodes));
            }
            return myOut;
        }

        //TODO; bytt VSGroupRow i MetaQuery

        ////returns the single "row" found when all PKs are spesified
        //public List<GroupingRow> GetRelevantGroupingRows(StringCollection valueSets)
        //{
        //    string valuesetString = this.prepareForInClause(valueSets);

        //    string sqlString = "SELECT DISTINCT ";

        //    sqlString +=
        //        DB.Grouping.ValuePoolCol.ForSelect() + ", " +
        //        DB.Grouping.GroupingCol.ForSelect() + ", " +
        //        DB.Grouping.PresTextCol.ForSelect() + ", " +
        //        DB.Grouping.DescriptionCol.ForSelect() + ", " +
        //        DB.Grouping.GroupPresCol.ForSelect() + ", " +
        //        DB.Grouping.GeoAreaNoCol.ForSelect() + ", " +
        //        DB.Grouping.KDBidCol.ForSelect() + ", " +
        //        DB.Grouping.SortCodeCol.ForSelect();

        //    bool sortByPrimaryLanguage = false;
        //    foreach (String langCode in mLanguageCodes)
        //    {
        //        if (DB.isSecondaryLanguage(langCode))
        //        {
        //            sqlString += ", " + DB.GroupingLang2.PresTextCol.ForSelectWithFallback(langCode, DB.Grouping.PresTextCol) + " ";
        //            sqlString += ", " + DB.GroupingLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.Grouping.SortCodeCol) + " ";
        //        }
        //        else
        //        {
        //            sortByPrimaryLanguage = true;
        //        }
        //    }

        //    sqlString += " /" + "*** SQLID: GetRelevantGroupingRows(StringCollection valueSets) ***" + "/ ";
        //    sqlString += " FROM " + DB.Grouping.GetNameAndAlias();
        //    sqlString += " INNER JOIN " + DB.VSGroup.GetNameAndAlias();
        //    sqlString += " ON " + DB.Grouping.GroupingCol.Is(DB.VSGroup.GroupingCol);
        //    sqlString += " AND " + DB.Grouping.ValuePoolCol.Is(DB.VSGroup.ValuePoolCol);

        //    foreach (String langCode in mLanguageCodes)
        //    {
        //        if (DB.isSecondaryLanguage(langCode))
        //        {
        //            sqlString += " LEFT JOIN " + DB.GroupingLang2.GetNameAndAlias(langCode);
        //            sqlString += " ON " + DB.Grouping.GroupingCol.Is(DB.GroupingLang2.GroupingCol, langCode) +
        //                         " AND " + DB.Grouping.ValuePoolCol.Is(DB.GroupingLang2.ValuePoolCol, langCode);
        //        }
        //    }
        //    sqlString += " WHERE " + DB.VSGroup.ValueSetCol.Id() + " IN  " + valuesetString;

        //    sqlString += " ORDER BY ";
        //    if (sortByPrimaryLanguage)
        //    {
        //        sqlString += DB.Grouping.SortCodeCol.Label() + "," + DB.Grouping.PresTextCol.Label();
        //    }
        //    else
        //    {
        //        sqlString += DB.GroupingLang2.SortCodeCol.Label(mLanguageCodes[0]) + "," + DB.GroupingLang2.PresTextCol.Label(mLanguageCodes[0]); ;
        //    }

        //    DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
        //    DataRowCollection myRows = ds.Tables[0].Rows;
        //    //if (myRows.Count < 1)
        //    //{
        //    //    throw new PCAxis.Sql.Exceptions.DbException(36, " ValuePool = " + aValuePool + " Grouping = " + aGrouping);
        //    //}

        //    List<GroupingRow> myOut = new List<GroupingRow>();
        //    foreach (DataRow myRow in myRows)
        //    {
        //        myOut.Add(new GroupingRow(myRow, DB, mLanguageCodes, (IMetaVersionComparator)this));
        //    }


        //    return myOut;
        //}





        //public List<VSGroupRow> GetVSGroupRowsSorted(string aValuePool, StringCollection aValueSets, string aGrouping, string aSortOrderLanguage)
        //{
        //    string sortOrderLanguage = aSortOrderLanguage;
        //    if (!this.mLanguageCodes.Contains(aSortOrderLanguage))
        //    {
        //        log.Error("requsted sortOrder language not in extaction, using first in list of languages");
        //        sortOrderLanguage = this.mLanguageCodes[0];
        //    }
        //    bool sortOnPrimaryLanguage = !this.mDbConfig.isSecondaryLanguage(sortOrderLanguage);

        //    string sqlString = GetVSGroup_SQLString_NoWhere();


        //    //these Left Join is (only) for sortorder purposes

        //    if (sortOnPrimaryLanguage)
        //    {
        //        sqlString += " LEFT JOIN " + DB.Value.GetNameAndAlias() +
        //                     " ON ( " + DB.VSGroup.ValuePoolCol.Is(DB.Value.ValuePoolCol) +
        //                            " AND " + DB.VSGroup.ValueCodeCol.Is(DB.Value.ValueCodeCol) + " ) ";
        //    }
        //    else
        //    {
        //        sqlString += " LEFT JOIN " + DB.ValueLang2.GetNameAndAlias(sortOrderLanguage) +
        //                   " ON ( " + DB.VSGroup.ValuePoolCol.Id() + " = " +
        //                             DB.ValueLang2.Alias + DB.GetMetaSuffix(sortOrderLanguage) + "." + DB.Value.ValuePool +  //Bug DB.Value.ValuePool skulle vært DB.ValueLang2.ValuePool?
        //                          " AND " + DB.VSGroup.ValueCodeCol.Id() + " = " +
        //                             DB.ValueLang2.Alias + DB.GetMetaSuffix(sortOrderLanguage) + "." + DB.Value.ValueCode + " ) ";  //Bug samme
        //    }

        //    sqlString += " WHERE " + DB.VSGroup.ValueSetCol.In(aValueSets) +
        //                    " AND " + DB.VSGroup.GroupingCol.Is(aGrouping) +
        //                    " AND " + DB.VSGroup.ValuePoolCol.Is(aValuePool);

        //    if (sortOnPrimaryLanguage)
        //    {
        //        sqlString += " ORDER BY " + DB.VSGroup.SortCodeCol.Id() + ", " +
        //                                       DB.Value.SortCodeCol.Id() + ", " +
        //                                       DB.VSGroup.GroupCodeCol.Id();
        //    }
        //    else
        //    {
        //        sqlString += " ORDER BY "
        //            + DB.VSGroupLang2.SortCodeCol.Id(sortOrderLanguage) + ", " +
        //              DB.ValueLang2.SortCodeCol.Id(sortOrderLanguage) + ", " +
        //              DB.VSGroup.GroupCodeCol.Id();

        //    }

        //    DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
        //    DataRowCollection myRows = ds.Tables[0].Rows;
        //    if (myRows.Count < 1)
        //    {
        //        string valueSetsString = this.prepareForInClause(aValueSets);
        //        throw new PCAxis.Sql.Exceptions.DbException(36, " ValueSet = " + valueSetsString + " Grouping = " + aGrouping + " ValueCode = " + " ValuePool = " + aValuePool);
        //    }
        //    List<VSGroupRow> myOut = new List<VSGroupRow>();
        //    foreach (DataRow myRow in myRows)
        //    {
        //        myOut.Add(new VSGroupRow(myRow, DB, mLanguageCodes, (IMetaVersionComparator)this));
        //    }
        //    return myOut;
        //}

      
        //public List<VSGroupRow> GetVSGroupRow(string aValuePool, StringCollection aValueSets, string aGrouping, string aGroupCode)
        //{

        //    string sqlString = GetVSGroup_SQLString_NoWhere();

        //    sqlString += " WHERE " + DB.VSGroup.ValueSetCol.In(aValueSets) +
        //                    " AND " + DB.VSGroup.GroupingCol.Is(aGrouping) +
        //                    " AND " + DB.VSGroup.ValuePoolCol.Is(aValuePool) +
        //                    " AND " + DB.VSGroup.GroupCodeCol.Is(aGroupCode) +
        //                    " ORDER BY " + DB.VSGroup.SortCodeCol.Id() + "," + DB.VSGroup.GroupCodeCol.Id();

        //    DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
        //    DataRowCollection myRows = ds.Tables[0].Rows;

        //    // piv could be 0 wich means that aGroupCode is an ordinary valuecode and not a groupcode
        //    //if (myRows.Count < 1)
        //    //{
        //    //    throw new PCAxis.Sql.Exceptions.DbException(36, " ValueSet = " + valueSetsString + " Grouping = " + aGrouping + " ValueCode = " + " ValuePool = " + aValuePool);
        //    //}
        //    List<VSGroupRow> myOut = new List<VSGroupRow>();
        //    foreach (DataRow myRow in myRows)
        //    {
        //        myOut.Add(new VSGroupRow(myRow, DB, mLanguageCodes, (IMetaVersionComparator)this));
        //    }
        //    return myOut;
        //}

        #endregion


  


    } // End class MetaQuery

} // End namespace
