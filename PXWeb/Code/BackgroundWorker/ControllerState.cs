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
        private ControllerStateData _data;

        public ControllerStateData Data { get { return _data; } }


        public ControllerState(string id, string fileSaveLocation, ControllerStateData? data)
        {
            _id = id;
            _fileSaveLocation = fileSaveLocation;
            _data = data ?? new ControllerStateData();
            
        }

        public void AddEvent(Event e)
        {
            _data.EventsFromLastRun.Add(e);
            Save();
        }
        public void Begin()
        {
            _data.EventsFromLastRun = new List<Event>();
            _data.State = ControllerStateType.Running;
            AddEvent(new Event("Information", $"Started at {DateTime.UtcNow}"));
        }
        public void End()
        {
            _data.LastRunTime = DateTime.UtcNow;
            _data.State = ControllerStateType.Finished;
            AddEvent(new Event("Information", $"Finished at {_data.LastRunTime}"));
        }
        private void Save()
        {
            string json = JsonSerializer.Serialize(_data);
            File.WriteAllText(_fileSaveLocation, json);
        }
    }
}
