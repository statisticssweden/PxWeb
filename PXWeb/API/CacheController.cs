using System.Web.Http;

namespace PXWeb.API
{
    /// <summary>
    /// Cotroller for PxWeb cache manipulation
    /// </summary>
    public class CacheController : ApiController
    {
        /// <summary>
        /// Method to delete specific cache item
        /// </summary>
        /// <param name="itemKey">Key of the cache item to be deleted</param>
        [HttpDelete]
        public void Delete([FromUri]string key)
        {
            // TODO: Authentication

            // TODO: Implement method to clear only cache corresponding the key parameter

            Management.PxContext.CacheController.Clear();
        }
    }
}