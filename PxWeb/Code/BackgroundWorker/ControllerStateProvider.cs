using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System;

namespace PxWeb.Code.BackgroundWorker
{
    public class ControllerStateProvider : IControllerStateProvider
    {
        private Dictionary<string, IControllerState> _states;

        public ControllerStateProvider()
        {
            _states = new Dictionary<string, IControllerState>();
        }
        public IControllerState Load(string id)
        {
            if (_states.ContainsKey(id)) return _states[id]; // Get from memory

            // Get from file
            string fileName = GetFileName(id);

            ControllerState state;
            if (!File.Exists(fileName))
            {
                state = new ControllerState(id, fileName, null);
            }
            else
            {
                try
                {
                    string text = File.ReadAllText(fileName);
                    var stateData = JsonSerializer.Deserialize<ControllerStateData>(text);
                    state = new ControllerState(id, fileName, stateData);
                }
                catch (Exception)
                {
                    state = new ControllerState(id, fileName, null);
                }

            }

            _states[id] = state;
            return state;
        }

        private string GetFileName(string id)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ControllerStates");
            if (!Directory.Exists(directoryPath)) { Directory.CreateDirectory(directoryPath); }
            return Path.Combine(directoryPath ,$"{id}.json");
        }
    }
}
