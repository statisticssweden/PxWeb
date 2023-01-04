using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PxWeb.Api2.Server.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PxWeb.Controllers.Api2.Admin
{
    [ApiController]
    public class IndexController : ControllerBase
    {
        [HttpGet]
        [Route("/api/v2/admin/index")]
        [SwaggerOperation("IndexDatabase")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        [SwaggerResponse(statusCode: 401, description: "Unauthorized")]
        public IActionResult IndexDatabase()
        {
            return Ok();
        }
    }
}
