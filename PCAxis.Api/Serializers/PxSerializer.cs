namespace PCAxis.Api.Serializers
{
    /// <summary>
    /// PX file serializer
    /// </summary>
    public class PxSerializer : IWebSerializer
    {
        #region IWebSerializer Members

        public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        {
            cacheResponse.ContentType = "application/octet-stream; charset=" + System.Text.Encoding.Default.WebName;
            PCAxis.Paxiom.IPXModelStreamSerializer serializer = new PCAxis.Paxiom.PXFileSerializer();

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
