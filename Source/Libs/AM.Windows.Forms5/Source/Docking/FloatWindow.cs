// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

/* FloatWindow.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public class FloatWindow
    : Form, INestedPanesContainer, IDockDragSource
{
    internal const int WM_CHECKDISPOSE = (int)(Win32.Msgs.WM_USER + 1);

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected internal FloatWindow (DockPanel dockPanel, DockPane pane)
    {
        DockPanel = null!;

        InternalConstruct (dockPanel, pane, false, Rectangle.Empty);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected internal FloatWindow (DockPanel dockPanel, DockPane pane, Rectangle bounds)
    {
        DockPanel = null!;

        InternalConstruct (dockPanel, pane, true, bounds);
    }

    private void InternalConstruct (DockPanel dockPanel, DockPane pane, bool boundsSpecified, Rectangle bounds)
    {
        if (dockPanel == null)
        {
            throw (new ArgumentNullException (Strings.FloatWindow_Constructor_NullDockPanel));
        }

        NestedPanes = new NestedPaneCollection (this);

        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        ShowInTaskbar = false;
        if (dockPanel.RightToLeft != RightToLeft)
        {
            RightToLeft = dockPanel.RightToLeft;
        }

        if (RightToLeftLayout != dockPanel.RightToLeftLayout)
        {
            RightToLeftLayout = dockPanel.RightToLeftLayout;
        }

        SuspendLayout();
        if (boundsSpecified)
        {
            Bounds = bounds;
            StartPosition = FormStartPosition.Manual;
        }
        else
        {
            StartPosition = FormStartPosition.WindowsDefaultLocation;
            Size = dockPanel.DefaultFloatWindowSize;
        }

        DockPanel = dockPanel;
        Owner = DockPanel.FindForm();
        DockPanel.AddFloatWindow (this);
        if (pane != null!)
        {
            pane.FloatWindow = this;
        }

        if (PatchController.EnableFontInheritanceFix == true)
        {
            Font = dockPanel.Font;
        }

        ResumeLayout();
    }

    /// <inheritdoc cref="Form.Dispose(bool)"/>
    protected override void Dispose (bool disposing)
    {
        if (disposing)
        {
            if (DockPanel != null!)
            {
                DockPanel.RemoveFloatWindow (this);
            }

            DockPanel = null!;
        }

        base.Dispose (disposing);
    }

    /// <summary>
    ///
    /// </summary>
    public bool AllowEndUserDocking { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public bool DoubleClickTitleBarToDock { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public NestedPaneCollection? NestedPanes { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public VisibleNestedPaneCollection VisibleNestedPanes => NestedPanes!.VisibleNestedPanes;

    /// <summary>
    ///
    /// </summary>
    public DockPanel DockPanel { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public DockState DockState => DockState.Float;

    /// <summary>
    ///
    /// </summary>
    public bool IsFloat => DockState == DockState.Float;

    internal bool IsDockStateValid (DockState dockState)
    {
        foreach (var pane in NestedPanes!)
        {
            foreach (var content in pane.Contents)
            {
                if (!DockHelper.IsDockStateValid (dockState, content.DockHandler.DockAreas))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <inheritdoc cref="Form.OnActivated"/>
    protected override void OnActivated (EventArgs e)
    {
        DockPanel.FloatWindows.BringWindowToFront (this);
        base.OnActivated (e);

        // Propagate the Activated event to the visible panes content objects
        foreach (var pane in VisibleNestedPanes)
        {
            foreach (var content in pane.Contents)
            {
                content.OnActivated (e);
            }
        }
    }

    /// <inheritdoc cref="Form.OnDeactivate"/>
    protected override void OnDeactivate (EventArgs e)
    {
        base.OnDeactivate (e);

        // Propagate the Deactivate event to the visible panes content objects
        foreach (var pane in VisibleNestedPanes)
        {
            foreach (var content in pane.Contents)
            {
                content.OnDeactivate (e);
            }
        }
    }

    /// <inheritdoc cref="Form.OnLayout"/>
    protected override void OnLayout (LayoutEventArgs levent)
    {
        VisibleNestedPanes.Refresh();
        RefreshChanges();
        Visible = (VisibleNestedPanes.Count > 0);
        SetText();

        base.OnLayout (levent);
    }


    [SuppressMessage ("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters",
        MessageId = "System.Windows.Forms.Control.set_Text(System.String)")]
    internal void SetText()
    {
        var theOnlyPane = (VisibleNestedPanes.Count == 1) ? VisibleNestedPanes[0] : null;

        if (theOnlyPane == null || theOnlyPane.ActiveContent == null)
        {
            Text = " "; // use " " instead of string.Empty because the whole title bar will disappear when ControlBox is set to false.
            Icon = null;
        }
        else
        {
            Text = theOnlyPane.ActiveContent.DockHandler.TabText;
            Icon = theOnlyPane.ActiveContent.DockHandler.Icon;
        }
    }

    /// <inheritdoc cref="Form.SetBoundsCore"/>
    protected override void SetBoundsCore (int x, int y, int width, int height, BoundsSpecified specified)
    {
        var rectWorkArea = SystemInformation.VirtualScreen;

        if (y + height > rectWorkArea.Bottom)
        {
            y -= (y + height) - rectWorkArea.Bottom;
        }

        if (y < rectWorkArea.Top)
        {
            y += rectWorkArea.Top - y;
        }

        base.SetBoundsCore (x, y, width, height, specified);
    }

    /// <inheritdoc cref="Form.WndProc"/>
    protected override void WndProc (ref Message m)
    {
        switch (m.Msg)
        {
            case (int)Win32.Msgs.WM_NCLBUTTONDOWN:
            {
                if (IsDisposed)
                {
                    return;
                }

                var result = Win32Helper.IsRunningOnMono
                    ? 0
                    : Win32.NativeMethods.SendMessage (this.Handle, (int)Win32.Msgs.WM_NCHITTEST, 0,
                        (uint)m.LParam);
                if (result == 2 && DockPanel.AllowEndUserDocking && this.AllowEndUserDocking) // HITTEST_CAPTION
                {
                    Activate();
                    DockPanel.BeginDrag (this);
                }
                else
                {
                    base.WndProc (ref m);
                }

                return;
            }
            case (int)Win32.Msgs.WM_NCRBUTTONDOWN:
            {
                var result = Win32Helper.IsRunningOnMono
                    ? Win32Helper.HitTestCaption (this)
                    : Win32.NativeMethods.SendMessage (this.Handle, (int)Win32.Msgs.WM_NCHITTEST, 0,
                        (uint)m.LParam);
                if (result == 2) // HITTEST_CAPTION
                {
                    var theOnlyPane = (VisibleNestedPanes.Count == 1) ? VisibleNestedPanes[0] : null;
                    if (theOnlyPane is { ActiveContent: { } })
                    {
                        theOnlyPane.ShowTabPageContextMenu (this, PointToClient (Control.MousePosition));
                        return;
                    }
                }

                base.WndProc (ref m);
                return;
            }
            case (int)Win32.Msgs.WM_CLOSE:
                if (NestedPanes!.Count == 0)
                {
                    base.WndProc (ref m);
                    return;
                }

                for (var i = NestedPanes.Count - 1; i >= 0; i--)
                {
                    var contents = NestedPanes[i].Contents;
                    for (var j = contents.Count - 1; j >= 0; j--)
                    {
                        var content = contents[j];
                        if (content.DockHandler.DockState != DockState.Float)
                        {
                            continue;
                        }

                        if (!content.DockHandler.CloseButton)
                        {
                            continue;
                        }

                        if (content.DockHandler.HideOnClose)
                        {
                            content.DockHandler.Hide();
                        }
                        else
                        {
                            content.DockHandler.Close();
                        }
                    }
                }

                return;
            case (int)Win32.Msgs.WM_NCLBUTTONDBLCLK:
            {
                var result = !DoubleClickTitleBarToDock || Win32Helper.IsRunningOnMono
                    ? Win32Helper.HitTestCaption (this)
                    : Win32.NativeMethods.SendMessage (this.Handle, (int)Win32.Msgs.WM_NCHITTEST, 0,
                        (uint)m.LParam);

                if (result != 2) // HITTEST_CAPTION
                {
                    base.WndProc (ref m);
                    return;
                }

                DockPanel.SuspendLayout (true);

                // Restore to panel
                foreach (var pane in NestedPanes!)
                {
                    if (pane.DockState != DockState.Float)
                    {
                        continue;
                    }

                    pane.RestoreToPanel();
                }


                DockPanel.ResumeLayout (true, true);
                return;
            }
            case WM_CHECKDISPOSE:
                if (NestedPanes!.Count == 0)
                {
                    Dispose();
                }

                return;
        }

        base.WndProc (ref m);
    }

    internal void RefreshChanges()
    {
        if (IsDisposed)
        {
            return;
        }

        if (VisibleNestedPanes.Count == 0)
        {
            ControlBox = true;
            return;
        }

        for (var i = VisibleNestedPanes.Count - 1; i >= 0; i--)
        {
            var contents = VisibleNestedPanes[i].Contents;
            for (var j = contents.Count - 1; j >= 0; j--)
            {
                var content = contents[j];
                if (content.DockHandler.DockState != DockState.Float)
                {
                    continue;
                }

                if (content.DockHandler is { CloseButton: true, CloseButtonVisible: true })
                {
                    ControlBox = true;
                    return;
                }
            }
        }

        //Only if there is a ControlBox do we turn it off
        //old code caused a flash of the window.
        if (ControlBox)
        {
            ControlBox = false;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual Rectangle DisplayingRectangle => ClientRectangle;

    internal void TestDrop (IDockDragSource dragSource, DockOutlineBase dockOutline)
    {
        if (VisibleNestedPanes.Count == 1)
        {
            var pane = VisibleNestedPanes[0];
            if (!dragSource.CanDockTo (pane))
            {
                return;
            }

            var ptMouse = Control.MousePosition;
            var lParam = Win32Helper.MakeLong (ptMouse.X, ptMouse.Y);
            if (!Win32Helper.IsRunningOnMono)
            {
                if (Win32.NativeMethods.SendMessage (Handle, (int)Win32.Msgs.WM_NCHITTEST, 0, lParam) ==
                    (uint)Win32.HitTest.HTCAPTION)
                {
                    dockOutline.Show (VisibleNestedPanes[0], -1);
                }
            }
        }
    }

    #region IDockDragSource Members

    #region IDragSource Members

    Control IDragSource.DragControl => this;

    #endregion

    bool IDockDragSource.IsDockStateValid (DockState dockState)
    {
        return IsDockStateValid (dockState);
    }

    bool IDockDragSource.CanDockTo (DockPane pane)
    {
        if (!IsDockStateValid (pane.DockState))
        {
            return false;
        }

        if (pane.FloatWindow == this)
        {
            return false;
        }

        return true;
    }

    private int m_preDragExStyle;

    Rectangle IDockDragSource.BeginDrag (Point ptMouse)
    {
        m_preDragExStyle =
            Win32.NativeMethods.GetWindowLong (this.Handle, (int)Win32.GetWindowLongIndex.GWL_EXSTYLE);
        Win32.NativeMethods.SetWindowLong (this.Handle,
            (int)Win32.GetWindowLongIndex.GWL_EXSTYLE,
            m_preDragExStyle | (int)(Win32.WindowExStyles.WS_EX_TRANSPARENT | Win32.WindowExStyles.WS_EX_LAYERED));
        return Bounds;
    }

    void IDockDragSource.EndDrag()
    {
        Win32.NativeMethods.SetWindowLong (this.Handle, (int)Win32.GetWindowLongIndex.GWL_EXSTYLE,
            m_preDragExStyle);

        Invalidate (true);
        Win32.NativeMethods.SendMessage (this.Handle, (int)Win32.Msgs.WM_NCPAINT, 1, 0);
    }

    /// <inheritdoc cref="IDockDragSource.FloatAt"/>
    public void FloatAt (Rectangle floatWindowBounds)
    {
        Bounds = floatWindowBounds;
    }

    /// <inheritdoc cref="IDockDragSource.DockTo(AM.Windows.Forms.Docking.DockPane,System.Windows.Forms.DockStyle,int)"/>
    public void DockTo (DockPane pane, DockStyle dockStyle, int contentIndex)
    {
        if (dockStyle == DockStyle.Fill)
        {
            for (var i = NestedPanes!.Count - 1; i >= 0; i--)
            {
                var paneFrom = NestedPanes[i];
                for (var j = paneFrom.Contents.Count - 1; j >= 0; j--)
                {
                    var c = paneFrom.Contents[j];
                    c.DockHandler.Pane = pane;
                    if (contentIndex != -1)
                    {
                        pane.SetContentIndex (c, contentIndex);
                    }

                    c.DockHandler.Activate();
                }
            }
        }
        else
        {
            var alignment = DockAlignment.Left;
            if (dockStyle == DockStyle.Left)
            {
                alignment = DockAlignment.Left;
            }
            else if (dockStyle == DockStyle.Right)
            {
                alignment = DockAlignment.Right;
            }
            else if (dockStyle == DockStyle.Top)
            {
                alignment = DockAlignment.Top;
            }
            else if (dockStyle == DockStyle.Bottom)
            {
                alignment = DockAlignment.Bottom;
            }

            MergeNestedPanes (VisibleNestedPanes, pane.NestedPanesContainer!.NestedPanes!, pane, alignment, 0.5);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="dockStyle"></param>
    /// <exception cref="ArgumentException"></exception>
    public void DockTo (DockPanel panel, DockStyle dockStyle)
    {
        if (panel != DockPanel)
        {
            throw new ArgumentException (Strings.IDockDragSource_DockTo_InvalidPanel, nameof (panel));
        }

        NestedPaneCollection? nestedPanesTo = null;

        if (dockStyle == DockStyle.Top)
        {
            nestedPanesTo = DockPanel.DockWindows[DockState.DockTop].NestedPanes;
        }
        else if (dockStyle == DockStyle.Bottom)
        {
            nestedPanesTo = DockPanel.DockWindows[DockState.DockBottom].NestedPanes;
        }
        else if (dockStyle == DockStyle.Left)
        {
            nestedPanesTo = DockPanel.DockWindows[DockState.DockLeft].NestedPanes;
        }
        else if (dockStyle == DockStyle.Right)
        {
            nestedPanesTo = DockPanel.DockWindows[DockState.DockRight].NestedPanes;
        }
        else if (dockStyle == DockStyle.Fill)
        {
            nestedPanesTo = DockPanel.DockWindows[DockState.Document].NestedPanes;
        }

        DockPane? prevPane = null;
        for (var i = nestedPanesTo!.Count - 1; i >= 0; i--)
            if (nestedPanesTo[i] != VisibleNestedPanes[0])
            {
                prevPane = nestedPanesTo[i];
            }

        MergeNestedPanes (VisibleNestedPanes, nestedPanesTo, prevPane!, DockAlignment.Left, 0.5);
    }

    private static void MergeNestedPanes (VisibleNestedPaneCollection nestedPanesFrom,
        NestedPaneCollection nestedPanesTo, DockPane prevPane, DockAlignment alignment, double proportion)
    {
        if (nestedPanesFrom.Count == 0)
        {
            return;
        }

        var count = nestedPanesFrom.Count;
        var panes = new DockPane[count];
        var prevPanes = new DockPane[count];
        var alignments = new DockAlignment[count];
        var proportions = new double[count];

        for (var i = 0; i < count; i++)
        {
            panes[i] = nestedPanesFrom[i];
            prevPanes[i] = nestedPanesFrom[i].NestedDockingStatus.PreviousPane!;
            alignments[i] = nestedPanesFrom[i].NestedDockingStatus.Alignment;
            proportions[i] = nestedPanesFrom[i].NestedDockingStatus.Proportion;
        }

        var pane = panes[0].DockTo (nestedPanesTo.Container, prevPane, alignment, proportion);
        panes[0].DockState = nestedPanesTo.DockState;

        for (var i = 1; i < count; i++)
        {
            for (var j = i; j < count; j++)
            {
                if (prevPanes[j] == panes[i - 1])
                {
                    prevPanes[j] = pane!;
                }
            }

            pane = panes[i].DockTo (nestedPanesTo.Container, prevPanes[i], alignments[i], proportions[i]);
            panes[i].DockState = nestedPanesTo.DockState;
        }
    }

    #endregion
}
