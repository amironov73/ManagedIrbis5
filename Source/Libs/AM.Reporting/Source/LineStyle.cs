// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* LineStyle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting;

/// <summary>
/// Specifies the style of a border line.
/// </summary>
public enum LineStyle
{
    /// <summary>
    /// Specifies a solid line.
    /// </summary>
    Solid,

    /// <summary>
    /// Specifies a line consisting of dashes.
    /// </summary>
    Dash,

    /// <summary>
    /// Specifies a line consisting of dots.
    /// </summary>
    Dot,

    /// <summary>
    /// Specifies a line consisting of a repeating pattern of dash-dot.
    /// </summary>
    DashDot,

    /// <summary>
    /// Specifies a line consisting of a repeating pattern of dash-dot-dot.
    /// </summary>
    DashDotDot,

    /// <summary>
    /// Specifies a double line.
    /// </summary>
    Double,

    /// <summary>
    /// Specifies a custom line.
    /// </summary>
    Custom
}
