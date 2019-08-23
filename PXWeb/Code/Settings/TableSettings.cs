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
    /// Internal class for reading and writing the Presentation.Table settings
    /// </summary>
    internal class TableSettings : ITableSettings
    {
        #region "Private fields"

        /// <summary>
        /// Table.Attribute settings
        /// </summary>
        private AttributeSettings _attributeSettings;

        #endregion

        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableNode">XML-node for the Presentation.Table settings</param>
        public TableSettings(XmlNode tableNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./tableTransformation";
            TableTransformation = SettingsHelper.GetSettingValue(xpath, tableNode, PCAxis.Web.Controls.TableTransformationType.NoTransformation);

            xpath = "./defaultLayout";
            DefaultLayout = SettingsHelper.GetSettingValue(xpath, tableNode, PCAxis.Web.Controls.TableLayoutType.Layout1);

            xpath = "./maxColumns";
            MaxColumns = SettingsHelper.GetSettingValue(xpath, tableNode, 50);

            xpath = "./maxRows";
            MaxRows = SettingsHelper.GetSettingValue(xpath, tableNode, 10000);

            xpath = "./titleVisible";
            TitleVisible = SettingsHelper.GetSettingValue(xpath, tableNode, true);

            xpath = "./attributes";
            node = SettingsHelper.GetNode(tableNode, xpath);
            _attributeSettings = new AttributeSettings(node);

        }

        /// <summary>
        /// Save the Presentation.Table settings to the settings file
        /// </summary>
        /// <param name="tableNode">XML-node for the Presentation.Table settings</param>
        public void Save(XmlNode tableNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./tableTransformation";
            SettingsHelper.SetSettingValue(xpath, tableNode, TableTransformation.ToString());

            xpath = "./defaultLayout";
            SettingsHelper.SetSettingValue(xpath, tableNode, DefaultLayout.ToString());

            xpath = "./maxColumns";
            SettingsHelper.SetSettingValue(xpath, tableNode, MaxColumns.ToString());

            xpath = "./maxRows";
            SettingsHelper.SetSettingValue(xpath, tableNode, MaxRows.ToString());

            xpath = "./titleVisible";
            SettingsHelper.SetSettingValue(xpath, tableNode, TitleVisible.ToString());

            xpath = "./attributes";
            node = SettingsHelper.GetNode(tableNode, xpath);
            _attributeSettings.Save(node);

        }

        #endregion
        
        
        #region ITableSettings Members

        public PCAxis.Web.Controls.TableTransformationType TableTransformation { get; set; }
        public PCAxis.Web.Controls.TableLayoutType DefaultLayout { get; set; }
        public int MaxColumns { get; set; }
        public int MaxRows { get; set; }
        public bool TitleVisible { get; set; }
        public IAttributeSettings Attributes
        {
            get { return _attributeSettings; }
        }

        #endregion
    }
}
