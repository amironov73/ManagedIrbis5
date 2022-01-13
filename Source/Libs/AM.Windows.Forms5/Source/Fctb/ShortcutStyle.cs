// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ShortcutSytle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Draws small rectangle for popup menu
/// </summary>
public class ShortcutStyle
    : Style
{
    #region Fields

    /// <summary>
    /// Перо для границы.
    /// </summary>
    public Pen borderPen;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ShortcutStyle
        (
            Pen borderPen
        )
    {
        this.borderPen = borderPen;
    }

    #endregion

    #region Style members

    /// <inheritdoc cref="Style.Draw"/>
    public override void Draw
        (
            Graphics graphics,
            Point position,
            TextRange range
        )
    {
        //get last char coordinates
        var p = range.tb.PlaceToPoint (range.End);

        //draw small square under char
        var rect = new Rectangle (p.X - 5, p.Y + range.tb.CharHeight - 2, 4, 3);
        graphics.FillPath (Brushes.White, GetRoundedRectangle (rect, 1));
        graphics.DrawPath (borderPen, GetRoundedRectangle (rect, 1));

        //add visual marker for handle mouse events
        AddVisualMarker (range.tb,
            new StyleVisualMarker (
                new Rectangle (p.X - range.tb.CharWidth, p.Y, range.tb.CharWidth, range.tb.CharHeight), this));
    }

    #endregion
}
