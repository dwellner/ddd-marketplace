using System;

namespace MarketPlace.Domain
{
    public class ClassifiedAdId : Value<ClassifiedAdId>
    {
        public Guid Value { get; }

        public ClassifiedAdId(Guid value)
        {
            if (value == default) throw new ArgumentException("Value must be specified", nameof(value));
            Value = value;
        }

        protected override object[] Values => new object[] { Value };
    }
}
