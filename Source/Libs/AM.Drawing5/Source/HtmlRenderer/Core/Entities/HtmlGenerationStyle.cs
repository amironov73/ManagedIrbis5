// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlGenerationStyle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Drawing.HtmlRenderer.Core.Entities;

/// <summary>
/// Controls the way styles are generated when html is generated.
/// </summary>
public enum HtmlGenerationStyle
{
    /// <summary>
    /// styles are not generated at all
    /// </summary>
    None = 0,

    /// <summary>
    /// style are inserted in style attribute for each html tag
    /// </summary>
    Inline = 1,

    /// <summary>
    /// style section is generated in the head of the html
    /// </summary>
    InHeader = 2
}
