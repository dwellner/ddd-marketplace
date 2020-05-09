using System.Threading.Tasks;
using MarketPlace.Domain.ClassifiedAd;
using Raven.Client.Documents.Session;

namespace MarketPlace.Infrastructure
{
    public class ClassifiedAdRavenDbRepository: IClassifiedAdRepository
    {
        readonly IAsyncDocumentSession session;

        public ClassifiedAdRavenDbRepository(IAsyncDocumentSession session) => this.session = session;

        public Task<ClassifiedAd> Load(ClassifiedAdId id) => session.LoadAsync<ClassifiedAd>(EntityId(id));
        public Task Add(ClassifiedAd ad) => session.StoreAsync(ad, EntityId(ad.Id));
        public Task<bool> exists(ClassifiedAdId id) => session.Advanced.ExistsAsync(EntityId(id));

        private string EntityId(ClassifiedAdId id) => $"CLassifiedAd/{id.Value}";
    }
}
