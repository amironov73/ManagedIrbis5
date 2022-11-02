// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RDashStyle.cs -- стиль штриха
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Drawing.HtmlRenderer.Adapters.Entities;

/// <summary>
/// Specifies the style of dashed lines drawn with a <see cref="RPen"/> object.
/// </summary>
public enum RDashStyle
{
    /// <summary>
    /// Сплошная линия.
    /// </summary>
    Solid,

    /// <summary>
    /// Тире.
    /// </summary>
    Dash,

    /// <summary>
    /// Точка.
    /// </summary>
    Dot,

    /// <summary>
    /// Тире и точка.
    /// </summary>
    DashDot,

    /// <summary>
    /// Тире и две точки.
    /// </summary>
    DashDotDot,

    /// <summary>
    /// Кастомный штрих.
    /// </summary>
    Custom
}
