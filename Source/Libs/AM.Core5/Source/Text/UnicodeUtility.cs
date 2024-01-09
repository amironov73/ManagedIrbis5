// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* UnicodeUtility.cs -- полезные методы для обработки Unicode-текстов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

namespace AM.Text;

/// <summary>
/// Полезные методы для обработки Unicode-текстов.
/// </summary>
public static class UnicodeUtility
{
    #region Public methods

    /// <summary>
    ///
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
                yield return new Codepoint ((int) text[i]);
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
            or >= 0x0500u and <= 0x052Fu // Cyrillic Supplement
    ;

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

        var codepoints = text.Codepoints ();
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
    ///
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
    ///
    /// </summary>
    public static UnicodeSequence AsUnicodeSequence
        (
            this string text
        )
    {
        return new UnicodeSequence (text.Codepoints());
    }

    #endregion
}
