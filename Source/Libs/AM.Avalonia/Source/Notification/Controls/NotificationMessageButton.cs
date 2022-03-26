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

/* NotificationMessageButton.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia.Controls;

#endregion

#nullable enable

namespace AM.Avalonia.Notification.Controls;

/// <summary>
/// The notification message button.
/// </summary>
/// <seealso cref="Button" />
/// <seealso cref="INotificationMessageButton" />
public class NotificationMessageButton : Button, INotificationMessageButton
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessageButton"/> class.
    /// </summary>
    public NotificationMessageButton()
        : this (null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessageButton"/> class.
    /// </summary>
    /// <param name="content">The content.</param>
    public NotificationMessageButton
        (
            object? content
        )
    {
        Content = content;
    }

    /// <summary>
    /// Called when a <see cref="T:System.Windows.Controls.Button" /> is clicked.
    /// </summary>
    protected override void OnClick()
    {
        base.OnClick();
        Callback?.Invoke (this);
    }

    /// <summary>
    /// Initializes the <see cref="NotificationMessageButton"/> class.
    /// </summary>
    static NotificationMessageButton()
    {
        // TODO
        //DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationMessageButton), new FrameworkPropertyMetadata(typeof(NotificationMessageButton)));
    }


    /// <summary>
    /// Gets or sets the callback.
    /// </summary>
    /// <value>
    /// The callback.
    /// </value>
    public Action<INotificationMessageButton>? Callback { get; set; }
}
