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
    /// Internal class for reading and writing the General.FileFormats settings
    /// </summary>
    internal class FileFormatsSettings : IFileFormatsSettings
    {
        #region "Private fields"

        /// <summary>
        /// General.FileFormats.Excel settings
        /// </summary>
        private ExcelSettings _excelSettings;

        #endregion


        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileFormatsNode">XML-node for the General.FileFormats settings</param>
        public FileFormatsSettings(XmlNode fileFormatsNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./cellLimitDownloads";
            CellLimitDownloads = SettingsHelper.GetSettingValue(xpath, fileFormatsNode, 10000);

            xpath = "./excel";
            node = fileFormatsNode.SelectSingleNode(xpath);
            _excelSettings = new ExcelSettings(node);
        }

        /// <summary>
        /// Save the General.FileFormats settings to the settings file
        /// </summary>
        /// <param name="pathsNode">XML-node for the General.FileFormats settings</param>
        public void Save(XmlNode fileFormatsNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./cellLimitDownloads";
            SettingsHelper.SetSettingValue(xpath, fileFormatsNode, CellLimitDownloads.ToString());

            xpath = "./excel";
            node = fileFormatsNode.SelectSingleNode(xpath);
            _excelSettings.Save(node);
        }

        #endregion


        #region IFileFormatsSettings Members

        public int CellLimitDownloads { get; set; }

        public IExcelSettings Excel
        {
            get { return _excelSettings; }
        }

        #endregion
    }
}
