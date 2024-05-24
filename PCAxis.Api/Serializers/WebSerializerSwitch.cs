using System;

namespace PCAxis.Api.Serializers
{
    /// <summary>
    /// Gets Serializer from format code
    /// </summary>
    public class WebSerializerSwitch
    {

        internal static IWebSerializer GetSerializer(String formatCode)
        {
            IWebSerializer serializer;
            switch (formatCode != null ? formatCode.ToLower() : null)
            {
                case null:
                case "px":
                    serializer = new PxSerializer();
                    break;
                case "csv":
                    serializer = new CsvSerializer();
                    break;
                case "csv2":
                    serializer = new Csv2Serializer();
                    break;
                case "csv3":
                    serializer = new Csv3Serializer();
                    break;
                case "json":
                    serializer = new JsonSerializer();
                    break;
                case "json-stat":
                    serializer = new JsonStatSeriaizer();
                    break;
                case "json-stat2":
                    serializer = new JsonStat2Seriaizer();
                    break;
                case "xlsx":
                    serializer = new XlsxSerializer();
                    break;
                //case "png":
                //    int? width = tableQuery.Response.GetParamInt("width");
                //    int? height = tableQuery.Response.GetParamInt("height");
                //    string encoding = tableQuery.Response.GetParamString("encoding");
                //    serializer = new ChartSerializer(width, height, encoding);
                //    break;
                case "sdmx":
                    serializer = new SdmxDataSerializer();
                    break;
                default:
                    throw new NotImplementedException("Serialization for " + formatCode + " is not implemented");
            }
            return serializer;
        }


    }
}
