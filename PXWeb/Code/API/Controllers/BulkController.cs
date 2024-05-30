using PXWeb.API;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using PXWeb.Management;
using PCAxis.Menu;
using System.IO;
using System.IO.Compression;
using PCAxis.Paxiom;
using PXWeb.Code.API.Interfaces;
using System.Windows.Forms;

namespace PXWeb.Code.API.Controllers
{
    [AuthenticationFilter]
    public class BulkController : ApiController
    {

        private readonly log4net.ILog _logger;
        private readonly IBulkService _bulkService;

        public BulkController(IBulkService bulkService, log4net.ILog logger)
        {
            _bulkService = bulkService;
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

            if (_bulkService.CreateBulkFilesForDatabase(database, language))
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Bulk files created successfully for database {database}");
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Could not generate bulk files");
        }


    }

      
}