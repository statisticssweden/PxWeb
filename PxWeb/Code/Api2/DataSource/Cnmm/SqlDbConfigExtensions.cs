using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using PCAxis.Menu;
using PCAxis.Paxiom;
using PCAxis.Sql;
using PCAxis.Sql.DbClient;
using PCAxis.Sql.DbConfig;

namespace PxWeb.Code.Api2.DataSource.Cnmm
{
    public static class SqlDbConfigExtensions
    {
        public static Dictionary<string, ItemSelection> GetMenuLookup(this SqlDbConfig DB, string language)
        {
            var menuLookup = new Dictionary<string, ItemSelection>();

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
            ItemSelection itemSelection;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string key = row[2].ToString().ToUpper();
                string menu = row[0].ToString();
                string selection = row[1].ToString();

                if (!menuLookup.ContainsKey(key))
                {
                    itemSelection = new ItemSelection(menu,selection);
                    menuLookup.Add(key, itemSelection); // Key always uppercase
                }
                else
                {
                    // TODO: Log that this is a duplicate key
                    Console.WriteLine(row[0] + " " + row[1]);
                }
            }

            if (!menuLookup.ContainsKey("START"))
            {
                itemSelection = new ItemSelection("START", "START");
                menuLookup.Add("START", itemSelection);
            }

            return menuLookup;
        }

        private static string GetMenuLookupQuery2_1(SqlDbConfig_21 DB, string language)
        {
            // TODO: Check language to avoid SQL injection


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
            // TODO: Check language to avoid SQL injection


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
            // TODO: Check language to avoid SQL injection
            
            if (!DB.isSecondaryLanguage(language))
            {
                return $@"SELECT 
                            {DB.MenuSelection.MenuCol.ForSelect()}, 
                            {DB.MenuSelection.SelectionCol.ForSelect()}, 
                            {DB.MenuSelection.SelectionCol.ForSelect()} 
                        FROM 
                            {DB.MenuSelection.GetNameAndAlias()}
                        WHERE 
                            {DB.MenuSelection.LevelNoCol.Id()} NOT IN (SELECT [Value] FROM {DB.MetaAdm.GetNameAndAlias()} WHERE {DB.MetaAdm.PropertyCol.Id()} = 'MenuLevels') 
                        UNION
                        SELECT 
                            {DB.MenuSelection.MenuCol.ForSelect()}, 
                            {DB.MainTable.MainTableCol.ForSelect()}, 
                            {DB.MainTable.TableIdCol.ForSelect()} 
                        FROM 
                            {DB.MainTable.GetNameAndAlias()} 
                            JOIN {DB.MenuSelection.GetNameAndAlias()} ON {DB.MenuSelection.SelectionCol.Id()} = {DB.MainTable.MainTableCol.Id()}";
            }
            else
            {
                return $@"SELECT 
                            {DB.MenuSelectionLang2.MenuCol.ForSelect(language)}, 
                            {DB.MenuSelectionLang2.SelectionCol.ForSelect(language)}, 
                            {DB.MenuSelectionLang2.SelectionCol.ForSelect(language)}
                        FROM 
                            {DB.MenuSelectionLang2.GetNameAndAlias(language)} 
                            JOIN {DB.MenuSelection.GetNameAndAlias()} ON {DB.MenuSelectionLang2.MenuCol.Id(language)} = {DB.MenuSelection.MenuCol.Id()} AND {DB.MenuSelectionLang2.SelectionCol.Id(language)} = {DB.MenuSelection.SelectionCol.Id()}
                        WHERE 
                            {DB.MenuSelection.LevelNoCol.Id()} NOT IN (SELECT {DB.MetaAdm.ValueCol.Id()} FROM {DB.MetaAdm.GetNameAndAlias()} WHERE {DB.MetaAdm.PropertyCol.Id()} = 'MenuLevels')
                        UNION
                        SELECT 
                            {DB.MenuSelection.MenuCol.ForSelect()}, 
                            {DB.MainTable.MainTableCol.ForSelect()}, 
                            {DB.MainTable.TableIdCol.ForSelect()} 
                        FROM 
                            {DB.MainTable.GetNameAndAlias()} 
                            JOIN {DB.SecondaryLanguage.GetNameAndAlias()} ON {DB.MainTable.MainTableCol.Id()} = {DB.SecondaryLanguage.MainTableCol.Id()} 
                            JOIN {DB.MenuSelection.GetNameAndAlias()} ON {DB.MenuSelection.SelectionCol.Id()} = {DB.MainTable.MainTableCol.Id()}
                        WHERE 
                            {DB.SecondaryLanguage.LanguageCol.Id()} = '{language}' AND
                            {DB.SecondaryLanguage.CompletelyTranslatedCol.Id()} = 'Y'";
            }
        }

        private static string GetMenuLookupQuery2_4(this SqlDbConfig_24 DB, string language)
        {
            // TODO: Check language to avoid SQL injection

            if (!DB.isSecondaryLanguage(language))
            {
                return $@"SELECT 
                            {DB.MenuSelection.MenuCol.ForSelect()}, 
                            {DB.MenuSelection.SelectionCol.ForSelect()}, 
                            {DB.MenuSelection.SelectionCol.ForSelect()} 
                        FROM 
                            {DB.MenuSelection.GetNameAndAlias()}
                        WHERE 
                            {DB.MenuSelection.LevelNoCol.Id()} NOT IN (SELECT [Value] FROM {DB.MetaAdm.GetNameAndAlias()} WHERE {DB.MetaAdm.PropertyCol.Id()} = 'MenuLevels') 
                        UNION
                        SELECT 
                            {DB.MenuSelection.MenuCol.ForSelect()}, 
                            {DB.MainTable.MainTableCol.ForSelect()}, 
                            {DB.MainTable.TableIdCol.ForSelect()} 
                        FROM 
                            {DB.MainTable.GetNameAndAlias()} 
                            JOIN {DB.MenuSelection.GetNameAndAlias()} ON {DB.MenuSelection.SelectionCol.Id()} = {DB.MainTable.MainTableCol.Id()}";
            }
            else
            {
                return $@"SELECT 
                            {DB.MenuSelectionLang2.MenuCol.ForSelect(language)}, 
                            {DB.MenuSelectionLang2.SelectionCol.ForSelect(language)}, 
                            {DB.MenuSelectionLang2.SelectionCol.ForSelect(language)}
                        FROM 
                            {DB.MenuSelectionLang2.GetNameAndAlias(language)} 
                            JOIN {DB.MenuSelection.GetNameAndAlias()} ON {DB.MenuSelectionLang2.MenuCol.Id(language)} = {DB.MenuSelection.MenuCol.Id()} AND {DB.MenuSelectionLang2.SelectionCol.Id(language)} = {DB.MenuSelection.SelectionCol.Id()}
                        WHERE 
                            {DB.MenuSelection.LevelNoCol.Id()} NOT IN (SELECT {DB.MetaAdm.ValueCol.Id()} FROM {DB.MetaAdm.GetNameAndAlias()} WHERE {DB.MetaAdm.PropertyCol.Id()} = 'MenuLevels')
                        UNION
                        SELECT 
                            {DB.MenuSelection.MenuCol.ForSelect()}, 
                            {DB.MainTable.MainTableCol.ForSelect()}, 
                            {DB.MainTable.TableIdCol.ForSelect()} 
                        FROM 
                            {DB.MainTable.GetNameAndAlias()} 
                            JOIN {DB.SecondaryLanguage.GetNameAndAlias()} ON {DB.MainTable.MainTableCol.Id()} = {DB.SecondaryLanguage.MainTableCol.Id()} 
                            JOIN {DB.MenuSelection.GetNameAndAlias()} ON {DB.MenuSelection.SelectionCol.Id()} = {DB.MainTable.MainTableCol.Id()}
                        WHERE 
                            {DB.SecondaryLanguage.LanguageCol.Id()} = '{language}' AND
                            {DB.SecondaryLanguage.CompletelyTranslatedCol.Id()} = 'Y'";
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

