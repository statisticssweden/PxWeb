using System;
using System.Collections.Generic;
using System.Text;


using System.Data; // For DataSet-objects.
using System.Data.Odbc; // For Odbc-connections.
using System.Collections.Specialized;

using System.Data.Common;
using log4net;



namespace PCAxis.Sql.SqlClientCleanup
{

    /// <summary>
    /// Odbc
    /// </summary>
    internal class MyDbVendorIsOdbc : MyDbVendor {

        private static readonly ILog log = LogManager.GetLogger(typeof(MyDbVendorIsOdbc));


        internal MyDbVendorIsOdbc( string connectionString)
            :base(new OdbcConnection(connectionString), connectionString) {
        }

        internal override DbConnection CreateDbConnection()
        {
            return new OdbcConnection(ConnectionString);
        }

        internal override DbConnectionStringBuilder GetDbConnectionStringBuilder(string connectionString)
        {
            return new OdbcConnectionStringBuilder(connectionString);
        }

        internal override DbCommand GetDbCommand(string commandString) {
            return new OdbcCommand(commandString, (OdbcConnection)this.dbconn);
        }

        internal override DbDataAdapter GetDbDataAdapter(string selectString) {
            return new OdbcDataAdapter(selectString, (OdbcConnection) this.dbconn);
        }

        internal override DbParameter GetEmptyDbParameter()
        {
            return new OdbcParameter();
        }

        internal override string GetParameterRef(string propertyName)
        {
            return "?";
        }

    }
}
