// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DockPaneCaptionBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public abstract class DockPaneCaptionBase
    : Control
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="pane"></param>
    protected internal DockPaneCaptionBase (DockPane pane)
    {
        DockPane = pane;

        SetStyle (ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.ResizeRedraw |
                  ControlStyles.UserPaint |
                  ControlStyles.AllPaintingInWmPaint, true);
        SetStyle (ControlStyles.Selectable, false);
    }

    /// <summary>
    ///
    /// </summary>
    public DockPane DockPane { get; }

    /// <summary>
    ///
    /// </summary>
    protected DockPane.AppearanceStyle Appearance => DockPane.Appearance;

    /// <summary>
    ///
    /// </summary>
    protected bool HasTabPageContextMenu => DockPane.HasTabPageContextMenu;

    /// <summary>
    ///
    /// </summary>
    /// <param name="position"></param>
    protected void ShowTabPageContextMenu (Point position)
    {
        DockPane.ShowTabPageContextMenu (this, position);
    }

    /// <inheritdoc cref="Control.OnMouseUp"/>
    protected override void OnMouseUp (MouseEventArgs eventArgs)
    {
        base.OnMouseUp (eventArgs);

        if (eventArgs.Button == MouseButtons.Right)
        {
            ShowTabPageContextMenu (new Point (eventArgs.X, eventArgs.Y));
        }
    }

    /// <inheritdoc cref="Control.OnMouseDown"/>
    protected override void OnMouseDown (MouseEventArgs eventArgs)
    {
        base.OnMouseDown (eventArgs);

        if (eventArgs.Button == MouseButtons.Left &&
            DockPane.DockPanel.AllowEndUserDocking &&
            DockPane.AllowDockDragAndDrop &&
            DockPane.ActiveContent != null &&
            (!DockHelper.IsDockStateAutoHide (DockPane.DockState) || CanDragAutoHide))
        {
            DockPane.DockPanel.BeginDrag (DockPane);
        }
    }

    /// <inheritdoc cref="Control.WndProc"/>
    protected override void WndProc (ref Message m)
    {
        if (m.Msg == (int)Win32.Msgs.WM_LBUTTONDBLCLK)
        {
            if (DockHelper.IsDockStateAutoHide (DockPane.DockState))
            {
                DockPane.DockPanel.ActiveAutoHideContent = null;
                return;
            }

            if (DockPane.IsFloat)
            {
                DockPane.RestoreToPanel();
            }
            else
            {
                DockPane.Float();
            }
        }

        base.WndProc (ref m);
    }

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
    protected virtual void OnRightToLeftLayoutChanged()
    {
        // пустое тело метода
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

    /// <summary>
    /// Gets a value indicating whether dock panel can be dragged when in auto hide mode.
    /// Default is false.
    /// </summary>
    protected virtual bool CanDragAutoHide => false;
}
