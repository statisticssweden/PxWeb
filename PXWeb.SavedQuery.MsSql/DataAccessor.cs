using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCAxis.Query;
using Newtonsoft.Json;

namespace PXWeb.SavedQuery.MsSql
{
    public class DataAccessor : ISavedQueryDatabaseAccessor
    {
        private string _connectionString;

        public DataAccessor()
        {
            if (string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["SavedQueryConnectionString"]))
            {
                throw new System.Configuration.ConfigurationErrorsException("AppSetting SavedQueryConnectionString not set in config file");
            }
            _connectionString = System.Configuration.ConfigurationManager.AppSettings["SavedQueryConnectionString"];
        }

        public PCAxis.Query.SavedQuery Load(int id)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new System.Data.SqlClient.SqlCommand("select QueryText from SavedQueryMeta where QueryId = @queryId", conn);
                cmd.Parameters.AddWithValue("queryId", id);
                string query = cmd.ExecuteScalar() as string;

                PCAxis.Query.SavedQuery sq = JsonHelper.Deserialize<PCAxis.Query.SavedQuery>(query) as PCAxis.Query.SavedQuery;
                return sq;
            }

            return null;
        }

        public int Save(PCAxis.Query.SavedQuery query, int? id)
        {
            query.Stats = null;
            string pxsjson = JsonConvert.SerializeObject(query);

            using (var conn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new System.Data.SqlClient.SqlCommand(
                    @"insert into 
                        SavedQueryMeta
                        (
	                        DataSourceType, 
	                        DatabaseId, 
	                        DataSourceId, 
	                        [Status], 
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
	                        @databaseType,
	                        @databaseId,
	                        @mainTable,
	                        'A',
	                        'P',
	                        'P',
	                        'Anonymous',
	                        @title,
	                        @creationDate,
	                        'PXSJSON',
	                        'D',
	                        @query,
                            0,
	                        0
                        );
                        SELECT @@IDENTITY AS 'Identity';", conn);
                cmd.Parameters.AddWithValue("databaseType", query.Sources[0].Type);
                cmd.Parameters.AddWithValue("databaseId", query.Sources[0].DatabaseId);
                cmd.Parameters.AddWithValue("mainTable", GetMaintable(query.Sources[0]));
                cmd.Parameters.AddWithValue("title", "");
                cmd.Parameters.AddWithValue("creationDate", DateTime.Now);
                cmd.Parameters.AddWithValue("query", pxsjson);
                int newid = Convert.ToInt32(cmd.ExecuteScalar());
                return newid;
            }

            return -1;
        }

        public bool MarkAsRunned(int id)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new System.Data.SqlClient.SqlCommand("update SavedQueryMeta set UsedDate = @lastUsed, Runs = Runs + 1 where QueryId = @queryId", conn);
                cmd.Parameters.AddWithValue("queryId", id);
                cmd.Parameters.AddWithValue("lastUsed", DateTime.Now);
                return cmd.ExecuteNonQuery() == 1;
            }

            return false;
        }

        public bool MarkAsFailed(int id)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new System.Data.SqlClient.SqlCommand("update SavedQueryMeta set UsedDate = @lastUsed, Runs = Runs + 1, Fails = Fails + 1 where QueryId = @queryId", conn);
                cmd.Parameters.AddWithValue("queryId", id);
                cmd.Parameters.AddWithValue("lastUsed", DateTime.Now);
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
