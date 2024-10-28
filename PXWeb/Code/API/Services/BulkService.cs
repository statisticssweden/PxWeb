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
        /// Creates bulk files for a database.
        /// The files are created in the bulk folder of the database. 
        /// One zip file is created for each table in the database.
        /// The zip file contains a CSV file with the table data.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="language">The language.</param>
        /// <returns><c>true</c> if the bulk files are created successfully; otherwise, <c>false</c>.</returns>
        public bool CreateBulkFilesForDatabase(string database)
        {
            var languages = GetLanguagesForBulkFiles();


            //Make sure we have languages to create bulk files for
            if (languages == null || languages.Count == 0)
            {
                _logger.Error("No languages found in databases.config");
                return false;
            }

            foreach (var language in languages)
            {

                var tables = _tableService.GetAllTables(database, language)
                                       .OrderBy(table => table.Text)
                                           .ToList();
                var bulkRoot = GetBulkRoot();                
                var dbPath = System.IO.Path.Combine(bulkRoot, database,language);
                var tempPath = System.IO.Path.Combine(dbPath, "temp");

                InitDatabaseFolder(dbPath, tempPath);
                _registry.SetContext(dbPath);
                _registry.SetLang(language);

                var serializer = new PCAxis.Paxiom.Csv2FileSerializer();
#if DEBUG
                if (tables != null && tables.Count > 10)
                {
                tables = tables.Take(10).ToList();
                }
#endif
                foreach (var table in tables)
                {
                    string tableId = table.TableId;
                    string tableText = table.Text;
                if (!_registry.ShouldTableBeUpdated(tableId, table.Published.Value))
                {
                    continue;
                }

                    var zipPath = System.IO.Path.Combine(dbPath, $"{tableId}_{language}.zip");
                    var generationDate = DateTime.Now;

                    var model = _tableService.GetTableModel(database, table.ID.Selection, language);

                    //Make sure we got a model
                    if (model == null)
                    {
                        continue;
                    }

                    var path = Path.Combine(tempPath, $"{tableId}_{language}.csv");
                    serializer.Serialize(model, path);

                    if (File.Exists(zipPath))
                    {
                        File.Delete(zipPath);
                    }

                    ZipFile.CreateFromDirectory(tempPath, zipPath);
                    File.Delete(path);

                    _registry.RegisterTableBulkFileUpdated(tableId, tableText, generationDate);
                }

                _registry.Save(language);
            }

            return true;
                
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

        /// <summary>
        /// Gets languages from the configuration file for input to creating bulk files.        
        /// </summary>
        /// <returns>List of languages</returns>
        public List<string> GetLanguagesForBulkFiles()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var configFilePath = System.IO.Path.Combine(path, "databases.config");

            var languages = new List<string>();

            try
            {
                // Load file
                var doc = XDocument.Load(configFilePath);

                // Get "DatabaseSet" element and "language" attribut
                var languageElements = doc.Descendants("DatabaseSet")
                                          .Attributes("language");

                // Extract languagecodes to a list
                languages = languageElements.Select(e => e.Value).ToList();
            }
            catch (Exception ex)
            {
               
                 _logger.Error("Error reading languages from config file", ex);
            }

            return languages;
        }

        
               
    

    }

}