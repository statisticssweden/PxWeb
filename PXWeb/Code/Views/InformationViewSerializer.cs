using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCAxis.Query;

namespace PXWeb.Views
{
    public class InformationViewSerializer : ScreenViewSerializerAdapter
    {
        public override PCAxis.Query.Output Save()
        {
            var output = new Output();
            output.Type = PxUrl.VIEW_INFORMATION_IDENTIFIER;
            
            var pxUrl = PXWeb.RouteInstance.PxUrlProvider.Create(null);
            string layout = pxUrl.Layout;

            output.Params.Add("layout", layout);

            return output;
        }

        public override void Render(string format, PCAxis.Query.SavedQuery query, PCAxis.Paxiom.PXModel model, bool safe)
        {
            RenderToScreen(query, model, "informationView", "InformationPresentation.aspx", safe);
        }
    }
}
