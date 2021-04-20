using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the General.Site settings
    /// </summary>
    public interface ISiteSettings
    {
        /// <summary>
        /// Application name
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Logo path
        /// </summary>
        string LogoPath { get; }

        /// <summary>
        /// Main header (H1) type for table pages
        /// </summary>
        MainHeaderForTablesType MainHeaderForTables { get; }
    }
}
