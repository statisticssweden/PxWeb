namespace PCAxis.Sql.SqlClientCleanup
{

using System;
using System.Collections.Specialized;
using System.Data; 
using System.Data.Common;
using System.Text;
using log4net;

    /// <summary>
    /// The purpose of this class is to hide the diffence of the classnames the database vendors.
    /// The handeling of differences in SQL is left to PxSQLCommand. 
    /// </summary>
    internal abstract class MyDbVendor : IDisposable  {
        #region Properties and Member variables

        private static readonly ILog log = LogManager.GetLogger(typeof(MyDbVendor));
        
        protected readonly DbConnection dbconn;

        internal  string mXtraDotForDatatables="";
        internal  string PrefixIndicatingTempTable = "";
        internal  string KeywordAfterCreateIndicatingTempTable = "";
        //Only oracle?
        internal  string TempTableCreateCommandEndClause = "";
        internal  Boolean ProgramMustTrunCAndDropTempTable = false;

        //In rare cases 1? in 12000? a distinct in the footnote sql caused an 40 second increase in executing time, but/and the rowset was empty,
        //which must mean that the distinct somehow changed how the query was executed. 
        //The below hack is a novices way to make oracle first "collect the rows" and then apply the distinct ( on 0 rows) 
        //Why this occues only for special case is not understood.  
        internal string OracleNinja1 = "";
        internal string OracleNinja2 = "";



        internal string ConnectionString { get; private set; }

        #endregion Properties and Member variables

        internal MyDbVendor(DbConnection aDbconn, string connectionString) {
            this.dbconn = aDbconn;
            ConnectionString = connectionString;
        }

        internal abstract DbConnectionStringBuilder GetDbConnectionStringBuilder(string connectionString);
        internal abstract DbCommand GetDbCommand(string commandString) ;

        internal abstract DbDataAdapter GetDbDataAdapter(string selectString);

        internal abstract DbParameter GetEmptyDbParameter();

        internal abstract DbConnection CreateDbConnection();

        /// <summary>
        /// The reference to a parameter, e.g. @maintable,:maintable or just ? depending on your db
        /// </summary>
        internal abstract string GetParameterRef(string propertyName) ;


        internal virtual string ConcatString(params string[] myStrings)
        {
            string ConcatedString = "";
            string endCommas = "";
            for (int i = 0; i < myStrings.Length; i++)
            {
                if (i < myStrings.Length - 1)
                {
                    ConcatedString += "CONCAT(" + myStrings[i] + ",";
                    endCommas += ")";
                }
                else
                {
                    ConcatedString += myStrings[i] + endCommas;
                }
            }
            return ConcatedString;
        }


        #region IDisposable Members

        /// <summary>
        /// Disposes the connection
        /// </summary>
        public void Dispose() {
            
            if (dbconn.State == ConnectionState.Open)
            {
                dbconn.Close();
                log.Debug("Closing database connection.");
            }

            dbconn.Dispose();
        }

        #endregion

        /// <summary>
        /// Opens the connection if it is not already open
        /// </summary>
        //internal void ensureConnectionOpen() {
        //    if (this.dbconn.State != ConnectionState.Open) {
        //        this.dbconn.Open();
        //        log.Debug("(Re)opening database connection.");
        //    }
        //}

        /// <summary>Joins all strings in commandStrings with a ; 
        /// </summary>
        /// <param name="commandStrings">The commandstrings to join</param>
        /// <returns></returns>
        internal virtual string joinCommandStrings(StringCollection commandStrings) {
            StringBuilder sb = new StringBuilder(commandStrings.Count * 50); // or ?
            
            foreach (string commandString in commandStrings) {
                sb.Append(commandString + ";");
            }

            return sb.ToString();
        }





        /// <summary>
        /// Inserts the 3 array into the/a temp table
        /// </summary>
        /// <param name="sqlString">Parameter based insert sql</param>
        /// <param name="parameters">The 3 parameters </param>
        /// <param name="valueCodesArray1">valueCodes1</param>
        /// <param name="valueCodesArray2">valueCodes2</param>
        /// <param name="parentCodeCounter">parentCodeCounter</param>
        internal virtual void BulkInsertIntoTemp(string sqlString, DbParameter[] parameters, string[] valueCodesArray1, string[] valueCodesArray2, int[] parentCodeCounter)
        {
            using (DbCommand pxDbCommand = this.GetDbCommand(sqlString))
            {
                using (var conn = pxDbCommand.Connection)
                {
                    conn.Open();
                    pxDbCommand.Parameters.AddRange(parameters);

                    for (int n = 0; n < valueCodesArray1.Length; n++)
                    {
                        parameters[0].Value = valueCodesArray1[n];
                        parameters[1].Value = valueCodesArray2[n];
                        parameters[2].Value = parentCodeCounter[n];
                        pxDbCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
