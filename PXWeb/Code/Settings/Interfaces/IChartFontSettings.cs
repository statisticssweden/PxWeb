namespace PXWeb
{
    /// <summary>
    /// Interface for chart fonts
    /// </summary>
    public interface IChartFontSettings
    {
        /// <summary>
        /// Font name 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Font size for chart title
        /// </summary>
        int TitleSize { get; }

        /// <summary>
        /// Font size for chart axis
        /// </summary>
        int AxisSize { get; }
    }
}
