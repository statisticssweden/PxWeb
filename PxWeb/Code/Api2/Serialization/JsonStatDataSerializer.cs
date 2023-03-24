using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;

namespace PxWeb.Code.Api2.Serialization
{
    public class PxDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "application/octet-stream; charset=" + System.Text.Encoding.Default.WebName;
            IPXModelStreamSerializer serializer = new PXFileSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
