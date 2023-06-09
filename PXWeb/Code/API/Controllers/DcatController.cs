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
        public class Input
        {
            public string DatabaseType;
            public string Database;
            public string BaseUri;
            public string License;
            public string BaseApiUrl;
            public string LandingPageUrl;
            public string Publisher;
            public List<string> Languages;
            public List<IDcatLanguageSpecificSettings> LanguageSpecificSettings;
        }

        [HttpPost]
        public HttpResponseMessage buildXML([FromBody] Input input)
        {
            if (input is null) { return Request.CreateResponse(HttpStatusCode.BadRequest, $"Missing json body"); }
            string databaseTypeLower = input.DatabaseType.ToLower();

            List<string> missingFields = new List<string>();
            if (input.DatabaseType is null)
            {
                missingFields.Add("DatabaseType");
            }
            if (input.Database is null)
            {
                missingFields.Add("Database");
            }
            if (input.BaseUri is null)
            {
                missingFields.Add("BaseUri");
            }
            if (input.License is null)
            {
                missingFields.Add("License");
            }
            if (input.BaseApiUrl is null)
            {
                missingFields.Add("BaseApiUrl");
            }
            if (input.LandingPageUrl is null)
            {
                missingFields.Add("LandingPageUrl");
            }
            if (input.Publisher is null)
            {
                missingFields.Add("Publisher");
            }
            if (input.Languages is null)
            {
                missingFields.Add("Languages");
            }
            if (input.LanguageSpecificSettings is null)
            {
                missingFields.Add("LanguageSpecificSettings");
            }

            if (missingFields.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Missing following parameters in json body: {string.Join(", ", missingFields)}");
            }

            string mainLanguage = new string(Settings.Current.General.Language.DefaultLanguage.Take(2).ToArray());

            RdfSettings settings = new RdfSettings()
            {
                BaseUri = input.BaseUri,
                CatalogTitles = input.LanguageSpecificSettings.Select(x => new KeyValuePair<string, string>(x.Language, x.CatalogTitle)).ToList(),
                CatalogDescriptions = input.LanguageSpecificSettings.Select(x => new KeyValuePair<string, string>(x.Language, x.CatalogDescription)).ToList(),
                License = input.License,
                BaseApiUrl = input.BaseApiUrl,
                LandingPageUrl = input.LandingPageUrl,
                PublisherName = input.Publisher,
                Languages = input.Languages,
                ThemeMapping = HttpContext.Current.Server.MapPath("~/TMapping.json"),
                MainLanguage = mainLanguage
            };
            if (databaseTypeLower == "cnmm") {
                settings.Fetcher = new CNMMFetcher();
                settings.DBid = input.Database;
            }
            else if(databaseTypeLower == "px")
            {
                settings.DBid = HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath) + input.Database + "/Menu.xml";
                if (!File.Exists(settings.DBid)) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Database does not exist: {input.Database}");
                string localThemeMapping = HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath) + input.Database + "/TMapping.json";
                if (File.Exists(localThemeMapping)) settings.ThemeMapping = localThemeMapping;
                settings.Fetcher = new PXFetcher(HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid database type: {input.DatabaseType}");
            }
            string savePath = HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath + input.Database + "/dcat-ap.xml");
            XML.WriteToFile(savePath, settings);
            return Request.CreateResponse(HttpStatusCode.OK, "Xml-file created successfully");
        }
    }
}