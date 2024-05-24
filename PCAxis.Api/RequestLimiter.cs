using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace PCAxis.Api
{
    /// <summary>
    /// The class that dose the request limiting by IP
    /// </summary>
    public class RequestLimiter
    {
        //White list that escapes the limitations of the IP filter
        private static List<string> WhiteList;

        static RequestLimiter()
        {
            WhiteList = new List<string>();

            if (ConfigurationManager.AppSettings["ip-filter-white-list"] != null)
            {
                string wl = ConfigurationManager.AppSettings["ip-filter-white-list"];
                foreach (var ip in wl.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(ip))
                    {
                        WhiteList.Add(ip);
                    }
                }
            }
        }

        public string KeyStart { get; set; }
        /// <summary>
        /// The size of time window
        /// </summary>
        public int LimiterTimeSpan { get; set; }
        /// <summary>
        /// The number of calls allowed during the time window
        /// </summary>
        public int LimiterRequests { get; set; }
        /// <summary>
        /// Makes the Limiter use a HttpHeader, if present, in stead of UserHostAddress
        /// </summary>
        public string LimiterHttpHeaderName { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="seconds">The time window in seconds</param>
        /// <param name="actions">The number of calls allowed</param>
        /// <param name="httpHeaderName">Null or a httpheaderName. This header will then to be used as key in stead of UserHostAddress.</param>
        public RequestLimiter(string keystart, int seconds, int actions, string httpHeaderName)
        {
            KeyStart = keystart;
            LimiterTimeSpan = seconds;
            LimiterRequests = actions;
            LimiterHttpHeaderName = httpHeaderName;
        }

        /// <summary>
        /// Returns true if the client is allowed to make a request at this time
        /// </summary>
        /// <param name="request">The request containing the IP address of the caller, or( well, and) any headers to be used.</param>
        /// <returns>true if maximum number of calls has not been reached otherwise false</returns>
        public bool ClientLimitOK(HttpRequest request)
        {
            string clientAddress;

            if (String.IsNullOrEmpty(LimiterHttpHeaderName) || String.IsNullOrEmpty(request.Headers[LimiterHttpHeaderName]))
            {
                clientAddress = request.UserHostAddress;
            }
            else
            {
                clientAddress = request.Headers[LimiterHttpHeaderName];
            }

            //Checks if the adress is in the white list
            if (WhiteList.Contains(clientAddress, StringComparer.InvariantCultureIgnoreCase))
            {
                return true;
            }

            //check if the number of calls frpm the address is less then the threshold
            //string key = "RQLIMIT:" + clientAddress;
            string key = KeyStart + clientAddress;

            if (HttpRuntime.Cache[key] != null)
            {
                DateTime expireTime = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(LimiterTimeSpan));
                List<DateTime> requestTimestamps = HttpRuntime.Cache[key] as List<DateTime>;
                requestTimestamps.RemoveAll(date => date < expireTime);

                requestTimestamps.Add(DateTime.UtcNow);
                if (requestTimestamps.Count > LimiterRequests)
                    return false;
            }
            else
            {
                HttpRuntime.Cache.Add(key,
                    new List<DateTime>() { DateTime.UtcNow },
                    null,
                    Cache.NoAbsoluteExpiration,
                    new TimeSpan(0, 0, LimiterTimeSpan),
                    CacheItemPriority.NotRemovable,
                    null);
            }
            return true;
        }
    }
}