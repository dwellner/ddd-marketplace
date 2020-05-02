namespace MarketPlace.Domain
{
    public class ClassifiedAd
    {
        public ClassifiedAdId Id { get; private set; }
        public UserId OwnerId { get; private set; }

        string title;
        string text;
        Price price;

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
        }

        public void SetTitle(string title) => this.title = title;
        public void UpdateText(string text) => this.text = text;
        public void UpdatePrice(Price price) => this.price = price;
    }
}
