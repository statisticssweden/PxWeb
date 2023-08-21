using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace PxWeb.Code.BackgroundWorker
{
    public class ControllerStateData
    {
        public ControllerStateType State { get; set; } = ControllerStateType.NotRun;
        public DateTime? LastRunTime { get; set; }
        public List<Event> EventsFromLastRun { get; set; } = new List<Event>();

    }
}
