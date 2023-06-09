using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;

namespace PxWeb.Code.Api2.Serialization
{
    public class PxDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "application/octet-stream; charset=" + EncodingUtil.GetEncoding(model.Meta.CodePage).WebName;
            response.Headers.Add("Content-Disposition", "inline;filename=data.px");
            IPXModelStreamSerializer serializer = new PXFileSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
