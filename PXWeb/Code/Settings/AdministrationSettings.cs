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
    /// Internal class for reading and writing the General.Administration settings
    /// </summary>
    internal class AdministrationSettings : IAdministrationSettings
    {
        #region Private fields
        /// <summary>
        /// IP addresses with permission to access the administration pages
        /// </summary>
        private List<string> _ipAddresses;
        /// <summary>
        /// Log
        /// </summary>
        private static log4net.ILog _logger;
        #endregion

        #region Public methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databasesNode">XML-node for the General.Administration settings</param>
        public AdministrationSettings(XmlNode administrationNode)
        {
            string xpath;
            XmlNode node;
            //XmlNodeList nodeList;

            _logger = log4net.LogManager.GetLogger(typeof(AdministrationSettings));

            xpath = "./useIPFilter";
            UseIPFilter = SettingsHelper.GetSettingValue(xpath, administrationNode, true);

            xpath = "./ipAddresses";
            node = administrationNode.SelectSingleNode(xpath);
            xpath = ".//ipAddress";
            _ipAddresses = SettingsHelper.GetSettingValue(xpath, node);

        }

        /// <summary>
        /// Save the General.Administration settings to the settings file
        /// </summary>
        /// <param name="administrationNode">XML-node for the General.Administration settings</param>
        public void Save(XmlNode administrationNode)
        {
            string xpath;

            xpath = "./useIPFilter";
            SettingsHelper.SetSettingValue(xpath, administrationNode, UseIPFilter.ToString());

            xpath = "./ipAddresses";
            SettingsHelper.SetSettingValue(xpath, administrationNode, "ipAddress", IPAddresses);
        }       

        #endregion

        #region IAdministrationSettings Members

        public bool UseIPFilter { get; set; }

        public System.Collections.Generic.IEnumerable<string> IPAddresses
        {
            get { return _ipAddresses; }
        }

        #endregion
    }
}
