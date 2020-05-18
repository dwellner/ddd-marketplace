using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Domain.Framework
{
    public abstract class AggregateRoot<TId> : IInternalEventHandler where TId : Value<TId>
    {
        public int Version { get; private set; } = -1;
        readonly List<object> changes = new List<object>();
        public void ClearChanges() => changes.Clear();

        public TId Id { get; protected set; }

        protected AggregateRoot() { }

        protected abstract void When(object @event);

        protected abstract void EnsureValidState();

        public void Load(IEnumerable<object> events) => events.ToList().ForEach(@event => {
                When(@event);
                Version++;
            });

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            changes.Add(@event);
        }

        public IEnumerable<object> GetChanges() => changes.AsEnumerable();

        protected void ApplyToEntity(IInternalEventHandler entity, object @event) => entity?.Handle(@event);

        void IInternalEventHandler.Handle(object @event) => When(@event);

    }   
}
