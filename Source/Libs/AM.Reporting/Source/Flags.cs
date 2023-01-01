// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Flags.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Reporting;

/// <summary>
/// Specifies a set of actions that can be performed on the object in the design mode.
/// </summary>
[Flags]
public enum Flags
{
    /// <summary>
    /// Specifies no actions.
    /// </summary>
    None = 0,

    /// <summary>
    /// Allows moving the object.
    /// </summary>
    CanMove = 1,

    /// <summary>
    /// Allows resizing the object.
    /// </summary>
    CanResize = 2,

    /// <summary>
    /// Allows deleting the object.
    /// </summary>
    CanDelete = 4,

    /// <summary>
    /// Allows editing the object.
    /// </summary>
    CanEdit = 8,

    /// <summary>
    /// Allows changing the Z-order of an object.
    /// </summary>
    CanChangeOrder = 16,

    /// <summary>
    /// Allows moving the object to another parent.
    /// </summary>
    CanChangeParent = 32,

    /// <summary>
    /// Allows copying the object to the clipboard.
    /// </summary>
    CanCopy = 64,

    /// <summary>
    /// Allows drawing the object.
    /// </summary>
    CanDraw = 128,

    /// <summary>
    /// Allows grouping the object.
    /// </summary>
    CanGroup = 256,

    /// <summary>
    /// Allows write children in the preview mode by itself.
    /// </summary>
    CanWriteChildren = 512,

    /// <summary>
    /// Allows write object's bounds into the report stream.
    /// </summary>
    CanWriteBounds = 1024,

    /// <summary>
    /// Allows the "smart tag" functionality.
    /// </summary>
    HasSmartTag = 2048,

    /// <summary>
    /// Specifies that the object's name is global (this is true for all report objects
    /// such as Text, Picture and so on).
    /// </summary>
    HasGlobalName = 4096,

    /// <summary>
    /// Specifies that the object can display children in the designer's Report Tree window.
    /// </summary>
    CanShowChildrenInReportTree = 8192,

    /// <summary>
    /// Specifies that the object supports mouse wheel in the preview window.
    /// </summary>
    InterceptsPreviewMouseEvents = 16384
}
