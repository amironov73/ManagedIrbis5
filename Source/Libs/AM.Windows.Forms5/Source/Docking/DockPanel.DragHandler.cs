// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DockPanelDragHandler.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

partial class DockPanel
{
    /// <summary>
    /// DragHandlerBase is the base class for drag handlers. The derived class should:
    ///   1. Define its public method BeginDrag. From within this public BeginDrag method,
    ///      DragHandlerBase.BeginDrag should be called to initialize the mouse capture
    ///      and message filtering.
    ///   2. Override the OnDragging and OnEndDrag methods.
    /// </summary>
    public abstract class DragHandlerBase
        : NativeWindow, IMessageFilter
    {
        /// <summary>
        ///
        /// </summary>
        protected DragHandlerBase()
        {
            // пустое тело конструктора
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract Control? DragControl { get; }

        /// <summary>
        ///
        /// </summary>
        protected Point StartMousePosition { get; private set; } = Point.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected bool BeginDrag()
        {
            if (DragControl == null)
            {
                return false;
            }

            StartMousePosition = Control.MousePosition;

            if (!Win32Helper.IsRunningOnMono)
            {
                if (!Win32.NativeMethods.DragDetect (DragControl.Handle, StartMousePosition))
                {
                    return false;
                }
            }

            DragControl.FindForm()!.Capture = true;
            AssignHandle (DragControl.FindForm()!.Handle);
            if (PatchController.EnableActiveXFix == false)
            {
                Application.AddMessageFilter (this);
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnDragging();

        /// <summary>
        ///
        /// </summary>
        /// <param name="abort"></param>
        protected abstract void OnEndDrag (bool abort);

        private void EndDrag (bool abort)
        {
            ReleaseHandle();

            if (PatchController.EnableActiveXFix == false)
            {
                Application.RemoveMessageFilter (this);
            }

            DragControl!.FindForm()!.Capture = false;

            OnEndDrag (abort);
        }

        bool IMessageFilter.PreFilterMessage (ref Message m)
        {
            if (PatchController.EnableActiveXFix == false)
            {
                if (m.Msg == (int)Win32.Msgs.WM_MOUSEMOVE)
                {
                    OnDragging();
                }
                else if (m.Msg == (int)Win32.Msgs.WM_LBUTTONUP)
                {
                    EndDrag (false);
                }
                else if (m.Msg == (int)Win32.Msgs.WM_CAPTURECHANGED)
                {
                    EndDrag (!Win32Helper.IsRunningOnMono);
                }
                else if (m.Msg == (int)Win32.Msgs.WM_KEYDOWN && (int)m.WParam == (int)Keys.Escape)
                {
                    EndDrag (true);
                }
            }

            return OnPreFilterMessage (ref m);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected virtual bool OnPreFilterMessage
            (
                ref Message m
            )
        {
            if (PatchController.EnableActiveXFix == true)
            {
                switch (m.Msg)
                {
                    case (int) Win32.Msgs.WM_MOUSEMOVE:
                        OnDragging();
                        break;

                    case (int) Win32.Msgs.WM_LBUTTONUP:
                        EndDrag (false);
                        break;

                    case (int) Win32.Msgs.WM_CAPTURECHANGED:
                        EndDrag (!Win32Helper.IsRunningOnMono);
                        break;

                    case (int) Win32.Msgs.WM_KEYDOWN when (int)m.WParam == (int)Keys.Escape:
                        EndDrag (true);
                        break;
                }
            }

            return false;
        }

        /// <inheritdoc cref="NativeWindow.WndProc"/>
        protected sealed override void WndProc
            (
                ref Message m
            )
        {
            if (PatchController.EnableActiveXFix == true)
            {
                //Manually pre-filter message, rather than using
                //Application.AddMessageFilter(this).  This fixes
                //the docker control for ActiveX objects
                this.OnPreFilterMessage (ref m);
            }

            if (m.Msg == (int)Win32.Msgs.WM_CANCELMODE || m.Msg == (int)Win32.Msgs.WM_CAPTURECHANGED)
            {
                EndDrag (true);
            }

            base.WndProc (ref m);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class DragHandler
        : DragHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dockPanel"></param>
        protected DragHandler
            (
                DockPanel dockPanel
            )
        {
            Sure.NotNull (dockPanel);

            DockPanel = dockPanel;
        }

        /// <summary>
        ///
        /// </summary>
        public DockPanel DockPanel { get; }

        /// <summary>
        ///
        /// </summary>
        protected IDragSource? DragSource { get; set; }

        /// <summary>
        ///
        /// </summary>
        protected sealed override Control? DragControl =>
            DragSource?.DragControl;

        /// <inheritdoc cref="DragHandlerBase.OnPreFilterMessage"/>
        protected sealed override bool OnPreFilterMessage
            (
                ref Message m
            )
        {
            if (m.Msg is (int)Win32.Msgs.WM_KEYDOWN or (int)Win32.Msgs.WM_KEYUP &&
                ((int)m.WParam == (int)Keys.ControlKey || (int)m.WParam == (int)Keys.ShiftKey))
            {
                OnDragging();
            }

            return base.OnPreFilterMessage (ref m);
        }
    }
}
