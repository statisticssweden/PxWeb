using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;
using PxWeb.Code.Api2.Cache;
using System.IO;

namespace PxWeb.Code.Api2.Serialization
{
    public class PxDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "application/octet-stream; charset=" + System.Text.Encoding.Default.WebName;
            PCAxis.Paxiom.IPXModelStreamSerializer serializer = new PCAxis.Paxiom.PXFileSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
