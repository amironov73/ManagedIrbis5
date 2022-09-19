// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia.Controls;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/Enterwell/Wpf.Notifications

/// <summary>
/// Button control which implements the
/// <see cref="INotificationMessageButton"/> interface
/// </summary>
public class NotificationMessageButton
    : Button, INotificationMessageButton
{
    /// <summary>
    /// style key of this control
    /// </summary>
    public Type StyleKey => typeof (Button);

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessageButton"/> class.
    /// </summary>
    public NotificationMessageButton()
        : this (null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessageButton"/> class.
    /// </summary>
    /// <param name="content">The content.</param>
    public NotificationMessageButton (object? content)
    {
        Content = content;
    }

    #endregion

    /// <inheritdoc cref="Button.OnClick"/>
    protected override void OnClick()
    {
        base.OnClick();
        Callback?.Invoke (this);
    }

    /// <summary>
    /// Gets or sets the callback.
    /// </summary>
    /// <value>
    /// The callback.
    /// </value>
    public Action<INotificationMessageButton>? Callback { get; set; }
}
