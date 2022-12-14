// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* NestedDockingStatus.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public sealed class NestedDockingStatus
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="pane"></param>
    internal NestedDockingStatus (DockPane pane)
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
    public NestedPaneCollection? NestedPanes { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public DockPane? PreviousPane { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public DockAlignment Alignment { get; private set; } = DockAlignment.Left;

    /// <summary>
    ///
    /// </summary>
    public double Proportion { get; private set; } = 0.5;

    /// <summary>
    ///
    /// </summary>
    public bool IsDisplaying { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public DockPane? DisplayingPreviousPane { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public DockAlignment DisplayingAlignment { get; private set; } = DockAlignment.Left;

    /// <summary>
    ///
    /// </summary>
    public double DisplayingProportion { get; private set; } = 0.5;

    /// <summary>
    ///
    /// </summary>
    public Rectangle LogicalBounds { get; private set; } = Rectangle.Empty;

    /// <summary>
    ///
    /// </summary>
    public Rectangle PaneBounds { get; private set; } = Rectangle.Empty;

    /// <summary>
    ///
    /// </summary>
    public Rectangle SplitterBounds { get; private set; } = Rectangle.Empty;

    internal void SetStatus
        (
            NestedPaneCollection? nestedPanes,
            DockPane? previousPane,
            DockAlignment alignment,
            double proportion
        )
    {
        NestedPanes = nestedPanes;
        PreviousPane = previousPane;
        Alignment = alignment;
        Proportion = proportion;
    }

    internal void SetDisplayingStatus (bool isDisplaying, DockPane? displayingPreviousPane,
        DockAlignment displayingAlignment, double displayingProportion)
    {
        IsDisplaying = isDisplaying;
        DisplayingPreviousPane = displayingPreviousPane;
        DisplayingAlignment = displayingAlignment;
        DisplayingProportion = displayingProportion;
    }

    internal void SetDisplayingBounds (Rectangle logicalBounds, Rectangle paneBounds, Rectangle splitterBounds)
    {
        LogicalBounds = logicalBounds;
        PaneBounds = paneBounds;
        SplitterBounds = splitterBounds;
    }
}
