using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCAxis.Query;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;


namespace PXWeb.SavedQuery.Oracle
{
    public class DataAccessor : ISavedQueryDatabaseAccessor
    {
        private string _connectionString;

        private string _savedQueryTableOwner;

        public DataAccessor()
        {
            if (string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["SavedQueryConnectionString"]))
            {
                throw new System.Configuration.ConfigurationErrorsException("AppSetting SavedQueryConnectionString not set in config file");
            }
            _connectionString = System.Configuration.ConfigurationManager.AppSettings["SavedQueryConnectionString"];


            if (string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["SavedQueryTableOwner"]))
            {
                throw new System.Configuration.ConfigurationErrorsException("AppSetting SavedQueryTableOwner not set in config file");
            }
            _savedQueryTableOwner = System.Configuration.ConfigurationManager.AppSettings["SavedQueryTableOwner"];
        }

        public PCAxis.Query.SavedQuery Load(int id)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                
                var cmd = new OracleCommand("select QueryText from "+ _savedQueryTableOwner + ".SavedQueryMeta where QueryId = :queryId", conn);
                cmd.Parameters.Add("queryId", id);
                string query = cmd.ExecuteScalar() as string;

                PCAxis.Query.SavedQuery sq = JsonHelper.Deserialize<PCAxis.Query.SavedQuery>(query) as PCAxis.Query.SavedQuery;
                return sq;
            }

            return null;
        }

        public int Save(PCAxis.Query.SavedQuery query, int? id = null)
        {
            query.Stats = null;
            string pxsjson = JsonConvert.SerializeObject(query);

            using (var conn = new OracleConnection(_connectionString))
            {
                string insertSQL = @"BEGIN
                        insert into 
                        {3}.SavedQueryMeta
                        (
                            {0}
                            DataSourceType, 
	                        DatabaseId, 
	                        DataSourceId, 
	                        ""STATUS"", 
	                        StatusUse, 
	                        StatusChange, 
	                        OwnerId, 
	                        MyDescription, 
	                        CreatedDate, 
	                        SavedQueryFormat, 
	                        SavedQueryStorage, 
	                        QueryText,
                            Runs,
                            Fails
                        )
                        values
                        (
                            {1}
	                        :databaseType,
	                        :databaseId,
	                        :mainTable,
	                        'A',
	                        'P',
	                        'P',
	                        'Anonymous',
	                        :title,
	                        sysdate,
	                        'PXSJSON',
	                        'D',
	                        :query,
                            0,
	                        0
                        ) {2};
                        END;";

                string queryIdPartCol = "";
                string queryIdPartValue = "";
                string returningPart = "returning queryid into :identity";

                if (id != null)
                {
                    queryIdPartCol = "QueryId, ";
                    queryIdPartValue = ":queryId, ";
                    returningPart = "";
                }

                insertSQL = string.Format(insertSQL, queryIdPartCol, queryIdPartValue, returningPart,_savedQueryTableOwner);

                conn.Open();
                var cmd = new OracleCommand(insertSQL, conn);
                cmd.BindByName = true;
                cmd.Parameters.Add("databaseType", query.Sources[0].Type);
                cmd.Parameters.Add("databaseId", query.Sources[0].DatabaseId);
                cmd.Parameters.Add("mainTable", GetMaintable(query.Sources[0]));
                cmd.Parameters.Add("title", " ");
                cmd.Parameters.Add("query", OracleDbType.Clob, pxsjson, System.Data.ParameterDirection.Input);
                cmd.Parameters.Add("identity", OracleDbType.Int16, System.Data.ParameterDirection.ReturnValue);

                if (id != null)
                {
                    cmd.Parameters.Add("queryId", id.Value);
                }

                cmd.ExecuteNonQuery();

                if (id == null)
                {
                    int newId = int.Parse(cmd.Parameters["identity"].Value.ToString());
                    return newId;
                }
                else
                {
                    return id.Value;
                }
            }

            return -1;
        }

        public bool MarkAsRunned(int id)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var cmd = new OracleCommand("update "+ _savedQueryTableOwner+".SavedQueryMeta set UsedDate = sysdate, Runs = Runs + 1 where QueryId = :queryId", conn);
                cmd.Parameters.Add("queryId", id);
                
                return cmd.ExecuteNonQuery() == 1;
            }

            return false;
        }

        public bool MarkAsFailed(int id)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var cmd = new OracleCommand("update " + _savedQueryTableOwner + ".SavedQueryMeta set UsedDate = sysdate, Runs = Runs + 1, Fails = Fails + 1 where QueryId = :queryId", conn);
                cmd.Parameters.Add("queryId", id);
                
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        private string GetMaintable(TableSource source)
        {
            if (source.Type == "PX")
            {
                return source.Source;
            }
            else if (source.Type == "CNMM")
            {
                return source.Source.Split('/').Last();
            }

            return source.Source;
        }
    }
}
