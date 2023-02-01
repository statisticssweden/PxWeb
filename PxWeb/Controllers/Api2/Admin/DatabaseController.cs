using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Px.Abstractions.Interfaces;
using Px.Search;
using PxWeb.Code.Api2.DataSource.PxFile;
using Swashbuckle.AspNetCore.Annotations;

namespace PxWeb.Controllers.Api2.Admin
{
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class DatabaseController : ControllerBase
    {
        private readonly IDataSource _dataSource;
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(IDataSource dataSource, ILogger<DatabaseController> logger)
        {
            _dataSource = dataSource;   
            _logger = logger;
        }

        [HttpPut]
        [Route("/api/v2/admin/database")]
        [SwaggerOperation("Database")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
        [SwaggerResponse(statusCode: 405, description: "Method Not Allowed")]
        public IActionResult Database()
        {
            try
            {
                if (_dataSource.GetType() != typeof(PxFileDataSource))
                {
                    return StatusCode(405, "Only possible to generate database for PX-file databases");
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
