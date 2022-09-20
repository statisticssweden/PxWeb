using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PxWeb
{
    /// <summary>
    /// Class that holds a response and request in the Cache
    /// </summary>
    public class ResponseBucket
    {
        public string Response { get; set; }
        public DateTime CreationTime { get; set; }
        public int HttpResponseCode { get; set; }
        public ResponseBucket()
        {
            HttpResponseCode = 200;
            CreationTime = DateTime.Now;
        }
    }
}