// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

/* IImageImporter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM;

using PdfSharpCore.Pdf;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// This interface will be implemented by specialized classes,
/// one for JPEG, one for BMP, one for PNG, one for GIF. Maybe more.
/// </summary>
internal interface IImageImporter
{
    /// <summary>
    /// Imports the image. Returns null if the image importer
    /// does not support the format.
    /// </summary>
    ImportedImage? ImportImage (StreamReaderHelper stream, PdfDocument document);

    /// <summary>
    /// Prepares the image data needed for the PDF file.
    /// </summary>
    ImageData PrepareImage (ImagePrivateData data);
}

// $THHO Add IDispose?.
/// <summary>
/// Helper for dealing with Stream data.
/// </summary>
internal class StreamReaderHelper
{
    internal StreamReaderHelper (Stream stream)
    {
        // TODO: Are there advantages of GetBuffer()? It should reduce LOH fragmentation.
        OriginalStream = stream;
        OriginalStream.Position = 0;
        if (OriginalStream.Length > int.MaxValue)
        {
            throw new ArgumentException ("Stream is too large.", nameof (stream));
        }

        Length = (int)OriginalStream.Length;
        Data = new byte[Length];
        OriginalStream.Read (Data, 0, Length).NotUsed();
    }

    internal byte GetByte (int offset)
    {
        if (CurrentOffset + offset >= Length)
        {
            Debug.Assert (false);
            return 0;
        }

        return Data[CurrentOffset + offset];
    }

    internal ushort GetWord (int offset, bool bigEndian)
    {
        return (ushort)(bigEndian
            ? GetByte (offset) * 256 + GetByte (offset + 1)
            : GetByte (offset) + GetByte (offset + 1) * 256);
    }

    internal uint GetDWord (int offset, bool bigEndian)
    {
        return (uint)(bigEndian
            ? GetWord (offset, true) * 65536 + GetWord (offset + 2, true)
            : GetWord (offset, false) + GetWord (offset + 2, false) * 65536);
    }

    private static void CopyStream (Stream input, Stream output)
    {
        var buffer = new byte[65536];
        int read;
        while ((read = input.Read (buffer, 0, buffer.Length)) > 0)
        {
            output.Write (buffer, 0, read);
        }
    }

    /// <summary>
    /// Resets this instance.
    /// </summary>
    public void Reset()
    {
        CurrentOffset = 0;
    }

    /// <summary>
    /// Gets the original stream.
    /// </summary>
    public Stream OriginalStream { get; }

    internal int CurrentOffset { get; set; }

    /// <summary>
    /// Gets the data as byte[].
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Gets the length of Data.
    /// </summary>
    public int Length { get; }
}

/// <summary>
/// The imported image.
/// </summary>
internal abstract class ImportedImage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImportedImage"/> class.
    /// </summary>
    protected ImportedImage
        (
            IImageImporter importer,
            ImagePrivateData data,
            PdfDocument document
        )
    {
        Data = data;
        _document = document;
        data.Image = this;
        _importer = importer;
    }


    /// <summary>
    /// Gets information about the image.
    /// </summary>
    public ImageInformation Information { get; private set; } = new ImageInformation();

    /// <summary>
    /// Gets a value indicating whether image data for the PDF file was already prepared.
    /// </summary>
    public bool HasImageData => _imageData != null;

    /// <summary>
    /// Gets the image data needed for the PDF file.
    /// </summary>
    public ImageData ImageData
    {
        get
        {
            if (!HasImageData)
            {
                _imageData = PrepareImageData();
            }

            return _imageData!;
        }
        private set => _imageData = value;
    }

    private ImageData? _imageData;

    internal virtual ImageData PrepareImageData()
    {
        throw new NotImplementedException();
    }

    private IImageImporter _importer;
    internal ImagePrivateData Data;
    internal readonly PdfDocument _document;
}

/// <summary>
/// Public information about the image, filled immediately.
/// Note: The stream will be read and decoded on the first call to PrepareImageData().
/// ImageInformation can be filled for corrupted images that will throw an expection on PrepareImageData().
/// </summary>
internal class ImageInformation
{
    internal enum ImageFormats
    {
        /// <summary>
        /// Standard JPEG format (RGB).
        /// </summary>
        JPEG,

        /// <summary>
        /// Grayscale JPEG format.
        /// </summary>
        JPEGGRAY,

        /// <summary>
        /// JPEG file with inverted CMYK, thus RGBW.
        /// </summary>
        JPEGRGBW,

        /// <summary>
        /// JPEG file with CMYK.
        /// </summary>
        JPEGCMYK,

        /// <summary>
        ///
        /// </summary>
        Palette1,

        /// <summary>
        ///
        /// </summary>
        Palette4,

        /// <summary>
        ///
        /// </summary>
        Palette8,

        /// <summary>
        ///
        /// </summary>
        RGB24,

        /// <summary>
        ///
        /// </summary>
        ARGB32
    }

    internal ImageFormats ImageFormat;

    internal uint Width;
    internal uint Height;

    /// <summary>
    /// The horizontal DPI (dots per inch). Can be 0 if not supported by the image format.
    /// Note: JFIF (JPEG) files may contain either DPI or DPM or just the aspect ratio. Windows BMP files will contain DPM. Other formats may support any combination, including none at all.
    /// </summary>
    internal decimal HorizontalDPI;

    /// <summary>
    /// The vertical DPI (dots per inch). Can be 0 if not supported by the image format.
    /// </summary>
    internal decimal VerticalDPI;

    /// <summary>
    /// The horizontal DPM (dots per meter). Can be 0 if not supported by the image format.
    /// </summary>
    internal decimal HorizontalDPM;

    /// <summary>
    /// The vertical DPM (dots per meter). Can be 0 if not supported by the image format.
    /// </summary>
    internal decimal VerticalDPM;

    /// <summary>
    /// The horizontal component of the aspect ratio. Can be 0 if not supported by the image format.
    /// Note: Aspect ratio will be set if either DPI or DPM was set, but may also be available in the absence of both DPI and DPM.
    /// </summary>
    internal decimal HorizontalAspectRatio;

    /// <summary>
    /// The vertical component of the aspect ratio. Can be 0 if not supported by the image format.
    /// </summary>
    internal decimal VerticalAspectRatio;

    /// <summary>
    /// The colors used. Only valid for images with palettes, will be 0 otherwise.
    /// </summary>
    internal uint ColorsUsed;
}

/// <summary>
/// Contains internal data. This includes a reference to the Stream if data for PDF was not yet prepared.
/// </summary>
internal abstract class ImagePrivateData
{
    /// <summary>
    /// Gets the image.
    /// </summary>
    public ImportedImage? Image { get; internal set; }
}

/// <summary>
/// Contains data needed for PDF. Will be prepared when needed.
/// </summary>
internal abstract class ImageData
{
}
