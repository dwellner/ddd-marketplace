using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.ClassifiedAd
{
    public class PictureId : IdValue<PictureId, Guid>
    {
        public PictureId(Guid value) : base(value) { }
    }
}
