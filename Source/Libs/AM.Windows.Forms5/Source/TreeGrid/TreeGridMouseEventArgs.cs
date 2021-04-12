// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridMouseEventArgs.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public class TreeGridMouseEventArgs
        : MouseEventArgs
    {

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeGridMouseEventArgs"/> class.
        /// </summary>
        /// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons"/> values indicating which mouse button was pressed.</param>
        /// <param name="clicks">The number of times a mouse button was pressed.</param>
        /// <param name="x">The x-coordinate of a mouse click, in pixels.</param>
        /// <param name="y"></param>
        /// <param name="delta">A signed count of the number of detents the wheel has rotated.</param>
        public TreeGridMouseEventArgs
            (
                MouseButtons button,
                int clicks,
                int x,
                int y,
                int delta
            )
            : base(button, clicks, x, y, delta)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeGridMouseEventArgs"/> class.
        /// </summary>
        /// <param name="args">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        public TreeGridMouseEventArgs
            (
            MouseEventArgs args
            )
            : base
            (
                args.Button,
                args.Clicks,
                args.X,
                args.Y,
                args.Delta
            )
        {
        }

        #endregion

        #region Properties

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

        #endregion
    }
}
