using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Service;
using Raven.Client.Documents.Session;

namespace MarketPlace.ClassifiedAd
{
    public class ClassifiedAdRepository: RavenDbEntityRepository<Domain.ClassifiedAd.ClassifiedAd, ClassifiedAdId>, IClassifiedAdRepository
    {
        public ClassifiedAdRepository(IAsyncDocumentSession session) : base(session) { }

        protected override string EntityType => "ClassifiedAd";
    }
}
