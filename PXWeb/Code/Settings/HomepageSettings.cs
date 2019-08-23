using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing homepage settings for a specific language
    /// </summary>
    internal class HomepageSettings : IHomepageSettings
    {
        #region "public methods"


        /// <summary>
        /// Default constructor
        /// </summary>
        public HomepageSettings()
        {
            Language = "";
            Url = "~/Default.aspx";
            IsExternal = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="homepageNode">XML node for homepage information for a specific language</param>
        public HomepageSettings(XmlNode homepageNode)
        {
            string xpath;

            xpath = "./language";
            Language = SettingsHelper.GetSettingValue(xpath, homepageNode, "");

            xpath = "./url";
            Url = SettingsHelper.GetSettingValue(xpath, homepageNode, "~/Default.aspx");

            xpath = "./isExternal";
            IsExternal = SettingsHelper.GetSettingValue(xpath, homepageNode, false);
        }

        /// <summary>
        /// Save homepage information to settings file
        /// </summary>
        /// <param name="homepageNode"></param>
        public void Save(XmlNode homepageNode)
        {
            string xpath;

            xpath = "./language";
            SettingsHelper.SetSettingValue(xpath, homepageNode, Language);

            xpath = "./url";
            SettingsHelper.SetSettingValue(xpath, homepageNode, Url);

            xpath = "./isExternal";
            SettingsHelper.SetSettingValue(xpath, homepageNode, IsExternal.ToString());
        }

        #endregion

        #region IHomepageSettings Members

        public string Language { get; set; }
        public string Url { get; set; }
        public bool IsExternal { get; set; }

        #endregion
    }
}