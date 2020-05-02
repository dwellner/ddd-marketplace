namespace MarketPlace.Domain.Monetization
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }
}
