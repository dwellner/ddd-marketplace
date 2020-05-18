using System;
using System.Threading.Tasks;
using MarketPlace.Domain.UserProfile;
using MarketPlace.Service;

namespace MarketPlace.UserProfile
{
    public class UserProfileService: IApplicationService
    {
        private readonly IAggregateStore store;
        private readonly IFailoverPolicy failoverPolicy;

        public UserProfileService(IAggregateStore store, IFailoverPolicyProvider failoverPolicyProvider)
        {
            this.store = store;
            failoverPolicy = failoverPolicyProvider.CommandRetryPolicy;
        }

        public async Task Handle(object command) =>
            await failoverPolicy.ExecuteAsync(async () => {
                switch (command)
                {
                    case Contracts.V1.Create cmd:
                        await HandleCreate(cmd);
                        break;
                    case Contracts.V1.UpdateFullName c:
                        await HandleUpdate(c.Id, profile =>
                        profile.UpdateFullName(UserFullName.FromString(c.NewName)));
                        break;
                    case Contracts.V1.UpdateDisplayName c:
                        await HandleUpdate(c.Id, profile =>
                        profile.UpdateDisplayName(UserDisplayName.FromString(c.NewName)));
                        break;
                    case Contracts.V1.MarkProfilePhotoUploaded c:
                        await HandleUpdate(c.Id, profile =>
                        profile.ProfilePhotoUploaded(new Uri(c.photoUrl)));
                        break;
                    default: throw new ArgumentException($"Unsupported command type: {command.GetType().Name}");
                };
            });

        private async Task HandleCreate(Contracts.V1.Create cmd)
        {
            var id = new UserProfileId(cmd.Id);
            var exists = await store.Exists<Domain.UserProfile.UserProfile, UserProfileId>(id);
            if (exists) throw new InvalidOperationException("UserProfileId with same id already exists");

            var userProfile = new Domain.UserProfile.UserProfile(id,
                UserFullName.FromString(cmd.FullName), UserDisplayName.FromString(cmd.DisplayName));

            await store.Save<Domain.UserProfile.UserProfile, UserProfileId>(userProfile);
        }

        private async Task HandleUpdate(Guid guid, Action<Domain.UserProfile.UserProfile> operation) =>
            await this.HandleUpdate(store, new UserProfileId(guid), operation);
    }

}

