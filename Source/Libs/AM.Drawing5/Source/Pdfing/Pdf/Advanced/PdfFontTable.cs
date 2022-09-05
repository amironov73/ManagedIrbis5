// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfFontTable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Collections.Generic;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

internal enum FontType
{
    /// <summary>
    /// TrueType with WinAnsi encoding.
    /// </summary>
    TrueType = 1,

    /// <summary>
    /// TrueType with Identity-H or Identity-V encoding (unicode).
    /// </summary>
    Type0 = 2,
}

/// <summary>
/// Contains all used fonts of a document.
/// </summary>
internal sealed class PdfFontTable
    : PdfResourceTable
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of this class, which is a singleton for each document.
    /// </summary>
    public PdfFontTable (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets a PdfFont from an XFont. If no PdfFont already exists, a new one is created.
    /// </summary>
    public PdfFont GetFont (XFont font)
    {
        var selector = font.Selector;
        if (selector == null)
        {
            selector = ComputeKey (font); //new FontSelector(font);
            font.Selector = selector;
        }

        if (!_fonts.TryGetValue (selector, out var pdfFont))
        {
            pdfFont = font.Unicode
                ? new PdfType0Font (Owner, font, font.IsVertical)
                : new PdfTrueTypeFont (Owner, font);

            //pdfFont.Document = _document;
            Debug.Assert (pdfFont.Owner == Owner);
            _fonts[selector] = pdfFont;
        }

        return pdfFont;
    }

#if true
    /// <summary>
    /// Gets a PdfFont from a font program. If no PdfFont already exists, a new one is created.
    /// </summary>
    public PdfFont GetFont (string idName, byte[] fontData)
    {
        Debug.Assert (false);

        //FontSelector selector = new FontSelector(idName);
        string selector = null; // ComputeKey(font); //new FontSelector(font);
        if (!_fonts.TryGetValue (selector, out var pdfFont))
        {
            //if (font.Unicode)
            pdfFont = new PdfType0Font (Owner, idName, fontData, false);

            //else
            //  pdfFont = new PdfTrueTypeFont(_owner, font);
            //pdfFont.Document = _document;
            Debug.Assert (pdfFont.Owner == Owner);
            _fonts[selector] = pdfFont;
        }

        return pdfFont;
    }
#endif

    /// <summary>
    /// Tries to gets a PdfFont from the font dictionary.
    /// Returns null if no such PdfFont exists.
    /// </summary>
    public PdfFont TryGetFont (string idName)
    {
        Debug.Assert (false);

        //FontSelector selector = new FontSelector(idName);
        string selector = null;
        _fonts.TryGetValue (selector, out var pdfFont);
        return pdfFont;
    }

    internal static string ComputeKey (XFont font)
    {
        var glyphTypeface = font.GlyphTypeface;
        var key = glyphTypeface.Fontface.FullFaceName.ToLowerInvariant() +
                  (glyphTypeface.IsBold ? "/b" : "") + (glyphTypeface.IsItalic ? "/i" : "") + font.Unicode;
        return key;
    }

    /// <summary>
    /// Map from PdfFontSelector to PdfFont.
    /// </summary>
    readonly Dictionary<string, PdfFont> _fonts = new Dictionary<string, PdfFont>();

    public void PrepareForSave()
    {
        foreach (var font in _fonts.Values)
        {
            font.PrepareForSave();
        }
    }
}
