using System;
using System.Collections.Generic;

namespace PxWeb.Config.Api2
{
    public class Feature
    {
        public string Id { get; set; } = String.Empty;
        public List<Param> Params { get; set; } = new List<Param>();
    }
}
