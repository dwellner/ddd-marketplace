using System;
namespace MarketPlace.ClassifiedAd
{
    public static class Contracts
    {
        public static class V1
        {
            public class Create
            {
                public Guid Id { get; set; }
                public Guid OwnerId { get; set; }
            }

            public class SetTitle
            {
                public Guid Id { get; set; }
                public string Title { get; set; }
            }

            public class UpdateText
            {
                public Guid Id { get; set; }
                public string Text { get; set; }
            }

            public class UpdatePrice
            {
                public Guid Id { get; set; }
                public decimal Amount { get; set; }
                public string CurrencyCode { get; set; }
            }

            public class RequestToPublish
            {
                public Guid Id { get; set; }
            }

        }
    }
}
