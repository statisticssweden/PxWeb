using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the Navigation settings
    /// </summary>
    public interface INavigationSettings
    {
        /// <summary>
        /// If the features NavigationFlow is enabled or not
        /// </summary>
        /// </summary>
        bool ShowNavigationFlow { get;  }
    }
}
