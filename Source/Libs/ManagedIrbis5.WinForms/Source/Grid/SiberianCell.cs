// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianCell.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianCell
    {
        #region Events

        /// <summary>
        /// Fired on click.
        /// </summary>
        public event EventHandler<SiberianClickEventArgs>? Click;

        /// <summary>
        /// Measure content.
        /// </summary>
        public event EventHandler<SiberianMeasureEventArgs>? Measure;

        /// <summary>
        /// Fired when tooltip needed.
        /// </summary>
        public event EventHandler<SiberianToolTipEventArgs>? ToolTip;

        #endregion

        #region Properties

        /// <summary>
        /// Column.
        /// </summary>
        public SiberianColumn? Column { get; internal set; }

        /// <summary>
        /// Grid.
        /// </summary>
        public SiberianGrid? Grid => Row?.Grid;

        /// <summary>
        /// Row.
        /// </summary>
        public SiberianRow? Row { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SiberianCell()
        {
        }

        #endregion

        #region Private members

        /// <summary>
        /// Handle <see cref="Click"/> event.
        /// </summary>
        protected internal virtual void HandleClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            Click?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Get tooltip for the cell.
        /// </summary>
        protected internal virtual void HandleToolTip
            (
                SiberianToolTipEventArgs eventArgs
            )
        {
            var args = new SiberianToolTipEventArgs();
            ToolTip?.Invoke(this, args);
        }

        /// <summary>
        /// Measure content of the cell.
        /// </summary>
        protected internal virtual void MeasureContent
            (
                SiberianDimensions dimensions
            )
        {
            Measure?.Invoke(this, new SiberianMeasureEventArgs(dimensions));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Close editor.
        /// </summary>
        public virtual void CloseEditor
            (
                bool accept
            )
        {
            var grid = Grid;

            if (grid is {Editor: { }})
            {
                grid.Editor.Dispose();
                grid.Editor = null;

                grid.Invalidate();
            }
        }

        /// <summary>
        /// Handles click on the cell.
        /// </summary>
        public virtual void OnClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            // Nothing to do here?
        }

        /// <summary>
        /// Draw the cell.
        /// </summary>
        public virtual void Paint
            (
                PaintEventArgs args
            )
        {
            // Nothing to do here?
        }

        #endregion
    }
}
