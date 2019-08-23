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
    /// Internal class for reading and writing the Menu settings
    /// </summary>
    internal class MenuSettings : IMenuSettings
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="menuNode">XML-node for the Menu settings</param>
        public MenuSettings(XmlNode menuNode)
        {
            string xpath;
 
            xpath = "./menuMode";
            MenuMode = SettingsHelper.GetSettingValue(xpath, menuNode, MenuModeType.TreeViewWithFiles);

            xpath = "./numberOfValuesInDefaultView";
            NumberOfValuesInDefaultView = SettingsHelper.GetSettingValue(xpath, menuNode, 3);

            xpath = "./showRoot";
            ShowRoot = SettingsHelper.GetSettingValue(xpath, menuNode, false);

            xpath = "./expandAll";
            ExpandAll = SettingsHelper.GetSettingValue(xpath, menuNode, false);

            xpath = "./sortByAlias";
            SortByAlias = SettingsHelper.GetSettingValue(xpath, menuNode, true);

            xpath = "./showSelectLink";
            ShowSelectLink = SettingsHelper.GetSettingValue(xpath, menuNode, true);

            xpath = "./viewLinkMode";
            ViewLinkMode = SettingsHelper.GetSettingValue(xpath, menuNode, MenuViewLinkModeType.DefaultValues);

            xpath = "./showDownloadLink";
            ShowDownloadLink = SettingsHelper.GetSettingValue(xpath, menuNode, PCAxis.Web.Controls.DownloadLinkVisibilityType.ShowIfSmallFile);

            xpath = "./showModifiedDate";
            ShowModifiedDate = SettingsHelper.GetSettingValue(xpath, menuNode, true);

            xpath = "./showLastUpdated";
            ShowLastUpdated = SettingsHelper.GetSettingValue(xpath, menuNode, true);

            xpath = "./showFileSize";
            ShowFileSize = SettingsHelper.GetSettingValue(xpath, menuNode, true);

            xpath = "./showTableCategory";
            ShowTableCategory = SettingsHelper.GetSettingValue(xpath, menuNode, false);

            xpath = "./showTableUpdatedAfterPublish";
            ShowTableUpdatedAfterPublish = SettingsHelper.GetSettingValue(xpath, menuNode, false);

            xpath = "./showVariablesAndValues";
            ShowVariablesAndValues = SettingsHelper.GetSettingValue(xpath, menuNode, true);

            xpath = "./metadataAsIcons";
            MetadataAsIcons = SettingsHelper.GetSettingValue(xpath, menuNode, true);

            xpath = "./showTextToMetadata";
            ShowTextToMetadata = SettingsHelper.GetSettingValue(xpath, menuNode, true);

            xpath = "./showMenuExplanation";
            ShowMenuExplanation = SettingsHelper.GetSettingValue(xpath, menuNode, true);


        }

        /// <summary>
        /// Save the Menu settings to the settings file
        /// </summary>
        /// <param name="menuNode">XML-node for the Menu settings</param>
        public void Save(XmlNode menuNode)
        {
            string xpath;
            //XmlNode node;

            xpath = "./menuMode";
            SettingsHelper.SetSettingValue(xpath, menuNode, MenuMode.ToString());

            xpath = "./numberOfValuesInDefaultView";
            SettingsHelper.SetSettingValue(xpath, menuNode, NumberOfValuesInDefaultView.ToString());

            xpath = "./showRoot";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowRoot.ToString());

            xpath = "./expandAll";
            SettingsHelper.SetSettingValue(xpath, menuNode, ExpandAll.ToString());

            xpath = "./sortByAlias";
            SettingsHelper.SetSettingValue(xpath, menuNode, SortByAlias.ToString());

            xpath = "./showSelectLink";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowSelectLink.ToString());

            xpath = "./viewLinkMode";
            SettingsHelper.SetSettingValue(xpath, menuNode, ViewLinkMode.ToString());
            
            xpath = "./showDownloadLink";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowDownloadLink.ToString());

            xpath = "./showModifiedDate";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowModifiedDate.ToString());

            xpath = "./showLastUpdated";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowLastUpdated.ToString());

            xpath = "./showFileSize";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowFileSize.ToString());

            xpath = "./showTableCategory";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowTableCategory.ToString());

            xpath = "./showTableUpdatedAfterPublish";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowTableUpdatedAfterPublish.ToString());

            xpath = "./showVariablesAndValues";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowVariablesAndValues.ToString());

            xpath = "./metadataAsIcons";
            SettingsHelper.SetSettingValue(xpath, menuNode, MetadataAsIcons.ToString());

            xpath = "./showTextToMetadata";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowTextToMetadata.ToString());

            xpath = "./showMenuExplanation";
            SettingsHelper.SetSettingValue(xpath, menuNode, ShowMenuExplanation.ToString());
        }

        #endregion

        #region IMenuSettings Members

        public MenuModeType MenuMode { get; set; }
        public int NumberOfValuesInDefaultView { get; set; }
        public bool ShowRoot { get; set; }
        public bool ExpandAll { get; set; }
        public bool SortByAlias { get; set; }
        public bool ShowSelectLink { get; set; }
        public PCAxis.Web.Controls.DownloadLinkVisibilityType ShowDownloadLink { get; set; }
        public MenuViewLinkModeType ViewLinkMode { get; set; }
        public bool ShowModifiedDate { get; set; }
        public bool ShowLastUpdated { get; set; }
        public bool ShowFileSize { get; set; }
        public bool ShowTableCategory { get; set; }
        public bool ShowTableUpdatedAfterPublish { get; set; }
        public bool ShowVariablesAndValues { get; set; }
        public bool MetadataAsIcons { get; set; }
        public bool ShowTextToMetadata { get; set; }
        public bool ShowMenuExplanation { get; set; }
        #endregion

    }
}
