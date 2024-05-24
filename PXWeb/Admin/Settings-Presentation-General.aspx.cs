using System;
using System.Web.UI;

namespace PXWeb.Admin
{
    public partial class Settings_Presentation_General : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Settings.Current.Presentation.PromptMandatoryFootnotes)
            {
                cboPromptNotes.SelectedIndex = 0;
            }
            else
            {
                cboPromptNotes.SelectedIndex = 1;
            }


            cboNewTitleLayout.SelectedValue = Settings.Current.Presentation.NewTitleLayout.ToString();

        }

        protected void imgPromptNotesInfo_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationGeneralPromptNotes", "PxWebAdminSettingsPresentationGeneralPromptNotesInfo");
        }

        protected void NewTitleLayoutInfo_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationGeneralLayoutVisible", "PxWebAdminSettingsPresentationGeneralNewTitleLayoutInfo");
        }

        /// <summary>
        /// Save Menu settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {

            if (PXWeb.Settings.BeginUpdate())
            {
                try
                {
                    PXWeb.PresentationSettings pres = (PXWeb.PresentationSettings)PXWeb.Settings.NewSettings.Presentation;
                    pres.PromptMandatoryFootnotes = cboPromptNotes.SelectedIndex == 0;

                    pres.NewTitleLayout = bool.Parse(cboNewTitleLayout.SelectedValue);
                    PXWeb.Settings.Save();
                }
                finally
                {
                    PXWeb.Settings.EndUpdate();
                }
            }
        }
    }
}
