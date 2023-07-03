using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace PxWeb.Code.BackgroundWorker
{
    public class LongRunningService : BackgroundService
    {
        private readonly BackgroundWorkerQueue queue;
        private IControllerStateProvider _stateProvider;

        public LongRunningService(IControllerStateProvider stateProvider, BackgroundWorkerQueue queue)
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

                IControllerState state = _stateProvider.Load(id);

                state.Begin();

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception e)
                {
                    state.AddEvent(new Event("Error", e.Message));
                }

                state.End();
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
