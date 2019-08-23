using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCAxis.Api
{
    /// <summary>
    /// Extension methods to the HttpContext to send the responses
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Sends data without caching it
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentType"></param>
        /// <param name="data"></param>
        public static void Send(this HttpContext context, string contentType, byte[] data)
        {
            context.Response.ContentType = contentType;
            context.Response.OutputStream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Sends data 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheResponse"></param>
        /// <param name="doCache"></param>
        public static void Send(this HttpContext context, ResponseBucket cacheResponse, bool doCache)
        {
            context.Send(cacheResponse.ContentType, cacheResponse.ResponseData);

            // Store request in cache
            if (doCache)
            {
                ApiCache.Current.Store(cacheResponse);
            }
        }

         /// <summary>
         /// Send a error response to the client
         /// </summary>
         /// <param name="context"></param>
         /// <param name="data"></param>
         /// <param name="code"></param>
        public static void SendJSONError(this HttpContext context, string data, int code)
        {
            context.Response.StatusCode = code;
            Send(context, "application/json", context.Response.ContentEncoding.GetBytes(data));
        }
    }
}