// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ImageSharpImageSource.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;

#endregion

#nullable enable

namespace PdfSharpCore.Utils;

/// <summary>
/// Источник картинок -- ImageSharp.
/// </summary>
public class ImageSharpImageSource<TPixel>
    : ImageSource
    where TPixel : unmanaged, IPixel<TPixel>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="image"></param>
    /// <param name="imgFormat"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    public static IImageSource FromImageSharpImage
        (
            Image<TPixel> image,
            IImageFormat imgFormat,
            int quality = 75
        )
    {
        var _path = "*" + Guid.NewGuid().ToString ("B");

        return new ImageSharpImageSourceImpl<TPixel> (_path, image, quality, imgFormat is PngFormat);
    }

    /// <inheritdoc cref="ImageSource.FromBinaryImpl"/>
    protected override IImageSource FromBinaryImpl
        (
            string name,
            Func<byte[]> imageSource,
            int quality = 75
        )
    {
        var image = Image.Load<TPixel> (imageSource.Invoke(), out var imgFormat);
        return new ImageSharpImageSourceImpl<TPixel> (name, image, quality, imgFormat is PngFormat);
    }

    /// <inheritdoc cref="ImageSource.FromFileImpl"/>
    protected override IImageSource FromFileImpl (string path, int quality = 75)
    {
        var image = Image.Load<TPixel> (path, out var imgFormat);
        return new ImageSharpImageSourceImpl<TPixel> (path, image, quality, imgFormat is PngFormat);
    }

    /// <inheritdoc cref="ImageSource.FromStreamImpl"/>
    protected override IImageSource FromStreamImpl (string name, Func<Stream> imageStream, int quality = 75)
    {
        using var stream = imageStream.Invoke();
        var image = Image.Load<TPixel> (stream, out var imgFormat);

        return new ImageSharpImageSourceImpl<TPixel> (name, image, quality, imgFormat is PngFormat);
    }

    private class ImageSharpImageSourceImpl<TPixel2>
        : IImageSource, IDisposable
        where TPixel2: unmanaged, IPixel<TPixel2>
    {
        private Image<TPixel2> Image { get; }
        private readonly int _quality;

        public int Width => Image.Width;
        public int Height => Image.Height;
        public string Name { get; }
        public bool Transparent { get; internal set; }

        public ImageSharpImageSourceImpl (string name, Image<TPixel2> image, int quality, bool isTransparent)
        {
            Name = name;
            Image = image;
            _quality = quality;
            Transparent = isTransparent;
        }

        public void SaveAsJpeg (MemoryStream ms)
        {
            Image.SaveAsJpeg (ms, new JpegEncoder { Quality = _quality });
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Image.Dispose();
        }

        public void SaveAsPdfBitmap (MemoryStream ms)
        {
            var bmp = new BmpEncoder { BitsPerPixel = BmpBitsPerPixel.Pixel32 };
            Image.Save (ms, bmp);
        }
    }
}
