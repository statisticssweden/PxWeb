using PCAxis.Web.Core.Management;
using System;
using System.IO;
using System.Web.UI;

namespace PXWeb
{
    public partial class ErrorGeneral : System.Web.UI.Page
    {
        private System.Globalization.CultureInfo m_culture;

        protected void Page_Load(object sender, EventArgs e)
        {
            int statusCode = 500;

            //Logo
            imgSiteLogo.Src = Path.Combine(PXWeb.Settings.Current.General.Paths.ImagesPath, PXWeb.Settings.Current.General.Site.LogoPath);
            imgSiteLogo.Alt = "PX-Web";
            if (PXWeb.Settings.Current.General.Site.LogoPath.Length < 5)
            {
                imgSiteLogo.Visible = false;
            }

            SetLocalizedTexts();

            if (!string.IsNullOrWhiteSpace(Request.QueryString["errcode"]))
            {
                int.TryParse(Request.QueryString["errcode"], out statusCode);
            }

            if (statusCode.Equals(429))
            {
                lblErrorMessage.Text = "429 - Too many requests in too short timeframe. Please try again later.";
                lblSavedQueryErrorMessage.Text = "429 - Too many requests in too short timeframe. Please try again later.";
            }

            // Problem with a saved query
            if (!string.IsNullOrWhiteSpace(Request.QueryString["aspxerrorpath"]) && SavedQueryError())
            {
                lblErrorMessage.Visible = false;
                lblSavedQueryErrorMessage.Visible = true;
            }
            else
            {
                lblErrorMessage.Visible = true;
                lblSavedQueryErrorMessage.Visible = false;
            }

            //Check if the error is caused by invalid contents in querys string parameter value
            Exception exc = Server.GetLastError();
            if (exc != null)
            {
                if (exc is PCAxis.Web.Core.Exceptions.InvalidQuerystringParameterException)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text += "&nbsp;" + PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebIllegalQuerystringErrorMessage", m_culture);
                }
                else if (exc.InnerException != null)
                {
                    if (exc.InnerException is PCAxis.Web.Core.Exceptions.InvalidQuerystringParameterException)
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text += "&nbsp;" + PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebIllegalQuerystringErrorMessage", m_culture);
                    }
                }
            }

            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = statusCode;
        }

        /// <summary>
        /// Removes the ApplicationPath from the Request.QueryString and after that
        /// checks if the Request.QueryString starts with sq. If it dose the error 
        /// comes from a saved query.
        /// </summary>
        /// <returns>true if an saved query causing the error </returns>
        protected bool SavedQueryError()
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["aspxerrorpath"]) && !string.IsNullOrWhiteSpace(Request.ApplicationPath))
            {
                var errorUrl = Request.QueryString["aspxerrorpath"];
                var appPath = Request.ApplicationPath;
                int pathLenght = appPath.Length;

                //Removes application path from error url
                errorUrl = errorUrl.Remove(0, pathLenght);

                //Removes '/' from start
                if (errorUrl.StartsWith("/"))
                    errorUrl = errorUrl.Remove(0, 1);

                if (errorUrl.StartsWith("sq"))
                    return true;
                else
                {
                    return false;
                }

            }
            return false;
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

        private void SetLocalizedTexts()
        {
            string lang = null;

            try
            {
                lang = PxUrlObj.Language;
            }
            catch (Exception)
            {
            }

            if (string.IsNullOrWhiteSpace(lang))
            {
                lang = Settings.Current.General.Language.DefaultLanguage ?? "en";
            }

            m_culture = new System.Globalization.CultureInfo(lang);

            lblErrorMessage.Text = LocalizationManager.GetLocalizedString("PxWebGeneralErrorMessage", m_culture);
            lblSavedQueryErrorMessage.Text = LocalizationManager.GetLocalizedString("PxWebSavedQueryExecuteErrorMessage", m_culture);
            lnkStart.Text = LocalizationManager.GetLocalizedString("PxWebReturnToStartpage", m_culture);
            Page.Title = LocalizationManager.GetLocalizedString("PxWebTitleErrorGeneral", m_culture);
        }
    }
}
