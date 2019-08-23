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
    /// Internal class for reading and writing the General settings
    /// </summary>
    internal class GeneralSettings : IGeneralSettings
    {
        #region "Private fields"

        /// <summary>
        /// General.Site settings
        /// </summary>
        private SiteSettings _siteSettings;

        /// <summary>
        /// General.Languages settings
        /// </summary>
        private LanguagesSettings _languagesSettings;

        /// <summary>
        /// General.Paths settings
        /// </summary>
        private PathsSettings _pathsSettings;

        /// <summary>
        /// General.FileFormats settings
        /// </summary>
        private FileFormatsSettings _fileFormatsSettings;

        /// <summary>
        /// General.Modules settings
        /// </summary>
        private ModulesSettings _modulesSettings;

        /// <summary>
        /// General.Global settings
        /// </summary>
        private GlobalSettings _globalSettings;

        /// <summary>
        /// General.Databases settings
        /// </summary>
        private DatabasesSettings _databasesSettings;

        /// <summary>
        /// General.Administration settings
        /// </summary>
        private AdministrationSettings _administrationSettings;

        #endregion

        #region "Public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="generalNode">XML-node for the General settings</param>
        public GeneralSettings(XmlNode generalNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./site";
            node = generalNode.SelectSingleNode(xpath);
            _siteSettings = new SiteSettings(node);

            xpath = "./paths";
            node = generalNode.SelectSingleNode(xpath);
            _pathsSettings = new PathsSettings(node);

            xpath = "./languages";
            node = generalNode.SelectSingleNode(xpath);
            _languagesSettings = new LanguagesSettings(node);

            xpath = "./fileFormats";
            node = generalNode.SelectSingleNode(xpath);
            _fileFormatsSettings = new FileFormatsSettings(node);

            xpath = "./modules";
            node = generalNode.SelectSingleNode(xpath);
            _modulesSettings = new ModulesSettings(node);

            xpath = "./global";
            node = generalNode.SelectSingleNode(xpath);
            _globalSettings = new GlobalSettings(node);

            xpath = "./databases";
            node = generalNode.SelectSingleNode(xpath);
            _databasesSettings = new DatabasesSettings(node);

            xpath = "./administration";
            node = generalNode.SelectSingleNode(xpath);
            _administrationSettings = new AdministrationSettings(node);
        }

        /// <summary>
        /// Save General settings to the settings file
        /// </summary>
        /// <param name="generalNode">XML-node for the general settings</param>
        public void Save(XmlNode generalNode)
        {
            string xpath;
            XmlNode node;

            // Save General.Site settings
            xpath = "./site";
            node = generalNode.SelectSingleNode(xpath);
            _siteSettings.Save(node);

            // Save General.Paths settings
            xpath = "./paths";
            node = generalNode.SelectSingleNode(xpath);
            _pathsSettings.Save(node);

            // Save General.Languages settings
            xpath = "./languages";
            node = generalNode.SelectSingleNode(xpath);
            _languagesSettings.Save(node);

            // Save General.FileFormats settings
            xpath = "./fileFormats";
            node = generalNode.SelectSingleNode(xpath);
            _fileFormatsSettings.Save(node);

            // Save General.Modules settings
            xpath = "./modules";
            node = generalNode.SelectSingleNode(xpath);
            _modulesSettings.Save(node);

            // Save General.Global settings
            xpath = "./global";
            node = generalNode.SelectSingleNode(xpath);
            _globalSettings.Save(node);

            // Save General.Databases settings
            xpath = "./databases";
            node = generalNode.SelectSingleNode(xpath);
            _databasesSettings.Save(node);

            // Save General.Administration settings
            xpath = "./administration";
            node = generalNode.SelectSingleNode(xpath);
            _administrationSettings.Save(node);
        }

        #endregion


        #region IGeneralSettings Members

        public ISiteSettings Site { get { return _siteSettings; } }

        public IPathsSettings Paths
        {
            get { return _pathsSettings; }
        }

        public ILanguagesSettings Language
        {
            get { return _languagesSettings; }
        }

        public IFileFormatsSettings FileFormats
        {
            get { return _fileFormatsSettings; }
        }

        public IModulesSettings Modules
        {
            get { return _modulesSettings; }
        }

        public IGlobalSettings Global
        {
            get { return _globalSettings; }
        }

        public IDatabasesSettings Databases
        {
            get { return _databasesSettings; }
        }

        public IAdministrationSettings Administration
        {
            get { return _administrationSettings; }
        }

        #endregion
    }
}
