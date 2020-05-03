using System;
using System.Diagnostics.CodeAnalysis;
    
namespace MarketPlace.Domain.ClassifiedAd
{
    public class ClassifiedAdText : Value<ClassifiedAdTitle>
    {
        protected override object[] Values => new object[] { };

        internal ClassifiedAdText(string text) {
            Text = text;
        }

        public static ClassifiedAdText FromString([NotNull] string text) {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("text must not be empty", nameof(text));
            return new ClassifiedAdText(text);
        }

        public string Text { get; }
    }
}
