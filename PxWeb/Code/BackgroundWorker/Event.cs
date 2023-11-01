namespace PxWeb.Code.BackgroundWorker
{
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
