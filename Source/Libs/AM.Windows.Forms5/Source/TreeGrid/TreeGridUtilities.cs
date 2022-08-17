// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridUtilities.cs -- полезные методы для TreeGrid
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Полезные методы для <see cref="TreeGrid"/>.
/// </summary>
public static class TreeGridUtilities
{
    #region Public methods

    /// <summary>
    /// Toes the string alignment.
    /// </summary>
    /// <param name="alignment">The alignment.</param>
    /// <returns></returns>
    public static StringAlignment ToStringAlignment
        (
            this TreeGridAlignment alignment
        )
    {
        Sure.Defined (alignment);

        var result = alignment switch
        {
            TreeGridAlignment.Center => StringAlignment.Center,
            TreeGridAlignment.Far => StringAlignment.Far,
            TreeGridAlignment.Near => StringAlignment.Near,

            _ => throw new ArgumentOutOfRangeException (nameof (alignment), alignment, nameof (TreeGridAlignment))
        };

        return result;
    }

    /// <summary>
    /// Получение кисти для отрисовки текста ячейки.
    /// </summary>
    public static Brush GetForegroundBrush
        (
            TreeGrid grid,
            TreeGridNode node,
            TreeGridNodeState state
        )
    {
        Sure.NotNull (grid);
        Sure.NotNull (node);
        Sure.Defined (state);

        var result = (node.ForegroundColor == Color.Empty)
            ? grid.Palette.Foreground.Brush
            : new SolidBrush (node.ForegroundColor);

        if ((state & TreeGridNodeState.Selected) != 0)
        {
            result = grid.Palette.SelectedForeground;
        }

        if ((state & TreeGridNodeState.Disabled) != 0)
        {
            result = grid.Palette.Disabled;
        }

        if ((state & TreeGridNodeState.ReadOnly) != 0)
        {
            result = grid.Palette.ReadOnlyForeground;
        }

        return result;
    }

    /// <summary>
    /// Получение кисти для отрисовки фона ячейки.
    /// </summary>
    public static Brush GetBackgroundBrush
        (
            TreeGrid grid,
            TreeGridNode node,
            TreeGridNodeState state
        )
    {
        Sure.NotNull (grid);
        Sure.NotNull (node);
        Sure.Defined (state);

        var result = (node.BackgroundColor == Color.Empty)
            ? grid.Palette.Background.Brush
            : new SolidBrush (node.BackgroundColor);

        if ((state & TreeGridNodeState.Selected) != 0)
        {
            result = grid.Palette.SelectedBackground;
        }

        return result;
    }

    /// <summary>
    /// Отрисовка ячейки.
    /// </summary>
    public static void DrawTreeCell
        (
            TreeGridDrawCellEventArgs eventArgs,
            TreeGridDrawLayout layout
        )
    {
        Sure.NotNull (eventArgs);
        Sure.NotNull (layout);

        var node = eventArgs.Node;
        if (node is null)
        {
            Magna.Logger.LogDebug (nameof (DrawTreeCell) + ": node is null");
            return;
        }

        var grid = eventArgs.Grid;
        if (grid is null)
        {
            Magna.Logger.LogDebug (nameof (DrawTreeCell) + ": grid is null");
            return;
        }

        var graphics = eventArgs.Graphics;
        if (graphics is null)
        {
            Magna.Logger.LogDebug (nameof (DrawTreeCell) + ": graphics is null");
            return;
        }

        var bounds = eventArgs.Bounds;
        var title = layout.TextOverride ?? eventArgs.TextOverride ?? node.Title;

        graphics.FillRectangle
            (
                eventArgs.GetBackgroundBrush(),
                bounds
            );

        if (!layout.Expand.IsEmpty)
        {
            var openOrClosed = eventArgs.GetStateBitmap();
            var top = bounds.Top
                      + (bounds.Height - openOrClosed.Height) / 2;

            graphics.DrawImage
                (
                    openOrClosed,
                    layout.Expand.Left,
                    top,
                    8,
                    8
                );
        }

        if (!layout.Check.IsEmpty)
        {
            CheckBoxRenderer.DrawCheckBox
                (
                    graphics,
                    layout.Check.Location,
                    node.Checked
                        ? CheckBoxState.CheckedNormal
                        : CheckBoxState.UncheckedNormal
                );
        }

        if (!layout.Icon.IsEmpty)
        {
            graphics.DrawIcon
                (
                    node.Icon!,
                    layout.Icon.Left,
                    layout.Icon.Top
                );
        }

        if (!string.IsNullOrEmpty (title))
        {
            using var format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                HotkeyPrefix = HotkeyPrefix.None
            };
            format.FormatFlags |= StringFormatFlags.NoWrap;
            format.Trimming = StringTrimming.EllipsisCharacter;

            graphics.DrawString
                (
                    title,
                    grid.Font,
                    eventArgs.GetForegroundBrush(),
                    layout.Text,
                    format
                );
        }
    }

    #endregion
}
