// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XBitmapSource.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Defines an abstract base class for pixel based images.
/// </summary>
public abstract class XBitmapSource : XImage
{
    // TODO: Move code from XImage to this class.

    /// <summary>
    /// Gets the width of the image in pixels.
    /// </summary>
    public override int PixelWidth => PixelWidth;

    /// <summary>
    /// Gets the height of the image in pixels.
    /// </summary>
    public override int PixelHeight => PixelHeight;
}
