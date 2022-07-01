using PXWeb.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PXWeb.API
{
        public class DcatXMLController : ApiController
        {
            [HttpPost]
            public HttpResponseMessage buildXML(string database)//, string baseURI, string catalogTitle, string catalogDescription, string license, string baseApiUrl, string landingPageUrl, string publisher, List<string> languages, string preferredLanguage)
            {
            //var statusCode = HttpStatusCode.Created;
            //List<DataBaseMessage> result = null;
            //_logger.Info("buildingXML - started");
            //try
            //{

            //    _logger.Info("buildingXML - finished without error");
            //}
            //catch(Exception e)
            //{
            //    statusCode = HttpStatusCode.InternalServerError;
            //    _logger.Error(e.Message);
            //}
            //return Request.CreateResponse(statusCode, result);

            return Request.CreateResponse(HttpStatusCode.OK, $"Det gick bra att generera för {database}");
             }
        }
}