// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Extensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Text.Searching;

/// <summary>
/// Provides extension methods.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Determines whether this instance contains the specified words.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="words">The words.</param>
    /// <returns>The matched words.</returns>
    public static IEnumerable<WordMatch> Contains
        (
            this string text,
            IEnumerable<string> words
        )
    {
        return new AhoCorasick (words).Search (text);
    }

    /// <summary>
    /// Determines whether this instance contains the specified words.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="words">The words.</param>
    /// <returns>The matched words.</returns>
    public static IEnumerable<WordMatch> Contains
        (
            this string text,
            params string[] words
        )
    {
        return new AhoCorasick (words).Search (text);
    }

    /// <summary>
    /// Determines whether this instance contains the specified words.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="comparer">The comparer used to compare individual characters.</param>
    /// <param name="words">The words.</param>
    /// <returns>The matched words.</returns>
    public static IEnumerable<WordMatch> Contains
        (
            this string text,
            IEqualityComparer<char> comparer,
            IEnumerable<string> words
        )
    {
        return new AhoCorasick (comparer, words).Search (text);
    }

    /// <summary>
    /// Determines whether this instance contains the specified words.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="comparer">The comparer used to compare individual characters.</param>
    /// <param name="words">The words.</param>
    /// <returns>The matched words.</returns>
    public static IEnumerable<WordMatch> Contains
        (
            this string text,
            IEqualityComparer<char> comparer,
            params string[] words
        )
    {
        return new AhoCorasick (comparer, words).Search (text);
    }
}
