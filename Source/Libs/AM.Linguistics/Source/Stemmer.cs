// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Stemmer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Linguistics
{
    /// <summary>
    /// Стеммер Портера
    /// </summary>
    public static class Stemmer
    {
        private const string VOWEL = "аеиоуыэюя";
        private const string PERFECTIVEGROUND = "((ив|ивши|ившись|ыв|ывши|ывшись)|((?<=[ая])(в|вши|вшись)))$";
        private const string REFLEXIVE = "(с[яь])$";
        private const string ADJECTIVE = "(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|еых|ую|юю|ая|яя|ою|ею)$";
        private const string PARTICIPLE = "((ивш|ывш|ующ)|((?<=[ая])(ем|нн|вш|ющ|щ)))$";

        private const string VERB =
            "((ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ены|ить|ыть|ишь|ую|ю)|((?<=[ая])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)))$";

        private const string NOUN = "(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|и|ы|ь|ию|ью|ю|ия|ья|я)$";
        private const string RVRE = "^(.*?[аеиоуыэюя])(.*)$";
        private const string DERIVATIONAL = "[^аеиоуыэюя][аеиоуыэюя]+[^аеиоуыэюя]+[аеиоуыэюя].*(?<=о)сть?$";
        private const string SUPERLATIVE = "(ейше|ейш)?";

        /// <summary>
        /// Parses stemm of the word
        /// </summary>
        /// <param name="word"></param>
        /// <returns>Stemm of the word</returns>
        public static string Stemm (string word)
        {
            word = word.ToLower();
            word = word.Replace ("ё", "е");
            if (IsMatch (word, RVRE))
            {
                // Step 1
                if (!Replace (ref word, PERFECTIVEGROUND, ""))
                {
                    Replace (ref word, REFLEXIVE, "");
                    if (Replace (ref word, ADJECTIVE, ""))
                    {
                        Replace (ref word, PARTICIPLE, "");
                    }
                    else
                    {
                        if (!Replace (ref word, VERB, ""))
                        {
                            Replace (ref word, NOUN, "");
                        }
                    }
                }

                // Step 2
                Replace (ref word, "и$", "");

                // Step 3
                if (IsMatch (word, DERIVATIONAL))
                {
                    Replace (ref word, "ость?$", "");
                }

                // Step 4
                if (!Replace (ref word, "ь$", ""))
                {
                    Replace (ref word, SUPERLATIVE, "");
                    Replace (ref word, "нн$", "н");
                }
            }

            return word;
        }

        private static bool IsMatch (string word, string matchingPattern)
        {
            return new Regex (matchingPattern).IsMatch (word);
        }

        private static bool Replace (ref string replace, string cleaningPattern, string by)
        {
            string original = replace;
            replace = new Regex (cleaningPattern,
                    RegexOptions.ExplicitCapture |
                    RegexOptions.Singleline
                ).Replace (replace, by);
            return original != replace;
        }
    }
}
