using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;

namespace PxWeb.Code.BackgroundWorker
{
    public class LongRunningService : BackgroundService
    {
        private readonly BackgroundWorkerQueue queue;
        private IStateProvider _stateProvider;

        public LongRunningService(IStateProvider stateProvider, BackgroundWorkerQueue queue)
        {
            this.queue = queue;
            _stateProvider = stateProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await queue.DequeueAsync(stoppingToken);
                string id = getControllerIdFromTask(workItem);

                ResponseState state = _stateProvider.Load(id);

                state.Begin();
                await workItem(stoppingToken);
                state.End();
                _stateProvider.Save(id, state);
            }
        }

        private string getControllerIdFromTask(System.Func<CancellationToken, Task> workItem)
        {
            var type = workItem.Method.DeclaringType;
            while (type.DeclaringType != null) { type = type.DeclaringType; } // Avoid compiler generated classes
            return type.FullName;
        }
    }
}
