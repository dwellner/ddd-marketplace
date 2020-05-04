namespace MarketPlace.Domain.ClassifiedAd
{
    internal static class PictureExtensions
    {
        internal static bool HasValidSize(this Picture picture) => picture != null &&
            picture.pictureSize.Width >= 800 &&
            picture.pictureSize.Height >= 600;
    }
}
