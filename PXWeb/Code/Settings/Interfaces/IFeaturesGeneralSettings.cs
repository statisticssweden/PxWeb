using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the Features.General settings
    /// </summary>
    public interface IFeaturesGeneralSettings
    {
        /// <summary>
        /// If the Charts features is enabled or not
        /// </summary>
        bool ChartsEnabled { get; }
        /// <summary>
        /// If the API feature is enabled or not
        /// </summary>
        bool ApiEnabled { get; }
        /// <summary>
        /// If the Saved query feature is enabled or not
        /// </summary>
        bool SavedQueryEnabled { get; }
        /// <summary>
        /// If the User friendly URL:s feature is enabled or not
        /// </summary>
        bool UserFriendlyUrlsEnabled { get; }
        /// <summary>
        /// If logging of user statistics shall be switched on or off
        /// </summary>
        bool UserStatisticsEnabled { get; }
        /// <summary>
        /// If search functionality is enabled or not
        /// </summary>
        bool SearchEnabled { get; }
        /// <summary>
        /// If background worker process shall be used or not
        /// </summary>
        bool BackgroundWorkerEnabled { get; }
        /// <summary>
        /// If bulk link is enabled or not
        /// </summary>
        bool BulkLinkEnabled { get; }
        /// <summary>
        /// Clear the cache at the specified time(s)
        /// </summary>
        string ClearCache { get; }

    }
}
