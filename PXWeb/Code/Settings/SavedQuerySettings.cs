using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using PCAxis.Query;

namespace PXWeb
{
    internal class SavedQuerySettings : ISavedQuerySettings
    {
        public SavedQueryStorageType StorageType { get; set; }
        public bool EnableCache { get; set; }
        public int CacheTime { get; set; }
        public bool EnableCORS { get; set; }
        public bool ShowPeriodAndId { get; set; }
        public bool EnableLimitRequest { get; set; }
        public int LimiterRequests { get; set; }
        public int LimiterTimespan { get; set; }
        public string LimiterHttpHeaderName { get; }

        #region "Public methods"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiNode">XML-node for the saved query settings</param>
        public SavedQuerySettings(XmlNode sqNode)
        {
            string xpath;

            xpath = "./storageType";
            StorageType = SettingsHelper.GetSettingValue(xpath, sqNode, SavedQueryStorageType.File);

            xpath = "./enableCache";
            EnableCache = SettingsHelper.GetSettingValue(xpath, sqNode, true);

            xpath = "./cacheTime";
            CacheTime = SettingsHelper.GetSettingValue(xpath, sqNode, 2);

            xpath = "./enableCORS";
            EnableCORS = SettingsHelper.GetSettingValue(xpath, sqNode, true);

            xpath = "./showPeriodAndId";
            ShowPeriodAndId = SettingsHelper.GetSettingValue(xpath, sqNode, false);

            xpath = "./enableLimiterRequest";
            EnableLimitRequest = SettingsHelper.GetSettingValue(xpath, sqNode, true);

            xpath = "./limiterRequests";
            LimiterRequests = SettingsHelper.GetSettingValue(xpath, sqNode, 10);

            xpath = "./limiterTimespan";
            LimiterTimespan = SettingsHelper.GetSettingValue(xpath, sqNode, 10);

            xpath = "./limiterHttpHeaderName";
            LimiterHttpHeaderName = SettingsHelper.GetSettingValue(xpath, sqNode, "");

        }

        /// <summary>
        /// Save Features.SavedQuery settings to the settings file
        /// </summary>
        /// <param name="sqNode">XML-node for the Features.SavedQuery settings</param>

        public void Save(XmlNode sqNode)
        {
            string xpath;

            xpath = "./storageType";
            SettingsHelper.SetSettingValue(xpath, sqNode, StorageType.ToString());

            xpath = "./enableCache";
            SettingsHelper.SetSettingValue(xpath, sqNode, EnableCache.ToString());

            xpath = "./cacheTime";
            SettingsHelper.SetSettingValue(xpath, sqNode, CacheTime.ToString());

            xpath = "./enableCORS";
            SettingsHelper.SetSettingValue(xpath, sqNode, EnableCORS.ToString());

            xpath = "./showPeriodAndId";
            SettingsHelper.SetSettingValue(xpath, sqNode, ShowPeriodAndId.ToString());

            xpath = "./enableLimiterRequest";
            SettingsHelper.SetSettingValue(xpath, sqNode, EnableLimitRequest.ToString());

            xpath = "./limiterRequests";
            SettingsHelper.SetSettingValue(xpath, sqNode, LimiterRequests.ToString());

            xpath = "./limiterTimespan";
            SettingsHelper.SetSettingValue(xpath, sqNode, LimiterTimespan.ToString());
        }
        #endregion
    }
    
}
