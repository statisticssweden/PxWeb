using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PXWeb.API
{
    /// <summary>
    /// Cotroller for PxWeb cache manipulation
    /// </summary>
    
    [AuthenticationFilter]
    public class CacheController : ApiController
    {
        private static log4net.ILog _logger;
        private Services.ICacheService _service;

        public CacheController(Services.ICacheService service, log4net.ILog logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Method to clear cache
        /// </summary>
        [HttpDelete]
        public HttpResponseMessage Delete()
        {
            var statusCode = HttpStatusCode.NoContent;
            try 
            {
                _service.ClearCache();
                _logger.Info("Cleared cache");
            } 
            catch(Exception e) 
            {
                _logger.Error(e);
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Request.CreateResponse(statusCode);
        }
    }
}