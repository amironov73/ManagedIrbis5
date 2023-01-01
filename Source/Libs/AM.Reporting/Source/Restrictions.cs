// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Restrictions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Reporting;

/// <summary>
/// Specifies a set of actions that cannot be performed on the object in the design mode.
/// </summary>
[Flags]
public enum Restrictions
{
    /// <summary>
    /// Specifies no restrictions.
    /// </summary>
    None = 0,

    /// <summary>
    /// Restricts moving the object.
    /// </summary>
    DontMove = 1,

    /// <summary>
    /// Restricts resizing the object.
    /// </summary>
    DontResize = 2,

    /// <summary>
    /// Restricts modifying the object's properties.
    /// </summary>
    DontModify = 4,

    /// <summary>
    /// Restricts editing the object.
    /// </summary>
    DontEdit = 8,

    /// <summary>
    /// Restricts deleting the object.
    /// </summary>
    DontDelete = 16,

    /// <summary>
    /// Hides all properties of the object.
    /// </summary>
    HideAllProperties = 32
}
