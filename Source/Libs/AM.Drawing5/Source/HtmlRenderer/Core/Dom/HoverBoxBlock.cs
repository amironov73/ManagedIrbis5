// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HoverBoxBlock.cs -- блоки CSS, у которых есть селектор ":hover"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Core.Entities;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Блоки CSS, у которых есть селектор ":hover".
/// </summary>
internal sealed class HoverBoxBlock
{
    #region Properties

    /// <summary>
    /// Поле с <c>:hover</c>.
    /// </summary>
    public CssBox CssBox { get; }

    /// <summary>
    /// Данные блока стиля <c>:hover</c>.
    /// </summary>
    public CssBlock CssBlock { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HoverBoxBlock
        (
            CssBox cssBox,
            CssBlock cssBlock
        )
    {
        CssBox = cssBox;
        CssBlock = cssBlock;
    }

    #endregion
}
