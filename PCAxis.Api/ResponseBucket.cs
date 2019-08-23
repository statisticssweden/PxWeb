using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
 
        public ResponseBucket()
        {
            CreationTime = DateTime.Now;
        }
    }
}
