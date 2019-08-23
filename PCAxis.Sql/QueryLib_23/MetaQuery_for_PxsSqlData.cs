using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

using PCAxis.Sql.DbConfig;
using PCAxis.Sql.Exceptions;

using System.Collections.Specialized;
using System.Reflection;
using PCAxis.Sql.Parser_23;



namespace PCAxis.Sql.QueryLib_23
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
                " WHERE " + DB.ValueGroup.GroupingCol.Is() +
                  " AND " + DB.SubTableVariable.MainTableCol.Is() +
                  " AND " + DB.SubTableVariable.VariableCol.Is() +
                  " AND " + DB.SubTableVariable.SubTableCol.Is() +
                  " AND " + DB.ValueGroup.GroupCodeCol.Is() +
                  " AND " + DB.ValueGroup.ValueCodeCol.Like();




            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[6];
            parameters[0] = DB.ValueGroup.GroupingCol.GetStringParameter(grouping);
            parameters[1] = DB.SubTableVariable.MainTableCol.GetStringParameter(mainTable);
            parameters[2] = DB.SubTableVariable.VariableCol.GetStringParameter(variable);
            parameters[3] = DB.SubTableVariable.SubTableCol.GetStringParameter(subTable);
            parameters[4] = DB.ValueGroup.GroupCodeCol.GetStringParameter(groupCode);
            parameters[5] = DB.ValueGroup.ValueCodeCol.GetStringParameter(wildCard);

            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString, parameters);
        }




        #endregion wildcards in groupvalues


        #region creating and populating temptables


        public string MakeTempTable(string VariableName, string VariableNumber, IList<PXSqlValue> values)
        {
            if (values.Count < 1)
            {

                throw new BugException("BUG! values.Count < 1");
            }
            StringCollection valueCodes = new StringCollection();
            foreach (PXSqlValue value in values)
            {
                 valueCodes.Add(value.ValueCode);
            }

            string tempTabellId = mSqlCommand.MakeTempTableJustValues(VariableName, VariableNumber, DB.UseTemporaryTables,  valueCodes);

            dropTheseTables.Add(tempTabellId);

            return tempTabellId;
        }


        public string MakeTempTable(string VariableName, string VariableNumber, IList<PXSqlGroup> groups, bool makeGroupFactorCol)
        {
            if (groups.Count < 1)
            {
                
                throw new BugException("BUG! dataCodes.Count < 1");
            }

            StringCollection childCodes = new StringCollection();
            StringCollection parentCodes = new StringCollection();
            List<int> parentCodeCounterList = new List<int>();
            int parentCodeCounter = 0;
            foreach (PXSqlGroup group in groups)
            {
                parentCodeCounter++;   
                foreach (string childCode in group.ChildCodes)
                {
                    childCodes.Add(childCode);
                    parentCodes.Add(group.ParentCode);
                    parentCodeCounterList.Add(parentCodeCounter);
                }
            }

            string tempTabellId = mSqlCommand.MakeTempTable(VariableName, VariableNumber, makeGroupFactorCol, DB.UseTemporaryTables, childCodes, parentCodes, parentCodeCounterList);

            dropTheseTables.Add(tempTabellId);
            return tempTabellId;
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


        public DataRowCollection ExecuteSelect(string sqlString, System.Data.Common.DbParameter[] parameters)
        {
            DataSet ds = mSqlCommand.ExecuteSelect(sqlString, parameters);
            return ds.Tables[0].Rows;
        }



    }
}
