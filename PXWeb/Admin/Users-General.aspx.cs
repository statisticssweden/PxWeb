using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCAxis.Menu.Extensions;
using System.Configuration;
using System.Web.Configuration;

namespace PXWeb.Admin
{
    public partial class Users_General : System.Web.UI.Page
    {
        private readonly string Admin = "admin";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblOutput.Visible = false;

            if (!IsPostBack)
            {
                HideObjects();
                FillGrid();
            }
            else
            {
                CreateEvents();               
            }                    
        }                      

        /// <summary>
        /// When default user authentication is used hide possibilty to change and add users
        /// </summary>
        private void HideObjects()
        {
            if (Membership.Provider.Name.Equals("PXWebDefaultMembershipProvider"))
            {
                grvUsers.Columns[2].Visible = false;
                grvUsers.Columns[3].Visible = false;
                grvUsers.Columns[4].Visible = false;
                grvUsers.Columns[5].Visible = false;
                grvUsers.Columns[6].Visible = false;
                grvUsers.Columns[7].Visible = false;
                grvUsers.Columns[8].Visible = false;

                plcNewUser.Visible = false;
            }
            else
            {
                plcNewPassword.Visible = false;
            }
        }

        protected void FillGrid()
        {          
            CreateEvents();
            
            try
            {               
                grvUsers.DataSource = Membership.GetAllUsers();
                grvUsers.DataBind();
            }
            catch (Exception e)
            {
                plcNewUser.Visible = false;
                lblOutput.Visible = true;
                lblOutput.Text = e.Message;
            }            
        }

        protected void CreateEvents()
        {            
            grvUsers.RowDeleting += DeleteUser;
            grvUsers.RowEditing += EditUser;
            grvUsers.RowCancelingEdit += CancelingEdit;
            grvUsers.RowUpdating += RowUpdating;
            grvUsers.PageIndexChanging += PageIndexChanging;
        }

        protected bool UserIslockedOut(MembershipUser user)
        {
            return user.IsLockedOut;
        }       
        
        protected bool HasAdminAccess(string user)
        {
            if (Membership.Provider.Name.Equals("PXWebDefaultMembershipProvider"))
            {
                return true;
            }

            return Roles.IsUserInRole(user, Admin);
        }

        protected string LicenseExpires(string user)
        {
            if (HasAdminAccess(user))
            {
                return Master.GetLocalizedString("PxWebAdminUsersNeverExpires");
            }
            var profile = ProfileBase.Create(user);
           
            var license = ((DateTime) profile.GetPropertyValue("License"));
           
            return license == DateTime.MinValue ? Master.GetLocalizedString("PxWebAdminUsersHasExpired") : license.AddYears(1).ToShortDateString();
        }

        protected string GetLicenseNumber(string user)
        {
            if (Membership.Provider.Name.Equals("PXWebDefaultMembershipProvider"))
            {
                return "0";
            }

            var profile = ProfileBase.Create(user);
            
            var licenseNumber = (int)profile.GetPropertyValue("LicenseNumber");
            return licenseNumber.ToString();
        }

        private void EditUser(object sender, GridViewEditEventArgs e)
        {
            grvUsers.EditIndex = e.NewEditIndex;
            FillGrid();
        }

        private void DeleteUser(object sender, GridViewDeleteEventArgs e)
        {            
            Membership.DeleteUser(e.Values[0].ToString(), true);           
            FillGrid();
        }

        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            if (!val1.IsValid || !val2.IsValid || !val3.IsValid || !val4.IsValid || !val5.IsValid) return;
            
            try
            {
                var user = Membership.CreateUser(tbNewName.Text, tbNewPassword.Text, tbNewMail.Text);
                var profile = ProfileBase.Create(user.UserName);
                profile.SetPropertyValue("License", DateTime.Now);
                if (!string.IsNullOrEmpty(tbNewLicenseNumber.Text))
                {
                    profile.SetPropertyValue("LicenseNumber", int.Parse(tbNewLicenseNumber.Text));
                }               
                profile.Save();

                if (cbNewAdmin.Checked)
                {
                    Roles.AddUserToRole(user.UserName, Admin);
                }

                lblOutput.Text = string.Format("{0} {1}", user.UserName, Master.GetLocalizedString("PxWebAdminUsersAddedUser"));
                lblOutput.Visible = true;
                tbNewName.Text = string.Empty;
                tbNewMail.Text = string.Empty;
                tbNewPassword.Text = string.Empty;
                cbNewAdmin.Checked = false;
                tbNewLicenseNumber.Text = string.Empty;

                FillGrid();              
            }
            catch (Exception ex)
            {
                lblOutput.Text = ex.Message;
                lblOutput.Visible = true;
            }        
        }           

        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvUsers.PageIndex = e.NewPageIndex;
            FillGrid();
        }

        private void CancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvUsers.EditIndex = -1;
            FillGrid();
        }

        protected void RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var user = Membership.GetUser(((Label)grvUsers.Rows[e.RowIndex].FindControl("lblName")).Text);
                if (user == null) return;

                var profile = ProfileBase.Create(user.UserName);
                if (((CheckBox)grvUsers.Rows[e.RowIndex].FindControl("cbUpdateLicense")).Checked)
                {
                    profile.SetPropertyValue("License", DateTime.Now);
                }
                profile.SetPropertyValue("LicenseNumber", int.Parse(((TextBox)grvUsers.Rows[e.RowIndex].FindControl("tbLicenseNumber")).Text));
                profile.Save();

                user.Email = ((TextBox)grvUsers.Rows[e.RowIndex].FindControl("tbMail")).Text;
                var wantsAdmin = ((CheckBox)grvUsers.Rows[e.RowIndex].FindControl("cbAdmin")).Checked;
                var newPassword = ((TextBox)grvUsers.Rows[e.RowIndex].FindControl("tbPassword")).Text;

                if (!string.IsNullOrEmpty(newPassword))
                {
                    user.ChangePassword(user.ResetPassword(), newPassword);
                }

                if (wantsAdmin && !HasAdminAccess(user.UserName))
                {
                    Roles.AddUserToRole(user.UserName, Admin);
                }
                else if (!wantsAdmin && HasAdminAccess(user.UserName))
                {
                    Roles.RemoveUserFromRole(user.UserName, Admin);
                }

                if (user.IsLockedOut && !((CheckBox)grvUsers.Rows[e.RowIndex].FindControl("cbLocked")).Checked)
                {
                    user.UnlockUser();
                }

                Membership.UpdateUser(user);

                grvUsers.EditIndex = -1;
                FillGrid();
            }
            catch (Exception ex)
            {
                lblOutput.Text = ex.Message;
                lblOutput.Visible = true;              
            }       
        }


        #region "Change password"

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

        #endregion

    }
}
