using System;

namespace MarketPlace.Domain.Framework
{
    public abstract class IdValue<TValue,TId> : Value<TValue> where TValue : Value<TValue>
    {
        public TId Value { get; }

        public IdValue(TId value)
        {
            if (value == null || value.Equals(default(TId))) throw new ArgumentException("Value must be specified", nameof(value));
            Value = value;
        }

        protected override object[] Values => new object[] { Value };

        public static implicit operator TId (IdValue<TValue,TId> idValue) => idValue.Value;
    }
}
