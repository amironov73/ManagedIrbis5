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

/* INotificationAnimation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Controls.Primitives;

#endregion

#nullable enable

namespace AM.Avalonia.Notification;

/// <summary>
/// The animation properties for a notification message or some
/// other item.
/// </summary>
public interface INotificationAnimation
{
    /// <summary>
    /// Set DismissAnimation
    /// </summary>
    bool DismissAnimation { get; set; }

    /// <summary>
    /// Set StartAnimation
    /// </summary>
    bool StartAnimation { get; set; }

    /// <summary>
    /// Gets or sets whether the item animates in and out.
    /// </summary>
    bool Animates { get; set; }

    /// <summary>
    /// Gets the animatable UIElement.
    /// Typically this is the whole Control object so that the entire
    /// item can be animated.
    /// </summary>
    INotificationAnimation AnimatableElement { get; }
}
