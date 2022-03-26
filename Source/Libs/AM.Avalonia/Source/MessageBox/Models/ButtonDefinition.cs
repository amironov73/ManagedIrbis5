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

/* ButtonDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Avalonia.Models;

/// <summary>
///
/// </summary>
public class ButtonDefinition
{
    /// <summary>
    /// Text in button
    /// </summary>
    public string Name { get; set; } = "OK";

    /// <summary>
    /// When true and if ENTER key is pressed, the button will be called
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// When true and if ESC key is pressed, the button will be called
    /// </summary>
    public bool IsCancel { get; set; }
}
