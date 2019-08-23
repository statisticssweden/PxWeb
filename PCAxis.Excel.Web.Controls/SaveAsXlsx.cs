using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Web.Core;
using PCAxis.Web.Core.Management;
using PCAxis.Web.Controls;
using PCAxis.Paxiom;


namespace PCAxis.Excel.Web.Controls
{
    public class SaveAsXlsx : FileTypeMarkerControlBase<SaveAsXlsxCodebehind, SaveAsXlsx>
    {
        public SaveAsXlsx()
        {

        }

        public override void SerializeAndStream()
        {
        //            Using stream As IO.MemoryStream = New IO.MemoryStream()
        //    Dim excelSerializer As New ExcelFileSerializer

        //    If Me.SelectedFormat.Equals(Plugins.FileFormats.XLS_DOUBLE_COLUMN) Then
        //        excelSerializer.DoubleColumn = DoubleColumnType.AlwaysDoubleColumns
        //    Else
        //        'Get doublecolumn from application setting
        //        excelSerializer.DoubleColumn = Settings.Files.DoubleColumnFile
        //    End If

        //    'Get information level from application setting
        //    excelSerializer.InformationLevel = Settings.Files.CompleteInfoFile
        //    'End If

        //    excelSerializer.Serialize(PaxiomManager.PaxiomModel, stream)
        //    StreamFile(stream, Me.SelectedFileType.MimeType, Me.SelectedFileType.FileExtension)
        //End Using

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                PCAxis.Excel.XlsxSerializer ser;
                ser = new XlsxSerializer();

                if (this.SelectedFormat.Equals("FileTypeExcelXDoubleColumn"))
                {
                    ser.DoubleColumn = DoubleColumnType.AlwaysDoubleColumns;
                }
                else
                {
                    //Get doublecolumn from application setting
                    ser.DoubleColumn = Settings.Files.DoubleColumnFile;
                }

                //    'Get information level from application setting
                ser.InformationLevel = Settings.Files.CompleteInfoFile;
                

                ser.Serialize(PaxiomManager.PaxiomModel, stream);
                StreamFile(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx");
            }
        }
    }
}
