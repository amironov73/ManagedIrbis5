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

/* MessageBoxInputParams.cs --
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
public class MessageBoxInputParams
    : MessageBoxCustomParams
{
    /// <summary>
    /// Hide input letters
    /// </summary>
    public bool IsPassword { get; set; } = false;

    public PasswordRevealModes PasswordRevealMode { get; set; } = PasswordRevealModes.Hold;

    /// <summary>
    /// Watermark text
    /// </summary>
    public string WatermarkText { get; set; } = null;

    /// <summary>
    /// Multiline in input
    /// </summary>
    public bool Multiline { get; set; }

    /// <summary>
    /// Default result of input
    /// </summary>
    public string InputDefaultValue { get; set; }
}
