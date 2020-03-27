using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCAxis.Query;
using PCAxis.Paxiom;
using PCAxis.Web.Core.Management;
using PCAxis.Web.Controls;
using PXWeb.Views;

namespace PXWeb.Views
{
    public class SelectionViewSerializer : ScreenViewSerializerAdapter
    {
        public override void Render(string format, PCAxis.Query.SavedQuery query, PXModel model, bool safe)
        {
            RenderToScreen(query, model, "", "Selection.aspx", safe);
        }

        public override Output Save()
        {
            var output = new Output();
            output.Type = PxUrl.PAGE_SELECT;

            var pxUrl = PXWeb.RouteInstance.PxUrlProvider.Create(null);

            return output;
        }
    }
}