using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PCAxis.Metadata
{
    /// <summary>
    /// Class that encapsulates a metadata.config file containing link-definitions to metadata systems
    /// </summary>
    public class MetaLinkManager : IMetaIdProvider
    {
        /// <summary>
        /// Class holding format information for a link
        /// </summary>
        private class MetaLinkFormat
        {
            /// <summary>
            /// Format of the link text
            /// </summary>
            public string LinkTextFormat { get; set; }

            /// <summary>
            /// Format of the link (URL)
            /// </summary>
            public string LinkFormat { get; set; }

            /// <summary>
            /// Hyperlink target
            /// Valid values:
            /// _blank    - Load in new window
            /// _self     - Load in the same frame as it was clicked
            /// _parent   - Load in the parent frameset
            /// _top      - Load in the full body of the window
            /// framename - Load in named frame
            /// </summary>
            public string Target { get; set; }

            /// <summary>
            /// Get number of expected parameters to the text link
            /// </summary>
            /// <returns></returns>
            public int NumberOfTextParameters()
            {
                return GetNumberOfParameters(LinkTextFormat);
            }

            /// <summary>
            /// Get number of expected parameters to the link
            /// </summary>
            /// <returns></returns>
            public int NumberOfLinkParameters()
            {
                return GetNumberOfParameters(LinkFormat);
            }

            /// <summary>
            /// Internal method for finding the number of expected parameters
            /// </summary>
            /// <param name="txt"></param>
            /// <returns></returns>
            private int GetNumberOfParameters(string txt)
            {
                int count = 0;
                int paramIndex = 0;
                int index = 0;
                bool ok = true;

                if (string.IsNullOrWhiteSpace(txt))
                {
                    return 0;
                }


                while (ok)
                {
                    index = txt.IndexOf("{" + paramIndex.ToString() + "}", index); // Find {0}, next time {1} and so on...
                    if (index != -1)
                    {
                        count += 1;
                        paramIndex += 1;
                    }
                    else
                    {
                        ok = false;
                    }
                }

                return count;
            }
        }





        #region "Private fields"

        /// <summary>
        /// Configuration file
        /// </summary>
        XmlDocument _xdoc;

        /// <summary>
        /// List of available metadata systems
        /// </summary>
        private List<MetadataSystem> _metadataSystems;

        /// <summary>
        /// Dictionary of metadata systems containing table information. 
        /// Key = Metadata system id, Value = dictionary of linkformats per language (key = language, value = linkformat-object). 
        /// </summary>
        private Dictionary<string, Dictionary<string, MetaLinkFormat>> _tableLinkFormats;

        /// <summary>
        /// Dictionary of metadata systems containing variable information. 
        /// Key = Metadata system id, Value = dictionary of linkformats per language (key = language, value = linkformat-object). 
        /// </summary>
        private Dictionary<string, Dictionary<string, MetaLinkFormat>> _variableLinkFormats;

        /// <summary>
        /// Dictionary of metadata systems containing value information. 
        /// Key = Metadata system id, Value = dictionary of linkformats per language (key = language, value = linkformat-object). 
        /// </summary>
        private Dictionary<string, Dictionary<string, MetaLinkFormat>> _valueLinkFormats;

        /// <summary>
        /// Logging to Log4Net
        /// </summary>
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(MetaLinkManager));

        /// <summary>
        /// Character that separates the systems within a META-ID 
        /// </summary>
        private char[] _systemSeparator = { ',' };

        /// <summary>
        /// Character that separates the parameters within a system META-ID
        /// </summary>
        private char[] _paramSeparator = { ':' };

        #endregion





        /// <summary>
        /// Constructor
        /// </summary>
        public MetaLinkManager()
        {
            _metadataSystems = new List<MetadataSystem>();
            _tableLinkFormats = new Dictionary<string, Dictionary<string, MetaLinkFormat>>();
            _variableLinkFormats = new Dictionary<string, Dictionary<string, MetaLinkFormat>>();
            _valueLinkFormats = new Dictionary<string, Dictionary<string, MetaLinkFormat>>();
        }

        /// <summary>
        /// Returns the param separator
        /// </summary>
        /// <returns></returns>
        public char[] GetParamSeparator()
        {
            return _paramSeparator;
        }

        /// <summary>
        /// Returns the system separator
        /// </summary>
        /// <returns></returns>
        public char[] GetSystemSeparator()
        {
            return _systemSeparator;
        }

        /// <summary>
        /// Load metadata.config file
        /// </summary>
        /// <param name="configurationFile">Path to the configuration file</param>
        /// <returns>True if the configuration file was successfully loaded, else false</returns>
        public bool LoadConfiguration(string configurationFile)
        {
            if (!System.IO.File.Exists(configurationFile))
            {
                _logger.ErrorFormat("Metadata configuration file '{0}' does not exist", configurationFile);
                return false;
            }

            _xdoc = new XmlDocument();
            _xdoc.Load(configurationFile);

            // Table-level
            LoadConfigurationSection("onTable", _tableLinkFormats);

            // Variable-level
            LoadConfigurationSection("onVariable", _variableLinkFormats);

            // Value-level
            LoadConfigurationSection("onValue", _valueLinkFormats);

            return true;
        }

        /// <summary>
        /// Load sub section of the configuration file
        /// </summary>
        /// <param name="section">Name of the section</param>
        /// <param name="dictionary">Dictionary to store section data in</param>
        /// <returns></returns>
        private bool LoadConfigurationSection(string section, Dictionary<string, Dictionary<string, MetaLinkFormat>> dictionary)
        {
            string xpath;
            XmlNode node;
            XmlNodeList xmlnodes;

            xpath = "/metaId/" + section;
            node = _xdoc.SelectSingleNode(xpath);

            // Find all system nodes
            xpath = ".//metaSystem";
            xmlnodes = node.SelectNodes(xpath);

            foreach (XmlNode sysNode in xmlnodes)
            {
                string sysId = sysNode.Attributes["id"].Value; // system id

                if (!string.IsNullOrWhiteSpace(sysId))
                {
                    AddMetadataSystem(sysId);

                    if (!dictionary.ContainsKey(sysId))
                    {
                        dictionary.Add(sysId, new Dictionary<string, MetaLinkFormat>()); // add system to dictionary

                        // Find all language nodes for the system
                        xpath = ".//link";
                        XmlNodeList langNodes = sysNode.SelectNodes(xpath);

                        foreach (XmlNode langNode in langNodes)
                        {
                            string language = langNode.Attributes["px-lang"].Value;
                            string textFormat = langNode.Attributes["labelStringFormat"].Value;
                            string linkFormat = langNode.Attributes["urlStringFormat"].Value;
                            string target;
                            if ((langNode.Attributes["target"] != null) && (!string.IsNullOrEmpty(langNode.Attributes["target"].Value)))
                            {
                                target = langNode.Attributes["target"].Value;
                            }
                            else
                            {
                                target = "_blank";
                            }

                            if (!string.IsNullOrWhiteSpace(language) && !string.IsNullOrWhiteSpace(textFormat) && !string.IsNullOrWhiteSpace(linkFormat))
                            {
                                if (!dictionary[sysId].ContainsKey(language))
                                {
                                    MetaLinkFormat format = new MetaLinkFormat();
                                    format.LinkTextFormat = textFormat;
                                    format.LinkFormat = linkFormat;
                                    format.Target = target;

                                    dictionary[sysId].Add(language, format); // Add format for this language to dictionary
                                }
                            }
                        }

                    }
                }

            }

            return true;
        }


        /// <summary>
        /// Add metadatasystem to list of available systems
        /// </summary>
        /// <param name="system">Id of the metadatasystem</param>
        private void AddMetadataSystem(string system)
        {
            foreach (MetadataSystem sys in _metadataSystems)
            {
                if (sys.ID.Equals(system))
                {
                    return;
                }
            }

            _metadataSystems.Add(new MetadataSystem(system, system));
        }


        /// <summary>
        /// Create links
        /// </summary>
        /// <param name="metaId">META-ID</param>
        /// <param name="language">Language</param>
        /// <param name="dictionary">Dictionary containing the link formats</param>
        /// <returns></returns>
        private MetaLink[] GetLinks(string metaId, string language, Dictionary<string, Dictionary<string, MetaLinkFormat>> dictionary)
        {
            List<MetaLink> lst = new List<MetaLink>();

            // Get link-information per system
            string[] systemLinks = metaId.Split(_systemSeparator, StringSplitOptions.RemoveEmptyEntries);

            foreach (MetadataSystem system in _metadataSystems)
            {
                foreach (string systemLink in systemLinks)
                {
                    // Split the system link into parameters
                    object[] systemLinkParams = systemLink.Split(_paramSeparator, StringSplitOptions.RemoveEmptyEntries);

                    if (systemLinkParams.Length > 0)
                    {
                        string sysId = (string)systemLinkParams[0];

                        // The first parameter shall be the id of the metadata system
                        if (system.ID.Equals(sysId))
                        {
                            // Verify that we have metadata format information for the specified system and language
                            if (dictionary.ContainsKey(sysId) && dictionary[sysId].ContainsKey(language))
                            {
                                // Remove first parameter value which is the metadata system id
                                systemLinkParams = systemLinkParams.Where((val, idx) => idx != 0).ToArray();

                                // Get format object from dictionary
                                MetaLinkFormat format = dictionary[sysId][language];

                                // How many parameters are required for the link text?
                                int parameters = format.NumberOfTextParameters();

                                if (systemLinkParams.Length >= parameters)
                                {
                                    MetaLink lnk = new MetaLink();
                                    lnk.System = system.ID;

                                    // Create link text
                                    lnk.LinkText = string.Format(format.LinkTextFormat, systemLinkParams);

                                    // How many parameters are required for the link?
                                    parameters = format.NumberOfLinkParameters();

                                    if (systemLinkParams.Length >= parameters)
                                    {
                                        // Create the link
                                        lnk.Link = string.Format(format.LinkFormat, systemLinkParams);
                                        lnk.Target = format.Target;

                                        // Add link to the return-list
                                        lst.Add(lnk);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return lst.ToArray();
        }



        #region "Implementation of IMetaIdProvider"

        public bool Initialize(string configurationFile)
        {
            return LoadConfiguration(configurationFile);
        }

        public MetadataSystem[] MetadataSystems
        {
            get
            {
                return _metadataSystems.ToArray();
            }
        }

        public MetaLink[] GetTableLinks(string metaId, string language)
        {
            return GetLinks(metaId, language, _tableLinkFormats);
        }

        public MetaLink[] GetVariableLinks(string metaId, string language)
        {
            return GetLinks(metaId, language, _variableLinkFormats);
        }

        public MetaLink[] GetValueLinks(string metaId, string language)
        {
            return GetLinks(metaId, language, _valueLinkFormats);
        }

        #endregion
    }
}
