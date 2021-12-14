// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* Resolve.cs -- набор парсеров для нужд ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Globalization;

using AM.Text;

using Sprache;

#endregion

#nullable enable

namespace AM.Scripting
{
    /// <summary>
    /// Набор парсеров для нужд ИРБИС.
    /// </summary>
    public static class Resolve
    {
        #region Fields

        /// <summary>
        /// Знак минус.
        /// </summary>
        public static readonly Parser<char> Minus = Parse.Char ('-');

        /// <summary>
        /// Точка.
        /// </summary>
        public static readonly Parser<char> Dot = Parse.Char ('.');

        /// <summary>
        /// Символ подчеркивания.
        /// </summary>
        public static readonly Parser<char> Underscore = Parse.Char ('_');

        /// <summary>
        /// Арабские цифры.
        /// </summary>
        public static readonly Parser<char> Arabic
            = Parse.Chars ("0123456789").Named ("arabic digit");

        /// <summary>
        /// Латинская буква.
        /// </summary>
        public static readonly Parser<char> Latin
            = Parse.Chars ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
                .Named ("latin character");

        /// <summary>
        /// Латинская буква или символ пдчеркивания.
        /// </summary>
        public static readonly Parser<char> LatinOrUnderscore
            = Latin.Or (Underscore);

        /// <summary>
        /// Латинская буква, арабская цифра или символ пдчеркивания.
        /// </summary>
        public static readonly Parser<char> LatinOrArabicOrUnderscore
            = Latin.Or (Arabic).Or (Underscore);

        /// <summary>
        /// Общепринятый идентификатор.
        /// </summary>
        public static readonly Parser<string> Identifier =
            from firstLetter in LatinOrUnderscore
            from tail in LatinOrArabicOrUnderscore.Many().Text()
            select firstLetter + tail;

        /// <summary>
        /// Беззнаковое целое.
        /// </summary>
        public static readonly Parser<uint> UnsignedInteger =
            Arabic.AtLeastOnce().Text().Select
                (
                    s => uint.Parse (s, CultureInfo.InvariantCulture)
                );

        /// <summary>
        /// Целое со знаком.
        /// </summary>
        public static readonly Parser<int> SignedInteger =
            Minus.Perhaps().Concat (Arabic.AtLeastOnce()).Text().Select
                (
                    s => int.Parse (s, CultureInfo.InvariantCulture)
                );

        /// <summary>
        /// Беззнаковое длинное целое.
        /// </summary>
        public static readonly Parser<ulong> UnsignedLong =
            Arabic.AtLeastOnce()
                .Text()
                .Select (s => ulong.Parse (s, CultureInfo.InvariantCulture));

        /// <summary>
        /// Длинное целое со знаком.
        /// </summary>
        public static readonly Parser<long> SignedLong =
            Minus.Perhaps()
                .Concat (Arabic.AtLeastOnce())
                .Text()
                .Select (s => long.Parse (s, CultureInfo.InvariantCulture));

        private static readonly Parser<string> FloatBody =
            from minus in Minus.Perhaps().Text()
            from whole in Arabic.AtLeastOnce().Text()
            from frac in Dot.Once().Concat (Arabic.AtLeastOnce()).Text().Optional()
            from exp in Parse.Chars ('e', 'E').Once()
                .Concat (Minus.Perhaps())
                .Concat (Arabic.AtLeastOnce())
                .Text().Optional()
            select minus + whole + frac.GetOrDefault() + exp.GetOrDefault();

        /// <summary>
        /// Число с плавающей точкой одинарной точности.
        /// </summary>
        public static readonly Parser<float> Float =
            FloatBody.Select (s => float.Parse (s, CultureInfo.InvariantCulture));

        /// <summary>
        /// Число с плавающей точкой двойной точности.
        /// </summary>
        public static readonly Parser<double> Double =
            FloatBody.Select (s => double.Parse (s, CultureInfo.InvariantCulture));

        #endregion

        #region Extension methods

        /// <summary>
        /// Экранирование в строке специальных символов,
        /// таких как <c>\n</c>.
        /// </summary>
        public static string? EscapeText
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty (text))
            {
                return text;
            }

            var builder = StringBuilderPool.Shared.Get();
            builder.EnsureCapacity (text.Length);

            foreach (var c in text)
            {
                switch (c)
                {
                    case '\a':
                        builder.Append ("\\a");
                        break;

                    case '\b':
                        builder.Append ("\\b");
                        break;

                    case '\f':
                        builder.Append ("\\f");
                        break;

                    case '\n':
                        builder.Append ("\\n");
                        break;

                    case '\r':
                        builder.Append ("\\r");
                        break;

                    case '\t':
                        builder.Append ("\\t");
                        break;

                    case '\v':
                        builder.Append ("\\v");
                        break;

                    case '\\':
                        builder.Append ("\\\\");
                        break;

                    case '"':
                        builder.Append ("\\\"");
                        break;

                    default:
                        if (c < ' ')
                        {
                            builder.Append ($"\\u{((int) c):xxxx}");
                        }
                        else
                        {
                            builder.Append (c);
                        }
                        break;
                }
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        }

        /// <summary>
        /// Преобразует строку, содержащую escape-последовательности,
        /// к нормальному виду.
        /// </summary>
        public static string? UnescapeText
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty (text))
            {
                return text;
            }

            var navigator = new TextNavigator (text);
            var builder = StringBuilderPool.Shared.Get();
            builder.EnsureCapacity (text.Length);

            while (!navigator.IsEOF)
            {
                var c = navigator.ReadChar();
                if (c == '\\')
                {
                    var c2 = navigator.ReadChar();
                    c2 = c2 switch
                    {
                        'a' => '\a',
                        'b' => '\b',
                        'f' => '\f',
                        'n' => '\n',
                        'r' => '\r',
                        't' => '\t',
                        'u' => ParseUnicode(),
                        'v' => '\v',
                        '\'' => '\'',
                        '"' => '"',
                        '\\' => '\\',
                        '0' => '\0',
                        _ => '?'
                    };
                    builder.Append (c2);
                }
                else
                {
                    builder.Append (c);
                }
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;

            char ParseUnicode()
            {
                return (char) int.Parse
                    (
                        navigator.ReadString (4).Span,
                        NumberStyles.HexNumber
                    );
            }
        }

        /// <summary>
        /// Парсинг строкового литерала с экранированными символами.
        /// </summary>
        public static Parser<string> EscapedLiteral
            (
                char limiter = '"',
                char escapeSymbol = '\\'
            )
        {
            var escapist = new Escapist (limiter, escapeSymbol);

            return i => escapist.Parse (i);
        }

        /// <summary>
        /// Повторяется ноль или один раз.
        /// </summary>
        public static Parser<IEnumerable<T>> Perhaps<T> (this Parser<T> parser) => parser.Repeat (0, 1);

        /// <summary>
        /// Выражение в круглых скобках.
        /// </summary>
        public static Parser<T>? RoundBraces<T> (this Parser<T>? parser) =>
            parser.Contained (Parse.Char ('(').Token(), Parse.Char (')').Token());

        /// <summary>
        /// Выражение в фигурных скобках.
        /// </summary>
        public static Parser<T>? CurlyBraces<T> (this Parser<T>? parser) =>
            parser.Contained (Parse.Char ('{').Token(), Parse.Char ('}').Token());

        #endregion
    }
}
