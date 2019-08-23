using System;
using System.Collections.Generic;
using System.Text;
using PCAxis.Sql.DbConfig;

using System.Data;
using System.Reflection;

using PCAxis.Sql.Parser_21;

using System.Collections.Specialized;

namespace PCAxis.Sql.QueryLib_21
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

        public DataSet GetAllTimeValues(string mainTable, string sortOrder) {
            // Get the name of the current method.
            string currentMethod = MethodBase.GetCurrentMethod().Name;

            string sqlString =
                "SELECT DISTINCT " + DB.ContentsTime.TimePeriodCol.Id() +
                "/*** SQLID: " + currentMethod + "_01 ***/ " +
                " FROM " + DB.ContentsTime.GetNameAndAlias() +
                " WHERE " + DB.ContentsTime.MainTableCol.Is(mainTable) +
                " ORDER BY " + DB.ContentsTime.TimePeriodCol.Id() + " " + sortOrder;


            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString);
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

        private DataSet GetTimeValues(string mainTable, ICollection<string> valuesFromPxs) {

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
                " WHERE " + DB.ContentsTime.MainTableCol.Is(mainTable) +
                " AND " + DB.ContentsTime.TimePeriodCol.In(valuesFromPxs); 

            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString);
        }
        #endregion fixed list of timevalues

        #region  the last timevalue ( N=1 below)
        public PxSqlValues GetTimeValueList(string mainTable) {
            DataSet ds = this.GetTimeValues(mainTable);
            return GetTimeValues(mainTable, ds, true);
        }
        
        private DataSet GetTimeValues(string mainTable) {
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
                "SELECT MAX(" + DB.ContentsTime.TimePeriodCol.Id() + ") AS " + DB.ContentsTime.TimePeriod + " " +
                "/*** SQLID: " + currentMethod + "_02 ***/ " +
                " FROM " + DB.ContentsTime.GetNameAndAlias() +
                " WHERE " + DB.ContentsTime.MainTableCol.Is(mainTable);
            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString);
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

            string innerSelect = "SELECT DISTINCT " + DB.ContentsTime.MainTable + ", " + DB.ContentsTime.TimePeriod +
                " FROM " + DB.ContentsTime.GetNameAndAlias() +
                " WHERE " + DB.ContentsTime.MainTableCol.Is(mainTable);

            string sqlString =
                "SELECT a." + DB.ContentsTime.TimePeriod + ", COUNT(*) /*** SQLID: GetTimeValues_03 ***/" +
                " FROM " +
                "(" + innerSelect + ") a," +
                "(" + innerSelect + ") b " +
                " WHERE a." + DB.ContentsTime.TimePeriod + " <= b." + DB.ContentsTime.TimePeriod +
                " GROUP BY a." + DB.ContentsTime.TimePeriod +
                " HAVING COUNT(*) <= " + NoOfTimeValues +
                " ORDER BY COUNT(*) DESC ";

            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString);
        }

        #endregion last N timevalues


        #region from start timevalue
        public PxSqlValues GetTimeValueList(string mainTable,  string StartTimeValue) {
            DataSet ds = this.GetTimeValues(mainTable,StartTimeValue);
            return GetTimeValues(mainTable, ds, true);
        }

        private DataSet GetTimeValues(string mainTable, string StartTimeValue) {
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
                " WHERE " + DB.ContentsTime.MainTableCol.Is(mainTable) +
                " AND " + DB.ContentsTime.TimePeriodCol.Id() + " >= '" + StartTimeValue + "'" +
                " ORDER BY " + DB.ContentsTime.TimePeriod + " ASC ";

            // Execute the SQL and return the result (records) as a System.Data.DataSet-object.
            return mSqlCommand.ExecuteSelect(sqlString);
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
                timeCode = row[this.DB.ContentsTime.TimePeriod].ToString();
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
