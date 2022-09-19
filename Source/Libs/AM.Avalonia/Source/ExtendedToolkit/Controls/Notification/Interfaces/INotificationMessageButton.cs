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

#endregion

#nullable enable

//ported from https://github.com/Enterwell/Wpf.Notifications

namespace Avalonia.ExtendedToolkit.Controls;

/// <summary>
///
/// </summary>
public interface INotificationMessageButton
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
    /// </value>
    bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    /// <value>
    /// The content.
    /// </value>
    object Content { get; set; }

    /// <summary>
    /// Gets or sets the callback.
    /// </summary>
    /// <value>
    /// The callback.
    /// </value>
    Action<INotificationMessageButton>? Callback { get; set; }
}
