using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCAxis.Web.Core.Management;
using System.IO;
using System.Globalization;

namespace PXWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel = null;
            PCAxis.Web.Controls.VariableSelector.SelectedVariableValues.Clear();
            if (!IsPostBack)
            {
                Master.SetBreadcrumb(PCAxis.Web.Controls.Breadcrumb.BreadcrumbMode.Home);
            }

            Master.HeadTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebTitleDefault");

        }

        protected override void OnInit(EventArgs args)
        {
            SetLocalizedTexts();
            GetDatabases();
            base.OnInit(args);
        }

        private void GetDatabases()
        {
            List<DatabaseInfo> dbinfoList = new List<DatabaseInfo>();
            foreach (string pxDatabase in PXWeb.Settings.Current.General.Databases.PxDatabases)
            {
                DatabaseInfo foundDb = PXWeb.Settings.Current.General.Databases.GetPxDatabase(pxDatabase);
                if (foundDb != null)
                {
                    if (foundDb.HasLanguage(LocalizationManager.CurrentCulture.Name))
                    {
                        dbinfoList.Add(foundDb);
                    }
                }
            }
            foreach (string cnmmDatabase in PXWeb.Settings.Current.General.Databases.CnmmDatabases)
            {
                DatabaseInfo foundDb = PXWeb.Settings.Current.General.Databases.GetCnmmDatabase(cnmmDatabase);
                if (foundDb != null)
                {
                    dbinfoList.Add(foundDb);
                }
            }
            rptDatabases.DataSource = dbinfoList;
            rptDatabases.DataBind();
        }

        private void SetLocalizedTexts()
        {
            string lang = LocalizationManager.GetTwoLetterLanguageCode();

            ((PxWeb)this.Master).FooterText = "Default";
        }

        //Removed Request.ApplicationPath from CreateLink(...
        //so that the links will be right even if the site is running without UserFriendlyURL
        protected void rptDatabases_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkManager.LinkItem[] li = new LinkManager.LinkItem[2];
            RepeaterItem itm = e.Item;
           
            if ((itm.ItemType == ListItemType.Item) || (itm.ItemType == ListItemType.AlternatingItem)) 
            {
                HyperLink lnk = (HyperLink)itm.FindControl("lnkDatabasesItem");
                li[0] = new LinkManager.LinkItem("px_language", LocalizationManager.GetTwoLetterLanguageCode());
                li[1] = new LinkManager.LinkItem("px_db", ((DatabaseInfo)itm.DataItem).Id);
                lnk.NavigateUrl = PCAxis.Web.Core.Management.LinkManager.CreateLink("Menu.aspx".Replace("\\", "/"), false, li);                
            } 
        }

        /// <summary>
        /// Get the database name in the currently selected language
        /// </summary>
        /// <param name="o">The databse object</param>
        /// <returns>The database name</returns>
        protected string DatabaseName(object o)
        {
            DatabaseInfo dbInfo = (DatabaseInfo)o;
            return dbInfo.GetDatabaseName(LocalizationManager.GetTwoLetterLanguageCode());
        }
    }
}
