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

/* INotificagionMessageFactory.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Avalonia.Notification;

/// <summary>
/// The notification message factory.
/// </summary>
public interface INotificationMessageFactory
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <returns>Returns new instance of notification message.</returns>
    INotificationMessage GetMessage();

    /// <summary>
    /// Gets the button.
    /// </summary>
    /// <returns>Returns new instance of notification message button.</returns>
    INotificationMessageButton GetButton();
}
