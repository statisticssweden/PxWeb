using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Web.Core;
using PCAxis.Web.Core.Management;
using PCAxis.Web.Controls;
using PCAxis.Paxiom;
using System.Configuration;

namespace PCAxis.JsonStat2.Web.Controls
{
    public class JsonStat2FileType : FileTypeMarkerControlBase<JsonStat2FileTypeCodebehind, JsonStat2FileType>
    {
        public JsonStat2FileType()
        {

        }

        public override void SerializeAndStream()
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                PCAxis.Serializers.JsonStat2.JsonStat2Serializer ser;
                ser = new Serializers.JsonStat2.JsonStat2Serializer();

                //var geoVariablesStr = ConfigurationManager.AppSettings["geoVariables"];

                //if (!string.IsNullOrEmpty(geoVariablesStr))
                //{
                //    ser.GeoVariableCodes = geoVariablesStr.Split(",".ToCharArray());
                //}

                ser.Serialize(PaxiomManager.PaxiomModel, stream);
                StreamFile(stream, "application/json", "json");
            }
        }
    }
}
