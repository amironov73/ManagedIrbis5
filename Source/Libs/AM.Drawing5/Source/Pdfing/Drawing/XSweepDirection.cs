// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XSweepDirection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing;

/// <summary>
/// Defines the direction an elliptical arc is drawn.
/// </summary>
public enum XSweepDirection // Same values as System.Windows.Media.SweepDirection.
{
    /// <summary>
    /// Specifies that arcs are drawn in a counter clockwise (negative-angle) direction.
    /// </summary>
    Counterclockwise = 0,

    /// <summary>
    /// Specifies that arcs are drawn in a clockwise (positive-angle) direction.
    /// </summary>
    Clockwise = 1,
}
