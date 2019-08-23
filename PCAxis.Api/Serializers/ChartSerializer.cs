using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;
using PCAxis.Query;
using System.IO;
using PCAxis.Paxiom.Operations;
using PCAxis.Chart;
using System.Security.Cryptography;

namespace PCAxis.Api.Serializers
{
    /// <summary>
    /// Chart serializer
    /// </summary>
    public class ChartSerializer : IWebSerializer
    {
        private PCAxis.Chart.ChartSerializer _chartSerializer;
        private string _encoding;

        public ChartSerializer(int? width, int? height, string encoding)
        {
            _chartSerializer = new PCAxis.Chart.ChartSerializer();
            if(width.HasValue)
                _chartSerializer.Settings.Width = width.Value;
            if(height.HasValue)
                _chartSerializer.Settings.Height = height.Value;
            _encoding = string.IsNullOrEmpty(encoding) ? "binary" : encoding;
        }

        #region IWebSerializer Members

        public void Serialize(PXModel model, System.IO.MemoryStream httpResponse)
        {
            Stream s;

            if (_encoding == "base64")
            {
                s = new CryptoStream(httpResponse, new ToBase64Transform(), CryptoStreamMode.Write); 
            }
            else
            {
                s = httpResponse;
            }

            _chartSerializer.Serialize(model, s);
            s.Flush();
        }


        public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        {
            
            if (_encoding == "base64")
            {
                cacheResponse.ContentType = "text/plain; charset=" + System.Text.Encoding.Default.WebName;
            }
            else
            {
                cacheResponse.ContentType = "image/png; charset=" + System.Text.Encoding.Default.WebName;
            }

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                Serialize(model, stream);
                stream.Flush();
                cacheResponse.ResponseData = stream.ToArray();
            }
        }

        #endregion
    }
}
