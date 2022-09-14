// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using AM;

namespace PdfSharpCore.Pdf;

/// <summary>
/// Holds information how to handle the document when it is saved as PDF stream.
/// </summary>
public sealed class PdfDocumentOptions
{
    #region Construction

    internal PdfDocumentOptions
        (
            PdfDocument document
        )
    {
        document.NotUsed();

        //_deflateContents = true;
        //_writeProcedureSets = true;
    }

    #endregion

    /// <summary>
    /// Gets or sets the color mode.
    /// </summary>
    public PdfColorMode ColorMode { get; set; } = PdfColorMode.Rgb;

    /// <summary>
    /// Gets or sets a value indicating whether to compress content streams of PDF pages.
    /// </summary>
    public bool CompressContentStreams { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that all objects are not compressed.
    /// </summary>
    public bool NoCompression { get; set; }

    /// <summary>
    /// Gets or sets the flate encode mode. Besides the balanced default mode you can set modes for best compression (slower) or best speed (larger files).
    /// </summary>
    public PdfFlateEncodeMode FlateEncodeMode { get; set; } = PdfFlateEncodeMode.Default;

    /// <summary>
    /// Gets or sets a value indicating whether to compress bilevel images using CCITT compression.
    /// With true, PDFsharp will try FlateDecode CCITT and will use the smallest one or a combination of both.
    /// With false, PDFsharp will always use FlateDecode only - files may be a few bytes larger, but file creation is faster.
    /// </summary>
    public bool EnableCcittCompressionForBilevelImages { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to compress JPEG images with the FlateDecode filter.
    /// </summary>
    public PdfUseFlateDecoderForJpegImages UseFlateDecoderForJpegImages { get; set; }
        = PdfUseFlateDecoderForJpegImages.Never;
}
