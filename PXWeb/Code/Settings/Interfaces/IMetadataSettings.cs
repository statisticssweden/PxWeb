using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for metadata settings
    /// </summary>
    public interface IMetadataSettings
    {
        /// <summary>
        /// If links to metadata system shall be displayed or not
        /// </summary>
        bool UseMetadata { get; }

        /// <summary>
        /// Path to the Metadata.config file
        /// </summary>
        string MetaSystemConfigFile { get; }

        /// <summary>
        /// Implementation for creating metadata links
        /// </summary>
        PCAxis.Metadata.IMetaIdProvider MetaLinkMethod { get; }
    }
}
