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

namespace PdfSharpCore.Pdf;

/// <summary>
/// Specifies the encoding schema used for an XFont when converted into PDF.
/// </summary>
public enum PdfFontEncoding
{
    // TABLE

    /// <summary>
    /// Cause a font to use Windows-1252 encoding to encode text rendered with this font.
    /// Same as Windows1252 encoding.
    /// </summary>
    WinAnsi = 0,

    ///// <summary>
    ///// Cause a font to use Windows-1252 (aka WinAnsi) encoding to encode text rendered with this font.
    ///// </summary>
    //Windows1252 = 0,

    /// <summary>
    /// Cause a font to use Unicode encoding to encode text rendered with this font.
    /// </summary>
    Unicode = 1

    // Implementation note: PdfFontEncoding uses incorrect terms.
    // WinAnsi correspond to WinAnsiEncoding, while Unicode uses glyph indices.
    // Furthermre the term WinAnsi is an oxymoron.
    // Reference: TABLE  D.1 Latin-text encodings / Page 996
}
