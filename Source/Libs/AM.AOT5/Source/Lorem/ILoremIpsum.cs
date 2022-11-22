// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ILoremIpsum.cs -- общий интерфейс генератора бреда
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Text.Lorem;

/// <summary>
/// Общий интерфейс генератора бреда.
/// </summary>
public interface ILoremIpsum
{
    /// <summary>
    /// Gets a loremipsum string with length of given lenght of letters.
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns>A lorem ipsum string with a given length.</returns>
    string GetLetters (int length);

    /// <summary>
    /// Gets a loremipsum string of given lenght of words.
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns>a lorem ipsum string</returns>
    string GetWords (int length);

    /// <summary>
    /// Gets a loremipsum string of given lenght of paragraphs.
    /// </summary>
    /// <returns>a lorem ipsum string</returns>
    string GetParagraphs (int count);
}
