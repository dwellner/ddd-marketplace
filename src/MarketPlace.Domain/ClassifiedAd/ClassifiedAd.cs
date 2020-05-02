using MarketPlace.Domain.Monetization;

namespace MarketPlace.Domain.ClassifiedAd
{
    public class ClassifiedAd: Entity
    {
        public ClassifiedAdId Id { get; private set; }
        public UserId OwnerId { get; private set; }
        public ClassifiedAdState State { get; private set; }

        ClassifiedAdTitle title;
        ClassifiedAdText text;
        Price price;

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            Raise(new Events.ClassifiedAdCreated { Id = Id, OwnerId = ownerId.Value });
        }

        public void SetTitle(ClassifiedAdTitle title)
        {
            this.title = title;
            EnsureValidState();
            Raise(new Events.ClassifiedAdTitleChanged { Id = Id, AdTitle = title.Text });
        }
        public void UpdateText(ClassifiedAdText text)
        {
            this.text = text;
            EnsureValidState();
            Raise(new Events.ClassifiedAdTextUpdated { Id = Id, AdText= text.Text });
        }
        public void UpdatePrice(Price price)
        {
            this.price = price;
            EnsureValidState();
            Raise(new Events.ClassifiedAdPriceUpdated { Id = Id, Price= price.Amount, CurrenctCode = price.Currency.CurrencyCode});
        }

        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingReview;
            EnsureValidState();
            Raise(new Events.ClassifiedAdSentForReview { Id = Id });
        }

        private void EnsureValidState()
        {
            var valid = Id != null && OwnerId != null &&  State switch 
            {
                ClassifiedAdState.PendingReview => title != null && text != null && price != null,
                ClassifiedAdState.Active => title != null && text != null && price != null,
                _ => true
            };

            if (!valid) throw new InvalidEntityStateException(this, $"Poist-checks failed in state: ${State}");
        }
    }

    public enum ClassifiedAdState
    {
        PendingReview,
        Active,
        Inactive,
        MarkedAsSold
    }
}
