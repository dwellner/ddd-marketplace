using System;
namespace MarketPlace.UserProfile
{
    public static class Contracts
    {
        public static class V1
        {
            public class Create
            {
                public Guid Id { get; set; }
                public string FullName { get; set; }
                public string DisplayName { get; set; }
            }

            public class UpdateFullName
            {
                public Guid Id { get; set; }
                public string NewName { get; set; }
            }

            public class UpdateDisplayName
            {
                public Guid Id { get; set; }
                public string NewName { get; set; }
            }

            public class MarkProfilePhotoUploaded
            {
                public Guid Id { get; set; }
                public string photoUrl { get; set; }
            }
        }
    }
}
