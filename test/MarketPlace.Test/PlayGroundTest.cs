using System;
using MarketPlace.Domain;
using Xunit;

namespace MarketPlace.Test
{
    public class MoneyTest
    {
        [Fact]
        public void SumShouldMatchTotalAmount()
        {
            var one = Money.FromDecimal(1, "EUR");
            var two = Money.FromDecimal(2, "EUR");
            var five = Money.FromDecimal(5, "EUR");

            Assert.Equal(five, two + two + two - one);
        }

        [Fact]
        public void SameAmountOfDifferentCurrenciesShouldNotBeEqual()
        {
            var oneEuro = Money.FromDecimal(1, "EUR");
            var onePound = Money.FromDecimal(1, "GDP");

            Assert.NotEqual(oneEuro, onePound);
        }

        [Fact]
        public void AddOrSubtractionOfDifferentCurrenciesShoulFail()
        {
            var oneEuro = Money.FromDecimal(1, "EUR");
            var twoPound = Money.FromDecimal(2, "GDP");

            Assert.Throws<CurrenyMismatchException>(() => oneEuro + twoPound);
            Assert.Throws<CurrenyMismatchException>(() => oneEuro - twoPound);
        }

        [Fact]
        public void EuroCurrencyCannotHaveMoreThan2Decimals()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(3.234m, "EUR"));
        }


    }
}
