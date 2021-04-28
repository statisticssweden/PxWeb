using System.Web.Http;

namespace PXWeb.API
{
    public class CacheController : ApiController
    {
        /// <summary>
        /// Method to delete specific cache item
        /// </summary>
        /// <param name="itemKey">Key of the cache item to be deleted</param>
        [HttpDelete, Route("api/cache")]
        public void Delete(string key)
        {
            // TODO: Authentication

            // TODO: Implement method to clear only cache corresponding the key parameter

            Management.PxContext.CacheController.Clear();
        }
    }
}