// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PlatformFontResolver.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using PdfSharpCore.Drawing;

#endregion

#nullable enable

#pragma warning disable 1591

namespace PdfSharpCore.Fonts;

/// <summary>
/// Default platform specific font resolving.
/// </summary>
public static class PlatformFontResolver
{
    /// <summary>
    /// Resolves the typeface by generating a font resolver info.
    /// </summary>
    /// <param name="familyName">Name of the font family.</param>
    /// <param name="isBold">Indicates whether a bold font is requested.</param>
    /// <param name="isItalic">Indicates whether an italic font is requested.</param>
    public static FontResolverInfo? ResolveTypeface
        (
            string familyName,
            bool isBold,
            bool isItalic
        )
    {
        var fontResolvingOptions = new FontResolvingOptions (FontHelper.CreateStyle (isBold, isItalic));

        return ResolveTypeface
            (
                familyName,
                fontResolvingOptions,
                XGlyphTypeface.ComputeKey (familyName, fontResolvingOptions)
            );
    }

    /// <summary>
    /// Internal implementation.
    /// </summary>
    internal static FontResolverInfo? ResolveTypeface
        (
            string familyName,
            FontResolvingOptions fontResolvingOptions,
            string typefaceKey
        )
    {
        // Internally we often have the typeface key already.
        if (string.IsNullOrEmpty (typefaceKey))
        {
            typefaceKey = XGlyphTypeface.ComputeKey (familyName, fontResolvingOptions);
        }

        // The user may call ResolveTypeface anytime from anywhere, so check cache in FontFactory in the first place.
        if (FontFactory.TryGetFontResolverInfoByTypefaceKey (typefaceKey, out var fontResolverInfo))
        {
            return fontResolverInfo;
        }

        // Let the platform create the requested font source and save both PlattformResolverInfo
        // and XFontSource in FontFactory cache.
        // It is possible that we already have the correct font source. E.g. we already have the regular typeface in cache
        // and looking now for the italic typeface, but no such font exists. In this case we get the regular font source
        // and cache again it with the italic typeface key. Furthermore in glyph typeface style simulation for italic is set.
        XFontSource? fontSource = null;

        // If no such font exists return null. PDFsharp will fail.
        if (fontSource == null)
        {
            return null;
        }

        //#if (CORE || GDI) && !WPF
        //            // TODO: Support style simulation for GDI+ platform fonts.
        //            fontResolverInfo = new PlatformFontResolverInfo(typefaceKey, false, false, gdiFont);
        //#endif
        if (fontResolvingOptions.OverrideStyleSimulations)
        {
        }
        else
        {
        }

        FontFactory.CacheFontResolverInfo (typefaceKey, fontResolverInfo);

        // Register font data under the platform specific face name.
        // Already done in CreateFontSource.
        // FontFactory.CacheNewFontSource(typefaceKey, fontSource);

        return fontResolverInfo;
    }
}
