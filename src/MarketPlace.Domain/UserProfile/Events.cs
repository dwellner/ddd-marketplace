using System;

namespace MarketPlace.Domain.UserProfile
{
    public static class Events
    {
        public class UserProfileCreated
        {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
            public string DisplayName { get; set; }
        }

        public class UserProfilePhotoUploaded
        {
            public Guid UserId { get; set; }
            public string PhotoUrl { get; set; }
        }

        public class UserProfileFullNameUpdated
        {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
        }

        public class UserProfileDisplayNameUpdated
        {
            public Guid UserId { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
