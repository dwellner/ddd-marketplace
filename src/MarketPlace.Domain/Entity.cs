using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Domain
{
    public abstract class Entity
    {
        readonly List<object> events = new List<object>();

        protected void Raise(object @event) => events.Add(@event);

        protected Entity() { }

        public IEnumerable<object> GetChanges() => events.AsEnumerable();
    }
}
