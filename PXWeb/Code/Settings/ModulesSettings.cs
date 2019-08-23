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
    /// Internal class for reading and writing the General.Modules settings
    /// </summary>
    internal class ModulesSettings : IModulesSettings
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modulesNode">XML-node for the General.Modules settings</param>
        public ModulesSettings(XmlNode modulesNode)
        {
            string xpath;
            //XmlNode node;

            xpath = "./showLogin";
            ShowLogin = SettingsHelper.GetSettingValue(xpath, modulesNode, false);

            xpath = "./usageLogging";
            UsageLogging = SettingsHelper.GetSettingValue(xpath, modulesNode, false);

            xpath = "./savedQueries";
            SavedQueries = SettingsHelper.GetSettingValue(xpath, modulesNode, false);

        }

        /// <summary>
        /// Save the General.Modules settings to the settings file
        /// </summary>
        /// <param name="pathsNode">XML-node for the General.Modules settings</param>
        public void Save(XmlNode modulesNode)
        {
            string xpath;
            //XmlNode node;

            xpath = "./showLogin";
            SettingsHelper.SetSettingValue(xpath, modulesNode, ShowLogin.ToString());

            xpath = "./usageLogging";
            SettingsHelper.SetSettingValue(xpath, modulesNode, UsageLogging.ToString());

            xpath = "./savedQueries";
            SettingsHelper.SetSettingValue(xpath, modulesNode, SavedQueries.ToString());
        }

        #endregion

        
        
        #region IModulesSettings Members

        public bool ShowLogin { get; set; }
        public bool UsageLogging { get; set; }
        public bool SavedQueries { get; set; }

        #endregion
    }
}
