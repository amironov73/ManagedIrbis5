// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridDrawColumnEventArgs.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public sealed class TreeGridDrawCellEventArgs
        : EventArgs
    {
        /// <summary>
        /// Gets or sets the graphics.
        /// </summary>
        /// <value>The graphics.</value>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Gets or sets the tree grid.
        /// </summary>
        /// <value>The tree grid.</value>
        public TreeGrid TreeGrid { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>The node.</value>
        public TreeGridNode Node { get; set; }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>The column.</value>
        public TreeGridColumn Column { get; set; }

        /// <summary>
        /// Gets or sets the bounds.
        /// </summary>
        /// <value>The bounds.</value>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public TreeGridNodeState State { get; set; }

        /// <summary>
        /// Gets or sets the background override.
        /// </summary>
        /// <value>The background override.</value>
        public Brush BackgroundOverride { get; set; }

        /// <summary>
        /// Gets or sets the foreground override.
        /// </summary>
        /// <value>The foreground override.</value>
        public Brush ForegroundOverride { get; set; }

        /// <summary>
        /// Gets or sets the text override.
        /// </summary>
        /// <value>The text override.</value>
        public string TextOverride { get; set; }

        /// <summary>
        /// Gets the foreground brush.
        /// </summary>
        /// <returns></returns>
        public Brush GetForegroundBrush ()
        {
            return ForegroundOverride
                ?? TreeGridUtilities.GetForegroundBrush
                (
                    TreeGrid,
                    Node,
                    State
                );
        }

        /// <summary>
        /// Gets the background brush.
        /// </summary>
        /// <returns></returns>
        public Brush GetBackgroundBrush ()
        {
            return BackgroundOverride
                ?? TreeGridUtilities.GetBackgroundBrush
                (
                    TreeGrid,
                    Node,
                    State
                );
        }

        public Bitmap GetStateBitmap ()
        {
            // Bitmap result = Node.Expanded
            //                   ? TreeEdit.Properties.Resources.Open
            //                   : TreeEdit.Properties.Resources.Closed;
            // //Icon icon = Icon.FromHandle(openOrClosed.GetHicon());
            // result.MakeTransparent(Color.White);
            // return result;

            return new Bitmap(16, 16);
        }

        /// <summary>
        /// Draws the background.
        /// </summary>
        public void DrawBackground()
        {
            Brush brush = GetBackgroundBrush();

            Graphics.FillRectangle
                (
                brush,
                Bounds
                );
        }

        /// <summary>
        /// Draws the text.
        /// </summary>
        public void DrawText()
        {
            Brush brush = GetForegroundBrush();

            int left = Bounds.Left;
            int index = Column._index;
            string text = null;
            if (index == 0)
            {
                text = Node.Title;
                left += Node.Level * 16;

            }
            else
            {
                object data = null;
                if ((index - 1) < Node.Data.Count)
                {
                    data = Node.Data[index - 1];
                }
                if (data != null)
                {
                    text = data.ToString();
                }
            }
            Rectangle r = new Rectangle
                (
                left,
                Bounds.Top,
                Bounds.Width - (left - Bounds.Left),
                Bounds.Height
                );

            //if (Column.Kind == TreeGridColumnKind.Ordinary)
            //{
            if (!string.IsNullOrEmpty(text))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = Column.Alignment
                        .ToStringAlignment();
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    format.FormatFlags |= StringFormatFlags.NoWrap;
                    Graphics.DrawString
                        (
                            text,
                            TreeGrid.Font,
                            brush,
                            r,
                            format
                        );
                }
            }
        }

        /// <summary>
        /// Draws the frame.
        /// </summary>
        public void DrawSelection()
        {
            if ((State & TreeGridNodeState.Selected) != 0)
            {
                using (Pen pen = new Pen(Color.Black))
                {
                    pen.DashStyle = DashStyle.Dot;
                    Rectangle r = Bounds;
                    r.Inflate(-1, -1);
                    Graphics.DrawRectangle(pen, r);
                }
            }
        }
    }
}
