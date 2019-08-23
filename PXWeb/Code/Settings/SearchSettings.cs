using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the search settings
    /// </summary>
    internal class SearchSettings : ISearchSettings 
    {
        #region "ISearchSettings members"
        
        public int CacheTime { get; set; }
        public int ResultListLength { get; set; }
        public PCAxis.Search.DefaultOperator DefaultOperator { get; set; }
        
        #endregion


        #region "Public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="menuNode">XML-node for the Search settings</param>
        public SearchSettings(XmlNode searchNode)
        {
            string xpath;
 
            xpath = "./cacheTime";
            CacheTime = SettingsHelper.GetSettingValue(xpath, searchNode, 60);

            xpath = "./resultListLength";
            ResultListLength = SettingsHelper.GetSettingValue(xpath, searchNode, 250);

            xpath = "./defaultOperator";
            DefaultOperator = SettingsHelper.GetSettingValue(xpath, searchNode, PCAxis.Search.DefaultOperator.OR);
        }

        /// <summary>
        /// Save the Search settings to the settings file
        /// </summary>
        /// <param name="menuNode">XML-node for the search settings</param>
        public void Save(XmlNode searchNode)
        {
            string xpath;

            xpath = "./cacheTime";
            SettingsHelper.SetSettingValue(xpath, searchNode, CacheTime.ToString());

            xpath = "./resultListLength";
            SettingsHelper.SetSettingValue(xpath, searchNode, ResultListLength.ToString());

            xpath = "./defaultOperator";
            SettingsHelper.SetSettingValue(xpath, searchNode, DefaultOperator.ToString());
        }


        #endregion
    }
}