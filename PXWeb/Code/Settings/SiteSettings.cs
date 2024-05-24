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
            ApplicationName = SettingsHelper.GetSettingValue(xpath, siteNode, "PxWeb");

            xpath = "./logoPath";
            LogoPath = SettingsHelper.GetSettingValue(xpath, siteNode, "PX-WEb.gif");

            xpath = "./mainHeaderForTables";
            MainHeaderForTables = SettingsHelper.GetSettingValue(xpath, siteNode, MainHeaderForTablesType.TableName);

            xpath = "./showExternalSearchLink";
            ShowExternalSearchLink = SettingsHelper.GetSettingValue(xpath, siteNode, false);
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

            xpath = "./mainHeaderForTables";
            SettingsHelper.SetSettingValue(xpath, siteNode, MainHeaderForTables.ToString());

            xpath = "./showExternalSearchLink";
            SettingsHelper.SetSettingValue(xpath, siteNode, ShowExternalSearchLink.ToString());
        }

        #endregion

        #region ISiteSettings Members

        public string ApplicationName { get; set; }
        public string LogoPath { get; set; }

        public MainHeaderForTablesType MainHeaderForTables { get; set; }

        public bool ShowExternalSearchLink { get; set; }

        #endregion
    }
}
