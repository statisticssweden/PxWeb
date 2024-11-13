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
        static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(MenuController));

        /// <summary>
        /// Method to clear cache
        /// </summary>
        [HttpPost]
        public HttpResponseMessage RebuildMenu(string database, bool languageDependent = true, string sortBy = "Title", bool buildSearchIndex = true)
        {
            var statusCode = HttpStatusCode.Created;
            List<DatabaseMessage> result = null;
            _logger.Info("RebuildMenu - started");
            try
            {
                // Validate the database parameter (from CodeQL AI)
                if (database.Contains("..") || database.Contains("/") || database.Contains("\\"))
                {
                    //dont think it is possible to hit this
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid database name");
                }

                // this prevents typos
                if (!(PXWeb.Settings.Current.General.Databases.CnmmDatabases.Contains(database) ||
                      PXWeb.Settings.Current.General.Databases.PxDatabases.Contains(database)))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Unknown database name");
                }

                string path;

                path = System.Web.HttpContext.Current.Server.MapPath(Settings.Current.General.Paths.PxDatabasesPath);
                path = System.IO.Path.Combine(path, database);


                result = AdminTool.GenerateDatabase(path, languageDependent, sortBy);

                // Clear all caches
                PXWeb.Management.PxContext.CacheController.Clear();

                //Force that databases are read again
                PXWeb.DatabasesSettings databases = (PXWeb.DatabasesSettings)PXWeb.Settings.Current.General.Databases;
                databases.ResetDatabases();

                if (PXWeb.Settings.Current.Features.General.SearchEnabled && buildSearchIndex)
                {
                    PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);
                    PXWeb.SearchIndexSettings searchIndex = (PXWeb.SearchIndexSettings)db.SearchIndex;

                    // Check that the status has not been changed by another system before updating it
                    if (searchIndex.Status != SearchIndexStatusType.Indexing)
                    {
                        searchIndex.Status = SearchIndexStatusType.WaitingCreate;
                        db.Save();

                        BackgroundWorker.PxWebBackgroundWorker.HandleSearchIndex(database);
                    }
                }
                _logger.Info("RebuildMenu - finished without error");
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                _logger.Error(e.Message);
            }
            return Request.CreateResponse(statusCode, result);
        }
    }
}