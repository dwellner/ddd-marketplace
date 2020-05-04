using System;
namespace MarketPlace.Domain.ClassifiedAd
{
    public static class Events {

        public class ClassifiedAdCreated {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
        }

        public class ClassifiedAdTitleChanged {
            public Guid Id { get; set; }
            public string AdTitle { get; set; }
        }

        public class ClassifiedAdTextUpdated {
            public Guid Id { get; set; }
            public string AdText { get; set; }
        }

        public class ClassifiedAdPriceUpdated {
            public Guid Id { get; set; }
            public decimal Price { get; set; }
            public string CurrenctCode { get; set; }
        }

        public class ClassifiedAdSentForReview {
            public Guid Id { get; set; }
        }

        public class PictureAddedToClassifiedAd {
            public Guid ClassifiedAdId { get; set; }
            public Guid PictureId { get; set; }
            public string Url { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Order { get; set; }
        }
    }
}
