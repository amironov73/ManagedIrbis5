// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* BorderLines.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Reporting;

/// <summary>
/// Specifies the sides of a border.
/// </summary>
[Flags]
public enum BorderLines
{
    /// <summary>
    /// Specifies no border lines.
    /// </summary>
    None = 0,

    /// <summary>
    /// Specifies the left border line.
    /// </summary>
    Left = 1,

    /// <summary>
    /// Specifies the right border line.
    /// </summary>
    Right = 2,

    /// <summary>
    /// Specifies the top border line.
    /// </summary>
    Top = 4,

    /// <summary>
    /// Specifies the bottom border line.
    /// </summary>
    Bottom = 8,

    /// <summary>
    /// Specifies all border lines.
    /// </summary>
    All = 15
}
