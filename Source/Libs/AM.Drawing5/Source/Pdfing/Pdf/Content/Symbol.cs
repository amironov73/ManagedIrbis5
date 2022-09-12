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

namespace PdfSharpCore.Pdf.Content;

#pragma warning disable 1591

/// <summary>
/// Terminal symbols recognized by PDF content stream lexer.
/// </summary>
public enum CSymbol
{
    /// <summary>
    ///
    /// </summary>
    None,

    /// <summary>
    ///
    /// </summary>
    Comment,

    /// <summary>
    ///
    /// </summary>
    Integer,

    /// <summary>
    ///
    /// </summary>
    Real,

    /*Boolean?,*/

    /// <summary>
    ///
    /// </summary>
    String,

    /// <summary>
    ///
    /// </summary>
    HexString,

    /// <summary>
    ///
    /// </summary>
    UnicodeString,

    /// <summary>
    ///
    /// </summary>
    UnicodeHexString,

    /// <summary>
    ///
    /// </summary>
    Name,

    /// <summary>
    ///
    /// </summary>
    Operator,

    /// <summary>
    ///
    /// </summary>
    BeginArray,

    /// <summary>
    ///
    /// </summary>
    EndArray,

    /// <summary>
    ///
    /// </summary>
    Dictionary, // HACK: << ... >> is scanned as string literal.

    /// <summary>
    ///
    /// </summary>
    Eof,

    /// <summary>
    ///
    /// </summary>
    Error = -1,
}
