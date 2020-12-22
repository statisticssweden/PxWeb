using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the API settings
    /// </summary>
    public interface IApiSettings
    {
        /// <summary>
        /// URL root. root of API url
        /// </summary>
        string UrlRoot { get; }
        /// <summary>
        /// URL route identifying API request
        /// </summary>
        string RoutePrefix { get; }

        /// <summary>
        /// Maximum number of values returned from query
        /// </summary>
        int MaxValuesReturned { get; }

        /// <summary>
        /// Maximum number of requests (works togheter with LimiterTimespan)
        /// </summary>
        int LimiterRequests { get; }

        /// <summary>
        /// Timespan for how many request can be made (see LimiterRequests)
        /// </summary>
        int LimiterTimespan { get; }

        /// <summary>
        /// Maximum number of cells returned from query
        /// </summary>
        int FetchCellLimit { get; }

        /// <summary>
        /// Is Cross-Origin-Resource-Sharing allowed?
        /// </summary>
        bool EnableCORS { get; }

        /// <summary>
        /// If cacheing of requests shall be made or not
        /// </summary>
        bool EnableCache { get; }

        /// <summary>
        /// Default height for chart
        /// </summary>
        bool ShowQueryInformation { get; }

        /// <summary>
        /// URL to API info page
        /// </summary>
        string InfoURL { get; }

        /// <summary>
        /// The response format that will be displayed in the API example web control
        /// </summary>
        string DefaultExampleResponseFormat { get; }

        /// <summary>
        /// If save API query button will be displayed
        /// </summary>
        bool ShowSaveApiQueryButton { get; }

        /// <summary>
        /// Text for saved API query
        /// </summary>
        string SaveApiQueryText { get; }
    }
}
