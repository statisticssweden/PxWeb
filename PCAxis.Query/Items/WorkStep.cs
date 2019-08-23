using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Query
{
    public class WorkStep
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Params { get; private set; }

        public WorkStep()
        {
            Params = new Dictionary<string, string>();
        }
    }
}
