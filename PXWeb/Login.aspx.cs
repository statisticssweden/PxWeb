using System.Security.Authentication;
using PCAxis.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PXWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Title
            litTitle.Text = Server.HtmlEncode(GetLocalizedString("PxWebApplicationTitle"));
            if (string.IsNullOrEmpty(litTitle.Text))
            {
                litTitle.Text = "PX-Web";
            }
            //Logo
            
            imgSiteLogo.Src = Path.Combine(
                PXWeb.Settings.Current.General.Paths.ImagesPath, 
                PXWeb.Settings.Current.General.Site.LogoPath);

            imgSiteLogo.Alt = GetLocalizedString("PxWebLogoAlt");

            //Application name
            litAppName.Text = Server.HtmlEncode(GetLocalizedString("PxWebApplicationName"));

            LoginControl.FailureText = Server.HtmlEncode(GetLocalizedString("PxWebAdminUsersLoginFailure"));       
        }

        protected void LogIn(object sender, AuthenticateEventArgs e)
        {                                      
            if (Membership.ValidateUser(LoginControl.UserName, LoginControl.Password))
            {
                FormsAuthentication.RedirectFromLoginPage(LoginControl.UserName, false);
            }
            else
            {             
                e.Authenticated = false;
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
