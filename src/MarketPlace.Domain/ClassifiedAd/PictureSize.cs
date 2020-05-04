using System;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.ClassifiedAd
{
    public class PictureSize : Value<PictureSize>
    {
        public int Width { get; internal set; }
        public int Height { get; internal set; }

        protected override object[] Values => new object[] { Width, Height };

        public PictureSize(int width, int height)
        {
            Width = (width > 0) ? width : throw new ArgumentException("Image width must be greater than zero", nameof(width));
            Height = (height > 0) ? height : throw new ArgumentException("Image height must be greater than zero", nameof(height));
        }

        internal PictureSize() { }
    }
}
