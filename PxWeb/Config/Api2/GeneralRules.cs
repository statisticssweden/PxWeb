using System;

namespace PxWeb.Config.Api2
{
    public class GeneralRules
    {
        public string Endpoint { get; set; } = "*";
        public int Limit { get; set; } = 1;
        public string Period { get; set; } = String.Empty;
    }
}
