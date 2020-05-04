using System;

namespace MarketPlace.Domain.Framework
{
    public abstract class Entity<TId>: IInternalEventHandler
    {
        private readonly Action<object> applier;

        internal TId Id { get; set; }

        protected Entity(Action<object> applier) {
            this.applier = applier;
        }

        protected abstract void When(object @event);

        protected abstract void EnsureValidState();

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            applier(@event);
        }

        void IInternalEventHandler.Handle(object @event) => When(@event);
    }   
}
