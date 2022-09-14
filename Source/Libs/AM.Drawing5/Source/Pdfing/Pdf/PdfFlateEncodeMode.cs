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
/// Sets the mode for the Deflater (FlateEncoder).
/// </summary>
public enum PdfFlateEncodeMode
{
    /// <summary>
    /// The default mode.
    /// </summary>
    Default,

    /// <summary>
    /// Fast encoding, but larger PDF files.
    /// </summary>
    BestSpeed,

    /// <summary>
    /// Best compression, but takes more time.
    /// </summary>
    BestCompression
}
