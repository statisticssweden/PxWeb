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
        string _fallbackPath;
        string _languagePath;
        XmlNodeList _xmlnodesFallback;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCultures();

                foreach (var db in PXWeb.Settings.Current.General.Databases.AllPxDatabases)
                {
                    cboSelectDb.Items.Add(new ListItem(db.Id, db.Id));
                }

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

        protected void btnGenerateXML_Click(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// Populate dropdown with languages
        /// </summary>
        private void GetCultures()
        {
            cboLanguage.DataSource = CultureInfo.GetCultures(CultureTypes.AllCultures);
            cboLanguage.DataTextField = "EnglishName";
            cboLanguage.DataValueField = "Name";
            cboLanguage.DataBind();
            sortDropDown(ref cboLanguage, false);

            ListItem li = new ListItem(Master.GetLocalizedString("PxWebAdminToolsLanguageManagerSelectLanguage"), "");
            cboLanguage.Items.Insert(0, li);
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
