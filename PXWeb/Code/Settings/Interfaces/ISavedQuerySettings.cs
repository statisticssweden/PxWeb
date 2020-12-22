using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Query;

namespace PXWeb
{
    public interface ISavedQuerySettings
    {
        SavedQueryStorageType StorageType { get; }

        /// <summary>
        /// If cacheing of requests shall be made or not
        /// </summary>
        bool EnableCache { get; }

        /// <summary>
        /// For how long time the saved query shall be cached
        /// </summary>
        int CacheTime { get; }

        /// <summary>
        /// Is Cross-Origin-Resource-Sharing allowed?
        /// </summary>
        bool EnableCORS { get; }

        /// <summary>
        /// Should the user see the period and id for the loaded saved query?
        /// </summary>
        bool ShowPeriodAndId { get; set; }

        /// <summary>
        /// Request should be limited
        /// </summary>
        bool EnableLimitRequest { get; set; }

        /// <summary>
        /// Maximum number of requests (works togheter with LimiterTimespan)
        /// </summary>
        int LimiterRequests { get; }

        /// <summary>
        /// Timespan for how many request can be made (see LimiterRequests)
        /// </summary>
        int LimiterTimespan { get; }

        /// <summary>
        /// Use headerfield instead of client ip-address
        /// </summary>
        string LimiterHttpHeaderName { get; }

    }
}
