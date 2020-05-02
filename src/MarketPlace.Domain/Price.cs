using System;
namespace MarketPlace.Domain
{
    public class Price : Money
    {
        Price(decimal value, string currencyCode) : base(value, currencyCode) {
            if (value < 0) throw new ArgumentException("Price cannot be negative", nameof(value));
        }

        public static new Price FromDecimal(decimal amount, string currencyCode) => new Price(amount, currencyCode);

    }
}
