using PXWeb.API;
using PXWeb.Code.API.Interfaces;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PXWeb.Code.API.Controllers
{
    [AuthenticationFilter]
    public class BulkController : ApiController
    {
        private readonly log4net.ILog _logger;
        private readonly IBulkService _bulkService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkController"/> class.
        /// </summary>
        /// <param name="bulkService">The bulk service.</param>
        /// <param name="logger">The logger.</param>
        public BulkController(IBulkService bulkService, log4net.ILog logger)
        {
            _bulkService = bulkService;
            _logger = logger;
        }

        /// <summary>
        /// Creates bulk files for the specified database and language.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="language">The language. This sould be the defult language to make sure that all tables are included</param>
        /// <returns>The HTTP response message indicating the result of the operation.</returns>
        [HttpPost]
        public HttpResponseMessage CreateBulkFiles(string database)
        {
            _logger.Info($"CreateBulkFiles - started for database {database}");

            DatabaseInfo dbi = PXWeb.Settings.Current.General.Databases.GetDatabase(database);
            //Validate database and language parameters
            if (!(dbi != null ))
            {
                _logger.Warn($"Invalid parameter: database={database}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid parameter");
            }

            if (_bulkService.CreateBulkFilesForDatabase(database))
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Bulk files created successfully for database {database}");
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Could not generate bulk files");
        }
    }


}