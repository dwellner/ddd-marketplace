using System;
namespace MarketPlace.Domain.Framework
{
    public interface IInternalEventHandler
    {
        void Handle(object @event);
    }
}
