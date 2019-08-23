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

        }

        protected void imgPromptNotesInfo_Click(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationGeneralPromptNotes", "PxWebAdminSettingsPresentationGeneralPromptNotesInfo");
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
