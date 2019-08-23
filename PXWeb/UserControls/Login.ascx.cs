using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using PCAxis.Web.Core.Management;
using System.Globalization;
namespace PXWeb.UserControls
{
    public partial class Login : System.Web.UI.UserControl
    {
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(Login));
        protected void Page_Load(object sender, EventArgs e)
        {
           
            Button searchBtn = (Button)Page.Master.FindControl("form1$ContentPlaceHolderMain$pxSearch$pnlSearch$cmdSearch");
  
            if (!IsPostBack) 
            {
                SetFields();
            }
        }

        protected void SetFields()
        {
            Button showLoginSectionBtn = (Button)LoginControl.FindControl("ShowLoginSection");
            var loggedInSection = LoginControl.FindControl("LoggedInDiv");
            var logInSection = LoginControl.FindControl("LoginSection");
            var showLoginSectionHolder = (Label)LoginControl.FindControl("ShowLoginSectionHolder");
            Label loggedInAsLabel = (Label)LoginControl.FindControl("LoggedInAs");
            Label UserNameLabel = (Label)LoginControl.FindControl("lblUserName");
            Label PasswordLabel = (Label)LoginControl.FindControl("lblPassword");

            UserNameLabel.Text = Server.HtmlEncode(GetLocalizedString("PxWebUsername"));
            PasswordLabel.Text = Server.HtmlEncode(GetLocalizedString("PxWebPassword"));   
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                showLoginSectionBtn.Text = Server.HtmlEncode(GetLocalizedString("PxWebLogin"));
                loggedInSection.Visible = false;
                if (showLoginSectionHolder.Text == "show")
                {
                    logInSection.Visible = false;
                }
                else
                {
                    logInSection.Visible = true;
                    Button loginBtn = (Button)LoginControl.FindControl("Login");
                    loginBtn.Text = Server.HtmlEncode(GetLocalizedString("PxWebLogin"));
                }
            }
            else
            {
                showLoginSectionBtn.Text = Server.HtmlEncode(GetLocalizedString("PxWebLogout"));
                loggedInSection.Visible = true;
                logInSection.Visible = false;
                loggedInAsLabel.Text = Server.HtmlEncode(GetLocalizedString("PxWebLoggedIn")) + " " + HttpContext.Current.User.Identity.Name; 
            }
        }

        protected void LogIn(object sender, AuthenticateEventArgs e)
        {
            try
            {
               //Session.Clear();
                if (string.IsNullOrEmpty(LoginControl.UserName) || string.IsNullOrEmpty(LoginControl.Password))
                {
                    LoginControl.FailureText = Server.HtmlEncode(GetLocalizedString("PxWebLoginMissingUserOrPassw"));
                    return;

                }
                if (Membership.ValidateUser(LoginControl.UserName, LoginControl.Password))
                {

                    FormsAuthentication.RedirectFromLoginPage(LoginControl.UserName, false);
                    Button showLoginSectionBtn = (Button)LoginControl.FindControl("ShowLoginSection");
                    var showLoginSectionHolder = (Label)LoginControl.FindControl("ShowLoginSectionHolder");
                    Label loggedInAsLabel = (Label)LoginControl.FindControl("LoggedInAs");
                    loggedInAsLabel.Text = "Logget inn som " + LoginControl.UserName;
                    showLoginSectionHolder.Text = "show";

                    string currentUrl = Request.RawUrl;
                    Response.Redirect(currentUrl);
                }
                else
                {
                    LoginControl.FailureText = Server.HtmlEncode(GetLocalizedString("PxWebWrongLogin"));
                }

            }

            catch (Exception ex)
            {
                _logger.Error("Failed to Login. Username " + LoginControl.UserName + "Error:" + ex.Message );
                throw;
            }
        }
        protected void LoginExpand(object sender, EventArgs e)
        {
            var showLoginSectionHolder = (Label)LoginControl.FindControl("ShowLoginSectionHolder");
            var showLoginSectionBtn = (Button)sender;
            Button loginBtn = (Button)LoginControl.FindControl("Login");

            if(showLoginSectionBtn.Text == Server.HtmlEncode(GetLocalizedString("PxWebLogout")))
            {
                LogOut();
            }
            else
            {
                this.Page.Form.DefaultFocus = LoginControl.UserName;
                if (showLoginSectionHolder.Text == "show")
                {
                    showLoginSectionHolder.Text = "hide";
                }
                else
                {
                    showLoginSectionHolder.Text = "show";
                }
            }
            SetFields();
        }

        private IPxUrl _pxUrl = null;
        private IPxUrl PxUrlObj
        {
            get
            {
                if (_pxUrl == null)
                {
                    _pxUrl = RouteInstance.PxUrlProvider.Create(null);
                }

                return _pxUrl;
            }
        }

        protected void LogOut()
        {
            try
            {

                Button showLoginSectionBtn = (Button)LoginControl.FindControl("ShowLoginSection");
                FormsAuthentication.SignOut();
                Session.Clear();
               Response.Redirect("~/" + PxUrl.PX_START + "/" + PxUrlObj.Language + "/");
            }

            catch (Exception ex)
            {
                _logger.Error("Failed to Logout. Username " + LoginControl.UserName + "Error:" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Get text in the currently selected language
        /// </summary>
        /// <param name="key">Key identifying the string in the language file</param>
        /// <returns>Localized string</returns>
        public string GetLocalizedString(string key)
        {
            string lang = LocalizationManager.CurrentCulture.Name;
            return PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(key, new CultureInfo(lang));
        }
    }
}
