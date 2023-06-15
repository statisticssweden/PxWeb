using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Px.Abstractions.Interfaces;
using Px.Search;
using PxWeb.Api2.Server.Models;
using PxWeb.Config.Api2;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PxWeb.Code.BackgroundWorker;

namespace PxWeb.Controllers.Api2.Admin
{
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class SearchindexController : ControllerBase
    {
        private readonly IDataSource _dataSource;
        private readonly ISearchBackend _backend;
        private readonly IPxApiConfigurationService _pxApiConfigurationService;
        private readonly ILogger<SearchindexController> _logger;
        private readonly IStateProvider _stateProvider;
        private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

        public SearchindexController(BackgroundWorkerQueue backgroundWorkerQueue, IStateProvider stateProvider, IDataSource dataSource, ISearchBackend backend, IPxApiConfigurationService pxApiConfigurationService, ILogger<SearchindexController> logger)
        {
            _dataSource = dataSource;
            _backend = backend; 
            _pxApiConfigurationService = pxApiConfigurationService; 
            _logger = logger;
            _stateProvider = stateProvider;
            _backgroundWorkerQueue = backgroundWorkerQueue;
        }

        /// <summary>
        /// Index the whole database in all languages
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/v2/admin/searchindex")]
        [SwaggerOperation("IndexDatabase")]
        [SwaggerResponse(statusCode: 202, description: "Accepted")]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
        public IActionResult IndexDatabase()
        {
            _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
            {
                try
                {
                    List<string> languages = new List<string>();

                    var config = _pxApiConfigurationService.GetConfiguration();

                    if (config.Languages.Count == 0)
                    {
                        _logger.LogError("No languages configured for PxApi");
                        return;
                    }
                    foreach (var lang in config.Languages)
                    {
                        languages.Add(lang.Id);
                    }

                    Indexer indexer = new Indexer(_dataSource, _backend, _logger);
                    indexer.IndexDatabase(languages);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            });
            return new AcceptedResult();
        }

        /// <summary>
        /// Update index for the specified tables
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("/api/v2/admin/searchindex")]
        [SwaggerOperation("IndexDatabase")]
        [SwaggerResponse(statusCode: 202, description: "Accepted")]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
        public IActionResult IndexDatabase([FromBody, Required] string[] tables)
        {
            _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
            {
                try
                {
                    List<string> languages = new List<string>();
                    List<string> tableList = new List<string>(tables);

                    if (tableList.Count == 0)
                    {
                        _logger.LogError("No tables specified for index update");
                        return;
                    }

                    var config = _pxApiConfigurationService.GetConfiguration();

                    if (config.Languages.Count == 0)
                    {
                        _logger.LogError("No languages configured for PxApi");
                        return;
                    }

                    foreach (var lang in config.Languages)
                    {
                        languages.Add(lang.Id);
                    }

                    Indexer indexer = new Indexer(_dataSource, _backend, _logger);
                    indexer.UpdateTableEntries(tableList, languages);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            });
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("/api/v2/admin/searchindex")]
        [SwaggerOperation("IndexDatabase")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
        public IActionResult GetState()
        {
            string id = GetType().FullName;
            ResponseState state = _stateProvider.Load(id);
            return new JsonResult(state);
        }
    }
}
