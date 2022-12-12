// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Codepoints.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Text;

/// <summary>
/// Helpful definitions for commonly- and not-so-commonly-used codepoints.
/// </summary>
public static class Codepoints
{
    /// <summary>
    /// The right-to-left mark
    /// </summary>
    public static readonly Codepoint RLM = new ("U+200F");

    /// <summary>
    /// The left-to-right mark
    /// </summary>
    public static readonly Codepoint LRM = new ("U+200E");

    /// <summary>
    /// ZWJ is used to combine multiple emoji codepoints into a single emoji symbol.
    /// </summary>
    public static readonly Codepoint ZWJ = new ("U+200D");

    /// <summary>
    /// ORC is used as a placeholder to indicate an object should replace this codepoint in the string.
    /// </summary>
    public static readonly Codepoint ObjectReplacementCharacter = new ("U+FFFC");

    /// <summary>
    ///
    /// </summary>
    public static readonly Codepoint ORC = ObjectReplacementCharacter;

    /// <summary>
    /// The "combined enclosing keycap" is used by emoji to box icons
    /// </summary>
    public static readonly Codepoint Keycap = new ("U+20E3");

    /// <summary>
    /// Variation selectors come after a unicode codepoint to indicate that it should be represented in a particular format.
    /// </summary>
    public static class VariationSelectors
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly Codepoint VS15 = new ("U+FE0E");

        /// <summary>
        ///
        /// </summary>
        public static readonly Codepoint TextSymbol = VS15;

        /// <summary>
        ///
        /// </summary>
        public static readonly Codepoint VS16 = new ("U+FE0F");

        /// <summary>
        ///
        /// </summary>
        public static readonly Codepoint EmojiSymbol = VS16;
    }
}
