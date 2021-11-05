using PXWeb.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace PXWeb.API
{
    /// <summary>
    /// API for controlling the menu creation/recreation
    /// </summary>
    [AuthenticationFilter]
    public class MenuController : ApiController
    {
        /// <summary>
        /// Method to clear cache
        /// </summary>
        [HttpPost]
        public HttpResponseMessage Delete(string database, bool languageDependent = true, string sortBy = "Title")
        {
            var statusCode = HttpStatusCode.Created;
            List<DatabaseMessage> result = null;
            try
            {
                string path;

                path = System.Web.HttpContext.Current.Server.MapPath(Settings.Current.General.Paths.PxDatabasesPath);
                path = System.IO.Path.Combine(path, database);

                
                result = AdminTool.GenerateDatabase(path, languageDependent, sortBy);

                // Clear all caches
                PXWeb.Management.PxContext.CacheController.Clear();
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Request.CreateResponse(statusCode, result);
        }
    }
}