using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    //Interface for settings concerning the homepage for a specific language
    public interface IHomepageSettings
    {
        /// <summary>
        /// Name (code) for the language
        /// </summary>
        string Language { get; }

        /// <summary>
        /// URL to the homepage
        /// </summary>
        string Url { get; }

        /// <summary>
        /// If the database home page is external or (located outside PX-Web)
        /// </summary>
        bool IsExternal { get; }
    }
}
