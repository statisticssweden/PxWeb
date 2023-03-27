using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;
using PCAxis.Serializers;

namespace PxWeb.Code.Api2.Serialization
{
    public class JsonStat2DataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
            IPXModelStreamSerializer serializer = new JsonStat2Serializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
