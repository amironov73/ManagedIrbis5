// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DockHelper.cs --
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
public static class DockHelper
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public static bool IsDockStateAutoHide
        (
            DockState dockState
        )
    {
        return dockState is DockState.DockLeftAutoHide
            or DockState.DockRightAutoHide
            or DockState.DockTopAutoHide
            or DockState.DockBottomAutoHide;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool IsDockStateValid
        (
            DockState dockState,
            DockAreas dockableAreas
        )
    {
        if ((dockableAreas & DockAreas.Float) == 0 &&
            dockState == DockState.Float)
        {
            return false;
        }

        if ((dockableAreas & DockAreas.Document) == 0 &&
            dockState == DockState.Document)
        {
            return false;
        }

        if ((dockableAreas & DockAreas.DockLeft) == 0 &&
            dockState is DockState.DockLeft or DockState.DockLeftAutoHide)
        {
            return false;
        }

        if ((dockableAreas & DockAreas.DockRight) == 0 &&
            dockState is DockState.DockRight or DockState.DockRightAutoHide)
        {
            return false;
        }

        if ((dockableAreas & DockAreas.DockTop) == 0 &&
            dockState is DockState.DockTop or DockState.DockTopAutoHide)
        {
            return false;
        }

        if ((dockableAreas & DockAreas.DockBottom) == 0 &&
            dockState is DockState.DockBottom or DockState.DockBottomAutoHide)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool IsDockWindowState
        (
            DockState state
        )
    {
        return state is DockState.DockTop
            or DockState.DockBottom
            or DockState.DockLeft
            or DockState.DockRight
            or DockState.Document;
    }

    /// <summary>
    ///
    /// </summary>
    public static DockState ToggleAutoHideState
        (
            DockState state
        )
    {
        return state switch
        {
            DockState.DockLeft => DockState.DockLeftAutoHide,
            DockState.DockRight => DockState.DockRightAutoHide,
            DockState.DockTop => DockState.DockTopAutoHide,
            DockState.DockBottom => DockState.DockBottomAutoHide,
            DockState.DockLeftAutoHide => DockState.DockLeft,
            DockState.DockRightAutoHide => DockState.DockRight,
            DockState.DockTopAutoHide => DockState.DockTop,
            DockState.DockBottomAutoHide => DockState.DockBottom,
            _ => state
        };
    }

    /// <summary>
    ///
    /// </summary>
    public static DockPane? PaneAtPoint
        (
            Point pt,
            DockPanel dockPanel
        )
    {
        if (!Win32Helper.IsRunningOnMono)
        {
            for (var control = Win32Helper.ControlAtPoint (pt); control != null; control = control.Parent)
            {
                if (control is IDockContent content
                    && content.DockHandler.DockPanel == dockPanel)
                {
                    return content.DockHandler.Pane;
                }

                if (control is DockPane pane
                    && pane.DockPanel == dockPanel)
                {
                    return pane;
                }
            }
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    public static FloatWindow? FloatWindowAtPoint
        (
            Point pt,
            DockPanel dockPanel
        )
    {
        if (!Win32Helper.IsRunningOnMono)
        {
            for (var control = Win32Helper.ControlAtPoint (pt); control != null; control = control.Parent)
            {
                if (control is FloatWindow floatWindow
                    && floatWindow.DockPanel == dockPanel)
                {
                    return floatWindow;
                }
            }
        }

        return null;
    }

    #endregion
}
