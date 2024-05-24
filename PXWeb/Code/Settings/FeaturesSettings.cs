using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the Features settings
    /// </summary>
    internal class FeaturesSettings : IFeaturesSettings
    {
        #region "Private fields"

        /// <summary>
        /// Features.General settings
        /// </summary>
        private FeaturesGeneralSettings _featuresGeneralSettings;

        /// <summary>
        /// Features.Charts settings
        /// </summary>
        private ChartsSettings _chartsSettings;

        /// <summary>
        /// Features.Api settings
        /// </summary>
        private ApiSettings _apiSettings;

        /// <summary>
        /// Features.Search settings
        /// </summary>
        private SearchSettings _searchSettings;

        /// <summary>
        /// Features.BackgroundWorker settings
        /// </summary>
        private BackgroundWorkerSettings _backgroundWorkerSettings;

        /// <summary>
        /// Features.SavedQuery settings
        /// </summary>
        private SavedQuerySettings _savedQuerySettings;

        #endregion


        #region "Public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="generalNode">XML-node for the Features settings</param>
        public FeaturesSettings(XmlNode featuresNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./general";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _featuresGeneralSettings = new FeaturesGeneralSettings(node);

            xpath = "./charts";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _chartsSettings = new ChartsSettings(node);

            xpath = "./api";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _apiSettings = new ApiSettings(node);

            xpath = "./search";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _searchSettings = new SearchSettings(node);

            xpath = "./backgroundWorker";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _backgroundWorkerSettings = new BackgroundWorkerSettings(node);

            xpath = "./savedQuery";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _savedQuerySettings = new SavedQuerySettings(node);
        }

        /// <summary>
        /// Save Features settings to the settings file
        /// </summary>
        /// <param name="generalNode">XML-node for the Features settings</param>
        public void Save(XmlNode featuresNode)
        {
            string xpath;
            XmlNode node;

            // Save Features.General settings
            xpath = "./general";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _featuresGeneralSettings.Save(node);

            // Save Features.Charts settings
            xpath = "./charts";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _chartsSettings.Save(node);

            // Save Features.Api settings
            xpath = "./api";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _apiSettings.Save(node);

            // Save Features.Search settings
            xpath = "./search";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _searchSettings.Save(node);

            // Save Features.BackgroundWorker settings
            xpath = "./backgroundWorker";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _backgroundWorkerSettings.Save(node);
            // Save Features.BackgroundWorker settings
            xpath = "./backgroundWorker";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _backgroundWorkerSettings.Save(node);

            //Save Features.SavedQuery settings
            xpath = "./savedQuery";
            node = SettingsHelper.GetNode(featuresNode, xpath);
            _savedQuerySettings.Save(node);
        }

        #endregion

        #region IFeaturesSettings Members

        public IFeaturesGeneralSettings General
        {
            get { return _featuresGeneralSettings; }
        }

        public IChartSettings Charts
        {
            get { return _chartsSettings; }
        }

        public IApiSettings Api
        {
            get { return _apiSettings; }
        }

        public ISearchSettings Search
        {
            get { return _searchSettings; }
        }

        public IBackgroundWorkerSettings BackgroundWorker
        {
            get { return _backgroundWorkerSettings; }
        }

        public ISavedQuerySettings SavedQuery
        {
            get { return _savedQuerySettings; }
        }
        #endregion
    }
}
