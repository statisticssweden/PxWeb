using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace PxWeb.Code.BackgroundWorker
{
    public class StateProvider : IStateProvider
    {
        private Dictionary<string, ResponseState> _states;

        public StateProvider()
        {
            _states = new Dictionary<string, ResponseState>();
        }
        public ResponseState Load(string id)
        {
            if (_states.ContainsKey(id)) return _states[id]; // Get from memory

            // Get from file
            string fileName = getFileName(id);

            ResponseState state;
            if (!File.Exists(fileName))
            {
                state = new ResponseState();
            }
            else
            {
                try
                {
                    string text = File.ReadAllText(fileName);
                    state = JsonSerializer.Deserialize<ResponseState>(text);
                    if (state is null) state = new ResponseState();
                }
                catch (JsonException)
                {
                    state = new ResponseState();
                }

            }

            _states[id] = state;
            return state;
        }

        public void Save(string id, ResponseState state)
        {
            _states[id] = state;

            string json = JsonSerializer.Serialize(state);
            string fileName = getFileName(id);
            File.WriteAllText(fileName, json);
        }

        private string getFileName(string id)
        {  
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ControllerStates" ,$"{id}.json");
        }
    }
}
