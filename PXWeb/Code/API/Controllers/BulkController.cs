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

namespace PXWeb.Code.API.Controllers
{
    [AuthenticationFilter]
    public class BulkController : ApiController
    {
        internal class FileInfo
        {
            public string TableId { get; set; }
            public DateTime GenerationDate { get; set; }
        }


        [HttpPost]
        public HttpResponseMessage CreateBulkFiles(string database, string language)
        {
            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(database);
            //Validate database and lanuage parameters

            if (!(dbi != null && dbi.HasLanguage(language)))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid parameters");
            }

            var tables = GetTables(database, language);
            string bulkRoot = GetBulkRoot();

            var dbPath = System.IO.Path.Combine(bulkRoot, database);
            var tempPath = System.IO.Path.Combine(dbPath, "temp");
            InitDatabaseFolder(dbPath, tempPath);

            var historyPath = System.IO.Path.Combine(dbPath, "content.json");

            var history = new List<FileInfo>();
            if (File.Exists(historyPath))
            {
                history = JsonConvert.DeserializeObject<List<FileInfo>>(File.ReadAllText(historyPath));
            }

            var serializer = new PCAxis.Paxiom.Csv3FileSerializer();

            foreach (var table in tables)
            {
                string tableId = table.TableId;
                var fileInfo = history.FirstOrDefault(x => x.TableId == tableId);
                var zipPath = System.IO.Path.Combine(dbPath, $"{tableId}.zip");
                if (fileInfo != null)
                {
                    if (table.Published != null &&
                        fileInfo.GenerationDate > table.Published.Value &&
                        File.Exists(zipPath))
                    {
                        continue;
                    }
                }
                else
                {
                    fileInfo = new FileInfo
                    {
                        TableId = tableId,
                    };
                }
                fileInfo.GenerationDate = DateTime.Now;
                var model = GetModel(database, table.ID.Selection, "sv");
                var path = Path.Combine(tempPath, $"{tableId}.csv");
                serializer.Serialize(model, path);
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
                ZipFile.CreateFromDirectory(tempPath, zipPath);
                File.Delete(path);
                history.Add(fileInfo);
            }


            string json = JsonConvert.SerializeObject(history, Formatting.Indented);
            File.WriteAllText(historyPath, json);

            CreateIndexFile(history, bulkRoot);
            return Request.CreateResponse(HttpStatusCode.OK, $"Bulk files created successfully for database {database}");
        }

        private static void CreateIndexFile(List<FileInfo> files, string location)
        {
            var content = new StringBuilder("<!DOCTYPE html><html lang=\"en\">\r\n\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n  <title>Bulk files</title>\r\n</head>\r\n\r\n<body>\r\n  <h1>Bulk files</h1>\r\n");

            foreach (var file in files)
            {
                content.Append($"<a href=\"{file.TableId}.zip\">{file.TableId}.zip</a><br>\r\n");
            }

            content.Append("</body>\r\n\r\n</html>");

            //write content to file
            File.WriteAllText(Path.Combine(location, "index.html"), content.ToString());
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