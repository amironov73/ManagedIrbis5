// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DefaultCharacterMatcher.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using SkiaSharp;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit;

internal class DefaultCharacterMatcher
    : ICharacterMatcher
{
    private SKFontManager _fontManager = SKFontManager.Default;

    /// <inheritdoc />
    public SKTypeface MatchCharacter
        (
            string familyName,
            int weight,
            int width,
            SKFontStyleSlant slant,
            string[]? bcp47,
            int character
        )
    {
        return _fontManager.MatchCharacter (familyName, weight, width, slant, bcp47, character);
    }
}
