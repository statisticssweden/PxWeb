using System.Collections.Generic;

namespace PxWeb.Models.Api2
{
    public class Feature
    {
        public string Id { get; set; }

        public List<Param> Params { get; set; }
    }
}
