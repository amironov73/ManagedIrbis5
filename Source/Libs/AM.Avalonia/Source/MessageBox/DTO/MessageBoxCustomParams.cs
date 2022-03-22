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

/* MessageBoxCustomParams.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Avalonia.Enums;
using AM.Avalonia.Models;

#endregion

#nullable enable

namespace AM.Avalonia.DTO;

/// <summary>
///
/// </summary>
public class MessageBoxCustomParams
    : AbstractMessageBoxParams
{
    /// <summary>
    /// Icon of window
    /// </summary>
    public Icon Icon { get; set; } = Icon.None;

    /// <summary>
    /// Buttons of message box
    /// </summary>
    public IEnumerable<ButtonDefinition> ButtonDefinitions { get; set; }
}
