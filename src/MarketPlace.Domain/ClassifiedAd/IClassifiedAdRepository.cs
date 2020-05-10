using System.Threading.Tasks;

namespace MarketPlace.Domain.ClassifiedAd
{
    public interface IClassifiedAdRepository
    {
        Task<ClassifiedAd> Load(ClassifiedAdId id);

        Task Add(ClassifiedAd add);

        Task<bool> Exists(ClassifiedAdId id);
    }
}
