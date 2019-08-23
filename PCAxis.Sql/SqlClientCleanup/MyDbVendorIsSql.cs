using System;
using System.Collections.Generic;
using System.Text;


using System.Data; // For DataSet-objects.
using System.Data.SqlClient; // For MS SQLServer-connections.

using System.Collections.Specialized;

using System.Data.Common;
using log4net;


namespace PCAxis.Sql.SqlClientCleanup
{

    /// <summary> The (MS)Sql version of MyDbVendor.
    /// </summary>
    internal class MyDbVendorIsSql : MyDbVendor {
        
        private static readonly ILog log = LogManager.GetLogger(typeof(MyDbVendorIsSql));
        private static int? CommandTimeout { get; set; }
       
        internal MyDbVendorIsSql(string connectionString)
            : base(new SqlConnection(connectionString), connectionString) {
               mXtraDotForDatatables = ".";
               PrefixIndicatingTempTable = "#";
               KeywordAfterCreateIndicatingTempTable = "";
        }

        internal override DbConnection CreateDbConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        static MyDbVendorIsSql()
        {
            var x = System.Configuration.ConfigurationManager.AppSettings.GetValues("SqlCommandTimeout");

            if (x != null)
            {
                int t;

                if (int.TryParse(x[0], out t))
                {
                    CommandTimeout = t;
                }
            }
        }

        internal override DbConnectionStringBuilder GetDbConnectionStringBuilder(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString);
        }

        internal override DbCommand GetDbCommand(string commandString ) {
            var cmd = new SqlCommand(commandString, (SqlConnection)this.dbconn);

            if (CommandTimeout.HasValue) cmd.CommandTimeout = CommandTimeout.Value;

            return cmd;
        }

        internal override DbDataAdapter GetDbDataAdapter(string selectString) {
            var cmd = new SqlCommand(selectString, (SqlConnection)this.dbconn);

            if (CommandTimeout.HasValue) cmd.CommandTimeout = CommandTimeout.Value;
    
            return new SqlDataAdapter(cmd);
        }

        internal override DbParameter GetEmptyDbParameter()
        {
            return new SqlParameter();
        }

        internal override string GetParameterRef(string propertyName)
        {
            return "@" + propertyName;
        }


        internal override string ConcatString(params string[] myStrings)
        {
            string ConcatedString = "";
            for (int i = 0; i < myStrings.Length; i++)
            {
                    ConcatedString += "ISNULL("+ myStrings[i] + ",'')" + "+";
            }
            return ConcatedString.TrimEnd('+');
        }

    }
}
