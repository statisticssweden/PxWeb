using PCAxis.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PXWeb.UserControls
{
    public partial class MenuExplanation : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            explanationText.Text = Server.HtmlEncode(GetLocalizedString("PxWebMenuExplanation"));
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