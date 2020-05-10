using System;
using System.Threading.Tasks;
using MarketPlace.Domain.Framework;
using Raven.Client.Documents.Session;

namespace MarketPlace.Service
{
    public abstract class RavenDbEntityRepository<TEntity, TEntityId> where TEntity : AggregateRoot<TEntityId> where TEntityId: IdValue<TEntityId,Guid>
    {
        readonly IAsyncDocumentSession session;

        public RavenDbEntityRepository(IAsyncDocumentSession session) => this.session = session;

        public Task<TEntity> Load(TEntityId id) => session.LoadAsync<TEntity>(EntityId(id));
        public Task Add(TEntity entity) => session.StoreAsync(entity, EntityId(entity.Id));
        public Task<bool> Exists(TEntityId id) => session.Advanced.ExistsAsync(EntityId(id));

        private string EntityId(TEntityId id) => $"{EntityType}/{id.Value}";

        protected abstract string EntityType { get; }
    }
}
