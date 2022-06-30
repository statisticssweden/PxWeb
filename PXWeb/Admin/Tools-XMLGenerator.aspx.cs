using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using PXWeb.Database;
using System.Xml;
using System.Globalization;
using System.Collections.Generic;

namespace PXWeb.Admin
{
    public partial class Tools_XMLGenerator : System.Web.UI.Page
    {
        protected void fillPxDatabases(DropDownList ddl)
        {
            foreach (var db in PXWeb.Settings.Current.General.Databases.AllPxDatabases)
            {
                ddl.Items.Add(new ListItem(db.Id, db.Id));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillPxDatabases(cboSelectDb);
            }

        }
        protected void imgSelectDb_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectDb", "PxWebAdminToolsXMLGeneratorSelectDbInfo");
        }

        protected void imgSelectPreferredLanguage_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectPreferredLanguage", "PxWebAdminToolsXMLGeneratorSelectPreferredLanguageInfo");
        }

        protected void imgSelectBaseURI_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectBaseURI", "PxWebAdminToolsXMLGeneratorSelectBaseURIInfo");
        }
        protected void imgSelectCatalogTitle_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectCatalogTitle", "PxWebAdminToolsXMLGeneratorSelectCatalogTitleInfo");
        }

        protected void imgSelectCatalogDesc_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectCatalogDesc", "PxWebAdminToolsXMLGeneratorSelectCatalogDescInfo");
        }
        protected void imgSelectLicense_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectLicense", "PxWebAdminToolsXMLGeneratorSelectLicenseInfo");
        }

        private string firstTwo(string s) {
            return s.Substring(0,2);
        }

        protected void btnGenerateXML_Click(object sender, EventArgs e)
        {
            string baseURI = textBoxSelectBaseURI.Text;
            string catalogTitle = textBoxSelectCatalogTitle.Text;
            string catalogDescription = textBoxSelectCatalogDesc.Text;
            string license = textBoxSelectLicense.Text;
            List<string> languages = new List<string>();
            string preferredLanguage = firstTwo(Settings.Current.General.Language.DefaultLanguage);
            foreach (LanguageSettings ls in Settings.Current.General.Language.SiteLanguages) {
                languages.Add(firstTwo(ls.Name));
            }
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectLicense",preferredLanguage);
            //Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectLicense", preferredLanguage);
        }

        /// <summary> 
        /// Sorts the dropdown in descending order 
        /// </summary> 
        /// <param name="pList">the list to sort</param> 
        /// <param name="pByValue">Sort the list by values or text</param>  
        private void sortDropDown(ref DropDownList pList, bool pByValue)
        {
            SortedList lListItems = new SortedList();
            string key;

            //add listbox items to SortedList  
            foreach (ListItem lItem in pList.Items)
            {
                if (!string.IsNullOrEmpty(lItem.Value))
                {
                    if (pByValue)
                    {
                        key = lItem.Value + " (" + lItem.Text + ")";
                    }
                    else
                    {
                        key = lItem.Text + " (" + lItem.Value + ")";
                    }

                    if (!lListItems.Contains(key))
                    {
                        lListItems.Add(key, lItem);
                    }
                }
            }

            //clear dropdown 
            pList.Items.Clear();

            //add sorted items to dropdown 
            for (int i = 0; i < lListItems.Count; i++)
            {
                pList.Items.Add((ListItem)lListItems[lListItems.GetKey(i)]);
            }
        }
    }
}
