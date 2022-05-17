using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PxWeb.Attributes.Api2;
using PxWeb.Config.Api2;
using PxWeb.Models.Api2;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace PxWeb.Controllers.Api2
{
    [ApiController]
    public class ConfigurationApiController : ControllerBase
    {
        private readonly IPxApiConfigurationService _pxApiConfigurationService;
        private readonly ILogger<ConfigurationApiController> _logger;

        public ConfigurationApiController(IPxApiConfigurationService pxApiConfigurationService, ILogger<ConfigurationApiController> logger)
        {
            _pxApiConfigurationService = pxApiConfigurationService;
            _logger = logger;
        }

        /// <summary>
        /// Get PxApi configuration settings
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="429">Error respsone for 429</response>
        [HttpGet]
        [Route("/v2/config")]
        [ValidateModelState]
        [SwaggerOperation("GetConfiguration")]
        [SwaggerResponse(statusCode: 200, type: typeof(PxApiConfigurationOptions), description: "Success")]
        [SwaggerResponse(statusCode: 429, type: typeof(Problem), description: "Error respsone for 429")]
        public virtual IActionResult GetConfiguration()
        {
            ////TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            //// return StatusCode(200, default(Folder));

            ////TODO: Uncomment the next line to return response 429 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            //// return StatusCode(429, default(Problem));
            try
            {
                var op = _pxApiConfigurationService.GetConfiguration();


                return new ObjectResult(op);
            }
            catch (NullReferenceException ex) {
                _logger.LogError("GetConfigtion caused an exception", ex);
            }
            return StatusCode(500, new Problem() { Status = 500, Title = "Something went wrong fetching the API configuration", Type = "https://TODO/ConfigError", });
        }

    }
}
