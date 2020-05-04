using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Domain.Framework
{
    public abstract class AggregateRoot<TId> : IInternalEventHandler where TId : Value<TId>
    {
        readonly List<object> changes = new List<object>();

        public TId Id { get; protected set; }

        protected AggregateRoot() { }

        protected abstract void When(object @event);

        protected abstract void EnsureValidState();

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
