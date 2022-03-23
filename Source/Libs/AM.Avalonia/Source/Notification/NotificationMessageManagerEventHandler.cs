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

/* NotificationMessageManagerEventHandler.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Avalonia.Notification;

/// <summary>
/// The notification message manager event handler.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="args">The <see cref="NotificationMessageManagerEventArgs"/> instance containing the event data.</param>
public delegate void NotificationMessageManagerEventHandler
    (
        object sender,
        NotificationMessageManagerEventArgs args
    );
