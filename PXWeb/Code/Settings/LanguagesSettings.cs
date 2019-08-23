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
using System.Collections.Generic;
using System.Globalization;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the General.Languages settings
    /// </summary>
    internal class LanguagesSettings : ILanguagesSettings
    {
        #region "Private fields"
        private List<string> _allLanguages;
        private List<ILanguageSettings> _siteLanguages;
        
        /// <summary>
        /// Object to control loading of "All languages" in a multithreaded environment 
        /// </summary>
        private static object _allLanguagesSettingLock = new Object();
        #endregion
        
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="languagesNode">XML-node for the General.Languages settings</param>
        public LanguagesSettings(XmlNode languagesNode)
        {
            if (languagesNode != null)
            {
                string xpath;
                XmlNodeList nodeList;
                LanguageSettings lang;

                _siteLanguages = new List<ILanguageSettings>();
                xpath = ".//language";
                nodeList = languagesNode.SelectNodes(xpath);
                foreach (XmlNode node in nodeList)
                {
                    lang = new LanguageSettings(node);

                    if (lang.Name.Length > 0)
                    {
                        if (lang.DefaultLanguage)
                        {
                            DefaultLanguage = lang.Name;
                        }

                        _siteLanguages.Add(lang);
                    }

                    //_siteLanguages.Add(node.InnerText);

                    //// Check if it is the default language
                    //if (node.Attributes["default"] != null)
                    //{
                    //    if (string.Compare(node.Attributes["default"].Value.ToLower(), "true") == 0)
                    //    {
                    //        DefaultLanguage = node.InnerText;
                    //    }
                    //}
                }
            }
        }

        /// <summary>
        /// Save the General.Languages settings to the settings file
        /// </summary>
        /// <param name="siteNode">XML-node for the General.Languages settings</param>
        public void Save(XmlNode languagesNode)
        {
            if (languagesNode != null)
            {
                XmlNode languageNode;

                //Remove all existing languages in file
                languagesNode.RemoveAll();

                //Add new languages
                foreach (LanguageSettings lang in SiteLanguages)
                {
                    //Create language-node
                    languageNode = languagesNode.OwnerDocument.CreateNode(XmlNodeType.Element, "language", "");
                    lang.Save(languageNode);

                    languagesNode.AppendChild(languageNode);
                }
            }
            
        }


        /// <summary>
        /// Checks if a language is a site language
        /// </summary>
        /// <param name="lang">Language to check</param>
        /// <returns>True if the language is a site language, else false</returns>
        public bool IsSiteLanguage(string lang)
        {
            foreach (LanguageSettings li in Settings.Current.General.Language.SiteLanguages)
            {
                if (lang.Equals(li.Name))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Load all available languages 
        /// </summary>
        private void LoadAllLanguages()
        {
            string path;
            string fileNameWithoutExtension;
            string[] splitFileName;
            CultureInfo cultureInfo;

            _allLanguages = new List<string>();

            //if (System.IO.Path.IsPathRooted(PXWeb.Settings.Current.General.Paths.LanguagesPath))
            //{
            //    path = PXWeb.Settings.Current.General.Paths.LanguagesPath;
            //}
            //else
            //{
                path = System.Web.HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.LanguagesPath);
            //}

            foreach (string file in System.IO.Directory.GetFiles(path, "*.xml", System.IO.SearchOption.TopDirectoryOnly))
            {
                fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file);
                splitFileName = fileNameWithoutExtension.Split('.');
                cultureInfo = null;

                if (splitFileName.Length > 1)
                {
                    try
                    {
                        cultureInfo = CultureInfo.GetCultureInfo(splitFileName[1]);
                    }
                    catch
                    {
                        //Do nothing
                    }
                }
                else
                {
                    cultureInfo = CultureInfo.GetCultureInfo("en");
                }
                if (cultureInfo != null)
                {
                    _allLanguages.Add(cultureInfo.Name);
                }
            }
        }

        #endregion

        #region ILanguagesSettings Members

        public System.Collections.Generic.IEnumerable<string> AllLanguages
        {
            get 
            {
                if (_allLanguages == null)
                {
                    // Assure that only one thread at a time can load "All languages"
                    lock (_allLanguagesSettingLock)
                    {
                        if (_allLanguages == null)
                        {
                            LoadAllLanguages();
                        }
                    }
                }

                return _allLanguages; 
            }
        }

        public string DefaultLanguage { get; set; }

        public System.Collections.Generic.IEnumerable<ILanguageSettings> SiteLanguages
        {
            get { return _siteLanguages; }
        }

        public void ResetLanguages()
        {
            _allLanguages = null;
        }

        #endregion
    }
}
