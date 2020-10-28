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
using System.Collections.Generic;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the Selection settings
    /// </summary>
    internal class SelectionSettings : ISelectionSettings
    {
        #region "Private fields"

        /// <summary>
        /// Selection.Hierarchies settings
        /// </summary>
        private HierarchiesSettings _hierarchiesSettings;

        /// <summary>
        /// Selection.MarkingTips settings
        /// </summary>
        private MarkingTipsSettings _markingTipsSettings;

        /// <summary>
        /// Presentation views
        /// </summary>
        private List<string> _presentationViews;

        /// <summary>
        /// Output formats (file formats)
        /// </summary>
        private List<string> _outputFormats;

        #endregion

        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="selectionNode">XML-node for the Selection settings</param>
        public SelectionSettings(XmlNode selectionNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./cellLimitScreen";
            CellLimitScreen = SettingsHelper.GetSettingValue(xpath, selectionNode, 10000);

            xpath = "./showMandatoryMark";
            ShowMandatoryMark = SettingsHelper.GetSettingValue(xpath, selectionNode, true);

            xpath = "./showSelectionLimits";
            ShowSelectionLimits = SettingsHelper.GetSettingValue(xpath, selectionNode, true);

            xpath = "./allowAggregations";
            AllowAggregations = SettingsHelper.GetSettingValue(xpath, selectionNode, true);

            xpath = "./hierarchies";
            node = selectionNode.SelectSingleNode(xpath);
            _hierarchiesSettings = new HierarchiesSettings(node);

            xpath = "./markingTips";
            node = selectionNode.SelectSingleNode(xpath);
            _markingTipsSettings = new MarkingTipsSettings(node);

            xpath = "./maxRowsWithoutSearch";
            MaxRowsWithoutSearch = SettingsHelper.GetSettingValue(xpath, selectionNode, 500);

            xpath = "./alwaysShowTimeVariableWithoutSearch";
            AlwaysShowTimeVariableWithoutSearch = SettingsHelper.GetSettingValue(xpath, selectionNode, false);

            xpath = "./listSize";
            ListSize = SettingsHelper.GetSettingValue(xpath, selectionNode, 6);

            xpath = "./presentationViews";
            node = SettingsHelper.GetNode(selectionNode, xpath);
            xpath = ".//plugin";
            _presentationViews = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./outputFormats";
            node = selectionNode.SelectSingleNode(xpath);
            xpath = ".//outputFormat";
            _outputFormats = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./valuesetMustBeSelectedFirst";
            ValuesetMustBeSelectedFirst = SettingsHelper.GetSettingValue(xpath, selectionNode, false);

            xpath = "./showAllAvailableValuesSearchButton";
            ShowAllAvailableValuesSearchButton = SettingsHelper.GetSettingValue(xpath, selectionNode, false);

            xpath = "./searchButtonMode";
            SearchButtonMode = SettingsHelper.GetSettingValue(xpath, selectionNode, PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode.ManyValues);

            xpath = "./titleFromMenu";
            TitleFromMenu = SettingsHelper.GetSettingValue(xpath, selectionNode, false);

            xpath = "./standardApplicationHeadTitle";
            StandardApplicationHeadTitle = SettingsHelper.GetSettingValue(xpath, selectionNode, false);

            xpath = "./metadataAsLinks";
            MetadataAsLinks = SettingsHelper.GetSettingValue(xpath, selectionNode, true);

            xpath = "./selectValuesFromGroup";
            SelectValuesFromGroup = SettingsHelper.GetSettingValue(xpath, selectionNode, false);

            xpath = "./buttonsForContentVariable";
            ButtonsForContentVariable = SettingsHelper.GetSettingValue(xpath, selectionNode, false);

            xpath = "./searchValuesBeginningOfWordCheckBoxDefaultChecked";
            SearchValuesBeginningOfWordCheckBoxDefaultChecked = SettingsHelper.GetSettingValue(xpath, selectionNode, false);

            xpath = "./preSelectFirstContentAndTime";
            PreSelectFirstContentAndTime = SettingsHelper.GetSettingValue(xpath, selectionNode, false);

            xpath = "./showNoFootnoteForSelection";
            ShowNoFootnoteForSelection = SettingsHelper.GetSettingValue(xpath, selectionNode, true);
        }
        
        /// <summary>
        /// Save the Selection settings to the settings file
        /// </summary>
        /// <param name="selectionNode">XML-node for the Selection settings</param>
        public void Save(XmlNode selectionNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./cellLimitScreen";
            SettingsHelper.SetSettingValue(xpath, selectionNode, CellLimitScreen.ToString());

            xpath = "./showMandatoryMark";
            SettingsHelper.SetSettingValue(xpath, selectionNode, ShowMandatoryMark.ToString());

            xpath = "./showSelectionLimits";
            SettingsHelper.SetSettingValue(xpath, selectionNode, ShowSelectionLimits.ToString());

            xpath = "./allowAggregations";
            SettingsHelper.SetSettingValue(xpath, selectionNode, AllowAggregations.ToString());

            xpath = "./hierarchies";
            node = selectionNode.SelectSingleNode(xpath);
            _hierarchiesSettings.Save(node);

            xpath = "./markingTips";
            node = selectionNode.SelectSingleNode(xpath);
            _markingTipsSettings.Save(node);

            xpath = "./maxRowsWithoutSearch";
            SettingsHelper.SetSettingValue(xpath, selectionNode, MaxRowsWithoutSearch.ToString());

            xpath = "./alwaysShowTimeVariableWithoutSearch";
            SettingsHelper.SetSettingValue(xpath, selectionNode, AlwaysShowTimeVariableWithoutSearch.ToString());

            xpath = "./listSize";
            SettingsHelper.SetSettingValue(xpath, selectionNode, ListSize.ToString());

            xpath = "./valuesetMustBeSelectedFirst";
            SettingsHelper.SetSettingValue(xpath, selectionNode, ValuesetMustBeSelectedFirst.ToString());

            xpath = "./showAllAvailableValuesSearchButton";
            SettingsHelper.SetSettingValue(xpath, selectionNode, ShowAllAvailableValuesSearchButton.ToString());

            xpath = "./searchButtonMode";
            SettingsHelper.SetSettingValue(xpath, selectionNode, SearchButtonMode.ToString());

            xpath = "./titleFromMenu";
            SettingsHelper.SetSettingValue(xpath, selectionNode, TitleFromMenu.ToString());

            xpath = "./standardApplicationHeadTitle";
            SettingsHelper.SetSettingValue(xpath, selectionNode, StandardApplicationHeadTitle.ToString());

            xpath = "./metadataAsLinks";
            SettingsHelper.SetSettingValue(xpath, selectionNode, MetadataAsLinks.ToString());

            xpath = "./selectValuesFromGroup";
            SettingsHelper.SetSettingValue(xpath, selectionNode, SelectValuesFromGroup.ToString());

            xpath = "./buttonsForContentVariable";
            SettingsHelper.SetSettingValue(xpath, selectionNode, ButtonsForContentVariable.ToString());

            xpath = "./searchValuesBeginningOfWordCheckBoxDefaultChecked";
            SettingsHelper.SetSettingValue(xpath, selectionNode, SearchValuesBeginningOfWordCheckBoxDefaultChecked.ToString());

            xpath = "./preSelectFirstContentAndTime";
            SettingsHelper.SetSettingValue(xpath, selectionNode, PreSelectFirstContentAndTime.ToString());

            xpath = "./showNoFootnoteForSelection";
            SettingsHelper.SetSettingValue(xpath, selectionNode, ShowNoFootnoteForSelection.ToString());
        }

        #endregion

        #region ISelectionSettings Members

        public int CellLimitScreen { get; set; }
        public bool ShowMandatoryMark { get; set; }
        public bool ShowSelectionLimits { get; set; }
        public bool AllowAggregations { get; set; }
        public IHierarchiesSettings Hierarchies
        {
            get { return _hierarchiesSettings; }
        }

        public IMarkingTipsSettings MarkingTips
        {
            get { return _markingTipsSettings; }
        }

        public int MaxRowsWithoutSearch { get; set; }
        public bool AlwaysShowTimeVariableWithoutSearch { get; set; }       
        public int ListSize { get; set; }

        public bool ValuesetMustBeSelectedFirst { get; set; }
        public bool ShowAllAvailableValuesSearchButton { get; set; }

        public PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode SearchButtonMode { get; set; }

        public bool TitleFromMenu { get; set; }
        public bool StandardApplicationHeadTitle { get; set; }
        public bool MetadataAsLinks { get; set; }
        public bool SelectValuesFromGroup { get; set; }
        public bool ButtonsForContentVariable { get; set; }
        public bool SearchValuesBeginningOfWordCheckBoxDefaultChecked { get; set; }

        public bool PreSelectFirstContentAndTime { get; set; }

        public bool ShowNoFootnoteForSelection { get; set; }

        #endregion

    }
}
