using MarketPlace.Domain;
using Xunit;

namespace MarketPlace.Test
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
