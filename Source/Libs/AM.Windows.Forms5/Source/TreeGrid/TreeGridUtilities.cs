// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridUtilities.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
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
            StringAlignment result = StringAlignment.Near;
            switch (alignment)
            {
                case TreeGridAlignment.Center:
                    result = StringAlignment.Center;
                    break;
                case TreeGridAlignment.Far:
                    result = StringAlignment.Far;
                    break;
            }
            return result;
        }

        public static Brush GetForegroundBrush
            (
                TreeGrid grid,
                TreeGridNode node,
                TreeGridNodeState state
            )
        {
            Brush result = (node.ForegroundColor == Color.Empty)
                ? (Brush)grid.Palette.Foreground
                : new SolidBrush(node.ForegroundColor);

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
                //brush = TreeGrid.Palette.
            }

            return result;
        }

        public static Brush GetBackgroundBrush
            (
                TreeGrid grid,
                TreeGridNode node,
                TreeGridNodeState state
            )
        {
            Brush result = (node.BackgroundColor == Color.Empty)
                ? (Brush)grid.Palette.Backrground
                : new SolidBrush(node.BackgroundColor);

            if ((state & TreeGridNodeState.Selected) != 0)
            {
                result = grid.Palette.SelectedBackground;
            }

            return result;
        }

        public static void DrawTreeCell
            (
                TreeGridDrawCellEventArgs args,
                TreeGridDrawLayout layout
            )
        {
            TreeGridNode node = args.Node;
            TreeGrid grid = args.TreeGrid;
            Graphics graphics = args.Graphics;
            Rectangle bounds = args.Bounds;
            string title = layout.TextOverride
                ?? args.TextOverride
                ?? node.Title;

            graphics.FillRectangle
                (
                    args.GetBackgroundBrush(),
                    bounds
                );

            if (!layout.Expand.IsEmpty)
            {
                Bitmap openOrClosed = args.GetStateBitmap();
                int top = bounds.Top
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
                CheckBoxRenderer
                    .DrawCheckBox
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
                       node.Icon,
                       layout.Icon.Left,
                       layout.Icon.Top
                    );
            }

            if (!string.IsNullOrEmpty(title))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.LineAlignment = StringAlignment.Center;
                    format.HotkeyPrefix = HotkeyPrefix.None;
                    format.FormatFlags |= StringFormatFlags.NoWrap;
                    format.Trimming = StringTrimming.EllipsisCharacter;

                    graphics.DrawString
                        (
                            title,
                            grid.Font,
                            args.GetForegroundBrush(),
                            layout.Text,
                            format
                        );
                }
            }
        }

        #endregion
    }
}
