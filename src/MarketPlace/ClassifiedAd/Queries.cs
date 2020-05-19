using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MarketPlace.ClassifiedAd.QueryModels;
using static MarketPlace.Projections.ReadModels;

namespace MarketPlace.ClassifiedAd
{
    public static class RavenQueries
    {
        public static Task<List<ClassifiedAdListItem>> Query(this IEnumerable<ClassifiedAdDetails> items,
            GetPublishedClassifiedAds query) => Task.FromResult(new List<ClassifiedAdListItem>());
        // TODO: state == actve

        public static Task<List<ClassifiedAdListItem>> Query(this IEnumerable<ClassifiedAdDetails> items,
            GetClassifiedAdsOwnedBy query) => Task.FromResult(new List<ClassifiedAdListItem>());


        public static Task<ClassifiedAdDetails> Query(this IEnumerable<ClassifiedAdDetails> items,
            GetPublicClassifiedAd query) => Task.FromResult(
                items.FirstOrDefault(item => item.ClassifiedAdId == query.classifiedAdId));
    }
}

