// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfContent.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using PdfSharpCore.Drawing.Pdf;
using PdfSharpCore.Pdf.Filters;
using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents the content of a page. PDFsharp supports only one content stream per page.
/// If an imported page has an array of content streams, the streams are concatenated to
/// one single stream.
/// </summary>
public sealed class PdfContent
    : PdfDictionary
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfContent"/> class.
    /// </summary>
    public PdfContent (PdfDocument document)
        : base (document)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfContent"/> class.
    /// </summary>
    internal PdfContent (PdfPage? page)
        : base (page != null ? page.Owner : null)
    {
        //_pageContent = new PageContent(page);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfContent"/> class.
    /// </summary>
    /// <param name="dict">The dict.</param>
    public PdfContent (PdfDictionary dict) // HACK PdfContent
        : base (dict)
    {
        // A PdfContent dictionary is always unfiltered.
        Decode();
    }

    #endregion

    /// <summary>
    /// Sets a value indicating whether the content is compressed with the ZIP algorithm.
    /// </summary>
    public bool Compressed
    {
        set
        {
            if (value)
            {
                var filter = Elements[PdfStream.Keys.Filter];
                if (filter == null)
                {
                    var bytes = Filtering.FlateDecode.Encode (Stream.Value, _document.Options.FlateEncodeMode);
                    Stream.Value = bytes;
                    Elements.SetInteger (PdfStream.Keys.Length, Stream.Length);
                    Elements.SetName (PdfStream.Keys.Filter, "/FlateDecode");
                }
            }
        }
    }

    /// <summary>
    /// Unfilters the stream.
    /// </summary>
    void Decode()
    {
        if (Stream is { Value: not null })
        {
            var item = Elements[PdfStream.Keys.Filter];
            if (item != null)
            {
                var decodeParms = Elements[PdfStream.Keys.DecodeParms];
                var bytes = Filtering.Decode (Stream.Value, item, decodeParms);
                if (bytes != null)
                {
                    Stream.Value = bytes;
                    Elements.Remove (PdfStream.Keys.Filter);
                    Elements.Remove (PdfStream.Keys.DecodeParms);
                    Elements.SetInteger (PdfStream.Keys.Length, Stream.Length);
                }
            }
        }
    }

    /// <summary>
    /// Surround content with q/Q operations if necessary.
    /// </summary>
    internal void PreserveGraphicsState()
    {
        // If a content stream is touched by PDFsharp it is typically because graphical operations are
        // prepended or appended. Some nasty PDF tools does not preserve the graphical state correctly.
        // Therefore we try to relieve the problem by surrounding the content stream with push/restore
        // graphic state operation.
        if (Stream != null)
        {
            var value = Stream.Value;
            var length = value.Length;
            if (length != 0 && ((value[0] != (byte)'q' || value[1] != (byte)'\n')))
            {
                var newValue = new byte[length + 2 + 3];
                newValue[0] = (byte)'q';
                newValue[1] = (byte)'\n';
                Array.Copy (value, 0, newValue, 2, length);
                newValue[length + 2] = (byte)' ';
                newValue[length + 3] = (byte)'Q';
                newValue[length + 4] = (byte)'\n';
                Stream.Value = newValue;
                Elements.SetInteger ("/Length", Stream.Length);
            }
        }
    }

    internal override void WriteObject (PdfWriter writer)
    {
        if (_pdfRenderer != null)
        {
            // GetContent also disposes the underlying XGraphics object, if one exists
            //Stream = new PdfStream(PdfEncoders.RawEncoding.GetBytes(pdfRenderer.GetContent()), this);
            _pdfRenderer.Close();
            Debug.Assert (_pdfRenderer == null);
        }

        if (Stream != null)
        {
            //if (Owner.Options.CompressContentStreams)
            if (Owner.Options.CompressContentStreams && Elements.GetName ("/Filter").Length == 0)
            {
                Stream.Value = Filtering.FlateDecode.Encode (Stream.Value, _document.Options.FlateEncodeMode);

                //Elements["/Filter"] = new PdfName("/FlateDecode");
                Elements.SetName ("/Filter", "/FlateDecode");
            }

            Elements.SetInteger ("/Length", Stream.Length);
        }

        base.WriteObject (writer);
    }

    internal XGraphicsPdfRenderer? _pdfRenderer;

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    internal sealed class Keys : PdfStream.Keys
    {
        /// <summary>
        /// Gets the KeysMeta for these keys.
        /// </summary>
        public static DictionaryMeta Meta
        {
            get { return _meta ??= CreateMeta (typeof (Keys)); }
        }

        static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta
    {
        get { return Keys.Meta; }
    }
}
