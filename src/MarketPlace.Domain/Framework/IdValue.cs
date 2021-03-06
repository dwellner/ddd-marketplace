﻿using System;

namespace MarketPlace.Domain.Framework
{
    public abstract class IdValue<TValue,TId> : Value<TValue> where TValue : Value<TValue>
    {
        protected IdValue() { }

        public TId Value { get; private set; }

        public IdValue(TId value)
        {
            if (value == null || value.Equals(default(TId))) throw new ArgumentException("Value must be specified", nameof(value));
            Value = value;
        }

        protected override object[] GetValues() => new object[] { Value };
        public override string ToString() => Value.ToString();

        public static implicit operator TId (IdValue<TValue,TId> idValue) => idValue.Value;
    }
}
