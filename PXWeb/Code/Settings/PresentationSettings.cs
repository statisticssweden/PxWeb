using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the Presentation settings
    /// </summary>
    internal class PresentationSettings : IPresentationSettings
    {
        #region "Private fields"

        /// <summary>
        /// Presentation.Table settings
        /// </summary>
        private TableSettings _tableSettings;

        /// <summary>
        /// Presentation.CommandBarSettings
        /// </summary>
        private CommandBarSettings _commandBarSettings;

        #endregion

        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="presentationNode">XML-node for the Presentation settings</param>
        public PresentationSettings(XmlNode presentationNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./promptMandatoryFootnotes";
            PromptMandatoryFootnotes = SettingsHelper.GetSettingValue(xpath, presentationNode, true);

            xpath = "./NewTitleLayout";
            NewTitleLayout = SettingsHelper.GetSettingValue(xpath, presentationNode, true);

            xpath = "./table";
            node = presentationNode.SelectSingleNode(xpath);
            _tableSettings = new TableSettings(node);

            xpath = "./commandBar";
            node = presentationNode.SelectSingleNode(xpath);
            _commandBarSettings = new CommandBarSettings(node);
        }

        /// <summary>
        /// Save the Presentation settings to the settings file
        /// </summary>
        /// <param name="presentationNode">XML-node for the Presentation settings</param>
        public void Save(XmlNode presentationNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./promptMandatoryFootnotes";
            SettingsHelper.SetSettingValue(xpath, presentationNode, PromptMandatoryFootnotes.ToString());

            xpath = "./NewTitleLayout";
            SettingsHelper.SetSettingValue(xpath, presentationNode, NewTitleLayout.ToString());

            xpath = "./table";
            node = presentationNode.SelectSingleNode(xpath);
            _tableSettings.Save(node);

            xpath = "./commandBar";
            node = presentationNode.SelectSingleNode(xpath);
            _commandBarSettings.Save(node);
        }

        #endregion


        #region IPresentationSettings Members

        public bool PromptMandatoryFootnotes { get; set; }

        public bool NewTitleLayout { get; set; }

        public ITableSettings Table
        {
            get { return _tableSettings; }
        }

        public ICommandBarSettings CommandBar
        {
            get { return _commandBarSettings; }
        }

        #endregion
    }
}
