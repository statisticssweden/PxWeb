using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;

namespace PxWeb.Code.Api2.Serialization
{
    public class CsvTabDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "text/csv; charset=" + System.Text.Encoding.Default.WebName;
            IPXModelStreamSerializer serializer = new CsvFileSerializer();
            ((CsvFileSerializer)serializer).Title = false;
            ((CsvFileSerializer)serializer).Delimiter = '\t';
            serializer.Serialize(model, response.Body);
        }
    }
}
