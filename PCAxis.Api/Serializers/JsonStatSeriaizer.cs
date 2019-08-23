using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PCAxis.Api.Serializers
{
    class JsonStatSeriaizer : IWebSerializer
    {
        #region IWebSerializer Members


        public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        {
            cacheResponse.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
            var jsonStatSerializer = new PCAxis.Serializers.JsonStat.JsonStatSerializer();

            var geoVariablesStr = ConfigurationManager.AppSettings["geoVariables"];

            if (!string.IsNullOrEmpty(geoVariablesStr))
            {
                jsonStatSerializer.GeoVariableCodes = geoVariablesStr.Split(",".ToCharArray());
            }
            
            PCAxis.Paxiom.IPXModelStreamSerializer serializer = jsonStatSerializer;
            
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                serializer.Serialize(model, stream);
                stream.Flush();
                cacheResponse.ResponseData = stream.ToArray();
            }
        }

        #endregion
    }
}
