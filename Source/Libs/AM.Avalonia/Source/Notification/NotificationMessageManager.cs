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

/* NotificationMessageManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia;

#endregion

#nullable enable

namespace AM.Avalonia.Notification;

/// <summary>
/// The notification message manager.
/// </summary>
/// <seealso cref="INotificationMessageManager" />
public class NotificationMessageManager : AvaloniaObject, INotificationMessageManager
{
    private readonly List<INotificationMessage> queuedMessages = new ();

    /// <summary>
    /// Occurs when new notification message is queued.
    /// </summary>
    public event NotificationMessageManagerEventHandler? OnMessageQueued;

    /// <summary>
    /// Occurs when notification message is dismissed.
    /// </summary>
    public event NotificationMessageManagerEventHandler? OnMessageDismissed;

    /// <summary>
    /// Gets or sets the factory.
    /// </summary>
    /// <value>
    /// The factory.
    /// </value>
    public INotificationMessageFactory Factory { get; set; } = new NotificationMessageFactory();


    /// <summary>
    /// Queues the specified message.
    /// This will ignore the <c>null</c> message or already queued notification message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void Queue
        (
            INotificationMessage? message
        )
    {
        if (message == null || queuedMessages.Contains (message))
        {
            return;
        }

        queuedMessages.Add (message);

        TriggerMessageQueued (message);
    }

    /// <summary>
    /// Triggers the message queued event.
    /// </summary>
    /// <param name="message">The message.</param>
    private void TriggerMessageQueued (INotificationMessage message)
    {
        OnMessageQueued?.Invoke (this, new NotificationMessageManagerEventArgs (message));
    }

    /// <summary>
    /// Dismisses the specified message.
    /// This will ignore the <c>null</c> or not queued notification message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void Dismiss
        (
            INotificationMessage? message
        )
    {
        if (message == null || !queuedMessages.Contains (message))
        {
            return;
        }

        queuedMessages.Remove (message);

        if (message is INotificationAnimation animatableMessage)
        {
            // var animation = animatableMessage.AnimationOut;
            if (
                animatableMessage.Animates &&
                animatableMessage.AnimatableElement != null)
            {
                animatableMessage.AnimatableElement.DismissAnimation = true;
                Task.Delay (500).ContinueWith (
                    context => { this.TriggerMessageDismissed (message); },
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                TriggerMessageDismissed (message);
            }
        }
        else
        {
            TriggerMessageDismissed (message);
        }
    }

    /// <summary>
    /// Triggers the message dismissed event.
    /// </summary>
    /// <param name="message">The message.</param>
    private void TriggerMessageDismissed (INotificationMessage message)
    {
        OnMessageDismissed?.Invoke (this, new NotificationMessageManagerEventArgs (message));
    }
}
