using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;
using PCAxis.Serializers;

namespace PxWeb.Code.Api2.Serialization
{
    public class XlsxDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; 
            IPXModelStreamSerializer serializer = new XlsxSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
