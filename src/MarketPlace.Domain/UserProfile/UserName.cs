using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.UserProfile
{
    public class UserFullName : Value<UserFullName>
    {
        public string Value { get; private set; }

        public static UserFullName FromString(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("User full name cannot be empty", nameof(value));
            if (value.Length > 40) throw new ArgumentException("User name cannot exceed 40 characters", nameof(value));
            return new UserFullName(value);
        }

        protected override object[] GetValues() => new object[] { Value };

        internal UserFullName(string value) => Value = value;
    }

}
