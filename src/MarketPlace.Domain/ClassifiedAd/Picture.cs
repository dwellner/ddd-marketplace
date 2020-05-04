using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.ClassifiedAd
{
    public class PictureId : IdValue<PictureId, Guid>
    {
        public PictureId(Guid value) : base(value) { }
    }

    public class PictureSize: Value<PictureSize>
    {
        public int Width { get; internal set; }
        public int Height { get; internal set; }

        protected override object[] Values => new object[] { Width, Height };

        public PictureSize(int width, int height)
        {
            Width = (width > 0) ? width : throw new ArgumentException("Image width must be greater than zero", nameof(width));
            Height = (height> 0) ? height : throw new ArgumentException("Image height must be greater than zero", nameof(height));
        }

        internal PictureSize() { }
    }

    public class Picture : Entity<PictureId>
    {
        internal PictureSize pictureSize { get; set; }
        internal Uri Location { get; set; }
        internal int Order { get; set; }

        public Picture(Action<object> applier) : base(applier)
        {
        }

        protected override void EnsureValidState()
        {
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddedToClassifiedAd e:
                    Id = new PictureId(e.PictureId);
                    Location = new Uri(e.Url);
                    pictureSize = new PictureSize { Width = e.Width, Height = e.Height };
                    Order = e.Order;
                    break;
            }
        }
    }
}
