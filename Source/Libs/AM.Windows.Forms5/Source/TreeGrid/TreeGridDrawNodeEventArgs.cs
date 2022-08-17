// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TreeGridDrawRowEventArgs.cs -- аргументы события для перерисовки узла грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргументы события для перерисовки узла грида.
/// </summary>
public sealed class TreeGridDrawNodeEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Канва для рисования.
    /// </summary>
    public Graphics? Graphics { get; set; }

    /// <summary>
    /// Грид.
    /// </summary>
    public TreeGrid? Grid { get; set; }

    /// <summary>
    /// Канва, подлежащая перерисовке.
    /// </summary>
    public TreeGridNode? Node { get; set; }

    /// <summary>
    /// Прямоугольник, подлежащий перерисовке.
    /// </summary>
    public Rectangle Bounds { get; set; }

    /// <summary>
    /// Состояние ноды
    /// </summary>
    public TreeGridNodeState State { get; set; }

    /// <summary>
    /// Переопределение цвета фона.
    /// </summary>
    public Brush? BackgroundOverride { get; set; }

    /// <summary>
    /// Переопределение цвета текста.
    /// </summary>
    public Brush? ForegroundOverride { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Отрисовка фона.
    /// </summary>
    public void DrawBackground()
    {
        var graphics = Graphics;
        if (graphics is null)
        {
            Magna.Logger.LogDebug (nameof (DrawBackground) + ": graphics is null");
            return;
        }

        var grid = Grid;
        if (grid is null)
        {
            Magna.Logger.LogDebug (nameof (DrawBackground) + ": grid is null");
            return;
        }

        var node = Node;
        if (node is null)
        {
            Magna.Logger.LogDebug (nameof (DrawBackground) + ": node is null");
            return;
        }

        var brush = ForegroundOverride
            ?? TreeGridUtilities.GetBackgroundBrush
                (
                    grid,
                    node,
                    State
                );

        graphics.FillRectangle (brush, Bounds);
    }

    /// <summary>
    /// Отрисовка текста.
    /// </summary>
    public void DrawText()
    {
        var graphics = Graphics;
        if (graphics is null)
        {
            Magna.Logger.LogDebug (nameof (DrawText) + ": graphics is null");
            return;
        }

        var grid = Grid;
        if (grid is null)
        {
            Magna.Logger.LogDebug (nameof (DrawText) + ": grid is null");
            return;
        }

        var node = Node;
        if (node is null)
        {
            Magna.Logger.LogDebug (nameof (DrawText) + ": node is null");
            return;
        }

        var brush = ForegroundOverride
            ?? TreeGridUtilities.GetForegroundBrush
                (
                    grid,
                    node,
                    State
                );

        var text = node.Title;
        if (!string.IsNullOrEmpty (text))
        {
            using var format = new StringFormat
            {
                Alignment = StringAlignment.Near,
                Trimming = StringTrimming.EllipsisCharacter
            };
            format.FormatFlags |= StringFormatFlags.NoWrap;
            graphics.DrawString
                (
                    text,
                    grid.Font,
                    brush,
                    Bounds,
                    format
                );
        }
    }

    /// <summary>
    /// Отрисовка рамки выделения.
    /// </summary>
    public void DrawSelection()
    {
        var graphics = Graphics;
        if (graphics is null)
        {
            Magna.Logger.LogDebug (nameof (DrawSelection) + ": graphics is null");
            return;
        }

        if ((State & TreeGridNodeState.Selected) != 0)
        {
            using var pen = new Pen (Color.Black) { DashStyle = DashStyle.Dot };
            var r = Bounds;
            r.Inflate (-1, -1);
            graphics.DrawRectangle (pen, r);
        }
    }

    #endregion
}
