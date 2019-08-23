using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Web.Core;
using PCAxis.Web.Controls.CommandBar.Plugin;
using System.Web.UI.WebControls;

namespace PCAxis.Excel.Web.Controls
{
    public class SaveAsXlsxCodebehind : FileTypeControlBase<SaveAsXlsxCodebehind, SaveAsXlsx> 
    {
        public SaveAsXlsxCodebehind()
        {
            this.Load += SaveAsChart_Load;
        }

 

        #region Controls

        protected Button ContinueButton;

        protected Button CancelButton;

        protected PlaceHolder ExcelFileFormatContainer;

        protected ListBox ExcelFileFormats;

        #endregion

 

        /// <summary>
        /// Called when the user control is loaded
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        /// <remarks></remarks>
        private void SaveAsChart_Load(object sender, System.EventArgs e)
        {
            if (Marker.ShowUI)
            {
                ContinueButton.Text = GetLocalizedString("CtrlSaveAsContinueButton");
                CancelButton.Text = GetLocalizedString("CtrlSaveAsCancelButton");

                this.CancelButton.Click += CancelButton_Click;
                this.ContinueButton.Click += Continue_Click;

                if (CommandBarPluginManager.FileTypes["xlsx"] != null)
                {
                    FileType fileType = CommandBarPluginManager.FileTypes["xlsx"];
                    ExcelFileFormats.Items.Clear();

                    foreach (KeyValuePair<String, String> kvp in fileType.FileFormats)
                    {
                        ListItem li = new ListItem();
                        li.Text = this.GetLocalizedString(kvp.Value);
                        li.Value = kvp.Key;
                        this.ExcelFileFormats.Items.Add(li);
                    }

                    if (ExcelFileFormats.Items.Count > 0)
                    {
                        ExcelFileFormats.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                SetSelectedFormat();
                OnFinished();
                Marker.SerializeAndStream();
            }
        }


        /// <summary>
        /// Handles Save button click. Creates the Excel file and streams it to the user.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        /// <remarks></remarks>
        private void Continue_Click(object sender, System.EventArgs e)
        {
            SetSelectedFormat();
            OnFinished();
            Marker.SerializeAndStream();
        }

       /// <summary>
       /// Set selected file format
       /// </summary>
        /// <remarks></remarks>
        private void SetSelectedFormat()
        {
            if (Marker.ShowUI)
            {
                if (ExcelFileFormats.SelectedIndex > -1)
                {
                    this.Marker.SelectedFormat = ExcelFileFormats.SelectedValue;
                }
                else
                {
                    //Set default
                    this.Marker.SelectedFormat = "FileTypeXlsx";   
                }
            }
        }

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            OnFinished();
        }
    }
}
