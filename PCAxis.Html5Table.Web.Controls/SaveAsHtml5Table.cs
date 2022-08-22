using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Web.Core;
using PCAxis.Web.Core.Management;
using PCAxis.Web.Controls;
using PCAxis.Paxiom;
using System.Configuration;

namespace PCAxis.Html5Table.Web.Controls
{
    public class SaveAsHtml5Table : FileTypeMarkerControlBase<SaveAsHtml5TableCodebehind, SaveAsHtml5Table>
    {
        public SaveAsHtml5Table()
        {

        }

        public override void SerializeAndStream()
        {
			using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
			{
                PCAxis.Serializers.Html5TableSerializer ser;
				ser = new PCAxis.Serializers.Html5TableSerializer();


				ser.Serialize(PaxiomManager.PaxiomModel, stream);
				StreamFile(stream, "text/html", "htm");
			}
		}
    }
}
