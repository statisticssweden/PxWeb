﻿using PCAxis.Menu;
using PCAxis.Paxiom;
using PCAxis.Sql.DbConfig;
using PXWeb.Code.API.Interfaces;
using PXWeb.Management;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // Added: progress logging controls
        private const int ProgressLogInterval = 50; // tables
        private static readonly TimeSpan ProgressTimeInterval = TimeSpan.FromSeconds(30); // max time between progress logs

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkService"/> class.
        /// </summary>
        /// <param name="registry">The bulk registry.</param>
        /// <param name="tableService">The table service.</param>
        public BulkService(IBulkRegistry registry, ITableService tableService)
        {
            _registry = registry;
            _tableService = tableService;
            _logger = log4net.LogManager.GetLogger(typeof(BulkService));
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
                    _logger.Warn($"No tables found for {database}/{language}.");
                    continue;
                }

                var dbPath = GetDatabasePath(database, language);
                var tempPath = Path.Combine(dbPath, "temp");

                InitDatabaseFolder(dbPath, tempPath);
                _registry.SetContext(dbPath, language);

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
            int total = tables.Count;
            int processed = 0;
            int updated = 0;
            int skippedUnchanged = 0;
            int skippedNullModel = 0;
            int errors = 0;
            var overall = Stopwatch.StartNew();
            var progressTimer = Stopwatch.StartNew();
            _logger.Info($"Bulk start {database}/{language}. Tables to process: {total}");

            foreach (var table in tables)
            {
                processed++;
                try
                {
                    if (!_registry.ShouldTableBeUpdated(table.TableId, table.Published.Value))
                    {
                        skippedUnchanged++;
                    }
                    else
                    {
                        var model = _tableService.GetTableModel(database, table.ID.Selection, language);
                        if (model == null)
                        {
                            skippedNullModel++;
                        }
                        else
                        {
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
                            updated++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors++;
                    _logger.Error($"Error generating bulk file for table {table.TableId} ({processed}/{total}) i {database}/{language}: {ex.Message}", ex);
                }

                // Progress logging (count or time based)
                if (_logger.IsInfoEnabled && (processed % ProgressLogInterval == 0 || progressTimer.Elapsed > ProgressTimeInterval))
                {
                    long managedMem = GC.GetTotalMemory(false) / (1024 * 1024); // MB
                    long procMem = 0;
                    try
                    {
                        procMem = Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024); // MB
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn("Failed to get process memory usage", ex);
                    }
                    int tempFileCount = 0;
                    try
                    {
                        tempFileCount = Directory.Exists(tempPath) ? Directory.GetFiles(tempPath).Length : 0;
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn($"Failed to count temp files in {tempPath}", ex);
                    }
                    _logger.Info($"Progress {database}/{language}: {processed}/{total}. Updated: {updated}, Unchanged: {skippedUnchanged}, NullModel: {skippedNullModel}, Errors: {errors}. ManagedMem(MB): {managedMem}, ProcMem(MB): {procMem}, TempFiles: {tempFileCount}. Elapsed: {overall.Elapsed}.");
                    progressTimer.Restart();
                }
            }

            // Log before Save
            _logger.Info($"Saving registry for {database}/{language}...");
            try
            {
                _registry.Save();
                _logger.Info($"Registry saved for {database}/{language}.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during Save() for {database}/{language}: {ex.Message}", ex);
            }
            try
            {
                _logger.Info($"Bulk complete {database}/{language}. Totalt: {total}, Updated: {updated}, Unchanged: {skippedUnchanged}, NullModel: {skippedNullModel}, Errors: {errors}. Total time: {overall.Elapsed}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during final logging for {database}/{language}: {ex.Message}", ex);
            }
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