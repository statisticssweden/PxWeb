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

namespace PXWeb.Code.API.Services
{
    public class BulkService : IBulkService
    {
        private readonly IBulkRegistry _registry;
        public BulkService(IBulkRegistry registry)
        {
            _registry = registry;
        }

        public bool CreateBulkFilesForDatabase(string database, string language)
        {
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

            return true;
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