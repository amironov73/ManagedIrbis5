// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ImageSoruce.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;

/// <summary>
///
/// </summary>
public abstract class ImageSource
{
    /// <summary>
    /// Gets or sets the image source implementation to use for reading images.
    /// </summary>
    /// <value>The image source impl.</value>
    public static ImageSource ImageSourceImpl { get; set; } = null!;

    /// <summary>
    ///
    /// </summary>
    public interface IImageSource
    {
        /// <summary>
        ///
        /// </summary>
        int Width { get; }

        /// <summary>
        ///
        /// </summary>
        int Height { get; }

        /// <summary>
        ///
        /// </summary>
        string Name { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ms"></param>
        void SaveAsJpeg (MemoryStream ms);

        /// <summary>
        ///
        /// </summary>
        bool Transparent { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ms"></param>
        void SaveAsPdfBitmap (MemoryStream ms);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    protected abstract IImageSource FromFileImpl (string path, int quality = 75);

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imageSource"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    protected abstract IImageSource FromBinaryImpl (string name, Func<byte[]> imageSource, int quality = 75);

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imageStream"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    protected abstract IImageSource FromStreamImpl (string name, Func<Stream> imageStream, int quality = 75);


    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    public static IImageSource FromFile (string path, int quality = 75)
    {
        return ImageSourceImpl.FromFileImpl (path, quality);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imageSource"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    public static IImageSource FromBinary (string name, Func<byte[]> imageSource, int quality = 75)
    {
        return ImageSourceImpl.FromBinaryImpl (name, imageSource, quality);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imageStream"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    public static IImageSource FromStream (string name, Func<Stream> imageStream, int quality = 75)
    {
        return ImageSourceImpl.FromStreamImpl (name, imageStream, quality);
    }
}
