// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Enums.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
[Flags]
[Serializable]
[Editor (typeof (DockAreasEditor), typeof (System.Drawing.Design.UITypeEditor))]
public enum DockAreas
{
    /// <summary>
    ///
    /// </summary>
    Float = 1,

    /// <summary>
    ///
    /// </summary>
    DockLeft = 2,

    /// <summary>
    ///
    /// </summary>
    DockRight = 4,

    /// <summary>
    ///
    /// </summary>
    DockTop = 8,

    /// <summary>
    ///
    /// </summary>
    DockBottom = 16,

    /// <summary>
    ///
    /// </summary>
    Document = 32
}

/// <summary>
///
/// </summary>
public enum DockState
{
    /// <summary>
    ///
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///
    /// </summary>
    Float = 1,

    /// <summary>
    ///
    /// </summary>
    DockTopAutoHide = 2,

    /// <summary>
    ///
    /// </summary>
    DockLeftAutoHide = 3,

    /// <summary>
    ///
    /// </summary>
    DockBottomAutoHide = 4,

    /// <summary>
    ///
    /// </summary>
    DockRightAutoHide = 5,

    /// <summary>
    ///
    /// </summary>
    Document = 6,

    /// <summary>
    ///
    /// </summary>
    DockTop = 7,

    /// <summary>
    ///
    /// </summary>
    DockLeft = 8,

    /// <summary>
    ///
    /// </summary>
    DockBottom = 9,

    /// <summary>
    ///
    /// </summary>
    DockRight = 10,

    /// <summary>
    ///
    /// </summary>
    Hidden = 11
}

/// <summary>
///
/// </summary>
public enum DockAlignment
{
    /// <summary>
    ///
    /// </summary>
    Left,

    /// <summary>
    ///
    /// </summary>
    Right,

    /// <summary>
    ///
    /// </summary>
    Top,

    /// <summary>
    ///
    /// </summary>
    Bottom
}

/// <summary>
///
/// </summary>
public enum DocumentStyle
{
    /// <summary>
    ///
    /// </summary>
    DockingMdi,

    /// <summary>
    ///
    /// </summary>
    DockingWindow,

    /// <summary>
    ///
    /// </summary>
    DockingSdi,

    /// <summary>
    ///
    /// </summary>
    SystemMdi,
}

/// <summary>
/// The location to draw the DockPaneStrip for Document style windows.
/// </summary>
public enum DocumentTabStripLocation
{
    /// <summary>
    ///
    /// </summary>
    Top,

    /// <summary>
    ///
    /// </summary>
    Bottom
}
