using System;

namespace MarketPlace.Domain.Monetization
{

    public class Money: Value<Money>
    {
        public const string defaultCurrency = "EUR";

        protected override object[] Values => new object[] { Amount, Currency };

        private Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) {
            if (amount == default) throw new ArgumentException("Value must be specified", nameof(amount));
            if (currencyCode== default) throw new ArgumentException("currency Code must be specified", nameof(currencyCode));

            var currency = currencyLookup.FindCurrency(currencyCode);
            if (currency == Currency.None) throw new ArgumentException($"Invalid currency code: {currencyCode}");
            if (Math.Round(amount, currency.DecimalPlaces) != amount) throw new ArgumentException($"amount must have no more than {currency.DecimalPlaces} decimals for currency {currency.CurrencyCode}", nameof(amount));

            Amount = amount;
            Currency = currency;
        }

        public static Money FromDecimal(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
            => new Money(amount, currencyCode, currencyLookup);

        public decimal Amount { get; }
        public Currency Currency { get; }

        public Money Add(Money other)
        {
            if (Currency.CurrencyCode != other.Currency.CurrencyCode) throw new CurrenyMismatchException("Cannot add Money of different currencies");
            return new Money(Amount + other.Amount, Currency);
        }
        public Money Subtract(Money other)
        {
            if (Currency.CurrencyCode != other.Currency.CurrencyCode) throw new CurrenyMismatchException("Cannot subtract Money of different currencies");
            return new Money(Amount - other.Amount, Currency);
        }

        public static Money operator + (Money left, Money right) => left.Add(right);
        public static Money operator - (Money left, Money right) => left.Subtract(right);
        public override string ToString() => $"{Currency.CurrencyCode} {Amount}";
    }

    public class CurrenyMismatchException : Exception
    {
       public CurrenyMismatchException(string message) : base(message) { }
    }
}
