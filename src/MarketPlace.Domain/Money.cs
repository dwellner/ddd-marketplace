using System;

namespace MarketPlace.Domain
{
    public class Money: Value<Money>
    {
        protected override object[] Values => new object[] { Amount, CurrencyCode };

        protected Money(decimal amount, string currencyCode) {
            if (amount == default) throw new ArgumentException("Value must be specified", nameof(amount));
            if (currencyCode== default) throw new ArgumentException("currency Code must be specified", nameof(currencyCode));
            if (Math.Round(amount, 2) != amount) throw new ArgumentException("amount must have no more than 3 decimals", nameof(amount));

            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public static Money FromDecimal(decimal amount, string currencyCode) => new Money(amount, currencyCode);

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
    }

    public class CurrenyMismatchException : Exception
    {
       public CurrenyMismatchException(string message) : base(message) { }
    }
}
