using MarketPlace.Domain.Monetization;

namespace MarketPlace
{
    public class CurrencyLookup : ICurrencyLookup
    {
        public Currency FindCurrency(string currencyCode)
        {
            // only supporting EURO for now
            if (currencyCode != null && currencyCode.ToUpper().Equals("EUR"))
                return new Currency { CurrencyCode = "EUR" };
            return Currency.None;
        }
    }
}
