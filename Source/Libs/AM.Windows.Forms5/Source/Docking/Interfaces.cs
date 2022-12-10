// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

/* Interfaces.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public interface IDockContent
    : IContextMenuStripHost
{
    /// <summary>
    ///
    /// </summary>
    DockContentHandler DockHandler { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="eventArgs"></param>
    void OnActivated (EventArgs eventArgs);

    /// <summary>
    ///
    /// </summary>
    /// <param name="eventArgs"></param>
    void OnDeactivate (EventArgs eventArgs);
}

/// <summary>
///
/// </summary>
public interface IContextMenuStripHost
{
    /// <summary>
    ///
    /// </summary>
    void ApplyTheme();
}

/// <summary>
///
/// </summary>
public interface INestedPanesContainer
{
    /// <summary>
    ///
    /// </summary>
    DockState DockState { get; }

    /// <summary>
    ///
    /// </summary>
    Rectangle DisplayingRectangle { get; }

    /// <summary>
    ///
    /// </summary>
    NestedPaneCollection? NestedPanes { get; }

    /// <summary>
    ///
    /// </summary>
    VisibleNestedPaneCollection VisibleNestedPanes { get; }

    /// <summary>
    ///
    /// </summary>
    bool IsFloat { get; }
}

/// <summary>
///
/// </summary>
public interface IDragSource
{
    /// <summary>
    ///
    /// </summary>
    Control DragControl { get; }
}

/// <summary>
///
/// </summary>
public interface IDockDragSource
    : IDragSource
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="ptMouse"></param>
    /// <returns></returns>
    Rectangle BeginDrag (Point ptMouse);

    /// <summary>
    ///
    /// </summary>
    void EndDrag();

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockState"></param>
    /// <returns></returns>
    bool IsDockStateValid (DockState dockState);

    /// <summary>
    ///
    /// </summary>
    /// <param name="pane"></param>
    /// <returns></returns>
    bool CanDockTo (DockPane pane);

    /// <summary>
    ///
    /// </summary>
    /// <param name="floatWindowBounds"></param>
    void FloatAt (Rectangle floatWindowBounds);

    /// <summary>
    ///
    /// </summary>
    /// <param name="pane"></param>
    /// <param name="dockStyle"></param>
    /// <param name="contentIndex"></param>
    void DockTo (DockPane pane, DockStyle dockStyle, int contentIndex);

    /// <summary>
    ///
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="dockStyle"></param>
    void DockTo (DockPanel panel, DockStyle dockStyle);
}

/// <summary>
///
/// </summary>
public interface ISplitterDragSource
    : IDragSource
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="rectSplitter"></param>
    void BeginDrag (Rectangle rectSplitter);

    /// <summary>
    ///
    /// </summary>
    void EndDrag();

    /// <summary>
    ///
    /// </summary>
    bool IsVertical { get; }

    /// <summary>
    ///
    /// </summary>
    Rectangle DragLimitBounds { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="offset"></param>
    void MoveSplitter (int offset);
}

/// <summary>
///
/// </summary>
public interface ISplitterHost
    : ISplitterDragSource
{
    /// <summary>
    ///
    /// </summary>
    DockPanel DockPanel { get; }

    /// <summary>
    ///
    /// </summary>
    DockState DockState { get; }

    /// <summary>
    ///
    /// </summary>
    bool IsDockWindow { get; }
}
