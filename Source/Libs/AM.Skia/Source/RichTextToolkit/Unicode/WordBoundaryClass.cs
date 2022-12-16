// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* WordBoundaryClass.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Skia.RichTextKit;

/// <summary>
/// Unicode word boundary group classes
/// </summary>
/// <remarks>
/// Note, these need to match those used by the JavaScript script that
/// generates the .trie resources
/// </remarks>
internal enum WordBoundaryClass
{
    /// <summary>
    /// Character is an letter or number
    /// </summary>
    AlphaDigit = 0,

    /// <summary>
    /// Character should be ignored when locating word boundaries
    /// </summary>
    Ignore = 1,

    /// <summary>
    /// Character is a spacing character
    /// </summary>
    Space = 2,

    /// <summary>
    /// Character is a punctuation character
    /// </summary>
    Punctuation = 3,
}
