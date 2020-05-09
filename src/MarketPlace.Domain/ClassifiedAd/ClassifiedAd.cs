using System;
using System.Collections.Generic;
using System.Linq;
using MarketPlace.Domain.Framework;
using MarketPlace.Domain.Monetization;

namespace MarketPlace.Domain.ClassifiedAd
{
    public class ClassifiedAd: AggregateRoot<ClassifiedAdId>
    {
        List<Picture> Pictures { get; } = new List<Picture>();
        public UserId OwnerId { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public UserId ApprovedById { get; private set; }

        protected ClassifiedAd() { }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId) => 
            Apply(new Events.ClassifiedAdCreated { Id = id, OwnerId = ownerId.Value });

        public void SetTitle(ClassifiedAdTitle title) =>
            Apply(new Events.ClassifiedAdTitleChanged { Id = Id, AdTitle = title.Text });

        public void UpdateText(ClassifiedAdText text) => 
            Apply(new Events.ClassifiedAdTextUpdated { Id = Id, AdText= text.Text });

        public void UpdatePrice(Price price) => 
            Apply(new Events.ClassifiedAdPriceUpdated { Id = Id, Price= price.Amount, CurrenctCode = price.CurrencyCode});

        public void RequestToPublish() => 
            Apply(new Events.ClassifiedAdSentForReview { Id = Id });

        public void AddPicture(PictureId pictureId, PictureSize size, Uri location) =>
            Apply(new Events.PictureAddedToClassifiedAd
            {
                ClassifiedAdId = Id,
                PictureId = pictureId.Value,
                Height = size.Height,
                Width = size.Width,
                Url = location.ToString(),
                Order = Pictures.Any() ? Pictures.Max(p => p.Order) + 1 : 1
            });

        public void ResizePicture(PictureId pictureId, PictureSize newSize)
        {
            var picture = Pictures.FirstOrDefault(p => p.Id == pictureId);
            if (picture == null) throw new ArgumentException("Invaliid picture id", nameof(pictureId));
            picture.Resize(newSize);
        }
            

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    Price = new Price(e.Price, e.CurrenctCode);
                    break;
                case Events.ClassifiedAdSentForReview _:
                    State = ClassifiedAdState.PendingReview;
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    Text = new ClassifiedAdText(e.AdText);
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    Title = new ClassifiedAdTitle(e.AdTitle);
                    break;
                case Events.PictureAddedToClassifiedAd e:
                    var picture = new Picture(Apply);
                    ApplyToEntity(picture, @event);
                    Pictures.Add(picture);
                    break;
            }
        }

        Picture FirstPicture => Pictures.OrderBy(p => p.Order).FirstOrDefault();

        protected override void EnsureValidState()
        {
            var hasTitleTextAndPrice = Title != null && Text != null && Price != null;
            var hasBeenApproved = ApprovedById != null;

            var valid = Id != null && OwnerId != null &&  State switch 
            {
                ClassifiedAdState.PendingReview => hasTitleTextAndPrice && FirstPicture.HasValidSize(),
                ClassifiedAdState.Active => hasTitleTextAndPrice && FirstPicture.HasValidSize() && hasBeenApproved,
                _ => true
            };

            if (!valid) throw new InvalidEntityStateException(this, $"Post-checks failed in state: ${State}");
        }

    }
}
