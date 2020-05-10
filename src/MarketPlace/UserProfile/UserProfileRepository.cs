using System.Threading.Tasks;
using MarketPlace.Domain.UserProfile;
using Raven.Client.Documents.Session;

namespace MarketPlace.UserProfile
{
    public class UserProfileRepository: IUserProfileRepository
    {
        readonly IAsyncDocumentSession session;

        public UserProfileRepository(IAsyncDocumentSession session) => this.session = session;

        public Task<Domain.UserProfile.UserProfile> Load(UserProfileId id) => session.LoadAsync<Domain.UserProfile.UserProfile>(EntityId(id));
        public Task Add(Domain.UserProfile.UserProfile profile) => session.StoreAsync(profile, EntityId(profile.Id));
        public Task<bool> exists(UserProfileId id) => session.Advanced.ExistsAsync(EntityId(id));

        private string EntityId(UserProfileId id) => $"UserProfile/{id.Value}";
    }
}
