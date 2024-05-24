using PCAxis.Web.Core;
using PCAxis.Web.Core.Management;


namespace PX.Json.Web.Controls
{
    public class SaveAsJson : FileTypeMarkerControlBase<SaveAsJsonCodebehind, SaveAsJson>
    {
        public SaveAsJson()
        {

        }

        public override void SerializeAndStream()
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                PCAxis.Serializers.JsonSerializer ser;
                ser = new PCAxis.Serializers.JsonSerializer();


                ser.Serialize(PaxiomManager.PaxiomModel, stream);
                StreamFile(stream, "application/json", "json");

            }
        }
    }
}
