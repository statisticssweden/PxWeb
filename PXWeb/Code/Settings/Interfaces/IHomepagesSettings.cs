using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for database homepages settings
    /// </summary>
    public interface IHomepagesSettings
    {
        /// <summary>
        /// Get homepage for the specified language
        /// </summary>
        /// <param name="language">Language code</param>
        /// <returns>IHomepage Settings object</returns>
        IHomepageSettings GetHomepage(string language);

    }
}
