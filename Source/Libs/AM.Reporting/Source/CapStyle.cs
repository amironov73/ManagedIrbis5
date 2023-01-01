// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* CapStyle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting;

/// <summary>
/// Specifies a line cap style.
/// </summary>
public enum CapStyle
{
    /// <summary>
    /// Specifies a line without a cap.
    /// </summary>
    None,

    /// <summary>
    /// Specifies a line with a circle cap.
    /// </summary>
    Circle,

    /// <summary>
    /// Specifies a line with a square cap.
    /// </summary>
    Square,

    /// <summary>
    /// Specifies a line with a diamond cap.
    /// </summary>
    Diamond,


    /// <summary>
    /// Specifies a line with an arrow cap.
    /// </summary>
    Arrow
}
