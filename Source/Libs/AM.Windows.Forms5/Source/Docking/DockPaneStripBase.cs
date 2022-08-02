// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable VirtualMemberCallInConstructor

/* DockPaneStripBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public abstract class DockPaneStripBase
    : Control
{
    /// <summary>
    ///
    /// </summary>
    [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    protected internal class Tab
        : IDisposable
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Tab (IDockContent content)
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

        /// <summary>
        ///
        /// </summary>
        public Form? ContentForm => Content as Form;

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Dispose (bool disposing)
        {
        }

        private Rectangle? _rect;

        /// <summary>
        ///
        /// </summary>
        public Rectangle? Rectangle
        {
            get
            {
                if (_rect != null)
                {
                    return _rect;
                }

                return _rect = System.Drawing.Rectangle.Empty;
            }

            set { _rect = value; }
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
        public int Count => DockPane.DisplayingContents.Count;

        /// <summary>
        ///
        /// </summary>
        public Tab this [int index]
        {
            get
            {
                var content = DockPane.DisplayingContents[index];
                if (content == null)
                {
                    throw new ArgumentOutOfRangeException (nameof (index));
                }

                return content.DockHandler.GetTab (DockPane.TabStripControl);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool Contains (Tab tab)
        {
            return IndexOf (tab) >= 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        public bool Contains (IDockContent content)
        {
            return IndexOf (content) != -1;
        }

        /// <summary>
        ///
        /// </summary>
        public int IndexOf (Tab? tab)
        {
            if (tab == null)
            {
                return -1;
            }

            return DockPane.DisplayingContents.IndexOf (tab.Content);
        }

        /// <summary>
        ///
        /// </summary>
        public int IndexOf (IDockContent content)
        {
            return DockPane.DisplayingContents.IndexOf (content);
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected DockPaneStripBase (DockPane pane)
    {
        DockPane = pane;

        SetStyle (ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle (ControlStyles.Selectable, false);
        AllowDrop = true;
    }

    /// <summary>
    ///
    /// </summary>
    protected DockPane DockPane { get; }

    /// <summary>
    ///
    /// </summary>
    protected DockPane.AppearanceStyle Appearance => DockPane.Appearance;

    private TabCollection? _tabs;

    /// <summary>
    ///
    /// </summary>
    protected TabCollection Tabs => _tabs ??= new (DockPane);

    internal void RefreshChanges()
    {
        if (IsDisposed)
        {
            return;
        }

        OnRefreshChanges();
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnRefreshChanges()
    {
    }

    /// <summary>
    ///
    /// </summary>
    protected internal abstract int MeasureHeight();

    /// <summary>
    ///
    /// </summary>
    protected internal abstract void EnsureTabVisible (IDockContent content);

    /// <summary>
    ///
    /// </summary>
    protected int HitTest()
    {
        return HitTest (PointToClient (Control.MousePosition));
    }

    /// <summary>
    ///
    /// </summary>
    protected internal abstract int HitTest (Point point);

    /// <summary>
    ///
    /// </summary>
    protected virtual bool MouseDownActivateTest (MouseEventArgs e)
    {
        return true;
    }

    /// <summary>
    ///
    /// </summary>
    public abstract GraphicsPath GetOutline (int index);

    /// <summary>
    ///
    /// </summary>
    protected internal virtual Tab CreateTab (IDockContent content)
    {
        return new Tab (content);
    }

    private Rectangle _dragBox = Rectangle.Empty;

    /// <inheritdoc cref="Control.OnMouseDown"/>
    protected override void OnMouseDown (MouseEventArgs e)
    {
        base.OnMouseDown (e);
        var index = HitTest();
        if (index != -1)
        {
            if (e.Button == MouseButtons.Middle)
            {
                // Close the specified content.
                TryCloseTab (index);
            }
            else
            {
                var content = Tabs[index].Content;
                if (DockPane.ActiveContent != content)
                {
                    // Test if the content should be active
                    if (MouseDownActivateTest (e))
                    {
                        DockPane.ActiveContent = content;
                    }
                }
            }
        }

        if (e.Button == MouseButtons.Left)
        {
            var dragSize = SystemInformation.DragSize;
            _dragBox = new Rectangle (new Point (e.X - dragSize.Width / 2,
                e.Y - dragSize.Height / 2), dragSize);
        }
    }

    /// <inheritdoc cref="Control.OnMouseMove"/>
    protected override void OnMouseMove (MouseEventArgs e)
    {
        base.OnMouseMove (e);

        if (e.Button != MouseButtons.Left || _dragBox.Contains (e.Location))
        {
            return;
        }

        if (DockPane.ActiveContent == null)
        {
            return;
        }

        if (DockPane.DockPanel.AllowEndUserDocking && DockPane.AllowDockDragAndDrop &&
            DockPane.ActiveContent.DockHandler.AllowEndUserDocking)
        {
            DockPane.DockPanel.BeginDrag (DockPane.ActiveContent.DockHandler);
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected bool HasTabPageContextMenu => DockPane.HasTabPageContextMenu;

    /// <summary>
    ///
    /// </summary>
    protected void ShowTabPageContextMenu (Point position)
    {
        DockPane.ShowTabPageContextMenu (this, position);
    }

    /// <summary>
    ///
    /// </summary>
    protected bool TryCloseTab (int index)
    {
        if (index >= 0 || index < Tabs.Count)
        {
            // Close the specified content.
            var content = Tabs[index].Content;
            DockPane.CloseContent (content);
            if (PatchController.EnableSelectClosestOnClose == true)
            {
                SelectClosestPane (index);
            }

            return true;
        }

        return false;
    }

    private void SelectClosestPane (int index)
    {
        if (index > 0 && DockPane.DockPanel.DocumentStyle == DocumentStyle.DockingWindow)
        {
            index = index - 1;

            if (index >= 0 || index < Tabs.Count)
            {
                DockPane.ActiveContent = Tabs[index].Content;
            }
        }
    }

    /// <inheritdoc cref="Control.OnMouseUp"/>
    protected override void OnMouseUp (MouseEventArgs e)
    {
        base.OnMouseUp (e);

        if (e.Button == MouseButtons.Right)
        {
            ShowTabPageContextMenu (new Point (e.X, e.Y));
        }
    }

    /// <inheritdoc cref="Control.WndProc"/>
    protected override void WndProc (ref Message m)
    {
        if (m.Msg == (int)Win32.Msgs.WM_LBUTTONDBLCLK)
        {
            base.WndProc (ref m);

            var index = HitTest();
            if (DockPane.DockPanel.AllowEndUserDocking && index != -1)
            {
                var content = Tabs[index].Content;
                if (content.DockHandler.CheckDockState (!content.DockHandler.IsFloat) != DockState.Unknown)
                {
                    content.DockHandler.IsFloat = !content.DockHandler.IsFloat;
                }
            }

            return;
        }

        base.WndProc (ref m);
    }

    /// <inheritdoc cref="Control.OnDragOver"/>
    protected override void OnDragOver (DragEventArgs drgevent)
    {
        base.OnDragOver (drgevent);

        var index = HitTest();
        if (index != -1)
        {
            var content = Tabs[index].Content;
            if (DockPane.ActiveContent != content)
            {
                DockPane.ActiveContent = content;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected void ContentClosed()
    {
        if (_tabs.Count == 0)
        {
            DockPane.ClearLastActiveContent();
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected abstract Rectangle GetTabBounds (Tab tab);

    internal static Rectangle ToScreen
        (
            Rectangle rectangle,
            Control? parent
        )
    {
        if (parent == null)
        {
            return rectangle;
        }

        return new Rectangle (parent.PointToScreen (new Point (rectangle.Left, rectangle.Top)),
            new Size (rectangle.Width, rectangle.Height));
    }

    /// <inheritdoc cref="Control.CreateAccessibilityInstance"/>
    protected override AccessibleObject CreateAccessibilityInstance()
    {
        return new DockPaneStripAccessibleObject (this);
    }

    /// <summary>
    ///
    /// </summary>
    public class DockPaneStripAccessibleObject
        : ControlAccessibleObject
    {
        private readonly DockPaneStripBase _strip;

        /// <summary>
        ///
        /// </summary>
        public DockPaneStripAccessibleObject (DockPaneStripBase strip)
            : base (strip)
        {
            _strip = strip;
        }

        /// <inheritdoc cref="ControlAccessibleObject.Role"/>
        public override AccessibleRole Role => AccessibleRole.PageTabList;

        /// <inheritdoc cref="AccessibleObject.GetChildCount"/>
        public override int GetChildCount()
        {
            return _strip.Tabs.Count;
        }

        /// <inheritdoc cref="AccessibleObject.GetChild"/>
        public override AccessibleObject GetChild (int index)
        {
            return new DockPaneStripTabAccessibleObject (_strip, _strip.Tabs[index], this);
        }

        /// <inheritdoc cref="AccessibleObject.HitTest"/>
        public override AccessibleObject? HitTest (int x, int y)
        {
            var point = new Point (x, y);
            foreach (var tab in _strip.Tabs)
            {
                var rectangle = _strip.GetTabBounds (tab);
                if (ToScreen (rectangle, _strip).Contains (point))
                {
                    return new DockPaneStripTabAccessibleObject (_strip, tab, this);
                }
            }

            return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected class DockPaneStripTabAccessibleObject
        : AccessibleObject
    {
        private DockPaneStripBase _strip;
        private Tab _tab;

        internal DockPaneStripTabAccessibleObject (DockPaneStripBase strip, Tab tab, AccessibleObject parent)
        {
            _strip = strip;
            _tab = tab;

            Parent = parent;
        }

        /// <inheritdoc cref="AccessibleObject.Parent"/>
        public override AccessibleObject Parent { get; }

        /// <inheritdoc cref="AccessibleObject.Role"/>
        public override AccessibleRole Role => AccessibleRole.PageTab;

        /// <inheritdoc cref="AccessibleObject.Bounds"/>
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
        public override string Name
        {
            get { return _tab.Content.DockHandler.TabText; }
            set
            {
                //base.Name = value;
            }
        }
    }
}
