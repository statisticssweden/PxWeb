﻿namespace PXWeb
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
        /// What information should the file name be based on
        /// </summary>
        PCAxis.Paxiom.FileBaseNameType FileBaseName { get; }

        /// <summary>
        /// Excel settings
        /// </summary>
        IExcelSettings Excel { get; }
    }
}
