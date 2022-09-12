// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Internal;

/// <summary>
/// Helper functions for RGB and CMYK colors.
/// </summary>
static class ColorSpaceHelper
{
    /// <summary>
    /// Checks whether a color mode and a color match.
    /// </summary>
    public static XColor EnsureColorMode (PdfColorMode colorMode, XColor color)
    {
        return colorMode switch
        {
            PdfColorMode.Rgb when color.ColorSpace != XColorSpace.Rgb =>
                XColor.FromArgb ((int)(color.A * 255), color.R, color.G, color.B),

            PdfColorMode.Cmyk when color.ColorSpace != XColorSpace.Cmyk =>
                XColor.FromCmyk (color.A, color.C, color.M, color.Y, color.K),

            _ => color
        };
    }

    /// <summary>
    /// Checks whether the color mode of a document and a color match.
    /// </summary>
    public static XColor EnsureColorMode
        (
            PdfDocument document,
            XColor color
        )
    {
        Sure.NotNull (document);

        return EnsureColorMode (document.Options.ColorMode, color);
    }

    /// <summary>
    /// Determines whether two colors are equal referring to their CMYK color values.
    /// </summary>
    public static bool IsEqualCmyk (XColor x, XColor y)
    {
        return x.ColorSpace == XColorSpace.Cmyk
               && y.ColorSpace == XColorSpace.Cmyk
               && x.C == y.C
               && x.M == y.M
               && x.Y == y.Y
               && x.K == y.K;
    }
}
