using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using PCAxis.Charting;
using PCAxis.Paxiom;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;

namespace PCAxis.Chart
{
    public class PxWebChart : PCAxis.Charting.PxChart
    {
        /// <summary>
        /// Units (text for Y-axis for column chart)
        /// </summary>
        private string _units = "";

        /// <summary>
        /// Variable (text for X-axis for column chart)
        /// </summary>
        private string _variable = "";

        /// <summary>
        /// Current culture
        /// </summary>
        public CultureInfo CurrentCulture { get; set; }

        /// <summary>
        /// Current settings
        /// </summary>
        public ChartSettings Settings { get; set; }

        public PxWebChart(PXModel px, ChartSettings settings, PxChartSettings settingsAsLambda)
            : base(px, settingsAsLambda)
        {
            CurrentCulture = settings.CurrentCulture;
            Settings = settings;

            // Source text
            if (!String.IsNullOrEmpty(px.Meta.Source))
            {
                // Use Source from model if it exists...
                InfoText = ChartHelper.GetLocalizedString("PxWebChartsSourceLabel", CurrentCulture) + " " + px.Meta.Source;
            }
            else
            {
                // ...else use localized text
                InfoText = ChartHelper.GetLocalizedString("PxWebChartsSourceLabel", CurrentCulture) + " " + ChartHelper.GetLocalizedString("PxWebChartsSource", CurrentCulture);
            }
            
        }

        /// <summary>
        /// Extract metadata from Paxiom model
        /// </summary>
        /// <param name="px">PXModel object</param>
        protected override PXModel initializePxData(PXModel px)
        {
            base.EliminateAllSingleValueVariables = true;
            px = base.initializePxData(px);

            // Get text for X- and Y-axis
            _units = px.Meta.ContentInfo.Units;
            _variable = px.Meta.Variables.Last().Name;

            return px;
        }

        /// <summary>
        /// Draw additional graphics in chart
        /// </summary>
        /// <param name="graphics"></param>
        protected override void additionalGraphics(Graphics graphics)
        {
            System.Drawing.Image img = null;
            
            int borderThickness = 0;

            if (Settings != null &&  Settings.LineThicknessPhrame != null && Settings.LineThicknessPhrame > 0)
            {
                borderThickness = Settings.LineThicknessPhrame;
            }


            if (Settings != null && Settings.ShowLogo && Settings.LogotypePath != "" && Settings.Logotype != "")
            {

                try
                {

                    String appPath = GetAppPath();
                    String loggtypePath = Settings.LogotypePath.ToString();

                    if (loggtypePath.StartsWith("~"))
                    {
                        loggtypePath = loggtypePath.Remove(0,1);
                    
                    }

                    if (loggtypePath.StartsWith("/") && appPath.ToString().EndsWith("/"))
                    {
                        appPath = appPath.ToString().Remove(appPath.ToString().Length - 1);
                    }

                    System.Net.WebRequest req = System.Net.WebRequest.Create(appPath + loggtypePath + Settings.Logotype);
                    System.Net.WebResponse response = req.GetResponse();
                    System.IO.Stream stream = response.GetResponseStream();

                    img = System.Drawing.Image.FromStream(stream);
                    System.Drawing.Size x = img.Size;
                    stream.Close();

                    graphics.DrawImage(img, 2 + borderThickness, Height - img.Height - 2 - borderThickness, img.Width, img.Height);
                    response.Close();

                }
                catch (System.Exception ex)
                {

                }

            }
            // Source text in bottom left corner
            if (Settings != null)
            {
                if (InfoText != "" && Settings.ShowSource)
                {
                    graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    int floatX = 3;
                    Font f = new Font(Font.Small, FontStyle.Italic);

                    if (img != null && Settings.ShowLogo)
                    {
                        floatX += img.Width;

                    }

                    graphics.DrawString(
                       InfoText,
                       f,
                       Font.InfoTextBrush,
                       floatX + borderThickness,
                       Height - 5 - Font.Small.Height - borderThickness
                    );
                }
            }


            WriteTopAxisText(graphics);
        }

        private void WriteTopAxisText(Graphics graphics)
        {
             
            float x;
            ChartArea ca = ChartAreas.First();

            // Change axis text?
            switch (Series.First().ChartType)
            {
                case SeriesChartType.StackedArea100:
                case SeriesChartType.StackedColumn100:
                case SeriesChartType.StackedBar100:
                    _units = ChartHelper.GetLocalizedString("PxWebChartPercent", CurrentCulture);
                    break;
                default:
                    break;
            }

            switch (Series.First().ChartType)
            {
                case SeriesChartType.Bar:
                case SeriesChartType.StackedBar:
                case SeriesChartType.StackedBar100:
                    x = (float)ca.AxisY.ValueToPixelPosition(ca.AxisY.Minimum);

                    if (ca.AxisX.CustomLabels.Count > 0)
                    {
                        CustomLabel tl = ca.AxisX.CustomLabels.Last();
                        double topGrid = tl.FromPosition + (tl.ToPosition - tl.FromPosition) / 2;

                        float y = (float)ca.AxisX.ValueToPixelPosition(topGrid) - 17;

                        graphics.DrawString(_variable, Font.Small, Brushes.Black, x, y);
                    }
                    break;
                default:
                    x = (float)ca.AxisX.ValueToPixelPosition(ca.AxisX.Minimum);

                    if (ca.AxisY.CustomLabels.Count > 0)
                    {
                        CustomLabel tl = ca.AxisY.CustomLabels.Last();
                        double topGrid = tl.FromPosition + (tl.ToPosition - tl.FromPosition) / 2;

                        float y = (float)ca.AxisY.ValueToPixelPosition(topGrid) - 17;

                        graphics.DrawString(_units, Font.Small, Brushes.Black, x, y);
                    }
                    break;
            }

        }

        protected override void additionalFunctionalityLast()
        {
            base.additionalFunctionalityLast();

            switch (Series.First().ChartType)
            {
                case SeriesChartType.Radar:
                    break;
                case SeriesChartType.Bar:
                case SeriesChartType.StackedBar:
                    ChartAreas[0].AxisY.Title = _units;
                    ChartAreas[0].AxisY.TitleFont = Font.Small;
                    ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Far;
                    break;
                case SeriesChartType.StackedBar100:
                    ChartAreas[0].AxisY.Title = ChartHelper.GetLocalizedString("PxWebChartPercent", CurrentCulture);
                    ChartAreas[0].AxisY.TitleFont = Font.Small;
                    ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Far;
                    break;
                default:
                    ChartAreas[0].AxisX.Title = _variable;
                    ChartAreas[0].AxisX.TitleFont = Font.Small;
                    ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Far;
                    break;
            }
        }

        protected override void addTitle()
        {
            Titles.Add(
               new Title(Title, Docking.Top)
               {
                   Position = new ElementPosition(0, 0, 100, 14),
                   Alignment = ContentAlignment.TopLeft,
                   Font = Font.Regular,
                   Text = Title
               }
            );
        }

        protected override void addChartArea()
        {
            //int chartSpace = showLegend ? 86 - LegendPctOfChartHeight : 86;
            //chartSpace -= InfoTextSpace;

            int chartSpace = showLegend ? 86 - (int)LegendHeightAsPctOfChart : 86;
            chartSpace -= (int)SpaceUsedForInfoText;

            ChartArea a =
               new ChartArea("Main")
               {
                   Position = new ElementPosition(0, 12, 100, chartSpace),
                   AxisX = templateAxisX,
                   AxisY = templateAxisY
               };

            ChartAreas.Add(a);
        }

        /// <summary>
        /// Add legend
        /// </summary>
        protected override void addLegend()
        {           
            if (showLegend)
            {
                LegendItemOrder order = LegendItemOrder.ReversedSeriesOrder;

                if ((ChartTypes.Length == 1) && (ChartTypes[0] == SeriesChartType.Pie))
                {
                    order = LegendItemOrder.SameAsSeriesOrder;
                }

                Legends.Add(
                   new Legend("Main")
                   {
                       Position = new ElementPosition(0, 98 - legendHeight - SpaceUsedForInfoText - 1, 100, legendHeight),
                       LegendItemOrder = order
                   }
                );
            }
        }


        private String  GetAppPath ()
        {
            String appPath = string.Empty;
            System.Web.HttpContext context = System.Web.HttpContext.Current; 

            appPath = String.Format("{0}://{1}{2}{3}", 
                                    context.Request.Url.Scheme,
                                    context.Request.Url.Host,
                                    context.Request.Url.Port.Equals(80) ? String.Empty : ":" + context.Request.Url.Port.ToString(), 
                                    context.Request.ApplicationPath);
            return appPath;
        }
    }
}
