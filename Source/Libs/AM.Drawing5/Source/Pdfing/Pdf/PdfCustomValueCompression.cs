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
/// This class is undocumented and may change or drop in future releases.
/// </summary>
public enum PdfCustomValueCompressionMode
{
    /// <summary>
    /// Use document default to determine compression.
    /// </summary>
    Default,

    /// <summary>
    /// Leave custom values uncompressed.
    /// </summary>
    Uncompressed,

    /// <summary>
    /// Compress custom values using FlateDecode.
    /// </summary>
    Compressed
}
