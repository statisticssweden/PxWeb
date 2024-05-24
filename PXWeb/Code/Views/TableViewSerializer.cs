using PCAxis.Paxiom;
using PCAxis.Query;
using PCAxis.Web.Controls;
using System;

namespace PXWeb.Views
{
    public class TableViewSerializer : ScreenViewSerializerAdapter
    {
        public override Output Save()
        {
            var output = new Output();
            output.Type = PxUrl.VIEW_TABLE_IDENTIFIER;

            var pxUrl = PXWeb.RouteInstance.PxUrlProvider.Create(null);
            string layout = pxUrl.Layout;

            output.Params.Add("layout", layout);
            output.Params.Add("table_zerooption", TableManager.Settings.ZeroOption.ToString());

            return output;
        }

        public override void Render(string format, PCAxis.Query.SavedQuery query, PXModel model, bool safe)
        {
            TableManager.Settings.ZeroOption = CheckParameter(query, "table_zerooption") ? (PCAxis.Paxiom.ZeroOptionType)Enum.Parse(typeof(PCAxis.Paxiom.ZeroOptionType), query.Output.Params["table_zerooption"], true) : ZeroOptionType.ShowAll;

            RenderToScreen(query, model, "tableViewLayout1", "Table.aspx", safe);
        }


    }
}
