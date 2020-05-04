using System.Threading.Tasks;
using MarketPlace.Domain;
using MarketPlace.Domain.ClassifiedAd;

namespace MarketPlace.EntityStore
{
    public class ClassifiedAdRepository: IClassifiedAdRepository
    {
        public Task<ClassifiedAd> Load(ClassifiedAdId id) => Task.FromResult(new ClassifiedAd(new ClassifiedAdId(id), new UserId(id)));

        public Task Add(ClassifiedAd ad) => Task.CompletedTask;

        public Task<bool> exists(ClassifiedAdId id) => Task.FromResult(false);
    }
}
