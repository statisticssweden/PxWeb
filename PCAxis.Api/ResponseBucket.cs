using System;

namespace PCAxis.Api
{
    /// <summary>
    /// Class that holds a response and request in the Cache
    /// </summary>
    public class ResponseBucket
    {
        public string Key { get; set; }

        public string Url { get; set; }

        public string PostData { get; set; }

        public string ContentType { get; set; }

        public byte[] ResponseData { get; set; }

        public DateTime CreationTime { get; set; }

        public MenuObject Menu { get; set; }

        public int HttpResponseCode { get; set; }

        public ResponseBucket()
        {
            HttpResponseCode = 200;
            CreationTime = DateTime.Now;
        }
    }
}
