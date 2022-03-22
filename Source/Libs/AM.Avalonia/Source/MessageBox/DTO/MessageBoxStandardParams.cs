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

/* MessageBoxStandardParams.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia.Enums;

#endregion

#nullable enable

namespace AM.Avalonia.DTO;

/// <summary>
///
/// </summary>
public class MessageBoxStandardParams
    : AbstractMessageBoxParams
{
    /// <summary>
    /// Icon of window
    /// </summary>
    public Icon Icon { get; set; } = Icon.None;

    /// <summary>
    /// Default buttons
    /// </summary>
    public ButtonEnum ButtonDefinitions { get; set; } = ButtonEnum.Ok;

    /// <summary>
    ///
    /// </summary>
    public ClickEnum EnterDefaultButton { get; set; } = ClickEnum.Default;

    /// <summary>
    ///
    /// </summary>
    public ClickEnum EscDefaultButton { get; set; } = ClickEnum.Default;
}
