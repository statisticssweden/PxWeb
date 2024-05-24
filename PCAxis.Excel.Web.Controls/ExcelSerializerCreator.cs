using PCAxis.Paxiom;

namespace PCAxis.Excel.Web.Controls
{
    public class ExcelSerializerCreator : PCAxis.Web.Core.ISerializerCreator
    {

        public Paxiom.IPXModelStreamSerializer Create(string fileInfo)
        {
            PCAxis.Serializers.XlsxSerializer ser;
            ser = new PCAxis.Serializers.XlsxSerializer();

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
