// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AutoHideStripBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public abstract class AutoHideStripBase
    : Control
{
    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    protected class Tab : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        protected internal Tab (IDockContent content)
        {
            Content = content;
        }

        ~Tab()
        {
            Dispose (false);
        }

        /// <summary>
        ///
        /// </summary>
        public IDockContent Content { get; }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose (bool disposing)
        {
            // пустое тело метода
        }
    }

    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    protected sealed class TabCollection
        : IEnumerable<Tab>
    {
        #region IEnumerable Members

        IEnumerator<Tab> IEnumerable<Tab>.GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        #endregion

        internal TabCollection (DockPane pane)
        {
            DockPane = pane;
        }

        /// <summary>
        ///
        /// </summary>
        public DockPane DockPane { get; }

        /// <summary>
        ///
        /// </summary>
        public DockPanel DockPanel => DockPane.DockPanel;

        /// <summary>
        ///
        /// </summary>
        public int Count => DockPane.DisplayingContents.Count;

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Tab this [int index]
        {
            get
            {
                var content = DockPane.DisplayingContents[index];
                if (content == null)
                {
                    throw new ArgumentOutOfRangeException (nameof (index));
                }

                content.DockHandler.AutoHideTab ??= (DockPanel.AutoHideStripControl.CreateTab (content));

                return (content.DockHandler.AutoHideTab as Tab)!;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public bool Contains (Tab tab)
        {
            return (IndexOf (tab) != -1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool Contains (IDockContent content)
        {
            return (IndexOf (content) != -1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public int IndexOf (Tab? tab)
        {
            if (tab == null)
            {
                return -1;
            }

            return IndexOf (tab.Content);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public int IndexOf (IDockContent content)
        {
            return DockPane.DisplayingContents.IndexOf (content);
        }
    }

    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    protected class Pane
        : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dockPane"></param>
        protected internal Pane (DockPane dockPane)
        {
            DockPane = dockPane;
        }

        ~Pane()
        {
            Dispose (false);
        }

        /// <summary>
        ///
        /// </summary>
        public DockPane DockPane { get; }

        /// <summary>
        ///
        /// </summary>
        public TabCollection AutoHideTabs
        {
            get
            {
                DockPane.AutoHideTabs ??= new TabCollection (DockPane);

                return (DockPane.AutoHideTabs as TabCollection)!;
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose (bool disposing)
        {
            // пустое тело метода
        }
    }

    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    protected sealed class PaneCollection
        : IEnumerable<Pane>
    {
        private class AutoHideState
        {
            public AutoHideState (DockState dockState)
            {
                this.DockState = dockState;
            }

            public DockState DockState { get; }

            public bool Selected { get; set; }
        }

        private class AutoHideStateCollection
        {
            private readonly AutoHideState[] _states;

            public AutoHideStateCollection()
            {
                _states = new[]
                {
                    new AutoHideState (DockState.DockTopAutoHide),
                    new AutoHideState (DockState.DockBottomAutoHide),
                    new AutoHideState (DockState.DockLeftAutoHide),
                    new AutoHideState (DockState.DockRightAutoHide)
                };
            }

            public AutoHideState this [DockState dockState]
            {
                get
                {
                    for (var i = 0; i < _states.Length; i++)
                    {
                        if (_states[i].DockState == dockState)
                        {
                            return _states[i];
                        }
                    }

                    throw new ArgumentOutOfRangeException (nameof (dockState));
                }
            }

            public bool ContainsPane (DockPane pane)
            {
                if (pane.IsHidden)
                {
                    return false;
                }

                for (var i = 0; i < _states.Length; i++)
                {
                    if (_states[i].DockState == pane.DockState && _states[i].Selected)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        internal PaneCollection (DockPanel panel, DockState dockState)
        {
            DockPanel = panel;
            States = new AutoHideStateCollection
            {
                [DockState.DockTopAutoHide] =
                {
                    Selected = (dockState == DockState.DockTopAutoHide)
                },
                [DockState.DockBottomAutoHide] =
                {
                    Selected = (dockState == DockState.DockBottomAutoHide)
                },
                [DockState.DockLeftAutoHide] =
                {
                    Selected = (dockState == DockState.DockLeftAutoHide)
                },
                [DockState.DockRightAutoHide] =
                {
                    Selected = (dockState == DockState.DockRightAutoHide)
                }
            };
        }

        /// <summary>
        ///
        /// </summary>
        public DockPanel DockPanel { get; }

        private AutoHideStateCollection States { get; }

        /// <summary>
        ///
        /// </summary>
        public int Count
        {
            get
            {
                var count = 0;
                foreach (var pane in DockPanel.Panes)
                {
                    if (States.ContainsPane (pane))
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Pane this [int index]
        {
            get
            {
                var count = 0;
                foreach (var pane in DockPanel.Panes)
                {
                    if (!States.ContainsPane (pane))
                    {
                        continue;
                    }

                    if (count == index)
                    {
                        pane.AutoHidePane ??= DockPanel.AutoHideStripControl.CreatePane (pane);

                        return (pane.AutoHidePane as Pane)!;
                    }

                    count++;
                }

                throw new ArgumentOutOfRangeException (nameof (index));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pane"></param>
        /// <returns></returns>
        public bool Contains (Pane pane)
        {
            return (IndexOf (pane) != -1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pane"></param>
        /// <returns></returns>
        public int IndexOf (Pane? pane)
        {
            if (pane == null)
            {
                return -1;
            }

            var index = 0;
            foreach (var dockPane in DockPanel.Panes)
            {
                if (!States.ContainsPane (pane.DockPane))
                {
                    continue;
                }

                if (ReferenceEquals (pane, dockPane.AutoHidePane))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        #region IEnumerable Members

        IEnumerator<Pane> IEnumerable<Pane>.GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        #endregion
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="panel"></param>
    protected AutoHideStripBase (DockPanel panel)
    {
        DockPanel = panel;
        PanesTop = new PaneCollection (panel, DockState.DockTopAutoHide);
        PanesBottom = new PaneCollection (panel, DockState.DockBottomAutoHide);
        PanesLeft = new PaneCollection (panel, DockState.DockLeftAutoHide);
        PanesRight = new PaneCollection (panel, DockState.DockRightAutoHide);

        SetStyle (ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle (ControlStyles.Selectable, false);
    }

    /// <summary>
    ///
    /// </summary>
    protected DockPanel DockPanel { get; private set; }

    /// <summary>
    ///
    /// </summary>
    protected PaneCollection PanesTop { get; private set; }

    /// <summary>
    ///
    /// </summary>
    protected PaneCollection PanesBottom { get; private set; }

    /// <summary>
    ///
    /// </summary>
    protected PaneCollection PanesLeft { get; private set; }

    /// <summary>
    ///
    /// </summary>
    protected PaneCollection PanesRight { get; private set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockState"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>

    protected PaneCollection GetPanes (DockState dockState)
    {
        return dockState switch
        {
            DockState.DockTopAutoHide => PanesTop,
            DockState.DockBottomAutoHide => PanesBottom,
            DockState.DockLeftAutoHide => PanesLeft,
            DockState.DockRightAutoHide => PanesRight,
            _ => throw new ArgumentOutOfRangeException (nameof (dockState))
        };
    }

    internal int GetNumberOfPanes (DockState dockState)
    {
        return GetPanes (dockState).Count;
    }

    /// <summary>
    /// The top left rectangle in auto hide strip area.
    /// </summary>
    protected Rectangle RectangleTopLeft
    {
        get
        {
            var standard = MeasureHeight();
            var padding = DockPanel.Theme.Measures.DockPadding;
            var width = PanesLeft.Count > 0 ? standard : padding;
            var height = PanesTop.Count > 0 ? standard : padding;
            return new Rectangle (0, 0, width, height);
        }
    }

    /// <summary>
    /// The top right rectangle in auto hide strip area.
    /// </summary>
    protected Rectangle RectangleTopRight
    {
        get
        {
            var standard = MeasureHeight();
            var padding = DockPanel.Theme.Measures.DockPadding;
            var width = PanesRight.Count > 0 ? standard : padding;
            var height = PanesTop.Count > 0 ? standard : padding;
            return new Rectangle (Width - width, 0, width, height);
        }
    }

    /// <summary>
    /// The bottom left rectangle in auto hide strip area.
    /// </summary>
    protected Rectangle RectangleBottomLeft
    {
        get
        {
            var standard = MeasureHeight();
            var padding = DockPanel.Theme.Measures.DockPadding;
            var width = PanesLeft.Count > 0 ? standard : padding;
            var height = PanesBottom.Count > 0 ? standard : padding;
            return new Rectangle (0, Height - height, width, height);
        }
    }

    /// <summary>
    /// The bottom right rectangle in auto hide strip area.
    /// </summary>
    protected Rectangle RectangleBottomRight
    {
        get
        {
            var standard = MeasureHeight();
            var padding = DockPanel.Theme.Measures.DockPadding;
            var width = PanesRight.Count > 0 ? standard : padding;
            var height = PanesBottom.Count > 0 ? standard : padding;
            return new Rectangle (Width - width, Height - height, width, height);
        }
    }

    /// <summary>
    /// Gets one of the four auto hide strip rectangles.
    /// </summary>
    /// <param name="dockState">Dock state.</param>
    /// <returns>The desired rectangle.</returns>
    /// <remarks>
    /// As the corners are represented by <see cref="RectangleTopLeft"/>, <see cref="RectangleTopRight"/>, <see cref="RectangleBottomLeft"/>, and <see cref="RectangleBottomRight"/>,
    /// the four strips can be easily calculated out as the borders.
    /// </remarks>
    protected internal Rectangle GetTabStripRectangle (DockState dockState)
    {
        if (dockState == DockState.DockTopAutoHide)
        {
            return new Rectangle (RectangleTopLeft.Width, 0,
                Width - RectangleTopLeft.Width - RectangleTopRight.Width, RectangleTopLeft.Height);
        }

        if (dockState == DockState.DockBottomAutoHide)
        {
            return new Rectangle (RectangleBottomLeft.Width, Height - RectangleBottomLeft.Height,
                Width - RectangleBottomLeft.Width - RectangleBottomRight.Width, RectangleBottomLeft.Height);
        }

        if (dockState == DockState.DockLeftAutoHide)
        {
            return new Rectangle (0, RectangleTopLeft.Height, RectangleTopLeft.Width,
                Height - RectangleTopLeft.Height - RectangleBottomLeft.Height);
        }

        if (dockState == DockState.DockRightAutoHide)
        {
            return new Rectangle (Width - RectangleTopRight.Width, RectangleTopRight.Height,
                RectangleTopRight.Width, Height - RectangleTopRight.Height - RectangleBottomRight.Height);
        }

        return Rectangle.Empty;
    }

    private GraphicsPath? _displayingArea;

    private GraphicsPath DisplayingArea => _displayingArea ??= new GraphicsPath();

    private void SetRegion()
    {
        DisplayingArea.Reset();
        DisplayingArea.AddRectangle (RectangleTopLeft);
        DisplayingArea.AddRectangle (RectangleTopRight);
        DisplayingArea.AddRectangle (RectangleBottomLeft);
        DisplayingArea.AddRectangle (RectangleBottomRight);
        DisplayingArea.AddRectangle (GetTabStripRectangle (DockState.DockTopAutoHide));
        DisplayingArea.AddRectangle (GetTabStripRectangle (DockState.DockBottomAutoHide));
        DisplayingArea.AddRectangle (GetTabStripRectangle (DockState.DockLeftAutoHide));
        DisplayingArea.AddRectangle (GetTabStripRectangle (DockState.DockRightAutoHide));
        Region = new Region (DisplayingArea);
    }

    /// <inheritdoc cref="Control.OnMouseDown"/>
    protected override void OnMouseDown (MouseEventArgs e)
    {
        base.OnMouseDown (e);

        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        var content = HitTest();
        if (content == null)
        {
            return;
        }

        SetActiveAutoHideContent (content);

        content.DockHandler.Activate();
    }

    /// <inheritdoc cref="Control.OnMouseHover"/>
    protected override void OnMouseHover (EventArgs e)
    {
        base.OnMouseHover (e);

        if (!DockPanel.ShowAutoHideContentOnHover)
        {
            return;
        }

        // IMPORTANT: VS2003/2005 themes only.
        var content = HitTest();
        SetActiveAutoHideContent (content);

        // requires further tracking of mouse hover behavior,
        ResetMouseEventArgs();
    }

    private void SetActiveAutoHideContent (IDockContent? content)
    {
        if (content != null)
        {
            if (DockPanel.ActiveAutoHideContent != content)
            {
                DockPanel.ActiveAutoHideContent = content;
            }
            else if (!DockPanel.ShowAutoHideContentOnHover)
            {
                DockPanel.ActiveAutoHideContent = null; // IMPORTANT: Not needed for VS2003/2005 themes.
            }
        }
    }

    /// <inheritdoc cref="Control.OnLayout"/>
    protected override void OnLayout (LayoutEventArgs levent)
    {
        RefreshChanges();
        base.OnLayout (levent);
    }

    internal void RefreshChanges()
    {
        if (IsDisposed)
        {
            return;
        }

        SetRegion();
        OnRefreshChanges();
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnRefreshChanges()
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected internal abstract int MeasureHeight();

    private IDockContent? HitTest()
    {
        var ptMouse = PointToClient (Control.MousePosition);
        return HitTest (ptMouse);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    protected virtual Tab CreateTab (IDockContent content)
    {
        return new Tab (content);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockPane"></param>
    /// <returns></returns>
    protected virtual Pane CreatePane (DockPane dockPane)
    {
        return new Pane (dockPane);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    protected abstract IDockContent? HitTest (Point point);

    /// <inheritdoc cref="Control.CreateAccessibilityInstance"/>
    protected override AccessibleObject CreateAccessibilityInstance()
    {
        return new AutoHideStripsAccessibleObject (this);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tab"></param>
    /// <returns></returns>
    protected abstract Rectangle GetTabBounds (Tab tab);

    internal static Rectangle ToScreen (Rectangle rectangle, Control? parent)
    {
        if (parent == null)
        {
            return rectangle;
        }

        return new Rectangle (parent.PointToScreen (new Point (rectangle.Left, rectangle.Top)),
            new Size (rectangle.Width, rectangle.Height));
    }

    /// <summary>
    ///
    /// </summary>
    public class AutoHideStripsAccessibleObject
        : ControlAccessibleObject
    {
        private readonly AutoHideStripBase _strip;

        /// <summary>
        ///
        /// </summary>
        /// <param name="strip"></param>
        public AutoHideStripsAccessibleObject (AutoHideStripBase strip)
            : base (strip)
        {
            _strip = strip;
        }

        /// <inheritdoc cref="ControlAccessibleObject.Role"/>
        public override AccessibleRole Role => AccessibleRole.Window;

        /// <inheritdoc cref="AccessibleObject.GetChildCount"/>
        public override int GetChildCount()
        {
            // Top, Bottom, Left, Right
            return 4;
        }

        /// <inheritdoc cref="AccessibleObject.GetChild"/>
        public override AccessibleObject GetChild (int index)
        {
            return index switch
            {
                0 => new AutoHideStripAccessibleObject (_strip, DockState.DockTopAutoHide, this),
                1 => new AutoHideStripAccessibleObject (_strip, DockState.DockBottomAutoHide, this),
                2 => new AutoHideStripAccessibleObject (_strip, DockState.DockLeftAutoHide, this),
                _ => new AutoHideStripAccessibleObject (_strip, DockState.DockRightAutoHide, this)
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override AccessibleObject? HitTest (int x, int y)
        {
            var rectangles = new Dictionary<DockState, Rectangle>
            {
                { DockState.DockTopAutoHide, _strip.GetTabStripRectangle (DockState.DockTopAutoHide) },
                { DockState.DockBottomAutoHide, _strip.GetTabStripRectangle (DockState.DockBottomAutoHide) },
                { DockState.DockLeftAutoHide, _strip.GetTabStripRectangle (DockState.DockLeftAutoHide) },
                { DockState.DockRightAutoHide, _strip.GetTabStripRectangle (DockState.DockRightAutoHide) },
            };

            var point = _strip.PointToClient (new Point (x, y));
            foreach (var rectangle in rectangles)
            {
                if (rectangle.Value.Contains (point))
                {
                    return new AutoHideStripAccessibleObject (_strip, rectangle.Key, this);
                }
            }

            return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class AutoHideStripAccessibleObject
        : AccessibleObject
    {
        private readonly AutoHideStripBase _strip;
        private readonly DockState _state;

        /// <summary>
        ///
        /// </summary>
        /// <param name="strip"></param>
        /// <param name="state"></param>
        /// <param name="parent"></param>
        public AutoHideStripAccessibleObject
            (
                AutoHideStripBase strip,
                DockState state, AccessibleObject parent
            )
        {
            _strip = strip;
            _state = state;
            Parent = parent;
        }

        /// <inheritdoc cref="AccessibleObject.Parent"/>
        public override AccessibleObject Parent { get; }

        /// <inheritdoc cref="AccessibleObject.Role"/>
        public override AccessibleRole Role => AccessibleRole.PageTabList;

        /// <inheritdoc cref="AccessibleObject.GetChildCount"/>
        public override int GetChildCount()
        {
            var count = 0;
            foreach (var pane in _strip.GetPanes (_state))
            {
                count += pane.AutoHideTabs.Count;
            }

            return count;
        }

        /// <inheritdoc cref="AccessibleObject.GetChild"/>
        public override AccessibleObject GetChild (int index)
        {
            var tabs = new List<Tab>();
            foreach (var pane in _strip.GetPanes (_state))
            {
                tabs.AddRange (pane.AutoHideTabs);
            }

            return new AutoHideStripTabAccessibleObject (_strip, tabs[index], this);
        }

        /// <inheritdoc cref="AccessibleObject.Bounds"/>
        public override Rectangle Bounds
        {
            get
            {
                var rectangle = _strip.GetTabStripRectangle (_state);
                return ToScreen (rectangle, _strip);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected class AutoHideStripTabAccessibleObject
        : AccessibleObject
    {
        private readonly AutoHideStripBase _strip;
        private readonly Tab _tab;

        internal AutoHideStripTabAccessibleObject (AutoHideStripBase strip, Tab tab, AccessibleObject parent)
        {
            _strip = strip;
            _tab = tab;

            Parent = parent;
        }

        /// <inheritdoc cref="AccessibleObject.Parent"/>
        public override AccessibleObject Parent { get; }

        /// <inheritdoc cref="AccessibleObject.Role"/>
        public override AccessibleRole Role => AccessibleRole.PageTab;

        /// <summary>
        ///
        /// </summary>
        public override Rectangle Bounds
        {
            get
            {
                var rectangle = _strip.GetTabBounds (_tab);
                return ToScreen (rectangle, _strip);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override string? Name
        {
            get => _tab.Content.DockHandler.TabText;
            set
            {
                //base.Name = value;
            }
        }
    }
}
