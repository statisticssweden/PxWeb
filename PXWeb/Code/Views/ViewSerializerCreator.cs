using System.Web;
using System.Web.UI;

namespace PXWeb.Views
{
    public class ViewSerializerCreator
    {
        public static IViewSerializer GetSerializer()
        {
            Page page = HttpContext.Current.Handler as System.Web.UI.Page;
            return GetSerializer(PxUrl.GetView(page));
        }

        public static IViewSerializer GetSerializer(string type)
        {
            switch (type)
            {
                case PxUrl.PAGE_SELECT:
                    return new SelectionViewSerializer();
                case PxUrl.VIEW_TABLE_IDENTIFIER:
                    return new TableViewSerializer();
                case PxUrl.VIEW_CHART_IDENTIFIER:
                    return new ChartViewSerializer();
                case PxUrl.VIEW_FOOTNOTES_IDENTIFIER:
                    return new TableViewSerializer();
                case PxUrl.VIEW_INFORMATION_IDENTIFIER:
                    return new InformationViewSerializer();
                case PxUrl.VIEW_SORTEDTABLE_IDENTIFIER:
                    return new SortedTableViewSerializer();
                case PCAxis.Web.Controls.Plugins.FileFormats.CHART_PNG:
                case PCAxis.Web.Controls.Plugins.FileFormats.CHART_JPEG:
                case PCAxis.Web.Controls.Plugins.FileFormats.CHART_GIF:
                    return new FileChartViewSerializer(type);
                default:
                    return new FileViewSerializer(type);
            }
        }
    }
}