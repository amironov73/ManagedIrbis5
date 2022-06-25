// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

using System.Drawing;
using AM.Drawing.HtmlRenderer.Adapters;

namespace AM.Windows.Forms.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WinForms Image object for core.
/// </summary>
internal sealed class ImageAdapter : RImage
{
    /// <summary>
    /// the underline win-forms image.
    /// </summary>
    private readonly Image _image;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Object"/> class.
    /// </summary>
    public ImageAdapter(Image image)
    {
        _image = image;
    }

    /// <summary>
    /// the underline win-forms image.
    /// </summary>
    public Image Image
    {
        get { return _image; }
    }

    public override double Width
    {
        get { return _image.Width; }
    }

    public override double Height
    {
        get { return _image.Height; }
    }

    public override void Dispose()
    {
        _image.Dispose();
    }
}