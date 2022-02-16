using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PXWeb.CustomControls
{
    public class UserManualScreenReader : System.Web.UI.WebControls.Literal
    {
        public string manualFor
        {
            get;
            set;
        }

        public string headerCode
        {
            get;
            set;
        }
        public string textCode
        {
            get;
            set;
        }

        public UserManualScreenReader() : base()
        {
            this.Text = "<span>Hello</span>";
            Load += Page_Load;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Localize();
        }
        private void Localize()
        {
            string headerKey = "";
            string textKey = "";
            if (string.IsNullOrEmpty(manualFor)) {
                headerKey = headerCode; 
                textKey = textCode; 
            }
            else
            {
                headerKey = String.Format("UserManualScreenReader_{0}_Header", manualFor);
                textKey = String.Format("UserManualScreenReader_{0}_Text", manualFor);
            }

            string heading = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(headerKey);
            string longText = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(textKey);

            StringBuilder builder = new StringBuilder();
            builder.Append(@"<section aria-label=""");
            builder.Append(this.Page.Server.HtmlEncode(heading));
            builder.Append(@"""><span class=""screenreader-only"">");
            builder.Append(this.Page.Server.HtmlEncode(longText));
            builder.Append("</span></section>");
            
            this.Text = builder.ToString();
        }

    }
}