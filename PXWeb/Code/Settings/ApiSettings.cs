using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the API settings
    /// </summary>
    internal class ApiSettings : IApiSettings
    {
        #region "Private fields"
        #endregion

        #region "Public methods"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiNode">XML-node for the API settings</param>
        public ApiSettings(XmlNode apiNode)
        {
            string xpath;

            xpath = "./urlRoot";
            UrlRoot = SettingsHelper.GetSettingValue(xpath, apiNode, "");

            xpath = "./urlRootV2";
            UrlRootV2 = SettingsHelper.GetSettingValue(xpath, apiNode, "");

            xpath = "./enableApiV2QueryLink";
            EnableApiV2QueryLink = SettingsHelper.GetSettingValue(xpath, apiNode, false);

            xpath = "./routePrefix";
            RoutePrefix = SettingsHelper.GetSettingValue(xpath, apiNode, "api/v1/");

            xpath = "./maxValuesReturned";
            MaxValuesReturned = SettingsHelper.GetSettingValue(xpath, apiNode, 1000);

            xpath = "./limiterRequests";
            LimiterRequests = SettingsHelper.GetSettingValue(xpath, apiNode, 10);

            xpath = "./limiterTimespan";
            LimiterTimespan = SettingsHelper.GetSettingValue(xpath, apiNode, 10);

            xpath = "./fetchCellLimit";
            FetchCellLimit = SettingsHelper.GetSettingValue(xpath, apiNode, 100000);

            xpath = "./enableCORS";
            EnableCORS = SettingsHelper.GetSettingValue(xpath, apiNode, true);

            xpath = "./enableCache";
            EnableCache = SettingsHelper.GetSettingValue(xpath, apiNode, true);

            xpath = "./showQueryInformation";
            ShowQueryInformation = SettingsHelper.GetSettingValue(xpath, apiNode, true);
            
            xpath = "./infoURL";
            InfoURL = SettingsHelper.GetSettingValue(xpath, apiNode, "");

            xpath = "./defaultExampleResponseFormat";
            DefaultExampleResponseFormat = SettingsHelper.GetSettingValue(xpath, apiNode, "px");

            xpath = "./showSaveApiQueryButton";
            ShowSaveApiQueryButton = SettingsHelper.GetSettingValue(xpath, apiNode, false);

            xpath = "./saveApiQueryText";
            SaveApiQueryText = SettingsHelper.GetSettingValue(xpath, apiNode, string.Empty);

        }

        /// <summary>
        /// Save Features.Api settings to the settings file
        /// </summary>
        /// <param name="apiNode">XML-node for the Features.Api settings</param>
        public void Save(XmlNode apiNode)
        {
            string xpath;

            xpath = "./urlRoot";
            SettingsHelper.SetSettingValue(xpath, apiNode, UrlRoot);

            xpath = "./urlRootV2";
            SettingsHelper.SetSettingValue(xpath, apiNode, UrlRootV2);

            xpath = "./enableApiV2QueryLink";
            SettingsHelper.SetSettingValue(xpath, apiNode, EnableApiV2QueryLink.ToString());

            xpath = "./routePrefix";
            SettingsHelper.SetSettingValue(xpath, apiNode, RoutePrefix);

            xpath = "./maxValuesReturned";
            SettingsHelper.SetSettingValue(xpath, apiNode, MaxValuesReturned.ToString());

            xpath = "./limiterRequests";
            SettingsHelper.SetSettingValue(xpath, apiNode, LimiterRequests.ToString());

            xpath = "./limiterTimespan";
            SettingsHelper.SetSettingValue(xpath, apiNode, LimiterTimespan.ToString());

            xpath = "./fetchCellLimit";
            SettingsHelper.SetSettingValue(xpath, apiNode, FetchCellLimit.ToString());

            xpath = "./enableCORS";
            SettingsHelper.SetSettingValue(xpath, apiNode, EnableCORS.ToString());

            xpath = "./enableCache";
            SettingsHelper.SetSettingValue(xpath, apiNode, EnableCache.ToString());

            xpath = "./showQueryInformation";
            SettingsHelper.SetSettingValue(xpath, apiNode, ShowQueryInformation.ToString());

            xpath = "./infoURL";
            SettingsHelper.SetSettingValue(xpath, apiNode, InfoURL);

            xpath = "./defaultExampleResponseFormat";
            SettingsHelper.SetSettingValue(xpath, apiNode, DefaultExampleResponseFormat);

            xpath = "./showSaveApiQueryButton";
            SettingsHelper.SetSettingValue(xpath, apiNode, ShowSaveApiQueryButton.ToString());

            xpath = "./saveApiQueryText";
            SettingsHelper.SetSettingValue(xpath, apiNode, SaveApiQueryText);
        }
        #endregion


        #region IApiSettings Members

        public int MaxValuesReturned { get; set; }
        public string UrlRoot { get; set; }
        public string UrlRootV2 { get; set; }
        public bool EnableApiV2QueryLink { get; set; }
        public string RoutePrefix { get; set; }
        public int LimiterRequests { get; set; }
        public int LimiterTimespan { get; set; }
        public bool EnableCORS { get; set; }
        public bool EnableCache { get; set; }
        public string ClearCache { get; set; }
        public bool ShowQueryInformation { get; set; }
        public string InfoURL { get; set; }
        public int FetchCellLimit { get; set; }
        public string DefaultExampleResponseFormat { get; set; }
        public bool ShowSaveApiQueryButton { get; set; }
        public string SaveApiQueryText { get; set; }


        #endregion
    }
}
