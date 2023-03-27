using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;
using PCAxis.Serializers;
using System.IO;
using System.Net.Sockets;

namespace PxWeb.Code.Api2.Serialization
{
    public class RelationalTableDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            // todo: find right format
            //application / octet - stream
            //response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet; charset=" + System.Text.Encoding.Default.WebName;
            IPXModelStreamSerializer serializer = new RelationtableFileSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
