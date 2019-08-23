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
    /// Internal class for reading and writing the Chart guidelines settings
    /// </summary>
    internal class ChartGuidelinesSettings : IChartGuidelinesSettings
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fontsNode">XML-node for the Charts guidelines settings</param>
        public ChartGuidelinesSettings(XmlNode guidelineNode)
        {
            string xpath;

            xpath = "./color";
            Color = SettingsHelper.GetSettingValue(xpath, guidelineNode, "#AAAAAA");

            xpath = "./horizontal";
            Horizontal = SettingsHelper.GetSettingValue(xpath, guidelineNode, true);

            xpath = "./vertical";
            Vertical = SettingsHelper.GetSettingValue(xpath, guidelineNode, false);
        }

        /// <summary>
        /// Save Features.Charts.Guidelines settings to the settings file
        /// </summary>
        /// <param name="generalNode">XML-node for the Features.Charts.Guidelines settings</param>
        public void Save(XmlNode guidelineNode)
        {
            string xpath;

            xpath = "./color";
            SettingsHelper.SetSettingValue(xpath, guidelineNode, Color);

            xpath = "./horizontal";
            SettingsHelper.SetSettingValue(xpath, guidelineNode, Horizontal.ToString());

            xpath = "./vertical";
            SettingsHelper.SetSettingValue(xpath, guidelineNode, Vertical.ToString());
        }

        #endregion

        #region IChartGuidelinesSettings Members

        public string Color { get; set; }
        public bool Horizontal { get; set; }
        public bool Vertical { get; set; }

        #endregion
    }
}
