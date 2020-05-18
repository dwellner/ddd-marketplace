using System.Threading.Tasks;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Service
{
    public interface IAggregateStore
    {
        Task<T> Load<T, TId>(TId id) where T : AggregateRoot<TId> where TId : Value<TId>;

        Task Save<T, TId>(T entity) where T : AggregateRoot<TId> where TId : Value<TId>;

        Task<bool> Exists<T, TId>(TId id);
    }
}
