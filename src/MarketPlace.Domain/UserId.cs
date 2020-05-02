using System;

namespace MarketPlace.Domain
{
    public class UserId : Value<UserId>
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            if (value == default) throw new ArgumentException("Value must be specified", nameof(value));
            Value = value;
        }

        protected override object[] Values => new object[] { Value };
    }
}
