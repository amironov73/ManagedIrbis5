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

namespace PdfSharpCore.Pdf.IO;

/// <summary>
/// INTERNAL USE ONLY.
/// </summary>
[Flags]
internal enum PdfWriterOptions
{
    /// <summary>
    /// If only this flag is specified the result is a regular
    /// valid PDF stream.
    /// </summary>
    Regular = 0x000000,

    /// <summary>
    /// Omit writing stream data. For debugging purposes only.
    /// With this option the result is not valid PDF.
    /// </summary>
    OmitStream = 0x000001,

    /// <summary>
    /// Omit inflate filter. For debugging purposes only.
    /// </summary>
    OmitInflation = 0x000002
}
