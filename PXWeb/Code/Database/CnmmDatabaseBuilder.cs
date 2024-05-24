//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using PCAxis.Menu.Implementations;
//using System.Diagnostics;
//using PCAxis.Menu;

//namespace PXWeb.Database
//{
//    /// <summary>
//    /// Class that builds menu xml-files for CNMM databases
//    /// </summary>
//    public class CnmmDatabaseBuilder
//    {
//        private string _databaseId;
//        private System.Xml.XmlDocument doc;
//        public CnmmDatabaseBuilder(string databaseId)
//        {
//            _databaseId = databaseId;
//            doc = new System.Xml.XmlDocument();
//            string configFile = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("dbconfigFile");
//            doc.Load(configFile);
//        }

//        public List<DatabaseMessage> Generate()
//        { 
//            string defaultLanguage = GetDefaultLanguageCode();
//            List<string> languages = GetOtherLanguages();
//            List<DatabaseMessage> msg = new List<DatabaseMessage>();
//            msg.Add(new DatabaseMessage(){ MessageType = DatabaseMessage.BuilderMessageType.Information, Message = "Start"});

//            //Added CNMM defult language if it differs from the PX-Web default language.
//            if (defaultLanguage != PXWeb.Settings.Current.General.Language.DefaultLanguage.ToLower())
//            {
//                languages.Add(defaultLanguage);
//            }

//            string rootPath = System.Web.HttpContext.Current.Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath);
//            //creates the xml file for the default language
//            if (String.Compare(defaultLanguage, PXWeb.Settings.Current.General.Language.DefaultLanguage, true) == 0)
//            {
//                CreateCmmnXml(defaultLanguage, 
//                    System.IO.Path.Combine(rootPath ,
//                                           _databaseId + ".xml"));
//            }
//            else if (languages.Contains(PXWeb.Settings.Current.General.Language.DefaultLanguage.ToLower()))
//            {
//                CreateCmmnXml(PXWeb.Settings.Current.General.Language.DefaultLanguage,
//                    System.IO.Path.Combine(rootPath,
//                                           _databaseId + ".xml"));
//            }
//            else
//            {
//                CreateCmmnXml(defaultLanguage,
//                    System.IO.Path.Combine(rootPath,
//                                           _databaseId + ".xml"));
//            }

//            // creates a xml menu file for the rest of the site languages
//            foreach (var language in PXWeb.Settings.Current.General.Language.SiteLanguages)
//            {
//                if (language != PXWeb.Settings.Current.General.Language.DefaultLanguage &&
//                    languages.Contains(language.ToLower()))
//                {
//                    CreateCmmnXml(language, 
//                        System.IO.Path.Combine(rootPath,
//                                           _databaseId + "." + language + ".xml"));    
//                }
//            }
//            msg.Add(new DatabaseMessage(){ MessageType = DatabaseMessage.BuilderMessageType.Information, Message = "End"});
//            return msg;
//        }

//        private string GetDefaultLanguageCode()
//        {
//            System.Xml.XmlElement node = (System.Xml.XmlElement)doc.SelectSingleNode(String.Format("/SqlDbConfig/Database[@id='{0}']/Languages/Language[@main='true']", _databaseId));
//            return node.Attributes["code"].Value;

//        }

//        private List<string> GetOtherLanguages()
//        {
//            List<string> langs = new List<string>();

//            var nodes = doc.SelectNodes(String.Format("/SqlDbConfig/Database[@id='{0}']/Languages/Language[@main='false']", _databaseId ));

//            foreach (System.Xml.XmlElement node in nodes)
//            {
//                langs.Add(node.Attributes["code"].Value);
//            }
//            return langs;
//        }

//        private void CreateCmmnXml(string language, string path)
//        {
//            DatamodelMenu.DatamodelMenuSettings settings;
//            settings = new DatamodelMenu.DatamodelMenuSettings();

//            //if (string.IsNullOrEmpty(language))
//            //{
//            //    settings.ExtractionLanguage = DatamodelMenu.ExtractionLanguage.Default;
//            //}
//            //else
//            //{
//            //    settings.ExtractionLanguage = DatamodelMenu.ExtractionLanguage.Other;
//            //}
//            settings.RestrictionMethod =
//            item =>
//            {
//                return true;
//            };
//            settings.AlterItemBeforeStorage =
//            item =>
//            {
//                if (item is PCAxis.Menu.Url)
//                {
//                    PCAxis.Menu.Url url = (PCAxis.Menu.Url)item;
//                    //url.LinkUrl = "http://www.scb.se/" + url.LinkUrl;
//                }
//                //item.Selection = "URL_" + item.Selection;
//                if (item is TableLink)
//                {
//                    TableLink tbl = (TableLink)item;
//                    tbl.Text = tbl.Text + " " + tbl.StartTime + " - " + tbl.EndTime;
//                    if (tbl.Published.HasValue )
//                    {
//                        tbl.SetAttribute("modified", tbl.Published.Value.ToShortDateString());
//                    }
//                }
//                if (String.IsNullOrEmpty(item.SortCode))
//                {
//                    item.SortCode = item.Text;
//                }
//            };

//            DatamodelMenu menu = PCAxis.Menu.Implementations.ConfigDatamodelMenu.Create(
//                        language,
//                        null,
//                        settings);

//            menu.LoadAll();

//            menu.RootItem.Sort();

//            menu.GetAsXML().Save(path);

//        }
//    }
//}
