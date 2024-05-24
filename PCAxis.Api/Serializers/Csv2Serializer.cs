namespace PCAxis.Api.Serializers
{
    /// <summary>
    /// CSV2 serializer
    /// </summary>
    public class Csv2Serializer : IWebSerializer
    {
        #region IWebSerializer Members


        public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        {
            cacheResponse.ContentType = "text/csv; charset=" + System.Text.Encoding.Default.WebName;

            PCAxis.Paxiom.IPXModelStreamSerializer serializer = new PCAxis.Paxiom.Csv2FileSerializer();

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
