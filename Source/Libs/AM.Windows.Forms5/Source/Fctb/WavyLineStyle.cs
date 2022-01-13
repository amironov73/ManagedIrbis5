// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* WavyLineSytle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Drawing;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// This style draws a wavy line below a given text range.
/// </summary>
/// <remarks>Thanks for Yallie</remarks>
public class WavyLineStyle
    : Style
{
    #region Properties

    private Pen Pen { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WavyLineStyle
        (
            int alpha,
            Color color
        )
    {
        Pen = new Pen (Color.FromArgb (alpha, color));
    }

    #endregion

    #region Private members

    private void DrawWavyLine
        (
            Graphics graphics,
            Point start,
            Point end
        )
    {
        if (end.X - start.X < 2)
        {
            graphics.DrawLine (Pen, start, end);
            return;
        }

        var offset = -1;
        var points = new List<Point>();

        for (var i = start.X; i <= end.X; i += 2)
        {
            points.Add (new Point (i, start.Y + offset));
            offset = -offset;
        }

        graphics.DrawLines (Pen, points.ToArray());
    }

    #endregion

    #region Style members

    /// <inheritdoc cref="Style.Draw"/>
    public override void Draw
        (
            Graphics graphics,
            Point pos,
            TextRange range
        )
    {
        var size = GetSizeOfRange (range);
        var start = new Point (pos.X, pos.Y + size.Height - 1);
        var end = new Point (pos.X + size.Width, pos.Y + size.Height - 1);
        DrawWavyLine (graphics, start, end);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="Style.Dispose"/>
    public override void Dispose()
    {
        base.Dispose();

        if (Pen != null)
        {
            Pen.Dispose();
        }
    }

    #endregion
}
