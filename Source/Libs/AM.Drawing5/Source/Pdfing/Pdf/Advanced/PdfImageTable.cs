// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* PdfImageTable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Collections.Generic;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Contains all used images of a document.
/// </summary>
internal sealed class PdfImageTable
    : PdfResourceTable
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of this class, which is a singleton for each document.
    /// </summary>
    public PdfImageTable (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets a PdfImage from an XImage. If no PdfImage already exists, a new one is created.
    /// </summary>
    public PdfImage GetImage (XImage image)
    {
        var selector = image._selector;
        if (selector == null)
        {
            selector = new ImageSelector (image);
            image._selector = selector;
        }

        if (!_images.TryGetValue (selector, out var pdfImage))
        {
            pdfImage = new PdfImage (Owner, image);

            //pdfImage.Document = _document;
            Debug.Assert (pdfImage.Owner == Owner);
            _images[selector] = pdfImage;
        }

        return pdfImage;
    }

    /// <summary>
    /// Map from ImageSelector to PdfImage.
    /// </summary>
    readonly Dictionary<ImageSelector, PdfImage> _images = new ();

    /// <summary>
    /// A collection of information that uniquely identifies a particular PdfImage.
    /// </summary>
    public class ImageSelector
    {
        /// <summary>
        /// Initializes a new instance of ImageSelector from an XImage.
        /// </summary>
        public ImageSelector (XImage image)
        {
            // HACK: implement a way to identify images when they are reused
            // TODO 4STLA Implementation that calculates MD5 hashes for images generated for the images can be found here: http://forum.PdfSharpCore.net/viewtopic.php?p=6959#p6959
            image._path ??= "*" + Guid.NewGuid().ToString ("B");

            // HACK: just use full path to identify
            _path = image._path.ToLowerInvariant();
        }

        public string? Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private string? _path;

        /// <inheritdoc cref="object.Equals(object?)"/>
        public override bool Equals (object? obj)
        {
            return obj is ImageSelector selector && _path == selector._path;
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return _path?.GetHashCode() ?? 0;
        }
    }
}
