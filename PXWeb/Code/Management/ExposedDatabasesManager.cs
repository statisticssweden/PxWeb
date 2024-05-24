using PCAxis.Query;
using System;
using System.Collections.Generic;
using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Class for handling the configuration file for API databases 
    /// </summary>
    public class ExposedDatabasesManager
    {
        #region "Private fields"
        private string _path;
        private XmlDocument _xdoc;
        private XmlNode _databasesNode;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to exposed databases configuration file</param>
        public ExposedDatabasesManager(string path)
        {
            _path = path;
            _xdoc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                _xdoc.Load(path);
            }
            else
            {
                _xdoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Databases></Databases>");
            }

        }

        public void Save(Dictionary<string, List<DbConfig>> dictDb)
        {
            ClearDatabases();

            foreach (KeyValuePair<string, List<DbConfig>> entry in dictDb)
            {
                InsertLanguageNode(entry);
            }

            _xdoc.Save(_path);
        }

        private void InsertLanguageNode(KeyValuePair<string, List<DbConfig>> entry)
        {
            XmlNode langNode = _xdoc.CreateNode(XmlNodeType.Element, "DatabaseSet", "");
            XmlAttribute att = _xdoc.CreateAttribute("language");
            att.Value = entry.Key;
            langNode.Attributes.Append(att);
            GetDatabasesNode().AppendChild(langNode);

            foreach (DbConfig db in entry.Value)
            {
                InsertDatabaseNode(langNode, db);
            }
        }

        private void InsertDatabaseNode(XmlNode langNode, DbConfig db)
        {
            XmlNode dbNode = _xdoc.CreateNode(XmlNodeType.Element, "Database", "");
            XmlAttribute att = _xdoc.CreateAttribute("id");
            att.Value = db.Name;
            dbNode.Attributes.Append(att);
            att = _xdoc.CreateAttribute("type");
            att.Value = db.Type;
            dbNode.Attributes.Append(att);
            if (db.Type == "PX")
            {
                att = _xdoc.CreateAttribute("rootPath");
                att.Value = db.RootPath;
                dbNode.Attributes.Append(att);
            }
            XmlNode nameNode = _xdoc.CreateNode(XmlNodeType.Element, "Name", "");
            nameNode.InnerText = db.Name;
            dbNode.AppendChild(nameNode);
            langNode.AppendChild(dbNode);
        }

        /// <summary>
        /// Clear all databases
        /// </summary>
        private void ClearDatabases()
        {
            XmlNode xe = GetDatabasesNode();

            if (_databasesNode != null)
            {
                while (xe.ChildNodes.Count > 0)
                {
                    xe.RemoveChild(xe.FirstChild);
                }
            }
        }

        /// <summary>
        /// Get the Databases node
        /// </summary>
        /// <returns></returns>
        private XmlNode GetDatabasesNode()
        {
            if (_databasesNode == null)
            {
                string xpath = String.Format("//Databases");
                _databasesNode = _xdoc.SelectSingleNode(xpath);
            }

            return _databasesNode;
        }

    }
}