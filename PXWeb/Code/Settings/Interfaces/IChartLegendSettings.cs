using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PXWeb
{
    /// <summary>
    /// Interface for chart legend settings
    /// </summary>
    public interface IChartLegendSettings
    {
        /// <summary>
        /// If legend shall be displayed or not
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Font size for legend
        /// </summary>
        int FontSize { get; }

        /// <summary>
        /// Height for legend (in percent of total chart height)
        /// </summary>
        int Height { get; }
    }
}
