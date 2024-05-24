﻿namespace PCAxis.Api.Serializers
{
    class JsonStat2Seriaizer : IWebSerializer
    {
        #region IWebSerializer Members


        public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        {
            cacheResponse.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;
            PCAxis.Paxiom.IPXModelStreamSerializer serializer = new PCAxis.Serializers.JsonStat2Serializer();

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
