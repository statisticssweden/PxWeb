using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the General.FileFormats.Excel settings
    /// </summary>
    internal class ExcelSettings : IExcelSettings
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="excelNode">XML-node for the General.FileFormats.Excel settings</param>
        public ExcelSettings(XmlNode excelNode)
        {
            string xpath;

            xpath = "./informationLevel";
            InformationLevel = SettingsHelper.GetSettingValue(xpath, excelNode, PCAxis.Paxiom.InformationLevelType.AllInformation);

            xpath = "./doubleColumn";
            DoubleColumn = SettingsHelper.GetSettingValue(xpath, excelNode, false);

            xpath = "./showDataNotes";
            ShowDataNotes = SettingsHelper.GetSettingValue(xpath, excelNode, false);

        }

        /// <summary>
        /// Save the General.FileFormats.Excel settings to the settings file
        /// </summary>
        /// <param name="excelNode">XML-node for the General.FileFormats.Excel settings</param>
        public void Save(XmlNode excelNode)
        {
            string xpath;

            xpath = "./informationLevel";
            SettingsHelper.SetSettingValue(xpath, excelNode, InformationLevel.ToString());

            xpath = "./doubleColumn";
            SettingsHelper.SetSettingValue(xpath, excelNode, DoubleColumn.ToString());

            xpath = "./showDataNotes";
            SettingsHelper.SetSettingValue(xpath, excelNode, ShowDataNotes.ToString());
        }

        #endregion


        #region IExcelSettings Members

        public PCAxis.Paxiom.InformationLevelType InformationLevel { get; set; }
        public bool DoubleColumn { get; set; }
        public bool ShowDataNotes { get; set; }

        #endregion
    }
}
