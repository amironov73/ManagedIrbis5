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

/* NotificationMessageFactory.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia.Notification.Controls;

#endregion

#nullable enable

namespace AM.Avalonia.Notification;

/// <summary>
/// The notification message factory.
/// </summary>
/// <seealso cref="INotificationMessageFactory" />
public class NotificationMessageFactory
    : INotificationMessageFactory
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <returns>
    /// Returns new instance of notification message.
    /// </returns>
    public INotificationMessage GetMessage()
    {
        return new NotificationMessage();
    }

    /// <summary>
    /// Gets the button.
    /// </summary>
    /// <returns>
    /// Returns new instance of notification message button.
    /// </returns>
    public INotificationMessageButton GetButton()
    {
        return new NotificationMessageButton();
    }
}
