using System;
using System.Linq;
using MarketPlace.Domain.Framework;
using MarketPlace.Domain.UserProfile;
using Xunit;

namespace MarketPlace.Test.Domain.UserProfiles
{
    public class UserProfileTest
    {
        [Fact]
        public void ShouldCreateWhenValidIdFullNameAndDisplayNameAreProvided()
        {
            var id = Guid.NewGuid();
            var userProfile = new UserProfile(new UserProfileId(id), UserFullName.FromString("Mike Seller"), UserDisplayName.FromString("mike88"));

            Assert.Equal(id, userProfile.Id.Value);
            Assert.Equal("Mike Seller", userProfile.FullName.Value);
            Assert.Equal("mike88", userProfile.DisplayName.Value);
        }

        [Fact]
        public void ShouldAllowUpdateOfFullName()
        {
            var id = Guid.NewGuid();
            var userProfile = new UserProfile(new UserProfileId(id), UserFullName.FromString("Mike Seller"), UserDisplayName.FromString("mike88"));
            userProfile.UpdateFullName(UserFullName.FromString("Mike Buyer"));
            Assert.Equal("Mike Buyer", userProfile.FullName.Value);
        }

        [Fact]
        public void ShouldAllowUpdateOfDisplayName()
        {
            var id = Guid.NewGuid();
            var userProfile = new UserProfile(new UserProfileId(id), UserFullName.FromString("Mike Seller"), UserDisplayName.FromString("mike88"));
            userProfile.UpdateDisplayName(UserDisplayName.FromString("Mike99"));
            Assert.Equal("Mike99", userProfile.DisplayName.Value);
        }

        [Fact]
        public void ShouldUpdatePhotoUrlUponProfilePhotoUploaded()
        {
            var userProfile = new UserProfile(new UserProfileId(Guid.NewGuid()), UserFullName.FromString("Mike Seller"), UserDisplayName.FromString("mike88"));
            userProfile.ProfilePhotoUploaded("http://example.com/photo.jpg");
            Assert.Equal("http://example.com/photo.jpg", userProfile.PhotoUrl.ToString());
        }

    }
}
