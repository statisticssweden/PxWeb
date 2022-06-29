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
        protected void fillDatabases(DropDownList ddl)
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
                GetCultures(cboLanguage);
                fillDatabases(cboSelectDb);
                List<DropDownList> list = new List<DropDownList> { new DropDownList() };
                GetCultures(list[0]);
                rptLanguages.DataSource = list;
                rptLanguages.DataBind();
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
            string baseURI = textBoxSelectBaseURI.Text;
            string catalogTitle = textBoxSelectCatalogTitle.Text;
            string catalogDescription = textBoxSelectCatalogDesc.Text;
            string license = textBoxSelectLicense.Text;
            string preferredLanguage = string.Join("",cboLanguage.Text.Take(2));
            Master.ShowInfoDialog("PxWebAdminToolsXMLGeneratorSelectLicense", preferredLanguage);
        }



        /// <summary>
        /// Populate dropdown with languages
        /// </summary>
        private void GetCultures(DropDownList ddl)
        {
            ddl.DataSource = CultureInfo.GetCultures(CultureTypes.AllCultures);
            ddl.DataTextField = "EnglishName";
            ddl.DataValueField = "Name";
            ddl.DataBind();
            sortDropDown(ref ddl, false);

            ListItem li = new ListItem(Master.GetLocalizedString("PxWebAdminToolsLanguageManagerSelectLanguage"), "");
            ddl.Items.Insert(0, li);
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
