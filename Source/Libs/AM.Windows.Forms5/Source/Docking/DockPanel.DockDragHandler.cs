// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable UnusedMember.Global

/* DockPanel.DockDragHandler.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

partial class DockPanel
{
    #region IHitTest

    /// <summary>
    ///
    /// </summary>
    public interface IHitTest
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        DockStyle HitTest (Point pt);

        /// <summary>
        ///
        /// </summary>
        DockStyle Status { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IPaneIndicator
        : IHitTest
    {
        /// <summary>
        ///
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        ///
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        ///
        /// </summary>
        int Left { get; }

        /// <summary>
        ///
        /// </summary>
        int Top { get; }

        /// <summary>
        ///
        /// </summary>
        int Right { get; }

        /// <summary>
        ///
        /// </summary>
        int Bottom { get; }

        /// <summary>
        ///
        /// </summary>
        Rectangle ClientRectangle { get; }

        /// <summary>
        ///
        /// </summary>
        int Width { get; }

        /// <summary>
        ///
        /// </summary>
        int Height { get; }

        /// <summary>
        ///
        /// </summary>
        GraphicsPath DisplayingGraphicsPath { get; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IPanelIndicator
        : IHitTest
    {
        /// <summary>
        ///
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        ///
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        ///
        /// </summary>
        Rectangle Bounds { get; }

        /// <summary>
        ///
        /// </summary>
        int Width { get; }

        /// <summary>
        ///
        /// </summary>
        int Height { get; }
    }

    /// <summary>
    ///
    /// </summary>
    public struct HotSpotIndex
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dockStyle"></param>
        public HotSpotIndex (int x, int y, DockStyle dockStyle)
        {
            X = x;
            Y = y;
            DockStyle = dockStyle;
        }

        /// <summary>
        ///
        /// </summary>
        public int X { get; }

        /// <summary>
        ///
        /// </summary>
        public int Y { get; }

        /// <summary>
        ///
        /// </summary>
        public DockStyle DockStyle { get; }
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    public sealed class DockDragHandler
        : DragHandler
    {
        /// <summary>
        ///
        /// </summary>
        public class DockIndicator
            : DragForm
        {
            #region consts

            private int _panelIndicatorMargin = 10;

            #endregion

            private DockDragHandler _dragHandler;

            /// <summary>
            ///
            /// </summary>
            /// <param name="dragHandler"></param>
            public DockIndicator
                (
                    DockDragHandler dragHandler
                )
            {
                _dockPane = null!;
                _hitTest = null!;

                _dragHandler = dragHandler;
                Controls.AddRange (new[]
                {
                    (Control) PaneDiamond,
                    (Control) PanelLeft,
                    (Control) PanelRight,
                    (Control) PanelTop,
                    (Control) PanelBottom,
                    (Control) PanelFill
                });
                Region = new Region (Rectangle.Empty);
            }

            private IPaneIndicator? _paneDiamond;

            private IPaneIndicator PaneDiamond =>
                _paneDiamond ??= _dragHandler.DockPanel.Theme.Extender.PaneIndicatorFactory.CreatePaneIndicator (
                    _dragHandler.DockPanel.Theme);

            private IPanelIndicator? _panelLeft;

            private IPanelIndicator PanelLeft =>
                _panelLeft ??= _dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator (
                    DockStyle.Left, _dragHandler.DockPanel.Theme);

            private IPanelIndicator? _panelRight;

            private IPanelIndicator PanelRight =>
                _panelRight ??= _dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator (
                    DockStyle.Right, _dragHandler.DockPanel.Theme);

            private IPanelIndicator? _panelTop;

            private IPanelIndicator PanelTop =>
                _panelTop ??= _dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator (
                    DockStyle.Top, _dragHandler.DockPanel.Theme);

            private IPanelIndicator? _panelBottom;

            private IPanelIndicator PanelBottom =>
                _panelBottom ??= _dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator (
                    DockStyle.Bottom, _dragHandler.DockPanel.Theme);

            private IPanelIndicator? _panelFill;

            private IPanelIndicator PanelFill =>
                _panelFill ??= _dragHandler.DockPanel.Theme.Extender.PanelIndicatorFactory.CreatePanelIndicator (
                    DockStyle.Fill, _dragHandler.DockPanel.Theme);

            private bool _fullPanelEdge;

            /// <summary>
            ///
            /// </summary>
            public bool FullPanelEdge
            {
                get => _fullPanelEdge;
                set
                {
                    if (_fullPanelEdge == value)
                    {
                        return;
                    }

                    _fullPanelEdge = value;
                    RefreshChanges();
                }
            }

            /// <summary>
            ///
            /// </summary>
            public DockDragHandler DragHandler => _dragHandler;

            /// <summary>
            ///
            /// </summary>
            public DockPanel DockPanel => DragHandler.DockPanel;

            private DockPane _dockPane;

            /// <summary>
            ///
            /// </summary>
            public DockPane DockPane
            {
                get => _dockPane;
                internal set
                {
                    if (_dockPane == value)
                    {
                        return;
                    }

                    var oldDisplayingPane = DisplayingPane;
                    _dockPane = value;
                    if (oldDisplayingPane != DisplayingPane)
                    {
                        RefreshChanges();
                    }
                }
            }

            private IHitTest _hitTest;

            private IHitTest HitTestResult
            {
                get => _hitTest;
                set
                {
                    if (_hitTest == value)
                    {
                        return;
                    }

                    if (_hitTest != null!)
                    {
                        _hitTest.Status = DockStyle.None;
                    }

                    _hitTest = value;
                }
            }

            private DockPane? DisplayingPane => ShouldPaneDiamondVisible() ? DockPane : null;

            private void RefreshChanges()
            {
                if (PatchController.EnablePerScreenDpi == true)
                {
                    //SHCore.PROCESS_DPI_AWARENESS value;
                    //if (SHCore.GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out value) == HRESULT.S_OK)
                    //{
                    //    if (value == SHCore.PROCESS_DPI_AWARENESS.PROCESS_SYSTEM_DPI_AWARE)
                    //    {
                    var allScreens = Screen.AllScreens;
                    var mousePos = Control.MousePosition;
                    foreach (var screen in allScreens)
                    {
                        if (screen.Bounds.Contains (mousePos))
                        {
                            Bounds = screen.Bounds;
                        }
                    }

                    //    }
                    //}
                }

                var region = new Region (Rectangle.Empty);
                var rectDockArea = FullPanelEdge ? DockPanel.DockArea : DockPanel.DocumentWindowBounds;

                rectDockArea = RectangleToClient (DockPanel.RectangleToScreen (rectDockArea));
                if (ShouldPanelIndicatorVisible (DockState.DockLeft))
                {
                    PanelLeft.Location = new Point (rectDockArea.X + _panelIndicatorMargin,
                        rectDockArea.Y + (rectDockArea.Height - PanelRight.Height) / 2);
                    PanelLeft.Visible = true;
                    region.Union (PanelLeft.Bounds);
                }
                else
                {
                    PanelLeft.Visible = false;
                }

                if (ShouldPanelIndicatorVisible (DockState.DockRight))
                {
                    PanelRight.Location =
                        new Point (rectDockArea.X + rectDockArea.Width - PanelRight.Width - _panelIndicatorMargin,
                            rectDockArea.Y + (rectDockArea.Height - PanelRight.Height) / 2);
                    PanelRight.Visible = true;
                    region.Union (PanelRight.Bounds);
                }
                else
                {
                    PanelRight.Visible = false;
                }

                if (ShouldPanelIndicatorVisible (DockState.DockTop))
                {
                    PanelTop.Location = new Point (rectDockArea.X + (rectDockArea.Width - PanelTop.Width) / 2,
                        rectDockArea.Y + _panelIndicatorMargin);
                    PanelTop.Visible = true;
                    region.Union (PanelTop.Bounds);
                }
                else
                {
                    PanelTop.Visible = false;
                }

                if (ShouldPanelIndicatorVisible (DockState.DockBottom))
                {
                    PanelBottom.Location = new Point (rectDockArea.X + (rectDockArea.Width - PanelBottom.Width) / 2,
                        rectDockArea.Y + rectDockArea.Height - PanelBottom.Height - _panelIndicatorMargin);
                    PanelBottom.Visible = true;
                    region.Union (PanelBottom.Bounds);
                }
                else
                {
                    PanelBottom.Visible = false;
                }

                if (ShouldPanelIndicatorVisible (DockState.Document))
                {
                    var rectDocumentWindow =
                        RectangleToClient (DockPanel.RectangleToScreen (DockPanel.DocumentWindowBounds));
                    PanelFill.Location =
                        new Point (rectDocumentWindow.X + (rectDocumentWindow.Width - PanelFill.Width) / 2,
                            rectDocumentWindow.Y + (rectDocumentWindow.Height - PanelFill.Height) / 2);
                    PanelFill.Visible = true;
                    region.Union (PanelFill.Bounds);
                }
                else
                {
                    PanelFill.Visible = false;
                }

                if (ShouldPaneDiamondVisible())
                {
                    var rect = RectangleToClient (DockPane.RectangleToScreen (DockPane.ClientRectangle));
                    PaneDiamond.Location = new Point (rect.Left + (rect.Width - PaneDiamond.Width) / 2,
                        rect.Top + (rect.Height - PaneDiamond.Height) / 2);
                    PaneDiamond.Visible = true;
                    using (var graphicsPath = PaneDiamond.DisplayingGraphicsPath.Clone() as GraphicsPath)
                    {
                        Point[] pts =
                        {
                            new Point (PaneDiamond.Left, PaneDiamond.Top),
                            new Point (PaneDiamond.Right, PaneDiamond.Top),
                            new Point (PaneDiamond.Left, PaneDiamond.Bottom)
                        };
                        using (var matrix = new Matrix (PaneDiamond.ClientRectangle, pts))
                        {
                            graphicsPath!.Transform (matrix);
                        }

                        region.Union (graphicsPath);
                    }
                }
                else
                {
                    PaneDiamond.Visible = false;
                }

                Region = region;
            }

            private bool ShouldPanelIndicatorVisible (DockState dockState)
            {
                if (!Visible)
                {
                    return false;
                }

                if (DockPanel.DockWindows[dockState].Visible)
                {
                    return false;
                }

                return DragHandler.DragSource.IsDockStateValid (dockState);
            }

            private bool ShouldPaneDiamondVisible()
            {
                if (DockPane == null!)
                {
                    return false;
                }

                if (!DockPanel.AllowEndUserNestedDocking)
                {
                    return false;
                }

                return DragHandler.DragSource.CanDockTo (DockPane);
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="bActivate"></param>
            public override void Show (bool bActivate)
            {
                base.Show (bActivate);
                if (PatchController.EnablePerScreenDpi != true)
                {
                    Bounds = SystemInformation.VirtualScreen;
                }

                RefreshChanges();
            }

            public void TestDrop()
            {
                var pt = Control.MousePosition;
                DockPane = DockHelper.PaneAtPoint (pt, DockPanel)!;

                if (TestDrop (PanelLeft, pt) != DockStyle.None)
                {
                    HitTestResult = PanelLeft;
                }
                else if (TestDrop (PanelRight, pt) != DockStyle.None)
                {
                    HitTestResult = PanelRight;
                }
                else if (TestDrop (PanelTop, pt) != DockStyle.None)
                {
                    HitTestResult = PanelTop;
                }
                else if (TestDrop (PanelBottom, pt) != DockStyle.None)
                {
                    HitTestResult = PanelBottom;
                }
                else if (TestDrop (PanelFill, pt) != DockStyle.None)
                {
                    HitTestResult = PanelFill;
                }
                else if (TestDrop (PaneDiamond, pt) != DockStyle.None)
                {
                    HitTestResult = PaneDiamond;
                }
                else
                {
                    HitTestResult = null!;
                }

                if (HitTestResult != null!)
                {
                    if (HitTestResult is IPaneIndicator)
                    {
                        DragHandler.Outline.Show (DockPane, HitTestResult.Status);
                    }
                    else
                    {
                        DragHandler.Outline.Show (DockPanel, HitTestResult.Status, FullPanelEdge);
                    }
                }
            }

            private static DockStyle TestDrop (IHitTest hitTest, Point pt)
            {
                return hitTest.Status = hitTest.HitTest (pt);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        public DockDragHandler (DockPanel panel)
            : base (panel)
        {
            Outline = null!;
            Indicator = null!;
        }

        /// <summary>
        ///
        /// </summary>
        public new IDockDragSource DragSource
        {
            get => (base.DragSource as IDockDragSource)!;
            set => base.DragSource = value;
        }

        /// <summary>
        ///
        /// </summary>
        public DockOutlineBase Outline { get; private set; }

        private DockIndicator Indicator { get; set; }

        private Rectangle FloatOutlineBounds { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dragSource"></param>
        public void BeginDrag (IDockDragSource dragSource)
        {
            DragSource = dragSource;

            if (!BeginDrag())
            {
                DragSource = null!;
                return;
            }

            Outline = DockPanel.Theme.Extender.DockOutlineFactory.CreateDockOutline();
            Indicator = DockPanel.Theme.Extender.DockIndicatorFactory.CreateDockIndicator (this);
            Indicator.Show (false);

            FloatOutlineBounds = DragSource.BeginDrag (StartMousePosition);
        }

        /// <inheritdoc cref="DragHandlerBase.OnDragging"/>
        protected override void OnDragging()
        {
            TestDrop();
        }

        /// <inheritdoc cref="DragHandlerBase.OnEndDrag"/>
        protected override void OnEndDrag (bool abort)
        {
            DockPanel.SuspendLayout (true);

            Outline.Close();
            Indicator.Close();

            EndDrag (abort);

            // Queue a request to layout all children controls
            DockPanel.PerformMdiClientLayout();

            DockPanel.ResumeLayout (true, true);

            DragSource.EndDrag();

            DragSource = null!;

            // Fire notification
            DockPanel.OnDocumentDragged();
        }

        private void TestDrop()
        {
            Outline.FlagTestDrop = false;

            Indicator.FullPanelEdge = ((Control.ModifierKeys & Keys.Shift) != 0);

            if ((Control.ModifierKeys & Keys.Control) == 0)
            {
                Indicator.TestDrop();

                if (!Outline.FlagTestDrop)
                {
                    var pane = DockHelper.PaneAtPoint (Control.MousePosition, DockPanel);
                    if (pane != null && DragSource.IsDockStateValid (pane.DockState))
                    {
                        pane.TestDrop (DragSource, Outline);
                    }
                }

                if (!Outline.FlagTestDrop && DragSource.IsDockStateValid (DockState.Float))
                {
                    var floatWindow = DockHelper.FloatWindowAtPoint (Control.MousePosition, DockPanel);
                    if (floatWindow != null)
                    {
                        floatWindow.TestDrop (DragSource, Outline);
                    }
                }
            }
            else
            {
                Indicator.DockPane = DockHelper.PaneAtPoint (Control.MousePosition, DockPanel)!;
            }

            if (!Outline.FlagTestDrop)
            {
                if (DragSource.IsDockStateValid (DockState.Float))
                {
                    var rect = FloatOutlineBounds;
                    rect.Offset (Control.MousePosition.X - StartMousePosition.X,
                        Control.MousePosition.Y - StartMousePosition.Y);
                    Outline.Show (rect);
                }
            }

            if (!Outline.FlagTestDrop)
            {
                Cursor.Current = Cursors.No;
                Outline.Show();
            }
            else
            {
                Cursor.Current = DragControl.Cursor;
            }
        }

        private void EndDrag (bool abort)
        {
            if (abort)
            {
                return;
            }

            if (!Outline.FloatWindowBounds.IsEmpty)
            {
                DragSource.FloatAt (Outline.FloatWindowBounds);
            }
            else switch (Outline.DockTo)
            {
                case DockPane pane:
                    DragSource.DockTo (pane, Outline.Dock, Outline.ContentIndex);
                    break;

                case DockPanel panel:
                    panel.UpdateDockWindowZOrder (Outline.Dock, Outline.FlagFullEdge);
                    DragSource.DockTo (panel, Outline.Dock);
                    break;
            }
        }
    }

    private DockDragHandler? _dockDragHandler;

    private DockDragHandler GetDockDragHandler()
    {
        return _dockDragHandler ??= new DockDragHandler (this);
    }

    internal void BeginDrag (IDockDragSource dragSource)
    {
        GetDockDragHandler().BeginDrag (dragSource);
    }
}
