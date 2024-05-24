namespace PCAxis.Api.Serializers
{
    class SdmxDataSerializer : IWebSerializer
    {
        #region IWebSerializer Members

        public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        {
            cacheResponse.ContentType = "text/xml; charset=" + System.Text.Encoding.Default.WebName;
            PCAxis.Paxiom.IPXModelStreamSerializer serializer = new PCAxis.Serializers.SdmxDataSerializer();

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
