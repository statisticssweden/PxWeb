using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PCAxis.Paxiom;
using Px.Abstractions.Interfaces;
using Px.Search;
using PxWeb.Code.Api2.DataSource.PxFile;
using PxWeb.Config.Api2;
using PXWeb.Database;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;

namespace PxWeb.Controllers.Api2.Admin
{
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class DatabaseController : ControllerBase
    {
        private readonly IDataSource _dataSource;
        private readonly PxApiConfigurationOptions _configOptions;
        private readonly ILogger<DatabaseController> _logger;
        private readonly IPxHost _hostingEnvironment;

        public DatabaseController(IDataSource dataSource, IOptions<PxApiConfigurationOptions> configOptions, ILogger<DatabaseController> logger, IPxHost hostingEnvironment)
        {
            _dataSource = dataSource;
            _configOptions = configOptions.Value;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPut]
        [Route("/api/v2/admin/database")]
        [SwaggerOperation("Database")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
        [SwaggerResponse(statusCode: 405, description: "Method Not Allowed")]
        public IActionResult Database([FromQuery(Name = "langdependent")] bool? langDependent, [FromQuery(Name = "sortorder")] string? sortOrder)
        {
            try
            {
                if (_dataSource.GetType() != typeof(PxFileDataSource))
                {
                    return StatusCode(405, "Only possible to generate database for PX-file databases");
                }

                PXWeb.Database.DatabaseSpider spider;
                spider = new PXWeb.Database.DatabaseSpider();
                spider.Handles.Add(new AliasFileHandler(_configOptions, _logger));
                spider.Handles.Add(new LinkFileHandler(_configOptions, _logger));
                spider.Handles.Add(new PxFileHandler());
                spider.Handles.Add(new MenuSortFileHandler(_configOptions, _logger));

                List<string> langs = new List<string>();
                foreach (Language lang in _configOptions.Languages)
                {
                    langs.Add(lang.Id);
                }

                string sorting = GetSorting(sortOrder);
                string databasePath = Path.Combine(_hostingEnvironment.RootPath, "Database");

                spider.Builders.Add(new MenuBuilder(_configOptions, _logger, _hostingEnvironment, langs.ToArray(), GetLangDependent(langDependent)) { SortOrder = GetSortOrder(sorting) });
                spider.Search(databasePath);

                List<DatabaseMessage> messages = spider.Messages;

                return Ok();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        private bool GetLangDependent(bool? langDependent)
        {
            if (langDependent == null)
            {
                return false;
            }
            else
            {
                return (bool)langDependent;
            }
        }

        private string GetSorting(string? sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortOrder))
            {
                return "matrix";
            }

            switch (sortOrder.ToLower())
            {
                case "matrix":
                    return "matrix";
                case "title":
                    return "title";
                case "filename":
                    return "filename";
                default:
                    return "matrix";
            }
        }

        private static Func<PCAxis.Paxiom.PXMeta, string, string> GetSortOrder(string sortOrder)
        {
            switch (sortOrder.ToLower())
            {
                case "matrix":
                    return (meta, path) => meta.Matrix;
                case "title":
                    return (meta, path) => !string.IsNullOrEmpty(meta.Description) ? meta.Description : meta.Title;
                case "filename":
                    return (meta, path) => System.IO.Path.GetFileNameWithoutExtension(path);
                default:
                    break;
            }
            return (meta, path) => path;
        }
    }
}
