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

/* HyperlinkContent.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Avalonia.Models;

/// <summary>
///
/// </summary>
public class HyperlinkContent
{
    /// <summary>
    /// Url what would be displayed if Alias is not set (as hyperlink) ,or it would be used as hyperlink for Alias.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Alias what would be clickable if set,else raw url would be displayed (also clickable).
    /// </summary>
    public string? Alias { get; set; }
}
