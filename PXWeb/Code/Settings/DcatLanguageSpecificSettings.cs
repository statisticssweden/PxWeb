using System.Xml;

namespace PXWeb
{
    internal class DcatLanguageSpecificSettings : IDcatLanguageSpecificSettings
    {
        #region "Private fields"

        #endregion

        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">XML-node for the Dcat settings</param>
        public DcatLanguageSpecificSettings(XmlNode node)
        {
            string xpath;

            xpath = "./Language";
            Language = SettingsHelper.GetSettingValue(xpath, node, "");

            xpath = "./CatalogTitle";
            CatalogTitle = SettingsHelper.GetSettingValue(xpath, node, "Catalog title");

            xpath = "./CatalogDescription";
            CatalogDescription = SettingsHelper.GetSettingValue(xpath, node, "Catalog description");

            xpath = "./PublisherName";
            PublisherName = SettingsHelper.GetSettingValue(xpath, node, "Publisher");

        }
        /// <summary>
        /// Constructor
        /// </summary>
        public DcatLanguageSpecificSettings(string language, string catalogTitle, string catalogDescription, string publisherName)
        {
            Language = language;
            CatalogTitle = catalogTitle;
            CatalogDescription = catalogDescription;
            PublisherName = publisherName;
        }

        /// <summary>
        /// Save Dcat settings to the settings file
        /// </summary>
        /// <param name="generalNode">XML-node for the Dcat settings</param>
        public void Save(XmlNode node)
        {
            string xpath;

            xpath = "./Language";
            SettingsHelper.SetSettingValue(xpath, node, Language);

            xpath = "./CatalogTitle";
            SettingsHelper.SetSettingValue(xpath, node, CatalogTitle);

            xpath = "./CatalogDescription";
            SettingsHelper.SetSettingValue(xpath, node, CatalogDescription);


            xpath = "./PublisherName";
            SettingsHelper.SetSettingValue(xpath, node, PublisherName);

        }

        public string Language { get; set; }
        public string CatalogTitle { get; set; }
        public string CatalogDescription { get; set; }
        public string PublisherName { get; set; }
        #endregion
    }
}