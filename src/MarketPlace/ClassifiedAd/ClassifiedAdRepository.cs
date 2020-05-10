using System.Threading.Tasks;
using MarketPlace.Domain.ClassifiedAd;
using Raven.Client.Documents.Session;

namespace MarketPlace.ClassifiedAd
{
    public class ClassifiedAdRepository: IClassifiedAdRepository
    {
        readonly IAsyncDocumentSession session;

        public ClassifiedAdRepository(IAsyncDocumentSession session) => this.session = session;

        public Task<Domain.ClassifiedAd.ClassifiedAd> Load(ClassifiedAdId id) => session.LoadAsync<Domain.ClassifiedAd.ClassifiedAd>(EntityId(id));
        public Task Add(Domain.ClassifiedAd.ClassifiedAd ad) => session.StoreAsync(ad, EntityId(ad.Id));
        public Task<bool> exists(ClassifiedAdId id) => session.Advanced.ExistsAsync(EntityId(id));

        private string EntityId(ClassifiedAdId id) => $"CLassifiedAd/{id.Value}";
    }
}
