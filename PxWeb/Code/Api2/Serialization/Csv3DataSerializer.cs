using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;
using PCAxis.Serializers;

namespace PxWeb.Code.Api2.Serialization
{
    public class Csv3DataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "text/csv; charset=" + System.Text.Encoding.Default.WebName;
            IPXModelStreamSerializer serializer = new Csv3FileSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
