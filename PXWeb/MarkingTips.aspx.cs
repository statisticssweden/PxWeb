using System;

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
                litTips.Text = "<p style='white-space:pre'>" + PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebMarkingTipsText") + "</p>";
            }
        }
    }
}
