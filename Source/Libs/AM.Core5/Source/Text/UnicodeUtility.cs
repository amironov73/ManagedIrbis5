// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* UnicodeUtility.cs -- полезные методы для обработки Unicode-текстов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using JetBrains.Annotations;

#endregion

namespace AM.Text;

/*

    Here's a concise list of pitfalls C# programmers should be aware
    of when working with Unicode strings:

    1. Assuming characters are always 16 bits (char data type).
       Some Unicode characters (like emojis) require more than 16 bits.
       Use StringInfo for accurate character counting.

       Example:
       string emoji = "😊";
       Console.WriteLine ($"Emoji length: {emoji.Length}"); // Output: 2
       // Proper way to count characters
       Console.WriteLine ($"Actual emoji count: {StringInfo.GetTextElementEnumerator (emoji).GetElementCount()}"); // Output: 1

    2. Incorrect string length calculations.
       Be aware that some characters may be represented differently
       (e.g., combined characters), affecting length calculations.

    3. Improper strings slicing.
       Use StringInfo or TextElementEnumerator for proper slicing
       of complex Unicode strings.

       Example:
       string complexText = "Hello 👋🏼 World";
       Console.WriteLine(complexText.Substring(6, 1)); // Might produce unexpected results

       // Proper way to slice strings
       var textElements = StringInfo.GetTextElementEnumerator (complexText);
       textElements.MoveNext(); // Skip "Hello "
       Console.WriteLine (textElements.GetTextElement()); // Output: 👋🏼

    4. Reverse iteration issues.
       Simple character array reversal can break multi-code point characters.
       Use StringInfo and EnumerateRunes for correct reversal.

       Example:
       string reverseMe = "Hello 👋🏼 World";
       char[] charArray = reverseMe.ToCharArray();
       Array.Reverse (charArray);
       Console.WriteLine (new string (charArray)); // Incorrect: dlroW 🏼👋 olleH

       // Proper way to reverse Unicode strings
       Console.WriteLine (string.Join ("", new StringInfo (reverseMe).String.EnumerateRunes().Reverse().ToArray())); // Correct: dlroW 👋🏼 olleH

    5. Case conversion complexities
       Be mindful of culture-specific case conversions, especially for languages with unique rules.

       Example:
       string lowercase = "istanbul";
       string uppercase = lowercase.ToUpper(CultureInfo.InvariantCulture);
       Console.WriteLine(uppercase); // Output: ISTANBUL

       string lowercase = "istanbul";
       string uppercase = lowercase.ToUpper(CultureInfo.InvariantCulture);
       Console.WriteLine(uppercase); // Output: ISTANBUL

       // Culture-specific case conversion
       Console.WriteLine (lowercase.ToUpper (new CultureInfo("tr-TR"))); // Output: İSTANBUL

    6. Normalization inconsistencies
       Always normalize strings before comparison to ensure equivalent
       representations are treated as equal.

       Example:
       string composed = "café";
       string decomposed = "cafe\u0301";
       Console.WriteLine (composed == decomposed); // Output: False
       Console.WriteLine (string.Equals (composed.Normalize(), decomposed.Normalize())); // Output: True

    7. Comparison and sorting challenges
       Use appropriate StringComparer instances, considering culture
       and case sensitivity requirements.

       Example:
       string[] words = { "résumé", "resume", "RESUME" };
       Array.Sort (words, StringComparer.InvariantCulture);
       Console.WriteLine (string.Join (", ", words)); // Output might not be as expected

       // Culture-specific sorting
       Array.Sort (words, StringComparer.Create (new CultureInfo("fr-FR"), ignoreCase: true));
       Console.WriteLine (string.Join(", ", words)); // More appropriate for French text


    8. Incorrect regular expression handling
       Use Unicode-aware regex patterns
       (e.g., \p{L} for letters, \p{M} for combining marks).

       Example:
       string unicodeText = "Hello 👋🏼 World";
       Console.WriteLine (Regex.IsMatch (unicodeText, @"\p{L}+")); // Matches only "Hello" and "World"

       // Use Unicode-aware regex
       Console.WriteLine (Regex.IsMatch (unicodeText, @"\p{L}+\p{M}*")); // Matches "Hello", "👋🏼", and "World"

    9. File system encoding mismatches
       Always specify the encoding (preferably UTF-8) when reading
       from or writing to files.


    10. Database storage and retrieval issues
        Use Unicode-compatible column types and parameterized
        queries to avoid encoding problems.

 */

/// <summary>
/// Полезные методы для обработки Unicode-текстов.
/// </summary>
[PublicAPI]
public static class UnicodeUtility
{
    #region Private members

    // Список служебных символов Unicode
    private static char[] _specialUnicodeCharacters =
    [
        '\u00AD', // Soft hyphen
        '\u00A0', // Non-breaking space
        '\u200B', // Zero-width space
        '\u200C', // Zero-width non-joiner
        '\u200D', // Zero-width joiner
        '\u202F', // Narrow no-break space
        '\u2060', // Word joiner
        '\uFEFF'  // Zero-width no-break space
    ];

    // Список символов, которые могут использоваться
    // для эксплуатации уязвимостей
    private static char[] _dangerousCharacters =
    [
        '\u200B', // Zero Width Space
        '\u202A', // Left-to-Right Embedding
        '\u202B', // Right-to-Left Embedding
        '\u202D', // Left-to-Right Override
        '\u202E', // Right-to-Left Override
        '\u2066', // Left-to-Right Isolate
        '\u2067', // Right-to-Left Isolate
        '\u2068', // First Strong Isolate
        '\u202C', // Pop Directional Formatting
        '\u2069'  // Pop Directional Isolate
    ];

    #endregion

    #region Public methods

    /// <summary>
    /// Перечисление кодов символов в строке.
    /// </summary>
    public static IEnumerable<Codepoint> Codepoints
        (
            this string text
        )
    {
        for (var i = 0; i < text.Length; ++i)
        {
            if (char.IsHighSurrogate (text[i]))
            {
                if (text.Length < i + 2)
                {
                    throw new InvalidEncodingException();
                }

                if (!char.IsLowSurrogate (text[i + 1]))
                {
                    throw new InvalidEncodingException();
                }

                yield return new Codepoint (char.ConvertToUtf32 (text[i], text[++i]));
            }
            else
            {
                yield return new Codepoint ((int)text[i]);
            }
        }
    }

    /// <summary>
    /// Кириллическая буква (включая базовый и расширенный набор)?
    /// Цифры и прочие знаки выдают <c>false</c>.
    /// </summary>
    public static bool IsCyrillicLetter (uint codepoint) =>
        codepoint is >= 0x0400u and <= 0x0481u // Basic Cyrillic
            or >= 0x048Au and <= 0x04FFu
            or >= 0x0500u and <= 0x052Fu; // Cyrillic Supplement

    /// <summary>
    /// Латинская буква (включая базовый и расширенный набор)?
    /// Цифры и прочие знаки выдают <c>false</c>.
    /// </summary>
    public static bool IsLatinLetter (uint codepoint) =>
        codepoint is >= 'A' and <= 'Z' or >= 'a' and <= 'z' // Basic Latin
            or >= 0xC0u and <= 0xD6u or >= 0xD8u and 0xF6u or >= 0xF8u and <= 0xFFu // Latin supplement
            or >= 0x0100u and <= 0x017Fu // Latin Extended A
            or >= 0x0180u and <= 0x024Fu; // Latin Extended B

    /// <summary>
    /// Текст содержит символы, не входящие в состав слова?
    /// </summary>
    public static bool TextContainsNonLatinNorCyrillicSymbols
        (
            string text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return false;
        }

        var codepoints = text.Codepoints();
        foreach (var chr in codepoints)
        {
            var value = chr.Value;
            if (!IsLatinLetter (value) && !IsCyrillicLetter (value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Перечисление символов в заданном тексте.
    /// </summary>
    public static IEnumerable<string> Letters
        (
            this string text
        )
    {
        for (var i = 0; i < text.Length; ++i)
        {
            if (char.IsHighSurrogate (text[i]))
            {
                if (text.Length < i + 2)
                {
                    throw new InvalidEncodingException();
                }

                if (!char.IsLowSurrogate (text[i + 1]))
                {
                    throw new InvalidEncodingException();
                }

                yield return $"{text[i]}{text[++i]}";
            }
            else
            {
                yield return $"{text[i]}";
            }
        }
    }

    /// <summary>
    /// Конвертация в последовательность кодов символов.
    /// </summary>
    public static UnicodeSequence AsUnicodeSequence
        (
            this string text
        )
    {
        return new UnicodeSequence (text.Codepoints());
    }

    /// <summary>
    /// Удаление из строки символов soft hyphen, non-breaking space и аналогичных
    /// </summary>
    [return: NotNullIfNotNull ("input")]
    public static string? RemoveSpecialCharacters
        (
            this string? input
        )
    {
        if (string.IsNullOrEmpty (input))
        {
            return input;
        }


        // сначала проверяем, есть ли смысл создавать новую строку
        var found = false;
        foreach (var c in input)
        {
            if (Array.IndexOf (_specialUnicodeCharacters, c) is not -1)
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            return input;
        }

        var result = StringBuilderPool.Shared.Get();
        result.EnsureCapacity (input.Length);
        foreach (var c in input)
        {
            if (Array.IndexOf (_specialUnicodeCharacters, c) is -1)
            {
                result.Append (c);
            }
        }

        return result.ReturnShared();
    }

    /// <summary>
    /// Удаление из строки символа ударения.
    /// </summary>
    [return: NotNullIfNotNull ("input")]
    public static string? StripAccentMarks
        (
            string? input
        )
    {
        if (string.IsNullOrEmpty (input))
        {
            return input;
        }

        // сначала проверяем, нужно ли это все
        var found = false;
        foreach (var c in input)
        {
            if (c is '\u0301')
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            return input;
        }

        var result = StringBuilderPool.Shared.Get();
        result.EnsureCapacity (input.Length);
        foreach (var c in input)
        {
            if (c is not '\u0301')
            {
                result.Append (c);
            }
        }

        return result.ReturnShared();
    }

    /// <summary>
    /// Сравнение строк с предварительной их нормализацией.
    /// </summary>
    /// <example>
    /// <code>
    /// string composed = "café";
    /// string decomposed = "cafe\u0301";
    /// Console.WriteLine (composed == decomposed); // Output: False
    /// Console.WriteLine (string.Equals(composed.Normalize(),
    ///         decomposed.Normalize())); // Output: True
    /// </code>
    /// </example>
    public static bool NormalizedEquals (string first, string second)
        => string.Equals (first.Normalize(), second.Normalize());

    /// <summary>
    /// Сравнение строк с предварительной их нормализацией.
    /// </summary>
    public static bool NormalizedEquals (string first, string second, StringComparison comparison)
        => string.Equals
            (
                first.Normalize(),
                second.Normalize(),
                comparison
            );

    /// <summary>
    /// Сравнение строк с точностью до диакретических символов.
    /// </summary>
    /// <example>
    /// <code>
    /// string s1 = "résumé";
    /// string s2 = "resume";
    /// string s3 = "rèsumè";
    ///
    /// Console.WriteLine ($"'résumé' vs 'resume': {string.Compare (s1, s2, CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace) == 0}");
    /// Console.WriteLine ($"'résumé' vs 'rèsumè': {string.Compare (s1, s3, CultureInfo.InvariantCulture, CompareOptions.IgnoreNonSpace) == 0}");
    /// </code>
    /// </example>
    public static int CompareIgnoringNonSpace
        (
            string first,
            string second
        )
        => string.Compare
            (
                first,
                second,
                CultureInfo.InvariantCulture,
                CompareOptions.IgnoreNonSpace
            );

    /// <summary>
    /// Сравнение строк с точностью до диакретических символов.
    /// </summary>
    public static int CompareIgnoringNonSpace
        (
            string first,
            string second,
            CultureInfo culture
        )
        => string.Compare
            (
                first,
                second,
                culture,
                CompareOptions.IgnoreNonSpace
            );

    /// <summary>
    /// Содержит ли указанный текст символы Unicode,
    /// допускающие эксплуатацию уязвимостей?
    /// </summary>
    /// <remarks>
    /// Многие редакторы кода и инструменты разработки не отображают
    /// символы BIDI и другие специальные символы Unicode, что делает
    /// их невидимыми для разработчиков. Это приводит к тому,
    /// что отображаемый код может выглядеть иначе, чем код, который
    /// фактически выполняется компилятором. Таким образом, злоумышленник
    /// может скрыть вредоносные изменения, что создает возможность
    /// для внедрения уязвимостей в программное обеспечение, особенно
    /// в открытых проектах. Это создает риск, что разработчик случайно
    /// вставит вредоносный код, скопировав его из уязвимого источника.
    /// Таким образом, уязвимость BIDI может быть использована
    /// для обхода стандартных проверок безопасности.
    /// </remarks>
    public static bool ContainsAnyDangerousCharacters
        (
            ReadOnlySpan<char> text
        )
    {
        foreach (var c in text)
        {
            if (Array.IndexOf (_dangerousCharacters, c) != -1)
            {
                return true;
            }

        }

        return false;
    }

    #endregion
}
