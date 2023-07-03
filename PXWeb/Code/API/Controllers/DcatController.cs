using PXWeb.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Px.Dcat;
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

            string databasePath = HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath);
            string themeMapping = HttpContext.Current.Server.MapPath("~/Themes.json");
            string organizationMapping = HttpContext.Current.Server.MapPath("~/Organizations.json");
            string localThemeMapping = databasePath + input.Database + "/Themes.json";
            string localOrganizationMapping = databasePath + input.Database + "/Organizations.json";

            if (File.Exists(localThemeMapping)) themeMapping = localThemeMapping;
            if (File.Exists(localOrganizationMapping)) organizationMapping = localOrganizationMapping;

            Px.Dcat.DcatSettings settings = new Px.Dcat.DcatSettings()
            {
                BaseUri = input.BaseUri,
                CatalogTitles = input.LanguageSpecificSettings.Select(x => new KeyValuePair<string, string>(x.Language, x.CatalogTitle)).ToList(),
                CatalogDescriptions = input.LanguageSpecificSettings.Select(x => new KeyValuePair<string, string>(x.Language, x.CatalogDescription)).ToList(),
                License = input.License,
                BaseApiUrl = input.BaseApiUrl,
                LandingPageUrl = input.LandingPageUrl,
                PublisherName = input.Publisher,
                Languages = input.Languages,
                ThemeMapping = themeMapping,
                OrganizationMapping = organizationMapping,
                MainLanguage = mainLanguage
            };
            if (databaseTypeLower == "cnmm") {
                settings.DatabaseType = Px.Dcat.Helpers.DatabaseType.CNMM;
                settings.DatabaseId = input.Database;
            }
            else if(databaseTypeLower == "px")
            {
                settings.DatabaseType = Px.Dcat.Helpers.DatabaseType.PX;
                settings.DatabaseId = HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath) + input.Database + "/Menu.xml";
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid database type: {input.DatabaseType}");
            }
            string savePath = HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath + input.Database + "/dcat-ap.xml");
            DcatWriter.WriteToFile(savePath, settings);
            return Request.CreateResponse(HttpStatusCode.OK, "Xml-file created successfully");
        }
    }
}