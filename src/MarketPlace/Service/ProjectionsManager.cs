using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using MarketPlace.Domain.ClassifiedAd;
using Serilog;

namespace MarketPlace.Service
{

    public class ProjectionsManager
    {
        private static readonly Serilog.ILogger log = Log.ForContext<ProjectionsManager>();

        private readonly IEventStoreConnection connection;
        private readonly List<IProjection> projections;
        private EventStoreAllCatchUpSubscription subscription;

        public ProjectionsManager(IEventStoreConnection connection, IEnumerable<IProjection> projections) 
        {
            this.connection = connection;
            this.projections = projections.ToList();
        }

        public void Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000, 500,
                Log.IsEnabled(Serilog.Events.LogEventLevel.Verbose), false, "try-out-subscription");
            subscription = connection.SubscribeToAllFrom(Position.Start, settings, EventAppeared);
        }

        public void Stop() => subscription?.Stop();

        private async Task EventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$")) return;
            var @event = resolvedEvent.Deserialize();
            log.Debug($"Projecting event: {@event.GetType().Name}");

            await Task.WhenAll(projections.Select(projection => projection.Project(@event)));
        }

    }
}
