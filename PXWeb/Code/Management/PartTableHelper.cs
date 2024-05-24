using System.Linq;

namespace PXWeb.Management
{
    /// <summary>
    /// Class to support the management of parttables in the url
    /// </summary>
    public class PartTableHelper
    {
        protected string dataBase;
        protected string partTable;
        protected string tableVariable;


        public static System.Data.SqlClient.SqlDataReader GetSubTablesVariables(string db, string table, string partTable)
        {
            System.Data.SqlClient.SqlDataReader rdr = null;
            string @paramHeadTable = table;
            string @paramPartTable = partTable;

            var database = PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[db];
            var tableType = (from tbl in database.Database.Tables
                             where tbl.modelName == "SubTableVariable"
                             select tbl).FirstOrDefault();

            string query = "select $VARIABLE, $VALUE_SET from @SUB_TABLE_VARIABLE" +
                            " where $MAIN_TABLE =  @paramHeadTable  and " +
                            "@SUB_TABLE = @paramPartTable" +
                            " and $VALUE_SET is not null " +
                            " order by $STORE_COLUMN_NO";

            query = query.Replace("$VARIABLE", GetColumn(tableType, "Variable"));
            query = query.Replace("$VALUE_SET", GetColumn(tableType, "ValueSet"));
            query = query.Replace("$MAIN_TABLE", GetColumn(tableType, "MainTable"));
            query = query.Replace("$STORE_COLUMN_NO", GetColumn(tableType, "StoreColumnNo"));

            query = query.Replace("@SUB_TABLE_VARIABLE", tableType.tableName);
            query = query.Replace("@paramPartTable", "'" + @paramPartTable + "'");
            query = query.Replace("@paramHeadTable", "'" + @paramHeadTable + "'");
            query = query.Replace("@SUB_TABLE", GetColumn(tableType, "SubTable"));

            //Connection string
            string connectionString = database.GetDefaultConnString();

            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString);
            con.Open();

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, con);
            rdr = cmd.ExecuteReader();
            return rdr;

        }

        public static string GetColumnName(string db, string tableVariabel)
        {

            var database = PCAxis.Sql.DbConfig.SqlDbConfigsStatic.DataBases[db];
            var tableType = (from tbl in database.Database.Tables
                             where tbl.modelName == "SubTableVariable"
                             select tbl).FirstOrDefault();


            return GetColumn(tableType, tableVariabel);

        }
        private static string GetColumn(PCAxis.Sql.DbConfig.TableType table, string p)
        {
            return (from col in table.Columns
                    where col.modelName == p
                    select col.columnName).FirstOrDefault();
        }

    }
}