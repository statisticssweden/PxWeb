using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;

namespace PxWeb.Code.Api2.Serialization
{
    public class Csv2DataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            response.ContentType = "text/csv; charset=" + EncodingUtil.GetEncoding(model.Meta.CodePage);
            IPXModelStreamSerializer serializer = new Csv2FileSerializer();
            serializer.Serialize(model, response.Body);
        }
    }
}
