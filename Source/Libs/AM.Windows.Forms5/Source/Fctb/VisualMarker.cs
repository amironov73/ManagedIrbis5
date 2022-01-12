// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* VisualMarker.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Fctb;

public class VisualMarker
{
    public readonly Rectangle rectangle;

    public VisualMarker (Rectangle rectangle)
    {
        this.rectangle = rectangle;
    }

    public virtual void Draw (Graphics gr, Pen pen)
    {
    }

    public virtual Cursor Cursor
    {
        get { return Cursors.Hand; }
    }
}

public class CollapseFoldingMarker : VisualMarker
{
    public readonly int iLine;

    public CollapseFoldingMarker (int iLine, Rectangle rectangle)
        : base (rectangle)
    {
        this.iLine = iLine;
    }

    public void Draw (Graphics gr, Pen pen, Brush backgroundBrush, Pen forePen)
    {
        //draw minus
        gr.FillRectangle (backgroundBrush, rectangle);
        gr.DrawRectangle (pen, rectangle);
        gr.DrawLine (forePen, rectangle.Left + 2, rectangle.Top + rectangle.Height / 2, rectangle.Right - 2,
            rectangle.Top + rectangle.Height / 2);
    }
}

public class ExpandFoldingMarker : VisualMarker
{
    public readonly int iLine;

    public ExpandFoldingMarker (int iLine, Rectangle rectangle)
        : base (rectangle)
    {
        this.iLine = iLine;
    }

    public void Draw (Graphics gr, Pen pen, Brush backgroundBrush, Pen forePen)
    {
        //draw plus
        gr.FillRectangle (backgroundBrush, rectangle);
        gr.DrawRectangle (pen, rectangle);
        gr.DrawLine (forePen, rectangle.Left + 2, rectangle.Top + rectangle.Height / 2, rectangle.Right - 2,
            rectangle.Top + rectangle.Height / 2);
        gr.DrawLine (forePen, rectangle.Left + rectangle.Width / 2, rectangle.Top + 2,
            rectangle.Left + rectangle.Width / 2, rectangle.Bottom - 2);
    }
}

public class FoldedAreaMarker : VisualMarker
{
    public readonly int iLine;

    public FoldedAreaMarker (int iLine, Rectangle rectangle)
        : base (rectangle)
    {
        this.iLine = iLine;
    }

    public override void Draw (Graphics gr, Pen pen)
    {
        gr.DrawRectangle (pen, rectangle);
    }
}

public class StyleVisualMarker : VisualMarker
{
    public Style Style { get; private set; }

    public StyleVisualMarker (Rectangle rectangle, Style style)
        : base (rectangle)
    {
        this.Style = style;
    }
}

public class VisualMarkerEventArgs : MouseEventArgs
{
    public Style Style { get; private set; }
    public StyleVisualMarker Marker { get; private set; }

    public VisualMarkerEventArgs (Style style, StyleVisualMarker marker, MouseEventArgs args)
        : base (args.Button, args.Clicks, args.X, args.Y, args.Delta)
    {
        this.Style = style;
        this.Marker = marker;
    }
}
