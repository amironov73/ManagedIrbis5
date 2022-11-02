// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* RFontStyle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Drawing.HtmlRenderer.Adapters.Entities;

/// <summary>
/// Specifies style information applied to text.
/// </summary>
[Flags]
public enum RFontStyle
{
    /// <summary>
    /// Обычное начертание.
    /// </summary>
    Regular = 0,

    /// <summary>
    /// Полужирное начертание.
    /// </summary>
    Bold = 1,

    /// <summary>
    /// Курсивное начертание.
    /// </summary>
    Italic = 2,

    /// <summary>
    /// Подчеркивание.
    /// </summary>
    Underline = 4,

    /// <summary>
    /// Зачеркивание.
    /// </summary>
    Strikeout = 8,
}
