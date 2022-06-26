// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* ImageAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Media.Imaging;

using AM.Drawing.HtmlRenderer.Adapters;

#endregion

#nullable enable

namespace AM.Windows.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WPF Image object for core.
/// </summary>
internal sealed class ImageAdapter 
    : RImage
{
    /// <summary>
    /// the underline WPF image.
    /// </summary>
    private readonly BitmapImage _image;

    /// <summary>
    /// Init.
    /// </summary>
    public ImageAdapter(BitmapImage image)
    {
        _image = image;
    }

    /// <summary>
    /// the underline WPF image.
    /// </summary>
    public BitmapImage Image
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
        if (_image.StreamSource != null)
            _image.StreamSource.Dispose();
    }
}