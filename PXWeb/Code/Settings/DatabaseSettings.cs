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
using System.IO;
using System.Collections.Generic;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing settings for a specific database
    /// </summary>
    internal class DatabaseSettings : IDatabaseSettings
    {
        #region "Private fields"

        /// <summary>
        /// Path to the settings file
        /// </summary>
        private string _path;

        /// <summary>
        /// Log object
        /// </summary>
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(DatabaseSettings));

        /// <summary>
        /// Home pages settings
        /// </summary>
        private HomepagesSettings _homepagesSettings;

        /// <summary>
        /// Search index settings
        /// </summary>
        private SearchIndexSettings _searchIndexSettings;

        /// <summary>
        /// The protection settings
        /// </summary>
        private ProtectionSettings _protection;

        /// <summary>
        /// Metadata settings
        /// </summary>
        private MetadataSettings _metadata;

        #endregion


        #region "public methods"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to the database settings file</param>
        public DatabaseSettings(string path)
        {

            if (string.IsNullOrEmpty(path))
            {
                _logger.Error("Database settings-file not specified");
                throw new System.Exception();
            }
            //if (!File.Exists(path))
            //{
            //    _logger.Error("Database settings-file '" + path + "' does not exist");
            //    throw new System.Exception();
            //}

            _path = path;

            if (!LoadSettings())
            {
                _logger.Error("What to do, what to do? :-)");
            }
            PerformInitiations();
        }

        /// <summary>
        /// Read the settings file and load the settings
        /// </summary>
        /// <returns>True if the settings was loaded successfully, else false</returns>
        private bool LoadSettings()
        {
            string xpath;
            XmlNode node, settingsNode;

            //try
            //{
                //Load settings-file
                XmlDocument xdoc = new XmlDocument();
                if (File.Exists(_path))
                {
                    //try
                    //{
                        xdoc.Load(_path);
                    //} 
                    //catch (Exception e) 
                    //{
                    //    _logger.Error("Error when trying to load settings.", e);
                    //    _logger.ErrorFormat("Could not use file {0}",_path);
                    //    _logger.Warn("Continuing using empty settings.");
                    //    xdoc = new XmlDocument();
                    //    xdoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><settings></settings>");
                    //}


                }
                else
                {
                    xdoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><settings></settings>");
                }

                xpath = "/settings";
                settingsNode = xdoc.SelectSingleNode(xpath);

                xpath = "/settings/homePages";
                node = SettingsHelper.GetNode(settingsNode, xpath);
                _homepagesSettings = new HomepagesSettings(node);

                xpath = "/settings/searchIndex";
                node = SettingsHelper.GetNode(settingsNode, xpath);
                _searchIndexSettings = new SearchIndexSettings(node);

                xpath = "/settings/protection";
                node = SettingsHelper.GetNode(settingsNode, xpath);
                _protection = new ProtectionSettings(node);

                xpath = "/settings/metadata";
                node = SettingsHelper.GetNode(settingsNode, xpath);
                _metadata = new MetadataSettings(node);

            //}
            //catch (System.Exception ex)
            //{
            //    _logger.Error("::LoadSettings() : Error when loading settings : " + ex.Message);
            //    return false;
            //}

//            _logger.Info("Database settings-file '" + _path + "' was loaded successfully");
            return true;
        }

        /// <summary>
        /// Save the database settings to the settings file
        /// </summary>
        public void Save()
        {
            string xpath;
            XmlNode node, settingsNode;

            XmlDocument xdoc = new XmlDocument();
            if (File.Exists(_path))
            {
                xdoc.Load(_path);
            }
            else
            {
                xdoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><settings></settings>");

                // Create database directory if it does not exist
                FileInfo f = new FileInfo(_path);
                if (!System.IO.Directory.Exists(f.DirectoryName))
                {
                    System.IO.Directory.CreateDirectory(f.DirectoryName);
                }
            }

            xpath = "/settings";
            settingsNode = xdoc.SelectSingleNode(xpath);

            xpath = "/settings/homePages";
            node = SettingsHelper.GetNode(settingsNode, xpath);
            _homepagesSettings.Save(node);

            xpath = "/settings/searchIndex";
            node = SettingsHelper.GetNode(settingsNode, xpath);
            _searchIndexSettings.Save(node);

            xpath = "/settings/protection";
            node = SettingsHelper.GetNode(settingsNode, xpath);
            _protection.Save(node);

            xpath = "/settings/metadata";
            node = SettingsHelper.GetNode(settingsNode, xpath);
            _metadata.Save(node);

            _logger.InfoFormat("Saving settings to file: {0}.", _path);
            xdoc.Save(_path);
            _logger.InfoFormat("Saving done.");
        }

        #endregion

        /// <summary>
        /// Performs necessary initiations
        /// </summary>
        private void PerformInitiations()
        {
            // Initiate metadata 
            if (_metadata.UseMetadata)
            {
                string metaConfig;

                if (!string.IsNullOrWhiteSpace(_metadata.MetaSystemConfigFile))
                {
                    // If metadata config file is explicitly declared - use that one
                    metaConfig = _metadata.MetaSystemConfigFile;
                }
                else
                {
                    // If no metadata config file is declared we assume that there are a metdata.config file in the database directory
                    string path = System.IO.Path.GetDirectoryName(_path);
                    metaConfig = System.IO.Path.Combine(path, "metadata.config");
                }

                if (System.IO.File.Exists(metaConfig))
                {
                    if (!_metadata.MetaLinkMethod.Initialize(metaConfig))
                    {
                        // Not possible to initiate metadata - Turn off metadata for this database
                        _metadata.UseMetadata = false;
                    }
                }
                else
                {
                    // Not possible to initiate metadata - Turn off metadata for this database
                    _metadata.UseMetadata = false;
                }
            }
        }

        #region IDatabaseSettings Members

        public IHomepagesSettings Homepages
        {
            get { return _homepagesSettings; }
        }

        public ISearchIndexSettings SearchIndex
        {
            get { return _searchIndexSettings; }
        }

        public IProtection Protection
        {
            get { return _protection; }
        }

        public IMetadataSettings Metadata
        {
            get { return _metadata; }
        }

        #endregion




  
    }
}
