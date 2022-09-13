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

namespace PdfSharpCore.Pdf;

/// <summary>
/// Identifies the state of the document
/// </summary>
[Flags]
enum DocumentState
{
    /// <summary>
    /// The document was created from scratch.
    /// </summary>
    Created = 0x0001,

    /// <summary>
    /// The document was created by opening an existing PDF file.
    /// </summary>
    Imported = 0x0002,

    /// <summary>
    /// The document is disposed.
    /// </summary>
    Disposed = 0x8000
}
