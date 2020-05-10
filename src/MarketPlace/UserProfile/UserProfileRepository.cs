using MarketPlace.Domain.UserProfile;
using MarketPlace.Service;
using Raven.Client.Documents.Session;

namespace MarketPlace.UserProfile
{
    public class UserProfileRepository: RavenDbEntityRepository<Domain.UserProfile.UserProfile, UserProfileId>, IUserProfileRepository
    {
        protected override string EntityType => "UserProfile";

        public UserProfileRepository(IAsyncDocumentSession session) : base(session) { }
    }
}
