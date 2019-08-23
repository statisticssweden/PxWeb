using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.DbConfig;

using System.Data;
using System.Reflection;

using PCAxis.Sql.Parser_23;

using System.Collections.Specialized;

namespace PCAxis.Sql.QueryLib_23
{
    /// <summary>
    /// "SQL library" with all meta queries.
    /// </summary>
    public partial class MetaQuery
    {
        #region Time stuff

        #region all timevalues

        public PxSqlValues GetAllTimeValuesList(string mainTable, string sortOrder) {
            DataSet ds = this.GetAllTimeValues(mainTable, sortOrder);
            return GetTimeValues(mainTable, ds, true);
        }


        public DataSet GetAllTimeValues(string aMainTable, string aSortOrder)
        {
            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;

            string sortOrder = "asc"; 
            if (! aSortOrder.ToUpper().Equals("ASC") )
            {
                sortOrder = "desc";
            }

            string sqlString =
                "SELECT DISTINCT " + DB.ContentsTime.TimePeriodCol.Id() +
                "/*** SQLID: " + currentMethod + "_01 ***/ " +
                " FROM " + DB.ContentsTime.GetNameAndAlias() +
                " WHERE " + DB.ContentsTime.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable")) +
                " ORDER BY " + DB.ContentsTime.TimePeriodCol.Id() + " " + sortOrder;

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", aMainTable);

            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.

            return mSqlCommand.ExecuteSelect(sqlString, parameters);
        }

        #endregion all timevalues

        #region fixed list of timevalues
        //For mPxsFile.Query.Time.TimeOption == 0
        public  PxSqlValues GetTimeValueList(string mainTable, Dictionary<string, int> mySelectedValues) {

            DataSet ds = this.GetTimeValues(mainTable, mySelectedValues.Keys);
            PxSqlValues myOut = GetTimeValues(mainTable, ds, false);

         
            foreach (KeyValuePair<string, int> time in mySelectedValues) {
                // Check to se if the value exist. If it was specified in the pxs, but is not present in the
                // database it will not exist.  Maybe this check should be replaced by an exception handling
                // due to preformance.
                if (myOut.ContainsKey(time.Key)) {
                    myOut[time.Key].SortCodePxs = time.Value;
                }
            }


            return myOut;
        }

        private DataSet GetTimeValues(string aMainTable, ICollection<string> valuesFromPxs) {

            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;

            // Get the contents-table data.
            // Standard (ANSI) SQL for this query.
            //
            // ORIGINALLY:
            // SELECT DISTINCT CTM.TimePeriod /*** SQLID: GetTimeValues_01 ***/
            // FROM MetaData.ContentsTime CTM
            // WHERE UPPER(MainTable) = UPPER('<mainTable>')
            //   AND UPPER(TimePeriod) IN (SelVal)
            //
            // 11.11.2008 ThH: CHANGED TO:
            // SELECT DISTINCT CTM.TimePeriod /*** SQLID: GetTimeValues_01 ***/
            // FROM MetaData.ContentsTime CTM
            // WHERE CTM.MainTable = '<mainTable>'
            //   AND CTM.TimePeriod IN (SelVal)
            //
            string sqlString =
                "SELECT DISTINCT " + DB.ContentsTime.TimePeriodCol.Id() +
                "/*** SQLID: " + currentMethod + "_01 ***/ " +
                " FROM " + DB.ContentsTime.GetNameAndAlias() +
                " WHERE " + DB.ContentsTime.MainTableCol.Is(mSqlCommand.GetParameterRef("aMainTable")) +
                " AND " + DB.ContentsTime.TimePeriodCol.In(mSqlCommand.GetParameterRef("valuesFromPxs"), valuesFromPxs.Count);


            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1+valuesFromPxs.Count];
            parameters[0] = mSqlCommand.GetStringParameter("aMainTable", aMainTable);
            int counter = 0;
            foreach (string valueFromPxs in valuesFromPxs)
            {
                parameters[counter + 1] = mSqlCommand.GetStringParameter("valuesFromPxs" + (counter + 1), valueFromPxs);
                counter++;
            }
           
           
            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString, parameters);
        }
        #endregion fixed list of timevalues

        #region  the last timevalue ( N=1 below)
        public PxSqlValues GetTimeValueList(string mainTable) {
            DataSet ds = this.GetTimeValues(mainTable);
            return GetTimeValues(mainTable, ds, true);
        }

        private DataSet GetTimeValues(string aMainTable)
        {
            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;

            // Get the contents-table data.
            // Standard (ANSI) SQL for this query.
            //
            // ORIGINALLY:
            // SELECT MAX(CTM.TimePeriod) TimePeriod /*** SQLID: GetTimeValues_01 ***/
            // FROM MetaData.ContentsTime CTM
            // WHERE UPPER(MainTable) = UPPER('<mainTable>')
            //
            // 11.11.2008 ThH: CHANGED TO:
            // SELECT MAX(CTM.TimePeriod) AS TimePeriod /*** SQLID: GetTimeValues_02 ***/
            // FROM MetaData.ContentsTime CTM
            // WHERE CTM.MainTable = '<mainTable>'
            //
            string sqlString =
                "SELECT MAX(" + DB.ContentsTime.TimePeriodCol.Id() + ") AS " + DB.ContentsTime.TimePeriodCol.PureColumnName() + " " +
                "/*** SQLID: " + currentMethod + "_02 ***/ " +
                " FROM " + DB.ContentsTime.GetNameAndAlias() +
                " WHERE " + DB.ContentsTime.MainTableCol.Is();

            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.

            // creating the parameters
            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[1];
            parameters[0] = DB.ContentsTime.MainTableCol.GetStringParameter(aMainTable);
            return mSqlCommand.ExecuteSelect(sqlString, parameters);
        }
        #endregion the last timevalue

        #region last N timevalues
        public PxSqlValues GetTimeValueList(string mainTable, int NoOfTimeValues) {
             DataSet ds = this.GetTimeValues(mainTable,NoOfTimeValues);
             return GetTimeValues(mainTable, ds, true);
        }


        private DataSet GetTimeValues(string mainTable, int NoOfTimeValues) {
            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;

            // Get the contents-table data.
            // Standard (ANSI) SQL for this query.
            //

            //
            // 11.11.2008 ThH: CHANGED TO:
            // SELECT a.TimePeriod, COUNT(*) /*** SQLID: GetTimeValues_03 ***/
            // FROM
            // (SELECT DISTINCT MainTable, TimePeriod
            //  FROM MetaData.ContentsTime CTM 
            //  WHERE CTM.MainTable = '<mainTable>') AS a,
            // (SELECT DISTINCT MainTable, TimePeriod
            //  FROM MetaData.ContentsTime CTM 
            //  WHERE CTM.MainTable = '<mainTable>') AS b
            // WHERE a.TimePeriod <= b.TimePeriod
            // GROUP BY a.TimePeriod
            // HAVING COUNT(*) <= <NoOfTimeValues>
            // ORDER BY COUNT(*) DESC
            //
            // PIV removed AS
            //

            string innerSelect = "SELECT DISTINCT " + DB.ContentsTime.MainTableCol.PureColumnName() + ", " + DB.ContentsTime.TimePeriodCol.PureColumnName() +
                " FROM " + DB.ContentsTime.GetNameAndAlias();
               

            string sqlString =
                "SELECT a." + DB.ContentsTime.TimePeriodCol.PureColumnName() + ", COUNT(*) /*** SQLID: GetTimeValues_03 ***/" +
                " FROM " +
                "(" + innerSelect + " WHERE " + DB.ContentsTime.MainTableCol.Is("mainTable_a") + ") a," +
                "(" + innerSelect + " WHERE " + DB.ContentsTime.MainTableCol.Is("mainTable_b") + ") b " +
                " WHERE a." + DB.ContentsTime.TimePeriodCol.PureColumnName() + " <= b." + DB.ContentsTime.TimePeriodCol.PureColumnName() +
                " GROUP BY a." + DB.ContentsTime.TimePeriodCol.PureColumnName() +
                " HAVING COUNT(*) <= " + NoOfTimeValues.ToString() + 
                " ORDER BY COUNT(*) DESC ";

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = DB.ContentsTime.MainTableCol.GetStringParameter(mainTable, "mainTable_a");
            parameters[1] = DB.ContentsTime.MainTableCol.GetStringParameter(mainTable, "mainTable_b");
            //the value is same for parameters[0] and parameters[1] , but the names migth be different depending on dbVendor  ( ":mainTable_a" and "mainTable_b" or "?" and "?" )


            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString, parameters);
        }

        #endregion last N timevalues


        #region from start timevalue
        public PxSqlValues GetTimeValueList(string mainTable,  string StartTimeValue) {
            DataSet ds = this.GetTimeValues(mainTable,StartTimeValue);
            return GetTimeValues(mainTable, ds, true);
        }

        private DataSet GetTimeValues(string aMainTable, string StartTimeValue) {
            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;

            // Get the contents-table data.
            // Standard (ANSI) SQL for this query.
            //
            // ORIGINALLY:
            // SELECT DISTINCT TimePeriod
            // FROM MetaData.ContentsTime
            // WHERE MainTable = '<mainTable>'
            //   AND TimePeriod >= '<StartTimeValue>'
            // ORDER BY TimePeriod ASC
            //
            // 11.11.2008 ThH: CHANGED TO:
            // SELECT DISTINCT CTM.TimePeriod /*** SQLID: GetTimeValues_04 ***/
            // FROM MetaData.ContentsTime CTM
            // WHERE CTM.MainTable = '<mainTable>'
            //   AND CTM.TimePeriod >= '<StartTimeValue>'
            // ORDER BY TimePeriod ASC
            //
            string sqlString =
                "SELECT DISTINCT " + DB.ContentsTime.TimePeriodCol.Id() + " /*** SQLID: GetTimeValues_04 ***/" +
                " FROM " + DB.ContentsTime.GetNameAndAlias() +
                " WHERE " + DB.ContentsTime.MainTableCol.Is() +
                " AND " + DB.ContentsTime.TimePeriodCol.GreaterOrEqual() +
                " ORDER BY " + DB.ContentsTime.TimePeriodCol.PureColumnName() + " ASC ";



            

            System.Data.Common.DbParameter[] parameters = new System.Data.Common.DbParameter[2];
            parameters[0] = DB.ContentsTime.MainTableCol.GetStringParameter(aMainTable);
            parameters[1] = DB.ContentsTime.TimePeriodCol.GetStringParameter(StartTimeValue);
            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString, parameters);
        }
        #endregion from start timevalue

        /// <summary>
        /// Converts a DataSet to a list of PXSqlValue for time
        /// </summary>
        /// <param name="mainTable">For error message</param>
        /// <param name="ds">The Dataset</param>
        /// <param name="setSortCode">Sets SortCodePxs to a counter. True for all ((int)mPxsFile.Query.Time.TimeOption != 0). </param>
        /// <returns></returns>
        private PxSqlValues GetTimeValues(string mainTable, DataSet ds, bool setSortCode) {
            PxSqlValues myOut = new PxSqlValues();

            DataRowCollection rows = ds.Tables[0].Rows;

            //Todo; fix exception
            if (rows.Count <= 0) {
                throw new ApplicationException("tabellen " + mainTable + " inneholder ikke data. (ingen tider)");
            }

            int timeSortOrder = 0;
            string timeCode = "";
            foreach (DataRow row in rows) {
                timeCode = row[this.DB.ContentsTime.TimePeriodCol.PureColumnName()].ToString();
                PXSqlValue mValue = new PXSqlValue(timeCode, LanguageCodes);


                if (setSortCode) {
                    mValue.SortCodePxs = timeSortOrder;
                    timeSortOrder++;
                }
                myOut.Add(mValue.ValueCode, mValue);
            }
            return myOut;
        }



        #endregion Time stuff

 
    

    }
         
}
