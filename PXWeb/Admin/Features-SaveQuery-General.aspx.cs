using PCAxis.Query;
using PXWeb.Misc;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PXWeb.Admin
{
    public partial class Features_SaveQuery_General : System.Web.UI.Page
    {
        private const char SEPARATOR = '|';

        protected void Page_init(object sender, EventArgs e)
        {
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
        /// Read and display Save Query settings  
        /// </summary>
        private void ReadSettings()
        {
            cboStorageType.SelectedValue = PXWeb.Settings.Current.Features.SavedQuery.StorageType.ToString();
            cboEnableCache.SelectedValue = PXWeb.Settings.Current.Features.SavedQuery.EnableCache.ToString();
            txtCacheTime.Text = PXWeb.Settings.Current.Features.SavedQuery.CacheTime.ToString();
            cboEnableCORS.SelectedValue = PXWeb.Settings.Current.Features.SavedQuery.EnableCORS.ToString();
            cboShowPeriodAndId.SelectedValue = PXWeb.Settings.Current.Features.SavedQuery.ShowPeriodAndId.ToString();
            cboEnableLimitRequest.SelectedValue = PXWeb.Settings.Current.Features.SavedQuery.EnableLimitRequest.ToString();
            txtLimiterRequestsSq.Text = PXWeb.Settings.Current.Features.SavedQuery.LimiterRequests.ToString();
            txtLimiterTimespanSq.Text = PXWeb.Settings.Current.Features.SavedQuery.LimiterTimespan.ToString();
        }

        /// <summary>
        /// Save Save Query settings
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
                        PXWeb.SavedQuerySettings savedQuery = (PXWeb.SavedQuerySettings)PXWeb.Settings.NewSettings.Features.SavedQuery;

                        savedQuery.StorageType = (SavedQueryStorageType)Enum.Parse(typeof(SavedQueryStorageType), cboStorageType.SelectedValue);
                        savedQuery.EnableCache = bool.Parse(cboEnableCache.SelectedValue);
                        savedQuery.CacheTime = int.Parse(txtCacheTime.Text.Trim());
                        savedQuery.EnableCORS = bool.Parse(cboEnableCORS.SelectedValue);
                        savedQuery.ShowPeriodAndId = bool.Parse(cboShowPeriodAndId.SelectedValue);
                        savedQuery.EnableLimitRequest = bool.Parse(cboEnableLimitRequest.SelectedValue);
                        savedQuery.LimiterRequests = int.Parse(txtLimiterRequestsSq.Text.Trim());
                        savedQuery.LimiterTimespan = int.Parse(txtLimiterTimespanSq.Text.Trim());
                        PXWeb.Settings.Save();
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();

                        PCAxis.Query.SavedQueryManager.StorageType = PXWeb.Settings.Current.Features.SavedQuery.StorageType;

                        PCAxis.Query.SavedQueryManager.Reset();
                        PXWeb.Management.SavedQueryPaxiomCache.Current.Reset();
                    }
                }
            }
        }


        //Text to information icons
        protected void StorageTypeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsSavedQueryStorageType", "PxWebAdminSettingsSavedQueryStorageTypeInfo");
        }
        protected void EnableCacheInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSavedQueryGeneralEnableCache", "PxWebAdminFeaturesSavedQueryGeneralEnableCacheInfo");
        }
        protected void CacheTimeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSavedQueryGeneralCacheTime", "PxWebAdminFeaturesSavedQueryGeneralCacheTimeInfo");
        }
        protected void EnableCORSInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSavedQueryGeneralEnableCORS", "PxWebAdminFeaturesSavedQueryGeneralEnableCORSInfo");
        }
        protected void EnableShowPeriodAndIdInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSavedQueryGeneralShowPeriodAndId", "PxWebAdminFeaturesSavedQueryGeneralShowPeriodAndIdInfo");
        }
        protected void EnableLimitRequestInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSavedQueryGeneralEnableLimitRequest", "PxWebAdminFeaturesSavedQueryGeneralEnableLimitRequestInfo");
        }
        protected void LimiterRequestsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSavedQueryGeneralLimiterRequests", "PxWebAdminFeaturesSavedQueryGeneralLimiterRequestsInfo");
        }
        protected void LimiterTimespanInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSavedQueryGeneralLimiterTimespan", "PxWebAdminFeaturesSavedQueryGeneralLimiterTimespanInfo");
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


    }



}