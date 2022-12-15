// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using SkiaSharp;

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit;

internal class DefaultCharacterMatcher : ICharacterMatcher
{
    public DefaultCharacterMatcher()
    {
    }

    private SKFontManager _fontManager = SKFontManager.Default;

    /// <inheritdoc />
    public SKTypeface MatchCharacter (string familyName, int weight, int width, SKFontStyleSlant slant, string[] bcp47,
        int character)
    {
        return _fontManager.MatchCharacter (familyName, weight, width, slant, bcp47, character);
    }
}
