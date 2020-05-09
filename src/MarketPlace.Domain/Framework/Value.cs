using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace MarketPlace.Domain.Framework
{
    public abstract class Value<TType> : IEquatable<TType> where TType : Value<TType>
    {

        public bool Equals([AllowNull] TType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Enumerable.SequenceEqual(GetValues(),other.GetValues());
        }

        protected abstract object[] GetValues();

        public override bool Equals(object obj) => obj is TType && Equals(obj as TType);

        public override int GetHashCode() => GetValues().Select(p => p != null ? p.GetHashCode() : 17).Aggregate((a,b)=> a * b);

        public static bool operator ==(Value<TType> left, Value<TType> right) => Equals(left, right);
        public static bool operator !=(Value<TType> left, Value<TType> right) => !Equals(left, right);

    }
}
