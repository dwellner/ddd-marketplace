using MarketPlace.Domain.Monetization;

namespace MarketPlace.Domain.ClassifiedAd
{
    public class ClassifiedAd: Entity
    {
        public ClassifiedAdId Id { get; private set; }
        public UserId OwnerId { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public UserId ApprovedById { get; private set; }

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
            }
        }

        protected override void EnsureValidState()
        {
            var valid = Id != null && OwnerId != null &&  State switch 
            {
                ClassifiedAdState.PendingReview => Title != null && Text != null && Price != null,
                ClassifiedAdState.Active => Title != null && Text != null && Price != null && ApprovedById != null,
                _ => true
            };

            if (!valid) throw new InvalidEntityStateException(this, $"Post-checks failed in state: ${State}");
        }

    }
}
