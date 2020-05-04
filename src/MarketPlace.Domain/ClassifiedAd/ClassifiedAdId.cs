using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.ClassifiedAd
{
    public class ClassifiedAdId : IdValue<ClassifiedAdId,Guid>
    {
        public ClassifiedAdId(Guid value) : base(value) { }
    }
}
