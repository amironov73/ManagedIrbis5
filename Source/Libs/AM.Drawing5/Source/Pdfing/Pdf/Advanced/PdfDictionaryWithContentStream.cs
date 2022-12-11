// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfDictionaryWithContentStream.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents a base class for dictionaries with a content stream.
/// Implement IContentStream for use with a content writer.
/// </summary>
public abstract class PdfDictionaryWithContentStream
    : PdfDictionary, IContentStream
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfDictionaryWithContentStream"/> class.
    /// </summary>
    protected PdfDictionaryWithContentStream()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfDictionaryWithContentStream"/> class.
    /// </summary>
    /// <param name="document">The document.</param>
    protected PdfDictionaryWithContentStream (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance from an existing dictionary. Used for object type transformation.
    /// </summary>
    protected PdfDictionaryWithContentStream (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets the resources dictionary of this dictionary. If no such dictionary exists, it is created.
    /// </summary>
    internal PdfResources? Resources =>
        _resources ??= (PdfResources?)Elements.GetValue (Keys.Resources, VCF.Create);

    private PdfResources? _resources;

    /// <summary>
    /// Implements the interface because the primary function is internal.
    /// </summary>
    PdfResources? IContentStream.Resources => Resources;

    internal string? GetFontName (XFont font, out PdfFont? pdfFont)
    {
        pdfFont = _document?.FontTable.GetFont (font);
        Debug.Assert (pdfFont != null);
        var name = Resources?.AddFont (pdfFont);
        return name;
    }

    string? IContentStream.GetFontName (XFont font, out PdfFont? pdfFont)
    {
        return GetFontName (font, out pdfFont);
    }

    internal string? GetFontName (string idName, byte[] fontData, out PdfFont? pdfFont)
    {
        pdfFont = _document?.FontTable.GetFont (idName, fontData);
        Debug.Assert (pdfFont != null);
        var name = Resources?.AddFont (pdfFont);
        return name;
    }

    string? IContentStream.GetFontName (string idName, byte[] fontData, out PdfFont? pdfFont)
    {
        return GetFontName (idName, fontData, out pdfFont);
    }

    /// <summary>
    /// Gets the resource name of the specified image within this dictionary.
    /// </summary>
    internal string? GetImageName (XImage image)
    {
        var pdfImage = _document?.ImageTable.GetImage (image);
        Debug.Assert (pdfImage != null);
        var name = Resources?.AddImage (pdfImage);
        return name;
    }

    /// <summary>
    /// Implements the interface because the primary function is internal.
    /// </summary>
    string IContentStream.GetImageName (XImage image)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the resource name of the specified form within this dictionary.
    /// </summary>
    internal string? GetFormName (XForm form)
    {
        var pdfForm = _document?.FormTable.GetForm (form);
        Debug.Assert (pdfForm != null);
        var name = Resources?.AddForm (pdfForm);
        return name;
    }

    /// <summary>
    /// Implements the interface because the primary function is internal.
    /// </summary>
    string IContentStream.GetFormName (XForm form)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    public class Keys : PdfDictionary.PdfStream.Keys
    {
        /// <summary>
        /// (Optional but strongly recommended; PDF 1.2) A dictionary specifying any
        /// resources (such as fonts and images) required by the form XObject.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional, typeof (PdfResources))]
        public const string Resources = "/Resources";
    }
}
