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
using PCAxisPlugins = PCAxis.Web.Controls.CommandBar.Plugin;

namespace PXWeb
{
    public partial class InformationSelection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;
            ((PxWeb)this.Master).FooterText = "Information";

            if (!IsPostBack)
            {
                Master.HeadTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebInformationSelection");
                Master.SetBreadcrumb(PCAxis.Web.Controls.Breadcrumb.BreadcrumbMode.SelectionSubPage, Master.GetLocalizedString("PxWebInformation"));
                InitializeTableInformation();
                InitializeInformation();
            }
        }

        /// <summary>
        /// Initializes the TableInformation web control
        /// </summary>
        private void InitializeTableInformation()
        {
            TableInformationInformationSelect.ShowSourceDescription = PXWeb.Settings.Current.General.Global.ShowSourceDescription;
        }

        /// <summary>
        /// Initializes the Information web control
        /// </summary>
        private void InitializeInformation()
        {
            InformationSelect.ShowInformationTypes = PXWeb.Settings.Current.General.Global.ShowInformationTypes.GetSelectedInformationTypes();
        }

    }
}
