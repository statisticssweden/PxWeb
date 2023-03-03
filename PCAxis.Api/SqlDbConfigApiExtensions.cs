using PCAxis.Menu;
using PCAxis.Sql.DbClient;
using PCAxis.Sql;
using PCAxis.Sql.DbConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCAxis.Api
{
    public static class SqlDbConfigApiExtensions
    {
        public static HashSet<string> GetApiPathLookup(this SqlDbConfig DB, string language, string rootNode)
        {
            var pathLookup = new HashSet<string>();

            string sql = "";
            if (DB is SqlDbConfig_21)
            {
                //sql = GetMenuLookupQuery2_1(DB as SqlDbConfig_21, language);
            }
            else if (DB is SqlDbConfig_22)
            {
                //sql = (DB as SqlDbConfig_22).GetMenuLookupQuery2_2(language);
            }
            else if (DB is SqlDbConfig_23)
            {
                sql = (DB as SqlDbConfig_23).GetPathLookupQuery2_3(language);
            }
            else if (DB is SqlDbConfig_24)
            {
                //sql = (DB as SqlDbConfig_24).GetMenuLookupQuery2_4(language);
            }
            else
            {
                return null;
            }

            var cmd = GetPxSqlCommand(DB);

            var dataSet = cmd.ExecuteSelect(sql);

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string key = row[0].ToString().ToUpper();

                if (!pathLookup.Contains(key))
                {
                    pathLookup.Add(key); // Key always uppercase
                }
                else
                {
                    // TODO: Log that this is a duplicate key
                    Console.WriteLine(row[0]);
                }
            }

            if (!pathLookup.Contains(rootNode))
            {
                pathLookup.Add(rootNode);
            }

            return pathLookup;
        }

        public static PxSqlCommand GetPxSqlCommand(SqlDbConfig DB)
        {
            InfoForDbConnection info;

            info = DB.GetInfoForDbConnection(DB.GetDefaultConnString());
            return new PxSqlCommandForTempTables(info.DataBaseType, info.DataProvider, info.ConnectionString);
        }


        private static string GetPathLookupQuery2_3(this SqlDbConfig_23 DB, string language)
        {
            return $@"WITH p(Menu, Selection, pathkey) AS (
                    SELECT
                    Menu,
                    Selection,
                    cast(Selection AS VARCHAR(max)) As pathkey
                    FROM
                    MenuSelection
                    UNION ALL
                    SELECT
                    ms.Menu,
                    ms.Selection,
                    ms.Selection + '/' + p.pathkey
                    FROM
                    MenuSelection ms inner join p on ms.Selection = p.Menu
                    )
                    select 'BE'+ '/' + pathkey from p where Menu = 'BE'"; 

        }


    }
}
