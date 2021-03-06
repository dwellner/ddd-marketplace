﻿using System;
using System.Diagnostics.CodeAnalysis;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.ClassifiedAd
{
    public class ClassifiedAdText : Value<ClassifiedAdTitle>
    {
        protected override object[] GetValues() => new object[] { };

        protected ClassifiedAdText() { }

        internal ClassifiedAdText(string text) {
            Text = text;
        }

        public static ClassifiedAdText FromString([NotNull] string text) {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("text must not be empty", nameof(text));
            return new ClassifiedAdText(text);
        }

        public string Text { get; private set; }
    }
}
