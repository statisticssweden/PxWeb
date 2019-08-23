using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for chart guidelines settings
    /// </summary>
    public interface IChartGuidelinesSettings
    {
        /// <summary>
        /// Color for guidelines
        /// </summary>
        string Color { get; }

        /// <summary>
        /// Display horizontal guidelines (default value)
        /// </summary>
        bool Horizontal { get; }

        /// <summary>
        /// Display vertical guidelines (default value)
        /// </summary>
        bool Vertical { get; }
    }
}
