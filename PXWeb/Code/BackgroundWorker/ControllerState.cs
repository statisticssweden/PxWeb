using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PxWeb.Code.BackgroundWorker
{
    public class ControllerState : IControllerState
    {
        private string _id;
        private string _fileSaveLocation;
        public ControllerStateType State { get; set; } = ControllerStateType.NotRun;
        public DateTime? LastRunTime { get; set; }
        public List<Event> EventsFromLastRun { get; set; } = new List<Event>();

        public ControllerState(string id, string fileSaveLocation)
        {
            _id = id;
            _fileSaveLocation = fileSaveLocation;
        }

        public void AddEvent(Event e)
        {
            EventsFromLastRun.Add(e);
            Save();
        }
        public void Begin()
        {
            EventsFromLastRun = new List<Event>();
            State = ControllerStateType.Running;
            AddEvent(new Event("Information", $"Started at {DateTime.UtcNow}"));
        }
        public void End()
        {
            LastRunTime = DateTime.UtcNow;
            State = ControllerStateType.Finished;
            AddEvent(new Event("Information", $"Finished at {LastRunTime}"));
        }
        private void Save()
        {
            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(_fileSaveLocation, json);
        }
    }
}
