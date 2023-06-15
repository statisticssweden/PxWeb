using System;
using System.Collections.Generic;

namespace PxWeb.Code.BackgroundWorker
{
    public class ResponseState
    {
        public string State { get; set; } = "Not run";
        public DateTime? LastRun { get; set; }
        public List<Event> EventsFromLastRun { get; set; } = new List<Event>();

        public void AddEvent(Event e)
        {
            EventsFromLastRun.Add(e);
        }
        public void Begin()
        {
            EventsFromLastRun = new List<Event>();
            AddEvent(new Event("Information", $"Started at {DateTime.UtcNow}"));
            State = "Running";
        }
        public void End()
        {
            LastRun = DateTime.UtcNow;
            AddEvent(new Event("Information", $"Finished at {LastRun}"));
            State = "Finished";
        }
    }

    public class Event
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public Event(string type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}
