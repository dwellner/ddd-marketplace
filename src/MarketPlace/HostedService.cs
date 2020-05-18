using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

namespace MarketPlace
{
    public class HostedService: IHostedService
    {
        private readonly IEventStoreConnection connection;

        public HostedService(IEventStoreConnection connection) => this.connection = connection;

        public Task StartAsync(CancellationToken cancellationToken) => connection.ConnectAsync();

        public Task StopAsync(CancellationToken cancellationToken)
        {
            connection.Close();
            return Task.CompletedTask;
        }
    }
}
