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
using System.Linq;


namespace PCAxis.Sql.QueryLib_21
{
    /// <summary>
    /// "SQL library" with all meta queries.
    /// </summary>
    public partial class MetaQuery : IMetaVersionComparator
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




        private string mMetaModelVersion;
        public string MetaModelVersion { get { return mMetaModelVersion; } }
        // *** Config/language settings from XMLs ***



        private readonly SqlDbConfig_21 mDbConfig;
        public SqlDbConfig_21 DB
        {
            get { return mDbConfig; }
        }
        private readonly InfoForDbConnection mSelectedDbInfo;
        public InfoForDbConnection SelectedDbInfo
        {
            get { return mSelectedDbInfo; }
        }

        public string mMd;
        public string Md
        {
            get { return mMd; }
        }



        #endregion
        
        #region Constructors
        public MetaQuery(SqlDbConfig_21 config, InfoForDbConnection selectedDbInfo)
        {
            this.mDbConfig = config;
            this.mSelectedDbInfo = selectedDbInfo;
            this.mMd = mDbConfig.MetatablesSchema;
            this.mSqlCommand = this.GetPxSqlCommand();
            this.mLanguageCodes = null;
            this.mMetaModelVersion = GetOmforentModelVersion();
        }

        public MetaQuery(SqlDbConfig_21 config, InfoForDbConnection selectedDbInfo, StringCollection LanguageCodes, bool useTempTables)
        {
            this.mDbConfig = config;
            this.mSelectedDbInfo = selectedDbInfo;
            this.mMd = mDbConfig.MetatablesSchema;
            this.mSqlCommand = this.GetPxSqlCommand();
            this.mLanguageCodes = LanguageCodes;
            this.mMetaModelVersion = GetOmforentModelVersion();
        }

        public PxSqlCommand GetPxSqlCommand()
        {
            //InfoForDbConnection dbInfo = mDbConfig.GetInfoForDbConnection();
            InfoForDbConnection dbInfo = SelectedDbInfo;
            return new PxSqlCommandForTempTables(dbInfo.DataBaseType, dbInfo.DataProvider, dbInfo.ConnectionString);
        }


        #endregion

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
            string sqlString;
            if (metaVersionGE("2.1"))
            {
                foreach (String dbLang in dbLanguages)
                {
                    if (DB.isSecondaryLanguage(dbLang))
                    {
                        sqlString = "select " + DB.MainTableLang2.StatusCol.ForSelect(dbLang) + " from " + DB.MainTableLang2.GetNameAndAlias(dbLang);
                        sqlString += " where " + DB.MainTableLang2.MainTableCol.Id(dbLang) + "='" + mainTableId + "'";
                        DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
                        DataRowCollection myRows = ds.Tables[0].Rows;
                        if (myRows.Count == 1 && myRows[0][DB.MainTableLang2.StatusCol.Label(dbLang)].ToString() == DB.Codes.StatusEng)
                        {
                            maintableLanguages.Add(dbLang);
                        }
                    }
                }
            }
            else
            {
                // could only be one secondary language. Find the one which is not primary.
                string secondaryLanguage = null;
                foreach (string dbLang in dbLanguages)
                {
                    if (DB.isSecondaryLanguage(dbLang))
                    {
                        secondaryLanguage = dbLang;
                    }
                }
                sqlString = "select " + DB.MainTable.StatusEngCol.ForSelect() + " from " + DB.MetaOwner + DB.MainTable.GetNameAndAlias();
                sqlString += " where " + DB.MainTable.MainTableCol.Id() + "='" + mainTableId + "'";
                DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
                DataRowCollection myRows = ds.Tables[0].Rows;
                if (myRows.Count == 1 && myRows[0][DB.MainTable.StatusEngCol.Label()].ToString() == DB.Codes.StatusEng)
                {
                    maintableLanguages.Add(secondaryLanguage);
                }
            }
            return maintableLanguages;
        }


        public List<MainTableVariableRow> GetMainTableVariableRows(string aMainTable)
        {
            List<MainTableVariableRow> myOut = new List<MainTableVariableRow>();


            string sqlString = MainTableVariableRow.sqlString(DB, aMainTable);
            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count < 1)
            {
                throw new DbPxsMismatchException(35, " MainTable = " + aMainTable);
            }
            foreach (DataRow sqlRow in myRows)
            {
                MainTableVariableRow outRow = new MainTableVariableRow(sqlRow, DB, mMetaModelVersion);
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
           
            if (metaVersionGE("2.1"))
            {
                sqlString += DB.ValueSet.PresTextCol.ForSelect() + ", ";
            }
            sqlString += DB.ValueSet.DescriptionCol.ForSelect() + ", " +
            DB.ValueSet.EliminationCol.ForSelect() + ", " +
            DB.ValueSet.ValuePoolCol.ForSelect() + ", " +
            DB.ValueSet.ValuePresCol.ForSelect() + ", " +
            DB.ValueSet.GeoAreaNoCol.ForSelect() + ", " +
            DB.ValueSet.KDBIdCol.ForSelect() + ", " +
            DB.ValueSet.SortCodeExistsCol.ForSelect() + ", " +
            DB.ValueSet.FootnoteCol.ForSelect();
            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    //TODO; 2.0 --> DONE
                    if (metaVersionGE("2.1"))
                    {
                        sqlString += ", " + DB.ValueSetLang2.PresTextCol.ForSelectWithFallback(langCode, DB.ValueSet.PresTextCol) + " ";
                        //  <-- TODO; 2.0 DONE
                    }
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

            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is(MainTable) +
                           " AND " + DB.SubTableVariable.VariableCol.Is(Variable);

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            //if (myRows.Count < 1) {
            //    throw new StrangeMetaDataException("No rows for " + " MainTable = " + MainTable + " Variable = " + Variable);
            //} 


            List<ValueSetRow> myOut = new List<ValueSetRow>();
            foreach (DataRow myRow in myRows)
            {
                myOut.Add(new ValueSetRow(myRow, DB, mLanguageCodes, (IMetaVersionComparator)this));
            }
            return myOut;
        }




        public ValueSetRow GetValueSetRow2(string aMainTable, string aSubTable, string aVariable)
        {
            SubTableVariableRow tmp = GetSubTableVariableRow(aMainTable, aSubTable, aVariable);
            return GetValueSetRow(tmp.ValueSet);
        }





        public DataSet GetValueWildCardBySubTable(string mainTable, string variable, string subtable, string wildCard)
        {
            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;
            string sqlWildCard = wildCard.Replace("*", "%").Replace("?", "_");

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
                " WHERE " + DB.SubTableVariable.MainTableCol.Is(mainTable) +
                  " AND " + DB.SubTableVariable.VariableCol.Is(variable);

            if (subtable != null)
            {
                sqlString = sqlString + " AND " + DB.SubTableVariable.SubTableCol.Is(subtable);
            }

            sqlString = sqlString + " AND " + DB.VSValue.ValueCodeCol.Like(sqlWildCard);
            sqlString = sqlString + " ORDER BY " + DB.Value.SortCodeCol.Id();
            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString);
        }


        public ValueRowDictionary GetValueRowDictionary(string mainTable, StringCollection subTables, string variable, StringCollection someValueCodes, string valueExtraExists)
        {

   
            //SqlDbConfig dbconf = DB;  
            // piv added DISTINCT

#if DEBUG
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            logTime.Info("Start " + System.Reflection.MethodBase.GetCurrentMethod().Name + " bad");
#endif

            //TODO: Creating tempTable should be kept for reuse in the datapart
            string myVariableNumber = "007";
            string tempTabId = mSqlCommand.MakeTempTableJustValues(variable, myVariableNumber, DB.Database.Connection.useTemporaryTables, someValueCodes);
            

            string subTablesString = this.prepareForInClause(subTables);

            string sqlString = "SELECT bx." + DB.Value.ValueCode + ", bz." + DB.Value.ValuePool + ", bz." + DB.VSValue.ValueSet
                             + ", bx.Language, bx." + DB.Value.ValueTextL + ", bx." + DB.Value.ValueTextS
                                + ", bx.SortCodeValue, bz.SortCodeVsValue";
            sqlString += " FROM (";
            #region language section
            int LangCounter = 0;
            foreach (String langCode in mLanguageCodes)
            {
                if (LangCounter > 0)
                {
                    sqlString += " UNION ";
                }
                if (!DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " SELECT '" + langCode + "' AS Language"
                               + ", " + DB.Value.ValueCodeCol.Id() + " AS " + DB.Value.ValueCode
                               + ", " + DB.Value.ValuePoolCol.Id() + " AS " + DB.Value.ValuePool;

                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {
                        string[] valueExtraCols = new string[4];
                        valueExtraCols[0] = DB.ValueExtra.ValueTextX1Col.Id();
                        valueExtraCols[1] = DB.ValueExtra.ValueTextX2Col.Id();
                        valueExtraCols[2] = DB.ValueExtra.ValueTextX3Col.Id();
                        valueExtraCols[3] = DB.ValueExtra.ValueTextX4Col.Id();
                        sqlString += ", " + mSqlCommand.getConcatString(valueExtraCols)
                                   + " AS " + DB.Value.ValueTextL
                                   + ", NULL AS " + DB.Value.ValueTextS;
                    }
                    else
                    {
                        sqlString += ", " + DB.Value.ValueTextLCol.Id() + " AS " + DB.Value.ValueTextL
                                   + ", " + DB.Value.ValueTextSCol.Id() + " AS " + DB.Value.ValueTextS;
                    }

                    sqlString += ", " + DB.Value.SortCodeCol.Id() + " AS SortCodeValue"
                               + " FROM " + DB.MetaOwner + DB.Value.TableName + " " + DB.Value.Alias;

                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {
                        sqlString += " JOIN " + DB.ValueExtra.GetNameAndAlias()
                                   + " ON (" + DB.ValueExtra.ValuePoolCol.Is(DB.Value.ValuePoolCol)
                                   + " AND " + DB.ValueExtra.ValueCodeCol.Is(DB.Value.ValueCodeCol) + ")";
                    }
                }
                else
                {
                    sqlString += " SELECT '" + langCode + "' AS Language"
                               + ", " + DB.ValueLang2.ValueCodeCol.Id(langCode) + " AS " + DB.Value.ValueCode
                               + ", " + DB.ValueLang2.ValuePoolCol.Id(langCode) + " AS " + DB.Value.ValuePool;

                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {
                        string[] valueExtraCols = new string[4];
                        valueExtraCols[0] = DB.ValueExtraLang2.ValueTextX1Col.Id(langCode);
                        valueExtraCols[1] = DB.ValueExtraLang2.ValueTextX2Col.Id(langCode);
                        valueExtraCols[2] = DB.ValueExtraLang2.ValueTextX3Col.Id(langCode);
                        valueExtraCols[3] = DB.ValueExtraLang2.ValueTextX4Col.Id(langCode);
                        sqlString += ", " + mSqlCommand.getConcatString(valueExtraCols)
                                   + " AS " + DB.Value.ValueTextL
                                   + ", NULL AS " + DB.Value.ValueTextS;
                    }
                    else
                    {
                        sqlString += ", " + DB.ValueLang2.ValueTextLCol.Id(langCode) + " AS " + DB.Value.ValueTextL
                                   + ", " + DB.ValueLang2.ValueTextSCol.Id(langCode) + " AS " + DB.Value.ValueTextS;
                    }

                    sqlString += ", " + DB.ValueLang2.SortCodeCol.Id(langCode) + " AS SortCodeValue"
                               + " FROM " + DB.ValueLang2.GetNameAndAlias(langCode);

                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {

                        sqlString += " JOIN " + DB.ValueExtraLang2.GetNameAndAlias(langCode)
                                   + " ON (" + DB.ValueExtraLang2.ValuePoolCol.Id(langCode) + " = " + DB.ValueLang2.ValuePoolCol.Id(langCode)
                                   + " AND " + DB.ValueExtraLang2.ValueCodeCol.Id(langCode) + " = " + DB.ValueLang2.ValueCodeCol.Id(langCode) + ")";
                    }
                }
                LangCounter++;
            }
            sqlString += ") bx";
            #endregion language section

            sqlString += " JOIN (SELECT " + DB.VSValue.ValueSet + " AS " + DB.VSValue.ValueSet
                       + ", " + DB.VSValue.ValuePool + " AS " + DB.Value.ValuePool
                       + ", " + DB.VSValue.ValueCode + " AS " + DB.Value.ValueCode
                       + ", " + DB.VSValue.SortCode + " AS SortCodeVsValue"
                       + " FROM " + DB.MetaOwner + DB.VSValue.TableName
                       + ") bz ON (bx." + DB.Value.ValuePool + " = bz." + DB.Value.ValuePool + " AND bx." + DB.Value.ValueCode + " = bz." + DB.Value.ValueCode +  ")";
            sqlString += " JOIN " + DB.MetaOwner + DB.SubTableVariable.TableName + " " + DB.SubTableVariable.Alias
                       + " ON (bz." + DB.VSValue.ValueSet + " = " + DB.SubTableVariable.Alias + "." + DB.SubTableVariable.ValueSet + ")";
            sqlString += " JOIN " + tempTabId
                + " ON  bx." + DB.Value.ValueCode + "="+tempTabId+".Group" + myVariableNumber+" ";
            #region where section
            sqlString += " WHERE " + DB.SubTableVariable.MainTableCol.Is(mainTable);

            // If extraction from only one subtable, this paragraph must be included
            if (subTables != null)
            {
                // IN is used instead of = to make it easier to extend the application to a collection of subtables
                sqlString += " AND " + DB.SubTableVariable.SubTableCol.Id() + " IN " + subTablesString;
            }

            sqlString += " AND " + DB.SubTableVariable.VariableCol.Is(variable);

  

            #endregion where section

            sqlString += " ORDER BY Language, SortCodeVsValue, SortCodeValue, bx." + DB.Value.ValueCode;


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            //DataRowCollection myRows = ds.Tables[0].Rows;
            DataTable myTable = ds.Tables[0];
            //if (myRows.Count != 1)
            //{
            //    throw new PCAxis.Sql.Exceptions.DbException(36," ValuePool = " + aValuePool + " ValueCode = " + aValueCode);
            //}

            //ValueRowDictionary myOut = new ValueRowDictionary(myRows, DB, mMetaModelVersion);
            ValueRowDictionary myOut = new ValueRowDictionary(myTable, DB, mMetaModelVersion);



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
        public ValueRowDictionary GetValueRowDictionary(string mainTable, StringCollection subTables, string variable, string valueExtraExists)
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

            string subTablesString = this.prepareForInClause(subTables);

            //TODO; 2.0 --<            
            //string sqlString = "SELECT " + DB.Value.ValueCode + ", " + DB.Value.ValuePool + ", " + DB.VSValue.ValueSet
            //                 + ", Language, " + DB.Value.ValueTextL + ", " + DB.Value.ValueTextS;
            //sqlString += ", SortCodeValue, SortCodeVsValue ";
            // <--- TODO 2.0
            string sqlString = "SELECT bw." + DB.Value.ValueCode + ", bw." + DB.Value.ValuePool + ", bw." + DB.VSValue.ValueSet
                 + ", bw.Language, bw." + DB.Value.ValueTextL + ", bw." + DB.Value.ValueTextS
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

                sqlString += "SELECT bx." + DB.Value.ValueCode + ", bz." + DB.Value.ValuePool + ", bz." + DB.VSValue.ValueSet
                                 + ", bx.Language, bx." + DB.Value.ValueTextL + ", bx." + DB.Value.ValueTextS
                                 + ", bx.SortCodeValue, bz.SortCodeVsValue FROM (";

                if (!DB.isSecondaryLanguage(langCode))
                {
                    #region primary language
                    sqlString += " SELECT '" + langCode + "' AS Language"
                               + ", " + DB.Value.ValueCodeCol.Id() + " AS " + DB.Value.ValueCode
                               + ", " + DB.Value.ValuePoolCol.Id() + " AS " + DB.Value.ValuePool;

                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {
                        //todo; test new 19.05.2010
                        string[] valueExtraCols = new string[4];
                        valueExtraCols[0] = DB.ValueExtra.ValueTextX1Col.Id();
                        valueExtraCols[1] = DB.ValueExtra.ValueTextX2Col.Id();
                        valueExtraCols[2] = DB.ValueExtra.ValueTextX3Col.Id();
                        valueExtraCols[3] = DB.ValueExtra.ValueTextX4Col.Id();
                        sqlString += ", " + mSqlCommand.getConcatString(valueExtraCols)
                                   + " AS " + DB.Value.ValueTextL
                                   + ", NULL AS " + DB.Value.ValueTextS;
                    }
                    else
                    {
                        sqlString += ", " + DB.Value.ValueTextLCol.Id() + " AS " + DB.Value.ValueTextL
                                   + ", " + DB.Value.ValueTextSCol.Id() + " AS " + DB.Value.ValueTextS;
                    }

                    sqlString += ", " + DB.Value.SortCodeCol.Id() + " AS SortCodeValue"
                               + " FROM " + DB.Value.GetNameAndAlias();

                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {
                        sqlString += " JOIN " + DB.ValueExtra.GetNameAndAlias()
                                   + " ON (" + DB.ValueExtra.ValuePoolCol.Is(DB.Value.ValuePoolCol)
                                   + " AND " + DB.ValueExtra.ValueCodeCol.Is(DB.Value.ValueCodeCol) + ")";
                    }
                    #endregion primary language
                }
                else
                {
                    #region secondary language
                    sqlString += " SELECT '" + langCode + "' AS Language"
                               + ", " + DB.ValueLang2.ValueCodeCol.Id(langCode) + " AS " + DB.Value.ValueCode
                               + ", " + DB.ValueLang2.ValuePoolCol.Id(langCode) + " AS " + DB.Value.ValuePool;

                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {
                        //todo; test new 19.05.2010
                        string[] valueExtraCols = new string[4];
                        valueExtraCols[0] = DB.ValueExtraLang2.ValueTextX1Col.Id(langCode);
                        valueExtraCols[1] = DB.ValueExtraLang2.ValueTextX2Col.Id(langCode);
                        valueExtraCols[2] = DB.ValueExtraLang2.ValueTextX3Col.Id(langCode);
                        valueExtraCols[3] = DB.ValueExtraLang2.ValueTextX4Col.Id(langCode);
                        sqlString += ", " + mSqlCommand.getConcatString(valueExtraCols)
                                   + " AS " + DB.Value.ValueTextL
                                   + ", NULL AS " + DB.Value.ValueTextS;
                    }
                    else
                    {
                        sqlString += ", " + DB.ValueLang2.ValueTextLCol.Id(langCode) + " AS " + DB.Value.ValueTextL
                                   + ", " + DB.ValueLang2.ValueTextSCol.Id(langCode) + " AS " + DB.Value.ValueTextS;
                    }

                    sqlString += ", " + DB.ValueLang2.SortCodeCol.Id(langCode) + " AS SortCodeValue"
                               + " FROM " + DB.ValueLang2.GetNameAndAlias(langCode);

                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {
                        //sqlString += " JOIN " + DB.MetaOwner + DB.ValueExtraLang2.TableName + " " + DB.ValueExtraLang2.Alias
                        sqlString += " JOIN " + DB.ValueExtraLang2.GetNameAndAlias(langCode)
                               + " ON (" + DB.ValueExtraLang2.ValuePoolCol.Id(langCode) + " = " + DB.ValueLang2.ValuePoolCol.Id(langCode)
                               + " AND " + DB.ValueExtraLang2.ValueCodeCol.Id(langCode) + " = " + DB.ValueLang2.ValueCodeCol.Id(langCode) + ")";
                    }
                    #endregion secondary language
                }

                sqlString += ") bx"
                           + " JOIN (SELECT " + DB.VSValue.ValueSet + " AS " + DB.VSValue.ValueSet
                           + ", " + DB.VSValue.ValuePool + " AS " + DB.Value.ValuePool
                           + ", " + DB.VSValue.ValueCode + " AS " + DB.Value.ValueCode
                           + ", " + DB.VSValue.SortCode + " AS SortCodeVsValue"
                           + " FROM " + DB.MetaOwner + DB.VSValue.TableName + ") bz"
                           + " ON (bx." + DB.Value.ValuePool + " = bz." + DB.Value.ValuePool + " AND bx." + DB.Value.ValueCode + " = bz." + DB.Value.ValueCode + ")"
                           + " JOIN " + DB.MetaOwner + DB.SubTableVariable.TableName + " " + DB.SubTableVariable.Alias
                           + " ON (bz." + DB.VSValue.ValueSet + " = " + DB.SubTableVariable.Alias + "." + DB.SubTableVariable.ValueSet + ")"
                           + " WHERE " + DB.SubTableVariable.MainTableCol.Is(mainTable);

                // If extraction from only one subtable, this paragraph must be included
                if (subTablesString != null)
                {
                    sqlString += " AND " + DB.SubTableVariable.Alias + "." + DB.SubTableVariable.SubTable + " IN " + subTablesString;
                }

                sqlString += " AND " + DB.SubTableVariable.VariableCol.Is(variable);

                LangCounter++;
            }
            #endregion language section
            sqlString += " ) bw  ORDER BY Language, SortCodeVsValue, SortCodeValue, " + DB.Value.ValueCode;


#if DEBUG
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            logTime.Info("Start " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif



            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);

            DataTable myTable = ds.Tables[0];

            ValueRowDictionary myOut = new ValueRowDictionary(myTable, DB, mMetaModelVersion);
#if DEBUG
            stopWatch.Stop();
            logTime.InfoFormat(System.Reflection.MethodBase.GetCurrentMethod().Name + " Done in ms = {0}", stopWatch.ElapsedMilliseconds);
#endif
            return myOut;
        }



        // newnewnew
        //flyttet hit fra fra_master2MetaQuery
        public List<ValueRow> GetValueRowsByValuePool(string valuePool, StringCollection someValueCodes, string valueExtraExists)
        {

            string joinString = "";

            string sqlString = "SELECT " + DB.Value.ValuePoolCol.ForSelect() + " ," +
                          DB.Value.ValueCodeCol.ForSelect();
            foreach (String langCode in mLanguageCodes)
            {
                if (!DB.isSecondaryLanguage(langCode))
                {
                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {
                        //todo; test new 19.05.2010
                        string[] valueExtraCols = new string[4];
                        valueExtraCols[0] = DB.ValueExtra.ValueTextX1Col.Id();
                        valueExtraCols[1] = DB.ValueExtra.ValueTextX2Col.Id();
                        valueExtraCols[2] = DB.ValueExtra.ValueTextX3Col.Id();
                        valueExtraCols[3] = DB.ValueExtra.ValueTextX4Col.Id();
                        sqlString += ", " + mSqlCommand.getConcatString(valueExtraCols)
                                       + " AS " + DB.Value.ValueTextLCol.Label()
                                       + ", NULL AS " + DB.Value.ValueTextSCol.Label();

                        joinString += " JOIN " + DB.ValueExtra.GetNameAndAlias()
                           + " ON (" + DB.ValueExtra.ValuePoolCol.Is(DB.Value.ValuePoolCol)
                           + " AND " + DB.ValueExtra.ValueCodeCol.Is(DB.Value.ValueCodeCol) + ") ";

                    }
                    else
                    {
                        sqlString += ", " + DB.Value.ValueTextSCol.ForSelect();
                        sqlString += ", " + DB.Value.ValueTextLCol.ForSelect();
                    }
                    sqlString += ", " + DB.Value.SortCodeCol.ForSelect();
                }
                else
                {

                    joinString += " LEFT JOIN " + DB.ValueLang2.GetNameAndAlias(langCode);
                    joinString += " ON ( " + DB.ValueLang2.ValuePoolCol.Id(langCode) + " = " + DB.Value.ValuePoolCol.Id() +
                                  " AND " + DB.ValueLang2.ValueCodeCol.Id(langCode) + " = " + DB.Value.ValueCodeCol.Id() + ")";

                    if (valueExtraExists == DB.Codes.ValueTextExistsX)
                    {
                        string[] valueExtraCols = new string[4];
                        valueExtraCols[0] = DB.ValueExtraLang2.ValueTextX1Col.Id(langCode);
                        valueExtraCols[1] = DB.ValueExtraLang2.ValueTextX2Col.Id(langCode);
                        valueExtraCols[2] = DB.ValueExtraLang2.ValueTextX3Col.Id(langCode);
                        valueExtraCols[3] = DB.ValueExtraLang2.ValueTextX4Col.Id(langCode);
                        sqlString += ", " + mSqlCommand.getConcatString(valueExtraCols) + " AS " + DB.ValueLang2.ValueTextLCol.Label(langCode)
                                   + ", NULL AS " + DB.ValueLang2.ValueTextSCol.Label(langCode);

                        joinString += " JOIN " + DB.MetaOwner + DB.ValueExtraLang2.TableName + " " + DB.ValueExtraLang2.Alias + DB.GetMetaSuffix(langCode)
                           + " ON (" + DB.ValueExtraLang2.Alias + DB.GetMetaSuffix(langCode) + "." + DB.ValueExtraLang2.ValuePool + " = " + DB.ValueLang2.Alias + DB.GetMetaSuffix(langCode) + "." + DB.ValueLang2.ValuePool
                           + " AND " + DB.ValueExtraLang2.Alias + DB.GetMetaSuffix(langCode) + "." + DB.ValueExtraLang2.ValueCode + " = " + DB.ValueLang2.Alias + DB.GetMetaSuffix(langCode) + "." + DB.ValueLang2.ValueCode + ")";

                    }
                    else
                    {
                        sqlString += ", " + GetColumnSelectionString(DB.ValueLang2.Alias + DB.GetMetaSuffix(langCode), DB.ValueLang2.ValueTextS);
                        sqlString += ", " + GetColumnSelectionString(DB.ValueLang2.Alias + DB.GetMetaSuffix(langCode), DB.ValueLang2.ValueTextL);

                    }

                    sqlString += ", " + GetColumnSelectionString(DB.ValueLang2.Alias + DB.GetMetaSuffix(langCode), DB.ValueLang2.SortCode);
                }
            }

            sqlString += " FROM " + DB.Value.GetNameAndAlias();


            sqlString += joinString;





            sqlString += " WHERE " + DB.Value.ValuePoolCol.Is(valuePool);

            if (someValueCodes.Count > 0)
            {
                sqlString += " AND ((" + DB.Value.ValueCodeCol.Id() + " IN (";
                int NumberOfCodes = 0;
                foreach (string VC in someValueCodes)
                {
                    if (NumberOfCodes >= 250)
                    {
                        sqlString += ")) OR (" + DB.Value.ValueCodeCol.Id() + " IN (";
                        NumberOfCodes = 0;
                    }
                    if (NumberOfCodes > 0)
                    {
                        sqlString += ",";
                    }
                    sqlString += "'" + VC + "'";
                    NumberOfCodes++;
                }
                sqlString += ")))";
            }


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;


            List<ValueRow> myOut = new List<ValueRow>();
            foreach (DataRow myRow in myRows)
            {
                ValueRow tmpRow = new ValueRow(myRow, DB, (IMetaVersionComparator)this, LanguageCodes, 1);

                myOut.Add(tmpRow);
            }

            return myOut;
        }
        //end  newnewnew


        private string GetColumnSelectionString(string columnPrefix, string columnName)
        {
            return columnPrefix + "." + columnName + " AS " + columnPrefix + "_" + columnName;

        }



        public int GetNumberOfValuesInValueSetById(string ValueSet)
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
                " ON " + DB.ValueSet.Alias + "." + DB.VSValue.ValueSet + " = " + DB.VSValue.ValueSetCol.Id() +   ////Bug?:ValueSet og  VSValue  ON " + DB.ValueSet.Alias + "." + DB.VSValue.ValueSet + " = " 
                " WHERE " + DB.ValueSet.ValueSetCol.Is(ValueSet);


            DataSet mMetaSet = mSqlCommand.ExecuteSelect(sqlString);
            return int.Parse((mMetaSet.Tables[0].Rows[0]["COUNT"]).ToString());

        }

        //sjekker at basen og config er enige om versjon og setter den
        private string GetOmforentModelVersion()
        {
            string versionFromConfig = DB.MetaModel;


            MetabaseInfoRow tmp2 = GetMetabaseInfoRow(DB.Keywords.Macro);

            string versionFromDb = tmp2.ModelVersion;
            if (versionFromConfig != versionFromDb)
            {
                string message = "Meta version is " + versionFromDb + " in database but " +
                    versionFromConfig + " in config file. They must be equal";
                log.Warn(message);
                //throw new PCAxis.Sql.Exceptions.DifferenceInMetaVersionException(message);
            }
            return versionFromConfig;
        }

        /// <summary>
        /// To determin if a feature( table/ column) should exists.
        /// </summary>
        /// <param name="featureFromVersion"></param>
        /// <returns>True if metaVersion is Greater than or equal to featureFromVersion</returns>
        public bool metaVersionGE(string featureFromVersion)
        {
            return (String.Compare(this.mMetaModelVersion, featureFromVersion, false, CultureInfo.InvariantCulture) >= 0);
        }

        /// <summary>
        /// To determin if a feature( table/ column) should exists.
        /// </summary>
        /// <param name="featureToVersion"></param>
        /// <returns>True if metaVersion is Less than or equal to featureToVersion</returns>
        public bool metaVersionLE(string featureToVersion)
        {
            return (String.Compare(this.mMetaModelVersion, featureToVersion, false, CultureInfo.InvariantCulture) <= 0);
        }

        //flyttet over fra fra_master2MetaQuery. Denne tabellen er ond

        //This assumes that there is just one row which has the given aTextType(is not a Prim.Key)
        // and that the (int aTextCatalogNo) (which is the PK) will be used when this is not the case
        public TextCatalogRow GetTextCatalogRow(string aTextType)
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = GetTextCatalog_SQLString_NoWhere();

            sqlString += " WHERE " + DB.TextCatalog.TextTypeCol.IsUppered(aTextType);


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36, " TextType = " + aTextType);
            }

            TextCatalogRow myOut = new TextCatalogRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator)this);
            return myOut;
        }

        //flyttet over fra fra_master2MetaQuery. Denne tabellen er ond
        public TextCatalogRow GetTextCatalogRow(int aTextCatalogNo)
        {
            //SqlDbConfig dbconf = DB;   
            string sqlString = GetTextCatalog_SQLString_NoWhere();

            sqlString += " WHERE " + DB.TextCatalog.TextCatalogNoCol.Is(aTextCatalogNo.ToString());


            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count != 1)
            {
                throw new PCAxis.Sql.Exceptions.DbException(36, " TextCatalogNo = " + aTextCatalogNo);
            }

            TextCatalogRow myOut = new TextCatalogRow(myRows[0], DB, mLanguageCodes, (IMetaVersionComparator)this);
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
           DB.Footnote.FootnoteTextCol.ForSelect();
            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.FootnoteLang2.FootnoteTextCol.ForSelectWithFallback(langCode, DB.Footnote.FootnoteTextCol);
                }
            }
            sqlString += " from ( " +
              "(select fi." + DB.FootnoteContents.MainTable + " as MainTable , fi." + DB.FootnoteContents.FootnoteNo + " as FootNoteNo, '2' as FootNoteType, fi." + DB.FootnoteContents.Contents + " as Contents, '*' as Variable, '*' as ValuePool, '*' as Valuecode," +
                      "'*' as TimePeriod, '*' as Subtable " +
               "from " + Md + DB.FootnoteContents.TableName + " fi) " +
            "union " +
              "(select fi." + DB.FootnoteContVbl.MainTable + " as MainTable, fi." + DB.FootnoteContVbl.FootnoteNo + " as  FootNoteNo, '3' as FootNoteType, fi." + DB.FootnoteContVbl.Contents + " as  Contents, fi." + DB.FootnoteContVbl.Variable + " as  Variable, '*' as ValuePool, '*' as Valuecode, " +
                      "'*' as TimePeriod, '*' as Subtable " +
               "from " + Md + DB.FootnoteContVbl.TableName + " fi) " +
            "union " +
              "(select fi." + DB.FootnoteContValue.MainTable + ", fi." + DB.FootnoteContValue.FootnoteNo + ", '4' as FootNoteType, fi." + DB.FootnoteContValue.Contents + " as Contents, fi." + DB.FootnoteContValue.Variable + " as Variable, fi." + DB.FootnoteContValue.ValuePool + " as ValuePool, fi." + DB.FootnoteContValue.ValueCode + " as ValueCode, " +
                      "'*' as TimePeriod, '*' as SubTable " +
               "from " + Md + DB.FootnoteContValue.TableName + " fi " +
               "where  COALESCE(fi." + DB.FootnoteContValue.Cellnote + ", 'XX') <> '" + DB.Codes.Yes + "') " +
            "union " +
              "(select fi." + DB.FootnoteContTime.MainTable + " as MainTable,fi." + DB.FootnoteContTime.FootnoteNo + " as FootNoteNo, '41' as FootNoteType, fi." + DB.FootnoteContTime.Contents + " as Contents, '*' as Variable, '*' as ValuePool, '*' as ValueCode," +
                      "fi." + DB.FootnoteContTime.TimePeriod + " as TimePeriod, '*' as SubTable " +
               "from " + Md + DB.FootnoteContTime.TableName + " fi " +
               "where  COALESCE(fi." + DB.FootnoteContTime.Cellnote + ", 'XX') <> '" + DB.Codes.Yes + "') " +
            "union " +
              "(select fih." + DB.FootnoteContValue.MainTable + ", fih." + DB.FootnoteContValue.FootnoteNo + " as FootNoteNo, '999' as FootNoteType, fih." + DB.FootnoteContValue.Contents + " as Contents, fih." + DB.FootnoteContValue.Variable + " Variable, fih." + DB.FootnoteContValue.ValuePool + " as ValuePool, fih." + DB.FootnoteContValue.ValueCode + " as ValueCode, " +
                      "fit." + DB.FootnoteContTime.TimePeriod + " as TimePeriod, '*' as SubTable " +
               "from " + Md + DB.FootnoteContValue.TableName + " fih, " + Md + DB.FootnoteContTime.TableName + " fit " +
               "where  fih." + DB.FootnoteContValue.Cellnote + " = '" + DB.Codes.Yes + "' " +
                 "and  fit." + DB.FootnoteContTime.Cellnote + " = '" + DB.Codes.Yes + "' " +
                 "and  fit." + DB.FootnoteContTime.FootnoteNo + " = fih." + DB.FootnoteContValue.FootnoteNo + ") " +
            "union " +
              "(select distinct dvt." + DB.SubTableVariable.MainTable + " as MainTable, fi." + DB.FootnoteVariable.FootnoteNo + " as  FootNoteNo, '5' as FootNoteType, '*' as Contents, fi." + DB.FootnoteContVbl.Variable + " as Variable, '*' as ValuePool, '*' as ValueCode, " +
                      "'*' as TimePeriod, '*' as SubTable " +
               "from  " + Md + DB.FootnoteVariable.TableName + " fi," + Md + DB.SubTableVariable.TableName + " dvt " +
               "where  dvt." + DB.SubTableVariable.Variable + "= fi." + DB.FootnoteVariable.Variable + ") " +
            "union " +
              "(select distinct dvt." + DB.SubTableVariable.MainTable + " as MainTable, fi." + DB.FootnoteValue.FootnoteNo + " as  FootNoteNo, '6' as FootNoteType, '*' as Contents, dvt." + DB.SubTableVariable.Variable + " as Variable, fi." + DB.FootnoteValue.ValuePool + " as ValuePool, fi." + DB.FootnoteValue.ValueCode + " as ValueCode, " +
                      "'*' as TimePeriod, '*' as SubTable " +
               "from " + Md + DB.FootnoteValue.TableName + " fi, " + Md + DB.SubTableVariable.TableName + " dvt, " + Md + DB.VSValue.TableName + " vmv " +
               "where vmv." + DB.VSValue.ValueSet + " = dvt. " + DB.ValueSet.ValueSet +
                 " and  vmv." + DB.VSValue.ValuePool + " = fi." + DB.FootnoteValue.ValuePool +
                 " and  vmv." + DB.VSValue.ValueCode + " = fi." + DB.FootnoteValue.ValueCode + ") " +
            "union " +
              "(select fi." + DB.FootnoteMainTable.MainTable + " as MainTable, fi." + DB.FootnoteMainTable.FootnoteNo + " as FootNoteNo , '7' as FootNoteType, '*' as Contents, '*' as Variable, '*' as ValuePool, '*' as ValueCode," +
                      "'*' as TimeStamp , '*' as SubTable " +
               "from " + Md + DB.FootnoteMainTable.TableName + " fi) " +
            "union " +
              "(select fi." + DB.FootnoteSubTable.MainTable + " as MainTable, " + DB.FootnoteSubTable.FootnoteNo + " as FootNote, '8' as FootNoteType, '*' as Contents, '*' as Variable, '*' as ValuePool, '*' as ValueCode," +
                      "'*' as TimePeriod, " + "fi." + DB.FootnoteSubTable.SubTable + " as SubTable " +
               "from " + Md + DB.FootnoteSubTable.TableName + " fi) ";
            if (String.Compare(mMetaModelVersion, "2.0", false, CultureInfo.InvariantCulture) > 0)
            {
                sqlString += "union " +
                  "(select distinct fi." + DB.FootnoteMaintValue.MainTable + " as MainTable, fi." + DB.FootnoteMaintValue.FootnoteNo + " as FootNoteNo, '9' as FootNoteType, '*' as Contents, dvt." + DB.SubTableVariable.Variable + ", fi." + DB.FootnoteMaintValue.ValuePool + " as ValuePool , fi." + DB.FootnoteMaintValue.ValueCode + " as ValueCode," +
                          "'*' as TimePeriod, '*' as SubTable " +
                   "from " + Md + DB.FootnoteMaintValue.TableName + " fi, " + Md + DB.SubTableVariable.TableName + " dvt," + Md + DB.VSValue.TableName + " vmv " +
                   "where  dvt." + DB.SubTableVariable.Variable + "= fi." + DB.FootnoteMaintValue.Variable +
                     " and  vmv." + DB.VSValue.ValueSet + " = dvt." + DB.SubTableVariable.ValueSet +
                     " and  vmv." + DB.VSValue.ValuePool + " = fi." + DB.FootnoteMaintValue.ValuePool +
                     " and  vmv." + DB.VSValue.ValueCode + " = fi." + DB.FootnoteMaintValue.ValueCode + ")";
            }
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


            sqlString += " where bx.MainTable = '" + mainTable + "' " +
               "and " + DB.Footnote.FootnoteNoCol.Id() + " = bx.FootNoteNo " +
             "order by bx.FootNoteType, bx.Contents, bx.Variable, bx.ValuePool, bx.ValueCode, bx.Timeperiod, bx.SubTable, bx.FootNoteNo";

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            List<RelevantFootNotesRow> myOut = new List<RelevantFootNotesRow>();
            foreach (DataRow myRow in myRows)
            {
                myOut.Add(new RelevantFootNotesRow(myRow, mDbConfig, mLanguageCodes, mMetaModelVersion));
            }
            return myOut;
        }
        //returns the single "row" found when all PKs are spesified
        public List<GroupingRow> GetRelevantGroupingRows(StringCollection valueSets)
        {
            string valuesetString = this.prepareForInClause(valueSets);

            string sqlString = "SELECT DISTINCT ";

            sqlString +=
                DB.Grouping.ValuePoolCol.ForSelect() + ", " +
                DB.Grouping.GroupingCol.ForSelect() + ", " +
                DB.Grouping.PresTextCol.ForSelect() + ", " +
                DB.Grouping.DescriptionCol.ForSelect() + ", " +
                DB.Grouping.GroupPresCol.ForSelect() + ", " +
                DB.Grouping.GeoAreaNoCol.ForSelect() + ", " +
                DB.Grouping.KDBidCol.ForSelect() + ", " +
                DB.Grouping.SortCodeCol.ForSelect();

            bool sortByPrimaryLanguage = false;
            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += ", " + DB.GroupingLang2.PresTextCol.ForSelectWithFallback(langCode, DB.Grouping.PresTextCol) + " ";
                    sqlString += ", " + DB.GroupingLang2.SortCodeCol.ForSelectWithFallback(langCode, DB.Grouping.SortCodeCol) + " ";
                }
                else
                {
                    sortByPrimaryLanguage = true;
                }
            }

            sqlString += " /" + "*** SQLID: GetRelevantGroupingRows(StringCollection valueSets) ***" + "/ ";
            sqlString += " FROM " + DB.Grouping.GetNameAndAlias();
            sqlString += " INNER JOIN " + DB.VSGroup.GetNameAndAlias();
            sqlString += " ON " + DB.Grouping.GroupingCol.Is(DB.VSGroup.GroupingCol);
            sqlString += " AND " + DB.Grouping.ValuePoolCol.Is(DB.VSGroup.ValuePoolCol);

            foreach (String langCode in mLanguageCodes)
            {
                if (DB.isSecondaryLanguage(langCode))
                {
                    sqlString += " LEFT JOIN " + DB.GroupingLang2.GetNameAndAlias(langCode);
                    sqlString += " ON " + DB.Grouping.GroupingCol.Is(DB.GroupingLang2.GroupingCol, langCode) +
                                 " AND " + DB.Grouping.ValuePoolCol.Is(DB.GroupingLang2.ValuePoolCol, langCode);
                }
            }
            sqlString += " WHERE " + DB.VSGroup.ValueSetCol.Id() + " IN  " + valuesetString;

            sqlString += " ORDER BY ";
            if (sortByPrimaryLanguage)
            {
                sqlString += DB.Grouping.SortCodeCol.Label() + "," + DB.Grouping.PresTextCol.Label();
            }
            else
            {
                sqlString += DB.GroupingLang2.SortCodeCol.Label(mLanguageCodes[0]) + "," + DB.GroupingLang2.PresTextCol.Label(mLanguageCodes[0]); ;
            }

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            //if (myRows.Count < 1)
            //{
            //    throw new PCAxis.Sql.Exceptions.DbException(36, " ValuePool = " + aValuePool + " Grouping = " + aGrouping);
            //}

            List<GroupingRow> myOut = new List<GroupingRow>();
            foreach (DataRow myRow in myRows)
            {
                myOut.Add(new GroupingRow(myRow, DB, mLanguageCodes, (IMetaVersionComparator)this));
            }


            return myOut;
        }





        public List<VSGroupRow> GetVSGroupRowsSorted(string aValuePool, StringCollection aValueSets, string aGrouping, string aSortOrderLanguage)
        {
            string sortOrderLanguage = aSortOrderLanguage;
            if (!this.mLanguageCodes.Contains(aSortOrderLanguage))
            {
                log.Error("requsted sortOrder language not in extaction, using first in list of languages");
                sortOrderLanguage = this.mLanguageCodes[0];
            }
            bool sortOnPrimaryLanguage = !this.mDbConfig.isSecondaryLanguage(sortOrderLanguage);

            string sqlString = GetVSGroup_SQLString_NoWhere();


            //these Left Join is (only) for sortorder purposes

            if (sortOnPrimaryLanguage)
            {
                sqlString += " LEFT JOIN " + DB.Value.GetNameAndAlias() +
                             " ON ( " + DB.VSGroup.ValuePoolCol.Is(DB.Value.ValuePoolCol) +
                                    " AND " + DB.VSGroup.ValueCodeCol.Is(DB.Value.ValueCodeCol) + " ) ";
            }
            else
            {
                sqlString += " LEFT JOIN " + DB.ValueLang2.GetNameAndAlias(sortOrderLanguage) +
                           " ON ( " + DB.VSGroup.ValuePoolCol.Id() + " = " +
                                     DB.ValueLang2.Alias + DB.GetMetaSuffix(sortOrderLanguage) + "." + DB.Value.ValuePool +  //Bug DB.Value.ValuePool skulle vært DB.ValueLang2.ValuePool?
                                  " AND " + DB.VSGroup.ValueCodeCol.Id() + " = " +
                                     DB.ValueLang2.Alias + DB.GetMetaSuffix(sortOrderLanguage) + "." + DB.Value.ValueCode + " ) ";  //Bug samme
            }

            sqlString += " WHERE " + DB.VSGroup.ValueSetCol.In(aValueSets) +
                            " AND " + DB.VSGroup.GroupingCol.Is(aGrouping) +
                            " AND " + DB.VSGroup.ValuePoolCol.Is(aValuePool);

            if (sortOnPrimaryLanguage)
            {
                sqlString += " ORDER BY " + DB.VSGroup.SortCodeCol.Id() + ", " +
                                            DB.VSGroup.GroupCodeCol.Id() + ", " +
                                               DB.Value.SortCodeCol.Id();
                                               
            }
            else
            {
                sqlString += " ORDER BY "
                    + DB.VSGroupLang2.SortCodeCol.Id(sortOrderLanguage) + ", " +
                      DB.VSGroup.GroupCodeCol.Id() + ", " +
                      DB.ValueLang2.SortCodeCol.Id(sortOrderLanguage); 


            }

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;
            if (myRows.Count < 1)
            {
                string valueSetsString = this.prepareForInClause(aValueSets);
                throw new PCAxis.Sql.Exceptions.DbException(36, " ValueSet = " + valueSetsString + " Grouping = " + aGrouping + " ValueCode = " + " ValuePool = " + aValuePool);
            }
            List<VSGroupRow> myOut = new List<VSGroupRow>();
            foreach (DataRow myRow in myRows)
            {
                myOut.Add(new VSGroupRow(myRow, DB, mLanguageCodes, (IMetaVersionComparator)this));
            }
            return myOut;
        }


        public List<VSGroupRow> GetVSGroupRow(string aValuePool, StringCollection aValueSets, string aGrouping, string aGroupCode)
        {

            string sqlString = GetVSGroup_SQLString_NoWhere();

            sqlString += " WHERE " + DB.VSGroup.ValueSetCol.In(aValueSets) +
                            " AND " + DB.VSGroup.GroupingCol.Is(aGrouping) +
                            " AND " + DB.VSGroup.ValuePoolCol.Is(aValuePool) +
                            " AND " + DB.VSGroup.GroupCodeCol.Is(aGroupCode) +
                            " ORDER BY " + DB.VSGroup.SortCodeCol.Id() + "," + DB.VSGroup.GroupCodeCol.Id();

            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            DataRowCollection myRows = ds.Tables[0].Rows;

            // piv could be 0 wich means that aGroupCode is an ordinary valuecode and not a groupcode
            //if (myRows.Count < 1)
            //{
            //    throw new PCAxis.Sql.Exceptions.DbException(36, " ValueSet = " + valueSetsString + " Grouping = " + aGrouping + " ValueCode = " + " ValuePool = " + aValuePool);
            //}
            List<VSGroupRow> myOut = new List<VSGroupRow>();
            foreach (DataRow myRow in myRows)
            {
                myOut.Add(new VSGroupRow(myRow, DB, mLanguageCodes, (IMetaVersionComparator)this));
            }
            return myOut;
        }

        #endregion


        /// <summary>
        /// quotes, commaseparates and encloses in ( )
        /// </summary>
        /// <param name="stringlist"></param>
        /// <returns></returns>
        private string prepareForInClause(StringCollection stringlist)
        {

            string myOut = "(";
            foreach (string aString in stringlist)
            {
                myOut += "'" + aString + "',";

            }
            myOut = myOut.TrimEnd(',');
            myOut += ")";
            return myOut;
        }


    } // End class MetaQuery

} // End namespace
