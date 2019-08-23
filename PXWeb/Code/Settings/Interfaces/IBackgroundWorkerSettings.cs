using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for background worker process settings
    /// </summary>
    public interface IBackgroundWorkerSettings
    {
        /// <summary>
        /// The time in seconds the worker process shall sleep between iterations
        /// </summary>
        int SleepTime { get; }
    }
}
