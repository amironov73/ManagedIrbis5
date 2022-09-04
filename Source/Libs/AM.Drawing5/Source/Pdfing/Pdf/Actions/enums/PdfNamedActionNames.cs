// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdftNamedActionNames.cs --
 * Ars Magna project, http://arsmagna.ru
 */


namespace PdfSharpCore.Pdf.Actions;

/// <summary>
/// Specifies the predefined PDF actions.
/// </summary>
public enum PdfNamedActionNames
{
    /// <summary>
    /// Go to next page.
    /// </summary>
    NextPage,

    /// <summary>
    /// Go to previous page.
    /// </summary>
    PrevPage,

    /// <summary>
    /// Go to first page.
    /// </summary>
    FirstPage,

    /// <summary>
    /// Go to last page.
    /// </summary>
    LastPage
}
