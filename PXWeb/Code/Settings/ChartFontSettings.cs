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
    /// Internal class for reading and writing the Chart font settings
    /// </summary>
    internal class ChartFontSettings : IChartFontSettings
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fontsNode">XML-node for the Charts font settings</param>
        public ChartFontSettings(XmlNode fontNode)
        {
            string xpath;

            xpath = "./name";
            Name = SettingsHelper.GetSettingValue(xpath, fontNode, "Verdana");

            xpath = "./titleSize";
            TitleSize = SettingsHelper.GetSettingValue(xpath, fontNode, 18);

            xpath = "./axisSize";
            AxisSize = SettingsHelper.GetSettingValue(xpath, fontNode, 12);
        }

        /// <summary>
        /// Save Features.Charts.Fonts settings to the settings file
        /// </summary>
        /// <param name="generalNode">XML-node for the Features.Charts.Fonts settings</param>
        public void Save(XmlNode fontNode)
        {
            string xpath;

            xpath = "./name";
            SettingsHelper.SetSettingValue(xpath, fontNode, Name);

            xpath = "./titleSize";
            SettingsHelper.SetSettingValue(xpath, fontNode, TitleSize.ToString());

            xpath = "./axisSize";
            SettingsHelper.SetSettingValue(xpath, fontNode, AxisSize.ToString());
        }

        #endregion

        #region IChartFontSettings Members

        public string Name { get; set; }
        public int TitleSize { get; set; }
        public int AxisSize { get; set; }

        #endregion
    }
}
