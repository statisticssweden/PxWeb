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

namespace PXWeb.Code.API.Controllers
{
    [AuthenticationFilter]
    public class BulkController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage CreateBulkFiles(string database, string language)
        {
            var tables = GetTables(database, language);



            return Request.CreateResponse(HttpStatusCode.OK, $"Bulk files created successfully for database {database}");
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