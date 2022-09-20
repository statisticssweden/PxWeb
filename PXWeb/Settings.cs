using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace PxWeb
{
    public class Settings
    {
        private Settings()
        {
            // Parse if we enabled CORS
            string strCORSEnabled = ConfigurationManager.AppSettings["enableCORS"];
            if (strCORSEnabled != null)
                EnableCORS = bool.Parse(strCORSEnabled);

            // Parse max values returned
            string strMaxValuesReturned = ConfigurationManager.AppSettings["maxValuesReturned"];
            if (strMaxValuesReturned != null)
                MaxValues = int.Parse(strMaxValuesReturned);
            else
                MaxValues = 10000;

            //Parse request limiter settings
            string strLimiterEnabled = ConfigurationManager.AppSettings["enableLimiter"];
            if (strLimiterEnabled != null)
                EnableLimiter = bool.Parse(strLimiterEnabled);
            else
                EnableLimiter = true;

            string strLimiterRequests = ConfigurationManager.AppSettings["limiterRequests"];
            if (strLimiterRequests != null)
                LimiterRequests = int.Parse(strLimiterRequests);
            else
                LimiterRequests = 5;

            string strLimiterTimeSpan = ConfigurationManager.AppSettings["limiterTimeSpan"];
            if (strLimiterTimeSpan != null)
                LimiterTimeSpan = int.Parse(strLimiterTimeSpan);
            else
                LimiterTimeSpan = 10;

            string strLimiterHttpHeaderName = ConfigurationManager.AppSettings["limiterHttpHeaderName"];
            LimiterHttpHeaderName = strLimiterHttpHeaderName;


            //Parse if cahce should be enabled
            string strCacheEnabled = ConfigurationManager.AppSettings["enableCache"];
            if (strCacheEnabled != null)
                EnableCache = bool.Parse(strCacheEnabled);
            else
                EnableCache = true;

            //parses the clera cache time periods
            string strClearCache = ConfigurationManager.AppSettings["clearCache"];
            if (strLimiterRequests != null)
                ClearCache = strClearCache;
            else
                ClearCache = "";


            // Parse max values returned
            string strFetchCellLimit = ConfigurationManager.AppSettings["fetchCellLimit"];
            if (strFetchCellLimit != null)
                FetchCellLimit = long.Parse(strFetchCellLimit);
            else
                FetchCellLimit = 100000;

            // Default response format
            string strDefaultResponseFormat = ConfigurationManager.AppSettings["defaultResponseFormat"];

            if (!string.IsNullOrEmpty(strDefaultResponseFormat))
                DefaultResponseFormat = strDefaultResponseFormat;
            else
                DefaultResponseFormat = "px"; //was hard coded in TableQuery class before
        }

        /// <summary>
        /// Is CORS should be enabled
        /// </summary>
        public bool EnableCORS { get; set; }

        /// <summary>
        /// The maximum values that are returned in a table response
        /// </summary>
        public int MaxValues { get; set; }

        /// <summary>
        /// If limiter should be enabled
        /// </summary>
        public bool EnableLimiter { get; set; }

        /// <summary>
        /// Limiter number of request
        /// </summary>
        public int LimiterRequests { get; set; }

        /// <summary>
        /// Limiter size of time window i seconds
        /// </summary>
        public int LimiterTimeSpan { get; set; }


        /// <summary>
        /// Makes the Limiter use a HttpHeader(e.g. X-Forwarded-For), if present, in stead of UserHostAddress
        /// </summary>
        public string LimiterHttpHeaderName { get; set; }

        /// <summary>
        /// If the cache should be enabled
        /// </summary>
        public bool EnableCache { get; set; }

        /// <summary>
        /// Limit of the number of cells that can be fetched
        /// </summary>
        public long FetchCellLimit { get; set; }

        /// <summary>
        /// Points in time for when the cache should be cleared separated with semicolon
        /// E.g. 09:30;14:52
        /// </summary>
        public string ClearCache { get; set; }

        /// <summary>
        /// Default response format API
        /// E.g. 09:30;14:52
        /// </summary>
        public string DefaultResponseFormat { get; set; }

        static Settings()
        {
            _settings = new Settings();
        }
        private static Settings _settings;
        public static Settings Current { get { return _settings; } }

    }
}