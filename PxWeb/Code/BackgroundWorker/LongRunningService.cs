using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;

namespace PxWeb.Code.BackgroundWorker
{
    public class LongRunningService : BackgroundService
    {
        private readonly BackgroundWorkerQueue queue;

        public LongRunningService(BackgroundWorkerQueue queue)
        {
            this.queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await queue.DequeueAsync(stoppingToken);

                await workItem(stoppingToken);
            }
        }
    }
}
