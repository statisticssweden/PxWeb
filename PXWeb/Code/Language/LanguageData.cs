using System;
using System.Xml;

namespace PXWeb.Language
{
    /// <summary>
    /// Represents and wraps a PX-Web language file
    /// </summary>
    public class LanguageData
    {
        private XmlDocument _xdoc;
        private string _path;
        private XmlNamespaceManager _xmlnsManager;
        private XmlNode _langNode;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to the xml-file containing the sentences for the language</param>
        /// <param name="language">The specific culture, for example sv</param>
        /// <remarks>
        /// Loads the file specified by the path parameter to the xmldocument if the file exists,
        /// otherwise the xmldocument is populated with only the language element
        /// </remarks>
        public LanguageData(string path, string language)
        {
            _path = path;
            _xdoc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                _xdoc.Load(path);
            }
            else
            {
                _xdoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><language xmlns=\"http://www.pc-axis.scb.se/\" name=\"" + language + "\"></language>");
            }

            _xmlnsManager = new XmlNamespaceManager(_xdoc.NameTable);
            _xmlnsManager.AddNamespace("px", "http://www.pc-axis.scb.se/");
        }

        /// <summary>
        /// Get the specified sentence from the current language
        /// </summary>
        /// <param name="key">Key of the localized string</param>
        /// <returns>The sentence in the current language</returns>
        public string GetSentence(string key)
        {
            string xpath = string.Format("//px:sentence [@name='{0}']", key);
            XmlNode n = _xdoc.SelectSingleNode(xpath, _xmlnsManager);

            if (n != null)
            {
                return n.Attributes["value"].Value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Clear all sentences
        /// </summary>
        public void ClearSentences()
        {
            XmlNode xe = GetLanguageNode();

            if (_langNode != null)
            {
                while (xe.ChildNodes.Count > 0)
                {
                    xe.RemoveChild(xe.FirstChild);
                }
            }
        }

        /// <summary>
        /// Insert a sentence
        /// </summary>
        /// <param name="name">Name (key) of the sentence</param>
        /// <param name="value">The sentence in text</param>
        public void InsertSentence(string name, string value)
        {
            XmlNode n = _xdoc.CreateNode(XmlNodeType.Element, "sentence", "http://www.pc-axis.scb.se/");
            XmlAttribute att = _xdoc.CreateAttribute("name");
            att.Value = name;
            n.Attributes.Append(att);
            att = _xdoc.CreateAttribute("value");
            att.Value = value;
            n.Attributes.Append(att);
            GetLanguageNode().AppendChild(n);
        }

        /// <summary>
        /// Save the language to file
        /// </summary>
        public void Save()
        {
            _xdoc.Save(_path);
        }

        /// <summary>
        /// Get the language node
        /// </summary>
        /// <returns></returns>
        private XmlNode GetLanguageNode()
        {
            if (_langNode == null)
            {
                string xpath = String.Format("//px:language");
                _langNode = _xdoc.SelectSingleNode(xpath, _xmlnsManager);
            }

            return _langNode;
        }
    }
}
