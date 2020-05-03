using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Domain
{
    public abstract class Entity
    {
        readonly List<object> events = new List<object>();

        protected Entity() { }

        protected abstract void When(object @event);

        protected abstract void EnsureValidState();

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            events.Add(@event);
        }

        public IEnumerable<object> GetChanges() => events.AsEnumerable();



    }   
}
