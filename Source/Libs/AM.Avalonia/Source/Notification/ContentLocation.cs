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

/* ContentLocation.cs -- места для расположение дополнительного контента
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Avalonia.Notification;

/// <summary>
/// Места для расположения дополнительного контента.
/// </summary>
public enum ContentLocation
{
    /// <summary>
    /// Наверху.
    /// </summary>
    Top,

    /// <summary>
    /// Внизу.
    /// </summary>
    Bottom,

    /// <summary>
    /// Слева.
    /// </summary>
    Left,

    /// <summary>
    /// Справа.
    /// </summary>
    Right,

    /// <summary>
    ///
    /// </summary>
    Main,

    /// <summary>
    ///
    /// </summary>
    AboveBadge
}
