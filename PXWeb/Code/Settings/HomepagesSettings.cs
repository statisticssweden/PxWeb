using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing databasespecific homepages settings
    /// </summary>
    internal class HomepagesSettings : IHomepagesSettings
    {
        #region "Private fields"

        /// <summary>
        /// Dictionary containing homepage per language
        /// </summary>
        private Dictionary<string, IHomepageSettings> _homepage;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="homepagesNode">XML node with homepages settings</param>
        public HomepagesSettings(XmlNode homepagesNode)
        {
            string xpath;
            //XmlNode node, settingsNode;
            _homepage = new Dictionary<string, IHomepageSettings>();

            if (homepagesNode != null)
            {
                XmlNodeList nodeList;
                HomepageSettings homePage;

                xpath = ".//homePage";
                nodeList = homepagesNode.SelectNodes(xpath);
                foreach (XmlNode n in nodeList)
                {
                    homePage = new HomepageSettings(n);
                    if (!_homepage.ContainsKey(homePage.Language))
                    {
                        _homepage.Add(homePage.Language, homePage);
                    }
                }
            }
        }

        /// <summary>
        /// Save the Database.Homepages settings to the settings file
        /// </summary>
        /// <param name="homepagesNode">XML-node for the Database.Homepages settings</param>
        public void Save(XmlNode homepagesNode)
        {
            if (homepagesNode != null)
            {
                XmlNode homepageNode;

                //Remove all existing languages in file
                homepagesNode.RemoveAll();

                //Add new homepages
                foreach (KeyValuePair<string, IHomepageSettings> p in _homepage)
                {
                    //Create homepage-node
                    homepageNode = homepagesNode.OwnerDocument.CreateNode(XmlNodeType.Element, "homePage", "");
                    ((HomepageSettings)p.Value).Save(homepageNode);
                    homepagesNode.AppendChild(homepageNode);
                }
            }

        }
        
        /// <summary>
        /// Function to get the homepage for the specified language
        /// </summary>
        /// <param name="language">Language code</param>
        /// <returns>IHomepageSettings object</returns>
        public IHomepageSettings GetHomepage(string language)
        {
            HomepageSettings homepage;

            if (!string.IsNullOrEmpty(language))
            {
                if (_homepage.ContainsKey(language))
                {
                    return _homepage[language];
                }
            }

            homepage = new HomepageSettings();
            homepage.Language = string.IsNullOrEmpty(language) ? "" : language;
            return homepage;
        }

    }
}