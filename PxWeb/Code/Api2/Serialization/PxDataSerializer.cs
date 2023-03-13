using Microsoft.AspNetCore.Http;
using PCAxis.Paxiom;

namespace PxWeb.Code.Api2.Serialization
{
    public class PxDataSerializer : IDataSerializer
    {
        public void Serialize(PXModel model, HttpResponse response)
        {
            //TODO
            //Create PXFileSerializer
            //Set Contenttype on the response
            //Serialize the data to the response stream 

        }
    }
}
