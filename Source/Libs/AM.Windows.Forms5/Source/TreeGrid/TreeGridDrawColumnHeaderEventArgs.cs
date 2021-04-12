// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridDrawColumnHeaderEventArgs.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public sealed class TreeGridDrawColumnHeaderEventArgs
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
        /// Gets or sets the rectangle.
        /// </summary>
        /// <value>The rectangle.</value>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Draws the background.
        /// </summary>
        public void DrawBackground()
        {
            Brush brush = TreeGrid.Palette.HeaderBackground;
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
            Brush brush = TreeGrid.Palette.HeaderForeground;
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                Graphics.DrawString
                    (
                        Column.Title,
                        TreeGrid.Font,
                        brush,
                        Bounds,
                        format
                    );
            }
        }
    }
}
