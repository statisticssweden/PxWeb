using PCAxis.Query;

namespace PXWeb.Views
{
    public class SortedTableViewSerializer : ScreenViewSerializerAdapter
    {
        public override PCAxis.Query.Output Save()
        {
            var output = new Output();
            output.Type = PxUrl.VIEW_SORTEDTABLE_IDENTIFIER;

            var pxUrl = PXWeb.RouteInstance.PxUrlProvider.Create(null);
            string layout = pxUrl.Layout;

            output.Params.Add("layout", layout);

            return output;
        }

        public override void Render(string format, PCAxis.Query.SavedQuery query, PCAxis.Paxiom.PXModel model, bool safe)
        {
            RenderToScreen(query, model, "tableViewSorted", "DataSort.aspx", safe);
        }
    }
}
