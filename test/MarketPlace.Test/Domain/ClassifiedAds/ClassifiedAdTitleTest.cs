using MarketPlace.Domain.ClassifiedAd;
using Xunit;

namespace MarketPlace.Test.Domain.ClassifiedAds
{
    public class ClassifiedAdTitleTest
    {
        [Fact]
        public void ShouldRemoveHtmlTags()
        {
            var title = ClassifiedAdTitle.FromTextOrHtml("<b>This is <i>my</i> ad</b>");
            Assert.Equal("This is my ad", title.Text);
        }

    }
}
