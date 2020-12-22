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
using PCAxis.Web.Controls;
using PCAxisPlugins = PCAxis.Web.Controls.CommandBar.Plugin;

namespace PXWeb
{
    public partial class InformationPresentation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;
            //((PxWeb)this.Master).FooterText = "Information";

            if (!IsPostBack)
            {
                Master.HeadTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxWebTitleInformationPresentation");
                if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
                {
                    InitializeCommandBar();
                }
                InitializeTableInformation();
                InitializeInformation();
            }
        }

        /// <summary>
        /// Initializes the TableInformation web control
        /// </summary>
        private void InitializeTableInformation()
        {
            TableInformationInformationView.ShowSourceDescription = PXWeb.Settings.Current.General.Global.ShowSourceDescription;
        }

        /// <summary>
        /// Initializes the Information web control
        /// </summary>
        private void InitializeInformation()
        {
            InformationView.ShowInformationTypes = PXWeb.Settings.Current.General.Global.ShowInformationTypes.GetSelectedInformationTypes();
        }
        /// <summary>
        /// Initializes CommandBar
        /// </summary>
        private void InitializeCommandBar()
        {
            Master.SetCommandBarFilter(PCAxisPlugins.CommandBarFilterFactory.GetFilter(PCAxisPlugins.CommandBarPluginFilterType.Information.ToString()));
            Master.SetCommandBarPresentationView(Plugins.Views.INFORMATION);
        }


    }
}
