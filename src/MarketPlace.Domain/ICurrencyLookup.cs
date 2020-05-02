namespace MarketPlace.Domain
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }
}
