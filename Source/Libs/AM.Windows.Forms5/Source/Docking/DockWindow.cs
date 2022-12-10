// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable VirtualMemberCallInConstructor

/* DockWindow.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
/// Dock window base class.
/// </summary>
[ToolboxItem (false)]
public class DockWindow
    : Panel, INestedPanesContainer, ISplitterHost
{
    private readonly SplitterBase? _splitter;

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockPanel"></param>
    /// <param name="dockState"></param>
    protected internal DockWindow
        (
            DockPanel dockPanel,
            DockState dockState
        )
    {
        NestedPanes = new NestedPaneCollection (this);
        DockPanel = dockPanel;
        DockState = dockState;
        Visible = false;

        SuspendLayout();

        if (DockState is DockState.DockLeft or DockState.DockRight or DockState.DockTop or DockState.DockBottom)
        {
            _splitter = DockPanel.Theme.Extender.WindowSplitterControlFactory.CreateSplitterControl (this);
            Controls.Add (_splitter);
        }

        if (DockState == DockState.DockLeft)
        {
            Dock = DockStyle.Left;
            _splitter!.Dock = DockStyle.Right;
        }
        else if (DockState == DockState.DockRight)
        {
            Dock = DockStyle.Right;
            _splitter!.Dock = DockStyle.Left;
        }
        else if (DockState == DockState.DockTop)
        {
            Dock = DockStyle.Top;
            _splitter!.Dock = DockStyle.Bottom;
        }
        else if (DockState == DockState.DockBottom)
        {
            Dock = DockStyle.Bottom;
            _splitter!.Dock = DockStyle.Top;
        }
        else if (DockState == DockState.Document)
        {
            Dock = DockStyle.Fill;
        }

        ResumeLayout();
    }

    /// <summary>
    ///
    /// </summary>
    public bool IsDockWindow => true;

    /// <summary>
    ///
    /// </summary>
    public VisibleNestedPaneCollection VisibleNestedPanes => NestedPanes.VisibleNestedPanes;

    /// <summary>
    ///
    /// </summary>
    public NestedPaneCollection NestedPanes { get; }

    /// <summary>
    ///
    /// </summary>
    public DockPanel DockPanel { get; }

    /// <summary>
    ///
    /// </summary>
    public DockState DockState { get; }

    /// <summary>
    ///
    /// </summary>
    public bool IsFloat => DockState == DockState.Float;

    /// <summary>
    ///
    /// </summary>
    internal DockPane? DefaultPane => VisibleNestedPanes.Count == 0 ? null : VisibleNestedPanes[0];

    /// <summary>
    ///
    /// </summary>
    public virtual Rectangle DisplayingRectangle
    {
        get
        {
            var rect = ClientRectangle;

            // if DockWindow is document, exclude the border
            if (DockState == DockState.Document)
            {
                rect.X += 1;
                rect.Y += 1;
                rect.Width -= 2;
                rect.Height -= 2;
            }

            // exclude the splitter
            else if (DockState == DockState.DockLeft)
            {
                rect.Width -= DockPanel.Theme.Measures.SplitterSize;
            }
            else if (DockState == DockState.DockRight)
            {
                rect.X += DockPanel.Theme.Measures.SplitterSize;
                rect.Width -= DockPanel.Theme.Measures.SplitterSize;
            }
            else if (DockState == DockState.DockTop)
            {
                rect.Height -= DockPanel.Theme.Measures.SplitterSize;
            }
            else if (DockState == DockState.DockBottom)
            {
                rect.Y += DockPanel.Theme.Measures.SplitterSize;
                rect.Height -= DockPanel.Theme.Measures.SplitterSize;
            }

            return rect;
        }
    }

    /// <inheritdoc cref="ScrollableControl.OnLayout"/>
    protected override void OnLayout (LayoutEventArgs levent)
    {
        VisibleNestedPanes.Refresh();
        if (VisibleNestedPanes.Count == 0)
        {
            if (Visible)
            {
                Visible = false;
            }
        }
        else if (!Visible)
        {
            Visible = true;
            VisibleNestedPanes.Refresh();
        }

        base.OnLayout (levent);
    }

    #region ISplitterDragSource Members

    void ISplitterDragSource.BeginDrag (Rectangle rectSplitter)
    {
    }

    void ISplitterDragSource.EndDrag()
    {
    }

    bool ISplitterDragSource.IsVertical => DockState is DockState.DockLeft or DockState.DockRight;

    Rectangle ISplitterDragSource.DragLimitBounds
    {
        get
        {
            var rectLimit = DockPanel.DockArea;
            var location = (ModifierKeys & Keys.Shift) == 0 ? Location : DockPanel.DockArea.Location;

            if (((ISplitterDragSource)this).IsVertical)
            {
                rectLimit.X += MeasurePane.MinSize;
                rectLimit.Width -= 2 * MeasurePane.MinSize;
                rectLimit.Y = location.Y;
                if ((ModifierKeys & Keys.Shift) == 0)
                {
                    rectLimit.Height = Height;
                }
            }
            else
            {
                rectLimit.Y += MeasurePane.MinSize;
                rectLimit.Height -= 2 * MeasurePane.MinSize;
                rectLimit.X = location.X;
                if ((ModifierKeys & Keys.Shift) == 0)
                {
                    rectLimit.Width = Width;
                }
            }

            return DockPanel.RectangleToScreen (rectLimit);
        }
    }

    void ISplitterDragSource.MoveSplitter (int offset)
    {
        if ((ModifierKeys & Keys.Shift) != 0)
        {
            SendToBack();
        }

        var rectDockArea = DockPanel.DockArea;
        if (DockState == DockState.DockLeft && rectDockArea.Width > 0)
        {
            if (DockPanel.DockLeftPortion > 1)
            {
                DockPanel.DockLeftPortion = Width + offset;
            }
            else
            {
                DockPanel.DockLeftPortion += offset / (double)rectDockArea.Width;
            }
        }
        else if (DockState == DockState.DockRight && rectDockArea.Width > 0)
        {
            if (DockPanel.DockRightPortion > 1)
            {
                DockPanel.DockRightPortion = Width - offset;
            }
            else
            {
                DockPanel.DockRightPortion -= offset / (double)rectDockArea.Width;
            }
        }
        else if (DockState == DockState.DockBottom && rectDockArea.Height > 0)
        {
            if (DockPanel.DockBottomPortion > 1)
            {
                DockPanel.DockBottomPortion = Height - offset;
            }
            else
            {
                DockPanel.DockBottomPortion -= offset / (double)rectDockArea.Height;
            }
        }
        else if (DockState == DockState.DockTop && rectDockArea.Height > 0)
        {
            if (DockPanel.DockTopPortion > 1)
            {
                DockPanel.DockTopPortion = Height + offset;
            }
            else
            {
                DockPanel.DockTopPortion += offset / (double)rectDockArea.Height;
            }
        }
    }

    #region IDragSource Members

    Control IDragSource.DragControl => this;

    #endregion

    #endregion
}
