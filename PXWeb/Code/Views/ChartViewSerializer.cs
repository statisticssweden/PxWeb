using PCAxis.Query;
using PCAxis.Web.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCAxis.Chart;

namespace PXWeb.Views
{
    public class ChartViewSerializer : ScreenViewSerializerAdapter
    {
        public override PCAxis.Query.Output Save()
        {
            var output = new Output();
            output.Type = PxUrl.VIEW_CHART_IDENTIFIER;

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

            RenderToScreen(query, model, "chartViewColumn", "Chart.aspx", safe);
        }
    }
}
