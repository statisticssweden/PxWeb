using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

using System.Data; // For DataSet-objects.
using System.Data.SqlClient; // For MS SQLServer-connections.
//using System.Data.OracleClient; // For Oracle-connections.
using System.Data.OleDb; // For OleDb-connections.
using System.Collections.Specialized;

using System.Data.Common;
using log4net;
    
using PCAxis.Sql.Exceptions;

namespace PCAxis.Sql.SqlClientCleanup
{
    /// <summary> 13:10
    /// This class represents a "common sql gateway" for all sql databases
    /// supported by the PC-Axis database working group (statistical databanks).
    /// 
    /// List of supported ADO dotNet database connections:
    ///   - Oracle (native)
    ///   - Microsoft Sql Server (native)
    ///   - Sybase (native)????
    ///   - OleDb (Other connections. E.g. MySql, PostgreSql, ...)
    /// 
    /// Developed by:
    ///   Statistics Norway (www.ssb.no), 2007.
    /// 
    /// System specification and development (alphabetic list):
    ///   Bjørn Roar Joneid (bnj@ssb.no)
    ///   Per Inge Vaaje (piv@ssb.no)
    ///   Thomas Hoel (thh@ssb.no)
    /// </summary>
    public class PxSqlCommandNoTempTables : IDisposable, PCAxis.Sql.PxSqlCommand
    {

        #region Fields and Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(PxSqlCommand));

        #if DEBUG
                 private static readonly log4net.ILog logTime = log4net.LogManager.GetLogger("LogTime");
        #endif

                 private readonly string connectionString;
        //this indicates which SQL version to use. Not needed?
        private readonly string mDataBaseType = "";


        private readonly MyDbVendor myDbVendor;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with connection-parameters.
        /// </summary>
        public PxSqlCommandNoTempTables(string dbType, string dataProvider, string connectionString) {
            this.connectionString = connectionString;
            // dataProvider =  "OleDb","Oracle","Sql","Odbc","SqlCe"
            if ( String.IsNullOrEmpty(connectionString)) {
                throw new PxsException(37);
            }
            if (String.IsNullOrEmpty(dataProvider)){
                throw new PxsException(38);
            }

            mDataBaseType = dbType;

            //log.Debug("PxSqlCommand started with type: " + dbType + ", dataProvider:" + dataProvider + " and string:" + connectionString);
            String lDataProvider = dataProvider.ToUpper();
            if (lDataProvider.Equals("ORACLE")) {
                myDbVendor = new MyDbVendorIsOracle(connectionString);
            } else if (lDataProvider.Equals("SQL")) {
                myDbVendor = new MyDbVendorIsSql(connectionString);
            } else if (lDataProvider.Equals("OLEDB")) {
                myDbVendor = new MyDbVendorIsOledb(connectionString);
            } else if (lDataProvider.Equals("ODBC")) {
                myDbVendor = new MyDbVendorIsOdbc(connectionString);
            } else {
                // lDataProvider.Equals("SQLCE") could not find namespace
                throw new PxsException(39, "\"OleDb\",\"Oracle\",\"Sql\", \"Odbc\"", dataProvider);

            }



        }

        #endregion Constructor

        #region Methods


        public DbConnectionStringBuilder connectionStringBuilder()
        {
            return myDbVendor.GetDbConnectionStringBuilder(connectionString);
        }


        /// <summary>
        /// Execute a select-statement and return the query result as a "System.Data DataSet".
        /// </summary>
        /// <param name="selectString">The SQL query (select).</param>
        /// <returns>A "System.Data DataSet" with the data result from the query.</returns>
        public DataSet ExecuteSelect(string selectString) {

            // Check SQL param
            if (String.IsNullOrEmpty(selectString)) {
                throw new BugException("Error PxSqlClient.PxSqlCommand.ExecuteSelect: Parameter \"selectString\" is empty/null!");
            }
            log.Debug(selectString);

            if(selectString.Contains(";"))
            {
                throw new BugException("Error PxSqlClient.PxSqlCommand.ExecuteSelect: Parameter \"selectString\" contains semicolon");
            }
            #if DEBUG
                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();
            #endif
            DataSet pxDataSet = new DataSet();
            using (DbDataAdapter pxDataAdapter = myDbVendor.GetDbDataAdapter(selectString)) {
                
                pxDataAdapter.Fill(pxDataSet);
                //TODO: Check for pxDataAdapter.FillError???
            }

            #if DEBUG
                stopWatch.Stop();
                logTime.DebugFormat("    Completed " + System.Reflection.MethodBase.GetCurrentMethod().Name + " in ms = {0} for sql = {1}", stopWatch.ElapsedMilliseconds, selectString);
            #endif

            return pxDataSet;
        }

        /// <summary>
        /// Executes a select query with parameters. Used from cnmm 2.3
        /// </summary>
        /// <param name="selectString">The parameterized querystring</param>
        /// <param name="parameters">Array of pararmeters, or null if there are no parameters</param>
        /// <returns>The answer as a dataset</returns>
        public DataSet ExecuteSelect(string selectString, DbParameter[] parameters)
        {
            // Check SQL param
            if (String.IsNullOrEmpty(selectString))
            {
                throw new BugException("Error PxSqlClient.PxSqlCommand.ExecuteSelect: Parameter \"selectString\" is empty/null!");
            }
            log.Debug(selectString);
            #if DEBUG
                    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();
            #endif

            DataSet pxDataSet = new DataSet();
            try
            {
                using (DbDataAdapter pxDataAdapter = myDbVendor.GetDbDataAdapter(selectString))
                {
                    if (parameters != null)
                    {
                        pxDataAdapter.SelectCommand.Parameters.AddRange(parameters);
                    }
                    pxDataAdapter.Fill(pxDataSet);
                    //TODO: Check for pxDataAdapter.FillError???
                }
            }
            catch (Exception e)
            {
                foreach (DbParameter para in parameters)
                {
                    log.Error("para name=" + para.ParameterName + " value=" + para.Value.ToString());
                }
                throw;
            }
            #if DEBUG
                    stopWatch.Stop();
                    logTime.DebugFormat("    Completed " + System.Reflection.MethodBase.GetCurrentMethod().Name + " in ms = {0} for sql = {1}", stopWatch.ElapsedMilliseconds, selectString);
            #endif
            return pxDataSet;
        }


       /// <summary>
       /// Returns a DbParameter with type= string and  parameterName and parameterValue as given.
       /// </summary>
       public DbParameter GetStringParameter(string parameterName, string parameterValue)
       {
           DbParameter myOut = myDbVendor.GetEmptyDbParameter();
           myOut.DbType = DbType.String;
           myOut.ParameterName = parameterName;
           myOut.Value = parameterValue;
           return myOut;
       }

       /// <summary>
       /// The reference to a parameter, e.g. @maintable,:maintable or just ? depending on your db
       /// </summary>
       /// <param name="propertyName">The "base" e.g. maintable</param>
       /// <returns>@maintable,:maintable or just ? depending on your db</returns>
       public string GetParameterRef(string propertyName)
       {
           return myDbVendor.GetParameterRef(propertyName);
       }

        /// <summary>
        /// Execute a "non query" command (insert, update, delete, create, drop, ...). Explicit connection.
        /// </summary>
        /// <param name="commandString">The SQL command.</param>
        /// <returns>For UPDATE, INSERT and DELETE statements. The return value is the number of rows affected by the command. For CREATE TABLE and DROP TABLE statements, the return value is 0. For all other types of statements, the return value is -1.</returns>
        public int ExecuteNonQuery(string commandString) {
            int numOfRowsAffected = -1;  // Default value.

            // Check  SQL
            if (String.IsNullOrEmpty(commandString)) {
                throw new BugException("PxSqlClient.PxSqlCommand.ExecuteNonQuery: Parameter \"commandString\" is empty/null!");
            }
            log.Debug(commandString);

            if (commandString.Contains(";"))
            {
                throw new BugException("PxSqlClient.PxSqlCommand.ExecuteNonQuery: Parameter \"commandString\" contains semicolon");
            }
            #if DEBUG
                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();
#endif

            using (DbCommand pxDbCommand = myDbVendor.GetDbCommand(commandString))
            {
                using (var conn = myDbVendor.CreateDbConnection())
                {
                    pxDbCommand.Connection = conn;
                    conn.Open();
                    numOfRowsAffected = pxDbCommand.ExecuteNonQuery();
                }
            }

                


            #if DEBUG
                stopWatch.Stop();
                logTime.DebugFormat("    Completed " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Done in ms = {0} for sql={1}", stopWatch.ElapsedMilliseconds, commandString);
            #endif

            return numOfRowsAffected;
        }

        /// <summary>
        /// To be used as partial tablename for temporary tables.
        /// e.g. tmpTabName="A"+getUniqueNumber(8)+"_tmp001"
        /// Must ensure that the length of the tablename does not excede the max  
        /// length of a tablename for the database.
        /// 
        /// </summary>
        /// <param name="lengthOfOtherChars">The length of the rest of the tablename</param>
        /// <returns></returns>
        public string getUniqueNumber(int lengthOfOtherChars) {
            int dbMaxLength = 30;//
            int outLength = dbMaxLength - lengthOfOtherChars - 15;


            string now = DateTime.Now.ToString("ddHHmmssfffffff"); //(15 chars)
            string sid = Guid.NewGuid().ToString("N");


            int diff = sid.Length - outLength;
            sid = sid.Substring(diff);

            return now + sid;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandStrings"></param>
        /// <param name="numberInBulk"></param>
        /// <returns></returns>
        public int InsertBulk(StringCollection commandStrings, int numberInBulk) {

            if (commandStrings == null || commandStrings.Count == 0) {
                throw new BugException("Error PxSqlClient.PxSqlCommand.InsertBulk: Parameter \"commandStrings\" is empty/null!");
            }
            #if DEBUG
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            logTime.DebugFormat("    Start " + System.Reflection.MethodBase.GetCurrentMethod().Name + " commandStrings.Count ={0}", commandStrings.Count);
            #endif
            StringCollection sqlStrings = new StringCollection();

            int numOfRowsAffected = 0; // jfi: denne var -1

            int ValuesCounter = 0;
            int rem = 0;

            //myDbVendor.ensureConnectionOpen(); //jfi: kan vi komme hit hvis den ikke er open



            foreach (string value in commandStrings) {
                if (value.Contains(";"))
                {
                    log.Error("Bad command: "+value);
                    throw new BugException("Error PxSqlClient.PxSqlCommand.InsertBulk: command contains semicolon");
                }
                sqlStrings.Add(value);
                ValuesCounter += 1;
                Math.DivRem(ValuesCounter, numberInBulk, out rem);
                if ((rem == 0) || ValuesCounter == commandStrings.Count) {
                    numOfRowsAffected += this.InsertBulk(sqlStrings);
                    sqlStrings.Clear();
                }
            }

            /*
             *jfi: er dette et bedre alt.? 
            
            for (int i = 0; i < commandStrings.Count; i++) {
                sqlStrings.Add(commandStrings[i]);
                if(sqlStrings.Count = numberInBulk || i = commandStrings.Count -1) {
                    numOfRowsAffected += this.InsertBulk(sqlStrings);
                    sqlStrings.Clear();
                }
            }
            
            */
            #if DEBUG
                  stopWatch.Stop();
                  logTime.DebugFormat("    " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Done in ms = {0}", stopWatch.ElapsedMilliseconds);
            #endif
            return numOfRowsAffected;
        }



        private int InsertBulk(StringCollection commandStrings) {
            #if DEBUG
                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();                
            #endif
            string command = myDbVendor.joinCommandStrings(commandStrings);

            log.Debug(command);

            using (DbCommand pxDbCommand = myDbVendor.GetDbCommand(command))
            {
                using (var conn = myDbVendor.CreateDbConnection())
                {
                    pxDbCommand.Connection = conn;
                    conn.Open();
                    // Return number of rows affected.
                    int myOut = pxDbCommand.ExecuteNonQuery();
#if DEBUG
                    stopWatch.Stop();
                    logTime.DebugFormat("           Completed inner " + System.Reflection.MethodBase.GetCurrentMethod().Name + " in ms = {0} for commandStrings.Count ={1}", stopWatch.ElapsedMilliseconds, commandStrings.Count);
#endif
                    return myOut;
                }
            }



             
        }

        #endregion


        #region things here to enable similar SQL-string-build-up for all "vendors"
        /// <summary>
        /// Returns "." for the datatables prefix for mssql otherwise ""
        /// </summary>
        /// <returns></returns>
        public string getExtraDotForDatatables() {
            return myDbVendor.mXtraDotForDatatables;
        }


        /// <summary>
        /// Prefix for the tablename. Some vendors use this to indicate that a table is temporary.
        /// </summary>
        /// <returns>"" or prefix</returns>
        public string getPrefixIndicatingTempTable() {
            return myDbVendor.PrefixIndicatingTempTable;
        }

        /// <summary>
        /// Extra keyword for temp tables. Some vendors use this to indicate that a table is temporary.
        /// </summary>
        /// <returns>spaces or Keyword</returns>
        public string getKeywordAfterCreateIndicatingTempTable() {
            return " " + myDbVendor.KeywordAfterCreateIndicatingTempTable + " ";
        }

        /// <summary>
        /// Extra clause for temp tables. Some vendors use this when a table is temporary.
        /// </summary>
        /// <returns>spaces or Keyword</returns>
        public string getTempTableCreateCommandEndClause() {
            return " " + myDbVendor.TempTableCreateCommandEndClause;
        }


        /// <summary>
        /// For oracle in ssb, In rare cases 1? in 12000? a distinct in the footnote sql caused an 40 second increase in executing time, but/and the rowset was empty,
        /// which must mean that the distinct somehow changed how the query was executed. 
        /// The OracleNinjaN hack is a novices way to make oracle first "collect the rows" and then apply the distinct ( on 0 rows) 
        /// Why this occues only for special case is not understood.  
        /// </summary>
        /// <returns>sql if oracle, for others empty string</returns>
        public string getOracleNinja1()
        {
            return " " + myDbVendor.OracleNinja1;
        }

        /// <summary>
        /// See getOracleNinja1
        /// </summary>
        /// <returns>sql if oracle, for others empty string</returns>
        public string getOracleNinja2()
        {
            return " " + myDbVendor.OracleNinja2;
        }
       

        /// <summary>
        /// Should clean up be done by this application
        /// </summary>
        /// <returns>true false</returns>
        public Boolean getProgramMustTruncAndDropTempTable() {
            return myDbVendor.ProgramMustTrunCAndDropTempTable;
        }


        #endregion things here to enable similar SQL-string-build-up for all "vendors"

        /// <summary>
        /// Concats string in a vendorspesific way
        /// </summary>
        /// <param name="myStrings">Strings to concat</param>
        /// <returns>A long string</returns>
        public string getConcatString(params string[] myStrings)
        {
            return myDbVendor.ConcatString(myStrings);
        }


        /// <summary>
        /// Makes temp tables when extraction does not use grouping
        /// </summary>
        public string MakeTempTableJustValues(string VariableName, string VariableNumber, bool UseTemporaryTables, StringCollection valueCodes)
        {
           #if DEBUG
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
           #endif

            bool makeGroupFactorCol = false;
            string tempTableId = this.GetTempTableId(VariableNumber, UseTemporaryTables);

            createTempTable(tempTableId, VariableName, VariableNumber, makeGroupFactorCol, UseTemporaryTables);

            // inserts the same code in 2 columns, so the table looks the same as for grouping.

            string sqlString = "INSERT INTO /*** MakeTempTableJustValues_01 ***/ " + tempTableId;
            //sqlString += " VALUES (" + this.GetParameterRef("aValueCode1") + ", " + this.GetParameterRef("aValueCode2") + "," + this.GetParameterRef("aValueCounter") + ")";
            //is this easier to read?: 
            sqlString += string.Format(" VALUES ({0}, {1}, {2})", this.GetParameterRef("aValueCode1"), this.GetParameterRef("aValueCode2"), this.GetParameterRef("aValueCounter"));
            

            
            DbParameter[] parameters = new DbParameter[3];
            parameters[0] = this.GetStringParameter("aValueCode1", "WillBeOverwritten");
            parameters[1] = this.GetStringParameter("aValueCode2","WillBeOverwritten");
            parameters[2] = this.myDbVendor.GetEmptyDbParameter();
            parameters[2].DbType = DbType.Int32;
            parameters[2].ParameterName = "aValueCounter";
            parameters[2].Value = 123;

            // ( all groups have 1 member) 
            int[] parentCodeCounter = Enumerable.Range(1, valueCodes.Count).ToArray();

            String[] valueCodesArray = valueCodes.Cast<String>().ToList().Select(c => c).ToArray();

            myDbVendor.BulkInsertIntoTemp(sqlString, parameters, valueCodesArray, valueCodesArray, parentCodeCounter);


#if DEBUG
            stopWatch.Stop();
            logTime.DebugFormat("           Completed " + System.Reflection.MethodBase.GetCurrentMethod().Name + " in ms = {0} for VariableName ={1}", stopWatch.ElapsedMilliseconds, VariableName);
#endif
            return tempTableId;
        }

        /// <summary>
        /// Makes temp tables when extraction uses grouping
        /// </summary>
        public string MakeTempTable(string VariableName, string VariableNumber, bool makeGroupFactorCol, bool UseTemporaryTables, StringCollection childCodes, StringCollection parentCodes, List<int> parentCodeCounterList)
        {
            //TODO: make this use myDbVendor.BulkInsertIntoTemp

            string tempTableId = this.GetTempTableId(VariableNumber, UseTemporaryTables);

            createTempTable(tempTableId, VariableName, VariableNumber, makeGroupFactorCol, UseTemporaryTables);

            string sqlString = "INSERT INTO /*** SQLID MakeTempTableJustValues_01 ***/ " + tempTableId;
            sqlString += " VALUES (" + this.GetParameterRef("aValueCode1") + ", " + this.GetParameterRef("aValueCode2") + "," + this.GetParameterRef("aValueCounter");
            if (makeGroupFactorCol)
            {
                sqlString += ",1";
            }
            sqlString += ")";

            using (DbCommand pxDbCommand = myDbVendor.GetDbCommand(sqlString))
            {
                using (var conn = myDbVendor.CreateDbConnection())
                {
                    pxDbCommand.Connection = conn;
                    conn.Open();

                    DbParameter[] parameters = new DbParameter[3];
                    parameters[0] = this.GetStringParameter("aValueCode1", "WillBeOverwritten");
                    parameters[1] = this.GetStringParameter("aValueCode2", "WillBeOverwritten");
                    parameters[2] = this.myDbVendor.GetEmptyDbParameter();
                    parameters[2].DbType = DbType.Int32;
                    parameters[2].ParameterName = "aValueCounter";
                    parameters[2].Value = 123;
                    pxDbCommand.Parameters.AddRange(parameters);

                    for (int ValuesCounter = 0; ValuesCounter < childCodes.Count; ValuesCounter++)
                    {
                        parameters[0].Value = childCodes[ValuesCounter];
                        parameters[1].Value = parentCodes[ValuesCounter];
                        parameters[2].Value = parentCodeCounterList[ValuesCounter];
                        pxDbCommand.ExecuteNonQuery();
                    }
                }
            }
            
            if (makeGroupFactorCol)
            {
                string sqlString2 =
                    "UPDATE /*** SQLID: MakeTempTable_03 ***/ " + tempTableId +
                    " SET groupfactor" + VariableNumber + " = " +
                    "(SELECT COUNT(*) FROM " + tempTableId + " a2 " +
                    "WHERE a2.group" + VariableNumber + " = " + tempTableId + ".group" + VariableNumber + ")";

               int numberOfRowsAffected = this.ExecuteNonQuery(sqlString2);
            }

            return tempTableId;
        }

        /// <summary>
        /// create a table temp table for a variable containting selected values.  
        /// It har 3 or 4 columns. The first contains incomming codes, the second output codes and the third is an output code counter used in the extraction-sql group by. 
        /// The fourth is used to determin if there are missing rows in the datatable, when the extraction uses sum and the default value for a missing row is a npm of type 3.    
        /// </summary>
        private void createTempTable(string tempTableId, string VariableName, string  VariableNumber, bool makeGroupFactorCol, bool UseTemporaryTables){
            log.Debug("tempTabellId:" + tempTableId + "        tempTableId len:" + tempTableId.Length);

            string sqlString = "CREATE /*** SQLID: createTempTable_01 ***/";

            if (UseTemporaryTables)
            {
                sqlString = sqlString + this.getKeywordAfterCreateIndicatingTempTable();
            }

            sqlString += " TABLE " + tempTableId;
          
            sqlString += "(A" + VariableName + " VARCHAR(20), Group" + VariableNumber + " VARCHAR(20), GroupNr" + VariableNumber + " INTEGER";

            if (makeGroupFactorCol)
            {
                sqlString += ", GroupFactor" + VariableNumber + " INTEGER";
            }
            sqlString += ")";

            if (UseTemporaryTables)
            {
                sqlString = sqlString + this.getTempTableCreateCommandEndClause();
            }

            this.ExecuteNonQuery(sqlString);
        } 

        private string GetTempTableId(string VariableNumber, bool UseTemporaryTables)
        {
            string myOut = "";

            string UniqueNumber = this.getUniqueNumber(7 + VariableNumber.Length);
            myOut = "A" + UniqueNumber + "_TMP" + VariableNumber;

            if (UseTemporaryTables)
            {
                myOut = this.getPrefixIndicatingTempTable() + myOut;
            }
            return myOut;
        }

        #region IDisposable Members

        public void Dispose() {
            myDbVendor.Dispose();
        }

        #endregion
    } // End class PxSqlCommand

}  // End namespace