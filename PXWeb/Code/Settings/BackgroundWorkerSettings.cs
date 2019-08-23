using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the background worker process settings
    /// </summary>
    internal class BackgroundWorkerSettings : IBackgroundWorkerSettings
    {
        #region "Public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiNode">XML-node for the background worker process settings</param>
        public BackgroundWorkerSettings(XmlNode node)
        {
            string xpath;

            xpath = "./sleepTime";
            SleepTime = SettingsHelper.GetSettingValue(xpath, node, 3600);
        }

        /// <summary>
        /// Save background worker process settings to the settings file
        /// </summary>
        /// <param name="apiNode">XML-node for the background worker process settings</param>
        public void Save(XmlNode node)
        {
            string xpath;

            xpath = "./sleepTime";
            SettingsHelper.SetSettingValue(xpath, node, SleepTime.ToString());
        }      
  
        #endregion
        
        #region "IBackgroundWorkerSettings members"

        public int SleepTime { get; set; }

        #endregion
    }
}