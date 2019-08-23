using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the General.FileFormats settings
    /// </summary>
    public interface IFileFormatsSettings
    {
        /// <summary>
        /// Maximum of downloadable cells in resulting table
        /// </summary>
        int CellLimitDownloads { get; }

        /// <summary>
        /// Excel settings
        /// </summary>
        IExcelSettings Excel { get; }
    }
}
