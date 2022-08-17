// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TreeGridDrawColumnEventArgs.cs -- аргументы для события, возникающего при перерисовке грида
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
/// Аргументы для события, возникающего при перерисовке грида.
/// </summary>
public sealed class TreeGridDrawCellEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Канва для отрисовки.
    /// </summary>
    public Graphics? Graphics { get; set; }

    /// <summary>
    /// Отрисовываемый грид.
    /// </summary>
    public TreeGrid? Grid { get; set; }

    /// <summary>
    /// Отрисовываемая нода.
    /// </summary>
    public TreeGridNode? Node { get; set; }

    /// <summary>
    /// Отрисовываемая колонка.
    /// </summary>
    public TreeGridColumn? Column { get; set; }

    /// <summary>
    /// Прямоугольник, подлежаший отрисовке.
    /// </summary>
    public Rectangle Bounds { get; set; }

    /// <summary>
    /// Состояние ноды.
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

    /// <summary>
    /// Переопределение текста.
    /// </summary>
    public string? TextOverride { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение кисти для текста.
    /// </summary>
    public Brush GetForegroundBrush()
    {
        return ForegroundOverride
               ?? TreeGridUtilities.GetForegroundBrush
                   (
                       Grid.ThrowIfNull(),
                       Node.ThrowIfNull(),
                       State
                   );
    }

    /// <summary>
    /// Получение кисти для фона.
    /// </summary>
    public Brush GetBackgroundBrush()
    {
        return BackgroundOverride
               ?? TreeGridUtilities.GetBackgroundBrush
                   (
                       Grid.ThrowIfNull(),
                       Node.ThrowIfNull(),
                       State
                   );
    }

    /// <summary>
    /// Получение иконки, отражающей текущее состояние ноды.
    /// </summary>
    public Bitmap GetStateBitmap()
    {
        // TODO: восстановить логику
        // Bitmap result = Node.Expanded
        //                   ? TreeEdit.Properties.Resources.Open
        //                   : TreeEdit.Properties.Resources.Closed;
        // //Icon icon = Icon.FromHandle(openOrClosed.GetHicon());
        // result.MakeTransparent(Color.White);
        // return result;

        return new Bitmap (16, 16);
    }

    /// <summary>
    /// Отрисовка фона.
    /// </summary>
    public void DrawBackground()
    {
        var brush = GetBackgroundBrush();
        Graphics?.FillRectangle (brush, Bounds);
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

        var node = Node;
        if (node is null)
        {
            Magna.Logger.LogDebug (nameof (DrawText) + ": node is null");
            return;
        }

        var column = Column;
        if (column is null)
        {
            Magna.Logger.LogDebug (nameof (DrawText) + ": column is null");
            return;
        }

        var grid = Grid;
        if (grid is null)
        {
            Magna.Logger.LogDebug (nameof (DrawText) + ": grid is null");
            return;
        }

        var brush = GetForegroundBrush();
        var left = Bounds.Left;
        var index = column._index;
        string? text = null;
        if (index == 0)
        {
            text = node.Title;
            left += node.Level * 16;
        }
        else
        {
            object? data = null;
            if (index - 1 < node.Data.Count)
            {
                data = node.Data[index - 1];
            }

            if (data != null)
            {
                text = data.ToString();
            }
        }

        var rectangle = new Rectangle
            (
                left,
                Bounds.Top,
                Bounds.Width - (left - Bounds.Left),
                Bounds.Height
            );

        if (!string.IsNullOrEmpty (text))
        {
            using var format = new StringFormat
            {
                Alignment = column.Alignment.ToStringAlignment(),
                Trimming = StringTrimming.EllipsisCharacter
            };
            format.FormatFlags |= StringFormatFlags.NoWrap;
            graphics.DrawString
                (
                    text,
                    grid.Font,
                    brush,
                    rectangle,
                    format
                );
        }
    }

    /// <summary>
    /// Отрисовка рамки вокруг выделенной ячейки.
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
