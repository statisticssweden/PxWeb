using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCAxis.Paxiom;

namespace PX.Json.Web.Controls
{
    public class JsonSerializerCreator : PCAxis.Web.Core.ISerializerCreator
    {
        public IPXModelStreamSerializer Create(string fileInfo)
        {
            PCAxis.Serializers.JsonSerializer ser;
            ser = new PCAxis.Serializers.JsonSerializer();

            return ser;
        }
    }
}
