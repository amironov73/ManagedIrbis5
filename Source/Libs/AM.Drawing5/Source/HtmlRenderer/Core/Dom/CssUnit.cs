// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CssUnit.cs -- всевозможные единицы длины CSS.
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Представляет всевозможные единицы длины CSS.
/// </summary>
/// <remarks>
/// http://www.w3.org/TR/CSS21/syndata.html#length-units
/// </remarks>
internal enum CssUnit
{
    /// <summary>
    /// Нет.
    /// </summary>
    None,

    /// <summary>
    /// em
    /// </summary>
    Ems,

    /// <summary>
    /// Пикселы.
    /// </summary>
    Pixels,

    /// <summary>
    /// ex
    /// </summary>
    Ex,

    /// <summary>
    /// Дюймы.
    /// </summary>
    Inches,

    /// <summary>
    /// Сантиметры.
    /// </summary>
    Centimeters,

    /// <summary>
    /// Миллиметры.
    /// </summary>
    Millimeters,

    /// <summary>
    /// Точки.
    /// </summary>
    Points,

    /// <summary>
    /// pica.
    /// </summary>
    Picas
}
