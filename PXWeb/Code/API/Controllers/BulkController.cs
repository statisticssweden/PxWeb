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

namespace PXWeb.Code.API.Controllers
{
    [AuthenticationFilter]
    public class BulkController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage CreateBulkFiles(string database, string language)
        {
            var tables = GetTables(database, language);
            string bulkRoot = GetBulkRoot();

            var dbPath = System.IO.Path.Combine(bulkRoot, database);
            var tempPath = System.IO.Path.Combine(dbPath, "temp");
            InitDatabaseFolder(dbPath, tempPath);

            return Request.CreateResponse(HttpStatusCode.OK, $"Bulk files created successfully for database {database}");
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