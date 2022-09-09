// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DirtyFlags.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace PdfSharpCore.Drawing.Pdf;

/// <summary>
///
/// </summary>
[Flags]
internal enum DirtyFlags
{
    /// <summary>
    ///
    /// </summary>
    Ctm = 0x00000001,

    /// <summary>
    ///
    /// </summary>
    ClipPath = 0x00000002,

    /// <summary>
    ///
    /// </summary>
    LineWidth = 0x00000010,

    /// <summary>
    ///
    /// </summary>
    LineJoin = 0x00000020,

    /// <summary>
    ///
    /// </summary>
    MiterLimit = 0x00000040,

    /// <summary>
    ///
    /// </summary>
    StrokeFill = 0x00000070,
}
