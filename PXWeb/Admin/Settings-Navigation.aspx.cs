using System;
using System.Web.UI;

namespace PXWeb.Admin
{
    public partial class Settings_Navigation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));

            if (!IsPostBack)
            {
                ReadSettings();
            }

        }
        /// <summary>
        /// Read and display navigation settings  
        /// </summary>
        private void ReadSettings()
        {
            cboShowNavigationFlow.SelectedValue = PXWeb.Settings.Current.Navigation.ShowNavigationFlow.ToString();
        }
        /// <summary>
        /// Save Navigation settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {

            if (PXWeb.Settings.BeginUpdate())
            {
                try
                {
                    PXWeb.NavigationSettings navigation = (PXWeb.NavigationSettings)PXWeb.Settings.NewSettings.Navigation;
                    navigation.ShowNavigationFlow = cboShowNavigationFlow.SelectedIndex == 0;
                    PXWeb.Settings.Save();
                }
                finally
                {
                    PXWeb.Settings.EndUpdate();
                }
            }
        }

        protected void imgNavigationFlowInfo_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsNavigationNavigationFlowVisible", "PxWebAdminSettingsNavigationNavigationFlowVisibleInfo");
        }
    }
}