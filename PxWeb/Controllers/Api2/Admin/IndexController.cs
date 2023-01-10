using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Px.Abstractions.Interfaces;
using Px.Search;
using PxWeb.Api2.Server.Models;
using PxWeb.Config.Api2;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace PxWeb.Controllers.Api2.Admin
{
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IDataSource _dataSource;
        private readonly ISearchBackend _backend;
        private readonly IPxApiConfigurationService _pxApiConfigurationService;

        public IndexController(IDataSource dataSource, ISearchBackend backend, IPxApiConfigurationService pxApiConfigurationService)
        {
            _dataSource = dataSource;
            _backend = backend; 
            _pxApiConfigurationService = pxApiConfigurationService; 
        }

        [HttpGet]
        [Route("/api/v2/admin/index")]
        [SwaggerOperation("IndexDatabase")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
        public IActionResult IndexDatabase()
        {
            List<string> languages = new List<string>();

            var config = _pxApiConfigurationService.GetConfiguration();

            if (config.Languages.Count == 0)
            {
                throw new System.Exception("No languages configured for PxApi");
            }

            foreach (var lang in config.Languages)
            {
                languages.Add(lang.Id);
            }

            Indexer indexer = new Indexer(_dataSource, _backend);
            indexer.IndexDatabase(languages);

            return Ok();
        }
    }
}
