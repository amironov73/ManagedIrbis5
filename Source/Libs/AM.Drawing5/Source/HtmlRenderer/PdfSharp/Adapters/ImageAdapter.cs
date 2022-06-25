// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* ImageAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Adapters;

using PdfSharp.Drawing;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.PdfSharp.Adapters;

/// <summary>
/// Adapter for WinForms Image object for core.
/// </summary>
internal sealed class ImageAdapter 
    : RImage
{
    /// <summary>
    /// the underline win-forms image.
    /// </summary>
    private readonly XImage _image;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Object"/> class.
    /// </summary>
    public ImageAdapter(XImage image)
    {
        _image = image;
    }

    /// <summary>
    /// the underline win-forms image.
    /// </summary>
    public XImage Image
    {
        get { return _image; }
    }

    public override double Width
    {
        get { return _image.PixelWidth; }
    }

    public override double Height
    {
        get { return _image.PixelHeight; }
    }

    public override void Dispose()
    {
        _image.Dispose();
    }
}