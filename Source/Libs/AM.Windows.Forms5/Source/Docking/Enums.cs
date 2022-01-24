// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

[Flags]
[Serializable]
[Editor (typeof (DockAreasEditor), typeof (System.Drawing.Design.UITypeEditor))]
public enum DockAreas
{
    Float = 1,
    DockLeft = 2,
    DockRight = 4,
    DockTop = 8,
    DockBottom = 16,
    Document = 32
}

public enum DockState
{
    Unknown = 0,
    Float = 1,
    DockTopAutoHide = 2,
    DockLeftAutoHide = 3,
    DockBottomAutoHide = 4,
    DockRightAutoHide = 5,
    Document = 6,
    DockTop = 7,
    DockLeft = 8,
    DockBottom = 9,
    DockRight = 10,
    Hidden = 11
}

public enum DockAlignment
{
    Left,
    Right,
    Top,
    Bottom
}

public enum DocumentStyle
{
    DockingMdi,
    DockingWindow,
    DockingSdi,
    SystemMdi,
}

/// <summary>
/// The location to draw the DockPaneStrip for Document style windows.
/// </summary>
public enum DocumentTabStripLocation
{
    Top,
    Bottom
}