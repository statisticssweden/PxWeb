using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using PCAxis.Search;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the search index settings
    /// </summary>
    internal class SearchIndexSettings : ISearchIndexSettings
    {
        private ISearchIndex _updater;
        private string _updateMethod;
        
        /// <summary>
        /// Log object
        /// </summary>
        private static log4net.ILog _logger;
        
        #region "ISearchIndexSettings members"

        public SearchIndexStatusType Status { get; set; }
        public string IndexUpdated { get; set; }
        public ISearchIndex UpdateMethod
        {
            get { return _updater; }
        }

        #endregion


        #region "Public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="menuNode">XML-node for the Search index settings</param>
        public SearchIndexSettings(XmlNode indexNode)
        {
            string xpath;
            _logger = log4net.LogManager.GetLogger(typeof(SearchIndexSettings));
 
            xpath = "./status";
            Status = SettingsHelper.GetSettingValue(xpath, indexNode, SearchIndexStatusType.NotIndexed);

            xpath = "./indexUpdated";
            IndexUpdated = SettingsHelper.GetSettingValue(xpath, indexNode, "");

            xpath = "./updateMethod";
            SetUpdateMethod(SettingsHelper.GetSettingValue(xpath, indexNode, ""));
        }

        /// <summary>
        /// Save the Search index settings to the settings file
        /// </summary>
        /// <param name="menuNode">XML-node for the search index settings</param>
        public void Save(XmlNode indexNode)
        {
            string xpath;

            xpath = "./status";
            SettingsHelper.SetSettingValue(xpath, indexNode, Status.ToString());

            xpath = "./indexUpdated";
            SettingsHelper.SetSettingValue(xpath, indexNode, IndexUpdated.ToString());

            xpath = "./updateMethod";
            SettingsHelper.SetSettingValue(xpath, indexNode, GetUpdateMethod());
        }

        public void SetUpdateMethod(string method)
        {
            _updateMethod = method;

            if (string.IsNullOrEmpty(_updateMethod))
            {
                _updater = new DefaultSearchIndex();
            }
            else
            {
                try
                {
                    var typeString = _updateMethod;
                    var parts = typeString.Split(',');
                    var typeName = parts[0].Trim();
                    var assemblyName = parts[1].Trim();
                    _updater = (ISearchIndex)Activator.CreateInstance(assemblyName, typeName).Unwrap();
                }
                catch (Exception)
                {
                    _updater = new DefaultSearchIndex();
                    _logger.Error("Could not create Search index updater '" + _updateMethod + "' --> Default search index updater created instead");
                }
            }
        }

        public string GetUpdateMethod()
        {
            return _updateMethod;
        }
        
        #endregion



    }
}