using PCAxis.Sql;
using PCAxis.Sql.DbClient;
using PCAxis.Sql.DbConfig;
using System.Collections.Generic;
using System.Data;

namespace PCAxis.Api
{
    public static class SqlDbConfigApiExtensions
    {
        public static HashSet<string> GetApiPathLookup(this SqlDbConfig DB, string rootNode)
        {
            var pathLookup = new HashSet<string>();
            string sql = "";
            var cmd = GetPxSqlCommand(DB);

            if (DB is SqlDbConfig_21)
            {
                sql = GetPathLookupQuery2_1(DB as SqlDbConfig_21, cmd);
            }
            else if (DB is SqlDbConfig_22)
            {
                sql = (DB as SqlDbConfig_22).GetPathLookupQuery2_2(cmd);
            }
            else if (DB is SqlDbConfig_23)
            {
                sql = (DB as SqlDbConfig_23).GetPathLookupQuery2_3(cmd);
            }
            else if (DB is SqlDbConfig_24)
            {
                sql = (DB as SqlDbConfig_24).GetPathLookupQuery2_4(cmd);
            }
            else
            {
                return null;
            }

            var dataSet = cmd.ExecuteSelect(sql);

            // Add all legal paths to lookup
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string key = row[0].ToString().ToUpper(); // Key always uppercase

                if (!pathLookup.Contains(key))
                {
                    pathLookup.Add(key);
                }
            }

            // Add rootnode to lookup
            if (!pathLookup.Contains(rootNode))
            {
                pathLookup.Add(rootNode);
            }

            return pathLookup;
        }

        private static PxSqlCommand GetPxSqlCommand(SqlDbConfig DB)
        {
            InfoForDbConnection info;

            info = DB.GetInfoForDbConnection(DB.GetDefaultConnString());
            return new PxSqlCommandForTempTables(info.DataBaseType, info.DataProvider, info.ConnectionString);
        }

        private static string GetPathLookupQuery2_1(this SqlDbConfig_21 DB, PxSqlCommand cmd)
        {
            return $@"WITH p({DB.MenuSelection.MenuCol.PureColumnName()}, {DB.MenuSelection.SelectionCol.PureColumnName()}, pathkey) AS (
                    SELECT
                        {DB.MenuSelection.MenuCol.Id()},
                        {DB.MenuSelection.SelectionCol.Id()},
                        cast({DB.MenuSelection.SelectionCol.Id()} AS VARCHAR(1000)) As pathkey
                    FROM
                        {DB.MenuSelection.GetNameAndAlias()}
                    UNION ALL
                    SELECT
                        {DB.MenuSelection.MenuCol.Id()},
                        {DB.MenuSelection.SelectionCol.Id()},
                        cast({cmd.getConcatString(DB.MenuSelection.SelectionCol.Id(), "'/'", "p.pathkey")} AS VARCHAR(1000)) 
                    FROM
                       {DB.MenuSelection.GetNameAndAlias()} inner join p on {DB.MenuSelection.SelectionCol.Id()} = p.{DB.MenuSelection.MenuCol.PureColumnName()}
                    )
                    select {cmd.getConcatString("'" + CnmmDatabaseApiRootHelper.DatabaseRoot + "/'", "pathkey")} from p where {DB.MenuSelection.MenuCol.PureColumnName()} = '{CnmmDatabaseApiRootHelper.DatabaseRoot}'";
        }

        private static string GetPathLookupQuery2_2(this SqlDbConfig_22 DB, PxSqlCommand cmd)
        {
            return $@"WITH p({DB.MenuSelection.MenuCol.PureColumnName()}, {DB.MenuSelection.SelectionCol.PureColumnName()}, pathkey) AS (
                    SELECT
                        {DB.MenuSelection.MenuCol.Id()},
                        {DB.MenuSelection.SelectionCol.Id()},
                        cast({DB.MenuSelection.SelectionCol.Id()} AS VARCHAR(1000)) As pathkey
                    FROM
                        {DB.MenuSelection.GetNameAndAlias()}
                    UNION ALL
                    SELECT
                        {DB.MenuSelection.MenuCol.Id()},
                        {DB.MenuSelection.SelectionCol.Id()},
                        cast({cmd.getConcatString(DB.MenuSelection.SelectionCol.Id(), "'/'", "p.pathkey")} AS VARCHAR(1000)) 
                    FROM
                       {DB.MenuSelection.GetNameAndAlias()} inner join p on {DB.MenuSelection.SelectionCol.Id()} = p.{DB.MenuSelection.MenuCol.PureColumnName()}
                    )
                    select {cmd.getConcatString("'" + CnmmDatabaseApiRootHelper.DatabaseRoot + "/'", "pathkey")} from p where {DB.MenuSelection.MenuCol.PureColumnName()} = '{CnmmDatabaseApiRootHelper.DatabaseRoot}'";
        }

        private static string GetPathLookupQuery2_3(this SqlDbConfig_23 DB, PxSqlCommand cmd)
        {
            return $@"WITH p({DB.MenuSelection.MenuCol.PureColumnName()}, {DB.MenuSelection.SelectionCol.PureColumnName()}, pathkey) AS (
                    SELECT
                        {DB.MenuSelection.MenuCol.Id()},
                        {DB.MenuSelection.SelectionCol.Id()},
                        cast({DB.MenuSelection.SelectionCol.Id()} AS VARCHAR(1000)) As pathkey
                    FROM
                        {DB.MenuSelection.GetNameAndAlias()}
                    UNION ALL
                    SELECT
                        {DB.MenuSelection.MenuCol.Id()},
                        {DB.MenuSelection.SelectionCol.Id()},
                        cast({cmd.getConcatString(DB.MenuSelection.SelectionCol.Id(), "'/'", "p.pathkey")} AS VARCHAR(1000)) 
                    FROM
                       {DB.MenuSelection.GetNameAndAlias()} inner join p on {DB.MenuSelection.SelectionCol.Id()} = p.{DB.MenuSelection.MenuCol.PureColumnName()}
                    )
                    select {cmd.getConcatString("'" + CnmmDatabaseApiRootHelper.DatabaseRoot + "/'", "pathkey")} from p where {DB.MenuSelection.MenuCol.PureColumnName()} = '{CnmmDatabaseApiRootHelper.DatabaseRoot}'";
        }

        private static string GetPathLookupQuery2_4(this SqlDbConfig_24 DB, PxSqlCommand cmd)
        {
            return $@"WITH p({DB.MenuSelection.MenuCol.PureColumnName()}, {DB.MenuSelection.SelectionCol.PureColumnName()}, pathkey) AS (
                    SELECT
                        {DB.MenuSelection.MenuCol.Id()},
                        {DB.MenuSelection.SelectionCol.Id()},
                        cast({DB.MenuSelection.SelectionCol.Id()} AS VARCHAR(1000)) As pathkey
                    FROM
                        {DB.MenuSelection.GetNameAndAlias()}
                    UNION ALL
                    SELECT
                        {DB.MenuSelection.MenuCol.Id()},
                        {DB.MenuSelection.SelectionCol.Id()},
                        cast({cmd.getConcatString(DB.MenuSelection.SelectionCol.Id(), "'/'", "p.pathkey")} AS VARCHAR(1000)) 
                    FROM
                       {DB.MenuSelection.GetNameAndAlias()} inner join p on {DB.MenuSelection.SelectionCol.Id()} = p.{DB.MenuSelection.MenuCol.PureColumnName()}
                    )
                    select {cmd.getConcatString("'" + CnmmDatabaseApiRootHelper.DatabaseRoot + "/'", "pathkey")} from p where {DB.MenuSelection.MenuCol.PureColumnName()} = '{CnmmDatabaseApiRootHelper.DatabaseRoot}'";
        }

    }
}
