using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;
using PCAxis.Serializers;

namespace PxWeb.Code.Api2.Serialization
{
    public class JsonStatDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
            IPXModelStreamSerializer serializer = new JsonStatSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
