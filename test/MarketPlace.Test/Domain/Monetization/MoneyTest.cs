using System;
using MarketPlace.Domain.Monetization;
using Xunit;

namespace MarketPlace.Test.Domain.Monetization
{

    public class MoneyTest
    {
        FakeCurrencyLookup currencyLookup = new FakeCurrencyLookup();


        [Fact]
        public void Same_amount_of_same_crrencies_should_be_equal()
        {
            var oneEuro = Money.FromDecimal(1, "EUR", currencyLookup);
            var anotherEuro = Money.FromDecimal(1, "EUR", currencyLookup);

            Assert.Equal(oneEuro, anotherEuro);
        }


        [Fact]
        public void Same_amount_of_different_currencies_should_not_be_equal()
        {
            var oneEuro = Money.FromDecimal(1, "EUR", currencyLookup);
            var onePound = Money.FromDecimal(1, "GDP", currencyLookup);

            Assert.NotEqual(oneEuro, onePound);
        }

        [Fact]
        public void sum_should_match_total_amount()
        {
            var one = Money.FromDecimal(1, "EUR", currencyLookup);
            var two = Money.FromDecimal(2, "EUR", currencyLookup);
            var five = Money.FromDecimal(5, "EUR", currencyLookup);

            Assert.Equal(five, two + two + two - one);
        }

        [Fact]
        public void add_or_subtraction_of_different_currencies_shoul_fail()
        {
            var oneEuro = Money.FromDecimal(1, "EUR", new FakeCurrencyLookup());
            var twoPound = Money.FromDecimal(2, "GDP", new FakeCurrencyLookup());

            Assert.Throws<CurrenyMismatchException>(() => oneEuro + twoPound);
            Assert.Throws<CurrenyMismatchException>(() => oneEuro - twoPound);
        }

        [Fact]
        public void euro_currency_cannot_have_more_than_two_decimals()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(3.234m, "EUR", new FakeCurrencyLookup()));
        }

        [Fact]
        public void currency_must_be_valid()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(3.50m, "FOO", new FakeCurrencyLookup()));
        }


    }
}
