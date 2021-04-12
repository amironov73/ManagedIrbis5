// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridDrawRowEventArgs.cs
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
    public sealed class TreeGridDrawNodeEventArgs
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
        /// Draws the background.
        /// </summary>
        public void DrawBackground()
        {
            Brush brush = ForegroundOverride
                ?? TreeGridUtilities.GetBackgroundBrush
                (
                    TreeGrid,
                    Node,
                    State
                );

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
            Brush brush = ForegroundOverride
                ?? TreeGridUtilities.GetForegroundBrush
                (
                    TreeGrid,
                    Node,
                    State
                );
            string text = Node.Title;

            if (!string.IsNullOrEmpty(text))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Near;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    format.FormatFlags |= StringFormatFlags.NoWrap;
                    Graphics.DrawString
                        (
                            text,
                            TreeGrid.Font,
                            brush,
                            Bounds,
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
