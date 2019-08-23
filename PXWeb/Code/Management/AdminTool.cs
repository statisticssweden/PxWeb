using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PXWeb.Database;

namespace PXWeb
{
    public class AdminTool
    {
        /// <summary>
        /// Creates menu.xml file for the specified database path
        /// </summary>
        /// <param name="databasePath">path to the database</param>
        /// <param name="langDependent">if files from all language or just the speficifc language should be included in the menu.xml</param>
        /// <returns></returns>
        public static List<DatabaseMessage> GenerateDatabase(string databasePath, bool langDependent)
        {
            return GenerateDatabase(databasePath, langDependent, "FileName");
        }

        /// <summary>
        /// Creates menu.xml file for the specified database path
        /// </summary>
        /// <param name="databasePath">path to the database</param>
        /// <param name="langDependent">if files from all language or just the speficifc language should be included in the menu.xml</param>
        /// <returns></returns>
        public static List<DatabaseMessage> GenerateDatabase(string databasePath, bool langDependent, string sortOrder)
        {
            PXWeb.Database.DatabaseSpider spider;
            spider = new PXWeb.Database.DatabaseSpider();
            spider.Handles.Add(new AliasFileHandler());
            spider.Handles.Add(new LinkFileHandler());
            spider.Handles.Add(new PxFileHandler());
            spider.Handles.Add(new MenuSortFileHandler());

            List<string> langs = new List<string>();
            foreach (LanguageSettings lang in Settings.Current.General.Language.SiteLanguages)
            {
                langs.Add(lang.Name);
            }

            //spider.Builders.Add(new MenuBuilder(Settings.Current.General.Language.SiteLanguages.ToList<string>().ToArray(), langDependent));
            spider.Builders.Add(new MenuBuilder(langs.ToArray(), langDependent) { SortOrder = GetSortOrder(sortOrder) });
            spider.Search(databasePath);

            return spider.Messages;
        }

        private static Func<PCAxis.Paxiom.PXMeta, string, string> GetSortOrder(string sortOrder)
        {
            switch (sortOrder)
            {
                case "Matrix":
                    return (meta, path) => meta.Matrix;
                case "Title":
                    return (meta, path) => !string.IsNullOrEmpty(meta.Description) ? meta.Description : meta.Title;
                case "FileName":
                    return (meta, path) => System.IO.Path.GetFileNameWithoutExtension(path);
                default:
                    break;
            }
            return (meta, path) => path;
        }

        /// <summary>
        /// Resets Language, Px databases and aggregation files currently cached in memmory.
        /// </summary>
        public static void ResetAll()
        {
            //resets the languages
            PCAxis.Paxiom.Localization.PxResourceManager.ResetResourceManager();
            PXWeb.LanguagesSettings langs = (PXWeb.LanguagesSettings)PXWeb.Settings.Current.General.Language;
            langs.ResetLanguages();

            //resets the databases
            PXWeb.DatabasesSettings databases = (PXWeb.DatabasesSettings)PXWeb.Settings.Current.General.Databases;
            databases.ResetDatabases();
            //Reload settings per database
            PXWeb.Settings.Current.LoadDatabaseSettings();

            //resets aggregation information
            PCAxis.Paxiom.GroupRegistry.GetRegistry().ReloadGroupingsAsync();

            //reset/clear saved cache
            PXWeb.Management.SavedQueryPaxiomCache.Current.Reset();

        }
    }
}
