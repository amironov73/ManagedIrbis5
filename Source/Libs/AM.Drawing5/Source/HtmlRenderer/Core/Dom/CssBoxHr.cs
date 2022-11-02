// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssBoxHr.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Adapters;
using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Core.Handlers;
using AM.Drawing.HtmlRenderer.Core.Parse;
using AM.Drawing.HtmlRenderer.Core.Utils;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// CSS box for hr element.
/// </summary>
internal sealed class CssBoxHr
    : CssBox
{
    #region Construction

    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="parent">the parent box of this box</param>
    /// <param name="tag">the html tag data of this box</param>
    public CssBoxHr
        (
            CssBox parent,
            HtmlTag tag
        )
        : base (parent, tag)
    {
        Display = CssConstants.Block;
    }

    #endregion

    #region CssBox members

    /// <summary>
    /// Measures the bounds of box and children, recursively.<br/>
    /// Performs layout of the DOM structure creating lines by set bounds restrictions.
    /// </summary>
    /// <param name="g">Device context to use</param>
    protected override void PerformLayoutImp (RGraphics g)
    {
        if (Display == CssConstants.None)
            return;

        RectanglesReset();

        var prevSibling = DomUtils.GetPreviousSibling (this);
        var left = ContainingBlock.Location.X + ContainingBlock.ActualPaddingLeft + ActualMarginLeft +
                   ContainingBlock.ActualBorderLeftWidth;
        var top = (prevSibling == null && ParentBox != null ? ParentBox.ClientTop : ParentBox == null ? Location.Y : 0)
                  + MarginTopCollapse (prevSibling)
                  + (prevSibling != null ? prevSibling.ActualBottom + prevSibling.ActualBorderBottomWidth : 0);
        Location = new RPoint (left, top);
        ActualBottom = top;

        //width at 100% (or auto)
        var minimumWidth = GetMinimumWidth();
        var width = ContainingBlock.Size.Width
                    - ContainingBlock.ActualPaddingLeft - ContainingBlock.ActualPaddingRight
                    - ContainingBlock.ActualBorderLeftWidth - ContainingBlock.ActualBorderRightWidth
                    - ActualMarginLeft - ActualMarginRight - ActualBorderLeftWidth - ActualBorderRightWidth;

        //Check width if not auto
        if (Width != CssConstants.Auto && !string.IsNullOrEmpty (Width))
        {
            width = CssValueParser.ParseLength (Width, width, this);
        }

        if (width < minimumWidth || width >= 9999)
            width = minimumWidth;

        var height = ActualHeight;
        if (height < 1)
        {
            height = Size.Height + ActualBorderTopWidth + ActualBorderBottomWidth;
        }

        if (height < 1)
        {
            height = 2;
        }

        if (height <= 2 && ActualBorderTopWidth < 1 && ActualBorderBottomWidth < 1)
        {
            BorderTopStyle = BorderBottomStyle = CssConstants.Solid;
            BorderTopWidth = "1px";
            BorderBottomWidth = "1px";
        }

        Size = new RSize (width, height);

        ActualBottom = Location.Y + ActualPaddingTop + ActualPaddingBottom + height;
    }

    /// <summary>
    /// Paints the fragment
    /// </summary>
    /// <param name="graphics">the device to draw to</param>
    protected override void PaintImp
        (
            RGraphics graphics
        )
    {
        var offset = HtmlContainer != null && !IsFixed ? HtmlContainer.ScrollOffset : RPoint.Empty;
        var rect = new RRect (Bounds.X + offset.X, Bounds.Y + offset.Y, Bounds.Width, Bounds.Height);

        if (rect.Height > 2 && RenderUtils.IsColorVisible (ActualBackgroundColor))
        {
            graphics.DrawRectangle (graphics.GetSolidBrush (ActualBackgroundColor), rect.X, rect.Y, rect.Width, rect.Height);
        }

        var b1 = graphics.GetSolidBrush (ActualBorderTopColor);
        BordersDrawHandler.DrawBorder (Border.Top, graphics, this, b1, rect);

        if (rect.Height > 1)
        {
            var b2 = graphics.GetSolidBrush (ActualBorderLeftColor);
            BordersDrawHandler.DrawBorder (Border.Left, graphics, this, b2, rect);

            var b3 = graphics.GetSolidBrush (ActualBorderRightColor);
            BordersDrawHandler.DrawBorder (Border.Right, graphics, this, b3, rect);

            var b4 = graphics.GetSolidBrush (ActualBorderBottomColor);
            BordersDrawHandler.DrawBorder (Border.Bottom, graphics, this, b4, rect);
        }
    }

    #endregion
}
