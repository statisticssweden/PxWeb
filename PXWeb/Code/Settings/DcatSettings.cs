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
    /// Internal class for reading and writing the Dcat settings
    /// </summary>
    internal class DcatSettings : IDcatSettings
    {
        #region "Private fields"

        #endregion

        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">XML-node for the Dcat settings</param>
        public DcatSettings(XmlNode node)
        {
            string xpath;

            xpath = "./BaseURI";
            BaseURI = SettingsHelper.GetSettingValue(xpath, node, "https://baseURI.com/");

            xpath = "./BaseApiUrl";
            BaseApiUrl = SettingsHelper.GetSettingValue(xpath, node, "https://baseAPI.com/");
            
            xpath = "./LandingPageUrl";
            LandingPageUrl = SettingsHelper.GetSettingValue(xpath, node, "https://baseLandingPage.com/");
            
            xpath = "./Database";
            Database = SettingsHelper.GetSettingValue(xpath, node, "Example");
            
            xpath = "./DatabaseType";
            DatabaseType = SettingsHelper.GetSettingValue(xpath, node, "PX");
            
            xpath = "./License";
            License = SettingsHelper.GetSettingValue(xpath, node, "http://creativecommons.org/publicdomain/zero/1.0/");

            xpath = "./DcatFileStatus";
            FileStatus = SettingsHelper.GetSettingValue(xpath, node, DcatStatusType.NotCreated);

            xpath = "./FileUpdated";
            FileUpdated = SettingsHelper.GetSettingValue(xpath, node, "");

            xpath = "./LanguageSpecific";
            var languageSpecificSettings = new List<IDcatLanguageSpecificSettings>();
            foreach (XmlNode languageNode in node.SelectNodes(xpath))
            {
                DcatLanguageSpecificSettings nodeSettings = new DcatLanguageSpecificSettings(languageNode);
                languageSpecificSettings.Add(nodeSettings);
            }
            LanguageSpecificSettings = languageSpecificSettings;
        }

        /// <summary>
        /// Save Dcat settings to the settings file
        /// </summary>
        /// <param name="generalNode">XML-node for the Dcat settings</param>
        public void Save(XmlNode node)
        {
            string xpath;

            xpath = "./BaseURI";
            SettingsHelper.SetSettingValue(xpath, node, BaseURI);

            xpath = "./BaseApiUrl";
            SettingsHelper.SetSettingValue(xpath, node, BaseApiUrl);

            xpath = "./LandingPageUrl";
            SettingsHelper.SetSettingValue(xpath, node, LandingPageUrl);

            xpath = "./Database";
            SettingsHelper.SetSettingValue(xpath, node, Database);

            xpath = "./DatabaseType";
            SettingsHelper.SetSettingValue(xpath, node, DatabaseType);

            xpath = "./License";
            SettingsHelper.SetSettingValue(xpath, node, License);

            xpath = "./DcatFileStatus";
            SettingsHelper.SetSettingValue(xpath, node, FileStatus.ToString());

            xpath = "./FileUpdated";
            SettingsHelper.SetSettingValue(xpath, node, FileUpdated);

            xpath = "./LanguageSpecific";
            var nodes = node.SelectNodes(xpath);

            // Remove all current nodes
            foreach (XmlNode n in nodes) n.ParentNode.RemoveChild(n);
            
            foreach (DcatLanguageSpecificSettings s in LanguageSpecificSettings)
            {
                var newNode = node.OwnerDocument.CreateNode(XmlNodeType.Element, "LanguageSpecific", "");
                s.Save(newNode);
                node.AppendChild(newNode);
            }
        }

        public string BaseURI { get; set; }
        public string BaseApiUrl { get; set; }
        public string LandingPageUrl { get; set; }
        public string Database { get; set; }
        public string DatabaseType { get; set; }
        public string License { get; set; }
        public DcatStatusType FileStatus { get; set; }
        public string FileUpdated { get; set; }
        public IEnumerable<IDcatLanguageSpecificSettings> LanguageSpecificSettings { get; set; }
        #endregion
    }
}
