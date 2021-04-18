// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TreeGridMouseEventArgs.cs -- событие мыши в TreeGrid
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Событие мыши в <see cref="TreeGrid"/>.
    /// </summary>
    public class TreeGridMouseEventArgs
        : MouseEventArgs
    {

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
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
        /// Конструктор.
        /// </summary>
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
        /// Грид, в котором произошло событие.
        /// </summary>
        public TreeGrid? TreeGrid { get; set; }

        /// <summary>
        /// Нода, в которой произошло событие (возможно, <c>null</c>).
        /// </summary>
        public TreeGridNode? Node { get; set; }

        /// <summary>
        /// Колонка, в которой произошло событие (возможно, <c>null</c>).
        /// </summary>
        public TreeGridColumn? Column { get; set; }

        #endregion

    } // class TreeGridMouseEventArgs

} // namespace AM.Windows.Forms
