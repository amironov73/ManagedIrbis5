// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* ButtonResult.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Avalonia.Enums;

/// <summary>
/// Result on click in message box button
/// </summary>
[Flags]
public enum ButtonResult
{
    /// <summary>
    /// OK.
    /// </summary>
    Ok,

    /// <summary>
    /// Yes.
    /// </summary>
    Yes,

    /// <summary>
    /// No.
    /// </summary>
    No,

    /// <summary>
    /// Abort.
    /// </summary>
    Abort,

    /// <summary>
    /// Cancel.
    /// </summary>
    Cancel,

    /// <summary>
    /// None.
    /// </summary>
    None
}
