using System.IO;
using System.Web;

namespace PCAxis.Api.Serializers
{
    /// <summary>
    /// Excel xlsx serializer
    /// </summary>
    class XlsxSerializer : IWebSerializer
    {

        #region IWebSerializer Members

        public void Serialize(PCAxis.Paxiom.PXModel model, HttpResponse httpResponse)
        {
            PCAxis.Serializers.XlsxSerializer serializer = new PCAxis.Serializers.XlsxSerializer();
            serializer.InformationLevel = PCAxis.Paxiom.InformationLevelType.AllInformation;
            serializer.DoubleColumn = PCAxis.Paxiom.Settings.Files.DoubleColumnFile;
            httpResponse.AddHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet; charset=" + System.Text.Encoding.Default.WebName);
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(model, ms);
                ms.Position = 0;
                ms.WriteTo(httpResponse.OutputStream);
            }
        }


        public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        {
            cacheResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet; charset=" + System.Text.Encoding.Default.WebName;
            PCAxis.Serializers.XlsxSerializer serializer = new PCAxis.Serializers.XlsxSerializer();
            serializer.InformationLevel = PCAxis.Paxiom.InformationLevelType.AllInformation;
            serializer.DoubleColumn = PCAxis.Paxiom.Settings.Files.DoubleColumnFile;

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
