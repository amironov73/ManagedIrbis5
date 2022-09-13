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
/// Specifies what color model is used in a PDF document.
/// </summary>
public enum PdfColorMode
{
    /// <summary>
    /// All color values are written as specified in the XColor objects they come from.
    /// </summary>
    Undefined,

    /// <summary>
    /// All colors are converted to RGB.
    /// </summary>
    Rgb,

    /// <summary>
    /// All colors are converted to CMYK.
    /// </summary>
    Cmyk
}
