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
            response.ContentType = "text/csv; charset=" + EncodingUtil.GetEncoding(model.Meta.CodePage).WebName;
            IPXModelStreamSerializer serializer = new RelationtableFileSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
