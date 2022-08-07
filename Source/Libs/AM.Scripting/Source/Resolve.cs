// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* Resolve.cs -- набор парсеров для нужд ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Numerics;

using AM.Text;

using Pidgin;

using static Pidgin.Parser;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Набор парсеров для нужд ИРБИС.
/// </summary>
public static class Resolve
{
    #region Fields

    /// <summary>
    /// Знак минус.
    /// </summary>
    public static readonly Parser<char, char> Minus = Char ('-');

    /// <summary>
    /// Точка.
    /// </summary>
    public static readonly Parser<char, char> Dot = Char ('.');

    /// <summary>
    /// Символ подчеркивания.
    /// </summary>
    public static readonly Parser<char, char> Underscore = Char ('_');

    /// <summary>
    /// Арабские цифры.
    /// </summary>
    public static readonly Parser<char, char> Arabic = OneOf
        (
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        )
        .Labelled ("arabic digit");

    /// <summary>
    /// Латинская буква.
    /// </summary>
    public static readonly Parser<char, char> Latin = OneOf
        (
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
            'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
            'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z'
        )
        .Labelled ("latin character");

    /// <summary>
    /// Латинская буква или символ пдчеркивания.
    /// </summary>
    public static readonly Parser<char, char> LatinOrUnderscore = OneOf
        (
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
            'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
            'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', '_'
        );

    /// <summary>
    /// Латинская буква, арабская цифра или символ пдчеркивания.
    /// </summary>
    public static readonly Parser<char, char> LatinOrArabicOrUnderscore = OneOf
        (
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
            'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
            'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '0', '_'
        );

    /// <summary>
    /// Общепринятый идентификатор.
    /// </summary>
    public static readonly Parser<char, string> Identifier = Map
        (
            (first, tail) => first + tail,
            LatinOrUnderscore,
            LatinOrArabicOrUnderscore.ManyString()
        );

    /// <summary>
    /// Литерал-строка с экранированными символами.
    /// </summary>
    public static Parser<char, string> EscapedStringLiteral() =>
        new EscapeParser ('"', '\\');

    #endregion

    #region Extension methods

    /// <summary>
    /// Добавляет поглощение пробельных символов перед указанным парсером.
    /// </summary>
    public static Parser<char, TResult> Clear<TResult>
        (
            Parser<char, TResult> parser
        )
    {
        return new ClearParser<TResult> (parser);
    }

    /// <summary>
    /// Число плавающей точкой двойной точности.
    /// </summary>
    public static readonly Parser<char, double> Double = new DoubleParser();

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

        return builder.ReturnShared();
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

        return builder.ReturnShared();

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
    /// Создание превью-парсера.
    /// </summary>
    /// <param name="first">Первый парсер -- его результат нам нужен.</param>
    /// <param name="second">Второй парсер -- его результат отбрасывается,
    /// он должен "заглянуть вперед" и сообщить, имеет ли смысл разбирать дальше.
    /// </param>
    public static Parser<TToken, TResult> Preview<TToken, TResult>
        (
            Parser<TToken, TResult> first,
            Parser<TToken, Unit> second
        )
    {
        return new PreviewParser<TToken, TResult> (first, second);
    }

    /// <summary>
    /// Чтение всей строки вплоть до конца.
    /// </summary>
    public static Parser<char, string> ReadLine
        (
            bool eatEol = false
        )
    {
        return new ReadLineParser (eatEol);
    }

    /// <summary>
    /// Парсер, выдающий нераспарсенный остаток текста.
    /// </summary>
    public static readonly Parser<char, string> Remainder = new RemainderParser();

    /// <summary>
    /// Продвинутое целое число (с подчеркиваниями).
    /// </summary>
    /// <param name="base">Основание системы счисления.</param>
    /// <param name="prefix">Префикс, например, <c>"0x"</c> (опционально).</param>
    /// <param name="suffixes">Суффиксы, например, <c>"l"</c> или <c>"UL"</c>
    /// (опционально).
    /// </param>
    public static Parser<char, int> Int32
        (
            int @base = 10,
            string? prefix = null,
            string[]? suffixes = null
        )
    {
        var parser = new AdvancedNumberParser (@base, prefix, suffixes);

        return parser.Select (value => parser.ParseInt32 (value));
    }

    /// <summary>
    /// Продвинутое целое число (с подчеркиваниями).
    /// </summary>
    /// <param name="base">Основание системы счисления.</param>
    /// <param name="prefix">Префикс, например, <c>"0x"</c> (опционально).</param>
    /// <param name="suffixes">Суффиксы, например, <c>"l"</c> или <c>"UL"</c>
    /// (опционально).
    /// </param>
    public static Parser<char, long> Int64
        (
            int @base = 10,
            string? prefix = null,
            string[]? suffixes = null
        )
    {
        var parser = new AdvancedNumberParser (@base, prefix, suffixes);

        return parser.Select (value => parser.ParseInt64 (value));
    }

    /// <summary>
    /// Продвинутое целое число (с подчеркиваниями).
    /// </summary>
    /// <param name="base">Основание системы счисления.</param>
    /// <param name="prefix">Префикс, например, <c>"0x"</c> (опционально).</param>
    /// <param name="suffixes">Суффиксы, например, <c>"l"</c> или <c>"UL"</c>
    /// (опционально).
    /// </param>
    public static Parser<char, uint> UInt32
        (
            int @base = 10,
            string? prefix = null,
            string[]? suffixes = null
        )
    {
        var parser = new AdvancedNumberParser (@base, prefix, suffixes, false);

        return parser.Select (value => parser.ParseUInt32 (value));
    }

    /// <summary>
    /// Продвинутое целое число (с подчеркиваниями).
    /// </summary>
    /// <param name="base">Основание системы счисления.</param>
    /// <param name="prefix">Префикс, например, <c>"0x"</c> (опционально).</param>
    /// <param name="suffixes">Суффиксы, например, <c>"l"</c> или <c>"UL"</c>
    /// (опционально).
    /// </param>
    public static Parser<char, ulong> UInt64
        (
            int @base = 10,
            string? prefix = null,
            string[]? suffixes = null
        )
    {
        var parser = new AdvancedNumberParser (@base, prefix, suffixes, false);

        return parser.Select (value => parser.ParseUInt64 (value));
    }

    /// <summary>
    /// Целое число произвольной длины.
    /// </summary>
    /// <param name="suffixes">Суффиксы, например, <c>"l"</c> или <c>"UL"</c>
    /// (опционально).
    /// </param>
    public static Parser<char, BigInteger> BigInteger
        (
            string[]? suffixes = null
        )
    {
        var parser = new AdvancedNumberParser (10, null, suffixes);

        return parser.Select (System.Numerics.BigInteger.Parse);
    }

    /// <summary>
    /// Парсер, поглощающий комментарии и пробелы.
    /// </summary>
    public static Parser<char, Unit> Swallow
        (
            params char[] delimiters
        )
    {
        return new SwallowParser (delimiters);
    }

    #endregion
}
