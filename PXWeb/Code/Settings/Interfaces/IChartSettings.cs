using PCAxis.Chart;
using System;
using System.Collections.Generic;

namespace PXWeb
{
    /// <summary>
    /// Interface for the chart settings
    /// </summary>
    public interface IChartSettings
    {
        /// <summary>
        /// Default height for chart
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Maximum height for chart
        /// </summary>
        int MaxHeight { get; }

        /// <summary>
        /// Default width for chart
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Maximum width for chart
        /// </summary>
        int MaxWidth { get; }

        /// <summary>
        /// Chart colors
        /// </summary>
        IEnumerable<string> Colors { get; }

        /// <summary>
        /// Default line thickness (only for chart types that use lines)
        /// </summary>
        int LineThickness { get; }

        /// <summary>
        /// Maximum line thickness
        /// </summary>
        int MaxLineThickness { get; }

        /// <summary>
        /// Maximum number of values for chart
        /// </summary>
        int MaxValues { get; }

        /// <summary>
        /// logotype image
        /// </summary>
        string Logotype { get; }

        /// <summary>
        /// Show logo/brand in bottom of graphs. Path to the
        /// logo is definded in the Logotype setting
        /// </summary>
        bool ShowLogo { get; }

        /// <summary>
        /// Show sourse in graphs
        /// </summary>
        bool ShowSource { get; }

        /// <summary>
        /// Color on the background
        /// </summary>
        string BackgroundColor { get; }

        /// <summary>
        /// Alpha of the color on the background
        /// </summary>
        int BackgroundAlpha { get; }


        /// <summary>
        /// Color on the background in the graph
        /// </summary>
        string BackgroundColorGraphs { get; }

        /// <summary>
        /// Alpha of the color on the background in the graph
        /// </summary>
        int ChartAlpha { get; }

        /// <summary>
        /// Line thickness on phrame 
        /// </summary>
        int LineThicknessPhrame { get; }

        /// <summary>
        /// Line thickness on phrame 
        /// </summary>
        String LineColorPhrame { get; }

        /// <summary>
        /// Default time sort order for chart
        /// </summary>
        ChartSettings.SortType TimeSortOrder { get; }

        /// <summary>
        /// Default label orientation
        /// </summary>
        ChartSettings.OrientationType LabelOrientation { get; }

        /// <summary>
        /// Chart font settings
        /// </summary>
        IChartFontSettings Font { get; }

        /// <summary>
        /// Chart legend settings
        /// </summary>
        IChartLegendSettings Legend { get; }

        /// <summary>
        /// Chart guidelines settings
        /// </summary>
        IChartGuidelinesSettings Guidelines { get; }

    }
}
