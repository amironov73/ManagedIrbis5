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

namespace PdfSharpCore.Pdf.Security;

/// <summary>
/// Specifies the security level of the PDF document.
/// </summary>
public enum PdfDocumentSecurityLevel
{
    /// <summary>
    /// Document is not protected.
    /// </summary>
    None,

    /// <summary>
    /// Document is protected with 40-bit security. This option
    /// is for compatibility with Acrobat 3 and 4 only.
    /// Use Encrypted128Bit whenever possible.
    /// </summary>
    Encrypted40Bit,

    /// <summary>
    /// Document is protected with 128-bit security.
    /// </summary>
    Encrypted128Bit
}
