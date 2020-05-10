using System;
using System.Threading.Tasks;
using MarketPlace.Domain.UserProfile;
using MarketPlace.Service;

namespace MarketPlace.UserProfile
{
    public class UserProfileService
    {
        private readonly IUserProfileRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IFailoverPolicy failoverPolicy;

        public UserProfileService(IUserProfileRepository repository, IUnitOfWork unitOfWork,
            IFailoverPolicyProvider failoverPolicyProvider)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            failoverPolicy = failoverPolicyProvider.CommandRetryPolicy;
        }

        public async Task Handle(object command) =>
            await failoverPolicy.ExecuteAsync(async () => {
                switch (command)
                {
                    case Contracts.V1.Create c:
                        await HandleCreate(() =>
                            new Domain.UserProfile.UserProfile(new UserProfileId(c.Id),
                            UserFullName.FromString(c.FullName), UserDisplayName.FromString(c.DisplayName)));
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

        private async Task HandleCreate(Func<Domain.UserProfile.UserProfile> creator)
        {
            var ad = creator();
            var exists = await repository.exists(ad.Id);
            if (exists) throw new InvalidOperationException("ClassifiedAd with same id already exists");
            await repository.Add(ad);
            await unitOfWork.Commit();
        }

        private async Task HandleUpdate(Guid id, Action<Domain.UserProfile.UserProfile> action)
        {
            var ad = await repository.Load(new UserProfileId(id));
            if (ad == null) throw new ArgumentException($"Invalid ad id: {id}");
            action(ad);
            await unitOfWork.Commit();
        }

    }
}

