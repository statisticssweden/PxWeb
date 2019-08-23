using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the General.Modules settings
    /// </summary>
    public interface IModulesSettings
    {
        /// <summary>
        /// If login shall be shown in PX-Web or not
        /// </summary>
        bool ShowLogin { get; }

        /// <summary>
        /// If usage logging shall be activated in PX-Web or not
        /// </summary>
        bool UsageLogging { get; }

        /// <summary>
        /// If saved queries shall be activated in PX-Web or not
        /// </summary>
        bool SavedQueries { get; }
    }
}
