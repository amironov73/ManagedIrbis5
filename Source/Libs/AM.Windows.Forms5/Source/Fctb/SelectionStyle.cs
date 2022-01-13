// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SelectionSytle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System;
using System.Drawing.Drawing2D;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Renderer for selected area
/// </summary>
public class SelectionStyle
    : Style
{
    #region Properties

    /// <summary>
    /// Кисть для фона.
    /// </summary>
    public Brush? BackgroundBrush { get; set; }

    /// <summary>
    /// Кисть для символов.
    /// </summary>
    public Brush? ForegroundBrush { get; }

    /// <summary>
    /// Экспортируемый стиль?
    /// </summary>
    public override bool IsExportable
    {
        get => false;
        set
        {
            // тело оставлено пустым
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SelectionStyle
        (
            Brush backgroundBrush,
            Brush? foregroundBrush = null
        )
    {
        BackgroundBrush = backgroundBrush;
        ForegroundBrush = foregroundBrush;
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
        //draw background
        if (BackgroundBrush is not null)
        {
            graphics.SmoothingMode = SmoothingMode.None;
            var rect = new Rectangle (position.X, position.Y,
                (range.End.Column - range.Start.Column) * range.tb.CharWidth, range.tb.CharHeight);
            if (rect.Width == 0)
            {
                return;
            }

            graphics.FillRectangle (BackgroundBrush, rect);

            //
            if (ForegroundBrush != null)
            {
                //draw text
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var r = new TextRange (range.tb, range.Start.Column, range.Start.Line,
                    Math.Min (range.tb[range.End.Line].Count, range.End.Column), range.End.Line);

                using var style = new TextStyle (ForegroundBrush, null, FontStyle.Regular);
                style.Draw (graphics, new Point (position.X, position.Y - 1), r);
            }
        }
    }

    #endregion
}
