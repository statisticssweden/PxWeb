using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;
using PCAxis.Serializers;

namespace PxWeb.Code.Api2.Serialization
{
    public class Html5TableDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "text/html; charset=" + System.Text.Encoding.Default.WebName;
            IPXModelStreamSerializer serializer = new Html5TableSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
