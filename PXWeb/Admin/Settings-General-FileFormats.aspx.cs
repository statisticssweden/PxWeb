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
    public partial class Settings_General_FileFormats : System.Web.UI.Page
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
        /// Read and display General.FileFormats settings  
        /// </summary>
        private void ReadSettings()
        {
            txtCellLimitDownload.Text = PXWeb.Settings.Current.General.FileFormats.CellLimitDownloads.ToString();
            cboExcelInformationLevel.SelectedValue = PXWeb.Settings.Current.General.FileFormats.Excel.InformationLevel.ToString();
            cboExcelDoubleColumn.SelectedValue = PXWeb.Settings.Current.General.FileFormats.Excel.DoubleColumn.ToString();
        }

        /// <summary>
        /// Save General.Global settings
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
                        PXWeb.FileFormatsSettings fileFormats = (PXWeb.FileFormatsSettings)PXWeb.Settings.NewSettings.General.FileFormats;
                        PXWeb.ExcelSettings excel = (PXWeb.ExcelSettings)PXWeb.Settings.NewSettings.General.FileFormats.Excel;

                        fileFormats.CellLimitDownloads = int.Parse(txtCellLimitDownload.Text);
                        switch (cboExcelInformationLevel.SelectedValue)
                        {
                            case "None":
                                excel.InformationLevel = PCAxis.Paxiom.InformationLevelType.None;
                                break;
                            case "MandantoryFootnotesOnly":
                                excel.InformationLevel = PCAxis.Paxiom.InformationLevelType.MandantoryFootnotesOnly;
                                break;
                            case "AllFootnotes":
                                excel.InformationLevel = PCAxis.Paxiom.InformationLevelType.AllFootnotes;
                                break;
                            default:
                                excel.InformationLevel = PCAxis.Paxiom.InformationLevelType.AllInformation;
                                break;
                        }

                        excel.DoubleColumn = bool.Parse(cboExcelDoubleColumn.SelectedValue);

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
        /// Validate Cell limit download
        /// </summary>
        /// <param name="source">Validator object</param>
        /// <param name="args">Validator arguments</param>
        public void ValidateCellLimitDownload(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;
            long lvalue;

            if (args.Value.Length == 0)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationMandatorySetting");
                return;
            }

            if (!long.TryParse(args.Value, out lvalue))
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationIntegerValue");
                return;
            }

            if (lvalue > Int32.MaxValue)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationTooLargeIntegerValue");
                return;
            }

            if (lvalue < 1)
            {
                args.IsValid = false;
                val.ErrorMessage = Master.GetLocalizedString("PxWebAdminSettingsValidationGreaterThanZero");
                return;
            }

            args.IsValid = true;
            return;
        }

        protected void CellLimitDownloadInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsFileFormatsCellLimitDownload", "PxWebAdminSettingsFileFormatsCellLimitDownloadInfo");
        }
        protected void ExcelInformationLevelInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsFileFormatsExcelInformationLevel", "PxWebAdminSettingsFileFormatsExcelInformationLevelInfo");
        }
        protected void ExcelDoubleColumnInfo(object sender, ImageClickEventArgs e)
        {
            Master.ShowInfoDialog("PxWebAdminSettingsFileFormatsExcelDoubleColumn", "PxWebAdminSettingsFileFormatsExcelDoubleColumnInfo");
        }
    }
}
