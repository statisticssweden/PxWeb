using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Px.Abstractions.Interfaces;
using Px.Search;
using PxWeb.Api2.Server.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace PxWeb.Controllers.Api2.Admin
{
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IDataSource _dataSource;
        private readonly ISearchBackend _backend;

        public IndexController(IDataSource dataSource, ISearchBackend backend)
        {
            _dataSource = dataSource;
            _backend = backend; 
        }

        [HttpGet]
        [Route("/api/v2/admin/index")]
        [SwaggerOperation("IndexDatabase")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
        public IActionResult IndexDatabase()
        {
            List<string> languages = new List<string>();

            //TODO: Get languages from configuration
            languages.Add("en");

            Indexer indexer = new Indexer(_dataSource, _backend);
            indexer.IndexDatabase(languages);

            return Ok();
        }
    }
}
