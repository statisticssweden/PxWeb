using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using PCAxis.Web.Controls.CommandBar.Plugin;
using PCAxis.Chart;
using PCAxis.Query;

namespace PXWeb
{
    public static class SettingsHelper
    {
        #region "Get node"
        public static XmlNode GetNode(XmlDocument xdoc, string xpath)
        {
            XmlNode node;
            string nodeName = "";

            node = xdoc.SelectSingleNode(xpath);

            if (node == null)
            {
                nodeName = xpath.Substring(xpath.LastIndexOf("/") + 1);
                node = xdoc.CreateNode("element", nodeName, "");
                xdoc.DocumentElement.AppendChild(node);
            }

            return node;
        }
        public static XmlNode GetNode(XmlNode n, string xpath)
        {
            XmlNode node;
            string nodeName = "";

            node = n.SelectSingleNode(xpath);

            if (node == null)
            {
                nodeName = xpath.Substring(xpath.LastIndexOf("/") + 1);
                node = n.OwnerDocument.CreateNode("element", nodeName, "");
                n.AppendChild(node);
            }

            return node;
        }
        #endregion

        #region "Get attribute values"
        /// <summary>
        /// Selects single node from passed XmlNode and returns attribute value, if the node or attribute is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node with attribute value</param>
        /// <param name="attributeName">Attribute to get value for</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node or attribute is not found</param>
        /// <returns>Attribute value as string</returns>
        public static string GetSettingAttributeValue(string xpath, string attributeName, XmlNode selectionNode, string defaultValue)
        {
            string returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                if (node.Attributes[attributeName] != null)
                {
                    returnValue = node.Attributes[attributeName].Value;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns attribute value, if the node or attribute is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node with attribute value</param>
        /// <param name="attributeName">Attribute to get value for</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node or attribute is not found</param>
        /// <returns>Attribute value as int</returns>
        public static int GetSettingAttributeValue(string xpath, string attributeName, XmlNode selectionNode, int defaultValue)
        {
            int returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                if (node.Attributes[attributeName] != null)
                {
                    if (!(int.TryParse(node.Attributes[attributeName].Value, out returnValue)))
                    {
                        returnValue = defaultValue;
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns attribute value, if the node or attribute is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node with attribute value</param>
        /// <param name="attributeName">Attribute to get value for</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node or attribute is not found</param>
        /// <returns>Attribute value as bool</returns>
        public static bool GetSettingAttributeValue(string xpath, string attributeName, XmlNode selectionNode, bool defaultValue)
        {
            bool returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                if (node.Attributes[attributeName] != null)
                {
                    if (!(bool.TryParse(node.Attributes[attributeName].Value, out returnValue)))
                    {
                        returnValue = defaultValue;
                    }
                }
            }
            return returnValue;
        }


        #endregion

        #region "Get setting values"

        /// <summary>
        /// Selects single node from passed XmlNode and returns its inner text, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply expression on</param>
        /// <param name="defaultValue">Value to return if node is not found</param>
        /// <returns>Setting value as string</returns>
        public static string GetSettingValue(string xpath, XmlNode selectionNode, string defaultValue)
        {
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                return node.InnerText;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns its inner text, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as an integer</param>
        /// <returns>Setting value as int</returns>
        public static int GetSettingValue(string xpath, XmlNode selectionNode, int defaultValue)
        {
            int returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                if (!(int.TryParse(node.InnerText, out returnValue)))
                {
                    returnValue = defaultValue;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns its inner text, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a boolean</param>
        /// <returns>Setting value as bool</returns>
        public static bool GetSettingValue(string xpath, XmlNode selectionNode, bool defaultValue)
        {
            bool returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                if (!(bool.TryParse(node.InnerText, out returnValue)))
                {
                    returnValue = defaultValue;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects NodeList from passed XmlNode and returns a list with the listed nodes inner text.
        /// </summary>
        /// <param name="xpath">Expression to select nodelist from given node</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <returns>Setting values as List of string</returns>
        public static List<string> GetSettingValue(string xpath, XmlNode selectionNode)
        {
            XmlNodeList nodeList;
            List<string> returnValue = new List<string>();
            if (selectionNode != null)
            {
                nodeList = selectionNode.SelectNodes(xpath);
                foreach (XmlNode n in nodeList)
                {
                    returnValue.Add(n.InnerText);
                }
            }
            return returnValue;
        }


        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding InformationLevelType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a InformationLevelType</param>
        /// <returns>Setting value as InformationLevelType</returns>
        public static PCAxis.Paxiom.InformationLevelType GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Paxiom.InformationLevelType defaultValue)
        {
            PCAxis.Paxiom.InformationLevelType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "MandantoryFootnotesOnly":
                        returnValue = PCAxis.Paxiom.InformationLevelType.MandantoryFootnotesOnly;
                        break;
                    case "AllFootnotes":
                        returnValue = PCAxis.Paxiom.InformationLevelType.AllFootnotes;
                        break;
                    case "AllInformation":
                        returnValue = PCAxis.Paxiom.InformationLevelType.AllInformation;
                        break;
                    default:
                        returnValue = PCAxis.Paxiom.InformationLevelType.None;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding DefaultOperator, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a PCAxis.Search.DefaultOperator</param>
        /// <returns>Setting value as PCAxis.Search.DefaultOperator</returns>
        public static PCAxis.Search.DefaultOperator GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Search.DefaultOperator defaultValue)
        {
            PCAxis.Search.DefaultOperator returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "AND":
                        returnValue = PCAxis.Search.DefaultOperator.AND;
                        break;
                    case "OR":
                        returnValue = PCAxis.Search.DefaultOperator.OR;
                        break;
                    default:
                        returnValue = PCAxis.Search.DefaultOperator.OR;
                        break;
                }
            }
            return returnValue;
        }


        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding SecrecyOptionType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a SecrecyOptionType</param>
        /// <returns>Setting value as SecrecyOptionType</returns>
        public static PCAxis.Paxiom.SecrecyOptionType GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Paxiom.SecrecyOptionType defaultValue)
        {
            PCAxis.Paxiom.SecrecyOptionType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "Simple":
                        returnValue = PCAxis.Paxiom.SecrecyOptionType.Simple;
                        break;
                    default:
                        returnValue = PCAxis.Paxiom.SecrecyOptionType.None;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding RoundingType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a RoundingType</param>
        /// <returns>Setting value as RoundingType</returns>
        public static PCAxis.Paxiom.RoundingType GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Paxiom.RoundingType defaultValue)
        {
            PCAxis.Paxiom.RoundingType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "BankersRounding":
                        returnValue = PCAxis.Paxiom.RoundingType.BankersRounding;
                        break;
                    case "RoundUp":
                        returnValue = PCAxis.Paxiom.RoundingType.RoundUp;
                        break;
                    default:
                        returnValue = PCAxis.Paxiom.RoundingType.None;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding DataNotePlacementType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a DataNotePlacementType</param>
        /// <returns>Setting value as DataNotePlacementType</returns>
        public static PCAxis.Paxiom.DataNotePlacementType GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Paxiom.DataNotePlacementType defaultValue)
        {
            PCAxis.Paxiom.DataNotePlacementType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "After":
                        returnValue = PCAxis.Paxiom.DataNotePlacementType.After;
                        break;
                    case "Before":
                        returnValue = PCAxis.Paxiom.DataNotePlacementType.Before;
                        break;
                    default:
                        returnValue = PCAxis.Paxiom.DataNotePlacementType.None;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding TableTransformationType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a TableTransformationType</param>
        public static PCAxis.Web.Controls.TableTransformationType GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Web.Controls.TableTransformationType defaultValue)
        {
            PCAxis.Web.Controls.TableTransformationType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "Sort":
                        returnValue = PCAxis.Web.Controls.TableTransformationType.Sort;
                        break;
                    case "SingleValueFirst":
                        returnValue = PCAxis.Web.Controls.TableTransformationType.SingleValueFirst;
                        break;
                    case "SingleValueFirstAndHeaderOnlyOneMultiple":
                        returnValue = PCAxis.Web.Controls.TableTransformationType.SingleValueFirstAndHeaderOnlyOneMultiple;
                        break;
                    default:
                        returnValue = PCAxis.Web.Controls.TableTransformationType.NoTransformation;
                        break;
                }
            }
            return returnValue;
        }


        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding TableLayoutType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a TableLayoutType</param>
        public static PCAxis.Web.Controls.TableLayoutType GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Web.Controls.TableLayoutType defaultValue)
        {
            PCAxis.Web.Controls.TableLayoutType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "Layout2":
                        returnValue = PCAxis.Web.Controls.TableLayoutType.Layout2;
                        break;
                    default:
                        returnValue = PCAxis.Web.Controls.TableLayoutType.Layout1;
                        break;
                }
            }
            return returnValue;
        }


        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding CommandBarViewMode, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a CommandBarViewMode</param>
        public static PCAxis.Web.Controls.CommandBar.CommandBarViewMode GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Web.Controls.CommandBar.CommandBarViewMode defaultValue)
        {
            PCAxis.Web.Controls.CommandBar.CommandBarViewMode returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "ImageButtons":
                        returnValue = PCAxis.Web.Controls.CommandBar.CommandBarViewMode.ImageButtons;
                        break;
                    case "Hidden":
                        returnValue = PCAxis.Web.Controls.CommandBar.CommandBarViewMode.Hidden;
                        break;
                    default:
                        returnValue = PCAxis.Web.Controls.CommandBar.CommandBarViewMode.DropDown;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding VariableSelectorSearchButtonViewMode, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a VariableSelectorSearchButtonViewMode</param>
        public static PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode defaultValue)
        {
            PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "ManyValues":
                        returnValue = PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode.ManyValues;
                        break;
                    case "Always":
                        returnValue = PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode.Always;
                        break;
                    default:
                        returnValue = PCAxis.Web.Controls.VariableSelectorSearchButtonViewMode.ManyValues;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding MenuModeType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a MenuModeType</param>
        public static MenuModeType GetSettingValue(string xpath, XmlNode selectionNode, MenuModeType defaultValue)
        {
            MenuModeType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "List":
                        returnValue = MenuModeType.List;
                        break;
                    case "TreeViewWithoutFiles":
                        returnValue = MenuModeType.TreeViewWithoutFiles;
                        break;
                    case "TreeViewWithFiles":
                        returnValue = MenuModeType.TreeViewWithFiles;
                        break;
                    case "TreeViewAndFiles":
                        returnValue = MenuModeType.TreeViewAndFiles;
                        break;
                    default:
                        returnValue = MenuModeType.TreeViewWithoutFiles;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding MenuViewLinkModeType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a MenuViewLinkModeType</param>
        public static MenuViewLinkModeType GetSettingValue(string xpath, XmlNode selectionNode, MenuViewLinkModeType defaultValue)
        {
            MenuViewLinkModeType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "Hidden":
                        returnValue = MenuViewLinkModeType.Hidden;
                        break;
                    case "AllValues":
                        returnValue = MenuViewLinkModeType.AllValues;
                        break;
                    default:
                        returnValue = MenuViewLinkModeType.DefaultValues;
                        break;
                }
            }
            return returnValue;
        }


        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding DownloadLinkVisibilityType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a DownloadLinkVisibilityType</param>
        public static PCAxis.Web.Controls.DownloadLinkVisibilityType GetSettingValue(string xpath, XmlNode selectionNode, PCAxis.Web.Controls.DownloadLinkVisibilityType defaultValue)
        {
            PCAxis.Web.Controls.DownloadLinkVisibilityType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "AlwaysShow":
                        returnValue = PCAxis.Web.Controls.DownloadLinkVisibilityType.AlwaysShow;
                        break;
                    case "ShowIfSmallFile":
                        returnValue = PCAxis.Web.Controls.DownloadLinkVisibilityType.ShowIfSmallFile;
                        break;
                    default:
                        returnValue = PCAxis.Web.Controls.DownloadLinkVisibilityType.AlwaysHide;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding SortType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a SortType</param>
        public static ChartSettings.SortType GetSettingValue(string xpath, XmlNode selectionNode, ChartSettings.SortType defaultValue)
        {
            ChartSettings.SortType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText.ToLower())
                {
                    case "ascending":
                        returnValue = ChartSettings.SortType.Ascending;
                        break;
                    case "descending":
                        returnValue = ChartSettings.SortType.Descending;
                        break;
                    default:
                        returnValue = ChartSettings.SortType.None;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding OrientationType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a OrientationType</param>
        public static ChartSettings.OrientationType GetSettingValue(string xpath, XmlNode selectionNode, ChartSettings.OrientationType defaultValue)
        {
            ChartSettings.OrientationType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText.ToLower())
                {
                    case "vertical":
                        returnValue = ChartSettings.OrientationType.Vertical;
                        break;
                    default:
                        returnValue = ChartSettings.OrientationType.Horizontal;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding SearchIndexStatusType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a SearchIndexStatusType</param>
        public static SearchIndexStatusType GetSettingValue(string xpath, XmlNode selectionNode, SearchIndexStatusType defaultValue)
        {
            SearchIndexStatusType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "Indexed":
                        returnValue = SearchIndexStatusType.Indexed;
                        break;
                    case "Indexing":
                        returnValue = SearchIndexStatusType.Indexing;
                        break;
                    case "NotIndexed":
                        returnValue = SearchIndexStatusType.NotIndexed;
                        break;
                    case "WaitingCreate":
                        returnValue = SearchIndexStatusType.WaitingCreate;
                        break;
                    case "WaitingUpdate":
                        returnValue = SearchIndexStatusType.WaitingUpdate;
                        break;
                    default:
                        returnValue = SearchIndexStatusType.NotIndexed;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns corresponding SavedQueryStorageType, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a SavedQueryStorageType</param>
        public static SavedQueryStorageType GetSettingValue(string xpath, XmlNode selectionNode, SavedQueryStorageType defaultValue)
        {
            SavedQueryStorageType returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                switch (node.InnerText)
                {
                    case "File":
                        returnValue = SavedQueryStorageType.File;
                        break;
                    case "Database":
                        returnValue = SavedQueryStorageType.Database;
                        break;
                    default:
                        returnValue = SavedQueryStorageType.File;
                        break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Selects single node from passed XmlNode and returns DateTime, if the node is not found the given defaultvalue is returned
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="defaultValue">Value to return if node is not found or not possible to parse as a DateTime</param>
        public static DateTime GetSettingValue(string xpath, XmlNode selectionNode, DateTime defaultValue)
        {
            DateTime returnValue = defaultValue;
            XmlNode node = null;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                if (PCAxis.Paxiom.Extensions.PxDate.IsPxDate(node.InnerText))
                {
                    returnValue = PCAxis.Paxiom.Extensions.PxDate.PxDateStringToDateTime(node.InnerText);
                }
            }
            return returnValue;
        }
        #endregion


        #region "Set values"

        /// <summary>
        /// Selects single node from passed XmlNode and sets given attributet to parameter value.
        /// </summary>
        /// <param name="xpath">Expression to select node containing attribute to change</param>
        /// <param name="attributeName">Name on attribute to change</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="newSettingValue">New arttribute value</param>
        /// <returns></returns>
        public static bool SetSettingAttributeValue(string xpath, string attributeName, XmlNode selectionNode, string newSettingValue)
        {
            XmlNode node = null;
            bool returnValue = false;
            if (selectionNode != null)
            {
                node = selectionNode.SelectSingleNode(xpath);
            }
            if (node != null)
            {
                if (node.Attributes[attributeName] != null)
                {
                    node.Attributes[attributeName].Value = newSettingValue;
                    returnValue = true;
                }
            }
            return returnValue;

        }


        /// <summary>
        /// Selects single node from passed XmlNode and sets the inner text to parameter value.
        /// </summary>
        /// <param name="xpath">Expression to select node containing setting value to change</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="newSettingValue">Value to change to</param>
        /// <returns>True if node is found, else false.</returns>
        public static bool SetSettingValue(string xpath, XmlNode selectionNode, string newSettingValue)
        {
            XmlNode node = null;
            if (selectionNode != null)
            {
                //node = selectionNode.SelectSingleNode(xpath);
                node = GetNode(selectionNode, xpath);
            }
            if (node != null)
            {
                node.InnerText = newSettingValue;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Selects single node from passed XmlNode, clears it and adds childnodes to it.
        /// </summary>
        /// <param name="xpath">Expression to select node to give the childnodes (eg. "./ipAddresses")</param>
        /// <param name="selectionNode">Node to apply xpath expression on</param>
        /// <param name="settingName">Same of setting to add as child to the selected node (eg. "ipAddress")</param>
        /// <param name="newSettingValues">Strings with values for the created childnodes (eg "127.0.0.1")</param>
        /// <returns>True if node is found, else false.</returns>
        public static bool SetSettingValue(string xpath, XmlNode selectionNode, string settingName, IEnumerable<string> newSettingValues)
        {
            XmlNode node = null;
            XmlNode n;
            if (selectionNode != null)
            {
                //node = selectionNode.SelectSingleNode(xpath);
                node = GetNode(selectionNode, xpath);
            }
            if (node != null)
            {
                node.RemoveAll();
                //Add all
                foreach (string settingValue in newSettingValues)
                {
                    n = selectionNode.OwnerDocument.CreateNode(XmlNodeType.Element, settingName, "");
                    n.InnerText = settingValue;
                    node.AppendChild(n);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region "Administration methods"
        
        /// <summary>
        /// Check if plugin shall be removed
        /// </summary>
        /// <param name="plugin">Plugin to check for removal</param>
        /// <returns>True if the plugin shall be removed, else false</returns>
        public static bool RemovePlugin(CommandBarPluginInfo plugin)
        {
            if (!PXWeb.Settings.Current.Features.General.ChartsEnabled)
            {
                //Remove chart plugins
                if (!String.IsNullOrEmpty(plugin.Category))
                {
                    if (plugin.Category.Equals(PCAxis.Web.Controls.Plugins.Categories.CHART))
                    {
                        // Do not display this plugin
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if file format shall be removed from list of available file formats (only in dropdown mode)
        /// </summary>
        /// <param name="fileGenerator">The file generator object</param>
        /// <param name="format">file format to check for removal</param>
        /// <returns></returns>
        public static bool RemoveFileFormat(string format)
        {
            if (!PXWeb.Settings.Current.Features.General.ChartsEnabled)
            {
                // Remove chart file formats
                if (CommandBarPluginManager.GetFileType(format).Category.Equals(PCAxis.Web.Controls.Plugins.Categories.CHART))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Check if file type shall be removed from list of available file types (only in button mode)
        /// </summary>
        /// <param name="fileGenerator">The file generator object</param>
        /// <param name="format">file type to check for removal</param>
        /// <returns></returns>
        public static bool RemoveFileType(PCAxis.Web.Core.FileType fileType)
        {
            if (!PXWeb.Settings.Current.Features.General.ChartsEnabled)
            {
                // Remove chart file types
                if (fileType.Category.Equals(PCAxis.Web.Controls.Plugins.Categories.CHART))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
