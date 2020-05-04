using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.ClassifiedAd
{

    public class Picture : Entity<PictureId>
    {
        internal PictureSize pictureSize { get; set; }
        internal Uri Location { get; set; }
        internal int Order { get; set; }

        public Picture(Action<object> applier) : base(applier) { }

        internal void Resize(PictureSize size)
        {
            Apply(new Events.ClassifiedAdPictureResized
            {
                PictureId = Id,
                Height = size.Height,
                Width = size.Width
            });
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
                case Events.ClassifiedAdPictureResized e:
                    pictureSize = new PictureSize { Width = e.Width, Height = e.Height };
                    break;
            }
        }
    }
}
