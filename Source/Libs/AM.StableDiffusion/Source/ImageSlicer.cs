// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ImageSlicer.cs -- умеет нарезать большое изображение на матрицу мелких
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.IO;

using JetBrains.Annotations;

using SkiaSharp;

#endregion

#nullable enable

namespace AM.StableDiffusion;

/// <summary>
/// Умеет нарезать большое изображение на матрицу мелких.
/// </summary>
[PublicAPI]
public sealed class ImageSlicer
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ImageSlicer()
        : this (Console.Out)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ImageSlicer
        (
            TextWriter logWriter
        )
    {
        Sure.NotNull (logWriter);

        _logWriter = logWriter;
    }

    #endregion

    #region Private members

    private readonly TextWriter _logWriter;

    #endregion

    #region Public methods

    /// <summary>
    /// Разрезание большого изображения.
    /// </summary>
    /// <param name="originalImagePath">Путь к оригинальному изображению.
    /// </param>
    /// <param name="outputDirectory">Директория, в которую следует
    /// поместить результат.</param>
    /// <param name="originX">Начальная точка: X.</param>
    /// <param name="originY">Начальная точка: Y.</param>
    /// <param name="chunkWidth">Ширина маленького изображения.</param>
    /// <param name="chunkHeight">Высота маленького изображения.</param>
    /// <param name="numberX">Количество маленьких изображений
    /// по горизонтали.
    /// </param>
    /// <param name="numberY">Количество маленьких изображений
    /// по вертикали.</param>
    public void Slice
        (
            string originalImagePath,
            string outputDirectory,
            int originX,
            int originY,
            int chunkWidth,
            int chunkHeight,
            int numberX,
            int numberY
        )
    {
        Sure.FileExists (originalImagePath);
        Sure.NotNullNorEmpty (outputDirectory);
        Sure.NonNegative (originX);
        Sure.NonNegative (originY);
        Sure.Positive (chunkWidth);
        Sure.Positive (chunkHeight);

        _logWriter.WriteLine ($"Start slicing {originalImagePath}");

        Directory.CreateDirectory (outputDirectory);
        DirectoryUtility.ClearDirectory (Path.GetFullPath (outputDirectory));

        var bigBitmap = SKBitmap.Decode (originalImagePath);
        var bigImage = SKImage.FromBitmap (bigBitmap);

        for (var indexY = 0; indexY < numberY; indexY++)
        {
            for (var indexX = 0; indexX < numberX; indexX++)
            {
                var info = new SKImageInfo (chunkWidth, chunkHeight);
                var surface = SKSurface.Create (info);
                var canvas = surface.Canvas;

                float left = originX + indexX * chunkWidth;
                float top = originY + indexY * chunkHeight;
                canvas.DrawImage
                    (
                        bigImage,
                        new SKRect
                            (
                                left,
                                top,
                                left + chunkWidth,
                                top + chunkHeight
                            ),
                        new SKRect
                            (
                                0,
                                0,
                                chunkWidth,
                                chunkHeight
                            )
                    );

                var chunk = surface.Snapshot();
                var data = chunk.Encode (SKEncodedImageFormat.Png, 100);
                var fileName = $"{indexY:000}_{indexX:000}.png";
                _logWriter.Write ($"\t{fileName}");
                _logWriter.Flush();
                fileName = Path.Combine (outputDirectory, fileName);
                File.Delete (fileName);
                using var stream = File.Create (fileName);
                data.SaveTo (stream);
                _logWriter.WriteLine (" done");
            }
        }

        _logWriter.WriteLine ($"End slicing {originalImagePath}");
    }

    #endregion
}
