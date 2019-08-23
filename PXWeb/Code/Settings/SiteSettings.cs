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
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the General.Site settings
    /// </summary>
    internal class SiteSettings : ISiteSettings
    {
       
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="siteNode">XML-node for the General.Site settings</param>
        public SiteSettings(XmlNode siteNode)
        {
            string xpath;

            //Load the Site settings 
            xpath = "./applicationName";
            ApplicationName = SettingsHelper.GetSettingValue(xpath, siteNode, "PX-Web");

            xpath = "./logoPath";
            LogoPath = SettingsHelper.GetSettingValue(xpath, siteNode, "PX-WEb.gif");

        }

        /// <summary>
        /// Save the General.Site settings to the settings file
        /// </summary>
        /// <param name="siteNode">XML-node for the General.Site settings</param>
        public void Save(XmlNode siteNode)
        {
            string xpath;

            xpath = "./applicationName";
            SettingsHelper.SetSettingValue(xpath, siteNode, ApplicationName);

            xpath = "./logoPath";
            SettingsHelper.SetSettingValue(xpath, siteNode, LogoPath);
        }

        #endregion

        #region ISiteSettings Members

        public string ApplicationName { get; set; }
        public string LogoPath { get; set; }
        
        #endregion
    }
}
