using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

namespace MarketPlace
{
    public class EventStoreService : IHostedService
    {
        private readonly IEventStoreConnection connection;
        private readonly Service.ProjectionsManager projectionsManager;

        public EventStoreService(IEventStoreConnection connection, Service.ProjectionsManager projectionsManager)
        {
            this.connection = connection;
            this.projectionsManager = projectionsManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            await connection.ConnectAsync();
            projectionsManager.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            projectionsManager.Stop();
            connection.Close();
            return Task.CompletedTask;
        }
    }
}
