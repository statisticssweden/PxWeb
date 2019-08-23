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
using System.Web.Configuration;

namespace PXWeb.Admin
{
    public partial class Tools_ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Validate the old password
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateOldPassword(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }

            Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            AuthenticationSection authenticationSection = (AuthenticationSection)config.GetSection("system.web/authentication");

            //Assumes only one user
            if (!FormsAuthentication.Authenticate(authenticationSection.Forms.Credentials.Users[0].Name, txtOldPassword.Text))
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminToolsChangePasswordWrongPassword");
                return;
            }

            args.IsValid = true;
        }

        /// <summary>
        /// Validate the new password
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateNewPassword(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }

            args.IsValid = true;
        }

        /// <summary>
        /// Validate the verify new password textbox
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateVerifyPassword(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }

            if (!txtNewPassword.Text.Equals(txtVerifyPassword.Text))
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminToolsChangePasswordVerifyIncorrect");
                return;
            }

            args.IsValid = true;
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                AuthenticationSection authenticationSection = (AuthenticationSection)config.GetSection("system.web/authentication");

                if (authenticationSection.Forms.Credentials.Users.Count == 1)
                {
                    authenticationSection.Forms.Credentials.Users[0].Password = PXWeb.Misc.Encryption.sha1(txtNewPassword.Text);
                    authenticationSection.Forms.Credentials.PasswordFormat = FormsAuthPasswordFormat.SHA1;
                    config.Save();

                    Master.ShowInfoDialog("PxWebAdminMenuToolsChangePassword", "PxWebAdminToolsChangePasswordSuccess");
                }
            }
        }
    }
}
