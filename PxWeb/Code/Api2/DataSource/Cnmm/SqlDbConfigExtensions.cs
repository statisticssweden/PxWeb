using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using PCAxis.Sql;
using PCAxis.Sql.DbClient;
using PCAxis.Sql.DbConfig;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public static class SqlDbConfigExtensions
    {
        public static Dictionary<string, string> GetMenuLookup(this SqlDbConfig DB, string language)
        {
            var menuLookup = new Dictionary<string, string>();

            string sql;
            if (DB is SqlDbConfig_21)
            {
                sql = GetMenuLookupQuery2_1(DB as SqlDbConfig_21, language);
            }
            else if (DB is SqlDbConfig_22)
            {
                sql = (DB as SqlDbConfig_22).GetMenuLookupQuery2_2(language);
            }
            else if (DB is SqlDbConfig_23)
            {
                sql = (DB as SqlDbConfig_23).GetMenuLookupQuery2_3(language);
            }
            else if (DB is SqlDbConfig_24)
            {
                sql = (DB as SqlDbConfig_24).GetMenuLookupQuery2_4(language);
            }
            else
            {
                return null;
            }

            var cmd = GetPxSqlCommand(DB);

            var dataSet = cmd.ExecuteSelect(sql);

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string key = row[1].ToString().ToUpper();
                
                if (!menuLookup.ContainsKey(key))
                {
                    menuLookup.Add(key, row[0] as string); // Key always uppercase
                }
                else
                {
                    // TODO: Log that this is a duplicate key
                    Console.WriteLine(row[0] + " " + row[1]);
                }
            }

            if (!menuLookup.ContainsKey("START"))
            {
                menuLookup.Add("START", "START"); 
            }

            return menuLookup;
        }

        private static string GetMenuLookupQuery2_1(SqlDbConfig_21 DB, string language)
        {
            if (!DB.isSecondaryLanguage(language))
            {
                return $@"SELECT {DB.MenuSelection.MenuCol.ForSelect()}, {DB.MenuSelection.SelectionCol.ForSelect()} FROM {DB.MenuSelection.GetNameAndAlias()}";
            }
            else
            {
                return $@"SELECT {DB.MenuSelectionLang2.MenuCol.ForSelect(language)}, {DB.MenuSelectionLang2.SelectionCol.ForSelect(language)} FROM {DB.MenuSelectionLang2.GetNameAndAlias(language)}";
            }
        }

        private static string GetMenuLookupQuery2_2(this SqlDbConfig_22 DB, string language)
        {
            if (!DB.isSecondaryLanguage(language))
            {
                return $@"SELECT {DB.MenuSelection.MenuCol.ForSelect()}, {DB.MenuSelection.SelectionCol.ForSelect()} FROM {DB.MenuSelection.GetNameAndAlias()}";
            }
            else
            {
                return $@"SELECT {DB.MenuSelectionLang2.MenuCol.ForSelect(language)}, {DB.MenuSelectionLang2.SelectionCol.ForSelect(language)} FROM {DB.MenuSelectionLang2.GetNameAndAlias(language)}";
            }
        }

        private static string GetMenuLookupQuery2_3(this SqlDbConfig_23 DB, string language)
        {
            if (!DB.isSecondaryLanguage(language))
            {
                return $@"SELECT {DB.MenuSelection.MenuCol.ForSelect()}, {DB.MenuSelection.SelectionCol.ForSelect()} FROM {DB.MenuSelection.GetNameAndAlias()}";
            }
            else
            {
                return $@"SELECT {DB.MenuSelectionLang2.MenuCol.ForSelect(language)}, {DB.MenuSelectionLang2.SelectionCol.ForSelect(language)} FROM {DB.MenuSelectionLang2.GetNameAndAlias(language)}";
            }
        }

        private static string GetMenuLookupQuery2_4(this SqlDbConfig_24 DB, string language)
        {
            if (!DB.isSecondaryLanguage(language))
            {
                return $@"SELECT {DB.MenuSelection.MenuCol.ForSelect()}, {DB.MenuSelection.SelectionCol.ForSelect()} FROM {DB.MenuSelection.GetNameAndAlias()}";
            }
            else
            {
                return $@"SELECT {DB.MenuSelectionLang2.MenuCol.ForSelect(language)}, {DB.MenuSelectionLang2.SelectionCol.ForSelect(language)} FROM {DB.MenuSelectionLang2.GetNameAndAlias(language)}";
            }
        }


        public static PxSqlCommand GetPxSqlCommand(SqlDbConfig DB)
        {
            InfoForDbConnection info;

            info = DB.GetInfoForDbConnection(DB.GetDefaultConnString());
            return new PxSqlCommandForTempTables(info.DataBaseType, info.DataProvider, info.ConnectionString);
        }

    }
}

