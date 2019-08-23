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

namespace PXWeb
{
    public partial class MarkingTips : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.SetBreadcrumb(PCAxis.Web.Controls.Breadcrumb.BreadcrumbMode.SelectionSubPage, Master.GetLocalizedString("PxWebMarkingTips"));
                lblHeader.Text = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebMarkingTipsHeader");
                litTips.Text = "<p style='white-space:pre'>" + PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebMarkingTipsText") + "</p>" ;
            }
        }
    }
}
