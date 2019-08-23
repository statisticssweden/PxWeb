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
    /// Internal class for reading and writing the Presentation.Table.Attribute settings
    /// </summary>
    internal class AttributeSettings : IAttributeSettings
    {
        #region "public methods"
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attributeNode">XML-node for the Attribute settings</param>
        public AttributeSettings(XmlNode attributeNode)
        {
            string xpath;

            xpath = "./displayAttributes";
            DisplayAttributes = SettingsHelper.GetSettingValue(xpath, attributeNode, false);

            xpath = "./displayDefaultAttributes";
            DisplayDefaultAttributes = SettingsHelper.GetSettingValue(xpath, attributeNode, false);

        }

        /// <summary>
        /// Save Presentation.Table.Attribute settings to the settings file
        /// </summary>
        /// <param name="attributeNode">XML-node for the Attribute settings</param>
        public void Save(XmlNode attributeNode)
        {
            string xpath;

            xpath = "./displayAttributes";
            SettingsHelper.SetSettingValue(xpath, attributeNode, DisplayAttributes.ToString());

            xpath = "./displayDefaultAttributes";
            SettingsHelper.SetSettingValue(xpath, attributeNode, DisplayDefaultAttributes.ToString());
        }

        #endregion

        #region IAttributeSettings Members

        public bool DisplayAttributes { get; set; }
        public bool DisplayDefaultAttributes { get; set; }

        #endregion
    }
}
