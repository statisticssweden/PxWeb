using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PxWeb.Api2.Server.Models;
using PxWeb.Attributes.Api2;
using PxWeb.Config.Api2;
using PxWeb.Models.Api2;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ConfigResponse = PxWeb.Models.Api2.ConfigResponse;
using Language = PxWeb.Models.Api2.Language;

namespace PxWeb.Controllers.Api2
{
    [ApiController]
    public class AdministrationApiController : PxWeb.Api2.Server.Controllers.AdministrationApiController
    {
        private readonly IPxApiConfigurationService _pxApiConfigurationService;
        private readonly ILogger<AdministrationApiController> _logger;

        public AdministrationApiController(IPxApiConfigurationService pxApiConfigurationService, ILogger<AdministrationApiController> logger)
        {
            _pxApiConfigurationService = pxApiConfigurationService;
            _logger = logger;
        }

        /// <summary>Get API configuration settings.</summary>
        /// <response code="200">Success</response>
        /// <response code="400">Error respsone for 400</response>
        /// <response code="404">Error respsone for 404</response>
        /// <response code="429">Error respsone for 429</response>
        [HttpGet]
        [Route("/api/v2/config")]
        [ValidateModelState]
        [SwaggerOperation("GetApiConfig", null)]
        [SwaggerResponse(200, "Success", typeof(ConfigResponse))]
        [SwaggerResponse(400, "Error respsone for 400", typeof(Problem))]
        [SwaggerResponse(404, "Error respsone for 404", typeof(Problem))]
        [SwaggerResponse(429, "Error respsone for 429", typeof(Problem))]

        public override IActionResult GetApiConfig()
        {
            ////TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            //// return StatusCode(200, default(Folder));

            ////TODO: Uncomment the next line to return response 429 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            //// return StatusCode(429, default(Problem));
            try
            {
                var op = _pxApiConfigurationService.GetConfiguration();
                
                var configResponse = new ConfigResponse
                {
                    ApiVersion = op.ApiVersion,
                    Languages = op.Languages.Select(x => new Language
                    {
                        Id = x.Id,
                        Lable = x.Label
                    }
                    ).ToList(),
                    SourceReferences = op.SourceReferences.Select(x => new Models.Api2.SourceReference
                    {
                        Language = x.Language,
                        Text = x.Text
                    }).ToList(),
                    Features = new List<Feature>(),
                    DefaultLanguage = op.DefaultLanguage,
                    License = op.License,
                    MaxCalls = op.MaxCalls,
                    MaxDataCells = op.MaxDataCells,
                    TimeWindow = op.TimeWindow,
                };

                Feature cors = new Feature() { Id = "CORS", Params = new List<Param>() };
                Param param = new Param() { Key = "enabled", Value = op.Cors.Enabled.ToString() };
                cors.Params.Add(param);
                configResponse.Features.Add(cors);


                return new ObjectResult(configResponse);
            }
            catch (NullReferenceException ex) {
                _logger.LogError("GetConfiguration caused an exception", ex);
            }
            return StatusCode(500, new Problem() { Status = 500, Title = "Something went wrong fetching the API configuration", Type = "https://TODO/ConfigError", });
        }

    }
}
