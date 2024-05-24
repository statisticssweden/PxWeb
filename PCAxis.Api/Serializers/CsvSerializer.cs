namespace PCAxis.Api.Serializers
{
    /// <summary>
    /// CSV serializer
    /// </summary>
    public class CsvSerializer : IWebSerializer
    {
        #region IWebSerializer Members


        public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        {
            cacheResponse.ContentType = "text/csv; charset=" + System.Text.Encoding.Default.WebName;
            PCAxis.Paxiom.IPXModelStreamSerializer serializer = new PCAxis.Paxiom.CsvFileSerializer();

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
