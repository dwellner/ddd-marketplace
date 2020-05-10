using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using static MarketPlace.ClassifiedAd.QueryModels;
using static MarketPlace.ClassifiedAd.ReadModels;

namespace MarketPlace.ClassifiedAd
{
    public static class Queries
    {
        public static Task<List<ClassifiedAdListItem>> Query(this IAsyncDocumentSession session,
            GetPublishedClassifiedAds query) => session
                .Query<Domain.ClassifiedAd.ClassifiedAd>()
                .Where(ad => ad.State == Domain.ClassifiedAd.ClassifiedAdState.Active)
                .Select(ad => new ClassifiedAdListItem
                {
                    ClassifiedAdId = ad.Id,
                    Price = ad.Price.Amount,
                    CurrencyCode = ad.Price.CurrencyCode,
                    Title = ad.Title.Text

                })
            .Skip(query.Page * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        public static Task<List<ClassifiedAdListItem>> Query(this IAsyncDocumentSession session,
            GetClassifiedAdsOwnedBy query) => session
                .Query<Domain.ClassifiedAd.ClassifiedAd>()
                .Where(ad => ad.OwnerId.Value == query.OwnerId)
                .Select(ad => new ClassifiedAdListItem
                {
                    ClassifiedAdId = ad.Id,
                    Price = ad.Price.Amount,
                    CurrencyCode = ad.Price.CurrencyCode,
                    Title = ad.Title.Text

                })
            .Skip(query.Page * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();


        public static Task<ClassifiedAdDetails> Query(this IAsyncDocumentSession session,
            GetPublicClassifiedAd query) =>
            (
                from ad in session
                    .Query<Domain.ClassifiedAd.ClassifiedAd>()
                    .Where(ad => ad.Id.Value == query.classifiedAdId)
                let user = RavenQuery
                    .Load<Domain.UserProfile.UserProfile>($"UserProfile/{ad.OwnerId.Value}")
                select new ClassifiedAdDetails
                {
                    ClassifiedAdId = ad.Id.Value,
                    Price = ad.Price.Amount,
                    CurrencyCode = ad.Price.CurrencyCode,
                    Title = ad.Title.Text,
                    Description = ad.Text.Text,
                    //PhotoUrls
                    SellersDisplayName = user.DisplayName.Value
                })
            .SingleAsync();
    }
}

