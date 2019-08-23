using System;
using System.Collections.Generic;
using System.Text;


using System.Data; // For DataSet-objects.
using Oracle.ManagedDataAccess.Client;// For Oracle-connections.


using System.Collections.Specialized;

using System.Data.Common;
using log4net;


namespace PCAxis.Sql.SqlClientCleanup
{

    /// <summary>
    /// The Oracle version of MyDbVendor.
    /// </summary>
    internal class MyDbVendorIsOracle:MyDbVendor  {
        
        private static readonly ILog log = LogManager.GetLogger(typeof(MyDbVendorIsOracle));
  
        internal MyDbVendorIsOracle(string connectionString)
            :base( new OracleConnection(connectionString), connectionString) {
            
            KeywordAfterCreateIndicatingTempTable = "GLOBAL TEMPORARY";
            TempTableCreateCommandEndClause = "ON COMMIT PRESERVE ROWS";

            ProgramMustTrunCAndDropTempTable = true;

            OracleNinja1 = " * from (SELECT ";
            OracleNinja2 = " union all select null,   null, null,   null,   null,   null,  null,   null,  null from dual  ) a ";

        }

        internal override DbConnection CreateDbConnection()
        {
            return new OracleConnection(ConnectionString);
        }

        internal override DbConnectionStringBuilder GetDbConnectionStringBuilder(string connectionString)
        {
            return new OracleConnectionStringBuilder(connectionString);
        }

        internal override  DbCommand GetDbCommand(string commandString) {

            return new OracleCommand(commandString, (OracleConnection)this.dbconn) {BindByName = true };
        }

        internal override DbDataAdapter GetDbDataAdapter(string selectString) {
            return new OracleDataAdapter(selectString, ConnectionString);

            //var aaa = new OracleDataAdapter(selectString, (OracleConnection)this.dbconn);
           

            //    return new OracleDataAdapter(selectString, (OracleConnection)this.dbconn);

        }

        internal override DbParameter GetEmptyDbParameter()
        {
            return new OracleParameter();
        }


        internal override string GetParameterRef(string propertyName)
        {
            return ":"+propertyName;
        }

        internal override string joinCommandStrings(StringCollection commandStrings) {
            StringBuilder sb = new StringBuilder(commandStrings.Count * 50); // or ?
            sb.Append("BEGIN ");
            foreach (string commandString in commandStrings) {
                sb.Append(commandString + ";");
            }
             sb.Append(" END;");
            return sb.ToString();
        }


        internal override void BulkInsertIntoTemp(string sqlString, DbParameter[] parameters, string[] valueCodesArray1, string[] valueCodesArray2, int[] parentCodeCounter)
        {
            using (var pxDbCommand = new OracleCommand(sqlString))
            {
                //using (var conn = new OracleConnection(this.dbconn.ConnectionString))
                using (var conn = new OracleConnection(ConnectionString))
                {
                    pxDbCommand.Connection = conn;
                    conn.Open();

                    pxDbCommand.ArrayBindCount = valueCodesArray1.Length;
                    pxDbCommand.BindByName = true;
                    parameters[0].Value = valueCodesArray1;
                    parameters[1].Value = valueCodesArray2;
                    parameters[2].Value = parentCodeCounter;
                    pxDbCommand.Parameters.AddRange(parameters);

                    pxDbCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
