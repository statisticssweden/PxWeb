using System.Collections.Generic;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the Presentation.CommandBarSettings settings
    /// </summary>
    internal class CommandBarSettings : ICommandBarSettings
    {
        #region "Private fields"

        private List<string> _operations;
        private List<string> _operationShortcuts;
        private List<string> _outputFormats;
        private List<string> _outputFormatShortcuts;
        private List<string> _presentationViews;
        private List<string> _presentationViewShortcuts;
        private List<string> _commandBarShortcuts;
        private List<string> _operationButtons;
        private List<string> _fileTypeButtons;
        private List<string> _presentationViewButtons;

        #endregion

        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandBarNode">XML-node for the Presentation.CommandBar settings</param>
        public CommandBarSettings(XmlNode commandBarNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./viewMode";
            ViewMode = SettingsHelper.GetSettingValue(xpath, commandBarNode, PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown);

            xpath = "./operations";
            node = commandBarNode.SelectSingleNode(xpath);
            xpath = ".//plugin";
            _operations = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./operationShortcuts";
            node = commandBarNode.SelectSingleNode(xpath);
            xpath = ".//plugin";
            _operationShortcuts = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./outputFormats";
            node = commandBarNode.SelectSingleNode(xpath);
            xpath = ".//outputFormat";
            _outputFormats = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./outputFormatShortcuts";
            node = commandBarNode.SelectSingleNode(xpath);
            xpath = ".//outputFormat";
            _outputFormatShortcuts = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./presentationViews";
            //node = commandBarNode.SelectSingleNode(xpath);
            node = SettingsHelper.GetNode(commandBarNode, xpath);
            xpath = ".//plugin";
            _presentationViews = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./presentationViewShortcuts";
            //node = commandBarNode.SelectSingleNode(xpath);
            node = SettingsHelper.GetNode(commandBarNode, xpath);
            xpath = ".//plugin";
            _presentationViewShortcuts = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./commandBarShortcuts";
            node = commandBarNode.SelectSingleNode(xpath);
            xpath = ".//plugin";
            _commandBarShortcuts = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./operationButtons";
            node = commandBarNode.SelectSingleNode(xpath);
            xpath = ".//plugin";
            _operationButtons = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./fileTypeButtons";
            node = commandBarNode.SelectSingleNode(xpath);
            xpath = ".//outputFormat";
            _fileTypeButtons = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./presentationViewButtons";
            node = SettingsHelper.GetNode(commandBarNode, xpath);
            xpath = ".//plugin";
            _presentationViewButtons = SettingsHelper.GetSettingValue(xpath, node);

        }

        /// <summary>
        /// Save the Presentation.CommandBar settings to the settings file
        /// </summary>
        /// <param name="commandBarNode">XML-node for the Presentation.CommandBar settings</param>
        public void Save(XmlNode commandBarNode)
        {
            string xpath;

            xpath = "./viewMode";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, ViewMode.ToString());

            xpath = "./operations";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "plugin", Operations);

            xpath = "./operationShortcuts";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "plugin", OperationShortcuts);

            xpath = "./outputFormats";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "outputFormat", OutputFormats);

            xpath = "./outputFormatShortcuts";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "outputFormat", OutputFormatShortcuts);

            xpath = "./presentationViews";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "plugin", PresentationViews);

            xpath = "./presentationViewShortcuts";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "plugin", PresentationViewShortcuts);

            xpath = "./commandBarShortcuts";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "plugin", CommandBarShortcuts);

            xpath = "./operationButtons";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "plugin", OperationButtons);

            xpath = "./fileTypeButtons";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "outputFormat", FileTypeButtons);

            xpath = "./presentationViewButtons";
            SettingsHelper.SetSettingValue(xpath, commandBarNode, "plugin", PresentationViewButtons);

        }

        /// <summary>
        /// Initializes CommandBar to standard PXWeb settings
        /// </summary>
        /// <param name="cmdBar">CommandBar to initialize</param>
        public static void InitializeCommandBar(PCAxis.Web.Controls.CommandBar.CommandBar cmdBar)
        {
            cmdBar.ViewMode = PXWeb.Settings.Current.Presentation.CommandBar.ViewMode;
            switch (cmdBar.ViewMode)
            {
                case PCAxis.Web.Controls.CommandBar.CommandBarViewMode.Hidden:
                    cmdBar.Visible = false;
                    break;
                case PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown:
                    cmdBar.Operations = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.Operations;
                    cmdBar.OperationShortcuts = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OperationShortcuts;
                    cmdBar.OutputFormats = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OutputFormats;
                    cmdBar.FileformatShortcuts = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OutputFormatShortcuts;
                    cmdBar.PresentationViews = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.PresentationViews;
                    cmdBar.PresentationViewShortcuts = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.PresentationViewShortcuts;
                    cmdBar.CommandbarShortcuts = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.CommandBarShortcuts;
                    break;
                case PCAxis.Web.Controls.CommandBar.CommandBarViewMode.ImageButtons:
                    cmdBar.OperationButtons = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.OperationButtons;
                    cmdBar.FiletypeButtons = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.FileTypeButtons;
                    cmdBar.PresentationViewButtons = (List<string>)PXWeb.Settings.Current.Presentation.CommandBar.PresentationViewButtons;
                    break;
                default:
                    cmdBar.Visible = false;
                    break;
            }
        }

        #endregion

        #region ICommandBarSettings Members

        public PCAxis.Web.Controls.CommandBar.CommandBarViewMode ViewMode { get; set; }

        public System.Collections.Generic.IEnumerable<string> Operations
        {
            get { return _operations; }
        }

        public System.Collections.Generic.IEnumerable<string> OperationShortcuts
        {
            get { return _operationShortcuts; }
        }

        public System.Collections.Generic.IEnumerable<string> OutputFormats
        {
            get { return _outputFormats; }
        }

        public System.Collections.Generic.IEnumerable<string> OutputFormatShortcuts
        {
            get { return _outputFormatShortcuts; }
        }

        public System.Collections.Generic.IEnumerable<string> PresentationViews
        {
            get { return _presentationViews; }
        }

        public System.Collections.Generic.IEnumerable<string> PresentationViewShortcuts
        {
            get { return _presentationViewShortcuts; }
        }

        public System.Collections.Generic.IEnumerable<string> CommandBarShortcuts
        {
            get { return _commandBarShortcuts; }
        }

        public System.Collections.Generic.IEnumerable<string> OperationButtons
        {
            get { return _operationButtons; }
        }

        public System.Collections.Generic.IEnumerable<string> FileTypeButtons
        {
            get { return _fileTypeButtons; }
        }

        public System.Collections.Generic.IEnumerable<string> PresentationViewButtons
        {
            get { return _presentationViewButtons; }
        }

        #endregion
    }
}
