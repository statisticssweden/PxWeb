using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;
using PCAxis.Query;
using System.IO;
using PCAxis.Paxiom.Operations;

namespace PCAxis.Api.Serializers
{
    /// <summary>
    /// JSON serializer
    /// </summary>
    public class JsonSerializer :IWebSerializer
    {       

        #region IWebSerializer Members

        public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        {
            cacheResponse.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
            PCAxis.Paxiom.IPXModelStreamSerializer serializer = new PXSerializers.Json.JsonSerializer();

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {

                serializer.Serialize(model, stream);
                
                cacheResponse.ResponseData = stream.ToArray();
            }
        }

        #endregion
    }
}
