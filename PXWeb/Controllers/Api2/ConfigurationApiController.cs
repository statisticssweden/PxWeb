using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PxWeb.Attributes.Api2;
using PxWeb.Config.Api2;
using PxWeb.Models.Api2;
using Swashbuckle.AspNetCore.Annotations;

namespace PxWeb.Controllers.Api2
{
    [ApiController]
    public class ConfigurationApiController : ControllerBase
    {
        private readonly IPxApiConfigurationService _pxApiConfigurationService;

        public ConfigurationApiController(IPxApiConfigurationService pxApiConfigurationService)
        {
            _pxApiConfigurationService = pxApiConfigurationService;
        }

        /// <summary>
        /// Get PxApi configuration settings
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="429">Error respsone for 429</response>
        [HttpGet]
        [Route("/v2/config")]
        public virtual IActionResult GetConfiguration()
        {
            ////TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            //// return StatusCode(200, default(Folder));

            ////TODO: Uncomment the next line to return response 429 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            //// return StatusCode(429, default(Problem));
            //string exampleJson = null;
            //exampleJson = "{\n  \"description\" : \"description\",\n  \"folderContents\" : [ {\n    \"description\" : \"description\",\n    \"id\" : \"id\",\n    \"label\" : \"label\",\n    \"objectType\" : \"objectType\"\n  }, {\n    \"description\" : \"description\",\n    \"id\" : \"id\",\n    \"label\" : \"label\",\n    \"objectType\" : \"objectType\"\n  } ],\n  \"links\" : [ {\n    \"rel\" : \"rel\",\n    \"href\" : \"href\"\n  }, {\n    \"rel\" : \"rel\",\n    \"href\" : \"href\"\n  } ],\n  \"id\" : \"id\",\n  \"label\" : \"label\",\n  \"objectType\" : \"objectType\",\n  \"tags\" : [ \"tags\", \"tags\" ]\n}";

            //var example = exampleJson != null
            //? JsonConvert.DeserializeObject<Folder>(exampleJson)
            //: default(Folder);            //TODO: Change the data returned
            //return new ObjectResult(example);

            PxApiConfigurationOptions op = new PxApiConfigurationOptions();
            //op.Language = "sv";
            //op.ApiVersion = "2.0";
            op = _pxApiConfigurationService.GetConfiguration();

            return new ObjectResult(op);
        }


        //public IActionResult GetConfiguration() => Ok(_pxApiConfigurationService.GetConfiguration());
    }
}
