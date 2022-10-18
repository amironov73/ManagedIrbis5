// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* UnicodeUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
///
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
