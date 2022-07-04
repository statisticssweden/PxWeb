using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml;
using PCAxis.Chart;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the Chart settings
    /// </summary>
    internal class DcatSettings : IDcatSettings
    {
        #region "Private fields"

        #endregion

        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="chartsNode">XML-node for the Charts settings</param>
        public DcatSettings(XmlNode chartsNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./BaseURI";
            BaseURI = SettingsHelper.GetSettingValue(xpath, chartsNode, "https://baseURI.com/");

            xpath = "./BaseApiUrl";
            BaseApiUrl = SettingsHelper.GetSettingValue(xpath, chartsNode, "https://baseAPI.com/");
            
            xpath = "./LandingPageUrl";
            LandingPageUrl = SettingsHelper.GetSettingValue(xpath, chartsNode, "https://baseLandingPage.com/");
            
            xpath = "./CatalogTitle";
            CatalogTitle = SettingsHelper.GetSettingValue(xpath, chartsNode, "Catalog title");
            
            xpath = "./CatalogDescription";
            CatalogDescription = SettingsHelper.GetSettingValue(xpath, chartsNode, "Catalog description");
            
            xpath = "./Publisher";
            Publisher = SettingsHelper.GetSettingValue(xpath, chartsNode, "SCB");
            
            xpath = "./Database";
            Database = SettingsHelper.GetSettingValue(xpath, chartsNode, "Example");
            
            xpath = "./DatabaseType";
            DatabaseType = SettingsHelper.GetSettingValue(xpath, chartsNode, "PX");
            
            xpath = "./License";
            License = SettingsHelper.GetSettingValue(xpath, chartsNode, "http://creativecommons.org/publicdomain/zero/1.0/");
        }

        /// <summary>
        /// Save Features.Charts settings to the settings file
        /// </summary>
        /// <param name="generalNode">XML-node for the Features.Charts settings</param>
        public void Save(XmlNode chartsNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./BaseURI";
            SettingsHelper.SetSettingValue(xpath, chartsNode, BaseURI);

            xpath = "./BaseApiUrl";
            SettingsHelper.SetSettingValue(xpath, chartsNode, BaseApiUrl);

            xpath = "./LandingPageUrl";
            SettingsHelper.SetSettingValue(xpath, chartsNode, LandingPageUrl);

            xpath = "./CatalogTitle";
            SettingsHelper.SetSettingValue(xpath, chartsNode, CatalogTitle);

            xpath = "./CatalogDescription";
            SettingsHelper.SetSettingValue(xpath, chartsNode, CatalogDescription);

            xpath = "./Publisher";
            SettingsHelper.SetSettingValue(xpath, chartsNode, Publisher);

            xpath = "./Database";
            SettingsHelper.SetSettingValue(xpath, chartsNode, Database);

            xpath = "./DatabaseType";
            SettingsHelper.SetSettingValue(xpath, chartsNode, DatabaseType);

            xpath = "./License";
            SettingsHelper.SetSettingValue(xpath, chartsNode, License);
        }

        public string BaseURI { get; set; }
        public string BaseApiUrl { get; set; }
        public string LandingPageUrl { get; set; }
        public string Publisher { get; set; }
        public string CatalogTitle { get; set; }
        public string CatalogDescription { get; set; }
        public string Database { get; set; }
        public string DatabaseType { get; set; }
        public string License { get; set; }
        #endregion
    }
}
