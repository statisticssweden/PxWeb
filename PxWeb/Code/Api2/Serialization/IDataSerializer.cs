using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;

namespace PxWeb.Code.Api2.Serialization
{
    public interface IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response);
    }
}
