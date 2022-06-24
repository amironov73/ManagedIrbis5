// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HoverBoxBlock.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Core.Entities;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// CSS boxes that have ":hover" selector on them.
/// </summary>
internal sealed class HoverBoxBlock
{
    /// <summary>
    /// the box that has :hover css on
    /// </summary>
    private readonly CssBox _cssBox;

    /// <summary>
    /// the :hover style block data
    /// </summary>
    private readonly CssBlock _cssBlock;

    /// <summary>
    /// Init.
    /// </summary>
    public HoverBoxBlock(CssBox cssBox, CssBlock cssBlock)
    {
        _cssBox = cssBox;
        _cssBlock = cssBlock;
    }

    /// <summary>
    /// the box that has :hover css on
    /// </summary>
    public CssBox CssBox
    {
        get { return _cssBox; }
    }

    /// <summary>
    /// the :hover style block data
    /// </summary>
    public CssBlock CssBlock
    {
        get { return _cssBlock; }
    }
}
