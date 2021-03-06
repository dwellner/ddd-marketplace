﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using MarketPlace.Domain.Framework;

namespace MarketPlace.Domain.ClassifiedAd
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        const int MAX_LENGTH = 100;

        protected override object[] GetValues() => new object[] { };

        protected ClassifiedAdTitle() { }

        internal ClassifiedAdTitle(string text)
        {
            Text = text;
        }

        public static ClassifiedAdTitle FromTextOrHtml([NotNull] string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("text must not be empty", nameof(text));
            if (text.Length > MAX_LENGTH) throw new ArgumentException($"text must not exceed {MAX_LENGTH} characters", nameof(text));

            return new ClassifiedAdTitle(Regex.Replace(text, "<.*?>", string.Empty));
        }

        public string Text { get; private set; }
    }
}
