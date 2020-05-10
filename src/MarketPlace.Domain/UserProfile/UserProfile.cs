using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.UserProfile
{

    public class UserProfile : AggregateRoot<UserProfileId>
    {
        public UserFullName FullName { get; private set; }
        public UserDisplayName DisplayName { get; private set; }
        public Uri PhotoUrl { get; private set; }

        protected UserProfile() { }

        public UserProfile(UserProfileId userProfileId, UserFullName fullName, UserDisplayName displayName) =>
            Apply(new Events.UserProfileCreated
            {
                UserId = userProfileId.Value,
                FullName = fullName.Value,
                DisplayName = displayName.Value
            });

        public void UpdateFullName(UserFullName newName) =>
            Apply(new Events.UserProfileFullNameUpdated
            {
                UserId = Id,
                FullName = newName.Value
            });

        public void UpdateDisplayName(UserDisplayName newName) =>
            Apply(new Events.UserProfileDisplayNameUpdated
            {
                UserId = Id,
                DisplayName = newName.Value
            });
         
        public void ProfilePhotoUploaded(Uri photoUrl) =>
            Apply(new Events.UserProfilePhotoUploaded
            {
                UserId = Id,
                PhotoUrl= photoUrl.ToString()
            });


        protected override void EnsureValidState()
        {
            bool isValid = Id != null && FullName != null && DisplayName != null;
            if (!isValid) throw new InvalidEntityStateException(this, $"Post-checks failed");

        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.UserProfileCreated e:
                    Id = new UserProfileId(e.UserId);
                    FullName = new UserFullName(e.FullName);
                    DisplayName = new UserDisplayName(e.DisplayName);
                    break;
                case Events.UserProfileDisplayNameUpdated e:
                    DisplayName = new UserDisplayName(e.DisplayName);
                    break;
                case Events.UserProfileFullNameUpdated e:
                    FullName = new UserFullName(e.FullName);
                    break;
                case Events.UserProfilePhotoUploaded e:
                    PhotoUrl = new Uri(e.PhotoUrl);
                    break;
            }
        }
    }
}
