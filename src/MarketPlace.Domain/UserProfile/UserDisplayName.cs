using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.UserProfile
{
    public class UserDisplayName : Value<UserFullName>
    {
        public string Value { get; private set; }

        public static UserDisplayName FromString(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("User displayname cannot be empty", nameof(value));
            if (value.Length > 12) throw new ArgumentException("User displayname cannot exceed 12 characters", nameof(value));
            return new UserDisplayName(value);
        }

        protected override object[] GetValues() => new object[] { Value };

        protected UserDisplayName() { }

        internal UserDisplayName(string value) => Value = value;
    }

}
