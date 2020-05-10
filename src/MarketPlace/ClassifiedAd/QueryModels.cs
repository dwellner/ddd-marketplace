using System;

namespace MarketPlace.ClassifiedAd
{
    public static class QueryModels
    {

        public class GetPublishedClassifiedAds
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
        }

        public class GetPublicClassifiedAd
        {
            public Guid classifiedAdId { get; set; }
        }

        public class GetClassifiedAdsOwnedBy
        {
            public Guid OwnerId { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
        }

    }

}
