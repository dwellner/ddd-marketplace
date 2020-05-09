using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.Monetization
{
    public class Currency : Value<Currency>
    {
        public string CurrencyCode { get; set;  }
        public bool IsInUse { get; set; } = true;
        public int DecimalPlaces { get; set; } = 2;

        protected override object[] GetValues() => new object[] { CurrencyCode, IsInUse, DecimalPlaces };

        public static Currency None = new Currency { IsInUse = false };
    }
}
