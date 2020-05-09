using MarketPlace.Domain.Monetization;

namespace MarketPlace.Test.Domain.Monetization
{
    class FakeCurrencyLookup : ICurrencyLookup
    {
        public Currency FindCurrency(string currencyCode)
        {
            switch (currencyCode.ToUpper())
            {
                case "EUR": return new Currency { CurrencyCode = "EUR" };
                case "GDP": return new Currency { CurrencyCode = "GDP" };
                default: return Currency.None;
            }
        }
    }
}
