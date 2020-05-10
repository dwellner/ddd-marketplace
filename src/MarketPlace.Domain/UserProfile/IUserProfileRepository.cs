using System.Threading.Tasks;

namespace MarketPlace.Domain.UserProfile
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> Load(UserProfileId id);

        Task Add(UserProfile add);

        Task<bool> exists(UserProfileId id);
    }
}
