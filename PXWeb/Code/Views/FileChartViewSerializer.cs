using PCAxis.Chart;
using PCAxis.Query;
using PCAxis.Web.Controls;
using System;

namespace PXWeb.Views
{
    public class FileChartViewSerializer : FileViewSerializer
    {
        public FileChartViewSerializer(string fileFormat)
        {
            _fileFormat = fileFormat;
        }

        public override PCAxis.Query.Output Save()
        {
            Output output = new Output();
            output.Type = _fileFormat;

            var pxUrl = PXWeb.RouteInstance.PxUrlProvider.Create(null);
            string layout = pxUrl.Layout;

            output.Params.Add("layout", layout);
            output.Params.Add("chart_title", ChartManager.Settings.Title);
            output.Params.Add("chart_width", ChartManager.Settings.Width.ToString());
            output.Params.Add("chart_height", ChartManager.Settings.Height.ToString());
            output.Params.Add("chart_linethickness", ChartManager.Settings.LineThickness.ToString());
            output.Params.Add("chart_timesortorder", ChartManager.Settings.TimeSortOrder.ToString());
            output.Params.Add("chart_labelorientation", ChartManager.Settings.LabelOrientation.ToString());
            output.Params.Add("chart_guidelines", ChartManager.Settings.Guidelines.ToString());
            output.Params.Add("chart_showlegend", ChartManager.Settings.ShowLegend.ToString());
            output.Params.Add("chart_legendheight", ChartManager.Settings.LegendHeight.ToString());

            return output;
        }

        public override void Render(string format, PCAxis.Query.SavedQuery query, PCAxis.Paxiom.PXModel model, bool safe)
        {
            if (query.Output.Params["layout"] == ChartTypes.CHART_COLUMNLINE)
            {
                ChartManager.Settings.IsColumnLine = true;
            }
            ChartManager.Settings.ChartType = ChartSettings.ConvertToChartType(query.Output.Params["layout"], System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column);
            ChartManager.Settings.UseSettingTitle = true;
            //Custom chart title only works for the language that was selected when the saved query was created.
            if (query.Sources[0].Language.ToLower() == model.Meta.CurrentLanguage.ToLower())
            {
                ChartManager.Settings.Title = CheckParameter(query, "chart_title") ? query.Output.Params["chart_title"] : ChartManager.Settings.Title;
            }
            else
            {
                ChartManager.Settings.Title = model.Meta.Title;
            }
            ChartManager.Settings.Width = CheckParameter(query, "chart_width") ? int.Parse(query.Output.Params["chart_width"]) : ChartManager.Settings.Width;
            ChartManager.Settings.Height = CheckParameter(query, "chart_height") ? int.Parse(query.Output.Params["chart_height"]) : ChartManager.Settings.Height;
            ChartManager.Settings.LineThickness = CheckParameter(query, "chart_linethickness") ? int.Parse(query.Output.Params["chart_linethickness"]) : ChartManager.Settings.LineThickness;
            ChartManager.Settings.TimeSortOrder = CheckParameter(query, "chart_timesortorder") ? (ChartSettings.SortType)Enum.Parse(typeof(ChartSettings.SortType), query.Output.Params["chart_timesortorder"], true) : ChartManager.Settings.TimeSortOrder;
            ChartManager.Settings.LabelOrientation = CheckParameter(query, "chart_labelorientation") ? (ChartSettings.OrientationType)Enum.Parse(typeof(ChartSettings.OrientationType), query.Output.Params["chart_labelorientation"], true) : ChartManager.Settings.LabelOrientation;
            ChartManager.Settings.Guidelines = CheckParameter(query, "chart_guidelines") ? (ChartSettings.GuidelinesType)Enum.Parse(typeof(ChartSettings.GuidelinesType), query.Output.Params["chart_guidelines"], true) : ChartManager.Settings.Guidelines;
            ChartManager.Settings.ShowLegend = CheckParameter(query, "chart_showlegend") ? bool.Parse(query.Output.Params["chart_showlegend"]) : ChartManager.Settings.ShowLegend;
            ChartManager.Settings.LegendHeight = CheckParameter(query, "chart_legendheight") ? int.Parse(query.Output.Params["chart_legendheight"]) : ChartManager.Settings.LegendHeight;

            base.Render(format, query, model, safe, GetFileExtension(format), GetMimeType(format));
        }

        private string GetFileExtension(string outputType)
        {
            switch (outputType)
            {
                case PCAxis.Web.Controls.Plugins.FileFormats.CHART_GIF:
                    return "gif";
                case PCAxis.Web.Controls.Plugins.FileFormats.CHART_JPEG:
                    return "jpeg";
                case PCAxis.Web.Controls.Plugins.FileFormats.CHART_PNG:
                    return "png";
                default:
                    return "png";
            }

        }

        private string GetMimeType(string outputType)
        {
            switch (outputType)
            {
                case PCAxis.Web.Controls.Plugins.FileFormats.CHART_GIF:
                    return "image/gif";
                case PCAxis.Web.Controls.Plugins.FileFormats.CHART_JPEG:
                    return "image/jpeg";
                case PCAxis.Web.Controls.Plugins.FileFormats.CHART_PNG:
                    return "image/png";
                default:
                    return "image/png";
            }
        }

        /// <summary>
        /// Check that the parameter exists and that it has a value
        /// </summary>
        /// <param name="query">Query object</param>
        /// <param name="key">Key for the parameter</param>
        /// <returns>True if the parameter exists and has a value, else false</returns>
        protected bool CheckParameter(PCAxis.Query.SavedQuery query, string key)
        {
            if (query.Output.Params.ContainsKey(key))
            {
                if (query.Output.Params[key] != null)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
