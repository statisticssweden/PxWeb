using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the General.Paths settings
    /// </summary>
    public interface IPathsSettings
    {
        /// <summary>
        /// Path to the language files
        /// </summary>
        string LanguagesPath { get; }

        /// <summary>
        /// Path to the images
        /// </summary>
        string ImagesPath { get; }

        /// <summary>
        /// Path to the PX-file databases
        /// </summary>
        string PxDatabasesPath { get; }

        /// <summary>
        /// Path to the aggregation files
        /// </summary>
        string PxAggregationsPath { get; }
    }
}
