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

namespace PdfSharpCore.Pdf.IO;

/// <summary>
/// Terminal symbols recognized by lexer.
/// </summary>
public enum Symbol
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
    Null,

    /// <summary>
    ///
    /// </summary>
    Integer,

    /// <summary>
    ///
    /// </summary>
    UInteger,

    /// <summary>
    ///
    /// </summary>
    Real,

    /// <summary>
    ///
    /// </summary>
    Boolean,

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
    Keyword,

    /// <summary>
    ///
    /// </summary>
    BeginStream,

    /// <summary>
    ///
    /// </summary>
    EndStream,

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
    BeginDictionary,

    /// <summary>
    ///
    /// </summary>
    EndDictionary,

    /// <summary>
    ///
    /// </summary>
    Obj,

    /// <summary>
    ///
    /// </summary>
    EndObj,

    /// <summary>
    ///
    /// </summary>
    R,

    /// <summary>
    ///
    /// </summary>
    XRef,

    /// <summary>
    ///
    /// </summary>
    Trailer,

    /// <summary>
    ///
    /// </summary>
    StartXRef,

    /// <summary>
    ///
    /// </summary>
    Eof,

    /// <summary>
    ///
    /// </summary>
    Long
}
