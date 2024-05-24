using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the General.Paths settings
    /// </summary>
    internal class PathsSettings : IPathsSettings
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathsNode">XML-node for the General.Paths settings</param>
        public PathsSettings(XmlNode pathsNode)
        {
            string xpath;

            xpath = "./languagesPath";
            LanguagesPath = SettingsHelper.GetSettingValue(xpath, pathsNode, "~/Resources/Languages");

            xpath = "./imagesPath";
            ImagesPath = SettingsHelper.GetSettingValue(xpath, pathsNode, "~/Resources/Images/");

            xpath = "./pxDatabasesPath";
            PxDatabasesPath = SettingsHelper.GetSettingValue(xpath, pathsNode, "~/Resources/PX/Databases/");

            xpath = "./pxAggreagationsPath";
            PxAggregationsPath = SettingsHelper.GetSettingValue(xpath, pathsNode, "~/Resources/PX/Aggregations/");
        }

        /// <summary>
        /// Save the General.Paths settings to the settings file
        /// </summary>
        /// <param name="pathsNode">XML-node for the General.Paths settings</param>
        public void Save(XmlNode pathsNode)
        {
            string xpath;

            xpath = "./languagesPath";
            SettingsHelper.SetSettingValue(xpath, pathsNode, LanguagesPath);

            xpath = "./imagesPath";
            SettingsHelper.SetSettingValue(xpath, pathsNode, ImagesPath);

            xpath = "./pxDatabasesPath";
            SettingsHelper.SetSettingValue(xpath, pathsNode, PxDatabasesPath);

            xpath = "./pxAggreagationsPath";
            SettingsHelper.SetSettingValue(xpath, pathsNode, PxAggregationsPath);
        }

        #endregion

        #region IPathsSettings Members

        public string LanguagesPath { get; set; }
        public string ImagesPath { get; set; }
        public string PxDatabasesPath { get; set; }
        public string PxAggregationsPath { get; set; }

        #endregion
    }
}
