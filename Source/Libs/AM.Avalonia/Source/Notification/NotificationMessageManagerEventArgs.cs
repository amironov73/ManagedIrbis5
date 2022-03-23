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

/* NotificationMessageManagerEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Avalonia.Notification;

/// <summary>
/// The notification message manager event arguments.
/// </summary>
/// <seealso cref="EventArgs" />
public class NotificationMessageManagerEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>
    /// The message.
    /// </value>
    public INotificationMessage Message { get; set; }


    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessageManagerEventArgs"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public NotificationMessageManagerEventArgs (INotificationMessage message)
    {
        Message = message;
    }
}
