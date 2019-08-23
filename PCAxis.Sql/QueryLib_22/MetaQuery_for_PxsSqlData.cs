using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

using PCAxis.Sql.DbConfig;
using PCAxis.Sql.Exceptions;

using System.Collections.Specialized;
using System.Reflection;
using PCAxis.Sql.Parser_22;



namespace PCAxis.Sql.QueryLib_22
{

    public partial class MetaQuery : IDisposable
    {
        /// <summary>
        /// list of temp tables. Droped at the end of the dataextraction or by dispose
        /// </summary>
        private StringCollection dropTheseTables = new StringCollection();


        //flytt til hoved
        public void Dispose() {
            this.DropTempTables();
            mSqlCommand.Dispose();
        }


        #region wildcards in groupvalues

        public StringCollection GetValueWildCardByGrouping(string mainTable, string variable, string subTable, string grouping, string groupCode, string wildCard) {
            // Resolve wildcards here
            StringCollection myOut = new StringCollection();
            DataSet ds = null;
            if (String.Compare(DB.MetaModel, "2.1", false, System.Globalization.CultureInfo.InvariantCulture) > 0) {
                // Datamodel > 2.1, new table structure
                ds = this.GetValueWildCardByGrouping22(mainTable, variable, subTable, grouping, groupCode, wildCard);
            } else {
                // Datamodel 2.1 or older, the old table structure. Use of Grouping is only valid if a SubTable is selected
                //ds = this.GetValueWildCardByGrouping21(mainTable, variable, subTable, grouping, groupCode, wildCard);
            }
            DataRowCollection mValueInfo = ds.Tables[0].Rows;
            foreach (DataRow row in mValueInfo) {
                throw new NotImplementedException();
                //TODO; tok bort myOut.Add(row[DB.VSGroup.ValueCode].ToString());
             //   myOut.Add(row[DB.VSGroup.ValueCode].ToString());
            }
            return myOut;
        }




        //private DataSet GetValueWildCardByGrouping21(string mainTable, string variable, string subTable, string grouping, string groupCode, string wildCard) {
        //    // To ensure that the correct grouping is used mainTable and variable has to be included
        //    // Get the name of the current method.
        //    string currentMethod = MethodBase.GetCurrentMethod().Name;
        //    string sqlWildCard = wildCard.Replace("*", "%").Replace("?", "_");

        //    // Get the contents-table data.
        //    // Standard (ANSI) SQL for this query.
        //    //
        //    //
        //    // SELECT DISTINCT VVL.ValueCode /*** SQLID: GetValueWildCardByGrouping21_01 ***/
        //    // FROM MetaData.VSGroup VGR
        //    // JOIN MetaData.SubTableVariable STV ON VGR.ValueSet = STV.ValueSet
        //    // WHERE STV.MainTable = '<mainTable>' AND STV.Variable = '<variable>'
        //    //   AND VGR.Grouping = '<grouping>' AND VGR.groupCode = '<groupCode>'
        //    //   AND VGR.ValueCode LIKE '<sqlWildCard>'
        //    //
        //    string sqlString =
        //        "SELECT DISTINCT " + DB.VSGroup.ValueCodeCol.Id() +
        //        "/*** SQLID: " + currentMethod + "_01 ***/ " +
        //        "FROM " + DB.VSGroup.GetNameAndAlias() +
        //        " JOIN " + DB.SubTableVariable.GetNameAndAlias() +
        //          " ON " + DB.VSGroup.ValueSetCol.Is( DB.SubTableVariable.ValueSetCol) +
        //        " WHERE " +  DB.SubTableVariable.MainTableCol.Is(mainTable) +
        //          " AND " + DB.SubTableVariable.VariableCol.Is(variable) +
        //          " AND " + DB.SubTableVariable.SubTableCol.Is(subTable) +
        //          " AND " + DB.VSGroup.GroupingCol.Is(grouping)+
        //          " AND " + DB.VSGroup.GroupCodeCol.Is(groupCode) +
        //          " AND " + DB.VSGroup.ValueCodeCol.Like(wildCard);

        //    // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
        //    return mSqlCommand.ExecuteSelect(sqlString);
        //}



        private DataSet GetValueWildCardByGrouping22(string mainTable, string variable, string subTable, string grouping, string groupCode, string wildCard) {
            // To ensure that the correct grouping is used mainTable and variable has to be included
            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;
            string sqlWildCard = wildCard.Replace("*", "%").Replace("?", "_");

            // Get the contents-table data.
            // Standard (ANSI) SQL for this query.
            //
            //
            // SELECT DISTINCT VVL.ValueCode /*** SQLID: GetValueWildCardByGrouping22_01 ***/
            // FROM MetaData.ValueGroup VGR
            // JOIN MetaData.Grouping GRP ON GRP.Grouping = VGR.Grouping AND VGR.ValuePool = GRP.ValuePool
            // JOIN MetaData.ValueSetGrouping VSG ON VSG.Grouping = GRP.Grouping
            // JOIN MetaData.SubTableVariable STV ON STV.ValueSet = VSG.ValueSet
            // WHERE VGR.Grouping = '<grouping>'
            //   AND STV.MainTable = '<mainTable>' AND STV.Variable = '<variable>' AND STV.SubTable = '<subTable>'
            //   AND VGR.groupCode = '<groupCode>'
            //   AND VGR.ValueCode LIKE '<sqlWildCard>'
            //
            //
            string sqlString =
                "SELECT DISTINCT " + DB.ValueGroup.ValueCodeCol.Id() +  //Bør bruke .ForSelect, men da må mottaker bruke ..Col.Label() 
                "/*** SQLID: " + currentMethod + "_01 ***/ " +
                "FROM " + DB.ValueGroup.GetNameAndAlias() +
                " JOIN " + DB.Grouping.GetNameAndAlias() +
                  " ON " + DB.Grouping.GroupingCol.Is(DB.ValueGroup.GroupingCol) +
                  " AND " + DB.Grouping.ValuePoolCol.Is(DB.ValueGroup.ValuePoolCol) +
                " JOIN " + DB.ValueSetGrouping.GetNameAndAlias() +
                  " ON " + DB.ValueSetGrouping.GroupingCol.Is(DB.Grouping.GroupingCol) +
                " JOIN " + DB.SubTableVariable.GetNameAndAlias() +
                  " ON " + DB.SubTableVariable.ValueSetCol.Is(DB.ValueSetGrouping.ValueSetCol) +
                " WHERE " + DB.ValueGroup.GroupingCol.Is(grouping) +
                  " AND " + DB.SubTableVariable.MainTableCol.Is(mainTable) +
                  " AND " + DB.SubTableVariable.VariableCol.Is(variable) +
                  " AND " + DB.SubTableVariable.SubTableCol.Is(subTable) +
                  " AND " + DB.ValueGroup.GroupCodeCol.Is(groupCode) +
                  " AND " + DB.ValueGroup.ValueCodeCol.Like(wildCard);

            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString);
        }




        #endregion wildcards in groupvalues


        #region creating and populating temptables


        public string MakeTempTable(string VariableName, string VariableNumber, IList<PXSqlValue> values, int numberInBulk)
        {
            bool makeGroupFactorCol = false;
            if (values.Count < 1)
            {

                throw new BugException("BUG! values.Count < 1");
            }
            string tempTabellId = CreateTempTables(VariableName, VariableNumber, makeGroupFactorCol);

            InsertIntoTempTablesBulk(tempTabellId, values, numberInBulk);

            return tempTabellId;
        }


        public string MakeTempTable(string VariableName, string VariableNumber, IList<PXSqlGroup> groups, int numberInBulk, bool makeGroupFactorCol)
        {
            if (groups.Count < 1)
            {
                
                throw new BugException("BUG! dataCodes.Count < 1");
            }
            //if (GroupCodes.Count != groupCodes.Count)
            //{
            //    throw new ApplicationException("BUG! dataCodes.Count != groupCodes.Count");
            //}

            string tempTabellId = CreateTempTables(VariableName, VariableNumber, makeGroupFactorCol);

            InsertIntoTempTablesBulk(tempTabellId, groups, numberInBulk, makeGroupFactorCol);

            if (makeGroupFactorCol) {
                //
                // ORIGINALLY:
                // UPDATE /*** SQLID: MakeTempTable_03 ***/ A317838TEMP1 a1
                // SET groupfactor1 =
                // (SELECT COUNT(*) FROM A317838TEMP1 a2
                // WHERE a2.grupp1 = a1.grupp1)
                //
                // 11.11.2008 ThH: CHANGED TO:
                // UPDATE /*** SQLID: MakeTempTable_03 ***/ A317838TEMP1 a1
                // SET groupfactor1 =
                // (SELECT COUNT(*) FROM A317838TEMP1 a2
                // WHERE a2.grupp1 = a1.grupp1)
                //
                // Changed because of syntax problem in ms sql server
                //string sqlString = 
                //    "UPDATE /*** SQLID: MakeTempTable_03 ***/ " + tempTabellId + " a1 " +
                //    "SET groupfactor" + VariableNumber + " = " +
                //    "(SELECT COUNT(*) FROM " + tempTabellId + " a2 " +
                //    "WHERE a2.group" + VariableNumber + " = a1.group" + VariableNumber + ")";

                string sqlString =
                    "UPDATE /*** SQLID: MakeTempTable_03 ***/ " + tempTabellId + 
                    " SET groupfactor" + VariableNumber + " = " +
                    "(SELECT COUNT(*) FROM " + tempTabellId + " a2 " +
                    "WHERE a2.group" + VariableNumber + " = " + tempTabellId + ".group" + VariableNumber + ")";

                mSqlCommand.ExecuteNonQuery(sqlString);
            }
            
            return tempTabellId;
        }

        private string CreateTempTables(string VariableName, string VariableNumber, bool makeGroupFactorCol) {
            // Get the name of the current method. 
            string currentMethod = MethodBase.GetCurrentMethod().Name;

            //TODO; FIX
            string UniqueNumber = mSqlCommand.getUniqueNumber(7 + VariableNumber.Length);
            string tempTabellId;
            tempTabellId = "A" + UniqueNumber + "_TMP" + VariableNumber;

            if (DB.UseTemporaryTables) {
                tempTabellId = mSqlCommand.getPrefixIndicatingTempTable() + tempTabellId;
            }

            dropTheseTables.Add(tempTabellId);


            //----
            log.Debug("tempTabellId:" + tempTabellId+"        tempTabellId len:" + tempTabellId.Length);
            //
            // ORIGINALLY:
            // CREATE /***SQLID: CreateTempTables_01 ***/ TABLE A317838TEMP1
            // (AVariabel VARCHAR2(20), Group1 VARCHAR2(20), GroupNr1 NUMBER(5,0)
            // , GroupFactor1 NUMBER(5,0)
            // )
            //
            // 11.11.2008 ThH: CHANGED TO:
            // CREATE /***SQLID: CreateTempTables_01 ***/ TABLE A317838TEMP1
            // (AVariabel VARCHAR(20), Group1 VARCHAR(20), GroupNr1 INTEGER
            // , GroupFactor1 INTEGER
            // )
            // if (DB.database.Connection.useTemporaryTables) {
            string sqlString = "CREATE /*** SQLID: " + currentMethod + "_01 ***/";

            if (DB.UseTemporaryTables) {
                    sqlString = sqlString + mSqlCommand.getKeywordAfterCreateIndicatingTempTable();
            }

            sqlString += " TABLE " + tempTabellId;
                
            //TODO; Mixed database/server instance         "(A" + VariableName + " VARCHAR(20), Group" + VariableNumber + " VARCHAR(20), GroupNr" + VariableNumber + " INTEGER";
            //TODO; this should not bed hardcoded -Just for test. Maybe should put element "tempCollation" in sqldbconfig.
            if ((DB.MainLanguage.code == "sv" || DB.Database.id=="FAO") && (DB.Database.Connection.dataProvider.ToString()=="Sql") )
            {
                sqlString += " (A" + VariableName + " VARCHAR(20) collate Finnish_Swedish_CI_AS, Group" + VariableNumber + " VARCHAR(20) collate Finnish_Swedish_CI_AS, GroupNr" + VariableNumber + " INTEGER";
            }
            else
            {
                sqlString += "(A" + VariableName + " VARCHAR(20), Group" + VariableNumber + " VARCHAR(20), GroupNr" + VariableNumber + " INTEGER";
            }
            if (makeGroupFactorCol) {
                sqlString += ", GroupFactor" + VariableNumber + " INTEGER";
            }
            sqlString += ")";

            if (DB.UseTemporaryTables) {
                    sqlString = sqlString + mSqlCommand.getTempTableCreateCommandEndClause();
            }

            mSqlCommand.ExecuteNonQuery(sqlString);
            return tempTabellId;
        }



        private void InsertIntoTempTablesBulk(string tempTabellId, IList<PXSqlValue> values, int numberInBulk)
        {
          
            string currentMethod = MethodBase.GetCurrentMethod().Name;
            StringCollection sqlStrings = new StringCollection();

            
            int ValuesCounter = 0;
           
            foreach (PXSqlValue value in values)
            {

                ValuesCounter++;

                string sqlString = "INSERT INTO /*** SQLID: " + currentMethod + "_01 ***/ " + tempTabellId;
                // inserts the same code in 2 columns, so the table looks the same as for grouping.
                // Perhaps just could
                sqlString += " VALUES ('" + value.ValueCode + "', '" + value.ValueCode + "'," + ValuesCounter;

                sqlString += ")";
                sqlStrings.Add(sqlString);

            }
            mSqlCommand.InsertBulk(sqlStrings, numberInBulk);
        }


        private void InsertIntoTempTablesBulk(string tempTabellId, IList<PXSqlGroup> groups, int numberInBulk, bool makeGroupFactorCol)
        {


            string currentMethod = MethodBase.GetCurrentMethod().Name;
            StringCollection sqlStrings = new StringCollection();

            //int ValuesMax = dataCodes.Count;
            //for (int ValuesCounter = 0; ValuesCounter < ValuesMax; ValuesCounter++)
            //{
            // 
            // INSERT INTO /*** SQLID: InsertIntoTempTablesBulk_01 ***/ A317838TEMP1
            // VALUES ('<dataCodes[ValuesCounter]>', '<groupCodes[ValuesCounter]>', <ValuesCounter+1>
            // ,1
            // )
            //
            int ValuesCounter = 0;
            //foreach (KeyValuePair<string, List<string>> VGroup in GroupCodes) {
            foreach (PXSqlGroup group in groups)
            {

                ValuesCounter++;
                foreach (string childCode in group.ChildCodes)
                {
                    string sqlString = "INSERT INTO /*** SQLID: " + currentMethod + "_01 ***/ " + tempTabellId;

                    sqlString += " VALUES ('" + childCode + "', '" + group.ParentCode + "'," + ValuesCounter;
                    if (makeGroupFactorCol)
                    {
                        sqlString += ",1";
                    }
                    sqlString += ")";
                    sqlStrings.Add(sqlString);
                }
            }
            mSqlCommand.InsertBulk(sqlStrings, numberInBulk);
        }


        #endregion creating and populating temptables

        /// <summary>
        /// Tries to drop the temporary Tables, unless they delete themselves.
        /// Hmm, perhaps auxiliary tables would have been a better word.
        /// Temporary Table in Oracle is a permanent table with temorary rows
        /// </summary>
        public void DropTempTables() {
            
            if (!DB.UseTemporaryTables) {
                foreach (string tableName in dropTheseTables) {
                    try {

                        mSqlCommand.ExecuteNonQuery("DROP TABLE " + tableName);
                    } catch (Exception e) {
                        log.Error("DROP TABLE failed:", e);
                        //don't want to stop because of this... 
                    }
                }
            } else if (mSqlCommand.getProgramMustTruncAndDropTempTable()) {
                foreach (string tableName in dropTheseTables) {
                    try {
                        mSqlCommand.ExecuteNonQuery("TRUNCATE TABLE " + tableName);
                        mSqlCommand.ExecuteNonQuery("DROP TABLE " + tableName);
                    } catch (Exception e) {
                        log.Error("TRUNC or DROP TABLE failed:", e);
                        //don't want to stop because of this... 
                    }
                }
            }
            dropTheseTables = new StringCollection();
        }


        public DataRowCollection ExecuteSelect(string sqlString) {
            DataSet ds = mSqlCommand.ExecuteSelect(sqlString);
            return ds.Tables[0].Rows;
        }



    }
}
