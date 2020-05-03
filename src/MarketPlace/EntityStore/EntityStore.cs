using System;
using System.Threading.Tasks;
using MarketPlace.Domain;
using MarketPlace.Domain.ClassifiedAd;

namespace MarketPlace.EntityStore
{
    public class EntityStore: IEntityStore<ClassifiedAd>
    {
        public Task<ClassifiedAd> GetById(Guid id)  => Task.FromResult(new ClassifiedAd(new ClassifiedAdId(id), new UserId(id)));

        public Task Create(ClassifiedAd entity) { return Task.CompletedTask; }

        public Task Save(ClassifiedAd entity) { return Task.CompletedTask; }
    }
}
