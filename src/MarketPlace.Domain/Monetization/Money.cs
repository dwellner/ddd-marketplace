using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.Monetization
{

    public class Money: Value<Money>
    {
        public const string defaultCurrency = "EUR";

        protected override object[] GetValues() => new object[] { Amount, CurrencyCode };

        protected Money() { }

        internal  Money(decimal amount, string currencyCode)
        {
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) {
            if (amount == default) throw new ArgumentException("Value must be specified", nameof(amount));
            if (currencyCode== default) throw new ArgumentException("currency Code must be specified", nameof(currencyCode));

            var currency = currencyLookup.FindCurrency(currencyCode);
            if (currency == Currency.None) throw new ArgumentException($"Invalid currency code: {currencyCode}");
            if (Math.Round(amount, currency.DecimalPlaces) != amount) throw new ArgumentException($"amount must have no more than {currency.DecimalPlaces} decimals for currency {currency.CurrencyCode}", nameof(amount));

            Amount = amount;
            CurrencyCode = currency.CurrencyCode;
        }

        public static Money FromDecimal(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
            => new Money(amount, currencyCode, currencyLookup);

        public decimal Amount { get; }
        public string CurrencyCode { get; }

        public Money Add(Money other)
        {
            if (CurrencyCode != other.CurrencyCode) throw new CurrenyMismatchException("Cannot add Money of different currencies");
            return new Money(Amount + other.Amount, CurrencyCode);
        }
        public Money Subtract(Money other)
        {
            if (CurrencyCode != other.CurrencyCode) throw new CurrenyMismatchException("Cannot subtract Money of different currencies");
            return new Money(Amount - other.Amount, CurrencyCode);
        }

        public static Money operator + (Money left, Money right) => left.Add(right);
        public static Money operator - (Money left, Money right) => left.Subtract(right);
        public override string ToString() => $"{CurrencyCode} {Amount}";
    }

    public class CurrenyMismatchException : Exception
    {
       public CurrenyMismatchException(string message) : base(message) { }
    }
}
