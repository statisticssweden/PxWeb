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
    public partial class Settings_Presentation_Table : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EnableSave(new EventHandler(MasterSave_Click));
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            cboTableTransform.SelectedValue = Settings.Current.Presentation.Table.TableTransformation.ToString();
            cboDefaultLayout.SelectedValue = Settings.Current.Presentation.Table.DefaultLayout.ToString();
            txtMaxColumns.Text = Settings.Current.Presentation.Table.MaxColumns.ToString();
            txtMaxRows.Text = Settings.Current.Presentation.Table.MaxRows.ToString();
            cboTitleVisible.SelectedValue = Settings.Current.Presentation.Table.TitleVisible.ToString();
            cboDisplayAttributes.SelectedValue = Settings.Current.Presentation.Table.Attributes.DisplayAttributes.ToString();
            cboDisplayDefaultAttributes.SelectedValue = Settings.Current.Presentation.Table.Attributes.DisplayDefaultAttributes.ToString();
        }

        /// <summary>
        /// Save Menu settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MasterSave_Click(Object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                if (PXWeb.Settings.BeginUpdate())
                {
                    try
                    {
                        PXWeb.TableSettings tbl = (PXWeb.TableSettings)PXWeb.Settings.NewSettings.Presentation.Table;
                        PXWeb.AttributeSettings attr = (PXWeb.AttributeSettings)PXWeb.Settings.NewSettings.Presentation.Table.Attributes;

                        tbl.TableTransformation = (PCAxis.Web.Controls.TableTransformationType)Enum.Parse(typeof(PCAxis.Web.Controls.TableTransformationType), cboTableTransform.SelectedValue.ToString());
                        tbl.DefaultLayout = (PCAxis.Web.Controls.TableLayoutType)Enum.Parse(typeof(PCAxis.Web.Controls.TableLayoutType), cboDefaultLayout.SelectedValue.ToString());
                        tbl.MaxColumns = int.Parse(txtMaxColumns.Text);
                        tbl.MaxRows = int.Parse(txtMaxRows.Text);
                        tbl.TitleVisible = bool.Parse(cboTitleVisible.SelectedValue);

                        attr.DisplayAttributes = bool.Parse(cboDisplayAttributes.SelectedValue);
                        attr.DisplayDefaultAttributes = bool.Parse(cboDisplayDefaultAttributes.SelectedValue);

                        PXWeb.Settings.Save();
                    }
                    finally
                    {
                        PXWeb.Settings.EndUpdate();
                    }
                }
            }
        }

        /// <summary>
        /// Validates that a entered value is a integer
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateMandatoryInteger(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            int value;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }

            if (!int.TryParse(args.Value, out value))
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationIntegerValue");
                return;
            }

            if (value < 1)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationGreaterThanZero");
                return;
            }

            args.IsValid = true;
            return;
        }

        protected void TableTransformInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationTableTransform", "PxWebAdminSettingsPresentationTableTransformInfo");
        }

        protected void DefaultLayoutInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationDefaultLayout", "PxWebAdminSettingsPresentationDefaultLayoutInfo");
        }

        protected void MaxColumnsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationTableMaxColumns", "PxWebAdminSettingsPresentationTableMaxColumnsInfo");
        }
        protected void MaxRowsInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationTableMaxRows", "PxWebAdminSettingsPresentationTableMaxRowsInfo");
        }
        protected void TitleVisibleInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationTableTitleVisible", "PxWebAdminSettingsPresentationTableTitleVisibleInfo");
        }
        protected void DisplayAttributesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationTableDisplayAttributes", "PxWebAdminSettingsPresentationTableDisplayAttributesInfo");
        }
        protected void DisplayDefaultAttributesInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsPresentationTableDisplayDefaultAttributes", "PxWebAdminSettingsPresentationTableDisplayDefaultAttributesInfo");
        }

    }
}
