using PCAxis.Web.Core;
using PCAxis.Web.Core.Management;
using System.Configuration;

namespace PCAxis.JsonStat.Web.Controls
{
    public class JsonStatFileType : FileTypeMarkerControlBase<JsonStatFileTypeCodebehind, JsonStatFileType>
    {
        public JsonStatFileType()
        {

        }

        public override void SerializeAndStream()
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                PCAxis.Serializers.JsonStatSerializer ser;
                ser = new PCAxis.Serializers.JsonStatSerializer();

                var geoVariablesStr = ConfigurationManager.AppSettings["geoVariables"];

                if (!string.IsNullOrEmpty(geoVariablesStr))
                {
                    ser.GeoVariableCodes = geoVariablesStr.Split(",".ToCharArray());
                }

                ser.Serialize(PaxiomManager.PaxiomModel, stream);
                StreamFile(stream, "application/json", "json");
            }
        }
    }
}
