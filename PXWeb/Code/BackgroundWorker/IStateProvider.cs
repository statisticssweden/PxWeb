namespace PxWeb.Code.BackgroundWorker
{
    public interface IStateProvider
    {
        public ResponseState Load(string id);
        public void Save(string id, ResponseState state);
    }
}
