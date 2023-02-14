using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PxWeb.Code.Api2.Cache;

namespace PxWeb.Controllers.Api2.Admin
{
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IPxCache _pxCache;

        public CacheController(IPxCache pxCache)
        {
            _pxCache = pxCache;
        }

        [HttpDelete]
        [Route("/api/v2/admin/cache")]
        public IActionResult Clear()
        {
            _pxCache.Clear();
            return Ok();
        }
    }
}
