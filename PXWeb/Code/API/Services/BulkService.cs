using PCAxis.Menu;
using PCAxis.Paxiom;
using PCAxis.Sql.DbConfig;
using PXWeb.Code.API.Interfaces;
using PXWeb.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace PXWeb.Code.API.Services
{
    public class BulkService : IBulkService
    {
        private readonly IBulkRegistry _registry;
        private readonly ITableService _tableService;
        private readonly log4net.ILog _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkService"/> class.
        /// </summary>
        /// <param name="registry">The bulk registry.</param>
        /// <param name="tableService">The table service.</param>
        public BulkService(IBulkRegistry registry, ITableService tableService)
        {
            _registry = registry;
            _tableService = tableService;
        }

        /// <summary>
        /// Creates bulk files for a database for every active language.
        /// The files are created in the bulk folder of the database in separate language folders. 
        /// One zip file is created for each table in the database.
        /// The zip file contains a CSV file with the table data.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <returns><c>true</c> if the bulk files are created successfully; otherwise, <c>false</c>.</returns>
        public bool CreateBulkFilesForDatabase(string database)
        {
            var languages = GetSiteLanguages();
            if (!DatabaseHasLanguages(database, languages))
            {
                _logger.Error($"Database {database} does not support the required languages.");
                return false;
            }

            foreach (var language in languages)
            {
                var tables = GetTablesForLanguage(database, language);
                if (tables == null || !tables.Any())
                {
                    continue;
                }

                var dbPath = GetDatabasePath(database, language);
                var tempPath = Path.Combine(dbPath, "temp");

                InitDatabaseFolder(dbPath, tempPath);
                _registry.SetContext(dbPath,language);                

                ProcessTables(database, language, tables, tempPath, dbPath);
            }

            return true;
        }

        private List<string> GetSiteLanguages()
        {
            return PXWeb.Settings.Current.General.Language.SiteLanguages.Select(l => l.Name).ToList();
        }

        private bool DatabaseHasLanguages(string database, List<string> languages)
        {
            var db = PXWeb.Settings.Current.General.Databases.GetDatabase(database);
            foreach (var lang in languages)
            {
                if (!db.HasLanguage(lang))
                {
                    return false;
                }
            }
            return true;
        }

        private List<TableLink> GetTablesForLanguage(string database, string language)
        {
            return _tableService.GetAllTables(database, language)
                                .OrderBy(table => table.Text)
                                .ToList();
        }

        private string GetDatabasePath(string database, string language)
        {
            var bulkRoot = GetBulkRoot();
            return Path.Combine(bulkRoot, database, language);
        }

        private void ProcessTables(string database, string language, List<TableLink> tables, string tempPath, string dbPath)
        {
            var serializer = new PCAxis.Paxiom.Csv2FileSerializer();

#if DEBUG
            if (tables.Count > 10)
            {
                tables = tables.Take(10).ToList();
            }
#endif

            foreach (var table in tables)
            {
                if (!_registry.ShouldTableBeUpdated(table.TableId, table.Published.Value))
                {
                    continue;
                }

                var model = _tableService.GetTableModel(database, table.ID.Selection, language);
                if (model == null)
                {
                    continue;
                }

                var csvPath = Path.Combine(tempPath, $"{table.TableId}_{language}.csv");
                serializer.Serialize(model, csvPath);

                var zipPath = Path.Combine(dbPath, $"{table.TableId}_{language}.zip");
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }

                ZipFile.CreateFromDirectory(tempPath, zipPath);
                File.Delete(csvPath);

                _registry.RegisterTableBulkFileUpdated(table.TableId, table.Text, DateTime.Now);
            }

            _registry.Save();
        }

        /// <summary>
        /// Gets the root path for the bulk files.
        /// It is one level above the database folder.
        /// </summary>
        /// <returns>The root path for the bulk files.</returns>
        private string GetBulkRoot()
        {
            var path = System.Web.HttpContext.Current.Server.MapPath(Settings.Current.General.Paths.PxDatabasesPath);
            if (path.EndsWith("\\"))
            {
                path = System.IO.Path.GetDirectoryName(path);
            }
            var bulkRoot = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "bulk");
            return bulkRoot;
        }

        /// <summary>
        /// Initializes the database folder for bulk operations.
        /// It creates a folder for the database and a temporary folder that is used during the generation of the bulk files.
        /// </summary>
        /// <param name="dbPath">The path of the database folder.</param>
        /// <param name="tempPath">The path of the temporary folder.</param>
        private void InitDatabaseFolder(string dbPath, string tempPath)
        {
            if (!System.IO.Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
                Directory.CreateDirectory(tempPath);
            }
            else
            {
                if (!System.IO.Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                {
                    // Clear temp folder    
                    var tempFiles = Directory.GetFiles(tempPath);
                    foreach (var file in tempFiles)
                    {
                        System.IO.File.Delete(file);
                    }
                }
            }
        }       

    }

}