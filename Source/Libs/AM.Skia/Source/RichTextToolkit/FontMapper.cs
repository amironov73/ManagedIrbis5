// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FontMapper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using SkiaSharp;

#endregion

#pragma warning disable CA2211

#nullable enable

namespace AM.Skia.RichTextKit;

/// <summary>
/// The FontMapper class is responsible for mapping style typeface information
/// to an SKTypeface.
/// </summary>
public class FontMapper
{
    #region Properties

    /// <summary>
    /// The default font mapper instance.
    /// </summary>
    /// <remarks>
    /// The default font mapper is used by any TextBlocks that don't
    /// have an explicit font mapper set (see the <see cref="TextBlock.FontMapper"/> property).
    ///
    /// Replacing the default font mapper allows changing the font mapping
    /// for all text blocks that don't have an explicit mapper assigned.
    /// </remarks>
    public static FontMapper Default = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Maps a given style to a specific typeface
    /// </summary>
    /// <param name="style">The style to be mapped</param>
    /// <param name="ignoreFontVariants">Indicates the mapping should ignore font variants (use to get font for ellipsis)</param>
    /// <returns>A mapped typeface</returns>
    public virtual SKTypeface TypefaceFromStyle
        (
            IStyle style,
            bool ignoreFontVariants
        )
    {
        // Extra weight for superscript/subscript
        var extraWeight = 0;
        if (!ignoreFontVariants &&
            style.FontVariant is FontVariant.SuperScript or FontVariant.SubScript)
        {
            extraWeight += 100;
        }

        // Get the typeface
        return SKTypeface.FromFamilyName
            (
                style.FontFamily,
                weight: (SKFontStyleWeight)(style.FontWeight + extraWeight),
                style.FontWidth,
                style.FontItalic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright
            )
            ?? SKTypeface.CreateDefault();
    }

    #endregion
}
