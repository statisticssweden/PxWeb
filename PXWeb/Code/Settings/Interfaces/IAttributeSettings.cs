using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the Presentation.Table.Attribute settings
    /// </summary>
    public interface IAttributeSettings
    {
        /// <summary>
        /// If attributes at cell level shall be displayed in the table or not
        /// </summary>
        bool DisplayAttributes { get; }

        /// <summary>
        /// If the default values for attributes shall be displayed beneath the table or not
        /// </summary>
        bool DisplayDefaultAttributes { get; }
    }
}
