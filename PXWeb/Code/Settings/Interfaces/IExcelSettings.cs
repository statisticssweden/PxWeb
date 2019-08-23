using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for the General.FileFormats.Excel settings
    /// </summary>
    public interface IExcelSettings
    {
        /// <summary>
        /// Level of information in Excel file
        /// </summary>
        PCAxis.Paxiom.InformationLevelType InformationLevel { get; }

        /// <summary>
        /// Code and text or only text?
        /// </summary>
        bool DoubleColumn { get; }

        /// <summary>
        /// If data notes should be shown in Excel files
        /// </summary>
        bool ShowDataNotes { get; }
    }
}
