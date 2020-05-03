using System;
namespace MarketPlace.Domain.Monetization
{
    public class Price : Money
    {
        Price(decimal value, string currencyCode, ICurrencyLookup currencyLookup) : base(value, currencyCode, currencyLookup) {
            if (value < 0) throw new ArgumentException("Price cannot be negative", nameof(value));
        }

        internal Price(decimal amount, string currencyCode) : base(amount, currencyCode) { }

        public static new Price FromDecimal(decimal amount, string currencyCode,ICurrencyLookup currencyLookup) =>
            new Price(amount, currencyCode,currencyLookup);

    }
}
