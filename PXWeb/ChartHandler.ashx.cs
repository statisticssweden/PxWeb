using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using PCAxis.Paxiom;
using PCAxis.Charting;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms.DataVisualization.Charting;
using PCAxis.Web.Controls;
using PCAxis.Chart;
using PCAxis.Web.Core.Management;

namespace PXWeb
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://www.scb.se/pcaxis/Charting/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ChartHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            ChartSettings settings = ChartManager.Settings;
            settings.IsColumnLine = IsColumnLine(context);
            if (!settings.IsColumnLine)
            {
                settings.ChartType = GetChartType(context, settings);
            }
            else
            {
                settings.ChartType = SeriesChartType.Column;
            }
            settings.Title = GetTitle(context, settings);
            settings.Width = GetWidth(context, settings);
            settings.Height = GetHeight(context, settings);
            settings.LineThickness = GetLineThickness(context, settings);
            settings.LabelOrientation = GetLabelOrientation(context, settings);
            settings.Guidelines = GetGuidelines(context, settings);
            settings.ShowLegend = GetLegend(context, settings);
            settings.LegendHeight = GetLegendHeight(context, settings);

            PxWebChart chart;
            chart = ChartHelper.GetChart(settings, PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel);

            MemoryStream s = new MemoryStream();

            // Display on screen
            context.Response.ContentType = "image/png";
            chart.SaveImage(s, ImageFormat.Png);
            
            s.WriteTo(context.Response.OutputStream);
            context.Response.End();
        }

        /// <summary>
        /// Is the selected chart type Column-Line?
        /// </summary>
        /// <param name="context">Http context</param>
        /// <returns>Returns true if selected charttype is Column-Line, else false</returns>
        private bool IsColumnLine(HttpContext context)
        {
            if (QuerystringManager.GetQuerystringParameter(context, ChartParameters.CHARTTYPE) == Plugins.Views.CHART_COLUMNLINE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get chart type
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="settings">Chart settings object</param>
        /// <returns>Chart type</returns>
        private SeriesChartType GetChartType(HttpContext context, ChartSettings settings)
        {
            switch (QuerystringManager.GetQuerystringParameter(context, ChartParameters.CHARTTYPE))
            {
                case Plugins.Views.CHART_LINE:
                    return SeriesChartType.Line;
                case Plugins.Views.CHART_BAR:
                    return SeriesChartType.Bar;
                case Plugins.Views.CHART_COLUMN:
                    return SeriesChartType.Column;
                case Plugins.Views.CHART_PIE:
                    return SeriesChartType.Pie;
                case Plugins.Views.CHART_POPULATIONPYRAMID:
                    return SeriesChartType.Pyramid;
                case Plugins.Views.CHART_COLUMNSTACKED:
                    return SeriesChartType.StackedColumn;
                case Plugins.Views.CHART_BARSTACKED:
                    return SeriesChartType.StackedBar;
                case Plugins.Views.CHART_COLUMNSTACKED100:
                    return SeriesChartType.StackedColumn100;
                case Plugins.Views.CHART_BARSTACKED100:
                    return SeriesChartType.StackedBar100;
                case Plugins.Views.CHART_AREA:
                    return SeriesChartType.Area;
                case Plugins.Views.CHART_AREASTACKED:
                    return SeriesChartType.StackedArea;
                case Plugins.Views.CHART_AREASTACKED100:
                    return SeriesChartType.StackedArea100;
                case Plugins.Views.CHART_POINT:
                    return SeriesChartType.Point;
                case Plugins.Views.CHART_RADAR:
                    return SeriesChartType.Radar;
            }
            return settings.ChartType;
        }

        /// <summary>
        /// Get title
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="settings">Chart settings object</param>
        /// <returns>Chart title</returns>
        private string GetTitle(HttpContext context, ChartSettings settings)
        {
            if (PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Data.Model.Meta.DescriptionDefault)
            {
                return PCAxis.Web.Core.Management.PaxiomManager.PaxiomModel.Data.Model.Meta.Description;
            }
            if (QuerystringManager.GetQuerystringParameter(context, ChartParameters.TITLE) != null)
            {
                return HttpUtility.UrlDecode(QuerystringManager.GetQuerystringParameter(context, ChartParameters.TITLE));
            }

            return "";
        }

        /// <summary>
        /// Get width
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="settings">Chart settings object</param>
        /// <returns>Chart width</returns>
        private int GetWidth(HttpContext context, ChartSettings settings)
        {
            int width = 0;
            if (int.TryParse(QuerystringManager.GetQuerystringParameter(context, ChartParameters.WIDTH), out width))
            {
                if (width > 0)
                {
                    if (width > PXWeb.Settings.Current.Features.Charts.MaxWidth)
                    {
                        return PXWeb.Settings.Current.Features.Charts.MaxWidth;
                    }
                    else
                    { 
                        return width;
                    }
                }
            }

            return settings.Width;
        }

        /// <summary>
        /// Get height
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="settings">Chart settings object</param>
        /// <returns>Chart height</returns>
        private int GetHeight(HttpContext context, ChartSettings settings)
        {
            int height = 0;
            if (int.TryParse(QuerystringManager.GetQuerystringParameter(context, ChartParameters.HEIGHT), out height))
            {
                if (height > 0)
                {
                    if (height > PXWeb.Settings.Current.Features.Charts.MaxHeight)
                    {
                        return PXWeb.Settings.Current.Features.Charts.MaxHeight;
                    }
                    else
                    { 
                        return height;
                    }
                }
            }

            return settings.Height;
        }

        /// <summary>
        /// Get Line thickness
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="settings">Chart settings object</param>
        /// <returns>Chart line thickness</returns>
        private int GetLineThickness(HttpContext context, ChartSettings settings)
        {
            int thickness = 0;
            if (int.TryParse(QuerystringManager.GetQuerystringParameter(context, ChartParameters.LINE_THICKNESS), out thickness))
            {
                if (thickness > 0)
                {
                    return thickness;
                }
            }

            return settings.LineThickness;
        }


        /// <summary>
        /// Get label orientation
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="settings">Chart settings object</param>
        /// <returns>Chart label orientation</returns>
        private ChartSettings.OrientationType GetLabelOrientation(HttpContext context, ChartSettings settings)
        {
            if (QuerystringManager.GetQuerystringParameter(context, ChartParameters.LABELORIENTATION) != null)
            {
                switch (QuerystringManager.GetQuerystringParameter(context, ChartParameters.LABELORIENTATION).ToLower())
                {
                    case "horizontal":
                        return ChartSettings.OrientationType.Horizontal;
                    case "vertical":
                        return ChartSettings.OrientationType.Vertical;
                    default:
                        break;
                }
            }

            return settings.LabelOrientation;
        }

        /// <summary>
        /// Get guidelines
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="settings">Chart settings object</param>
        /// <returns>Chart guidelines</returns>
        private ChartSettings.GuidelinesType GetGuidelines(HttpContext context, ChartSettings settings)
        {
            ChartSettings.GuidelinesType guidelines = ChartSettings.GuidelinesType.None;

            if ((QuerystringManager.GetQuerystringParameter(context, ChartParameters.GUIDELINES_HORIZONTAL) != null) || (QuerystringManager.GetQuerystringParameter(context, ChartParameters.GUIDELINES_VERTICAL) != null))
            {
                if (QuerystringManager.GetQuerystringParameter(context, ChartParameters.GUIDELINES_HORIZONTAL).ToLower() == "true")
                {
                    guidelines = guidelines | ChartSettings.GuidelinesType.Horizontal;
                }
                if (QuerystringManager.GetQuerystringParameter(context, ChartParameters.GUIDELINES_VERTICAL).ToLower() == "true")
                {
                    guidelines = guidelines | ChartSettings.GuidelinesType.Vertical;
                }

                return guidelines;
            }

            return settings.Guidelines;
        }

        /// <summary>
        /// Get show legend
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="settings">Chart settings object</param>
        /// <returns>If legend shall be displayed in chart or not</returns>
        private bool GetLegend(HttpContext context, ChartSettings settings)
        {
            bool showLegend;
            if (bool.TryParse(QuerystringManager.GetQuerystringParameter(context, ChartParameters.LEGEND), out showLegend))
            {

                return showLegend;
            }
            else
            {
                return settings.ShowLegend;
            }
        }

        /// <summary>
        /// Get legend height
        /// </summary>
        /// <param name="context">Http context</param>
        /// <param name="settings">Chart settings object</param>
        /// <returns>Chart legend height</returns>
        private int GetLegendHeight(HttpContext context, ChartSettings settings)
        {
            int height = 0;
            if (int.TryParse(QuerystringManager.GetQuerystringParameter(context, ChartParameters.LEGENDHEIGHT), out height))
            {
                if (height > 0)
                {
                    return height;
                }
            }

            return settings.LegendHeight;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
