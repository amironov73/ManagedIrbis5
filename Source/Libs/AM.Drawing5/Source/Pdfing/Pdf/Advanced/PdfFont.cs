// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfFont.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Text;

using AM.Text;

using PdfSharpCore.Fonts;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents a PDF font.
/// </summary>
public class PdfFont
    : PdfDictionary
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfFont"/> class.
    /// </summary>
    public PdfFont (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    #endregion

    internal PdfFontDescriptor FontDescriptor
    {
        get
        {
            Debug.Assert (_fontDescriptor != null);
            return _fontDescriptor;
        }
        set { _fontDescriptor = value; }
    }

    PdfFontDescriptor? _fontDescriptor;

    internal PdfFontEncoding FontEncoding;

    /// <summary>
    /// Gets a value indicating whether this instance is symbol font.
    /// </summary>
    public bool IsSymbolFont
    {
        get { return _fontDescriptor.IsSymbolFont; }
    }

    internal void AddChars (string text)
    {
        if (CMapInfo != null)
        {
            CMapInfo.AddChars (text);
        }
    }

    internal void AddGlyphIndices (string glyphIndices)
    {
        if (CMapInfo != null)
        {
            CMapInfo.AddGlyphIndices (glyphIndices);
        }
    }

    /// <summary>
    /// Gets or sets the CMapInfo.
    /// </summary>
    internal CMapInfo CMapInfo { get; set; }

    /// <summary>
    /// Gets or sets ToUnicodeMap.
    /// </summary>
    internal PdfToUnicodeMap ToUnicodeMap { get; set; }


    /// <summary>
    /// Adds a tag of exactly six uppercase letters to the font name
    /// according to PDF Reference Section 5.5.3 'Font Subsets'
    /// </summary>
    internal static string CreateEmbeddedFontSubsetName (string name)
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (64);
        var bytes = Guid.NewGuid().ToByteArray();
        for (var idx = 0; idx < 6; idx++)
            builder.Append ((char)('A' + bytes[idx] % 26));
        builder.Append ('+');
        builder.Append (name.StartsWith ("/") ? name[1..] : name);

        return builder.ReturnShared();
    }

    /// <summary>
    /// Predefined keys common to all font dictionaries.
    /// </summary>
    public class Keys
        : KeysBase
    {
        /// <summary>
        /// (Required) The type of PDF object that this dictionary describes;
        /// must be Font for a font dictionary.
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.Required, FixedValue = "Font")]
        public const string Type = "/Type";

        /// <summary>
        /// (Required) The type of font.
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.Required)]
        public const string Subtype = "/Subtype";

        /// <summary>
        /// (Required) The PostScript name of the font.
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.Required)]
        public const string BaseFont = "/BaseFont";

        /// <summary>
        /// (Required except for the standard 14 fonts; must be an indirect reference)
        /// A font descriptor describing the font�s metrics other than its glyph widths.
        /// Note: For the standard 14 fonts, the entries FirstChar, LastChar, Widths, and
        /// FontDescriptor must either all be present or all be absent. Ordinarily, they are
        /// absent; specifying them enables a standard font to be overridden.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.MustBeIndirect, typeof (PdfFontDescriptor))]
        public const string FontDescriptor = "/FontDescriptor";
    }
}
