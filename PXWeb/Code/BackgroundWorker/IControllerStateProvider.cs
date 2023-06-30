namespace PxWeb.Code.BackgroundWorker
{
    public interface IControllerStateProvider
    {
        public IControllerState Load(string id);
    }
}
