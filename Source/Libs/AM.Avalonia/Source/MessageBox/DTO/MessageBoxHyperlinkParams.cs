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

/* MessageBoxHyperlinkParams.cs --
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
public class MessageBoxHyperlinkParams
    : AbstractMessageBoxParams
{
    /// <summary>
    /// Icon of window
    /// </summary>
    public Icon Icon { get; set; } = Icon.None;

    /// <summary>
    /// Buttons
    /// </summary>
    public ButtonEnum ButtonDefinitions { get; set; } = ButtonEnum.Ok;

    public IEnumerable<HyperlinkContent> HyperlinkContentProvider { get; set; } = new List<HyperlinkContent>();
}
