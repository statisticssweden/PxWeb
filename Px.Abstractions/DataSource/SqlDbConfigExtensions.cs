using PCAxis.Sql;
using PCAxis.Sql.DbClient;
using PCAxis.Sql.DbConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px.Abstractions.DataSource
{
    public static class SqlDbConfigExtensions
    {
        public static Dictionary<string, string> GetMenuLookup(this SqlDbConfig DB)
        {
            var menuLookup = new Dictionary<string, string>();

            string sql;
            if (DB is SqlDbConfig_21)
            {
                sql = GetMenuLookupQuery2_1(DB as SqlDbConfig_21);
            }
            else if (DB is SqlDbConfig_22)
            {
                sql = GetMenuLookupQuery2_2(DB as SqlDbConfig_22);
            }
            else if (DB is SqlDbConfig_23)
            {
                sql = GetMenuLookupQuery2_3(DB as SqlDbConfig_23);
            }
            else if (DB is SqlDbConfig_24)
            {
                sql = GetMenuLookupQuery2_4(DB as SqlDbConfig_24);
            }
            else
            {
                return null;
            }

            var cmd = GetPxSqlCommand(DB);

            var dataSet = cmd.ExecuteSelect(sql);

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                if (menuLookup.ContainsKey(row[1] as string))
                {
                    Console.WriteLine(row[0] + " " + row[1]);
                }
                else
                {
                    menuLookup.Add(row[1] as string, row[0] as string);
                }
            }

            return menuLookup;
        }

        private static string GetMenuLookupQuery2_1(SqlDbConfig_21 DB)
        {
            return $@"SELECT {DB.MenuSelection.MenuCol.ForSelect()}, {DB.MenuSelection.SelectionCol.ForSelect()} FROM {DB.MenuSelection.GetNameAndAlias()}";
        }

        private static string GetMenuLookupQuery2_2(this SqlDbConfig_22 DB)
        {
            return $@"SELECT {DB.MenuSelection.MenuCol.ForSelect()}, {DB.MenuSelection.SelectionCol.ForSelect()} FROM {DB.MenuSelection.GetNameAndAlias()}";
        }

        private static string GetMenuLookupQuery2_3(this SqlDbConfig_23 DB)
        {
            return $@"SELECT {DB.MenuSelection.MenuCol.ForSelect()}, {DB.MenuSelection.SelectionCol.ForSelect()} FROM {DB.MenuSelection.GetNameAndAlias()}";
        }

        private static string GetMenuLookupQuery2_4(this SqlDbConfig_24 DB)
        {
            return $@"SELECT {DB.MenuSelection.MenuCol.ForSelect()}, {DB.MenuSelection.SelectionCol.ForSelect()} FROM {DB.MenuSelection.GetNameAndAlias()}";
        }


        public static PxSqlCommand GetPxSqlCommand(SqlDbConfig DB)
        {
            InfoForDbConnection info;

            info = DB.GetInfoForDbConnection(DB.GetDefaultConnString());
            return new PxSqlCommandForTempTables(info.DataBaseType, info.DataProvider, info.ConnectionString);
        }

    }
}

