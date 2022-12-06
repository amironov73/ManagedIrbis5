// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PathStart.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing;

/// <summary>
/// Indicates how to handle the first point of a path.
/// </summary>
internal enum PathStart
{
    /// <summary>
    /// Set the current position to the first point.
    /// </summary>
    MoveTo1st,

    /// <summary>
    /// Draws a line to the first point.
    /// </summary>
    LineTo1st,

    /// <summary>
    /// Ignores the first point.
    /// </summary>
    Ignore1st,
}
