using System;

namespace MarketPlace.Projections
{
    public static class ReadModels
    {

        public class ClassifiedAdDetails
        {
            public Guid ClassifiedAdId { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public string CurrencyCode { get; set; }
            public string Description { get; set; }
            public Guid SellerId { get; set; }
            public string SellerDisplayName { get; set; }
            public string[] PhotoUrls { get; set; }
        }

        public class ClassifiedAdListItem
        {
            public Guid ClassifiedAdId { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public string CurrencyCode { get; set; }
            public string PhotoUrl { get; set; }
        }

        public class UserDetails
        {
            public Guid Id { get; set; }
            public string Displayname { get; set; }
            public string Fullname { get; set; }
        }

    }

}
