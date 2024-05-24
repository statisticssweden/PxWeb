using System;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing metadata settings
    /// </summary>
    internal class MetadataSettings : IMetadataSettings
    {
        private PCAxis.Metadata.IMetaIdProvider _metaLinkCreator;
        private string _metaLinkMethod;

        /// <summary>
        /// Log object
        /// </summary>
        private static log4net.ILog _logger;

        #region "IMetadataSettings members"

        public bool UseMetadata { get; set; }
        public string MetaSystemConfigFile { get; set; }

        public PCAxis.Metadata.IMetaIdProvider MetaLinkMethod
        {
            get { return _metaLinkCreator; }
        }
        #endregion

        #region "Public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="metadataNode">XML-node for the metadata settings</param>
        public MetadataSettings(XmlNode metadataNode)
        {
            string xpath;
            _logger = log4net.LogManager.GetLogger(typeof(SearchIndexSettings));

            xpath = "./useMetadata";
            UseMetadata = SettingsHelper.GetSettingValue(xpath, metadataNode, false);

            xpath = "./metaSystemConfigFile";
            MetaSystemConfigFile = SettingsHelper.GetSettingValue(xpath, metadataNode, "");

            xpath = "./metaLinkMethod";
            SetMetaLinkMethod(SettingsHelper.GetSettingValue(xpath, metadataNode, ""));
        }

        /// <summary>
        /// Save the metadata settings to the settings file
        /// </summary>
        /// <param name="menuNode">XML-node for the metadata settings</param>
        public void Save(XmlNode metadataNode)
        {
            string xpath;

            xpath = "./useMetadata";
            SettingsHelper.SetSettingValue(xpath, metadataNode, UseMetadata.ToString());

            xpath = "./metaSystemConfigFile";
            SettingsHelper.SetSettingValue(xpath, metadataNode, MetaSystemConfigFile);

            xpath = "./metaLinkMethod";
            SettingsHelper.SetSettingValue(xpath, metadataNode, GetMetaLinkMethod());
        }

        public void SetMetaLinkMethod(string method)
        {
            _metaLinkMethod = method;

            if (string.IsNullOrEmpty(_metaLinkMethod))
            {
                _metaLinkCreator = new PCAxis.Metadata.MetaLinkManager();
            }
            else
            {
                try
                {
                    var typeString = _metaLinkMethod;
                    var parts = typeString.Split(',');
                    var typeName = parts[0].Trim();
                    var assemblyName = parts[1].Trim();
                    _metaLinkCreator = (PCAxis.Metadata.IMetaIdProvider)Activator.CreateInstance(assemblyName, typeName).Unwrap();
                }
                catch (Exception)
                {
                    _metaLinkCreator = new PCAxis.Metadata.MetaLinkManager();
                    _logger.Error("Could not create MetaIdProvider '" + _metaLinkMethod + "' --> Default Meta Id provider created instead");
                }
            }
        }

        public string GetMetaLinkMethod()
        {
            return _metaLinkMethod;
        }

        #endregion
    }
}