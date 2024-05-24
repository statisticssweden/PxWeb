using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the Chart legend settings
    /// </summary>
    internal class ChartLegendSettings : IChartLegendSettings
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fontsNode">XML-node for the Charts legend settings</param>
        public ChartLegendSettings(XmlNode legendNode)
        {
            string xpath;

            xpath = "./visible";
            Visible = SettingsHelper.GetSettingValue(xpath, legendNode, true);

            xpath = "./fontSize";
            FontSize = SettingsHelper.GetSettingValue(xpath, legendNode, 12);

            xpath = "./height";
            Height = SettingsHelper.GetSettingValue(xpath, legendNode, 20);
        }

        /// <summary>
        /// Save Features.Charts.Legend settings to the settings file
        /// </summary>
        /// <param name="generalNode">XML-node for the Features.Charts.Legend settings</param>
        public void Save(XmlNode legendNode)
        {
            string xpath;

            xpath = "./visible";
            SettingsHelper.SetSettingValue(xpath, legendNode, Visible.ToString());

            xpath = "./fontSize";
            SettingsHelper.SetSettingValue(xpath, legendNode, FontSize.ToString());

            xpath = "./height";
            SettingsHelper.SetSettingValue(xpath, legendNode, Height.ToString());
        }

        #endregion


        #region IChartLegendSettings Members

        public bool Visible { get; set; }
        public int FontSize { get; set; }
        public int Height { get; set; }

        #endregion
    }
}
