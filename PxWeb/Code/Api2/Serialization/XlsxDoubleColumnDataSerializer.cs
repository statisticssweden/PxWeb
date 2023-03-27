using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;
using PCAxis.Serializers;

namespace PxWeb.Code.Api2.Serialization
{
    public class XlsxDoubleColumnDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet; charset=" + System.Text.Encoding.Default.WebName;
            IPXModelStreamSerializer serializer = new XlsxSerializer();
            ((XlsxSerializer)serializer).DoubleColumn = DoubleColumnType.AlwaysDoubleColumns;
            serializer.Serialize(model, response.Body);
        }
    }
}
