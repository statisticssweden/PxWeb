using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace PCAxis.Query
{
    public static class JsonHelper
    {
        private static string SerializeJson(object obj, bool prettyPrint)
        {
            return JsonConvert.SerializeObject(obj, prettyPrint ? Formatting.Indented : Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string ToJSON(this object obj, bool prettyPrint)
        {
            return SerializeJson(obj, prettyPrint);
        }

        public static object Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}