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

/* PasswordRevealModes.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Avalonia.Enums;

/// <summary>
///
/// </summary>
public enum PasswordRevealModes
    : byte
{
    /// <summary>
    /// Don't show the reveal button
    /// </summary>
    None,

    /// <summary>
    /// Left click to toggle the reveal password
    /// </summary>
    Toggle,

    /// <summary>
    /// Left or right click and hold to temporary reveal the password
    /// </summary>
    Hold,

    /// <summary>
    /// Left click to toggle the reveal password | Right click and hold will temporary reveal password
    /// </summary>
    Both,
}
