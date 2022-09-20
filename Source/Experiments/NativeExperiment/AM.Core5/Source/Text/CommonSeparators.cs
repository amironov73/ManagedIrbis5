﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* CommonSeparators.cs -- широко применяемые разделители текста
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Text;

/// <summary>
/// Широко применяемые разделители текста, тщательно
/// собранные в одном месте.
/// </summary>
public static class CommonSeparators
{
    #region Properties

    /// <summary>
    /// Colon.
    /// </summary>
    public static readonly char[] Colon = { ':' };

    /// <summary>
    /// Comma.
    /// </summary>
    public static readonly char[] Comma = { ',' };

    /// <summary>
    /// Comma and semicolon.
    /// </summary>
    public static readonly char[] CommaAndSemicolon = { ',', ';' };

    /// <summary>
    /// Dot.
    /// </summary>
    public static readonly char[] Dot = { '.' };

    /// <summary>
    /// Equal sign.
    /// </summary>
    public static readonly char[] EqualSign = { '=' };

    /// <summary>
    /// Minus sign.
    /// </summary>
    public static readonly char[] Minus = { '-' };

    /// <summary>
    /// Newline.
    /// </summary>
    public static readonly char[] NewLine = { '\r', '\n' };

    /// <summary>
    /// Newline and percent sign.
    /// </summary>
    public static readonly char[] NewLineAndPercent = { '\r', '\n', '%' };

    /// <summary>
    /// Number sign.
    /// </summary>
    public static readonly char[] NumberSign = { '#' };

    /// <summary>
    /// Pipe sign.
    /// </summary>
    public static readonly char[] Pipe = { '|' };

    /// <summary>
    /// Semicolon.
    /// </summary>
    public static readonly char[] Semicolon = { ';' };

    /// <summary>
    /// Slash.
    /// </summary>
    public static readonly char[] Slash = { '/' };

    /// <summary>
    /// Space.
    /// </summary>
    public static readonly char[] Space = { ' ' };

    /// <summary>
    /// Space or tabulation.
    /// </summary>
    public static readonly char[] SpaceOrTab = { ' ', '\t' };

    /// <summary>
    /// Tabulation.
    /// </summary>
    public static readonly char[] Tab = { '\t' };

    /// <summary>
    /// Vertical line.
    /// </summary>
    public static readonly char[] VerticalLine = { '|' };

    #endregion
}
