// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssUnit.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Represents the possible units of the CSS lengths
/// </summary>
/// <remarks>
/// http://www.w3.org/TR/CSS21/syndata.html#length-units
/// </remarks>
internal enum CssUnit
{
    None,
    Ems,
    Pixels,
    Ex,
    Inches,
    Centimeters,
    Milimeters,
    Points,
    Picas
}
