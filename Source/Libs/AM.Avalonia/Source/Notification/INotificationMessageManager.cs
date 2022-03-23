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

/* INotificationMessageManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Avalonia.Notification;

/// <summary>
/// The notification message manager.
/// </summary>
public interface INotificationMessageManager
{
    /// <summary>
    /// Occurs when new notification message is queued.
    /// </summary>
    event NotificationMessageManagerEventHandler OnMessageQueued;

    /// <summary>
    /// Occurs when notification message is dismissed.
    /// </summary>
    event NotificationMessageManagerEventHandler OnMessageDismissed;

    /// <summary>
    /// Gets or sets the factory.
    /// </summary>
    /// <value>
    /// The factory.
    /// </value>
    INotificationMessageFactory Factory { get; set; }

    /// <summary>
    /// Queues the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Queue (INotificationMessage message);

    /// <summary>
    /// Dismisses the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    void Dismiss (INotificationMessage message);
}
