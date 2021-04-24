// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DragMoveUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Automatically drag and drop controls.
    /// </summary>
    /// <remarks>
    /// Borrowed from http://www.thomaslevesque.com/2009/05/06/windows-forms-automatically-drag-and-drop-controls-dragmove/
    /// </remarks>
    public static class DragMoveUtility
    {
        #region Nested classes

        private class DraggedControl : IDisposable
        {
            public Control? Target { get; private set; }
            public int XStart { get; private set; }
            public int YStart { get; private set; }
            public bool IsMoving { get; private set; }
            public bool IsTemporary { get; set; }

            public DraggedControl
                (
                    Control target
                )
            {
                Target = target;
                IsMoving = false;
                IsTemporary = false;
                Target.MouseDown += Target_MouseDown;
                Target.MouseMove += Target_MouseMove;
                Target.MouseUp += Target_MouseUp;
                Target.Disposed += Target_Disposed;
            }

            public DraggedControl
                (
                    Control target,
                    int xStart,
                    int yStart
                )
                : this(target)
            {
                XStart = xStart;
                YStart = yStart;
                IsMoving = (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left;
                IsTemporary = true;
            }

            void Target_MouseDown
                (
                    object? sender, MouseEventArgs e
                )
            {
                if (e.Button == MouseButtons.Left)
                {
                    IsMoving = true;
                    XStart = e.X;
                    YStart = e.Y;
                }
            }

            void Target_MouseUp
                (
                    object? sender,
                    MouseEventArgs e
                )
            {
                IsMoving = false;
                if (IsTemporary)
                {
                    Dispose();
                }
            }

            private void Target_MouseMove
                (
                    object? sender,
                    MouseEventArgs e
                )
            {
                if (IsMoving)
                {
                    if (Target is { } target)
                    {
                        var x = target.Location.X + e.X - XStart;
                        var y = target.Location.Y + e.Y - YStart;
                        target.Location = new Point(x, y);
                    }
                }
            }

            void Target_Disposed
                (
                    object? sender,
                    EventArgs e
                )
            {
                Target?.EnableDragMove(false);
            }

            public void Dispose()
            {
                if (Target is { } target)
                {
                    target.MouseDown -= Target_MouseDown;
                    target.MouseMove -= Target_MouseMove;
                    target.MouseUp -= Target_MouseUp;
                    target.Disposed -= Target_Disposed;
                    Target = null;
                }
            }
        }

        #endregion

        #region Private members

        private static readonly Dictionary<IntPtr, DraggedControl> DraggedControls = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Temporarily enables the DragMove behavior
        /// for the specified control, until the mouse
        /// button is released.
        /// </summary>
        /// <param name="control">The control to drag</param>
        public static void DragMove
            (
                this Control control
            )
        {
            var absolutePosition = Control.MousePosition;
            var relativePosition = control.PointToClient(absolutePosition);

            // ReSharper disable once ObjectCreationAsStatement
            new DraggedControl
                (
                    control,
                    relativePosition.X,
                    relativePosition.Y
                );
        }

        /// <summary>
        /// Enables or disables the DragMove behavior
        /// for the specified control.
        /// </summary>
        /// <param name="control">The control for which the
        /// DragMove behavior must be enabled or disabled
        /// </param>
        /// <param name="value">A value indicating whether
        /// to enable or disable the DragMove behavior
        /// </param>
        public static void EnableDragMove
            (
                this Control control,
                bool value
            )
        {
            if (value)
            {
                if (!DraggedControls.ContainsKey(control.Handle))
                    DraggedControls.Add
                        (
                            control.Handle,
                            new DraggedControl(control)
                        );
            }
            else
            {
                if (DraggedControls.ContainsKey(control.Handle))
                {
                    DraggedControls[control.Handle].Dispose();
                    DraggedControls.Remove(control.Handle);
                }
            }
        }

        /// <summary>
        /// Checks whether the DragMove behavior is enabled
        /// for the specified control.
        /// </summary>
        /// <param name="control">The control to check.</param>
        /// <returns>true if DragMove is enabled for this control;
        ///  otherwise, false</returns>
        public static bool IsDragMoveEnabled
            (
                this Control control
            )
        {
            return DraggedControls.ContainsKey(control.Handle);
        }

        #endregion
    }
}
