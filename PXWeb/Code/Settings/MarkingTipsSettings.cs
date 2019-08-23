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
    /// Internal class for reading and writing the Selection.MarkingTips settings
    /// </summary>
    internal class MarkingTipsSettings : IMarkingTipsSettings
    {
         #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="markingTipsNode">XML-node for the Selection.MarkingTips settings</param>
        public MarkingTipsSettings(XmlNode markingTipsNode)
        {
            string xpath;

            xpath = "./showMarkingTips";
            ShowMarkingTips = SettingsHelper.GetSettingValue(xpath, markingTipsNode, true);

            //xpath = "./markingTipsUrl";
            //MarkingTipsUrl = SettingsHelper.GetSettingValue(xpath, markingTipsNode, "MarkingTips.aspx");
        }

        /// <summary>
        /// Save the Selection.MarkingTips settings to the settings file.
        /// </summary>
        /// <param name="markingTipsNode">XML-node for the Selection.MarkingTips settings</param>
        public void Save(XmlNode markingTipsNode)
        {
            string xpath;

            xpath = "./showMarkingTips";
            SettingsHelper.SetSettingValue(xpath, markingTipsNode, ShowMarkingTips.ToString());

            //xpath = "./markingTipsUrl";
            //SettingsHelper.SetSettingValue(xpath, markingTipsNode, MarkingTipsUrl);
        }

        #endregion
        
        
        #region IMarkingTipsSettings Members

        public bool ShowMarkingTips { get; set; }
        //public string MarkingTipsUrl { get; set; }

        #endregion
    }
}
