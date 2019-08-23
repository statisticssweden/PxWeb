using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the Navigation settings
    /// </summary>
    internal class NavigationSettings: INavigationSettings
    {
       #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="menuNode">XML-node for the navigation flow settings</param>
        public NavigationSettings(XmlNode navigationNode)
        {
            string xpath;

            xpath = "./showNavigationFlow";
            ShowNavigationFlow = SettingsHelper.GetSettingValue(xpath, navigationNode, true);
        }

        /// <summary>
        /// Save the navigation settings to the settings file
        /// </summary>
        /// <param name="pathsNode">XML-node for the navigation settings</param>
        public void Save(XmlNode pathsNode)
        {
            string xpath;

            xpath = "./showNavigationFlow";
            SettingsHelper.SetSettingValue(xpath, pathsNode, ShowNavigationFlow.ToString());
        }
        #endregion
        #region INavigationSettings Members

        public bool ShowNavigationFlow { get; set; }


        #endregion
    }
}