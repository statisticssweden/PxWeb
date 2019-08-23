using System;
using System.Collections.Generic;
using System.Text;


using System.Data; // For DataSet-objects.
using System.Data.OleDb; // For OleDb-connections.
using System.Collections.Specialized;

using System.Data.Common;
using log4net;


namespace PCAxis.Sql.DbClient {

    /// <summary> The Oledb version of MyDbVendor
    ///  </summary>
    internal class MyDbVendorIsOledb : MyDbVendor {
        
        private static readonly ILog log = LogManager.GetLogger(typeof(MyDbVendorIsOledb));

        internal MyDbVendorIsOledb( string connectionString)
            : base(new OleDbConnection(connectionString)) {
        }

        internal override DbConnectionStringBuilder GetDbConnectionStringBuilder(string connectionString)
        {
            return new OleDbConnectionStringBuilder(connectionString);
        }

        internal override DbCommand GetDbCommand(string commandString ){
                return new OleDbCommand(commandString, (OleDbConnection) this.dbconn);
        }

        internal override DbDataAdapter GetDbDataAdapter(string selectString) {
            return new OleDbDataAdapter(selectString, (OleDbConnection) this.dbconn);
        }

        internal override DbParameter GetEmptyDbParameter()
        {
            return new OleDbParameter();
        }

        internal override string GetParameterRef(string propertyName)
        {
            return "?";
        }
    }
}
