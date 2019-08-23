using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the General.Administration settings
    /// </summary>
    public interface IAdministrationSettings
    {
        /// <summary>
        /// If IP filter shall be used for the administration pages or not
        /// </summary>
        bool UseIPFilter { get; }

        /// <summary>
        /// IP addresses with permissions to access the administration pages
        /// </summary>
        IEnumerable<string> IPAddresses { get; }

    }
}
