using PCAxis.Chart;
using System;
using System.Collections.Generic;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the Chart settings
    /// </summary>
    internal class ChartsSettings : IChartSettings
    {
        #region "Private fields"

        /// <summary>
        /// Chart colors
        /// </summary>
        private List<string> _colors;

        /// <summary>
        /// Chart.Font settings
        /// </summary>
        private ChartFontSettings _fontSettings;

        /// <summary>
        /// Chart.Legend settings
        /// </summary>
        private ChartLegendSettings _legendSettings;

        /// <summary>
        /// Chart.Guidelines settings
        /// </summary>
        private ChartGuidelinesSettings _guidelinesSettings;

        #endregion

        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="chartsNode">XML-node for the Charts settings</param>
        public ChartsSettings(XmlNode chartsNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./height";
            Height = SettingsHelper.GetSettingValue(xpath, chartsNode, 480);

            xpath = "./maxHeight";
            MaxHeight = SettingsHelper.GetSettingValue(xpath, chartsNode, 2000);

            xpath = "./width";
            Width = SettingsHelper.GetSettingValue(xpath, chartsNode, 680);

            xpath = "./maxWidth";
            MaxWidth = SettingsHelper.GetSettingValue(xpath, chartsNode, 3000);

            xpath = "./colors";
            node = SettingsHelper.GetNode(chartsNode, xpath);
            xpath = ".//color";
            _colors = SettingsHelper.GetSettingValue(xpath, node);

            xpath = "./lineThickness";
            LineThickness = SettingsHelper.GetSettingValue(xpath, chartsNode, 3);

            xpath = "./maxLineThickness";
            MaxLineThickness = SettingsHelper.GetSettingValue(xpath, chartsNode, 10);

            xpath = "./logotype";
            Logotype = SettingsHelper.GetSettingValue(xpath, chartsNode, "");

            xpath = "./maxValues";
            MaxValues = SettingsHelper.GetSettingValue(xpath, chartsNode, 50);

            xpath = "./timeSortOrder";
            TimeSortOrder = SettingsHelper.GetSettingValue(xpath, chartsNode, ChartSettings.SortType.None);

            xpath = "./labelOrientation";
            LabelOrientation = SettingsHelper.GetSettingValue(xpath, chartsNode, ChartSettings.OrientationType.Horizontal);

            xpath = "./font";
            node = SettingsHelper.GetNode(chartsNode, xpath);
            _fontSettings = new ChartFontSettings(node);

            xpath = "./legend";
            node = SettingsHelper.GetNode(chartsNode, xpath);
            _legendSettings = new ChartLegendSettings(node);

            xpath = "./guidelines";
            node = SettingsHelper.GetNode(chartsNode, xpath);
            _guidelinesSettings = new ChartGuidelinesSettings(node);

            xpath = "./showSourse";
            ShowSource = SettingsHelper.GetSettingValue(xpath, chartsNode, false);

            xpath = "./showLogo";
            ShowLogo = SettingsHelper.GetSettingValue(xpath, chartsNode, false);

            xpath = "./lineThicknessPhrame";
            LineThicknessPhrame = SettingsHelper.GetSettingValue(xpath, chartsNode, 1);

            xpath = "./lineColorPhrame";
            LineColorPhrame = SettingsHelper.GetSettingValue(xpath, chartsNode, "#000000");

            xpath = "./backgroundColorGraphs";
            BackgroundColorGraphs = SettingsHelper.GetSettingValue(xpath, chartsNode, "#FFFFFF");

            xpath = "./backgroundAlpha";
            BackgroundAlpha = SettingsHelper.GetSettingValue(xpath, chartsNode, 255);

            xpath = "./chartAlpha";
            ChartAlpha = SettingsHelper.GetSettingValue(xpath, chartsNode, 255);

            xpath = "./backgroundColor";
            BackgroundColor = SettingsHelper.GetSettingValue(xpath, chartsNode, "#FFFFFF");

        }

        /// <summary>
        /// Save Features.Charts settings to the settings file
        /// </summary>
        /// <param name="generalNode">XML-node for the Features.Charts settings</param>
        public void Save(XmlNode chartsNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./height";
            SettingsHelper.SetSettingValue(xpath, chartsNode, Height.ToString());

            xpath = "./maxHeight";
            SettingsHelper.SetSettingValue(xpath, chartsNode, MaxHeight.ToString());

            xpath = "./width";
            SettingsHelper.SetSettingValue(xpath, chartsNode, Width.ToString());

            xpath = "./maxWidth";
            SettingsHelper.SetSettingValue(xpath, chartsNode, MaxWidth.ToString());

            xpath = "./colors";
            SettingsHelper.SetSettingValue(xpath, chartsNode, "color", Colors);

            xpath = "./lineThickness";
            SettingsHelper.SetSettingValue(xpath, chartsNode, LineThickness.ToString());

            xpath = "./maxLineThickness";
            SettingsHelper.SetSettingValue(xpath, chartsNode, MaxLineThickness.ToString());

            xpath = "./logotype";
            SettingsHelper.SetSettingValue(xpath, chartsNode, Logotype);

            xpath = "./maxValues";
            SettingsHelper.SetSettingValue(xpath, chartsNode, MaxValues.ToString());

            xpath = "./timeSortOrder";
            SettingsHelper.SetSettingValue(xpath, chartsNode, TimeSortOrder.ToString());

            xpath = "./labelOrientation";
            SettingsHelper.SetSettingValue(xpath, chartsNode, LabelOrientation.ToString());

            xpath = "./font";
            node = SettingsHelper.GetNode(chartsNode, xpath);
            _fontSettings.Save(node);

            xpath = "./legend";
            node = SettingsHelper.GetNode(chartsNode, xpath);
            _legendSettings.Save(node);

            xpath = "./guidelines";
            node = SettingsHelper.GetNode(chartsNode, xpath);
            _guidelinesSettings.Save(node);

            xpath = "./showSourse";
            SettingsHelper.SetSettingValue(xpath, chartsNode, ShowSource.ToString());

            xpath = "./showLogo";
            SettingsHelper.SetSettingValue(xpath, chartsNode, ShowLogo.ToString());

            xpath = "./lineThicknessPhrame";
            SettingsHelper.SetSettingValue(xpath, chartsNode, LineThicknessPhrame.ToString());

            xpath = "./lineColorPhrame";
            SettingsHelper.SetSettingValue(xpath, chartsNode, LineColorPhrame.ToString());

            xpath = "./backgroundColorGraphs";
            SettingsHelper.SetSettingValue(xpath, chartsNode, BackgroundColorGraphs.ToString());

            xpath = "./backgroundAlpha";
            SettingsHelper.SetSettingValue(xpath, chartsNode, BackgroundAlpha.ToString());

            xpath = "./chartAlpha";
            SettingsHelper.SetSettingValue(xpath, chartsNode, ChartAlpha.ToString());

            xpath = "./backgroundColor";
            SettingsHelper.SetSettingValue(xpath, chartsNode, BackgroundColor.ToString());

        }

        #endregion

        #region IChartSettings Members

        public int Height { get; set; }
        public int MaxHeight { get; set; }
        public int Width { get; set; }
        public int MaxWidth { get; set; }
        public System.Collections.Generic.IEnumerable<string> Colors
        {
            get { return _colors; }
        }
        public int LineThickness { get; set; }
        public int MaxLineThickness { get; set; }
        public string Logotype { get; set; }
        public int MaxValues { get; set; }
        public bool ShowSource { get; set; }
        public bool ShowLogo { get; set; }
        public string BackgroundColorGraphs { get; set; }
        public int LineThicknessPhrame { get; set; }
        public String LineColorPhrame { get; set; }
        public string BackgroundColor { get; set; }
        public int BackgroundAlpha { get; set; }
        public int ChartAlpha { get; set; }
        public ChartSettings.SortType TimeSortOrder { get; set; }
        public ChartSettings.OrientationType LabelOrientation { get; set; }
        public IChartFontSettings Font
        {
            get { return _fontSettings; }
        }

        public IChartLegendSettings Legend
        {
            get { return _legendSettings; }
        }

        public IChartGuidelinesSettings Guidelines
        {
            get { return _guidelinesSettings; }
        }

        #endregion
    }
}
