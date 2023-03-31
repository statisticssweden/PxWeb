using PCAxis.Serializers;

namespace PxWeb.Code.Api2.Serialization
{
    public class SerializeManager : ISerializeManager
    {
        public IDataSerializer GetSerializer(string outputFormat)
        {
            switch (outputFormat.ToLower())
            {
                case "xlsx":
                    return new XlsxDataSerializer();
                case "xlsx_doublecolumn":
                    return new XlsxDoubleColumnDataSerializer();
                case "csv":
                    return new CsvDataSerializer();
                case "csv_tab":
                    return new CsvTabDataSerializer();
                case "csv_tabhead":
                    return new CsvTabHeadDataSerializer();
                case "csv_comma":
                    return new CsvCommaDataSerializer();
                case "csv_commahead":
                    return new CsvCommaHeadDataSerializer();
                case "csv_space":
                    return new CsvSpaceDataSerializer();
                case "csv_spacehead":
                    return new CsvSpaceHeadDataSerializer();
                case "csv_semicolon":
                    return new CsvSemicolonDataSerializer();
                case "csv_semicolonhead":
                    return new CsvSemicolonHeadDataSerializer();
                case "csv2":
                    return new Csv2DataSerializer();
                case "csv3":
                    return new Csv3DataSerializer();
                case "json_stat":
                    return new JsonStatDataSerializer();
                case "json_stat2":
                    return new JsonStat2DataSerializer();
                case "html5_table":
                    return new Html5TableDataSerializer();
                case "relational_table":
                    return new RelationalTableDataSerializer();
                case "px":
                    return new PxDataSerializer();
                default:
                    return new PxDataSerializer();
            }
        }
    }
}
