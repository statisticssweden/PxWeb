using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PXWeb.Misc;

namespace PXWeb.Admin
{
    public partial class Features_BackgroundWorker_General : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                lnkStop.Attributes.Add("onclick", "return confirm('" + Master.GetLocalizedString("PxWebAdminFeaturesBackgroundWorkerConfirmStop") + "');");
                lnkStart.Attributes.Add("onclick", "return confirm('" + Master.GetLocalizedString("PxWebAdminFeaturesBackgroundWorkerConfirmStart") + "');");
                lnkWakeUp.Attributes.Add("onclick", "return confirm('" + Master.GetLocalizedString("PxWebAdminFeaturesBackgroundWorkerConfirmWakeUp") + "');");
                
                ReadSettings();
            }
        }

        /// <summary>
        /// Read and display API settings  
        /// </summary>
        private void ReadSettings()
        {
            txtSleepTime.Text = PXWeb.Settings.Current.Features.BackgroundWorker.SleepTime.ToString();
        }

        /// <summary>
        /// Updates status periodically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateStatus(object sender, EventArgs e)
        {
            lnkWakeUp.Visible = false;
            lnkStop.Visible = false;
            lnkStart.Visible = false;

            lblCurrentActivityValue.Text = BackgroundWorker.PxWebBackgroundWorker.CurrentActivity;

            if (BackgroundWorker.PxWebBackgroundWorker.CurrentActivity.Equals("Sleeping"))
            {
                lnkWakeUp.Visible = true;
            }

            lblStatusValue.Text = BackgroundWorker.PxWebBackgroundWorker.Status.ToString();

            if (BackgroundWorker.PxWebBackgroundWorker.Status == BackgroundWorker.StatusType.Running)
            {
                lnkStop.Visible = true;
            }
            else if (BackgroundWorker.PxWebBackgroundWorker.Status == BackgroundWorker.StatusType.Stopped)
            {
                lnkStart.Visible = true;
            }
        }

        /// <summary>
        /// Save Background worker settings
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
                        PXWeb.BackgroundWorkerSettings backgroundWorker = (PXWeb.BackgroundWorkerSettings)PXWeb.Settings.NewSettings.Features.BackgroundWorker;

                        backgroundWorker.SleepTime = int.Parse(txtSleepTime.Text);

                        PXWeb.Settings.Save();

                        //Update settings in the background worker object
                        BackgroundWorker.PxWebBackgroundWorker.SleepTime = backgroundWorker.SleepTime;
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// Wake up background worker process if it is sleeping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WakeUp(object sender, EventArgs e)
        {
            BackgroundWorker.PxWebBackgroundWorker.WakeUp();
        }

        /// <summary>
        /// Stop background worker process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Stop(object sender, EventArgs e)
        {
            BackgroundWorker.PxWebBackgroundWorker.Stop();
        }
        
        /// <summary>
        /// Restart background worker process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Restart(object sender, EventArgs e)
        {
            BackgroundWorker.PxWebBackgroundWorker.Restart();
        }

        /// <summary>
        /// Validates that value is an integer value
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validation arguments</param>
        protected void ValidateMandatoryInteger(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
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

        protected void SleepTimeInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesBackgroundWorkerGeneralSleepTime", "PxWebAdminFeaturesBackgroundWorkerGeneralSleepTimeInfo");
        }
        protected void StatusInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesBackgroundWorkerGeneralStatus", "PxWebAdminFeaturesBackgroundWorkerGeneralStatusInfo");
        }
        protected void CurrentActivityInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminFeaturesBackgroundWorkerGeneralCurrentActivity", "PxWebAdminFeaturesBackgroundWorkerGeneralCurrentActivityInfo");
        }



    }
}