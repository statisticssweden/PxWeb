using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PxWeb.Api2.Server.Models;
using PxWeb.Attributes.Api2;
using PxWeb.Config.Api2;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Language = PxWeb.Api2.Server.Models.Language;

namespace PxWeb.Controllers.Api2
{
    [ApiController]
    public class ConfigurationApiController : PxWeb.Api2.Server.Controllers.ConfigurationApiController
    {
        private readonly IPxApiConfigurationService _pxApiConfigurationService;
        private readonly ILogger<ConfigurationApiController> _logger;

        public ConfigurationApiController(IPxApiConfigurationService pxApiConfigurationService, ILogger<ConfigurationApiController> logger)
        {
            _pxApiConfigurationService = pxApiConfigurationService;
            _logger = logger;
        }

        public override IActionResult GetApiConfiguration()
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
                        Label = x.Label
                    }
                    ).ToList(),
                    SourceReferences = op.SourceReferences.Select(x => new SourceReference
                    {
                        Language = x.Language,
                        Text = x.Text
                    }).ToList(),
                    Features = new List<ApiFeature>(),
                    DefaultLanguage = op.DefaultLanguage,
                    License = op.License,
                    MaxCallsPerTimeWindow = op.MaxCalls,
                    MaxDataCells = op.MaxDataCells,
                    TimeWindow = op.TimeWindow,
                };

                ApiFeature cors = new ApiFeature() { Id = "CORS", Params = new List<PxWeb.Api2.Server.Models.KeyValuePair>() };
                PxWeb.Api2.Server.Models.KeyValuePair keyValuePair = new PxWeb.Api2.Server.Models.KeyValuePair() { Key = "enabled", Value = op.Cors.Enabled.ToString() };
                cors.Params.Add(keyValuePair);
                configResponse.Features.Add(cors);


                return new ObjectResult(configResponse);
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError("GetConfiguration caused an exception", ex);
            }
            return StatusCode(500, new Problem() { Status = 500, Title = "Something went wrong fetching the API configuration", Type = "https://TODO/ConfigError", });
        }
    }
}
