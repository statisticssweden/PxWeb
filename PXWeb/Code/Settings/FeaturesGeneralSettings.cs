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
    /// Internal class for reading and writing the Features.General settings
    /// </summary>
    public class FeaturesGeneralSettings : IFeaturesGeneralSettings
    {
        #region Public methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databasesNode">XML-node for the Features.General settings</param>
        public FeaturesGeneralSettings(XmlNode featuresGeneralNode)
        {
            string xpath;

            xpath = "./chartsEnabled";
            ChartsEnabled = SettingsHelper.GetSettingValue(xpath, featuresGeneralNode, true);
            xpath = "./apiEnabled";
            ApiEnabled = SettingsHelper.GetSettingValue(xpath, featuresGeneralNode, false);
            xpath = "./saveQueryEnabled";
            SavedQueryEnabled = SettingsHelper.GetSettingValue(xpath, featuresGeneralNode, false);
            xpath = "./userFriendlyUrlsEnabled";
            UserFriendlyUrlsEnabled = SettingsHelper.GetSettingValue(xpath, featuresGeneralNode, false);
            xpath = "./userStatisticsEnabled";
            UserStatisticsEnabled = SettingsHelper.GetSettingValue(xpath, featuresGeneralNode, false);
            xpath = "./searchEnabled";
            SearchEnabled = SettingsHelper.GetSettingValue(xpath, featuresGeneralNode, false);
            xpath = "./backgroundWorkerEnabled";
            BackgroundWorkerEnabled = SettingsHelper.GetSettingValue(xpath, featuresGeneralNode, false);
            xpath = "./bulkLinkEnabled";
            BulkLinkEnabled = SettingsHelper.GetSettingValue(xpath, featuresGeneralNode, false);
            xpath = "./clearCache";
            ClearCache = SettingsHelper.GetSettingValue(xpath, featuresGeneralNode, "");

        }

        /// <summary>
        /// Save the Features.General settings to the settings file
        /// </summary>
        /// <param name="administrationNode">XML-node for the General.Administration settings</param>
        public void Save(XmlNode featuresGeneralNode)
        {
            string xpath;

            xpath = "./chartsEnabled";
            SettingsHelper.SetSettingValue(xpath, featuresGeneralNode, ChartsEnabled.ToString());
            xpath = "./apiEnabled";
            SettingsHelper.SetSettingValue(xpath, featuresGeneralNode, ApiEnabled.ToString());
            xpath = "./saveQueryEnabled";
            SettingsHelper.SetSettingValue(xpath, featuresGeneralNode, SavedQueryEnabled.ToString());
            xpath = "./userFriendlyUrlsEnabled";
            SettingsHelper.SetSettingValue(xpath, featuresGeneralNode, UserFriendlyUrlsEnabled.ToString());
            xpath = "./userStatisticsEnabled";
            SettingsHelper.SetSettingValue(xpath, featuresGeneralNode, UserStatisticsEnabled.ToString());
            xpath = "./searchEnabled";
            SettingsHelper.SetSettingValue(xpath, featuresGeneralNode, SearchEnabled.ToString());
            xpath = "./backgroundWorkerEnabled";
            SettingsHelper.SetSettingValue(xpath, featuresGeneralNode, BackgroundWorkerEnabled.ToString());
            xpath = "./bulkLinkEnabled";
            SettingsHelper.SetSettingValue(xpath, featuresGeneralNode, BulkLinkEnabled.ToString());
            xpath = "./clearCache";
            SettingsHelper.SetSettingValue(xpath, featuresGeneralNode, ClearCache.ToString());
        }       

        #endregion

        #region IFeaturesGeneralSettings Members

        public bool ChartsEnabled { get; set; }
        public bool ApiEnabled { get; set; }
        public bool SavedQueryEnabled { get; set; }
        public bool UserFriendlyUrlsEnabled { get; set; }
        public bool UserStatisticsEnabled { get; set; }
        public bool SearchEnabled { get; set; }
        public bool BackgroundWorkerEnabled { get; set; }
        public bool BulkLinkEnabled { get; set; }
        public string ClearCache { get; set; }
        

        #endregion
    }
}
