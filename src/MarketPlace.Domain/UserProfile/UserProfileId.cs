using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.UserProfile
{
    public class UserProfileId : IdValue<UserProfileId, Guid>
    {
        public UserProfileId(Guid value) : base(value) { }
    }
}
