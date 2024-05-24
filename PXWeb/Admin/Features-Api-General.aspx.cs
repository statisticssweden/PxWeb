using PCAxis.Paxiom.Configuration;
using PCAxis.Query;
using PXWeb.Misc;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PXWeb.Admin
{
    public partial class Features_Api_General : System.Web.UI.Page
    {
        private const char SEPARATOR = '|';

        protected void Page_init(object sender, EventArgs e)
        {
            CreateDatabaseTable(tblPxDatabases, PXWeb.Settings.Current.General.Databases.AllPxDatabases);
            CreateDatabaseTable(tblCnmmDatabases, PXWeb.Settings.Current.General.Databases.AllCnmmDatabases);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                ReadSettings();
            }
        }

        /// <summary>
        /// Read and display API settings  
        /// </summary>
        private void ReadSettings()
        {
            GetExposedDatabases();
            GetDatabasesLanguages();
            txtRoutePrefix.Text = PXWeb.Settings.Current.Features.Api.RoutePrefix;
            txtMaxValuesReturned.Text = PXWeb.Settings.Current.Features.Api.MaxValuesReturned.ToString();
            txtLimiterRequests.Text = PXWeb.Settings.Current.Features.Api.LimiterRequests.ToString();
            txtLimiterTimespan.Text = PXWeb.Settings.Current.Features.Api.LimiterTimespan.ToString();
            txtFetchCellLimit.Text = PXWeb.Settings.Current.Features.Api.FetchCellLimit.ToString();
            cboEnableCORS.SelectedValue = PXWeb.Settings.Current.Features.Api.EnableCORS.ToString();
            cboEnableCache.SelectedValue = PXWeb.Settings.Current.Features.Api.EnableCache.ToString();
            cboEnableApiV2QueryLink.SelectedValue = PXWeb.Settings.Current.Features.Api.EnableApiV2QueryLink.ToString();
            //txtClearCache.Text = PXWeb.Settings.Current.Features.Api.ClearCache;
            cboShowQueryInformation.SelectedValue = PXWeb.Settings.Current.Features.Api.ShowQueryInformation.ToString();
            cboDefaultExampleResponseFormat.SelectedValue = PXWeb.Settings.Current.Features.Api.DefaultExampleResponseFormat;
            cboShowSaveApiQueryButton.SelectedValue = PXWeb.Settings.Current.Features.Api.ShowSaveApiQueryButton.ToString();
            txtSaveApiQueryText.Text = PXWeb.Settings.Current.Features.Api.SaveApiQueryText.ToString();
            ShowHideTextboxForQueryPrefix();
            txtUrlRoot.Text = PXWeb.Settings.Current.Features.Api.UrlRoot;
            txtUrlRootV2.Text = PXWeb.Settings.Current.Features.Api.UrlRootV2;
        }

        /// <summary>
        /// Checks the checkboxes in the tables for the databases that are exposed via the API
        /// </summary>
        private void GetExposedDatabases()
        {
            foreach (LanguageSettings lang in PXWeb.Settings.Current.General.Language.SiteLanguages)
            {
                if (PCAxis.Query.ExposedDatabases.DatabaseConfigurations.ContainsKey(lang.Name))
                {
                    //PX databases
                    foreach (DatabaseInfo db in PXWeb.Settings.Current.General.Databases.AllPxDatabases)
                    {
                        CheckIfExposedDatabase(tblPxDatabases, lang, db);
                    }

                    //CNMM databases
                    foreach (DatabaseInfo db in PXWeb.Settings.Current.General.Databases.AllCnmmDatabases)
                    {
                        CheckIfExposedDatabase(tblCnmmDatabases, lang, db);
                    }

                }
            }
        }

        /// <summary>
        /// Enables and disables the checkboxes in the tables for the databases if they support the languages.
        /// </summary>
        private void GetDatabasesLanguages()
        {
            foreach (LanguageSettings lang in PXWeb.Settings.Current.General.Language.SiteLanguages)
            {
                //PX databases
                foreach (DatabaseInfo db in PXWeb.Settings.Current.General.Databases.AllPxDatabases)
                {
                    CheckIfDatabaseHasLanguage(tblPxDatabases, lang, db);
                }

                //CNMM databases
                foreach (DatabaseInfo db in PXWeb.Settings.Current.General.Databases.AllCnmmDatabases)
                {
                    CheckIfDatabaseHasLanguage(tblCnmmDatabases, lang, db);
                }

            }
        }

        /// <summary>
        /// If the database is exposed via the API its checkbox in the table will be checked 
        /// </summary>
        /// <param name="tbl">Table web control</param>
        /// <param name="lang">Language</param>
        /// <param name="db">Database</param>
        private void CheckIfExposedDatabase(System.Web.UI.WebControls.Table tbl, LanguageSettings lang, DatabaseInfo db)
        {
            if (PCAxis.Query.ExposedDatabases.DatabaseConfigurations[lang.Name].ContainsKey(db.Id))
            {
                CheckBox chk = (CheckBox)tbl.FindControl(lang.Name + SEPARATOR + db.Id);
                if (chk != null)
                {
                    chk.Checked = true;
                }
            }
        }

        /// <summary>
        /// Enables and disables the checkboxes in the tables for the databases if they support the languages. 
        /// </summary>
        /// <param name="tbl">Table web control</param>
        /// <param name="lang">Language</param>
        /// <param name="db">Database</param>
        private void CheckIfDatabaseHasLanguage(System.Web.UI.WebControls.Table tbl, LanguageSettings lang, DatabaseInfo db)
        {
            CheckBox chk = (CheckBox)tbl.FindControl(lang.Name + SEPARATOR + db.Id);

            if (!db.HasLanguage(lang.Name))
            {
                chk.Enabled = false;
            }

        }

        /// <summary>
        /// Create table showing databases and languages
        /// </summary>
        /// <param name="tbl">Table control</param>
        /// <param name="databases">Collection of database names</param>
        private void CreateDatabaseTable(System.Web.UI.WebControls.Table tbl, IEnumerable<DatabaseInfo> databases)
        {
            TableRow newRow;
            TableCell newCell;

            AddDatabaseTableHeader(tbl);

            foreach (DatabaseInfo db in databases)
            {
                newRow = new TableRow();
                tbl.Rows.Add(newRow);
                newCell = new TableCell();
                newCell.CssClass = "apiDatabaseCell";
                newCell.Text = db.Id;
                newRow.Cells.Add(newCell);

                foreach (LanguageSettings lang in PXWeb.Settings.Current.General.Language.SiteLanguages)
                {
                    newCell = new TableCell();
                    CheckBox chk = new CheckBox();
                    chk.ID = lang.Name + SEPARATOR + db.Id;
                    newCell.Controls.Add(chk);
                    newRow.Cells.Add(newCell);
                }
            }
        }

        /// <summary>
        /// Create header for database table
        /// </summary>
        /// <param name="tbl">Table control</param>
        private void AddDatabaseTableHeader(System.Web.UI.WebControls.Table tbl)
        {
            TableRow newRow;
            TableCell newCell;

            newRow = new TableRow();
            tbl.Rows.Add(newRow);

            newCell = new TableCell();
            newCell.CssClass = "apiDatabaseCell";
            newRow.Cells.Add(newCell);

            foreach (LanguageSettings lang in PXWeb.Settings.Current.General.Language.SiteLanguages)
            {
                newCell = new TableCell();
                newCell.Text = lang.Name;
                newRow.Cells.Add(newCell);
            }
        }

        /// <summary>
        /// Save API settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                if (PXWeb.Settings.BeginUpdate())
                {
                    try
                    {
                        PXWeb.ApiSettings api = (PXWeb.ApiSettings)PXWeb.Settings.NewSettings.Features.Api;

                        api.RoutePrefix = txtRoutePrefix.Text;
                        api.MaxValuesReturned = int.Parse(txtMaxValuesReturned.Text);
                        api.LimiterRequests = int.Parse(txtLimiterRequests.Text);
                        api.LimiterTimespan = int.Parse(txtLimiterTimespan.Text);
                        api.FetchCellLimit = int.Parse(txtFetchCellLimit.Text);
                        api.EnableCORS = bool.Parse(cboEnableCORS.SelectedValue);
                        api.EnableCache = bool.Parse(cboEnableCache.SelectedValue);
                        //api.ClearCache = txtClearCache.Text;
                        api.ShowQueryInformation = bool.Parse(cboShowQueryInformation.SelectedValue);
                        api.DefaultExampleResponseFormat = cboDefaultExampleResponseFormat.SelectedValue;
                        api.ShowSaveApiQueryButton = bool.Parse(cboShowSaveApiQueryButton.SelectedValue);
                        api.SaveApiQueryText = txtSaveApiQueryText.Text;

                        api.UrlRoot = txtUrlRoot.Text;
                        api.UrlRootV2 = txtUrlRootV2.Text;
                        api.EnableApiV2QueryLink = bool.Parse(cboEnableApiV2QueryLink.SelectedValue);

                        PXWeb.Settings.Save();

                        SaveExposedDatabases();

                        PCAxis.Api.Settings.Current.MaxValues = api.MaxValuesReturned;
                        PCAxis.Api.Settings.Current.LimiterRequests = api.LimiterRequests;
                        PCAxis.Api.Settings.Current.LimiterTimeSpan = api.LimiterTimespan;
                        PCAxis.Api.Settings.Current.FetchCellLimit = api.FetchCellLimit;
                        PCAxis.Api.Settings.Current.EnableCORS = api.EnableCORS;
                        PCAxis.Api.Settings.Current.EnableCache = api.EnableCache;
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// Save the databases exposed via the API to the configuration file
        /// </summary>
        private void SaveExposedDatabases()
        {
            Dictionary<string, List<DbConfig>> dictDb = new Dictionary<string, List<DbConfig>>();

            AddExposedDatabases(dictDb, tblPxDatabases, "PX");
            AddExposedDatabases(dictDb, tblCnmmDatabases, "CNMM");

            ExposedDatabasesManager dbMan = new ExposedDatabasesManager(AppSettingsHelper.GetAppSettingsPath("dbmetaFile"));
            dbMan.Save(dictDb);
            PCAxis.Query.ExposedDatabases.Reload();
            //PCAxis.Api.ApiCache.InvalidateCache();
            PCAxis.Api.ApiCache.Current.Clear();
        }

        /// <summary>
        /// Add exposed databases to save-dictionary
        /// </summary>
        /// <param name="dictDb">Dictionary containing databases to save to configuration file</param>
        /// <param name="tbl">Table web control</param>
        /// <param name="type">Type of database</param>
        private void AddExposedDatabases(Dictionary<string, List<DbConfig>> dictDb, System.Web.UI.WebControls.Table tbl, string type)
        {
            char[] separator = { SEPARATOR };
            string[] dbInfo;
            string dbRootDir = Server.MapPath(PXWeb.Settings.Current.General.Paths.PxDatabasesPath);

            foreach (TableRow row in tbl.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    if (cell.Controls.Count == 1)
                    {
                        if (cell.Controls[0] is CheckBox)
                        {
                            CheckBox chk = (CheckBox)cell.Controls[0];

                            if (chk.Checked)
                            {
                                dbInfo = chk.ID.Split(separator, StringSplitOptions.RemoveEmptyEntries); // ID format is language_database

                                if (dbInfo.Length != 2)
                                {
                                    continue;
                                }

                                if (dbInfo[0] == null || dbInfo[1] == null)
                                {
                                    continue;
                                }

                                if (!dictDb.ContainsKey(dbInfo[0]))
                                {
                                    // Add dictionary key for the current language
                                    dictDb.Add(dbInfo[0], new List<DbConfig>());
                                }

                                DbConfig database = new DbConfig();
                                database.Name = dbInfo[1];
                                if (type == "PX")
                                {
                                    database.RootPath = System.IO.Path.Combine(dbRootDir, dbInfo[1]);
                                }
                                database.Type = type;

                                dictDb[dbInfo[0]].Add(database); // Add db to the list of db:s for this language
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validate the route prefix setting
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateRoutePrefix(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }
            else
            {
                string errorKey = "";

                if (InputValidation.ValidateNoIllegalCharcters(source, args, out errorKey) == false)
                {
                    SetValidationError(val, args, errorKey);
                    return;
                }
            }

            args.IsValid = true;
        }

        /// <summary>
        /// Validate the url root setting
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateUrlRoot(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = false;
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateNoIllegalCharcters(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }

            args.IsValid = true;
        }

        /// <summary>
        /// Validates that value is an integer value
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        public void ValidateMandatoryInteger(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            string errorKey = "";

            if (InputValidation.ValidateMandatoryPositiveInteger(source, args, out errorKey) == false)
            {
                SetValidationError(val, args, errorKey);
                return;
            }
        }

        /// <summary>
        /// Set error message for validator with invalid value
        /// </summary>
        /// <param name="val">Validator object</param>
        /// <param name="args">Validator arguments</param>
        /// <param name="errorKey">Key for error message</param>
        /// <param name="parameters">Eventual parameters to the localized string</param>
        private void SetValidationError(CustomValidator val, System.Web.UI.WebControls.ServerValidateEventArgs args, string errorKey, params string[] parameters)
        {
            args.IsValid = false;
            if (parameters.Length > 0)
            {
                val.ErrorMessage = string.Format(Master.GetLocalizedString(errorKey), parameters);
            }
            else
            {
                val.ErrorMessage = Master.GetLocalizedString(errorKey);
            }
        }

        /// <summary>
        /// Is called when the value "Show API query button" changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboShowSaveApiQueryButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHideTextboxForQueryPrefix();
        }

        /// <summary>
        /// If the setting showApiQueryButton is true then show the control
        /// for setting text
        /// </summary>
        private void ShowHideTextboxForQueryPrefix()
        {
            if (bool.Parse(cboShowSaveApiQueryButton.SelectedValue))
            {
                pnlSaveApiQueryText.Visible = true;
            }
            else
            {
                pnlSaveApiQueryText.Visible = false;
            }
        }

        protected void PxDatabasesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralApiPxDatabases", "PxWebAdminFeaturesApiGeneralApiPxDatabasesInfo");
        }
        protected void CnmmDatabasesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralApiCnmmDatabases", "PxWebAdminFeaturesApiGeneralApiCnmmDatabasesInfo");
        }
        protected void RoutePrefixInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralRoutePrefix", "PxWebAdminFeaturesApiGeneralRoutePrefixInfo");
        }
        protected void MaxValuesReturnedInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralMaxValuesReturned", "PxWebAdminFeaturesApiGeneralMaxValuesReturnedInfo");
        }
        protected void LimiterRequestsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralLimiterRequests", "PxWebAdminFeaturesApiGeneralLimiterRequestsInfo");
        }
        protected void LimiterTimespanInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralLimiterTimespan", "PxWebAdminFeaturesApiGeneralLimiterTimespanInfo");
        }
        protected void EnableCORSInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralEnableCORS", "PxWebAdminFeaturesApiGeneralEnableCORSInfo");
        }
        protected void EnableCacheInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralEnableCache", "PxWebAdminFeaturesApiGeneralEnableCacheInfo");
        }

        protected void ShowQueryInformation(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralShowQueryInformation", "PxWebAdminFeaturesApiGeneralShowQueryInformationInfo");
        }

        protected void FetchCellLimitInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralFetchCellLimit", "PxWebAdminFeaturesApiGeneralFetchCellLimitInformationInfo");
        }
        protected void DefaultExampleResponseFormatInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralDefaultExampleResponseFormat", "PxWebAdminFeaturesApiGeneralDefaultExampleResponseFormatInfo");
        }

        protected void UrlRootInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralUrlRoot", "PxWebAdminFeaturesApiGeneralUrlRootInfo");
        }

        protected void UrlRootInfoV2(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralUrlRootV2", "PxWebAdminFeaturesApiGeneralUrlRootInfoV2");
        }
        protected void EnableApiV2QueryLinkInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralEnableApiV2QueryLink", "PxWebAdminFeaturesApiGeneralEnableApiV2QueryLinkInfo");
        }
        protected void ShowSaveApiQueryButtonInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralShowSaveApiQueryButton", "PxWebAdminFeaturesApiGeneralShowSaveApiQueryButtonInfo");
        }

        protected void SaveApiQueryTextInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesApiGeneralSaveApiQueryText", "PxWebAdminFeaturesApiGeneralSaveApiQueryTextInfo");
        }

    }
}
