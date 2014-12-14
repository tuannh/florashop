using System;
using System.Text;

namespace FloraShop.Core.Extensions
{
    public static class StringLocalizeExtensions
    {
        /// <summary>
        /// get localize the specified phrase.
        /// </summary>
        /// <param name="phrase">The phrase.</param>
        /// <param name="defaultText">The default text. This text will be inserted to database if no localize text found.</param>
        /// <returns> the specified localize phrase</returns>
        public static string Localize(this string phrase, string defaultText = "")
        {
            if (string.IsNullOrEmpty(defaultText))
                return phrase;

            return defaultText;
        }
    }
}
