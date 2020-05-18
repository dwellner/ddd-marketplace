using System;
using System.Threading.Tasks;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Service
{
    public static class ApplicationServiceExtensions
    {
        public static async Task HandleUpdate<T, TId>(this IApplicationService applicationService, IAggregateStore store, TId aggregateId, Action<T> operation)
            where T : AggregateRoot<TId> where TId : Value<TId> {

            var aggregate = await store.Load<T, TId>(aggregateId);
            if (aggregate == null) throw new InvalidOperationException($"Entity not found with id {aggregateId}");
            operation(aggregate);
            await store.Save<T,TId>(aggregate);
        }
    }
}
