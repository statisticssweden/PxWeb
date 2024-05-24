using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the General.Global settings
    /// </summary>
    internal class GlobalSettings : IGlobalSettings
    {
        #region "Private fields"

        /// <summary>
        /// General.Global.ShowInformationTypesSettings settings
        /// </summary>
        private ShowInformationTypesSettings _showInformationTypesSettings;

        #endregion

        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="globalNode">XML-node for the General.Global settings</param>
        public GlobalSettings(XmlNode globalNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./removeSingleContent";
            RemoveSingleContent = SettingsHelper.GetSettingValue(xpath, globalNode, true);

            xpath = "./strictAggregationCheck";
            StrictAggregationCheck = SettingsHelper.GetSettingValue(xpath, globalNode, true);

            xpath = "./secrecyOption";
            SecrecyOption = SettingsHelper.GetSettingValue(xpath, globalNode, PCAxis.Paxiom.SecrecyOptionType.None);

            xpath = "./roundingRule";
            RoundingRule = SettingsHelper.GetSettingValue(xpath, globalNode, PCAxis.Paxiom.RoundingType.BankersRounding);

            xpath = "./symbol1";
            Symbol1 = SettingsHelper.GetSettingValue(xpath, globalNode, ".");

            xpath = "./symbol2";
            Symbol2 = SettingsHelper.GetSettingValue(xpath, globalNode, "..");

            xpath = "./symbol3";
            Symbol3 = SettingsHelper.GetSettingValue(xpath, globalNode, "...");

            xpath = "./symbol4";
            Symbol4 = SettingsHelper.GetSettingValue(xpath, globalNode, "....");

            xpath = "./symbol5";
            Symbol5 = SettingsHelper.GetSettingValue(xpath, globalNode, ".....");

            xpath = "./symbol6";
            Symbol6 = SettingsHelper.GetSettingValue(xpath, globalNode, "......");

            xpath = "./dataSymbolNil";
            DataSymbolNil = SettingsHelper.GetSettingValue(xpath, globalNode, "#");

            xpath = "./dataSymbolSum";
            DataSymbolSum = SettingsHelper.GetSettingValue(xpath, globalNode, "#SUM");

            //xpath = "./decimalSeparator";
            //DecimalSeparator = SettingsHelper.GetSettingValue(xpath, globalNode, "Comma");

            //xpath = "./thousandSeparator";
            //ThousandSeparator = SettingsHelper.GetSettingValue(xpath, globalNode, "Space");

            xpath = "./showSourceDescription";
            ShowSourceDescription = SettingsHelper.GetSettingValue(xpath, globalNode, true);

            xpath = "./tableInformationLevel";
            TableInformationLevel = SettingsHelper.GetSettingValue(xpath, globalNode, PCAxis.Paxiom.InformationLevelType.AllInformation);

            xpath = "./uppercase";
            Uppercase = SettingsHelper.GetSettingValue(xpath, globalNode, true);

            xpath = "./dataNotePlacment";
            DataNotePlacement = SettingsHelper.GetSettingValue(xpath, globalNode, PCAxis.Paxiom.DataNotePlacementType.After);

            xpath = "./showInformationTypes";
            node = globalNode.SelectSingleNode(xpath);
            _showInformationTypesSettings = new ShowInformationTypesSettings(node);

            xpath = "./showinfofile";
            ShowInfoFile = SettingsHelper.GetSettingValue(xpath, globalNode, true);
        }

        /// <summary>
        /// Save the General.Global settings to the settings file
        /// </summary>
        /// <param name="globalNode">XML-node for the General.Global settings</param>
        public void Save(XmlNode globalNode)
        {
            string xpath;
            XmlNode node;

            xpath = "./removeSingleContent";
            SettingsHelper.SetSettingValue(xpath, globalNode, RemoveSingleContent.ToString());

            xpath = "./strictAggregationCheck";
            SettingsHelper.SetSettingValue(xpath, globalNode, StrictAggregationCheck.ToString());

            xpath = "./secrecyOption";
            SettingsHelper.SetSettingValue(xpath, globalNode, SecrecyOption.ToString());

            xpath = "./roundingRule";
            SettingsHelper.SetSettingValue(xpath, globalNode, RoundingRule.ToString());

            xpath = "./symbol1";
            SettingsHelper.SetSettingValue(xpath, globalNode, Symbol1.ToString());

            xpath = "./symbol2";
            SettingsHelper.SetSettingValue(xpath, globalNode, Symbol2.ToString());

            xpath = "./symbol3";
            SettingsHelper.SetSettingValue(xpath, globalNode, Symbol3.ToString());

            xpath = "./symbol4";
            SettingsHelper.SetSettingValue(xpath, globalNode, Symbol4.ToString());

            xpath = "./symbol5";
            SettingsHelper.SetSettingValue(xpath, globalNode, Symbol5.ToString());

            xpath = "./symbol6";
            SettingsHelper.SetSettingValue(xpath, globalNode, Symbol6.ToString());

            xpath = "./dataSymbolNil";
            SettingsHelper.SetSettingValue(xpath, globalNode, DataSymbolNil.ToString());

            xpath = "./dataSymbolSum";
            SettingsHelper.SetSettingValue(xpath, globalNode, DataSymbolSum.ToString());

            //xpath = "./decimalSeparator";
            //SettingsHelper.SetSettingValue(xpath, globalNode, DecimalSeparator.ToString());

            //xpath = "./thousandSeparator";
            //SettingsHelper.SetSettingValue(xpath, globalNode, ThousandSeparator.ToString());

            xpath = "./showSourceDescription";
            SettingsHelper.SetSettingValue(xpath, globalNode, ShowSourceDescription.ToString());

            xpath = "./tableInformationLevel";
            SettingsHelper.SetSettingValue(xpath, globalNode, TableInformationLevel.ToString());

            xpath = "./uppercase";
            SettingsHelper.SetSettingValue(xpath, globalNode, Uppercase.ToString());

            xpath = "./dataNotePlacment";
            SettingsHelper.SetSettingValue(xpath, globalNode, DataNotePlacement.ToString());

            xpath = "./showinfofile";
            SettingsHelper.SetSettingValue(xpath, globalNode, ShowInfoFile.ToString());

            xpath = "./showInformationTypes";
            node = globalNode.SelectSingleNode(xpath);
            _showInformationTypesSettings.Save(node);
        }

        #endregion


        #region IGlobalSettings Members

        public bool RemoveSingleContent { get; set; }
        public bool StrictAggregationCheck { get; set; }
        public PCAxis.Paxiom.SecrecyOptionType SecrecyOption { get; set; }
        public PCAxis.Paxiom.RoundingType RoundingRule { get; set; }
        public string Symbol1 { get; set; }
        public string Symbol2 { get; set; }
        public string Symbol3 { get; set; }
        public string Symbol4 { get; set; }
        public string Symbol5 { get; set; }
        public string Symbol6 { get; set; }
        public string DataSymbolNil { get; set; }
        public string DataSymbolSum { get; set; }
        //public string DecimalSeparator { get; set; }
        //public string ThousandSeparator { get; set; }
        public bool ShowSourceDescription { get; set; }
        public PCAxis.Paxiom.InformationLevelType TableInformationLevel { get; set; }
        public bool Uppercase { get; set; }
        public bool ShowInfoFile { get; set; }
        public PCAxis.Paxiom.DataNotePlacementType DataNotePlacement { get; set; }
        public IShowInformationTypesSettings ShowInformationTypes
        {
            get { return _showInformationTypesSettings; }
        }

        #endregion
    }
}
