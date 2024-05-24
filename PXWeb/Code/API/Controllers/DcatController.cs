using Px.Dcat;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PXWeb.API
{
    [AuthenticationFilter]
    public class DcatController : ApiController
    {
        public class DcatInput
        {
            public string DatabaseType;
            public string Database;
            public string BaseUri;
            public string License;
            public string BaseApiUrl;
            public string LandingPageUrl;
            public List<string> Languages;
            public List<DcatLanguageSpecificSettingsInput> LanguageSpecificSettings;
        }

        public class DcatLanguageSpecificSettingsInput : IDcatLanguageSpecificSettings
        {
            public string Language { get; set; }
            public string CatalogTitle { get; set; }
            public string CatalogDescription { get; set; }
            public string PublisherName { get; set; }
        }

        [HttpPost]
        public HttpResponseMessage buildXML([FromBody] DcatInput dcatInput)
        {
            if (dcatInput is null) { return Request.CreateResponse(HttpStatusCode.BadRequest, $"Missing json body"); }

            List<string> missingFields = new List<string>();
            if (dcatInput.DatabaseType is null)
            {
                missingFields.Add("DatabaseType");
            }
            if (dcatInput.Database is null)
            {
                missingFields.Add("Database");
            }
            if (dcatInput.BaseUri is null)
            {
                missingFields.Add("BaseUri");
            }
            if (dcatInput.License is null)
            {
                missingFields.Add("License");
            }
            if (dcatInput.BaseApiUrl is null)
            {
                missingFields.Add("BaseApiUrl");
            }
            if (dcatInput.LandingPageUrl is null)
            {
                missingFields.Add("LandingPageUrl");
            }
            if (dcatInput.Languages is null)
            {
                missingFields.Add("Languages");
            }
            if (dcatInput.LanguageSpecificSettings is null)
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
            string localThemeMapping = databasePath + dcatInput.Database + "/Themes.json";
            string localOrganizationMapping = databasePath + dcatInput.Database + "/Organizations.json";

            if (File.Exists(localThemeMapping)) themeMapping = localThemeMapping;
            if (File.Exists(localOrganizationMapping)) organizationMapping = localOrganizationMapping;

            Px.Dcat.DcatSettings settings = new Px.Dcat.DcatSettings()
            {
                BaseUri = dcatInput.BaseUri,
                CatalogTitles = dcatInput.LanguageSpecificSettings.Select(x => new KeyValuePair<string, string>(x.Language, x.CatalogTitle)).ToList(),
                CatalogDescriptions = dcatInput.LanguageSpecificSettings.Select(x => new KeyValuePair<string, string>(x.Language, x.CatalogDescription)).ToList(),
                License = dcatInput.License,
                BaseApiUrl = dcatInput.BaseApiUrl,
                LandingPageUrl = dcatInput.LandingPageUrl,
                PublisherNames = dcatInput.LanguageSpecificSettings.Select(x => new KeyValuePair<string, string>(x.Language, x.PublisherName)).ToList(),
                Languages = dcatInput.Languages,
                ThemeMapping = themeMapping,
                OrganizationMapping = organizationMapping,
                MainLanguage = mainLanguage
            };

            string databaseTypeLower = dcatInput.DatabaseType.ToLower();

            if (databaseTypeLower == "cnmm")
            {
                settings.DatabaseType = Px.Dcat.Helpers.DatabaseType.CNMM;
                settings.DatabaseId = dcatInput.Database;
            }
            else if (databaseTypeLower == "px")
            {
                settings.DatabaseType = Px.Dcat.Helpers.DatabaseType.PX;
                settings.DatabaseId = HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath) + dcatInput.Database + "/Menu.xml";
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Invalid database type: {dcatInput.DatabaseType}");
            }

            string savePath = HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath + dcatInput.Database + "/dcat-ap.xml");
            DcatWriter.WriteToFile(savePath, settings);
            return Request.CreateResponse(HttpStatusCode.OK, "Xml-file created successfully");
        }
    }
}