using PXWeb.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using PXWeb.Management;
using PCAxis.Menu;
using PCAxis.Paxiom.Localization;
using PCAxis.Sql.DbConfig;
using System.IO;
using Newtonsoft.Json;
using System.IO.Compression;
using PCAxis.Paxiom;
using System.Text;
using PCAxis.Web.Core.Enums;
using log4net.Repository.Hierarchy;
using PXWeb.Code.API.Interfaces;

namespace PXWeb.Code.API.Controllers
{
    [AuthenticationFilter]
    public class BulkController : ApiController
    {

        private readonly log4net.ILog _logger;
        private readonly IBulkRegistry _registry;

        public BulkController(IBulkRegistry registry, log4net.ILog logger)
        {
            _registry = registry;
            _logger = logger;
        }

        [HttpPost]
        public HttpResponseMessage CreateBulkFiles(string database, string language)
        {
            _logger.Info($"CreateBulkFiles - started for database {database}");

            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(database);
            //Validate database and lanuage parameters
            if (!(dbi != null && dbi.HasLanguage(language)))
            {
                _logger.Warn($"Invalid parameters: database={database}, language={language}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid parameters");
            }

            var tables = GetTables(database, language);
            var bulkRoot = GetBulkRoot();
            var dbPath = System.IO.Path.Combine(bulkRoot, database);
            var tempPath = System.IO.Path.Combine(dbPath, "temp");

            InitDatabaseFolder(dbPath, tempPath);
            _registry.SetContext(dbPath);
            var serializer = new PCAxis.Paxiom.Csv3FileSerializer();

            foreach (var table in tables)
            {
                string tableId = table.TableId;
                if (!_registry.ShouldTableBeUpdated(tableId, table.Published.Value))
                {
                    continue;
                }

                var zipPath = System.IO.Path.Combine(dbPath, $"{tableId}.zip");
                var generationDate = DateTime.Now;
                

                var model = GetModel(database, table.ID.Selection, language);
                var path = Path.Combine(tempPath, $"{tableId}.csv");
                serializer.Serialize(model, path);
                
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }

                ZipFile.CreateFromDirectory(tempPath, zipPath);
                File.Delete(path);

                _registry.RegisterTableBulkFileUpdated(tableId, generationDate);
            }

            _registry.Save();

            return Request.CreateResponse(HttpStatusCode.OK, $"Bulk files created successfully for database {database}");

            
        }







        private static PXModel GetModel(string database, string id, string language)
        {
            var builder = PxContext.CreatePaxiomBuilder(database, id);
            builder.SetPreferredLanguage(language);
            builder.BuildForSelection();
            builder.BuildForPresentation(PCAxis.Paxiom.Selection.SelectAll(builder.Model.Meta));
            return builder.Model;

        }

        private static string GetBulkRoot()
        {
            var path = System.Web.HttpContext.Current.Server.MapPath(Settings.Current.General.Paths.PxDatabasesPath);
            if (path.EndsWith("\\"))
            {
                path = System.IO.Path.GetDirectoryName(path);
            }
            var bulkRoot = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "bulk");
            return bulkRoot;
        }

        private static void InitDatabaseFolder(string dbPath, string tempPath)
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

        static List<TableLink> GetTables(string database, string language)
        {
            var root = PxContext.GetMenu(database, "", language, 10);
            var tables = new List<TableLink>();
            AddTables(root.CurrentItem, tables);

            return tables;
        }
        
        static void AddTables(PCAxis.Menu.Item item, List<TableLink> tables)
        {
            if (item is PxMenuItem)
            {
                foreach (var child in ((PxMenuItem)item).SubItems)
                {
                    AddTables(child, tables);
                }
            }
            else if (item is TableLink)
            {
                tables.Add((TableLink)item);
            }
        }
    }
}