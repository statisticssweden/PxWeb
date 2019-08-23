using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


using System.Data; // For DataSet-objects.
using System.Data.SqlClient; // For MS SQLServer-connections.

using System.Collections.Specialized;

using System.Data.Common;
using log4net;


namespace PCAxis.Sql.DbClient {

    /// <summary> The (MS)Sql version of MyDbVendor.
    /// </summary>
    internal class MyDbVendorIsSql : MyDbVendor {
        
        private static readonly ILog log = LogManager.GetLogger(typeof(MyDbVendorIsSql));
        private static int? CommandTimeout { get; set; }
       
        internal MyDbVendorIsSql(string connectionString)
            : base(new SqlConnection(connectionString)) {
               mXtraDotForDatatables = ".";
               PrefixIndicatingTempTable = "#";
               KeywordAfterCreateIndicatingTempTable = "";
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

        internal override void BulkInsertIntoTemp(string tableName, IEnumerable<string> columnNames, string sqlString, DbParameter[] parameters, string[] valueCodesArray1, string[] valueCodesArray2, int[] parentCodeCounter)
        {
           
            SqlConnection conn = (SqlConnection)this.dbconn;
            // make sure to enable triggers
            // more on triggers in next post
            SqlBulkCopy bulkCopy =
                new SqlBulkCopy(conn);

            // set the destination table name
            bulkCopy.DestinationTableName = tableName;

            //DataTable dataTable = new DataTable();

            //var columnArray = columnNames.ToArray();

            //dataTable.Columns.Add(columnArray[0], typeof(string));
            //dataTable.Columns.Add(columnArray[1], typeof(string));
            //dataTable.Columns.Add(columnArray[2], typeof(int));

            ////string[] valueCodesArray1, string[] valueCodesArray2, int[] parentCodeCounter

            //for (int i = 0; i < valueCodesArray1.Length; i++)
            //{
            //    var valueCell0 = valueCodesArray1[i];
            //    var valueCell1 = valueCodesArray2[i];
            //    var valueCell2 = parentCodeCounter[i];

            //    DataRow dr = dataTable.NewRow();

            //    dr[0] = valueCell0;
            //    dr[1] = valueCell1;
            //    dr[2] = valueCell2;

            //    dataTable.Rows.Add(dr);
            //}

            // write the data in the "dataTable"
            //bulkCopy.WriteToServer(dataTable);
            bulkCopy.WriteToServer(new BulkValueReader(valueCodesArray1,valueCodesArray2, parentCodeCounter, columnNames));
        }

        internal class BulkValueReader : IDataReader
        {
            private string[] _valueCodesArray1;
            private string[] _valueCodesArray2;
            private int[] _parentCodeCounter;
            private List<string> _columnNames;
            private int currentIndex;

            public BulkValueReader(string[] valueCodesArray1, string[] valueCodesArray2, int[] parentCodeCounter, IEnumerable<string> columnNames)
            {
                currentIndex = -1;
                _valueCodesArray1 = valueCodesArray1;
                _valueCodesArray2 = valueCodesArray2;
                _parentCodeCounter = parentCodeCounter;
                _columnNames = columnNames.ToList();
            }
            public object this[int i]
            {
                get
                {
                    if (i == 0) return _valueCodesArray1[currentIndex];
                    if (i == 1) return _valueCodesArray2[currentIndex];
                    if (i == 2) return _parentCodeCounter[currentIndex];
                    return null;
                }
            }

            public object this[string name]
            {
                get {
                    return this[_columnNames.IndexOf(name)];
                }
            }

            public int Depth => 0;

            public bool IsClosed => false;

            public int RecordsAffected => 0;

            public int FieldCount => 3;

            public void Close()
            {
                //Do nothing
            }

            public void Dispose()
            {
                _valueCodesArray1 = null;
                _valueCodesArray2 = null;
                _parentCodeCounter = null;
            }

            public bool GetBoolean(int i)
            {
                throw new NotImplementedException();
            }

            public byte GetByte(int i)
            {
                throw new NotImplementedException();
            }

            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public char GetChar(int i)
            {
                throw new NotImplementedException();
            }

            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public IDataReader GetData(int i)
            {
                throw new NotImplementedException();
            }

            public string GetDataTypeName(int i)
            {
                if (i == 0 || i == 1) return "string";
                return "int32";
            }

            public DateTime GetDateTime(int i)
            {
                throw new NotImplementedException();
            }

            public decimal GetDecimal(int i)
            {
                throw new NotImplementedException();
            }

            public double GetDouble(int i)
            {
                throw new NotImplementedException();
            }

            public Type GetFieldType(int i)
            {
                if (i == 0 || i == 1) return typeof(string);
                return typeof(int);
            }

            public float GetFloat(int i)
            {
                throw new NotImplementedException();
            }

            public Guid GetGuid(int i)
            {
                throw new NotImplementedException();
            }

            public short GetInt16(int i)
            {
                throw new NotImplementedException();
            }

            public int GetInt32(int i)
            {
                if (i == 2) return _parentCodeCounter[currentIndex];
                throw new NotImplementedException();
            }

            public long GetInt64(int i)
            {
                if (i == 2) return _parentCodeCounter[currentIndex];
                throw new NotImplementedException();
            }

            public string GetName(int i)
            {
                return _columnNames.ElementAt(i);
            }

            public int GetOrdinal(string name)
            {
                return _columnNames.IndexOf(name);
            }

            public DataTable GetSchemaTable()
            {
                throw new NotImplementedException();
            }

            public string GetString(int i)
            {
                if (i == 0) return _valueCodesArray1[currentIndex];
                if (i == 1) return _valueCodesArray2[currentIndex];
                throw new NotImplementedException();
            }

            public object GetValue(int i)
            {
                if (i == 0) return _valueCodesArray1[currentIndex];
                if (i == 1) return _valueCodesArray2[currentIndex];
                if (i == 2) return _parentCodeCounter[currentIndex];
                throw new NotImplementedException();
            }

            public int GetValues(object[] values)
            {
                throw new NotImplementedException();
            }

            public bool IsDBNull(int i)
            {
                throw new NotImplementedException();
            }

            public bool NextResult()
            {
                return false;
            }

            public bool Read()
            {
                currentIndex++;
                return currentIndex < _valueCodesArray1.Length;
            }
        }


    }
}
