// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TreeGridDrawColumnHeaderEventArgs.cs -- аргументы события для перерисовки заголовка колонки грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргументы события для перерисовки заголовка колонки грида.
/// </summary>
public sealed class TreeGridDrawColumnHeaderEventArgs
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
    /// Колонка.
    /// </summary>
    public TreeGridColumn? Column { get; set; }

    /// <summary>
    /// Прямоугольник, подлежащий перерисовке.
    /// </summary>
    public Rectangle Bounds { get; set; }

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

        var brush = grid.Palette.HeaderBackground;
        graphics.FillRectangle
            (
                brush,
                Bounds
            );
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

        var column = Column;
        if (column is null)
        {
            Magna.Logger.LogDebug (nameof (DrawText) + ": column is null");
            return;
        }

        var brush = grid.Palette.HeaderForeground;
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        var title = column.Title;
        if (!string.IsNullOrEmpty (title))
        {
            graphics.DrawString
                (
                    title,
                    grid.Font,
                    brush,
                    Bounds,
                    format
                );
        }
    }

    #endregion
}
