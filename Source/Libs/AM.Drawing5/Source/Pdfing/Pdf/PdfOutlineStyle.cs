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

#region Using directives

using System;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Specifies the font style for the outline (bookmark) text.
///  </summary>
[Flags]
public enum PdfOutlineStyle
{
    // Reference:  TABLE 8.5 Ouline Item flags / Page 587

    /// <summary>
    /// Outline text is displayed using a regular font.
    /// </summary>
    Regular = 0,

    /// <summary>
    /// Outline text is displayed using an italic font.
    /// </summary>
    Italic = 1,

    /// <summary>
    /// Outline text is displayed using a bold font.
    /// </summary>
    Bold = 2,

    /// <summary>
    /// Outline text is displayed using a bold and italic font.
    /// </summary>
    BoldItalic = 3
}
