using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the Selection.Hierarchies settings
    /// </summary>
    internal class HierarchiesSettings : IHierarchiesSettings
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hierarchiesNode">XML-node for the Selection.Hierarchies settings</param>
        public HierarchiesSettings(XmlNode hierarchiesNode)
        {
            string xpath;
            //XmlNode node;

            xpath = "./showHierarchies";
            ShowHierarchies = SettingsHelper.GetSettingValue(xpath, hierarchiesNode, true);

            xpath = "./hierarchicalLevelsOpen";
            HierarchicalLevelsOpen = SettingsHelper.GetSettingValue(xpath, hierarchiesNode, 1);
        }

        /// <summary>
        /// Save the Selection.Hierarchies settings to the settings file
        /// </summary>
        /// <param name="hierarchiesNode">XML-node for the Selection.Hierarchies settings</param>
        public void Save(XmlNode hierarchiesNode)
        {
            string xpath;
            //XmlNode node;

            xpath = "./showHierarchies";
            SettingsHelper.SetSettingValue(xpath, hierarchiesNode, ShowHierarchies.ToString());

            xpath = "./hierarchicalLevelsOpen";
            SettingsHelper.SetSettingValue(xpath, hierarchiesNode, HierarchicalLevelsOpen.ToString());
        }

        #endregion


        #region IHierarchiesSettings Members

        public bool ShowHierarchies { get; set; }
        public int HierarchicalLevelsOpen { get; set; }

        #endregion
    }
}
