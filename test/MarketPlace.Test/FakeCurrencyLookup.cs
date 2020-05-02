﻿using MarketPlace.Domain;

namespace MarketPlace.Test
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
