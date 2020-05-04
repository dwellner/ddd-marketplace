using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain
{
    public class UserId : IdValue<UserId, Guid>
    {
        public UserId(Guid value): base(value) { }
    }
}
