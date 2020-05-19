using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using MarketPlace.Domain.Framework;
using Newtonsoft.Json;

namespace MarketPlace.Service
{
    public class EventStoreAggregateStore : IAggregateStore
    {
        private static string GetStreamName<T, TId>(TId aggregateId) =>
            $"{typeof(T).Name}-{aggregateId}";
        private static string GetStreamName<T, TId>(T aggregate)
            where T : AggregateRoot<TId> where TId : Value<TId> =>
            $"{typeof(T).Name}-{aggregate.Id}";

        private static byte[] Serialize(object data) =>
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        private IEnumerable<EventData> GetChanges<T, TId>(T aggregate)
            where T : AggregateRoot<TId> where TId : Value<TId> =>
            aggregate.GetChanges().Select(@event => new EventData(
                eventId: Guid.NewGuid(),
                type: @event.GetType().Name,
                isJson: true,
                data: Serialize(@event),
                metadata: Serialize(new EventMetaData {
                    ClrType = @event.GetType().AssemblyQualifiedName
                })));

        private readonly IEventStoreConnection connection;

        public EventStoreAggregateStore(IEventStoreConnection connection)
        {
            this.connection = connection;
        }

        public async Task Save<T, TId>(T aggregate) where T : AggregateRoot<TId> where TId : Value<TId>
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));
            var changes = GetChanges<T, TId>(aggregate).ToArray();
            if (!changes.Any()) return;

            await connection.AppendToStreamAsync(
                GetStreamName<T, TId>(aggregate),
                aggregate.Version,
                changes);
            aggregate.ClearChanges();
        }

        public async Task<bool> Exists<T, TId>(TId aggregateId)
        {
            if (aggregateId == null) throw new ArgumentNullException(nameof(aggregateId));
            var result = await connection.ReadEventAsync(GetStreamName<T, TId>(aggregateId), 1, false);
            return result.Status != EventReadStatus.NoStream;
        }


        private async Task ApplyAll(string streamName, Action<ResolvedEvent[]> apply, int start = 0) {
            var page = await connection.ReadStreamEventsForwardAsync(streamName, start, 1024, false);
            apply(page.Events);
            if (!page.IsEndOfStream) await ApplyAll(streamName, apply, start + 1024);
        }
        
        public async Task<T> Load<T, TId>(TId aggregateId) where T : AggregateRoot<TId> where TId : Value<TId>
        {
            if (aggregateId == null) throw new ArgumentNullException(nameof(aggregateId));
            var aggregate = (T)Activator.CreateInstance(typeof(T), true);


            await ApplyAll(GetStreamName<T, TId>(aggregateId), events =>
                aggregate.Load(events.Select(e => e.Deserialize())));
            return aggregate;
        }
    }
}
