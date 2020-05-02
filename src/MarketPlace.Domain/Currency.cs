namespace MarketPlace.Domain
{
    public class Currency : Value<Currency>
    {
        public string CurrencyCode { get; set;  }
        public bool IsInUse { get; set; } = true;
        public int DecimalPlaces { get; set; } = 2;

        protected override object[] Values => new object[] { CurrencyCode, IsInUse, DecimalPlaces };

        public static Currency None = new Currency { IsInUse = false };
    }
}
