using System;
using System.Collections;
using System.Collections.Generic;
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
    public partial class FootnotesPresentation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PCAxis.Web.Core.Management.PaxiomManager.PaxiomModelBuilder = null;
            //((PxWeb)this.Master).FooterText = "Footnote";


            if (!IsPostBack)
            {
                Master.HeadTitle = PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString("PxFootnotesPresentation");
                if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel != null)
                {
                    InitializeCommandBar();
                }
                InitializeTableInformation();
            }
        }

        /// <summary>
        /// Initializes the TableInformation web control
        /// </summary>
        private void InitializeTableInformation()
        {
            TableInformationFootnoteView.ShowSourceDescription = PXWeb.Settings.Current.General.Global.ShowSourceDescription;
        }


        /// <summary>
        /// Initializes CommandBar
        /// </summary>
        private void InitializeCommandBar()
        {
           Master.SetCommandBarFilter(PCAxisPlugins.CommandBarFilterFactory.GetFilter(PCAxisPlugins.CommandBarPluginFilterType.Footnoote.ToString()));
           Master.SetCommandBarPresentationView(Plugins.Views.FOOTNOTE);
        }

        
    }
}
