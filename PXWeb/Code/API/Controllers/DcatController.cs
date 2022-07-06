using PXWeb.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Px.Rdf;
using System.Web;
using System.IO;

namespace PXWeb.API
{
        [AuthenticationFilter]
        public class DcatController : ApiController
        {
            [HttpPost]
            public HttpResponseMessage buildXML(string databaseType, string database, string baseURI, string catalogTitle, string catalogDescription, string license, string baseApiUrl, string landingPageUrl, string publisher, [FromUri] List<string> languages, string preferredLanguage)
            {
                string databaseTypeLower = databaseType.ToLower();
                RdfSettings settings = new RdfSettings()
                {
                    BaseUri = baseURI,
                    CatalogTitle = catalogTitle,
                    CatalogDescription = catalogDescription,
                    License = license,
                    BaseApiUrl = baseApiUrl,
                    LandingPageUrl = landingPageUrl,
                    PublisherName = publisher,
                    Languages = languages,
                    PreferredLanguage = preferredLanguage,
                    ThemeMapping = HttpContext.Current.Server.MapPath("~/TMapping.json")
                };
                if (databaseTypeLower == "cnmm") {
                    settings.Fetcher = new CNMMFetcher();
                    settings.DBid = database;
                }
                else if(databaseTypeLower == "px")
                {
                    settings.DBid = HttpContext.Current.Server.MapPath("~/Resources/PX/Databases/") + database + "/Menu.xml";
                    if (!File.Exists(settings.DBid)) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Database does not exist: {database}");
                    string localThemeMapping = HttpContext.Current.Server.MapPath("~/Resources/PX/Databases/") + database + "/TMapping.json";
                    if (File.Exists(localThemeMapping)) settings.ThemeMapping = localThemeMapping;
                    settings.Fetcher = new PXFetcher(HttpContext.Current.Server.MapPath("~/Resources/PX/Databases/"));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid database type");
                }
                XML.WriteToFile(HttpContext.Current.Server.MapPath("~/dcat-ap.xml"), settings);
                return Request.CreateResponse(HttpStatusCode.OK, $"Xml file created successfully, {databaseType}");
            }
        }
}