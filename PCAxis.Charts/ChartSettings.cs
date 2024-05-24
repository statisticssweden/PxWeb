using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

namespace PCAxis.Chart
{
    public class ChartSettings
    {
        #region enums

        public enum SortType
        {
            None,
            Ascending,
            Descending
        };

        public enum OrientationType
        {
            Horizontal,
            Vertical
        }

        [Flags]
        public enum GuidelinesType
        {
            None = 0x0,
            Horizontal = 0x1,
            Vertical = 0x2
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Set to true if selected chart type is Column-Line, else false.
        /// </summary>
        public bool IsColumnLine { get; set; }
        public SeriesChartType ChartType { get; set; }
        public bool UseSettingTitle { get; set; }
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int LineThickness { get; set; }
        public SortType TimeSortOrder { get; set; }
        public OrientationType LabelOrientation { get; set; }
        public GuidelinesType Guidelines { get; set; }
        public string GuidelinesColor { get; set; }
        public bool ShowLegend { get; set; }
        public int LegendFontSize { get; set; }
        public int LegendHeight { get; set; }
        public CultureInfo CurrentCulture { get; set; }
        public List<string> Colors { get; set; }
        public int ChartAlpha { get; set; }
        public int BackgroundAlpha { get; set; }
        public string BackgroundColor { get; set; }
        public string Logotype { get; set; }
        public string FontName { get; set; }
        public bool ShowSource { get; set; }
        public bool ShowLogo { get; set; }
        public string LogotypePath { get; set; }
        public int TitleFontSize { get; set; }
        public int AxisFontSize { get; set; }
        public string BackgroundColorGraphs { get; set; }
        public int LineThicknessPhrame { get; set; }
        public String LineColorPhrame { get; set; }

        #endregion

        public ChartSettings()
        {
            CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            Title = "";
            Width = 480;
            Height = 640;
            LineThickness = 2;
            TimeSortOrder = SortType.None;
            LabelOrientation = OrientationType.Horizontal;
            Guidelines = GuidelinesType.Horizontal;
            GuidelinesColor = "#757575";
            ShowLegend = true;
            LegendHeight = 20;
            LegendFontSize = 10;
            ChartType = SeriesChartType.Column;
            IsColumnLine = false;
            Colors = new List<string>();
            FontName = "Verdana";
            TitleFontSize = 14;
            AxisFontSize = 10;
            ShowSource = true;
            ShowSource = false;
            BackgroundColorGraphs = "#ffffff";
            LineThicknessPhrame = 0;
            LineColorPhrame = "#000000";
            Logotype = "";
            LogotypePath = "";
            BackgroundAlpha = 255;
            ChartAlpha = 255;
            BackgroundColor = "#ffffff";

        }

        public static SeriesChartType ConvertToChartType(string chartName, SeriesChartType defaultValue)
        {
            switch (chartName)
            {
                case ChartTypes.CHART_LINE:
                    return SeriesChartType.Line;
                case ChartTypes.CHART_BAR:
                    return SeriesChartType.Bar;
                case ChartTypes.CHART_COLUMN:
                    return SeriesChartType.Column;
                case ChartTypes.CHART_PIE:
                    return SeriesChartType.Pie;
                case ChartTypes.CHART_POPULATIONPYRAMID:
                    return SeriesChartType.Pyramid;
                case ChartTypes.CHART_COLUMNSTACKED:
                    return SeriesChartType.StackedColumn;
                case ChartTypes.CHART_BARSTACKED:
                    return SeriesChartType.StackedBar;
                case ChartTypes.CHART_COLUMNSTACKED100:
                    return SeriesChartType.StackedColumn100;
                case ChartTypes.CHART_BARSTACKED100:
                    return SeriesChartType.StackedBar100;
                case ChartTypes.CHART_AREA:
                    return SeriesChartType.Area;
                case ChartTypes.CHART_AREASTACKED:
                    return SeriesChartType.StackedArea;
                case ChartTypes.CHART_AREASTACKED100:
                    return SeriesChartType.StackedArea100;
                case ChartTypes.CHART_POINT:
                    return SeriesChartType.Point;
                case ChartTypes.CHART_RADAR:
                    return SeriesChartType.Radar;
            }
            return defaultValue;
        }

        public static ChartSettings.OrientationType ConvertToLabelOrientation(string orientation, ChartSettings.OrientationType defaultValue)
        {
            switch (orientation)
            {
                case "Horizontal":
                    return ChartSettings.OrientationType.Horizontal;
                case "Vertical":
                    return ChartSettings.OrientationType.Vertical;
                default:
                    break;
            }

            return defaultValue;
        }

    }
}
