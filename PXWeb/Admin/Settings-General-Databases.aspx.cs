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
using System.Collections.Generic;
using PCAxis.Web.Controls;
using PCAxis.Paxiom.Extensions;

namespace PXWeb.Admin
{
    public partial class Settings_General_Databases : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                ReadSettings();
            }
        }


        /// <summary>
        /// Read and display Databases settings  
        /// </summary>
        private void ReadSettings()
        {
            rptPxDatabases.DataSource = PXWeb.Settings.Current.General.Databases.AllPxDatabases;
            rptPxDatabases.DataBind();
            rptCnmmDatabases.DataSource = PXWeb.Settings.Current.General.Databases.AllCnmmDatabases;
            rptCnmmDatabases.DataBind();
        }

        /// <summary>
        /// Is the PX database selected or not?
        /// </summary>
        /// <param name="o">Database object</param>
        /// <returns>True if the database is selected, else false</returns>
        protected bool IsPxSelected(object o)
        {
            DatabaseInfo dbInfo = (DatabaseInfo)o;

            foreach (string db in PXWeb.Settings.Current.General.Databases.PxDatabases)
            {
                if (dbInfo.Id == db)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Is the Cnmm database selected or not?
        /// </summary>
        /// <param name="o">Database object</param>
        /// <returns>True if the database is selected, else false</returns>
        protected bool IsCnmmSelected(object o)
        {
            DatabaseInfo dbInfo = (DatabaseInfo)o;

            foreach (string db in PXWeb.Settings.Current.General.Databases.CnmmDatabases)
            {
                if (dbInfo.Id == db)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Get string representing when the PX database was last updated
        /// </summary>
        /// <param name="o">Database object</param>
        /// <returns>LastUpdated string.</returns>
        protected string PxLastUpdated(object o)
        {
            DatabaseInfo dbInfo = (DatabaseInfo)o;
            if (dbInfo.LastUpdated != DateTime.MinValue)
            {
                string lastUpdate = dbInfo.LastUpdated.DateTimeToPxDateString();
                return lastUpdate.PxDate(true);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Is Px database generated or not?
        /// </summary>
        /// <param name="o">Database object</param>
        /// <returns>True if Menu.xml exists for the database, else false</returns>
        protected bool IsPxDbGenerated(object o)
        {
            if (PxLastUpdated(o).Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// Get string representing the status of the search index associated with the database
        /// </summary>
        /// <param name="o">Database object</param>
        /// <returns>Search index status string</returns>
        protected string IndexStatus(object o)
        {
            DatabaseInfo dbInfo = (DatabaseInfo)o;
            if (dbInfo == null)
            {
                return "";
            }

            if (!PXWeb.Settings.Current.Database.ContainsKey(dbInfo.Id))
            {
                // Handle unselected CNMM databases without a database folder
                return "";
            }

            //PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.Database[dbInfo.Id];
            PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(dbInfo.Id);
            if (db == null)
            {
                return "";
            }

            PXWeb.SearchIndexSettings searchIndex = (PXWeb.SearchIndexSettings)db.SearchIndex;
            if (searchIndex == null)
            {
                return "";
            }

            if (searchIndex.Status == SearchIndexStatusType.Indexed)
            {
                return searchIndex.IndexUpdated.PxDate(true);
            }

            return searchIndex.Status.ToString();
        }

        /// <summary>
        /// Check if it is possible to create new search index
        /// </summary>
        /// <param name="o">Database object</param>
        /// <returns>Search index status string</returns>
        protected bool CheckIndexStatus(object o)
        {
            DatabaseInfo dbInfo = (DatabaseInfo)o;
            if (dbInfo == null)
            {
                return false;
            }

            if (!PXWeb.Settings.Current.Database.ContainsKey(dbInfo.Id))
            {
                return true;
            }

            PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.Database[dbInfo.Id];
            if (db == null)
            {
                return false;
            }

            PXWeb.SearchIndexSettings searchIndex = (PXWeb.SearchIndexSettings)db.SearchIndex;
            if (searchIndex == null)
            {
                return false;
            }

            if (searchIndex.Status == SearchIndexStatusType.Indexing || 
                searchIndex.Status == SearchIndexStatusType.WaitingCreate || 
                searchIndex.Status == SearchIndexStatusType.WaitingUpdate)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save Databases settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {
            if (PXWeb.Settings.BeginUpdate())
            {
                try
                {
                    PXWeb.DatabasesSettings databases = (PXWeb.DatabasesSettings)PXWeb.Settings.NewSettings.General.Databases;

                    SetSelectedDatabases(rptPxDatabases, (List<string>)databases.PxDatabases);
                    SetSelectedDatabases(rptCnmmDatabases, (List<string>)databases.CnmmDatabases);

                    PXWeb.Settings.Save();
                }
                finally
                {
                    PXWeb.Settings.EndUpdate();
                    //Reload settings per database
                    PXWeb.Settings.Current.LoadDatabaseSettings();
                }
            }
        }

        /// <summary>
        /// Set which databases are selected by the user
        /// </summary>
        /// <param name="rpt">Repeater</param>
        /// <param name="lst">List containing database id:s</param>
        private void SetSelectedDatabases(Repeater rpt, List<string> lst)
        {
            lst.Clear();

            foreach (RepeaterItem itm in rpt.Items)
            {
                CheckBox chk = (CheckBox)itm.FindControl("chkDbItem");
                if (chk != null)
                {
                    if (chk.Checked)
                    {
                        lst.Add(chk.Text);
                    }
                }
            }
        }

        /// <summary>
        /// Create new index for clicked database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateNewIndex(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;

            switch (lnk.CommandName)
            {
                case "CreateIndex":
                    CreateIndex(lnk.CommandArgument);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Create search index for the selected database. One index per site language will be created.
        /// </summary>
        /// <param name="database"></param>
        private void CreateIndex(string database)
        {
            if (string.IsNullOrEmpty(database))
            {
                return;
            }

            PXWeb.DatabaseSettings db = (PXWeb.DatabaseSettings)PXWeb.Settings.Current.GetDatabase(database);
            PXWeb.SearchIndexSettings searchIndex = (PXWeb.SearchIndexSettings)db.SearchIndex;

            // Check that the status has not been changed by another system before updating it
            if (searchIndex.Status != SearchIndexStatusType.Indexing)
            {
                searchIndex.Status = SearchIndexStatusType.WaitingCreate;
                db.Save();
                
                if (PXWeb.Settings.Current.Features.General.BackgroundWorkerEnabled)
                {
                    // Wake up the background worker if it is asleep
                    BackgroundWorker.PxWebBackgroundWorker.WakeUp();
                }
            }

            ReadSettings();
        }

        protected void imgPxDatabases_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsDatabasesPxDatabases", "PxWebAdminSettingsDatabasesPxDatabasesInfo");
        }

        protected void imgCnmmDatabases_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsDatabasesCnmmDatabases", "PxWebAdminSettingsDatabasesCnmmDatabasesInfo");
        }


        protected void rptDatabases_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnk = (LinkButton)e.Item.FindControl("lnkIndex");
                if (lnk != null)
                {
                    lnk.Attributes.Add("onclick", "return confirm('" + Master.GetLocalizedString("PxWebAdminSettingsDatabasesConfirmCreateIndex") + "');");
                }
            }
        }

    }
}
