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

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the General.Global.ShowInformationTypesSettings settings
    /// </summary>
    internal class ShowInformationTypesSettings : IShowInformationTypesSettings 
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="infoNode">XML-node for the General.Global.ShowInformationTypes settings</param>
        public ShowInformationTypesSettings(XmlNode infoNode)
        {
            string xpath;
            //XmlNode node;

            xpath = "./officialStatistics";
            OfficialStatistics = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);
            
            xpath = "./unit";
            Unit = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./contact";
            Contact = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./lastUpdated";
            LastUpdated = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./refPeriod";
            RefPeriod = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./stockFA";
            StockFa = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./cfPrices";
            CFPrices = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./dayADj";
            DayAdj = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./seasAdj";
            SeasAdj = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./basePeriod";
            BasePeriod = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./updateFrequency";
            UpdateFrequency = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./nextUpdate";
            NextUpdate = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./survey";
            Survey = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./link";
            Link = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./creationDate";
            CreationDate = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./copyright";
            Copyright = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./source";
            Source = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./matrix";
            Matrix = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);

            xpath = "./database";
            Database = SettingsHelper.GetSettingAttributeValue(xpath, "enabled", infoNode, true);
        }


        /// <summary>
        /// Save the General.Global.ShowInformationTypes settings to the settings file
        /// </summary>
        /// <param name="infoNode">XML-node for the General.Global.ShowInformationTypes settings</param>
        public void Save(XmlNode infoNode)
        {
            string xpath;

            xpath = "./officialStatistics";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, OfficialStatistics.ToString());
            
            xpath = "./unit";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, Unit.ToString());

            xpath = "./contact";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, Contact.ToString());

            xpath = "./lastUpdated";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, LastUpdated.ToString());

            xpath = "./refPeriod";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, RefPeriod.ToString());

            xpath = "./stockFA";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, StockFa.ToString());

            xpath = "./cfPrices";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, CFPrices.ToString());

            xpath = "./dayADj";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, DayAdj.ToString());

            xpath = "./seasAdj";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, SeasAdj.ToString());

            xpath = "./basePeriod";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, BasePeriod.ToString());

            xpath = "./updateFrequency";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, UpdateFrequency.ToString());

            xpath = "./nextUpdate";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, NextUpdate.ToString());

            xpath = "./survey";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, Survey.ToString());

            xpath = "./link";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, Link.ToString());

            xpath = "./creationDate";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, CreationDate.ToString());

            xpath = "./copyright";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, Copyright.ToString());

            xpath = "./source";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, Source.ToString());

            xpath = "./matrix";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, Matrix.ToString());

            xpath = "./database";
            SettingsHelper.SetSettingAttributeValue(xpath, "enabled", infoNode, Database.ToString());
        }



        #endregion


        #region IShowInformationTypesSettings Members

        public bool OfficialStatistics { get; set; }
        public bool Unit { get; set; }
        public bool Contact { get; set; }
        public bool LastUpdated { get; set; }
        public bool RefPeriod { get; set; }
        public bool StockFa { get; set; }
        public bool CFPrices { get; set; }
        public bool DayAdj { get; set; }
        public bool SeasAdj { get; set; }
        public bool BasePeriod { get; set; }
        public bool UpdateFrequency { get; set; }
        public bool NextUpdate { get; set; }
        public bool Survey { get; set; }
        public bool Link { get; set; }
        public bool CreationDate { get; set; }
        public bool Copyright { get; set; }
        public bool Source { get; set; }
        public bool Matrix { get; set; }
        public bool Database { get; set; }

        /// <summary>
        /// Get all selected InformationTypes
        /// </summary>
        /// <returns>List cointaing all the InformationTypes that are selected</returns>
        public List<PCAxis.Enums.InformationType> GetSelectedInformationTypes()
        {
            List<PCAxis.Enums.InformationType> lst = new List<PCAxis.Enums.InformationType>();

            foreach (PCAxis.Enums.InformationType infoType in System.Enum.GetValues(typeof(PCAxis.Enums.InformationType)))
            {
                switch (infoType)
                {
                    case PCAxis.Enums.InformationType.BasePeriod:
                        if (BasePeriod) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.CFPrices:
                        if (CFPrices) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.Contact:
                        if (Contact) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.Copyright:
                        if (Copyright) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.CreationDate:
                        if (CreationDate) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.Database:
                        if (Database) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.DayAdj:
                        if (DayAdj) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.LastUpdated:
                        if (LastUpdated) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.Link:
                        if (Link) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.Matrix:
                        if (Matrix) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.NextUpdate:
                        if (NextUpdate) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.RefPeriod:
                        if (RefPeriod) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.SeasAdj:
                        if (SeasAdj) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.Source:
                        if (Source) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.StockFa:
                        if (StockFa) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.Survey:
                        if (Survey) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.Unit:
                        if (Unit) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.OfficialStatistics:
                        if (OfficialStatistics) { lst.Add(infoType); }
                        break;
                    case PCAxis.Enums.InformationType.UpdateFrequency:
                        if (UpdateFrequency) { lst.Add(infoType); }
                        break;
                    default:
                        break;
                }
            }

            return lst;
        }
        
        #endregion
    }
}
