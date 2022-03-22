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

/* ButtonEnum.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Avalonia.Enums;

/// <summary>
/// Buttons in message box window
/// </summary>
public enum ButtonEnum
{
    /// <summary>
    /// OK.
    /// </summary>
    Ok,

    /// <summary>
    /// Yes or No.
    /// </summary>
    YesNo,

    /// <summary>
    /// OK or Cancel.
    /// </summary>
    OkCancel,

    /// <summary>
    /// OK or Abort.
    /// </summary>
    OkAbort,

    /// <summary>
    /// Yes, No or Cancel.
    /// </summary>
    YesNoCancel,

    /// <summary>
    /// Yes, No or Abort.
    /// </summary>
    YesNoAbort
}
