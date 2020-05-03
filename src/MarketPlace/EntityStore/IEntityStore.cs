using System;
using System.Threading.Tasks;

namespace MarketPlace.EntityStore
{
    public interface IEntityStore<TEntity>
    {
        Task Create(TEntity entity);

        Task Save(TEntity entity);

        Task<TEntity> GetById(Guid id);

    }
}
