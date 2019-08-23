using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PXWeb.Misc;

namespace PXWeb.Admin
{
    public partial class Features_Search_General : System.Web.UI.Page
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
        /// Read and display Search settings  
        /// </summary>
        private void ReadSettings()
        {
            txtCacheTime.Text = PXWeb.Settings.Current.Features.Search.CacheTime.ToString();
            txtResultListLength.Text = PXWeb.Settings.Current.Features.Search.ResultListLength.ToString();
            cboDefaultOperator.SelectedValue = PXWeb.Settings.Current.Features.Search.DefaultOperator.ToString();
        }

        /// <summary>
        /// Save Search settings
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
                        PXWeb.SearchSettings search = (PXWeb.SearchSettings)PXWeb.Settings.NewSettings.Features.Search;

                        search.CacheTime = int.Parse(txtCacheTime.Text);
                        search.ResultListLength = int.Parse(txtResultListLength.Text);

                        switch (cboDefaultOperator.SelectedValue)
                        {
                            case "AND":
                                search.DefaultOperator = PCAxis.Search.DefaultOperator.AND;
                                break;
                            case "OR":
                                search.DefaultOperator = PCAxis.Search.DefaultOperator.OR;
                                break;
                            default:
                                search.DefaultOperator = PCAxis.Search.DefaultOperator.OR;
                                break;
                        }

                        PCAxis.Search.SearchManager.Current.CacheTime = search.CacheTime;
                        PCAxis.Search.SearchManager.Current.SetDefaultOperator(search.DefaultOperator);

                        PXWeb.Settings.Save();
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                    }
                }
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
        protected void CacheTimeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSearchGeneralCacheTime", "PxWebAdminFeaturesSearchGeneralCacheTimeInfo");
        }
        protected void ResultListLengthInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSearchGeneralResultListLength", "PxWebAdminFeaturesSearchGeneralResultListLengthInfo");
        }
        protected void DefaultOperatorInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesSearchGeneralDefaultOperator", "PxWebAdminFeaturesSearchGeneralDefaultOperatorInfo");
        }

    }
}