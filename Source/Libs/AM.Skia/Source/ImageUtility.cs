// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* ImageUtility.cs -- полезные методы для работы с изображениями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using JetBrains.Annotations;

using SkiaSharp;

#endregion

namespace AM.Skia;

/// <summary>
/// Полезные методы для работы с изображениями.
/// </summary>
[PublicAPI]
public static class ImageUtility
{
    #region Public methods

    /// <summary>
    /// Масштабирование с одновременным посерением изображения.
    /// </summary>
    public static SKImage GrayscaleImage
        (
            SKImage image,
            int newWidth,
            int newHeight
        )
    {
        Sure.NotNull (image);
        Sure.Positive (newWidth);
        Sure.Positive (newHeight);

        var info = new SKImageInfo
        {
            Width = newWidth,
            Height = newHeight,
            ColorType = SKColorType.Gray8,
            AlphaType = SKAlphaType.Opaque
        };

        var rect = new SKRectI (0, 0, newWidth, newHeight);
        using var surface = SKSurface.Create (info);
        using var paint = new SKPaint();
        paint.IsAntialias = true;
        paint.FilterQuality = SKFilterQuality.High;

        surface.Canvas.DrawImage (image, rect, paint);
        var result = surface.Snapshot();

        // var data = result!.Encode (SKEncodedImageFormat.Png, 100);
        // using var stream = File.Create ("grayscaled.png");
        // data.SaveTo (stream);

        return result;
    }

    /// <summary>
    /// Свертка изображения до серого 64x64.
    /// </summary>
    public static byte[,] ReduceImage
        (
            SKImage image,
            int width = 64,
            int height = 64
        )
    {
        Sure.NotNull (image);
        Sure.Positive (width);
        Sure.Positive (height);

        using var scaled = GrayscaleImage (image, width, height);
        var pixmap = scaled.PeekPixels();
        var result = new byte[width, height];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var color = pixmap.GetPixelColor (x, y);
                result[x, y] = color.Blue;
            }
        }

        return result;
    }

    /// <summary>
    /// Сравнение сверток изображений.
    /// </summary>
    public static double CompareReducedImages
        (
            byte[,] left,
            byte[,] right
        )
    {
        Sure.NotNull (left);
        Sure.NotNull (right);
        Sure.AssertState (left.GetLength (0) == right.GetLength (0));
        Sure.AssertState (left.GetLength (1) == right.GetLength (1));

        var result = 0.0;
        var width = left.GetLength (0);
        var height = left.GetLength (1);
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var leftPixel = (double) left[x, y];
                var rightPixel = (double) right[x, y];
                result += Math.Abs (leftPixel - rightPixel);
            }
        }

        result /= width * height;

        return result;
    }

    #endregion
}
