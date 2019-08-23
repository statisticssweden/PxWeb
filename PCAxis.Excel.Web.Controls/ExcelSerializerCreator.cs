using PCAxis.Paxiom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Excel.Web.Controls
{
    public class ExcelSerializerCreator : PCAxis.Web.Core.ISerializerCreator
    {

        public Paxiom.IPXModelStreamSerializer Create(string fileInfo)
        {
            PCAxis.Excel.XlsxSerializer ser;
            ser = new XlsxSerializer();

            if (fileInfo.Equals("FileTypeExcelXDoubleColumn"))
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

            return ser;
        }
    }
}
