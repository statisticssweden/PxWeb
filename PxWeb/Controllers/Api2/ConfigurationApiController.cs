using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using org.sdmx;
using PxWeb.Api2.Server.Models;
using PxWeb.Attributes.Api2;
using PxWeb.Config.Api2;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Language = PxWeb.Api2.Server.Models.Language;

namespace PxWeb.Controllers.Api2
{
    [ApiController]
    public class ConfigurationApiController : PxWeb.Api2.Server.Controllers.ConfigurationApiController
    {
        private readonly IPxApiConfigurationService _pxApiConfigurationService;
        private readonly IIpRateLimitingConfigurationService _rateLimitConfigurationService;
        private readonly ILogger<ConfigurationApiController> _logger;
        private const int TimeWindow = 10;
        public ConfigurationApiController(IPxApiConfigurationService pxApiConfigurationService, IIpRateLimitingConfigurationService rateLimitConfigurationService, ILogger<ConfigurationApiController> logger)
        {
            _pxApiConfigurationService = pxApiConfigurationService;
            _rateLimitConfigurationService = rateLimitConfigurationService;
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
                int timeWindow = TimeWindow;
                int maxCallsPerTimeWindow = 30;
                var op = _pxApiConfigurationService.GetConfiguration();
                var rateLimitOp = _rateLimitConfigurationService.GetConfiguration();
                try
                {
                    var generalRules = rateLimitOp.GeneralRules.Where(x => x.Endpoint == "*").First();
                    timeWindow = GetTimeWindowInSek(generalRules.Period);
                    maxCallsPerTimeWindow = generalRules.Limit;
                }
                catch (Exception ex)
                {

                }

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
                    MaxCallsPerTimeWindow = maxCallsPerTimeWindow, 
                    MaxDataCells = op.MaxDataCells,
                    TimeWindow = timeWindow,
                    DefaultDataFormat = op.DefaultOutputFormat,
                    DataFormats = op.OutputFormats
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

        private int GetTimeWindowInSek(string timeWindowRuel)
        {
            string periodFormText = timeWindowRuel.ToLower()[timeWindowRuel.Length-1].ToString();
            string periodFormTime = timeWindowRuel.Remove(timeWindowRuel.Length - 1, 1);
            int time;
            if(int.TryParse(periodFormTime, out time))

            switch (periodFormText)
            {
                case "s":                         
                        return time;
                case "m": 
                        return time * 60;
                case "h": 
                        return time * 3600;
                case "d": 
                    return time * 86400;
                default:
                    return TimeWindow;
            }
            return TimeWindow;            
        }

    }
}
