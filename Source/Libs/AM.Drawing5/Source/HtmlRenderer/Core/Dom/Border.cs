// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Border.cs -- типы границ
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Типы границ.
/// </summary>
internal enum Border
{
    /// <summary>
    /// Верхняя граница.
    /// </summary>
    Top,

    /// <summary>
    /// Правая граница.
    /// </summary>
    Right,

    /// <summary>
    /// Нижняя граница.
    /// </summary>
    Bottom,

    /// <summary>
    /// Левая граница.
    /// </summary>
    Left
}
