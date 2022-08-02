// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DockPanel.AutoHideWindow.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

partial class DockPanel
{
    /// <summary>
    ///
    /// </summary>
    [ToolboxItem (false)]
    public class AutoHideWindowControl
        : Panel, ISplitterHost
    {
        /// <summary>
        ///
        /// </summary>
        protected class SplitterControl
            : SplitterBase
        {
            /// <summary>
            /// Конструктор.
            /// </summary>
            public SplitterControl (AutoHideWindowControl autoHideWindow)
            {
                m_autoHideWindow = autoHideWindow;
            }

            private AutoHideWindowControl m_autoHideWindow;

            private AutoHideWindowControl AutoHideWindow
            {
                get { return m_autoHideWindow; }
            }

            /// <inheritdoc cref="SplitterBase.SplitterSize"/>
            protected override int SplitterSize
            {
                get { return AutoHideWindow.DockPanel.Theme.Measures.AutoHideSplitterSize; }
            }

            /// <inheritdoc cref="SplitterBase.StartDrag"/>
            protected override void StartDrag()
            {
                AutoHideWindow.DockPanel.BeginDrag (AutoHideWindow, AutoHideWindow.RectangleToScreen (Bounds));
            }
        }

        #region consts

        private const int ANIMATE_TIME = 100; // in mini-seconds

        #endregion

        private Timer m_timerMouseTrack;

        /// <summary>
        ///
        /// </summary>
        protected SplitterBase m_splitter { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AutoHideWindowControl
            (
                DockPanel dockPanel
            )
        {
            DockPanel = dockPanel;

            m_timerMouseTrack = new Timer();
            m_timerMouseTrack.Tick += TimerMouseTrack_Tick;

            Visible = false;
            m_splitter = DockPanel.Theme.Extender.WindowSplitterControlFactory.CreateSplitterControl (this);
            Controls.Add (m_splitter);
        }

        /// <inheritdoc cref="Control.Dispose(bool)"/>
        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                m_timerMouseTrack.Dispose();
            }

            base.Dispose (disposing);
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsDockWindow => false;

        /// <summary>
        ///
        /// </summary>
        public DockPanel? DockPanel { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DockPane? ActivePane { get; private set; }

        private void SetActivePane()
        {
            var value = (ActiveContent == null ? null : ActiveContent.DockHandler.Pane);

            if (value == ActivePane)
            {
                return;
            }

            ActivePane = value;
        }

        private static readonly object AutoHideActiveContentChangedEvent = new object();

        /// <summary>
        ///
        /// </summary>
        public event EventHandler ActiveContentChanged
        {
            add => Events.AddHandler (AutoHideActiveContentChangedEvent, value);
            remove { Events.RemoveHandler (AutoHideActiveContentChangedEvent, value); }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnActiveContentChanged (EventArgs e)
        {
            var handler = (EventHandler?) Events[ActiveContentChangedEvent];
            if (handler != null)
            {
                handler (this, e);
            }
        }

        private IDockContent? m_activeContent;

        /// <summary>
        ///
        /// </summary>
        public IDockContent? ActiveContent
        {
            get => m_activeContent;
            set
            {
                var dockPanel = DockPanel.ThrowIfNull();

                if (value == m_activeContent)
                {
                    return;
                }

                if (value != null)
                {
                    if (!DockHelper.IsDockStateAutoHide (value.DockHandler.DockState) ||
                        value.DockHandler.DockPanel != DockPanel)
                    {
                        throw (new InvalidOperationException (Strings
                            .DockPanel_ActiveAutoHideContent_InvalidValue));
                    }
                }

                dockPanel.SuspendLayout();

                if (m_activeContent != null)
                {
                    if (m_activeContent.DockHandler.Form.ContainsFocus)
                    {
                        if (!Win32Helper.IsRunningOnMono)
                        {
                            dockPanel.ContentFocusManager.GiveUpFocus (m_activeContent);
                        }
                    }

                    AnimateWindow (false);
                }

                m_activeContent = value;
                SetActivePane();
                if (ActivePane != null)
                {
                    ActivePane.ActiveContent = m_activeContent;
                }

                if (m_activeContent != null)
                {
                    AnimateWindow (true);
                }

                dockPanel.ResumeLayout();
                dockPanel.RefreshAutoHideStrip();

                SetTimerMouseTrack();

                OnActiveContentChanged (EventArgs.Empty);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public DockState DockState
        {
            get { return ActiveContent == null ? DockState.Unknown : ActiveContent.DockHandler.DockState; }
        }

        private bool FlagAnimate { get; set; } = true;

        private bool m_flagDragging;

        internal bool FlagDragging
        {
            get => m_flagDragging;
            set
            {
                if (m_flagDragging == value)
                {
                    return;
                }

                m_flagDragging = value;
                SetTimerMouseTrack();
            }
        }

        private void AnimateWindow (bool show)
        {
            if (!FlagAnimate && Visible != show)
            {
                Visible = show;
                return;
            }

            Parent.SuspendLayout();

            var rectSource = GetRectangle (!show);
            var rectTarget = GetRectangle (show);
            int dxLoc, dyLoc;
            int dWidth, dHeight;
            dxLoc = dyLoc = dWidth = dHeight = 0;
            if (DockState == DockState.DockTopAutoHide)
            {
                dHeight = show ? 1 : -1;
            }
            else if (DockState == DockState.DockLeftAutoHide)
            {
                dWidth = show ? 1 : -1;
            }
            else if (DockState == DockState.DockRightAutoHide)
            {
                dxLoc = show ? -1 : 1;
                dWidth = show ? 1 : -1;
            }
            else if (DockState == DockState.DockBottomAutoHide)
            {
                dyLoc = (show ? -1 : 1);
                dHeight = (show ? 1 : -1);
            }

            if (show)
            {
                Bounds = DockPanel.GetAutoHideWindowBounds (new Rectangle (-rectTarget.Width, -rectTarget.Height,
                    rectTarget.Width, rectTarget.Height));
                if (Visible == false)
                {
                    Visible = true;
                }

                PerformLayout();
            }

            SuspendLayout();

            LayoutAnimateWindow (rectSource);
            if (Visible == false)
            {
                Visible = true;
            }

            var speedFactor = 1;
            var totalPixels = (rectSource.Width != rectTarget.Width)
                ? Math.Abs (rectSource.Width - rectTarget.Width)
                : Math.Abs (rectSource.Height - rectTarget.Height);
            var remainPixels = totalPixels;
            var startingTime = DateTime.Now;
            while (rectSource != rectTarget)
            {
                var startPerMove = DateTime.Now;

                rectSource.X += dxLoc * speedFactor;
                rectSource.Y += dyLoc * speedFactor;
                rectSource.Width += dWidth * speedFactor;
                rectSource.Height += dHeight * speedFactor;
                if (Math.Sign (rectTarget.X - rectSource.X) != Math.Sign (dxLoc))
                {
                    rectSource.X = rectTarget.X;
                }

                if (Math.Sign (rectTarget.Y - rectSource.Y) != Math.Sign (dyLoc))
                {
                    rectSource.Y = rectTarget.Y;
                }

                if (Math.Sign (rectTarget.Width - rectSource.Width) != Math.Sign (dWidth))
                {
                    rectSource.Width = rectTarget.Width;
                }

                if (Math.Sign (rectTarget.Height - rectSource.Height) != Math.Sign (dHeight))
                {
                    rectSource.Height = rectTarget.Height;
                }

                LayoutAnimateWindow (rectSource);
                if (Parent != null)
                {
                    Parent.Update();
                }

                remainPixels -= speedFactor;

                while (true)
                {
                    var time = new TimeSpan (0, 0, 0, 0, ANIMATE_TIME);
                    var elapsedPerMove = DateTime.Now - startPerMove;
                    var elapsedTime = DateTime.Now - startingTime;
                    if (((int)((time - elapsedTime).TotalMilliseconds)) <= 0)
                    {
                        speedFactor = remainPixels;
                        break;
                    }
                    else
                    {
                        speedFactor = remainPixels * (int)elapsedPerMove.TotalMilliseconds /
                                      (int)((time - elapsedTime).TotalMilliseconds);
                    }

                    if (speedFactor >= 1)
                    {
                        break;
                    }
                }
            }

            ResumeLayout();
            Parent.ResumeLayout();
        }

        private void LayoutAnimateWindow (Rectangle rect)
        {
            Bounds = DockPanel.GetAutoHideWindowBounds (rect);

            var rectClient = ClientRectangle;

            if (DockState == DockState.DockLeftAutoHide)
            {
                ActivePane.Location =
                    new Point (
                        rectClient.Right - 2 - DockPanel.Theme.Measures.AutoHideSplitterSize - ActivePane.Width,
                        ActivePane.Location.Y);
            }
            else if (DockState == DockState.DockTopAutoHide)
            {
                ActivePane.Location = new Point (ActivePane.Location.X,
                    rectClient.Bottom - 2 - DockPanel.Theme.Measures.AutoHideSplitterSize - ActivePane.Height);
            }
        }

        private Rectangle GetRectangle (bool show)
        {
            if (DockState == DockState.Unknown)
            {
                return Rectangle.Empty;
            }

            var rect = DockPanel.AutoHideWindowRectangle;

            if (show)
            {
                return rect;
            }

            if (DockState == DockState.DockLeftAutoHide)
            {
                rect.Width = 0;
            }
            else if (DockState == DockState.DockRightAutoHide)
            {
                rect.X += rect.Width;
                rect.Width = 0;
            }
            else if (DockState == DockState.DockTopAutoHide)
            {
                rect.Height = 0;
            }
            else
            {
                rect.Y += rect.Height;
                rect.Height = 0;
            }

            return rect;
        }

        private void SetTimerMouseTrack()
        {
            if (ActivePane == null || ActivePane.IsActivated || FlagDragging)
            {
                m_timerMouseTrack.Enabled = false;
                return;
            }

            // start the timer
            var hovertime = SystemInformation.MouseHoverTime;

            // assign a default value 400 in case of setting Timer.Interval invalid value exception
            if (hovertime <= 0)
            {
                hovertime = 400;
            }

            m_timerMouseTrack.Interval = 2 * (int)hovertime;
            m_timerMouseTrack.Enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual Rectangle DisplayingRectangle
        {
            get
            {
                var rect = ClientRectangle;
                var dockPanel = DockPanel.ThrowIfNull();

                switch (DockState)
                {
                    // exclude the border and the splitter
                    case DockState.DockBottomAutoHide:
                        rect.Y += 2 + dockPanel.Theme.Measures.AutoHideSplitterSize;
                        rect.Height -= 2 + dockPanel.Theme.Measures.AutoHideSplitterSize;
                        break;

                    case DockState.DockRightAutoHide:
                        rect.X += 2 + dockPanel.Theme.Measures.AutoHideSplitterSize;
                        rect.Width -= 2 + dockPanel.Theme.Measures.AutoHideSplitterSize;
                        break;

                    case DockState.DockTopAutoHide:
                        rect.Height -= 2 + dockPanel.Theme.Measures.AutoHideSplitterSize;
                        break;

                    case DockState.DockLeftAutoHide:
                        rect.Width -= 2 + dockPanel.Theme.Measures.AutoHideSplitterSize;
                        break;
                }

                return rect;
            }
        }

        public void RefreshActiveContent()
        {
            if (ActiveContent == null)
            {
                return;
            }

            if (!DockHelper.IsDockStateAutoHide (ActiveContent.DockHandler.DockState))
            {
                FlagAnimate = false;
                ActiveContent = null;
                FlagAnimate = true;
            }
        }

        public void RefreshActivePane()
        {
            SetTimerMouseTrack();
        }

        private void TimerMouseTrack_Tick (object? sender, EventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            if (ActivePane == null || ActivePane.IsActivated)
            {
                m_timerMouseTrack.Enabled = false;
                return;
            }

            var pane = ActivePane;
            var ptMouseInAutoHideWindow = PointToClient (Control.MousePosition);
            var ptMouseInDockPanel = DockPanel.PointToClient (Control.MousePosition);

            var rectTabStrip = DockPanel.GetTabStripRectangle (pane.DockState);

            if (!ClientRectangle.Contains (ptMouseInAutoHideWindow) && !rectTabStrip.Contains (ptMouseInDockPanel))
            {
                ActiveContent = null;
                m_timerMouseTrack.Enabled = false;
            }
        }

        #region ISplitterDragSource Members

        void ISplitterDragSource.BeginDrag (Rectangle rectSplitter)
        {
            FlagDragging = true;
        }

        void ISplitterDragSource.EndDrag()
        {
            FlagDragging = false;
        }

        bool ISplitterDragSource.IsVertical
        {
            get { return (DockState == DockState.DockLeftAutoHide || DockState == DockState.DockRightAutoHide); }
        }

        Rectangle ISplitterDragSource.DragLimitBounds
        {
            get
            {
                var rectLimit = DockPanel.DockArea;

                if ((this as ISplitterDragSource).IsVertical)
                {
                    rectLimit.X += MeasurePane.MinSize;
                    rectLimit.Width -= 2 * MeasurePane.MinSize;
                }
                else
                {
                    rectLimit.Y += MeasurePane.MinSize;
                    rectLimit.Height -= 2 * MeasurePane.MinSize;
                }

                return DockPanel.RectangleToScreen (rectLimit);
            }
        }

        void ISplitterDragSource.MoveSplitter (int offset)
        {
            var rectDockArea = DockPanel.DockArea;
            var content = ActiveContent;
            if (DockState == DockState.DockLeftAutoHide && rectDockArea.Width > 0)
            {
                if (content.DockHandler.AutoHidePortion < 1)
                {
                    content.DockHandler.AutoHidePortion += ((double)offset) / (double)rectDockArea.Width;
                }
                else
                {
                    content.DockHandler.AutoHidePortion = Width + offset;
                }
            }
            else if (DockState == DockState.DockRightAutoHide && rectDockArea.Width > 0)
            {
                if (content.DockHandler.AutoHidePortion < 1)
                {
                    content.DockHandler.AutoHidePortion -= ((double)offset) / (double)rectDockArea.Width;
                }
                else
                {
                    content.DockHandler.AutoHidePortion = Width - offset;
                }
            }
            else if (DockState == DockState.DockBottomAutoHide && rectDockArea.Height > 0)
            {
                if (content.DockHandler.AutoHidePortion < 1)
                {
                    content.DockHandler.AutoHidePortion -= ((double)offset) / (double)rectDockArea.Height;
                }
                else
                {
                    content.DockHandler.AutoHidePortion = Height - offset;
                }
            }
            else if (DockState == DockState.DockTopAutoHide && rectDockArea.Height > 0)
            {
                if (content.DockHandler.AutoHidePortion < 1)
                {
                    content.DockHandler.AutoHidePortion += ((double)offset) / (double)rectDockArea.Height;
                }
                else
                {
                    content.DockHandler.AutoHidePortion = Height + offset;
                }
            }
        }

        #region IDragSource Members

        Control IDragSource.DragControl
        {
            get { return this; }
        }

        #endregion

        #endregion
    }

    private AutoHideWindowControl AutoHideWindow
    {
        get { return m_autoHideWindow; }
    }

    internal Control AutoHideControl
    {
        get { return m_autoHideWindow; }
    }

    internal void RefreshActiveAutoHideContent()
    {
        AutoHideWindow.RefreshActiveContent();
    }

    internal Rectangle AutoHideWindowRectangle
    {
        get
        {
            var state = AutoHideWindow.DockState;
            var rectDockArea = DockArea;
            if (ActiveAutoHideContent == null)
            {
                return Rectangle.Empty;
            }

            if (Parent == null)
            {
                return Rectangle.Empty;
            }

            var rect = Rectangle.Empty;
            var autoHideSize = ActiveAutoHideContent.DockHandler.AutoHidePortion;
            if (state == DockState.DockLeftAutoHide)
            {
                if (autoHideSize < 1)
                {
                    autoHideSize = rectDockArea.Width * autoHideSize;
                }

                if (autoHideSize > rectDockArea.Width - MeasurePane.MinSize)
                {
                    autoHideSize = rectDockArea.Width - MeasurePane.MinSize;
                }

                rect.X = rectDockArea.X - Theme.Measures.DockPadding;
                rect.Y = rectDockArea.Y;
                rect.Width = (int)autoHideSize;
                rect.Height = rectDockArea.Height;
            }
            else if (state == DockState.DockRightAutoHide)
            {
                if (autoHideSize < 1)
                {
                    autoHideSize = rectDockArea.Width * autoHideSize;
                }

                if (autoHideSize > rectDockArea.Width - MeasurePane.MinSize)
                {
                    autoHideSize = rectDockArea.Width - MeasurePane.MinSize;
                }

                rect.X = rectDockArea.X + rectDockArea.Width - (int)autoHideSize + Theme.Measures.DockPadding;
                rect.Y = rectDockArea.Y;
                rect.Width = (int)autoHideSize;
                rect.Height = rectDockArea.Height;
            }
            else if (state == DockState.DockTopAutoHide)
            {
                if (autoHideSize < 1)
                {
                    autoHideSize = rectDockArea.Height * autoHideSize;
                }

                if (autoHideSize > rectDockArea.Height - MeasurePane.MinSize)
                {
                    autoHideSize = rectDockArea.Height - MeasurePane.MinSize;
                }

                rect.X = rectDockArea.X;
                rect.Y = rectDockArea.Y - Theme.Measures.DockPadding;
                rect.Width = rectDockArea.Width;
                rect.Height = (int)autoHideSize;
            }
            else if (state == DockState.DockBottomAutoHide)
            {
                if (autoHideSize < 1)
                {
                    autoHideSize = rectDockArea.Height * autoHideSize;
                }

                if (autoHideSize > rectDockArea.Height - MeasurePane.MinSize)
                {
                    autoHideSize = rectDockArea.Height - MeasurePane.MinSize;
                }

                rect.X = rectDockArea.X;
                rect.Y = rectDockArea.Y + rectDockArea.Height - (int)autoHideSize + Theme.Measures.DockPadding;
                rect.Width = rectDockArea.Width;
                rect.Height = (int)autoHideSize;
            }

            return rect;
        }
    }

    internal Rectangle GetAutoHideWindowBounds (Rectangle rectAutoHideWindow)
    {
        if (DocumentStyle == DocumentStyle.SystemMdi ||
            DocumentStyle == DocumentStyle.DockingMdi)
        {
            return (Parent == null)
                ? Rectangle.Empty
                : Parent.RectangleToClient (RectangleToScreen (rectAutoHideWindow));
        }
        else
        {
            return rectAutoHideWindow;
        }
    }

    internal void RefreshAutoHideStrip()
    {
        AutoHideStripControl.RefreshChanges();
    }
}
