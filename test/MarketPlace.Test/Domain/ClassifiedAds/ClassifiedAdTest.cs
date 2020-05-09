using System;
using MarketPlace.Domain;
using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Domain.Framework;
using MarketPlace.Domain.Monetization;
using MarketPlace.Test.Domain.Monetization;
using Xunit;

namespace MarketPlace.Test.Domain.ClassifiedAds
{
    public class ClassifiedAdTest
    {
        [Fact]
        public void Request_publishing_should_succeed_when_ad_is_complete()
        {
            var ad = new ClassifiedAd(new ClassifiedAdId(Guid.NewGuid()), new UserId(Guid.NewGuid()));
            ad.AddPicture(new PictureId(Guid.NewGuid()), new PictureSize(800, 600), new Uri("http://example.com"));
            ad.SetTitle(ClassifiedAdTitle.FromTextOrHtml("Fine car!"));
            ad.UpdateText(ClassifiedAdText.FromString("1981 Talbot. Mint condition, no rust"));
            ad.UpdatePrice(Price.FromDecimal(1500m, "EUR", new FakeCurrencyLookup()));

            ad.RequestToPublish();
            Assert.Equal(ClassifiedAdState.PendingReview, ad.State);
        }

        [Fact]
        public void Request_publishing_should_fail_when_title_is_missing()
        {
            var ad = new ClassifiedAd(new ClassifiedAdId(Guid.NewGuid()), new UserId(Guid.NewGuid()));
            ad.UpdateText(ClassifiedAdText.FromString("1981 Talbot. Mint condition, no rust"));
            ad.UpdatePrice(Price.FromDecimal(1500m, "EUR", new FakeCurrencyLookup()));

            Assert.Throws<InvalidEntityStateException>(() =>  ad.RequestToPublish());
        }

        [Fact]
        public void Request_publishing_should_fail_when_text_is_missing()
        {
            var ad = new ClassifiedAd(new ClassifiedAdId(Guid.NewGuid()), new UserId(Guid.NewGuid()));
            ad.SetTitle(ClassifiedAdTitle.FromTextOrHtml("Fine car!"));
            ad.UpdatePrice(Price.FromDecimal(1500m, "EUR", new FakeCurrencyLookup()));

            Assert.Throws<InvalidEntityStateException>(() => ad.RequestToPublish());
        }

        [Fact]
        public void Request_publishing_should_fail_when_price_is_missing()
        {
            var ad = new ClassifiedAd(new ClassifiedAdId(Guid.NewGuid()), new UserId(Guid.NewGuid()));
            ad.SetTitle(ClassifiedAdTitle.FromTextOrHtml("Fine car!"));
            ad.UpdateText(ClassifiedAdText.FromString("1981 Talbot. Mint condition, no rust"));

            Assert.Throws<InvalidEntityStateException>(() => ad.RequestToPublish());
        }

    }
}
