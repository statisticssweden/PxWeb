using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Globalization;

namespace PCAxis.Chart
{
    public class ChartHelper
    {
        public static PxWebChart GetChart(ChartSettings settings, PXModel model)
        {
            PxWebChart chart;


            CultureInfo ci = new CultureInfo(settings.CurrentCulture.Name);
            if (ci.IsNeutralCulture)
            {
                ci =  CultureInfo.CreateSpecificCulture(settings.CurrentCulture.Name);
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            chart = new PxWebChart(model,
                                   settings,
                                   c =>
                                   {
                                       if (settings.IsColumnLine)
                                       {
                                           // Column + Line chart
                                           c.ChartTypes = new SeriesChartType[] { SeriesChartType.Column, SeriesChartType.Line };
                                           c.SecondaryAxisFromSeriesNumber = 2;
                                           c.SecondaryAxisAddToSeriesName = " " + GetLocalizedString("PxWebChartRightScale", settings.CurrentCulture);
                                           c.SecondaryAxisAddToPrimarySeriesName = " " + GetLocalizedString("PxWebChartLeftScale", settings.CurrentCulture);
                                       }
                                       else
                                       {
                                           c.ChartType = settings.ChartType;
                                       }

                                       if (settings.ChartType == SeriesChartType.Pyramid)
                                       {
                                           c.MaleValueNameForPopulationPyramid = FindPyramidFirstValue(model);
                                       }
                                       c.OverrideTitle = settings.Title;
                                       c.Width = settings.Width;
                                       c.Height = settings.Height;
                                       c.DecimalPlaces = model.Meta.ShowDecimals == -1 ? model.Meta.Decimals : model.Meta.ShowDecimals;
                                       c.ShowGuideLines = ((settings.Guidelines & ChartSettings.GuidelinesType.Vertical) == ChartSettings.GuidelinesType.Vertical);
                                       c.LegendHeightAsPctOfChart = settings.LegendHeight;
                                       if (settings.ChartType == SeriesChartType.Radar)
                                       {
                                           c.AutoMoveMostValuesVariableToX = true;
                                       }
                                       else
                                       {
                                           c.AutoMoveMostValuesVariableToX = false;
                                       }

                                       c.Font.Family = new System.Drawing.FontFamily(settings.FontName);
                                       c.Font.SizeSmall = settings.AxisFontSize;
                                       c.Font.SizeRegular = settings.TitleFontSize;
                                   }
            );

            foreach (ChartArea area in chart.ChartAreas)
            {
                // Horizontal guidelines
                area.AxisY.MajorGrid.Enabled = ((settings.Guidelines & ChartSettings.GuidelinesType.Horizontal) == ChartSettings.GuidelinesType.Horizontal);
                // Guideline color
                area.AxisY.MajorGrid.LineColor = System.Drawing.ColorTranslator.FromHtml(settings.GuidelinesColor);
                area.AxisX.MajorGrid.LineColor = System.Drawing.ColorTranslator.FromHtml(settings.GuidelinesColor);
                Color color = System.Drawing.ColorTranslator.FromHtml(settings.BackgroundColorGraphs);
                area.BackColor = Color.FromArgb(settings.ChartAlpha, color);     
            }

            //If line thickness is more than 0 draw a line around the chart
            if ( settings.LineThicknessPhrame > 0 )
            {
                chart.BorderlineWidth = settings.LineThicknessPhrame;
                chart.BorderlineColor = System.Drawing.ColorTranslator.FromHtml(settings.LineColorPhrame);
                chart.BorderlineDashStyle = ChartDashStyle.Solid;                

            }
                         
            
            // Legend
            //If line thickness on phrame is more then
            // 0 the backcolor is set to transparent.
            if (chart.Legends.Count > 0)
            {
                foreach (Legend l in chart.Legends)
                {
                    l.Enabled = settings.ShowLegend;
                    l.LegendStyle = LegendStyle.Table;
                    l.TableStyle = LegendTableStyle.Auto;
                    l.Docking = Docking.Bottom;
                    l.Font = new System.Drawing.Font(settings.FontName, settings.LegendFontSize);
                    if (!settings.ShowLegend)
                    {
                        foreach (ChartArea area in chart.ChartAreas)
                        {
                            area.Position.Height += l.Position.Height;
                            l.Position.Height = 0;
                        }
                    }
                    if (settings.LineThicknessPhrame > 0)
                    {
                        l.BackColor = System.Drawing.Color.Transparent;
                    }
                }
            }

            // Label orientation
            if (settings.LabelOrientation == ChartSettings.OrientationType.Vertical)
            {
                foreach (ChartArea area in chart.ChartAreas)
                {
                    area.AxisX.LabelStyle.Angle = 90;
                }
            }

            // Line thickness
            foreach (Series ser in chart.Series)
            {
                ser.BorderWidth = settings.LineThickness;
                if (settings.ChartType == SeriesChartType.Radar)
                {
                    ser["RadarDrawingStyle"] = "Line";
                }

            }

            // Chart colors
            List<Color> lstColors = new List<Color>();

            foreach (string col in settings.Colors)
            {
                lstColors.Add(ColorTranslator.FromHtml(col));
            }

            chart.Palette = ChartColorPalette.None;
            chart.PaletteCustomColors = lstColors.ToArray();

            Color backgroundColor = ColorTranslator.FromHtml(settings.BackgroundColor);
            chart.BackColor = Color.FromArgb(settings.BackgroundAlpha, backgroundColor);

            if (settings.BackgroundAlpha != 255)
            {
                chart.AntiAliasing = AntiAliasingStyles.Graphics;
            }

            return chart;

        }

        /// <summary>
        /// Get localized string
        /// </summary>
        /// <param name="key">key in language file</param>
        /// <param name="culture">Current culture</param>
        /// <returns></returns>
        public static string GetLocalizedString(string key, CultureInfo culture)
        {
            return PCAxis.Paxiom.Localization.PxResourceManager.GetResourceManager().GetString(key, culture);
        }

        /// <summary>
        /// Find first value in pyramid chart
        /// </summary>
        /// <param name="model">PXModel object</param>
        /// <returns>Text of the first value</returns>
        private static string FindPyramidFirstValue(PXModel model)
        {
            foreach (Variable var in model.Meta.Variables)
            {
                if (var.Values.Count == 2)
                {
                    return var.Values[0].Text;
                }
            }

            return "";
        }

    }
}
